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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9202_公告列表接口
    /// </summary>
    public class Action9202 : BaseAction
    {
        private int _pageIndex;
        private int _pageSize;
        private int _pageCount;
        private List<GameNotice> _gameNotices;

        public Action9202(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9202, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(_pageCount);
            PushIntoStack(_gameNotices.Count);
            foreach (var item in _gameNotices)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.Title.ToNotNullString());
                dsItem.PushIntoStack(item.Content.ToNotNullString());
                dsItem.PushIntoStack(FormatDate(item.CreateDate));
                dsItem.PushIntoStack(item.NoticeType);
                PushIntoStack(dsItem);
            }
            PushIntoStack(IsToday(_gameNotices) ? 1 : 0);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref _pageIndex)
                 && httpGet.GetInt("PageSize", ref _pageSize))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            _gameNotices = new ShareCacheStruct<GameNotice>().FindAll(m => (m.ExpiryDate <= MathUtils.SqlMinDate && m.CreateDate.AddDays(7) >= DateTime.Now) ||
                (m.ExpiryDate > MathUtils.SqlMinDate && (m.ExpiryDate >= DateTime.Now && m.CreateDate.AddDays(7) >= DateTime.Now)));
            //_gameNotices = new ShareCacheStruct<GameNotice>().FindAll(s=>s.ExpiryDate >= DateTime.Now && s.CreateDate.AddDays(7) >= DateTime.Now);
            _gameNotices.QuickSort((x, y) =>
            {
                int result = y.IsTop.CompareTo(x.IsTop);
                if (result == 0)
                {
                    return y.CreateDate.CompareTo(x.CreateDate);
                }
                return result;
            });
            _gameNotices = _gameNotices.GetPaging(_pageIndex, _pageSize, out _pageCount);
            return true;
        }

        private static bool IsToday(List<GameNotice> noticesArray)
        {
            foreach (GameNotice notice in noticesArray)
            {
                if (notice.CreateDate.Date == DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }
    }
}