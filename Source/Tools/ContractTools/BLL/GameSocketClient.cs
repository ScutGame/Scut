using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using BLL;
using ZyGames.Common;
using ZyGames.Core.Data;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.OA.BLL.Remote;

namespace BLL
{
    /// <summary>
    /// 游戏服远端通信服务基类
    /// </summary>
    public abstract class GameSocketClient
    {
        protected int _currEmployeeID;
      
        protected HttpContext _context;

        private ClientSocket client;

        protected GameSocketClient()
        {
            _context = HttpContext.Current;

            
        }

        public MessageStructure result { get; set; }
        public MessageHead _head { get; set; }

        public int GameID { get { return GetParamAsInt("GameID"); } }

        public int ServerID { get { return GetParamAsInt("ServerID"); } }

        public int PageIndex
        {
            get
            {
                int pageIndex = GetParamAsInt("page");
                return pageIndex < 1 ? 1 : pageIndex;
            }
        }

        public int PageSize
        {
            get
            {
                int pageSize = GetParamAsInt("rows");
                return pageSize <= 0 ? 20 : pageSize;
            }
        }


        public string GetParam(string name)
        {
            return _context.Request[name] ?? "";
        }

        public DateTime GetParamAsDate(string name)
        {
            return ConvertHelper.ToDateTime(GetParam(name), ConvertHelper.SqlMinDate);
        }

        public int GetParamAsInt(string name)
        {
            return ConvertHelper.ToInt(GetParam(name));
        }

        public bool GetParamAsBool(string name)
        {
            return ConvertHelper.ToBool(GetParam(name));
        }

        public abstract void Request(string action);

        public void AppendPageParam(List<SqlParameter> parameters, int pageIndex, int pageSize)
        {
            int statIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            parameters.Add(SqlParamHelper.MakeInParam("@statIndex", SqlDbType.Int, 0, statIndex));
            parameters.Add(SqlParamHelper.MakeInParam("@endIndex", SqlDbType.Int, 0, endIndex));
        }

        protected string GetRemoteParam(int gameId, int serverId, string param)
        {
            return string.Format("MsgId={0}&Sid={1}&Uid={2}&ActionId={3}&GameType={4}&ServerID={5}{6}",
                1,
                "",
                0,
                1000,
                gameId,
                serverId,
                param);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">远端主机地址</param>
        /// <param name="port"></param>
        /// <param name="route">远端执行方法名</param>
        /// <param name="param">参数</param>
        protected void DoRequest(string server, string param)
        {
            string[] serverArray = server.Split(':');
           
            param += string.Format("&sign={0}", GetSign(param));
            param = HttpUtility.UrlEncode(param);
            DoRequest(serverArray[0], Convert.ToInt32(serverArray[1]), param);
        }
        protected void DoRequest(string host, int port, string param)
        {
            var remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            DoRequest(remoteEndPoint, param, 4096);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        /// <param name="route">远端执行方法名</param>
        /// <param name="param">参数</param>
        /// <param name="bufferSize"></param>
        protected void DoRequest(IPEndPoint remoteEndPoint, string param, int bufferSize)
        {
            client = new ClientSocket(new ClientSocketSettings(1024, remoteEndPoint));
            client.Disconnected += DoDisconnected;
            client.DataReceived += DoReceive;
            client.Connect();
            byte[] data = Encoding.UTF8.GetBytes("?d="+param);

            client.PostSend(data, 0, data.Length);
            if (!client.WaitAll(10000))
            {
                DoError("请求超时");
            }
        }
        private static string GetSign(string param)
        {
            string attachParam = param + "44CAC8ED53714BF18D60C5C7B6296000";
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(attachParam, "MD5") ?? "";
            return sign.ToLower();
        }

        private void DoDisconnected(object sender, SocketEventArgs e)
        {
            DoError("服务器已关闭");
        }
        private void DoReceive(object sender, SocketEventArgs e)
        {
            try
            {
               
                MessageReader writer = new MessageReader(e.Data);
                var head = writer.ReadHead();
                if (head.Success)
                {
                    SuccessCallback(writer, head);
                }
                else
                {
                    FailCallback(head);
                }
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
            finally
            {
                if (client != null)
                    client.StopWait();
            }
        }
       
        protected virtual void DoError(string msg)
        {
            new BaseLog().SaveLog(new Exception(msg));
            _context.Response.Write(new JsonObject().Add("state", false).Add("message", msg).ToJson());
        }

        protected virtual void FailCallback(MessageHead head)
        {
            string msg = string.Format("出错:{0}-{1}", head.ErrorCode, head.ErrorInfo);
            _context.Response.Write(new JsonObject().Add("state", false).Add("message", msg).ToJson());
        }
        protected virtual void FailCallback(Message head)
        {
            string msg = string.Format("出错:{0}-{1}", head.ErrorCode, head.ErrorInfo);
            _context.Response.Write(new JsonObject().Add("state", false).Add("message", msg).ToJson());
        }

       
        protected virtual void SuccessCallback(MessageReader writer, Message head)
        {
            
        }
        protected void WriteTableJson(JsonObject obj)
        {
            _context.Response.Write(obj.ToJson());
        }

        protected void WriteTableJson(int count, string rows)
        {
            string jsonData = string.Format("\"total\":{0},\"rows\":{1}", count, rows);
            _context.Response.Write("{" + jsonData + "}");
        }
    }
}
