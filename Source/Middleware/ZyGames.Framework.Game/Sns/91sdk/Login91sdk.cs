using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using ZyGames.Framework.Game.Sns._91sdk;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Game.Sns.Section;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 91SDK 0001
    /// </summary>
    public class Login91sdk : ILogin
    {
        private string _retailID = string.Empty;
        private string _sessionID = string.Empty;
        private string AppId;
        private string AppKey;
        private string Url;

        public Login91sdk(string retailID, string retailUser, string sessionID)
        {
            this._retailID = retailID;
            this._sessionID = sessionID;
            Uin = retailUser;
            var sec = SdkSectionFactory.Section91;
            if (sec != null)
            {
                var els = sec.Channels[retailID];
                if (els == null)
                {
                    TraceLog.ReleaseWrite("The sdkChannel section channel91:{0} is null.", retailID);
                }
                else
                {
                    Url = sec.Url;
                    AppId = els.AppId;
                    AppKey = els.AppKey;
                }
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannel 91 section is null.");
            }
        }


        private string Uin
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

        public string UserID
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

        #region ILogin 成员

        public string GetRegPassport()
        {
            return this.PassportID;
        }

        public bool CheckLogin()
        {
            int Act = 4;
            string Sign = string.Format("{0}{1}{2}{3}{4}", AppId, Act, Uin, _sessionID, AppKey);
            Sign = HashToMD5Hex(Sign);

            string urlData = string.Format("{0}?AppId={1}&Act={2}&Uin={3}&SessionID={4}&Sign={5}",
                Url,
                AppId,
                Act,
                Uin,
                SessionID,
                Sign
            );

            string result = HttpPostManager.GetStringData(urlData);
            try
            {
                SDKError sdk = JsonUtils.Deserialize<SDKError>(result);
                if (sdk.ErrorCode != "1")
                {
                    TraceLog.ReleaseWrite("91sdk login fail:{0},request url:{1}", sdk.ErrorDesc, urlData);
                    return false;
                }
                string[] arr = SnsManager.LoginByRetail(_retailID, Uin);
                this.UserID = arr[0];
                this.PassportID = arr[1];
                return true;
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
                return false;
            }

            //return !string.IsNullOrEmpty(UserID) && UserID != "0";
        }

        #endregion

        /// <summary>
        /// UTF8编码字符串计算MD5值(十六进制编码字符串)
        /// </summary>
        /// <param name="sourceStr">UTF8编码的字符串</param>
        /// <returns>MD5(十六进制编码字符串)</returns>
        private static string HashToMD5Hex(string sourceStr)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = md5.ComputeHash(bytes);
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sBuilder.Append(result[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

    }

    public class SDKError
    {
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
    }
}
