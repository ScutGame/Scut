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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 
        /// </summary>
        Wait,
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 
        /// </summary>
        Close,
        /// <summary>
        /// 
        /// </summary>
        Error
    }

    /// <summary>
    /// Socket send async result
    /// </summary>
    public class SocketAsyncResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public SocketAsyncResult(byte[] data)
        {
            Result = ResultCode.Wait;
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public ExSocket Socket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected internal Action<SocketAsyncResult> ResultCallback;

        /// <summary>
        /// 
        /// </summary>
        public ResultCode Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal void Callback()
        {
            if (ResultCallback != null)
            {
                try
                {
                    ResultCallback(this);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("ResultCallback error{0}", ex);
                }
            }
        }
    }
}
