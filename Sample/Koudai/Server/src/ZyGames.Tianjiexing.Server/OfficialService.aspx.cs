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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Contract.Page;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.Service
{
    public partial class OfficialService : RankingPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override IRanking GetRankingObject(int rankType)
        {
            switch (rankType)
            {
                case 1:
                    return RankingFactory.Get<UserRank>(UserLvRanking.RankingKey);
                case 2:
                    return RankingFactory.Get<UserRank>(CombatRanking.RankingKey);
                default:
                    throw new NotImplementedException();
            }
        }

        protected override string RankingToJson(IList<object> dataList)
        {
            List<MyClass> myClassList = new List<MyClass>();
            int index = 1;
            foreach (var item in dataList)
            {
                MyClass myClass = new MyClass();
                if (item is UserRank)
                {
                    var user = (UserRank)item;
                    myClass.RankNo = index.ToString();
                    myClass.RankName = user.NickName;
                    myClass.GradeNum = user.UserLv;
                    myClassList.Add(myClass);
                }
               
                index++;
            }
            return JsonUtils.Serialize(myClassList);
        }

        protected override string RankingToHtml(IList<object> dataList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<ul>");
            int index = 1;
            foreach (var item in dataList)
            {
                if (item is UserRank)
                {
                    var user = (UserRank)item;
                    stringBuilder.Append(GetHtml(index.ToString(), user.NickName));

                }
               
                index++;
            }
            stringBuilder.Append("</ul>");
            return stringBuilder.ToString();
        }

        private string GetHtml(string rankNo, string name)
        {
            return "<li><span class=\"mc\">" + rankNo + "</span><span class=\"dj\">" + name + "</span></li>";
        }
        private class MyClass
        {
            public String RankNo { get; set; }
            public String RankName { get; set; }
            public Int32 GradeNum { get; set; }
        }
    }
}