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
using System.Linq;
using System.Web;
using AccountServer.Lang;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Sns.Service;

namespace AccountServer
{
    /// <summary>
    /// Login or regster
    /// </summary>
    public class Default : BaseHttpHandler
    {
        protected override void OnRequest(HttpContext context, ResponseBody body)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                string param;
                if (CheckSign(context.Request, out param))
                {
                    HandlerData handlerData;
                    if (TryUrlQueryParse(param, out handlerData))
                    {
                        body.Handler = handlerData.Name;
                        body.Data = HandlerManager.Excute(handlerData);
                    }
                    else
                    {
                        body.StateCode = StateCode.NoHandler;
                        body.StateDescription = StateDescription.NoHandler;
                    }
                }
                else
                {
                    body.StateCode = StateCode.SignError;
                    body.StateDescription = StateDescription.SignError;//"Sign error.";
                }
            }
            catch (HandlerException handlerError)
            {
                body.StateCode = handlerError.StateCode;
                body.StateDescription = handlerError.Message;
                TraceLog.WriteError("Request handle error:{0}", handlerError);
            }
            catch (Exception error)
            {
                body.StateCode = StateCode.Error;
                body.StateDescription = StateDescription.Error;// "Process request fail.";
                TraceLog.WriteError("Request handle error:{0}", error);
            }
            var ms = watch.ElapsedMilliseconds;
            if (ms > 20)
            {
                TraceLog.Write("Request timeout:{0}ms", ms);
            }
        }
    }
}