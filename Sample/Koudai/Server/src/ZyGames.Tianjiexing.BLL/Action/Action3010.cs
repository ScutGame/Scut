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
using System.Collections.Generic;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3010_限时活动列表接口
    /// </summary>
    public class Action3010 : BaseAction
    {
        private string beginDate;
        private string endDate;
        private List<FestivalInfo> festivalList = new List<FestivalInfo>();
        private FestivalInfo festivalInfo = null;

        public Action3010(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3010, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(festivalList.Count);
            foreach (FestivalInfo item in festivalList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.FestivalID);
                dsItem.PushIntoStack(item.FestivalName.ToNotNullString());
                PushIntoStack(dsItem);
            }
            PushIntoStack(festivalInfo == null ? 0 : festivalInfo.FestivalID);
            PushIntoStack(festivalInfo == null ? string.Empty : festivalInfo.FestivalName.ToNotNullString());
            PushIntoStack(beginDate.ToNotNullString());
            PushIntoStack(endDate.ToNotNullString());
            PushIntoStack(festivalInfo == null ? string.Empty : festivalInfo.FestivalDesc.ToNotNullString());
        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                return true;
            }
        }

        public override bool TakeAction()
        {
            festivalList = NoviceHelper.LimitFestivalList();
            if (festivalList.Count > 0)
            {
                string shortDateFormat = LanguageManager.GetLang().FestivalDataFormat;
                festivalInfo = festivalList[0];
                beginDate = festivalInfo.StartDate.ToString(shortDateFormat);
                if (festivalInfo.EndDate != MathUtils.SqlMinDate)
                {
                    endDate = festivalInfo.EndDate.ToString(shortDateFormat);
                }
                else
                {
                    endDate = festivalInfo.StartDate.AddHours(festivalInfo.ContinuedTime).ToString(shortDateFormat);
                }
            }
            festivalList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.FestivalID.CompareTo(y.FestivalID);
            });
            return true;
        }
    }
}