/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Com.Rank;

namespace ZyGames.Framework.Game.Contract.Page
{
    /// <summary>
    /// 排行榜页
    /// </summary>
    public abstract class RankingPage : System.Web.UI.Page
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Contract.Page.RankingPage"/> class.
		/// </summary>
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
		/// <summary>
		/// Dos the ranking.
		/// </summary>
		/// <returns>The ranking.</returns>
		/// <param name="rankType">Rank type.</param>
		/// <param name="top">Top.</param>
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
		/// <summary>
		/// Gets the ranking object.
		/// </summary>
		/// <returns>The ranking object.</returns>
		/// <param name="rankType">Rank type.</param>
        protected abstract IRanking GetRankingObject(int rankType);
		/// <summary>
		/// Rankings to json.
		/// </summary>
		/// <returns>The to json.</returns>
		/// <param name="dataList">Data list.</param>
        protected abstract string RankingToJson(IList<object> dataList);
		/// <summary>
		/// Rankings to html.
		/// </summary>
		/// <returns>The to html.</returns>
		/// <param name="dataList">Data list.</param>
        protected abstract string RankingToHtml(IList<object> dataList);
    }
}