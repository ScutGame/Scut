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
using System.Threading.Tasks;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Game.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ProtocolSection : ConfigSection
    {
        /// <summary>
        /// init
        /// </summary>
        public ProtocolSection()
        {
            HttpHost = ConfigUtils.GetSetting("Game.Http.Host");
            HttpPort = ConfigUtils.GetSetting("Game.Http.Port", 80);
            HttpName = ConfigUtils.GetSetting("Game.Http.Name", "Service.aspx");
            HttpRequestTimeout = ConfigUtils.GetSetting("Game.Http.Timeout",120000);

            SocketMaxConnection = ConfigUtils.GetSetting("MaxConnections", 10000);
            SocketBacklog = ConfigUtils.GetSetting("Backlog", 1000);
            SocketMaxAcceptOps = ConfigUtils.GetSetting("MaxAcceptOps", 1000);
            SocketBufferSize = ConfigUtils.GetSetting("BufferSize", 8192);
            //no use
            SocketExpireInterval = ConfigUtils.GetSetting("ExpireInterval", 600) * 1000;
            SocketExpireTime = ConfigUtils.GetSetting("ExpireTime", 3600) * 1000;

            SignKey = ConfigUtils.GetSetting("Product.SignKey", "");
            GameIpAddress = ConfigUtils.GetSetting("Game.IpAddress");
            GamePort = ConfigUtils.GetSetting("Game.Port", 9101);
            EnableActionGZip = ConfigUtils.GetSetting("Game.Action.EnableGZip", true);
            ActionGZipOutLength = ConfigUtils.GetSetting("Game.Action.GZipOutLength", 1024);//1k
        }

        /// <summary>
        /// 
        /// </summary>
        public string HttpHost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int HttpPort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HttpName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int HttpRequestTimeout { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string GameIpAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SocketMaxConnection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SocketBacklog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SocketMaxAcceptOps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SocketBufferSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SocketExpireInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SocketExpireTime { get; set; }

        /// <summary>
        /// receive data sign key
        /// </summary>
        public string SignKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int GamePort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ActionGZipOutLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnableActionGZip { get; set; }

    }
}
