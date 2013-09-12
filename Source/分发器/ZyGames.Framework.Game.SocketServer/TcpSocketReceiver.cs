using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Security;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.RPC.Wcf;

namespace ZyGames.Framework.Game.SocketServer
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    internal class TcpSocketReceiver : ISocketReceiver
    {
        private static bool EnableError;
        private static string ErrorNotFind;
        private static string ErrorConnected;
        private static string ErrorTimeout;
        private static string ErrorUnknown;
        private static string ErrorCallAccessLimit;
        private static HashSet<string> _accessLimitIp = new HashSet<string>();
        //private MessageStructure _buffer;
        //private ParamGeter paramGeter;

        static TcpSocketReceiver()
        {
            EnableError = ConfigUtils.GetSetting("Enable.Error").ToBool();
            ErrorNotFind = ConfigUtils.GetSetting("Error.NotFind");
            ErrorConnected = ConfigUtils.GetSetting("Error.Connected");
            ErrorTimeout = ConfigUtils.GetSetting("Error.Timeout");
            ErrorUnknown = ConfigUtils.GetSetting("Error.Unknown");
            ErrorCallAccessLimit = ConfigUtils.GetSetting("Error.CallAccessLimit");
            string[] ips = ConfigUtils.GetSetting("SocketServer.Ip.CallAccessLimit").Split(',');
            foreach (var ip in ips)
            {
                _accessLimitIp.Add(ip);
            }
        }

        public TcpSocketReceiver()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] Receive(string session, byte[] buffer)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            BufferReader reader = new BufferReader(buffer);
            string routeName = "";
            string paramString = reader.ReadPacketString();

#if DEBUG
            //todo trace
            TraceLog.ReleaseWrite("Tcp param:{0}", paramString);
#endif
            bool isRoute = false;
            if (!string.IsNullOrEmpty(paramString) && paramString.StartsWith("route:", StringComparison.CurrentCultureIgnoreCase))
            {
                //检查参数格式：route:name?d=param
                isRoute = true;
                string[] paramArray = paramString.Split('?');
                if (paramArray.Length == 2)
                {
                    routeName = paramArray[0].Replace("route:", "");
                    paramString = "?" + paramArray[1];
                }
            }
            paramString = HttpUtility.UrlDecode(paramString, Encoding.UTF8);
            int index = paramString.IndexOf("?d=");
            if (index != -1)
            {
                index += 3;
                paramString = paramString.Substring(index, paramString.Length - index);
            }
            var paramGeter = new ParamGeter(paramString);
            string remoteAddress = session.RemoteAddress;
            MessageHead head = ParseMessageHead(paramGeter);
            head.HasGzip = true;
            session.UserData = head;
            MessageStructure ms = new MessageStructure();
#if DEBUG
            Console.WriteLine("{0}>>请求参数:MsgId:{1},ActionId:{2},ip:{3}", DateTime.Now.ToLongTimeString(),
                head.MsgId, head.Action, remoteAddress);

#endif
            var settings = ParseRequestSettings(paramGeter, remoteAddress);
            settings.ParamString = paramString;
            settings.RouteName = routeName;

            byte[] sendBuffer = new byte[0];
            RequestError error = RequestError.Success;
            try
            {
                if (isRoute)
                {
                    if (CheckCallAccessLimit(remoteAddress))
                    {
                        error = RequestError.Unknown;
                        head.ErrorInfo = ErrorCallAccessLimit;
                    }
                    else
                    {
                        ServiceRequest.CallRemote(settings, out sendBuffer);
                    }
                }
                else
                {
                    ServiceRequest.Request(settings, out sendBuffer);
                }
            }
            catch (CommunicationObjectFaultedException fault)
            {
                TraceLog.WriteError("The wcfclient request faulted:{0}", fault);
                error = RequestError.Closed;
                ServiceRequest.ResetChannel(settings);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SocketException)
                {
                    var sex = ex.InnerException as SocketException;
                    TraceLog.WriteError("The wcfclient request connect:{0}-{1}", sex.SocketErrorCode, sex);
                    if (sex.SocketErrorCode == SocketError.TimedOut)
                    {
                        error = RequestError.Timeout;
                    }
                    else
                    {
                        error = RequestError.UnableConnect;
                    }
                }
                else
                {
                    TraceLog.WriteError("The wcfclient request error:{0}", ex);
                    error = RequestError.Unknown;
                }
                ServiceRequest.ResetChannel(settings);
            }
            watch.Stop();
            switch (error)
            {
                case RequestError.Success:
                    ms.WriteGzipBuffer(sendBuffer);

                    string msg = string.Format("[{0}]请求响应{1}:route={8},MsgId={2},St={3},Action-{4},error:{5}-{6},bytes:{7},响应时间:{9}ms\r\n",
                                               DateTime.Now.ToLongTimeString(),
                                               session.RemoteAddress,
                                               head.MsgId,
                                               head.St,
                                               head.Action,
                                               head.ErrorCode,
                                               head.ErrorInfo,
                                               sendBuffer.Length,
                                               routeName,
                                               (int)watch.Elapsed.TotalMilliseconds);
                    TraceLog.ReleaseWrite(msg);
#if DEBUG
#endif
                    Console.WriteLine(msg);

                    break;
                case RequestError.Closed:
                case RequestError.NotFindService:
                    head.ErrorInfo = ErrorNotFind;
                    DoWriteError(ms, head);
                    break;
                case RequestError.UnableConnect:
                    head.ErrorInfo = ErrorConnected;
                    DoWriteError(ms, head);
                    break;
                case RequestError.Timeout:
                    head.ErrorInfo = ErrorTimeout;
                    DoWriteError(ms, head);
                    break;
                case RequestError.Unknown:
                    DoWriteError(ms, head);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("RequestError", error, "Not process RequestError enum.");
            }
            sendBuffer = ms.ReadBuffer();
            return sendBuffer;
        }

        private RequestSettings ParseRequestSettings(ParamGeter paramGeter, string remoteAddress)
        {
            int gameId = 0;
            int serverId = 0;
            if (paramGeter.Contains("sid"))
            {
                string[] array = paramGeter.GetString("sid").Split('|');
                if (array.Length > 2)
                {
                    string sid = array[0];
                    gameId = array[1].ToInt();
                    serverId = array[2].ToInt();
                }
                else
                {
                    gameId = paramGeter.GetInt("gametype");
                    serverId = paramGeter.GetInt("serverid");

                }
            }
            return new RequestSettings(gameId, serverId, remoteAddress);
        }

        private MessageHead ParseMessageHead(ParamGeter paramGeter)
        {
            int msgId = paramGeter.GetInt("msgid");
            int actionId = paramGeter.GetInt("actionId");
            string St = "st";
            string st = paramGeter.GetString("st");
            if (!string.IsNullOrEmpty(st))
            {
                St = st;
            }
            return new MessageHead(msgId, actionId, St, 0);
        }

        private bool CheckCallAccessLimit(string remoteAddress)
        {
            string[] host = remoteAddress.Split(':');
            string ip = "";
            string network = "";
            if (host.Length > 0)
            {
                ip = host[0];
                int index = ip.LastIndexOf('.');
                if (index != -1)
                {
                    network = ip.Substring(0, index + 1) + "*";
                }
            }

            return !(_accessLimitIp.Contains(ip) ||
                     _accessLimitIp.Contains(network));
        }

        private void DoWriteError(MessageStructure ms, MessageHead head)
        {
            head.ErrorCode = (int)MessageError.SystemError;
            head.ClientVersion = 1;
            string msg = string.Format("Request action:{0} wcfserver error:{1}-{2},MsgId:{3}",
                                       head.Action, head.ErrorCode, head.ErrorInfo, head.MsgId);
            TraceLog.WriteError(msg);
#if DEBUG
            Console.WriteLine(msg);
#endif
            ms.WriteBuffer(head);
        }

    }
}