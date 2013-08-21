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

namespace BLL
{
    /// <summary>
    /// 游戏服远端通信服务基类
    /// </summary>
    public abstract class GameSocketClient
    {
        protected int _currEmployeeID;
      
        protected HttpContext _context;



        protected GameSocketClient()
        {
            _context = HttpContext.Current;

            
        }

      

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

        protected void DoRequest(string host, int port, string route, string param)
        {
            DoRequest(host, port, route, param, 4096);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">远端主机地址</param>
        /// <param name="port"></param>
        /// <param name="route">远端执行方法名</param>
        /// <param name="param">参数</param>
        protected void DoRequest(string host, int port, string route, string param, int bufferSize)
        {
            using (SocketClient client = new SocketClient(host, port, bufferSize))
            {
                client.ErrorHandle += DoError;
                client.ReceiveHandle += DoReceive;

                if (client.Connect())
                {

                    param = GetRemoteParam(GameID, ServerID, param);
                    param = param + "&sign=" + GetSign(param);
                    param = HttpUtility.UrlEncode(param, Encoding.UTF8);
                    byte[] data = Encoding.UTF8.GetBytes(string.Format("{0}", param));
                    data = BufferUtils.MergeBytes(BufferUtils.GetSocketBytes(data.Length), data);
                    client.Send(data);
                    client.ReceiveResult();
                }
            }
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
            using (SocketClient client = new SocketClient(serverArray[0], Convert.ToInt32(serverArray[1]), 4096))
            {
                client.ErrorHandle += DoError;
                client.ReceiveHandle += DoReceive;

                if (client.Connect())
                {

                    //param = GetRemoteParam(GameID, ServerID, param);
                    param = param + "&sign=" + GetSign(param);
                    param = HttpUtility.UrlEncode(param, Encoding.UTF8);
                    byte[] data = Encoding.UTF8.GetBytes(param);
                    data = BufferUtils.MergeBytes(BufferUtils.GetSocketBytes(data.Length), data);
                    client.Send(data);
                    client.ReceiveResult();
                }
            }
        }
        private static string GetSign(string param)
        {
            string attachParam = param + "44CAC8ED53714BF18D60C5C7B6296000";
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(attachParam, "MD5") ?? "";
            return sign.ToLower();
        }
        private void DoReceive(byte[] data)
        {
            try
            {
                MessageStructure writer = new MessageStructure(data);
                var head = writer.ReadHeadGzip();
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

        protected virtual void SuccessCallback(MessageStructure writer, MessageHead head)
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
