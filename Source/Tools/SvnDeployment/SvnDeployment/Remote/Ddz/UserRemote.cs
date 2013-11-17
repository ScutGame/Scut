using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using ZyGames.Core.Data;
using ZyGames.Framework.RPC.IO;
using ZyGames.OA.BLL.Model;

namespace ZyGames.OA.BLL.Remote.Ddz
{
    /// <summary>
    /// 玩家
    /// </summary>
    public class UserRemote : GameRemote
    {
        public UserRemote(HttpContext context)
            : base(context)
        {
        }

        public override void Request(string action)
        {
            action = action.ToLower();
            switch (action)
            {
                case "query":
                    DoQuery();
                    break;
                default:
                    DoRemote(action);
                    break;
            }
        }


        private void DoQuery()
        {
            List<SqlParameter> paramValues = new List<SqlParameter>();
            string condition = "Where 1=1";
            JsonTable jsonTable = new JsonTable();
            if (GetParam("NickName").Length > 0)
            {
                condition += " and NickName like @NickName";
                paramValues.Add(SqlParamHelper.MakeInParam("@NickName", SqlDbType.VarChar, 0, "%" + GetParam("NickName") + "%"));
            }
            if (GetParam("Pid").Length > 0)
            {
                condition += " and Pid=@Pid";
                paramValues.Add(SqlParamHelper.MakeInParam("@Pid", SqlDbType.VarChar, 0, GetParam("Pid")));
            }
            if (GetParam("UserStatus").Length > 0)
            {
                condition += " and UserStatus=@UserStatus";
                paramValues.Add(SqlParamHelper.MakeInParam("@UserStatus", SqlDbType.VarChar, 0, GetParam("UserStatus")));
            }
            if (GetParam("MsgState").Length > 0)
            {
                condition += " and MsgState=@MsgState";
                paramValues.Add(SqlParamHelper.MakeInParam("@MsgState", SqlDbType.VarChar, 0, GetParam("MsgState")));
            }
            if (GetParam("fromDate").Length > 0)
            {
                condition += " and CreateDate>=@fromDate";
                paramValues.Add(SqlParamHelper.MakeInParam("@fromDate", SqlDbType.VarChar, 0, GetParam("fromDate")));
            }
            if (GetParam("toDate").Length > 0)
            {
                condition += " and CreateDate<=@toDate";
                paramValues.Add(SqlParamHelper.MakeInParam("@toDate", SqlDbType.VarChar, 0, GetParam("toDate")));
            }
            if (IsChannel)
            {
                condition += " and RetailID=@ChannelID";
                paramValues.Add(SqlParamHelper.MakeInParam("@ChannelID", SqlDbType.VarChar, 0, EmpRetailId));

            }

            var service = new DdzDataService(GameID, ServerID);
            AppendPageParam(paramValues, PageIndex, PageSize);
            var getter = service.Get<UserGetter>(condition, PageIndex, PageSize, paramValues);

            jsonTable.rows = getter.GetData();
            jsonTable.total = getter.RecordCount;

            _context.Response.Write(jsonTable.ToJson());
        }

        private void DoRemote(string action)
        {
            string host = "ddz.36you.net";
            int port = 9700;
            string route = "user.setStatus";
            string param = string.Format("&userID={0}&op={1}&EndDate={2}&OpUserID={3}&Reason={4}",
                GetParamAsInt("gameUserID"),
                action,
                GetParam("EndDate"),
                _currEmployeeID,
                GetParam("Reason"));
            DoRequest(host, port, route, param);
        }

        protected override void SuccessCallback(MessageStructure writer, MessageHead head)
        {
        }


    }
}
