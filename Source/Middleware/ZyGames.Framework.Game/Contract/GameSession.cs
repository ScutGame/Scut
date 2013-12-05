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
using ProtoBuf;
using System.Threading;
using System.Collections.Concurrent;
using ZyGames.Framework.RPC.Sockets;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 用户会话
    /// </summary>
    [ProtoContract]
    public class GameSession
    {
        /// <summary>
        /// SessionId
        /// </summary>
        [ProtoMember(1)]
        public string SessionId { get; internal set; }
        /// <summary>
        /// UserId
        /// </summary>
        [ProtoMember(2)]
        public long UserId { get; internal set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        [ProtoMember(7)]
        public DateTime LastActivityTime { get; internal set; }
        /// <summary>
        /// Gets or sets the SS identifier.
        /// </summary>
        /// <value>The SS identifier.</value>
        [ProtoMember(15)]
        public Guid SSId { get; internal set; }

        internal ExSocket Channel;

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool Connected { get { return Channel.WorkSocket.Connected; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GameSession()
        {
            LastActivityTime = DateTime.Now;
        }

        private int isInSession;
        internal bool EnterSession()
        {
            return Interlocked.CompareExchange(ref isInSession, 1, 0) == 0;
        }
        internal void ExitSession()
        {
            Interlocked.Exchange(ref isInSession, 0);
        }
    }
}