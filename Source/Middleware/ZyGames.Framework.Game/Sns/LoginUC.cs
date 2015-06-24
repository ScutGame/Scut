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
using ZyGames.Framework.Game.Configuration;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 9游0036
    /// </summary>
    public class LoginUC : AbstractLogin
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
		///
        public class UcConfig
        {
			/// <summary>
			/// Gets or sets the cp identifier.
			/// </summary>
			/// <value>The cp identifier.</value>
            public string CpId { get; set; }
			/// <summary>
			/// Gets or sets the game identifier.
			/// </summary>
			/// <value>The game identifier.</value>
            public string GameId { get; set; }
			/// <summary>
			/// Gets or sets the server identifier.
			/// </summary>
			/// <value>The server identifier.</value>
            public string ServerId { get; set; }
			/// <summary>
			/// Gets or sets the channel identifier.
			/// </summary>
			/// <value>The channel identifier.</value>
            public string ChannelId { get; set; }
			/// <summary>
			/// Gets or sets the API key.
			/// </summary>
			/// <value>The API key.</value>
            public string ApiKey { get; set; }
			/// <summary>
			/// Gets or sets the post URL.
			/// </summary>
			/// <value>The post URL.</value>
            public string PostUrl { get; set; }
			/// <summary>
			/// Gets or sets the service.
			/// </summary>
			/// <value>The service.</value>
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
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.LoginUC"/> class.
		/// </summary>
		/// <param name="retailID">Retail I.</param>
		/// <param name="sid">Sid.</param>
		/// <param name="mobileType">Mobile type.</param>
        public LoginUC(string retailID, string sid, string mobileType)
        {
            this._retailID = retailID;
            this._sessionID = sid;
            this._mobileType = mobileType.ToEnum<MobileType>();
        }
		/// <summary>
		/// 注册通行证
		/// </summary>
		/// <returns></returns>
        public override string GetRegPassport()
        {
            return this.PassportID;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override bool CheckLogin()
        {
            if (string.IsNullOrEmpty(_sessionID))
            {
                return false;
            }
            string key = _mobileType == MobileType.ptAndroid ? "android" : "ios";
            GameChannel gameChannel = ZyGameBaseConfigManager.GameSetting.GetChannelSetting(ChannelType.channelUC);
            if (gameChannel == null)
            {
                TraceLog.ReleaseWrite("The sdkChannel UC section is null.");
                return false;
            }
            GameSdkSetting setting = gameChannel.GetSetting(key);
            if (setting == null)
            {
                TraceLog.ReleaseWrite("The sdkChannel UC section channelUC:{0} is null.", key);
                return false;
            }

            string id = ((DateTime.Now - Convert.ToDateTime("1970-1-1")).TotalMilliseconds).ToString().Substring(0, 13);
		    string signSrc = setting.AppId + "sid=" + _sessionID + setting.AppKey;
            string sign = AMD5(signSrc);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"service\":\"").Append(gameChannel.Service).Append("\",");
            sb.Append("\"id\":\"").Append(id).Append("\",");
            sb.Append("\"game\":{");
            sb.Append("\"cpId\":\"").Append(setting.AppId).Append("\",");
            sb.Append("\"gameId\":\"").Append(setting.GameId).Append("\",");
            sb.Append("\"channelId\":\"").Append(gameChannel.ChannelId).Append("\",");
            sb.Append("\"serverId\":\"").Append(setting.ServerId).Append("\"},");
            sb.Append("\"data\":{");
            sb.Append("\"sid\":\"").Append(_sessionID).Append("\"},");
            sb.Append("\"encrypt\":\"").Append(encrypt).Append("\",");
            sb.Append("\"sign\":\"").Append(sign).Append("\"}");

            string result = httpPost(gameChannel.Url, sb.ToString(), Encoding.UTF8);
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
                TraceLog.ReleaseWrite("Danlesdk login fail:{0},request url:{1},param:{2},signsrc:{3}", result, gameChannel.Url, sb.ToString(), signSrc);
                return false;
            }

            _retailUser = ucinfo.data.ucid;

            string[] arr = SnsManager.LoginByRetail(_retailID, _retailUser);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            return true;
        }
        /// <summary>
        /// Https the post.
        /// </summary>
        /// <returns>The post.</returns>
        /// <param name="urlStr">URL string.</param>
        /// <param name="postData">Post data.</param>
        /// <param name="encoding">Encoding.</param>
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

	/// <summary>
	/// UC info.
	/// </summary>
    public class UCInfo
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string id { get; set; }
		/// <summary>
		/// Gets or sets the state.
		/// </summary>
		/// <value>The state.</value>
        public UCState state { get; set; }
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
        public UCdata data { get; set; }
    }
	/// <summary>
	/// UC state.
	/// </summary>
    public class UCState
    {
		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>The code.</value>
        public string code { get; set; }
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
        public string msg { get; set; }
    }
	/// <summary>
	/// U cdata.
	/// </summary>
    public class UCdata
    {
		/// <summary>
		/// Gets or sets the ucid.
		/// </summary>
		/// <value>The ucid.</value>
        public string ucid { get; set; }
		/// <summary>
		/// Gets or sets the name of the nick.
		/// </summary>
		/// <value>The name of the nick.</value>
        public string nickName { get; set; }
    }
}