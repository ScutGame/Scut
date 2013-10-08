/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Text;
using System.Net;
using System.IO;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Sns.Section;

namespace ZyGames.Framework.Game.Sns
{
    enum MobileType
    {
        //Win32
        ptWin32,
        //Ipod
        ptiPod,
        //Ipad
        ptiPad,
        //破解版iPhone和iPad
        ptiPhone,
        //非破解版iPhone
        ptiPhone_AppStore,
        //Android
        ptAndroid,
        // mac
        ptMac,
        // WP7
        ptWindowsPhone7,
        //未知
        ptUnknow
    };
    /// <summary>
    /// 9游0036
    /// 
    /// </summary>
    public class LoginUC : ILogin
    {
        public class UcConfig
        {
            public string CpId { get; set; }
            public string GameId { get; set; }
            public string ServerId { get; set; }
            public string ChannelId { get; set; }
            public string ApiKey { get; set; }
            public string PostUrl { get; set; }
            public string Service { get; set; }
        }

        //天界行 android 测试 695/64676/1007/2 fbd97e6b93d72997eea9e58fdbc01950 isDebug=1 sdk.test2.g.uc.cn 
        //正式 681/66943/1091/2 865e9567eddc2e883dc6c863afd5028a isDebug=0 sdk.g.uc.cn/ss 
        //ios(越狱) 测试 695/64677/1008/2 fbd97e6b93d72997eea9e58fdbc01950 isDebug=1 sdk.test2.g.uc.cn 
        //正式 681/75383/1090/2 865e9567eddc2e883dc6c863afd5028a isDebug=0 sdk.g.uc.cn/ss 
        private string _retailID = string.Empty;
        private string _retailUser = string.Empty;
        private string _sessionID = string.Empty;
        private MobileType _mobileType;
        private const string encrypt = "md5";

        public LoginUC(string retailID, string sid, string mobileType)
        {
            this._retailID = retailID;
            this._sessionID = sid;
            this._mobileType = mobileType.ToEnum<MobileType>();
        }

        #region ILogin 成员

        public string UserID
        {
            get;
            set;
        }

        public string PassportID
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string SessionID
        {
            get
            {
                return _sessionID;
            }
        }

        public string GetRegPassport()
        {
            return this.PassportID;
        }

        public bool CheckLogin()
        {
            if (string.IsNullOrEmpty(_sessionID))
            {
                return false;
            }
            var sec = SdkSectionFactory.SectionUC;
            if (sec == null)
            {
                TraceLog.ReleaseWrite("The sdkChannel UC section is null.");
                return false;
            }
            string key = _mobileType == MobileType.ptAndroid ? "android" : "ios";
            var channelElement = sec.Channels[key];
            if (channelElement == null)
            {
                TraceLog.ReleaseWrite("The sdkChannel UC section channelUC:{0} is null.", key);
                return false;
            }

            string id = ((DateTime.Now - Convert.ToDateTime("1970-1-1")).TotalMilliseconds).ToString().Substring(0, 13);
            string sign = AMD5(channelElement.CpId + "sid=" + _sessionID + channelElement.ApiKey);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"service\":\"").Append(sec.Service).Append("\",");
            sb.Append("\"id\":\"").Append(id).Append("\",");
            sb.Append("\"game\":{");
            sb.Append("\"cpId\":\"").Append(channelElement.CpId).Append("\",");
            sb.Append("\"gameId\":\"").Append(channelElement.GameId).Append("\",");
            sb.Append("\"channelId\":\"").Append(sec.ChannelId).Append("\",");
            sb.Append("\"serverId\":\"").Append(channelElement.ServerId).Append("\"},");
            sb.Append("\"data\":{");
            sb.Append("\"sid\":\"").Append(_sessionID).Append("\"},");
            sb.Append("\"encrypt\":\"").Append(encrypt).Append("\",");
            sb.Append("\"sign\":\"").Append(sign).Append("\"}");

            string result = httpPost(sec.Url, sb.ToString(), Encoding.UTF8);
            UCInfo ucinfo = null;
            try
            {
                ucinfo = JsonUtils.Deserialize<UCInfo>(result);
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
                return false;
            }
            if (ucinfo == null || ucinfo.state.code != "1")
            {
                TraceLog.ReleaseWrite("Danlesdk login fail:{0},request url:{1},param:{2}", result, sec.Url, sb.ToString());
                return false;
            }

            _retailUser = ucinfo.data.ucid;

            string[] arr = SnsManager.LoginByRetail(_retailID, _retailUser);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            return true;
        }

        #endregion


        private string AMD5(string str1)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str1, "MD5").ToLower();
        }


        public string httpPost(String urlStr, String postData, Encoding encoding)
        {
            HttpWebResponse resp = null;
            try
            {
                Uri uri = new Uri(urlStr);
                string postdata = postData;

                byte[] bytes = encoding.GetBytes(postdata);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.0; en-US; rv:1.8) Gecko/20051111 Firefox/1.5";
                req.Accept = "text/xml,application/xml,application/xhtml+xml,text/html";
                req.ProtocolVersion = System.Net.HttpVersion.Version11;
                req.KeepAlive = false;
                req.ContentType = "application/json";
                req.Method = "POST";
                req.ContentLength = bytes.Length;
                Stream os = req.GetRequestStream();
                os.Write(bytes, 0, bytes.Length); //Push it out there
                os.Close();
                resp = (HttpWebResponse)req.GetResponse();
                if (resp == null) return null;
                StreamReader sr = new StreamReader(resp.GetResponseStream(), encoding);

                string str = sr.ReadToEnd().Trim();

                sr.Close();

                return str;
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
            finally
            {
                if (null != resp)
                {
                    resp.Close();
                }
            }
            return "";

        }
    }


    public class UCInfo
    {
        public string id { get; set; }
        public UCState state { get; set; }
        public UCdata data { get; set; }
    }

    public class UCState
    {
        public string code { get; set; }
        public string msg { get; set; }
    }

    public class UCdata
    {
        public string ucid { get; set; }
        public string nickName { get; set; }
    }
}