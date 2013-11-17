using System;
using System.Collections.Generic;
using System.Web;
using ZyGames.Framework.RPC.IO;
using ZyGames.OA.BLL.Common;

namespace ZyGames.OA.BLL.Remote.Ddz
{
    /// <summary>
    /// 系统公告
    /// </summary>
    public class NoticeRemote : GameRemote
    {
        private string host = "ddz.36you.net";
        private int port = 9700;

        public NoticeRemote(HttpContext context)
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
                case "delete":
                    DoDelete();
                    break;
                default:
                    DoRemote();
                    break;
            }
        }

        private void DoDelete()
        {
            string route = "notice.remove";
            string param = string.Format("&NoticeID={0}", GetParam("NoticeID"));
            DoRequest(host, port, route, param);
        }

        private void DoQuery()
        {
            string route = "notice.inquiry";
            string param = string.Format("&pageIndex={0}&pageSize={1}", PageIndex, PageSize);
            DoRequest(host, port, route, param);
        }

        private void DoRemote()
        {
            string route = "notice.send";
            string param = string.Format("&NoticeID={0}&Title={1}&Content={2}&ExpiryDate={3}&IsTop={4}&IsBroadcast={5}&Creater={6}&CreateDate={7}&NoticeType={8}",
                GetParam("NoticeID"),
                GetParam("Title"),
                GetParam("Content"),
                GetParam("ExpiryDate"),
                GetParam("IsTop").Equals("on"),
                GetParam("IsBroadcast").Equals("on"),
                _currEmployeeID,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                1);
            DoRequest(host, port, route, param);
        }

        protected override void SuccessCallback(MessageStructure writer, MessageHead head)
        {
            int type = writer.ReadInt();
            if (type == 1)
            {
                int recordCount = writer.ReadInt();
                JsonObject jsonContainer = new JsonObject();
                List<JsonObject> jsonList = new List<JsonObject>();
                for (int i = 0; i < recordCount; i++)
                {
                    writer.RecordStart();
                    var item = new JsonObject();
                    item.Add("NoticeID", writer.ReadString());
                    item.Add("Title",  writer.ReadString());
                    item.Add("Content", writer.ReadString());
                    item.Add("IsBroadcast", writer.ReadInt());
                    item.Add("IsTop", writer.ReadInt());
                    item.Add("Creater", writer.ReadString());
                    item.Add("CreateDate", writer.ReadString());
                    item.Add("ExpiryDate", writer.ReadString());
                    jsonList.Add(item);
                    writer.RecordEnd();
                }
                jsonContainer.Add("total", recordCount);
                jsonContainer.Add("rows", jsonList.ToArray());
                WriteTableJson(jsonContainer);
            }
        }

    }
}
