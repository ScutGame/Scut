using System;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Wcf;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpServiceRequest
    {
        private static bool EnableError;
        private static string ErrorNotFind;
        private static string ErrorConnected;
        private static string ErrorTimeout;
        private static string ErrorUnknown;

        static HttpServiceRequest()
        {
            EnableError = ConfigUtils.GetSetting("Enable.Error").ToBool();
            ErrorNotFind = ConfigUtils.GetSetting("Error.NotFind");
            ErrorConnected = ConfigUtils.GetSetting("Error.Connected");
            ErrorTimeout = ConfigUtils.GetSetting("Error.Timeout");
            ErrorUnknown = ConfigUtils.GetSetting("Error.Unknown");

        }
        private HttpGet httpGet;
        private HttpGameResponse response;
        private MessageStructure _buffer;
        private string ParamString;
        private string RemoteAddress;
        private int GameId;
        private int ServerId;
        private int ActionId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public HttpServiceRequest(HttpContext context)
        {
            _buffer = new MessageStructure();
            httpGet = new HttpGet(context.Request);
            response = new HttpGameResponse(context.Response);
            ParamString = httpGet.ParamString;
            RemoteAddress = httpGet.RemoteAddress;
        }

        public void Request()
        {
            ReadParam();
            RequestSettings settings = new RequestSettings(GameId, ServerId, RemoteAddress, ParamString);

            byte[] sendBuffer = new byte[0];
            RequestError error = ServiceRequest.Request(settings, out sendBuffer);
            switch (error)
            {
                case RequestError.Success:
                    WriteBuffer(sendBuffer);
                    break;
                case RequestError.NotFindService:
                    WriteNotFindError();
                    break;
                case RequestError.UnableConnect:
                    WriteConnectionError();
                    break;
                case RequestError.Timeout:
                    WriteTimeoutError();
                    break;
                case RequestError.Unknown:
                    WriteUnknownError();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void ReadParam()
        {
            if (!CheckGameServer())
            {
                GameId = httpGet.GetInt("gametype");
                ServerId = httpGet.GetInt("serverid");
                ServerId = ServerId > 0 ? ServerId : 1;
            }
            ActionId = httpGet.GetInt("actionId");
        }


        private bool CheckGameServer()
        {
            if (httpGet.Contains("sid"))
            {
                string[] array = httpGet.GetString("sid").Split('|');
                if (array.Length > 2)
                {
                    string sid = array[0];
                    GameId = array[1].ToInt();
                    ServerId = array[2].ToInt();
                    return true;
                }
            }
            return false;
        }

        protected void WriteBuffer(byte[] buffer)
        {
            response.BinaryWrite(buffer);
        }

        protected void WriteNotFindError()
        {
            DoWriteError(ErrorNotFind);
            TraceLog.WriteError("Unable to find connection gameid:{0} serverId:{1} error.", GameId, ServerId);
        }

        protected void WriteConnectionError()
        {
            DoWriteError(ErrorConnected);
            TraceLog.WriteError("The connection to gameid:{0} serverId:{1} error.", GameId, ServerId);
        }

        protected void WriteTimeoutError()
        {
            DoWriteError(ErrorTimeout);
            TraceLog.WriteError("The socket-server [gameid:{0} serverId:{1} timeout error.", GameId, ServerId);
        }

        protected void WriteUnknownError()
        {
            DoWriteError(ErrorUnknown);
            TraceLog.WriteError("The receive to gameid:{0} serverId:{1} error:{2}", GameId, ServerId);
        }

        private void DoWriteError(string errorInfo)
        {
            var head = new MessageHead
            {
                ErrorCode = (int)MessageError.SystemError,
                Action = ActionId
            };
            if (EnableError)
            {
                head.ErrorInfo = errorInfo;
            }
            _buffer.WriteBuffer(head);
            WriteBuffer(_buffer.ReadBuffer());
        }


    }
}
