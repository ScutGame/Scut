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
    public class HttpServiceRequest : ServiceRequest
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

        protected override void ReadParam()
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

        protected override void WriteBuffer(byte[] buffer)
        {
            response.BinaryWrite(buffer);
        }

        protected override void WriteNotFindError()
        {
            DoWriteError(ErrorNotFind);
            TraceLog.WriteError("Unable to find connection gameid:{0} serverId:{1} error.", GameId, ServerId);
        }

        protected override void WriteConnectionError()
        {
            DoWriteError(ErrorConnected);
            TraceLog.WriteError("The connection to gameid:{0} serverId:{1} error.", GameId, ServerId);
        }

        protected override void WriteTimeoutError(TimeoutException timeout)
        {
            DoWriteError(ErrorTimeout);
            TraceLog.WriteError("The socket-server [gameid:{0} serverId:{1} timeout error:{2}", GameId, ServerId, timeout.ToString());
        }

        protected override void WriteUnknownError(Exception ex)
        {
            DoWriteError(ErrorUnknown);
            TraceLog.WriteError("The receive to gameid:{0} serverId:{1} error:{2}", GameId, ServerId, ex);
        }

        private void DoWriteError(string errorInfo)
        {
            var head = new MessageHead
            {
                ErrorCode = ErrorCode,
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
