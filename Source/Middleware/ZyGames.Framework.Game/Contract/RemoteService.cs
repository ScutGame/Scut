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
using System.Text;
using System.Web;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Service;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// Remote service.
    /// </summary>
    public class RemoteService
    {
        /// <summary>
        /// Create http proxy
        /// </summary>
        /// <param name="proxyId"></param>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static RemoteService CreateHttpProxy(string proxyId, string url, RemoteCallback callback)
        {
            return CreateHttpProxy(proxyId, url, Encoding.UTF8, null, callback);
        }

        /// <summary>
        /// Create tcp proxy
        /// </summary>
        /// <param name="proxyId"></param>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="timeout"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static RemoteService CreateHttpProxy(string proxyId, string url, Encoding encoding, int? timeout, RemoteCallback callback)
        {
            var client = new HttpRemoteClient(url, encoding, timeout);
            client.Callback += callback;
            return new RemoteService(proxyId, client);
        }

        /// <summary>
        /// Create tcp proxy
        /// </summary>
        /// <param name="proxyId"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="heartInterval"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static RemoteService CreateTcpProxy(string proxyId, string host, int port, int heartInterval, RemoteCallback callback)
        {
            var client = new SocketRemoteClient(host, port, heartInterval);
            client.Callback += callback;
            return new RemoteService(proxyId, client);
        }
        
        private readonly string _proxyId;
        private readonly RemoteClient _client;
        private int _msgId;
        private string _sessionId = "";
        private int _userId = 0;
        private string _proxySessionId = "";


        /// <summary>
        /// init
        /// </summary>
        /// <param name="proxyId">local name</param>
        /// <param name="client"></param>
        private RemoteService(string proxyId, RemoteClient client)
        {
            _proxySessionId = Guid.NewGuid().ToString("N");
            _proxyId = string.IsNullOrEmpty(proxyId) ? _proxySessionId : proxyId;
            _client = client;
            InitClient();
        }

        private void InitClient()
        {
            try
            {
                if (_client is SocketRemoteClient)
                {
                    var client = _client as SocketRemoteClient;
                    RequestParam heartParam = CreateParameter();
                    heartParam["ActionId"] = (int)ActionEnum.Heartbeat;
                    string post = string.Format("?d={0}", HttpUtility.UrlEncode(heartParam.ToPostString()));
                    client.HeartPacket = Encoding.ASCII.GetBytes(post);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routePath">Call method path</param>
        /// <param name="param"></param>
        public void Call(string routePath, RequestParam param)
        {
            _msgId++;
            param["MsgId"] = _msgId;
            param["route"] = routePath;
            string post = string.Format("d={0}", HttpUtility.UrlEncode(param.ToPostString()));
            if (_client.IsSocket)
            {
                post = "?" + post;
            }
            _client.Send(post);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public RequestParam CreateParameter()
        {
            var param = new RequestParam(GameEnvironment.Setting.ProductSignKey);
            param["Sid"] = _sessionId;
            param["Uid"] = _userId;
            param["ActionId"] = 0;
            param["ssid"] = _proxySessionId;
            param["isproxy"] = true;
            param["proxyId"] = _proxyId;
            return param;
        }
    }
}
