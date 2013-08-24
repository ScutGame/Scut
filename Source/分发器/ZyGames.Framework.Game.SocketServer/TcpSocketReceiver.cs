using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        public byte[] Receive(SocketSession session, byte[] buffer)
        {
            BufferReader reader = new BufferReader(buffer);
            string routeName = "";
            string paramString;
            if (!reader.ReadPacketString(out paramString))
            {
                paramString = new MessageStructure(buffer).ReadEndAsString();
            }
            //todo trace
            TraceLog.ReleaseWrite("Tcp param:{0}", paramString);
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
#if DEBUG
            Console.WriteLine("Tcp param:MsgId:{0},ActionId:{1},ip:{2}", head.MsgId, head.Action, remoteAddress);
#endif
            var settings = ParseRequestSettings(paramGeter, remoteAddress);
            settings.ParamString = paramString;
            settings.RouteName = routeName;

            byte[] sendBuffer = new byte[0];
            RequestError error;
            if (isRoute)
            {
                if (CheckCallAccessLimit(remoteAddress))
                {
                    error = RequestError.Unknown;
                    head.ErrorInfo = ErrorCallAccessLimit;
                }
                else
                {
                    error = ServiceRequest.CallRemote(settings, out sendBuffer);
                }
            }
            else
            {
                error = ServiceRequest.Request(settings, out sendBuffer);
            }
            switch (error)
            {
                case RequestError.Success:
                    string msg = string.Format("[{0}]请求响应{1}:route={8},MsgId={2},St={3},Action-{4},error:{5}-{6},bytes:{7}",
                                               DateTime.Now.ToString("HH:mm:ss"),
                                               session.RemoteAddress,
                                               head.MsgId,
                                               head.St,
                                               head.Action,
                                               head.ErrorCode,
                                               head.ErrorInfo,
                                               head.TotalLength + 4,
                                               routeName);
                    TraceLog.ReleaseWrite(msg);
                    Console.WriteLine(msg);
                    break;
                case RequestError.NotFindService:
                    head.ErrorInfo = ErrorNotFind;
                    sendBuffer = DoWriteError(head);
                    break;
                case RequestError.UnableConnect:
                    head.ErrorInfo = ErrorConnected;
                    sendBuffer = DoWriteError(head);
                    break;
                case RequestError.Timeout:
                    head.ErrorInfo = ErrorTimeout;
                    sendBuffer = DoWriteError(head);
                    break;
                case RequestError.Unknown:
                    sendBuffer = DoWriteError(head);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //增加一层包大小
            MessageStructure ds = new MessageStructure();
            ds.WriteByte(sendBuffer.Length);//用于Gig压缩后的包大小
            ds.WriteByte(sendBuffer);
            sendBuffer = ds.ReadBuffer();
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

        private byte[] DoWriteError(MessageHead head)
        {
            head.ErrorCode = (int)MessageError.SystemError;
            head.ClientVersion = 1;
            TraceLog.WriteError("Request wcf server error:{0}", head.ErrorInfo);
            var ms = new MessageStructure();
            ms.WriteBuffer(head);
            return ms.ReadBuffer();
        }

    }
}