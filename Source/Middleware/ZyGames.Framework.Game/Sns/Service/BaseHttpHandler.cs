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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.RPC.Http;

namespace ZyGames.Framework.Game.Sns.Service
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseHttpHandler : IHttpHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            ResponseType = ResponseType.Json;
            ResponseFormater = new JsonResponseFormater();
            var body = new ResponseBody();
            OnRequest(context, body);
            ProcessResponse(context.Response, body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public byte[] ProcessRequest(IHttpRequestContext context)
        {
            ResponseType = ResponseType.Json;
            ResponseFormater = new JsonResponseFormater();
            var body = new ResponseBody();
            OnRequest(context, body);
            return ProcessResponse(context, body);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        protected virtual void OnRequest(IHttpRequestContext context, ResponseBody body)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        protected virtual void OnRequest(HttpContext context, ResponseBody body)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="body"></param>
        protected void ProcessResponse(HttpResponse httpResponse, ResponseBody body)
        {
            try
            {
                SetResponseHead(httpResponse);
                var buffer = ResponseFormater.Serialize(body);
                httpResponse.BinaryWrite(buffer);
            }
            catch (Exception error)
            {
                TraceLog.WriteError("Response handle error:{0}", error);
                httpResponse.StatusCode = 500;
                httpResponse.StatusDescription = "Response error.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected byte[] ProcessResponse(IHttpRequestContext context, ResponseBody body)
        {
            return ResponseFormater.Serialize(body);
        }


        /// <summary>
        /// 
        /// </summary>
        protected IResponseFormater ResponseFormater { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected ResponseType ResponseType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponse"></param>
        protected void SetResponseHead(HttpResponse httpResponse)
        {
            httpResponse.ContentType = "text/plain; charset=utf8";
            switch (ResponseType)
            {
                case ResponseType.Json:
                    httpResponse.ContentType = "application/json; charset=utf8";
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual bool CheckSign(HttpRequest request, out string param)
        {
            param = null;
            var sign = request.QueryString["sign"];
            if (string.IsNullOrEmpty(sign)) return false;
            var query = request.Url.Query.Substring(1);
            var signIdx = query.IndexOf("sign", StringComparison.InvariantCultureIgnoreCase);
            param = query.Substring(0, signIdx - 1);
            var mysign = CryptoHelper.MD5_Encrypt(param + HandlerManager.SignKey, Encoding.UTF8);
            return String.Compare(sign, mysign, StringComparison.OrdinalIgnoreCase) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected virtual bool CheckSign(IHttpRequestContext context, out string param)
        {
            param = null;
            string query;
            string sign;
            if ("POST".Equals(context.Request.HttpMethod))
            {
                string postString;
                using (var read = new StreamReader(context.Request.InputStream))
                {
                    postString = read.ReadToEnd();
                }
                var signIdx = postString.IndexOf("sign=", StringComparison.InvariantCultureIgnoreCase);
                if (signIdx == -1) return false;
                param = postString.Substring(0, signIdx - 1);
                sign = signIdx + 5 < postString.Length ? postString.Substring(signIdx + 5) : "";
            }
            else
            {
                sign = context.Request.QueryString["sign"];
                if (string.IsNullOrEmpty(sign)) return false;
                query = context.Request.Url.Query.Substring(1);
                var signIdx = query.IndexOf("sign", StringComparison.InvariantCultureIgnoreCase);
                if (signIdx == -1) return false;
                param = query.Substring(0, signIdx - 1);
            }
            var mysign = CryptoHelper.MD5_Encrypt(param + HandlerManager.SignKey, Encoding.UTF8);
            return String.Compare(sign, mysign, StringComparison.OrdinalIgnoreCase) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="handlerData"></param>
        /// <returns></returns>
        protected bool TryUrlQueryParse(string query, out HandlerData handlerData)
        {
            handlerData = new HandlerData();
            handlerData.Params = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            var keyValues = (query ?? "").Split('&');
            foreach (var keyValue in keyValues)
            {
                var paris = keyValue.Split('=');
                if (paris.Length != 2) continue;
                string name = paris[0];
                string value = HttpUtility.UrlDecode(paris[1]);
                if (string.IsNullOrEmpty(name)) continue;

                if (string.Compare("Handler", name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    handlerData.Name = value;
                    continue;
                }
                handlerData.Params[name] = value;
            }
            return !string.IsNullOrEmpty(handlerData.Name);
        }

    }
}