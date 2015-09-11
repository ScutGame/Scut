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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// Remote struct
    /// </summary>
    public abstract class RemoteStruct
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly ActionGetter paramGetter;
        /// <summary>
        /// 
        /// </summary>
        protected readonly MessageStructure response;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="paramGetter"></param>
        /// <param name="response"></param>
        protected RemoteStruct(ActionGetter paramGetter, MessageStructure response)
        {
            this.paramGetter = paramGetter;
            this.response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        protected int ErrorCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected string ErrorInfo { get; set; }

        /// <summary>
        /// 是否影响输出, True：不响应
        /// </summary>
        protected bool IsNotRespond;

        /// <summary>
        /// 
        /// </summary>
        internal void DoRemote()
        {
            if (Check())
            {
                try
                {
                    TakeRemote();
                    if (!IsNotRespond)
                    {
                        WriteResponse();
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("DoRemote error:{0}", ex);
                    if (!IsNotRespond)
                    {
                        WriteError();
                    }
                }
            }
            else
            {
                if (!IsNotRespond)
                {
                    WriteError();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void WriteResponse()
        {
            BuildPacket();
            WriteRemote();
        }

        private void WriteRemote()
        {
            int msgId = paramGetter.GetMsgId();
            int actionId = paramGetter.GetActionId();
            var head = new MessageHead(msgId, actionId, ErrorCode, ErrorInfo);
            response.WriteBuffer(head);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void WriteError()
        {
            int msgId = paramGetter.GetMsgId();
            int actionId = paramGetter.GetActionId();
            var head = new MessageHead(msgId, actionId, (int)MessageError.SystemError, ErrorInfo);
            response.WriteBuffer(head);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool Check();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract void TakeRemote();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void BuildPacket();

    }
}
