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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3202_宠物配置下发接口
    /// </summary>
    public class Action3202 : BaseAction
    {
        private List<PetInfo> petList = new List<PetInfo>();
        private decimal minusNum = 0;

        public Action3202(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3202, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(petList.Count);
            foreach (var item in petList)
            {
                int gameCoin = (int)Math.Floor(item.CoinRate * ContextUser.UserLv * minusNum);
                int obtainNum = (int)Math.Floor(item.ObtainRate * ContextUser.UserLv * minusNum);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.PetId);
                dsItem.PushIntoStack(item.PetName);
                dsItem.PushIntoStack(item.PetHead);
                dsItem.PushIntoStack(item.ColdTime);
                dsItem.PushIntoStack(gameCoin);
                dsItem.PushIntoStack(obtainNum);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            minusNum = (decimal)FestivalHelper.TortoiseHare(ContextUser.UserID);
            petList = new ConfigCacheSet<PetInfo>().FindAll();
            return true;
        }
    }
}