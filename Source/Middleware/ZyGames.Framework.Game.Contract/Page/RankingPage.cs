using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Framework.Game.Contract.Page
{
    /// <summary>
    /// 排行榜页
    /// </summary>
    public abstract class RankingPage : System.Web.UI.Page
    {
        public RankingPage()
        {
            this.Load += new EventHandler(RankingPage_Load);
        }

        private void RankingPage_Load(object sender, EventArgs e)
        {
            try
            {
                string result = "FAILURE";
                int rankType = (Request["type"] ?? "0").ToInt();
                int top = (Request["top"] ?? "0").ToInt();
                string contentType = Request["contype"] ?? "html";

                if (rankType > 0)
                {
                    var dataList = DoRanking(rankType, top);
                    if (contentType.ToLower() == "json")
                    {
                        result = RankingToJson(dataList);
                    }
                    else
                    {
                        result = RankingToHtml(dataList);
                    }
                }
                Response.Write(result);
            }
            catch (Exception ex)
            {
                Response.Write("FAILURE");
                TraceLog.WriteError("RankingPage error:{0}", ex);
            }
        }

        protected IList<object> DoRanking(int rankType, int top)
        {
            List<object> list = new List<object>();
            var ranking = GetRankingObject(rankType);
            if (ranking != null)
            {
                int index = 0;
                var er = ranking.GetEnumerator();
                while (er.MoveNext())
                {
                    if (index >= top)
                    {
                        break;
                    }
                    list.Add(er.Current);
                    index++;
                }
            }
            return list;
        }

        protected abstract IRanking GetRankingObject(int rankType);

        protected abstract string RankingToJson(IList<object> dataList);

        protected abstract string RankingToHtml(IList<object> dataList);
    }
}
