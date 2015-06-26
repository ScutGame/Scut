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
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class RemotePackage
    {
        /// <summary>
        /// init
        /// </summary>
        public RemotePackage()
        {
            SendTime = DateTime.Now;
        }
        /// <summary>
        /// message id of client request
        /// </summary>
        public int MsgId { get; set; }

        /// <summary>
        /// 服务器间内部通讯通道
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorInfo { get; set; }

        /// <summary>
        /// Message of custom
        /// </summary>
        public object Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// is pushed package
        /// </summary>
        [JsonIgnore]
        public bool IsPushed { get { return MsgId == 0; } }

        /// <summary>
        /// callback
        /// </summary>
        public event Action<RemotePackage> Callback;

        internal virtual void OnCallback()
        {
            try
            {
                Action<RemotePackage> handler = Callback;
                if (handler != null) handler(this);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("RemotePackage OnCallback error:{0}", ex);
            }
        }
    }
}
