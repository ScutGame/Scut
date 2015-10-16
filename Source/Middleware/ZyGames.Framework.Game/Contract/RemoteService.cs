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
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common.Log;
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
        static RemoteService()
        {
            RequestParam.SignKey = GameEnvironment.Setting.ProductSignKey;
        }

        /// <summary>
        /// Create http proxy
        /// </summary>
        /// <param name="proxyId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static RemoteService CreateHttpProxy(string proxyId, string url)
        {
            return CreateHttpProxy(proxyId, url, Encoding.UTF8, null);
        }

        /// <summary>
        /// Create tcp proxy
        /// </summary>
        /// <param name="proxyId"></param>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static RemoteService CreateHttpProxy(string proxyId, string url, Encoding encoding, int? timeout)
        {
            var client = new HttpRemoteClient(url, encoding, timeout);
            client.Callback += OnNetHttpCallback;
            var proxy = new RemoteService(proxyId, client);
            client.RemoteTarget = proxy;
            return proxy;
        }

        /// <summary>
        /// Create tcp proxy
        /// </summary>
        /// <param name="proxyId"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="heartInterval"></param>
        /// <returns></returns>
        public static RemoteService CreateTcpProxy(string proxyId, string host, int port, int heartInterval)
        {
            var client = new SocketRemoteClient(host, port, heartInterval);
            client.Callback += OnNetTcpCallback;
            var proxy = new RemoteService(proxyId, client);
            client.RemoteTarget = proxy;
            return proxy;
        }

        private static void OnNetTcpCallback(object sender, RemoteEventArgs e)
        {
            try
            {
                RemoteService proxy = sender as RemoteService;
                if (proxy == null)
                {
                    return;
                }
                using (var ms = new MessageStructure(e.Data))
                {
                    var head = ms.ReadHeadGzip();
                    if (head != null)
                    {
                        var package = proxy.Find(head.MsgId);
                        if (package != null)
                        {
                            package.ErrorCode = head.ErrorCode;
                            package.ErrorInfo = head.ErrorInfo;
                            package.Message = ms.ReadBuffer();
                            proxy.Remove(head.MsgId);
                            package.OnCallback();
                        }
                        else
                        {
                            proxy.OnPushedHandle(e);
                        }
                    }
                    else
                    {
                        proxy.OnErrorHandle(e);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnNetTcpCallback error:{0}", ex);
            }
        }

        private static void OnNetHttpCallback(object sender, RemoteEventArgs e)
        {
            try
            {
                RemoteService proxy = sender as RemoteService;
                if (proxy == null)
                {
                    return;
                }
                using (var ms = new MessageStructure(e.Data))
                {
                    var head = ms.ReadHeadGzip();
                    if (head != null)
                    {
                        var package = proxy.Find(head.MsgId);
                        if (package != null)
                        {
                            package.ErrorCode = head.ErrorCode;
                            package.ErrorInfo = head.ErrorInfo;
                            package.Message = ms.ReadBuffer();
                            proxy.Remove(head.MsgId);
                            package.OnCallback();
                        }
                        else
                        {
                            proxy.OnPushedHandle(e);
                        }
                    }
                    else
                    {
                        proxy.OnErrorHandle(e);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnNetHttpCallback error:{0}", ex);
            }
        }

        private readonly string _proxyId;
        private readonly RemoteClient _client;
        private int _msgId;
        private string _sessionId = "";
        private int _userId = 0;
        private string _proxySessionId = "";
        private DictionaryExtend<int, RemotePackage> _packagePools;
        /// <summary>
        /// Server push to data callback
        /// </summary>
        public event RemoteCallback PushedHandle;

        /// <summary>
        /// 
        /// </summary>
        public event RemoteCallback ErrorHandle;

        private void OnErrorHandle(RemoteEventArgs e)
        {
            RemoteCallback handler = ErrorHandle;
            if (handler != null) handler(this, e);
        }

        private void OnPushedHandle(RemoteEventArgs e)
        {
            RemoteCallback handler = PushedHandle;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public int PackageCount { get { return _packagePools.Count; } }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="proxyId">local name</param>
        /// <param name="client"></param>
        private RemoteService(string proxyId, RemoteClient client)
        {
            _packagePools = new DictionaryExtend<int, RemotePackage>();
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
                    RequestParam heartParam = new RequestParam();
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
        /// <param name="routePath">Call method path, ex:className.method</param>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        public void Call(string routePath, RequestParam param, Action<RemotePackage> callback)
        {
            _msgId++;
            param["MsgId"] = _msgId;
            param["route"] = routePath;
            param["Sid"] = _sessionId;
            param["Uid"] = _userId;
            param["ActionId"] = 0;
            param["ssid"] = _proxySessionId;
            param["isproxy"] = true;
            param["proxyId"] = _proxyId;
            string post = string.Format("d={0}", HttpUtility.UrlEncode(param.ToPostString()));
            if (_client.IsSocket)
            {
                post = "?" + post;
            }
            var responsePack = new RemotePackage { MsgId = _msgId, RouteName = routePath };
            responsePack.Callback += callback;
            PutToWaitQueue(responsePack);
            _client.Send(post);
        }

        private void PutToWaitQueue(RemotePackage package)
        {
            _packagePools[package.MsgId] = package;
        }

        private RemotePackage Find(int msgId)
        {
            return _packagePools[msgId];
        }

        private bool Remove(int msgId)
        {
            return _packagePools.Remove(msgId);
        }
    }
}
