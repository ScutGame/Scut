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
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Sns._91sdk;
using ZyGames.Framework.Game.Sns.Section;

namespace ZyGames.Framework.Game.Sns
{
    public class Login360_V2 : AbstractLogin
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
        public Login360_V2(string retailID, string retailUser, string pid, string code)
        {
            this._retailID = retailID;
            this._pid = pid;
            this._retailUser = retailUser;
            this._code = code;
            var sec = SdkSectionFactory.Section360;
            if (sec != null)
            {
                var els = sec.Channels[retailID];
                if (els == null)
                {
                    TraceLog.ReleaseWrite("The sdkChannel section channel360:{0} is null.", retailID);
                }
                else
                {
                    Url = sec.Url;
                    AppId = els.AppId;
                    AppKey = els.AppKey;
                    _aceessTokenUrl = sec.AceessTokenUrl;
                    _appSecret = els.AppSecret;
                   
                }
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannel 360 section is null.");
            }
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

       

        public override string GetRegPassport()
        {
            return this.PassportID;
        }

        public override bool CheckLogin()
        {

            string getAccessTokenUrl = string.Format("{0}?grant_type=authorization_code&code={1}&client_id={2}&client_secret={3}&redirect_uri=oob",
                _aceessTokenUrl,
                _code,
                AppKey,
                _appSecret);
            string resultGetToken = HttpRequestManager.GetStringData(getAccessTokenUrl, "GET");
            if (!string.IsNullOrEmpty(resultGetToken))
            {
                var sdkGetToken = JsonUtils.Deserialize<SDK360GetTokenError>(resultGetToken);
                if (!string.IsNullOrEmpty(sdkGetToken.error_code))
                {
                    TraceLog.ReleaseWrite("360sdk login get token fail:{0},errorCode:{1},request url:{2}", sdkGetToken.error,
                                          sdkGetToken.error_code, getAccessTokenUrl);
                    return false;
                }
                AccessToken = sdkGetToken.access_token;
                RefeshToken = sdkGetToken.refresh_token;
                Scope = sdkGetToken.scope;
                ExpiresIn = Convert.ToInt32(sdkGetToken.expires_in);
            }
            else
            {
                return false;
            }
            string urlData = string.Format("{0}?access_token={1}",
                Url,
                AccessToken
            );

            string result = HttpRequestManager.GetStringData(urlData,"GET");
            try
            {
                if (!string.IsNullOrEmpty(result))
                {
                    var sdk = JsonUtils.Deserialize<SDK360Error>(result);
                    if (!string.IsNullOrEmpty(sdk.error_code))
                    {
                        TraceLog.ReleaseWrite("360sdk login get user info fail:{0},errorCode:{1},request url:{2}", sdk.error,
                                              sdk.error_code, urlData);
                        return false;
                    }
                    string[] arr = SnsManager.LoginByRetail(_retailID, sdk.id);
                    this.UserID = arr[0];
                    this.PassportID = arr[1];
                    QihooUserID = sdk.id;
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

        public class SDK360Error
        {
            public string error_code { get; set; }
            public string error { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string avatar { get; set; }
            public string sex { get; set; }
            public string area { get; set; }
            public string nick { get; set; }
        }

        public class SDK360GetTokenError
        {
            public string error_code { get; set; }
            public string error { get; set; }
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
 
        }
    }
}