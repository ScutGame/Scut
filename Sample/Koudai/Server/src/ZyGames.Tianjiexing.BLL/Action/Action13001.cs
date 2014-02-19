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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 13001_新手礼包列表接口
    /// </summary>
    public class Action13001 : BaseAction
    {
        private List<NoviceInfo> _noviceList = new List<NoviceInfo>();

        public Action13001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action13001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_noviceList.Count);
            foreach (NoviceInfo item in _noviceList)
            {
                NoviceActivities noviceActivities = new ConfigCacheSet<NoviceActivities>().FindKey(item.ID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.ID);
                dsItem.PushIntoStack(noviceActivities == null ? LanguageManager.GetLang().shortInt : noviceActivities.ActivitiesType);
                dsItem.PushIntoStack(noviceActivities == null ? string.Empty : noviceActivities.Description);
                this.PushIntoStack(dsItem);
            }
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
            var noviceActivitiesArray = new ConfigCacheSet<NoviceActivities>().FindAll();
            foreach (NoviceActivities novice in noviceActivitiesArray)
            {
                _noviceList = NoviceHelper.GetNoviceList(ContextUser.UserID, novice.ID);
            }
            return true;
        }
    }
}