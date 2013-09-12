using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.SocketServer.Net;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.RPC.Wcf;
using ZyGames.Framework.RPC.Web;

namespace ZyGames.Framework.Game.SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ListenerProxy gameListener = null;
            ListenerProxy clientListener = null;
            HttpListenerProxy httpListener = null;
            try
            {
                var gameTran = new GameTransponder();
                var clientTran = new HttpTransponder();
                clientTran.ReceiveCompleted += gameTran.Send;
                clientTran.SocketClosing += gameTran.OnTcpClientClosed;
                gameTran.ReceiveCompleted += clientTran.Send;

                ConnectSetting gameSetting = LoadGameSetting();
                ConnectSetting clientSetting = LoadClientSetting();
                HttpSettings httpSetting = LoadHttpSetting();
                gameListener = new ListenerProxy(gameTran, gameSetting);
                clientListener = new ListenerProxy(clientTran, clientSetting);
                httpListener = new HttpListenerProxy(httpSetting);
                httpListener.RequestCompleted += clientTran.Request;
                httpListener.RequestTimeout += clientTran.RequestTimeout;
                clientTran.ResponseCompleted += httpListener.PushSend;

                gameListener.Listen();
                Console.WriteLine("【游戏服监听端口:{0}】正在监听中...", gameSetting.Port);

                clientListener.Listen();
                Console.WriteLine("【客户端监听端口:{0}】正在监听中...", clientSetting.Port);

                httpListener.Listen();
                Console.WriteLine("【Http监听端口:{0}:{1}/{2}】正在监听中...", httpSetting.HostAddress, httpSetting.Port, httpSetting.GameAppName);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SocketServer error:{0}", ex);
            }
            finally
            {
                if (clientListener != null)
                {
                    clientListener.Dispose();
                }

                if (gameListener != null)
                {
                    gameListener.Dispose();
                }
                if (httpListener != null)
                {
                    httpListener.Dispose();
                }

            }
        }

        private static HttpSettings LoadHttpSetting()
        {
            string preKey = "HttpListener";
            var setting = new HttpSettings();
            setting.HostAddress = ConfigUtils.GetSetting(preKey + ".Host");
            setting.Port = ConfigUtils.GetSetting(preKey + ".Port", "80").ToInt();
            setting.GameAppName = ConfigUtils.GetSetting(preKey + ".GameAppName");
            setting.RequestTimeout = ConfigUtils.GetSetting(preKey + ".RequestTimeout", "120000").ToInt();
            return setting;
        }

        private static ConnectSetting LoadClientSetting()
        {
            return LoadSetting("ClientListener");
        }


        private static ConnectSetting LoadGameSetting()
        {
            return LoadSetting("GameListener");
        }

        private static ConnectSetting LoadSetting(string preKey)
        {
            var setting = new ConnectSetting();
            setting.Host = ConfigUtils.GetSetting(preKey + ".Host");
            setting.Port = ConfigUtils.GetSetting(preKey + ".Port").ToInt();
            setting.Backlog = ConfigUtils.GetSetting(preKey + ".Backlog", "10").ToInt();
            setting.ConnectTimeout = ConfigUtils.GetSetting(preKey + ".ContinuedTimeout", "60").ToInt();
            setting.BufferSize = ConfigUtils.GetSetting(preKey + ".BufferSize", "1024").ToInt();
            setting.MinPoolSize = ConfigUtils.GetSetting(preKey + ".MinConnectNum", "10").ToInt();
            setting.MaxPoolSize = ConfigUtils.GetSetting(preKey + ".MaxConnectNum", "1000").ToInt();
            setting.EnableReceiveTimeout = ConfigUtils.GetSetting(preKey + ".EnableReceiveTimeout", "false").ToBool();
            setting.ReceiveTimeout = ConfigUtils.GetSetting(preKey + ".ReceiveTimeout", "30000").ToInt();
            return setting;
        }
    }
}
