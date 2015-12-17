using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Service;
using ZyGames.Framework.RPC.Sockets;

namespace FrameworkUnitTest.Rpc
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
        /// Message of custom
        /// </summary>
        public object Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SendTime { get; set; }

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
    public class RemoteService
    {

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
                try
                {
                    using (var ms = new MessageStructure(e.Data))
                    {
                        var head = ms.ReadHeadGzip();
                        if (head != null)
                        {
                            var package = proxy.Find(head.MsgId);
                            if (package != null)
                            {
                                package.Message = ms.ReadBuffer();
                                proxy.Remove(head.MsgId);
                                package.OnCallback();
                                return;
                            }
                        }
                    }
                }
                catch (Exception)
                { }
                proxy.OnPushedHandle(e);
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
                            package.Message = ms.ReadBuffer();
                            proxy.Remove(head.MsgId);
                            package.OnCallback();
                        }
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
        /// 
        /// </summary>
        public event RemoteCallback PushedHandle;

        private void OnPushedHandle(RemoteEventArgs e)
        {
            RemoteCallback handler = PushedHandle;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public int PackageCount { get { return _packagePools.Count; } }

        public string LocalAddress { get { return _client.LocalAddress; } }
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
                    heartParam["ActionId"] = 0;
                    string post = heartParam.ToPostString();
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
            string post = param.ToPostString();
            if (_client.IsSocket)
            {
                post = "?" + post;
            }
            var responsePack = new RemotePackage { MsgId = _msgId, RouteName = routePath };
            responsePack.Callback += callback;
            PutToWaitQueue(responsePack);
            _client.Send(post);
        }

        public void Close()
        {
            _client.Close();
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

    [TestClass]
    public class SocketTest
    {
        private int port = 2015;
        private SocketListener socketListener;

        [TestInitialize]
        public void Init()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            int maxConnections = 10000;
            int backlog = 1000;
            int maxAcceptOps = 10000;
            int bufferSize = 1024;

            var socketSettings = new SocketSettings(maxConnections, backlog, maxAcceptOps, bufferSize, localEndPoint, 0, 0);
            socketListener = new SocketListener(socketSettings);
            socketListener.Connected += OnConnectCompleted;
            socketListener.DataReceived += OnReceivedCompleted;
            socketListener.StartListen();
        }

        [TestMethod]
        public void Wait()
        {
            while (true)
            {
                Thread.Sleep(60000);
            }
        }

        public void Stop()
        {
            socketListener.Dispose();
        }

        private void OnReceivedCompleted(ISocket socket, ConnectionEventArgs e)
        {
            Trace.WriteLine(string.Format("Received client {0} data {1}", e.Socket.RemoteEndPoint, Encoding.UTF8.GetString(e.Data)));
            socket.PostSend(e.Socket, e.Data, 0, e.Data.Length);
        }

        private void OnConnectCompleted(ISocket socket, ConnectionEventArgs e)
        {
            //Trace.WriteLine(string.Format("Server accept client {0} connected", e.Socket.RemoteEndPoint));
        }

      
        public void LoadTestConnect()
        {
            var runMin = 1;
            var times = DateTime.Now;
            Task.Factory.StartNew(() =>
            {
                while (DateTime.Now < times.AddMinutes(runMin))
                {
                    Trace.WriteLine(string.Format("Socket connect status: TotalCount={0}, CurrentCount={1}, CloseCount={2}, RejectedCount={3}",
                    socketListener.Status.TotalConnectCount,
                    socketListener.Status.CurrentConnectCount,
                    socketListener.Status.CloseConnectCount,
                    socketListener.Status.RejectedConnectCount));
                    Thread.Sleep(1000);
                }
            });

            while (DateTime.Now < times.AddMinutes(runMin))
            {
                Task.Factory.StartNew(() =>
                {
                    EndPoint LocalEndPoint = null;
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                    using (ClientSocket client = new ClientSocket(new ClientSocketSettings(1024, remoteEndPoint), new RequestHandler(new MessageHandler())))
                    {
                        try
                        {
                            client.Connect();
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(string.Format("Client {0} connect error:{1} ", LocalEndPoint == null ? "" : LocalEndPoint.ToString(), ex.Message));
                            Assert.Inconclusive();
                        }
                    }

                });
                Thread.Sleep(10);
            }
            Trace.WriteLine("Client connect test end");
        }

        [TestMethod]
        public void Connect()
        {
            var tasks = new Task[200];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew((index) =>
                {
                    EndPoint LocalEndPoint = null;
                    IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                    using (ClientSocket client = new ClientSocket(new ClientSocketSettings(1024, remoteEndPoint), new RequestHandler(new MessageHandler())))
                    {
                        try
                        {
                            client.IsSyncReceived = true;
                            client.Connect();
                            LocalEndPoint = client.LocalEndPoint;
                            Trace.WriteLine(string.Format("Client connect ok,{0}", LocalEndPoint));
                            string str = "hello" + index;
                            byte[] data = Encoding.UTF8.GetBytes(str);
                            if (client.PostSend(data, 0, data.Length).Wait(3 * 1000) && client.Receive(3 * 1000))
                            {
                                byte[] buffer;
                                if (client.TryReceiveBytes(out buffer))
                                {
                                    Assert.AreEqual(Encoding.UTF8.GetString(buffer), str);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(string.Format("Client {0} connect error:{1} ", LocalEndPoint == null ? "" : LocalEndPoint.ToString(), ex.Message));
                            Assert.Inconclusive();
                        }
                    }

                }, i);
            }

            Task.WaitAll(tasks);
            Trace.WriteLine("Client connect test end");
            Thread.Sleep(60000);
        }

        private string msg;
        [TestMethod]
        public void Remote()
        {
            SocketRemoteClient client = new SocketRemoteClient("127.0.0.1", port, 30);
            client.Callback += OnReceived;
            client.Connect();
            Trace.WriteLine("LocalAddress:" + client.LocalAddress);
            Trace.Flush();
            Task.Factory.StartNew(() =>
            {
                client.Send("hello").Wait();
                client.Wait();
                Assert.AreEqual(msg, "hello");
            }).Wait();

            Task.Factory.StartNew(() =>
           {
               client.Send("hello2").Wait(1000);
               client.Wait();
               Assert.AreEqual(msg, "hello2");
           }).Wait();

            client.Close();

        }

        private void OnReceived(object sender, RemoteEventArgs e)
        {
            msg = Encoding.UTF8.GetString(e.Data);
        }

        [TestMethod]
        public void Remote2()
        {
            try
            {
                string loginIP = ConfigUtils.GetSetting("Login.Host", "127.0.0.1");
                //tcp的心跳包发送间隔时间60s
                var tcpRemote = RemoteService.CreateTcpProxy("proxy2", loginIP, port, 60 * 1000);
                tcpRemote.PushedHandle += OnPushCallback;
                SendRequestMaxUsers(tcpRemote);
                Trace.WriteLine("LocalAddress:" + tcpRemote.LocalAddress);
                Trace.Flush();
                Thread.Sleep(1000);
                tcpRemote.Close();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Remote2 error:{0}", ex.ToString());
            }
        }

        private void OnPushCallback(object sender, RemoteEventArgs e)
        {
            msg = Encoding.UTF8.GetString(e.Data);
            Trace.WriteLine("call1 msg:"+ msg);
        }

        public void SendRequestMaxUsers(RemoteService tcpRemote)
        {
            RequestParam param = new RequestParam();
            param.Add("ServerID", 1);
            tcpRemote.Call("ServerRequestMaxUsers", param, p =>
            {
                Trace.WriteLine("call1:"+p.RouteName);
            });

            param = new RequestParam();
            param.Add("ServerID", 2);
            param.Add("ID", 2);
            tcpRemote.Call("ServerRequestMaxUsers", param, p =>
            {
                Trace.WriteLine("call2:"+ p.RouteName);
            });
        }

    }
}
