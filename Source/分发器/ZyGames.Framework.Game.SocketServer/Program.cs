using System;
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.RPC.Wcf;

namespace ZyGames.Framework.Game.SocketServer
{
    class Program
    {
        private static SocketListener _socketListener;

        static void Main(string[] args)
        {
            try
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                WcfServiceClientManager.Current.InitConfig(path, OnSendTo);
                string host = ConfigUtils.GetSetting("SocketServer.Host");
                int port = ConfigUtils.GetSetting("SocketServer.Port").ToInt();
                int backlog = ConfigUtils.GetSetting("SocketServer.Backlog").ToInt();
                int connectTimeout = ConfigUtils.GetSetting("SocketServer.ConnectTimeout", "600").ToInt();
                int bufferSize = ConfigUtils.GetSetting("SocketServer.BufferSize").ToInt();
                int minAsyncPool = ConfigUtils.GetSetting("SocketServer.MinAsyncPool").ToInt();
                int maxAsyncPool = ConfigUtils.GetSetting("SocketServer.MaxAsyncPool").ToInt();
                ISocketReceiver receiver = new TcpSocketReceiver();
                SocketAsyncPool pool = new SocketAsyncPool(minAsyncPool, maxAsyncPool, bufferSize, receiver);
                _socketListener = new SocketListener(pool, connectTimeout);
                if (!string.IsNullOrEmpty(host))
                {
                    _socketListener.Listen(host, port, backlog);
                }
                else
                {
                    _socketListener.Listen(port, backlog);
                }
                _socketListener.OnClosing += new SocketClosingHandle(OnSocketClosed);
                Console.WriteLine("【Socket服务器-{0}】分发请求监听中...", port);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SocketServer error:{0}", ex);
            }
            finally
            {
                if (_socketListener != null)
                {
                    _socketListener.Close();
                }
                WcfServiceClientManager.Dispose();
            }
        }

        private static void OnSocketClosed(string remoteaddress)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private static void OnSendTo(string param, byte[] buffer)
        {
            string curr = DateTime.Now.ToLongTimeString();
            string msg = string.Format("[{0}]开始Push数据到{1},{2}byte", curr, param, buffer.Length);

            MessageStructure ms = new MessageStructure(buffer);
            var head = ms.ReadHead();
            if (head == null || _socketListener == null)
            {
                TraceLog.WriteError("Send to:{0} head is null.", param);
                return;
            }
            param = param ?? "";
            string[] addressList = param.Split(',');
            if (addressList.Length == 0)
            {
                Console.WriteLine("Socket send address is empty.");
            }
            msg += string.Format(",action:{0},error:{1}-{2}", head.Action, head.ErrorCode, head.ErrorInfo);
            Console.WriteLine(msg);
            TraceLog.ReleaseWrite(msg);
            //增加一层包大小
            MessageStructure ds = new MessageStructure();
            ds.WriteByte(buffer.Length);//用于Gig压缩后的包大小
            ds.WriteByte(buffer);
            buffer = ds.ReadBuffer();

            HashSet<string> addressSet = new HashSet<string>(addressList);
            _socketListener.Notify(s => addressSet.Contains(s.RemoteAddress), buffer,
                s =>
                {
                    Console.WriteLine("[{0}]Socket发送成功:{1}!", DateTime.Now.ToString("HH:mm:ss"), s.RemoteAddress);
                },
                fail =>
                {
                    Console.WriteLine("[{0}]Socket发送失败或连接关闭:{1}", DateTime.Now.ToString("HH:mm:ss"), fail.RemoteAddress);
                },
                error =>
                {
                    Console.WriteLine("[{0}]Socket发送异常:{1}", DateTime.Now.ToString("HH:mm:ss"), error);
                });
        }
    }
}
