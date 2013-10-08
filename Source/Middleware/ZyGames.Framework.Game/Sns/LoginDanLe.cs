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
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Sns.Section;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 当乐0037
    /// </summary>
    public class LoginDanLe : ILogin
    {
        private string _retailID;
        private string _mid;
        private string _token;
        private string username = string.Empty;

        public LoginDanLe(string retailID, string retailUser, string password, string passportId)
        {
            this._retailID = retailID;
            this.Password = new DESAlgorithmNew().DecodePwd(password, GameEnvironment.ClientDesDeKey);
            this.username = HttpUtility.UrlEncode(retailUser, Encoding.UTF8).ToUpper();
            _mid = passportId.Equals("0") ? string.Empty : passportId;
        }

        public LoginDanLe(string retailID, string mid, string token)
        {
            this._retailID = retailID ?? "0037";
            _mid = mid.Equals("0") ? string.Empty : mid;
            _token = token;
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
            get;
            set;
        }

        protected static string GetSessionId()
        {
            string sessionId = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                sessionId = HttpContext.Current.Session.SessionID;
            }
            else
            {
                sessionId = System.Guid.NewGuid().ToString().Replace("-", string.Empty);
            }
            return sessionId;
        }

        public string GetRegPassport()
        {
            return this.PassportID;
        }

        public bool CheckLogin()
        {
            string url = "";
            string AppKey = "";
            string AppId = "";
            bool isOldVersion = false;
            var sec = SdkSectionFactory.SectionDanle;
            if (sec != null)
            {
                url = sec.Url;
                isOldVersion = sec.IsOldVersion;
                var els = sec.Channels[_retailID];
                AppKey = els.AppKey;
                AppId = els.AppId;
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannel Danle section is null.");
            }
            string Url = "";
            if (isOldVersion)
            {
                string sig = AMD5(string.Format("api_key={0}&mid={1}&username={2}&sha256_pwd={3}&secret_key={4}", AppId, _mid, username, SHA256(Password), AppKey));
                string vc = AMD5(string.Format("api_key={0}&mid={1}&username={2}&sig={3}", AppId, PassportID, username, sig));
                Url = string.Format("http://connect.d.cn/connect/json/member/login?api_key={0}&mid={1}&username={2}&vc={3}&sig={4}", AppId, _mid, username, vc, sig);

            }
            else
            {
                string sig = AMD5(string.Format("{0}|{1}", _token, AppKey));
                Url = string.Format("{0}?app_id={1}&mid={2}&token={3}&sig={4}", url, AppId, _mid, _token, sig);
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "Get";
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();

            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();

            DanleSDK sdk = null;
            try
            {
                sdk = JsonUtils.Deserialize<DanleSDK>(result);
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
                return false;
            }
            if (sdk == null || sdk.error_code != 0)
            {
                TraceLog.ReleaseWrite("Danlesdk login fail:{0},request url:{1}", result, Url);
                return false;
            }

            string[] arr = SnsManager.LoginByRetail(_retailID, sdk.memberId);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            SessionID = GetSessionId();
            return true;
        }

        #endregion

        private string AMD5(string str1)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str1, "MD5").ToUpper();
        }

        private string SHA256(string str)
        {
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            sha256.Clear();
            string result = string.Empty;
            foreach (byte x in tmpByte)
            {
                result += string.Format("{0:x2}", x);
            }
            return result.ToUpper();
        }
    }

    public class DanleSDK
    {
        public string memberId;
        public string username;
        public string nickname;
        public string gender;
        public int level;
        public string avatar_url;
        public string created_date;
        public string token;
        public int error_code;
        public string error_msg;
    }
}