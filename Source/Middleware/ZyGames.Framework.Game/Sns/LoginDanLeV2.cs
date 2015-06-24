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
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 当乐0037
    /// </summary>
    public class LoginDanLeV2 : AbstractLogin
    {
        private string _retailID;
        private string _mid;
        private string _token;
        private string username = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.LoginDanLeV2"/> class.
        /// </summary>
        /// <param name="retailID">Retail I.</param>
        /// <param name="RetailUser">Retail user.</param>
        /// <param name="token">Token.</param>
        public LoginDanLeV2(string retailID, string RetailUser, string token)
        {
            this._retailID = retailID ?? "0037";
            _mid = RetailUser.Equals("0") ? string.Empty : RetailUser;
            _token = token;
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
            string url = "";
            string AppKey = "";
            string AppId = "";
            bool isOldVersion = false;

            GameChannel gameChannel = ZyGameBaseConfigManager.GameSetting.GetChannelSetting(ChannelType.channelDanle);
            if (gameChannel != null)
            {
                url = gameChannel.Url;
                isOldVersion = "0.1".Equals(gameChannel.Version);
                GameSdkSetting setting = gameChannel.GetSetting(_retailID);
                if (setting != null)
                {
                    AppKey = setting.AppKey;
                    AppId = setting.AppId;
                }
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannelV2 Danle section is null.");
            }
            string sig = AMD5(_token + "|" + AppKey);
            string Url = string.Format("{0}?app_id={1}&mid={2}&token={3}&sig={4}", url, AppId, _mid, _token, sig);
            string result = HttpRequestManager.GetStringData(Url, "GET");

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
            if (sdk == null || sdk.error_code != 0 || sdk.memberId == null)
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


    }
    /// <summary>
    /// Danle v2 SD.
    /// </summary>
    public class DanleV2SDK
    {
        /// <summary>
        /// The member identifier.
        /// </summary>
        public string memberId;
        /// <summary>
        /// The username.
        /// </summary>
        public string username;
        /// <summary>
        /// The nickname.
        /// </summary>
        public string nickname;
        /// <summary>
        /// The gender.
        /// </summary>
        public string gender;
        /// <summary>
        /// The level.
        /// </summary>
        public int level;
        /// <summary>
        /// The avatar_url.
        /// </summary>
        public string avatar_url;
        /// <summary>
        /// The created_date.
        /// </summary>
        public string created_date;
        /// <summary>
        /// The token.
        /// </summary>
        public string token;
        /// <summary>
        /// The error_code.
        /// </summary>
        public int error_code;
        /// <summary>
        /// The error_msg.
        /// </summary>
        public string error_msg;
    }
}