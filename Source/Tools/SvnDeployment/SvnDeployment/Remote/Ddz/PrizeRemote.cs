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
    /// 奖励
    /// </summary>
    public class PrizeRemote : GameRemote
    {
        public PrizeRemote(HttpContext context)
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
            if (GetParam("GameUserID").Length > 0)
            {
                condition += " and UserID=@UserID";
                paramValues.Add(SqlParamHelper.MakeInParam("@UserID", SqlDbType.VarChar, 0, GetParam("GameUserID")));
            }
            if (GetParam("IsTasked").Length > 0)
            {
                condition += " and IsTasked=@IsTasked";
                paramValues.Add(SqlParamHelper.MakeInParam("@IsTasked", SqlDbType.Bit, 0, GetParamAsInt("IsTasked")));
            }
            if (GetParam("FromDate").Length > 0)
            {
                condition += " and CreateDate>=@fromDate";
                paramValues.Add(SqlParamHelper.MakeInParam("@fromDate", SqlDbType.VarChar, 0, GetParam("FromDate")));
            }
            if (GetParam("ToDate").Length > 0)
            {
                condition += " and CreateDate<=@toDate";
                paramValues.Add(SqlParamHelper.MakeInParam("@toDate", SqlDbType.VarChar, 0, GetParam("ToDate")));
            }

            var service = new DdzDataService(GameID, ServerID);
            AppendPageParam(paramValues, PageIndex, PageSize);
            var getter = service.Get<PrizeGetter>(condition, PageIndex, PageSize, paramValues);

            jsonTable.rows = getter.GetData();
            jsonTable.total = getter.RecordCount;

            _context.Response.Write(jsonTable.ToJson());
        }

        private void DoRemote(string action)
        {
            string host = "ddz.36you.net";
            int port = 9700;
            string route = "prize.send";
            string param = string.Format("&UserID={0}&op={1}&OpUserID={2}&Gold={3}&GameCoin={4}&GoodItem={5}&MailContent={6}",
                GetParam("UserIDList"),
                action,
                _currEmployeeID,
                GetParamAsInt("Gold"),
                GetParamAsInt("GameCoin"),
                GetParam("ItemPackage"),
                GetParam("MailContent"));
            DoRequest(host, port, route, param);
        }

        protected override void SuccessCallback(MessageStructure writer, MessageHead head)
        {
        }
    }
}
