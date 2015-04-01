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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Sns.Service;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountServerLogin : ILogin
    {
        private int _timeout;
        private readonly string _url;
        private readonly string _imei;
        /// <summary>
        /// 
        /// </summary>
        public AccountServerLogin()
        {
            ContentType = "application/json";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="imei"></param>
        /// <param name="timeout"></param>
        public AccountServerLogin(string url, string token, string imei, int timeout = 3000)
            : this()
        {
            _url = url;
            _imei = imei;
            _timeout = timeout;
            Token = token;
        }
        #region property
        /// <summary>
        /// 
        /// </summary>
        public string PassportID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int UserType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SessionID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetRegPassport()
        {
            var query = new Dictionary<string, string>();
            query["Handler"] = "Passport";
            query["IMEI"] = _imei;
            var task = Send(HttpUtils.BuildPostParams(query), _timeout);
            if (task.Wait(_timeout))
            {
                var responseStream = (task.Result).GetResponseStream();
                if (responseStream == null) return null;
                using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string json = sr.ReadToEnd();
                    var body = JsonUtils.Deserialize<ResponseBody<PassportInfo>>(json);
                    if (body != null && body.StateCode == StateCode.OK)
                    {
                        var token = body.Data as PassportInfo;
                        if (token != null)
                        {
                            PassportID = token.PassportId;
                            Password = token.Password;
                            return PassportID;
                        }
                    }
                    TraceLog.WriteError("AccountServer get passport error:{0}", json);
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckLogin()
        {
            var query = new Dictionary<string, string>();
            query["Handler"] = "Validate";
            query["Token"] = Token;
            string queryString = HttpUtils.BuildPostParams(query);
            var task = Send(queryString, _timeout);
            if (task.Wait(_timeout))
            {
                var responseStream = (task.Result).GetResponseStream();
                if (responseStream == null) return false;
                using (var sr = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string json = sr.ReadToEnd();
                    var body = JsonUtils.Deserialize<ResponseBody<LoginToken>>(json);
                    if (body == null)
                    {
                        return false;
                    }

                    if (body.StateCode == StateCode.OK)
                    {
                        var token = body.Data as LoginToken;
                        if (token != null)
                        {
                            PassportID = token.PassportId;
                            UserID = token.UserId.ToString();
                            return true;
                        }
                    }
                    if (body.StateCode == StateCode.TokenExpired || body.StateCode == StateCode.NoToken)
                    {
                        throw new HandlerException(body.StateCode, body.StateDescription);
                    }
                    TraceLog.WriteError("AccountServer login error:{0}", json);
                    return false;
                }
            }
            TraceLog.WriteError("AccountServer login timeout error:{0}", _url + "?" + queryString);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Task<WebResponse> Send(string query, int timeout = 3000, bool isPost = false)
        {
            string queryString = AppendSign(query);
            return isPost
                ? HttpUtils.PostAsync(_url, queryString, timeout, null, Encoding.UTF8, ContentType, null)
                : HttpUtils.GetAsync(string.Format("{0}/?{1}", _url, queryString), ContentType, timeout, null, null);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual string AppendSign(string query)
        {
            string signKey = GameEnvironment.Setting != null ? GameEnvironment.Setting.ProductSignKey : "";
            if (string.IsNullOrEmpty(signKey))
            {
                return query;
            }
            string sign = CryptoHelper.MD5_Encrypt(query + signKey, Encoding.UTF8);
            return string.Format("{0}&Sign={1}", query, sign);
        }
    }
}
