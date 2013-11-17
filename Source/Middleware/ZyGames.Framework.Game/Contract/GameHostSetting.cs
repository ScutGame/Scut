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
using System.Net;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class GameHostSetting
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <param name="remotePoint"></param>
        public GameHostSetting(int gameId, int serverId, IPEndPoint remotePoint)
        {
            GameId = gameId;
            ServerId = serverId;
            RemotePoint = remotePoint;
            ConnectTimeout = 3;
            ConnectNum = 10;
            IntervalTime = 10;
            EnableGzip = false;
            ClientNum = 10;
            BufferSize = 1024;
        }

        /// <summary>
        /// 客户端数
        /// </summary>
        public int ClientNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPEndPoint RemotePoint { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GameId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; private set; }
        /// <summary>
        /// 连接超时（秒）
        /// </summary>
        public int ConnectTimeout { get; set; }
        /// <summary>
        /// 心跳包间隔时间（秒）
        /// </summary>
        public int IntervalTime { get; set; }
        /// <summary>
        /// 是否开启Gzip压缩
        /// </summary>
        public bool EnableGzip { get; set; }

        /// <summary>
        /// 启用的连接数
        /// </summary>
        public int ConnectNum { get; set; }
    }
}