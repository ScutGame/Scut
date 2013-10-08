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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Game.Sns.Section;

namespace ZyGames.Framework.Game.Contract.Action
{
    /// <summary>
    /// 360SDK AccessToken刷新获取
    /// </summary>
    public abstract class ReAccessTokenAction : BaseStruct
    {
        protected string AccessToken360;
        private string RetailID = string.Empty;
        private string RefeshToken = string.Empty;
        private string Scope = string.Empty;
        protected ReAccessTokenAction(short actionId, HttpGet httpGet)
            : base(actionId, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(AccessToken360);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("RetailID", ref RetailID)
                && httpGet.GetString("RefeshToken", ref RefeshToken)
                && httpGet.GetString("Scope", ref Scope))
            {
                return true;
            }
            return false;
        }


        public override bool CheckAction()
        {
            if (!GameEnvironment.IsRunning)
            {
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                ErrorInfo = LanguageHelper.GetLang().ServerLoading;
                return false;
            }
            return true;
        }

        private BaseUser GetUser()
        {
            return UserFactory == null ? null : UserFactory(UserId);
        }

        public override bool TakeAction()
        {
            var user = GetUser();
            if (user!=null)
            {
                AccessToken360 = user.Token360 ?? AccessToken360;
            }
            var sec = SdkSectionFactory.Section360;
            string appKey = "";
            string appSecret = "";
            string url = "{0}?grant_type=refresh_token&refresh_token={1}&client_id={2}&client_secret={3}&scope={4}";
            if (sec != null)
            {
                var els = sec.Channels[RetailID];
                if (els != null)
                {
                    appKey = els.AppKey;
                    appSecret = els.AppSecret;
                    url = string.Format(url, sec.GetAceessTokenUrl, RefeshToken, appKey, appSecret, Scope);
                }
            }
            string result = HttpRequestManager.GetStringData(url, "GET");
            var getToken = JsonUtils.Deserialize<Login360_V2.SDK360GetTokenError>(result);

            if (getToken != null && !string.IsNullOrEmpty(getToken.error_code))
            {
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                ErrorInfo = LanguageHelper.GetLang().GetAccessFailure;
                TraceLog.WriteError("获取360 access_token 失败：url={0},result={1},error_code={2},error={3}", url, result,
                                    getToken.error_code, getToken.error);
                return false;
            }
            if (getToken != null)
            {
                AccessToken360 = getToken.access_token;
                user.Token360 = AccessToken360;
            }
            return true;
        }


    }
}