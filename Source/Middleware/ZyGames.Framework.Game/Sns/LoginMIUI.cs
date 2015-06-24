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
using System.Collections;
using System.Text;
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Sns
{
	/// <summary>
	/// Login MIU.
	/// </summary>
    public class LoginMIUI : AbstractLogin
    {
        private string _retailID = string.Empty;
        private string _pid = string.Empty;
        private string _retailUser = string.Empty;
        private string AppId = string.Empty;
        private string AppKey = string.Empty;
        private string _code = string.Empty;
        private string Url = string.Empty;
        private string _aceessTokenUrl = string.Empty;
        private string _appSecret = string.Empty;
        private string _sid = string.Empty;
		/// <summary>
		/// The parameters.
		/// </summary>
        protected Hashtable parameters = new Hashtable();

		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.LoginMIUI"/> class.
		/// </summary>
		/// <param name="retailID">Retail I.</param>
		/// <param name="retailUser">Retail user.</param>
		/// <param name="sid">Sid.</param>
        public LoginMIUI(string retailID, string retailUser, string sid)
        {
            this._retailID = retailID;
            this._retailUser = retailUser;
            this._sid = sid;
            GameChannel gameChannel = ZyGameBaseConfigManager.GameSetting.GetChannelSetting(ChannelType.channelMIUI);
            if (gameChannel != null)
            {
                Url = gameChannel.Url;
                GameSdkSetting sdkSetting = gameChannel.GetSetting(retailID);
                if (sdkSetting != null)
                {
                    AppId = sdkSetting.AppId;
                    AppKey = sdkSetting.AppKey;
                }
                else
                {
                    TraceLog.ReleaseWrite("The sdkChannel section channelMIUI:{0} is null.", retailID);
                }
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannel MIUI section is null.");
            }
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
            SetParameter("appId", AppId);
            SetParameter("session", _sid);
            SetParameter("uid", _retailUser);
            string sing = GetSign();
            sing = EncryptionManager.HMACSHA1Encrypt(sing, AppKey);
            string urlData = string.Format("appId={0}&session={1}&uid={2}&signature={3}",
                AppId,
                _sid,
                _retailUser,
                HttpUtility.UrlEncode(sing)
            );
            urlData = Url + "?" + urlData;
            string result = HttpRequestManager.GetStringData(urlData, "GET");
            try
            {
                if (!string.IsNullOrEmpty(result))
                {
                    var sdk = JsonUtils.Deserialize<SDKMIUIError>(result);
                    if (sdk == null || (!string.IsNullOrEmpty(sdk.errcode) && sdk.errcode != "200"))
                    {
                        TraceLog.ReleaseWrite("MIUIsdk login get user info fail:{0},errorCode:{1},request url:{2}", sdk.errMsg,
                                              sdk.errcode, urlData);
                        return false;
                    }
                    string[] arr = SnsManager.LoginByRetail(_retailID, _sid);
                    this.UserID = arr[0];
                    this.PassportID = arr[1];
                    QihooUserID = _sid;
                    SessionID = GetSessionId();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
                return false;
            }
        }
		/// <summary>
		/// Sets the parameter.
		/// </summary>
		/// <param name="parameter">Parameter.</param>
		/// <param name="parameterValue">Parameter value.</param>
        public void SetParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }

                parameters.Add(parameter, parameterValue);
            }
        }
		/// <summary>
		/// Gets the sign.
		/// </summary>
		/// <returns>The sign.</returns>
        public string GetSign()
        {

            ArrayList akeys = new ArrayList(parameters.Keys);
            akeys.Sort();
            StringBuilder strSign = new StringBuilder();
            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v)
                {
                    strSign.Append(k + "=" + v + "&");
                }
            }

            return strSign.ToString().TrimEnd('&');
        }
		/// <summary>
		/// 
		/// </summary>
        public class SDKMIUIError
        {
			/// <summary>
			/// Gets or sets the errcode.
			/// </summary>
			/// <value>The errcode.</value>
            public string errcode { get; set; }
			/// <summary>
			/// Gets or sets the error message.
			/// </summary>
			/// <value>The error message.</value>
            public string errMsg { get; set; }
        }


    }
}