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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3011_限时活动详情接口
    /// </summary>
    public class Action3011 : BaseAction
    {
        private int festivalID;
        private string beginDate;
        private string endDate;
        private FestivalInfo festivalInfo = null;

        public Action3011(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3011, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(festivalInfo == null ? string.Empty : festivalInfo.FestivalName.ToNotNullString());
            PushIntoStack(beginDate);
            PushIntoStack(endDate);
            PushIntoStack(festivalInfo == null ? string.Empty : festivalInfo.FestivalDesc.ToNotNullString());
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("FestivalID", ref festivalID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            festivalInfo = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festivalInfo != null)
            {
                string shortDateFormat = LanguageManager.GetLang().FestivalDataFormat;
                beginDate = festivalInfo.StartDate.ToString(shortDateFormat);//yyyy-MM-dd HH:mm:ss;ms
               // beginDate = string.Format("{0:M}", festivalInfo.StartDate);
                if (festivalInfo.EndDate != MathUtils.SqlMinDate)
                {
                   // endDate = string.Format("{0:M}", festivalInfo.EndDate);
                    endDate = festivalInfo.EndDate.ToString(shortDateFormat);
                }
                else
                {
                   // endDate = string.Format("{0:M}", festivalInfo.StartDate.AddHours(festivalInfo.ContinuedTime));
                    endDate = festivalInfo.StartDate.AddHours(festivalInfo.ContinuedTime).ToString(shortDateFormat);
                }
            }
            return true;
        }
    }
}