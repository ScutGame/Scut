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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1470_法宝属相接口
    /// </summary>
    public class Action1470 : BaseAction
    {
        private short zodiac;
        private short isRestraint;
        private Dictionary<short, decimal> zodiacList = new Dictionary<short, decimal>();

        public Action1470(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1470, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(zodiac);
            this.PushIntoStack(isRestraint);
            this.PushIntoStack(zodiacList.Count);
            foreach (var item in zodiacList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.Key);
                dsItem.PushIntoStack(item.Value.ToDecimal().ToNotNullString());
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
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                zodiac = (short)userTrump.Zodiac;
                zodiacList = TrumpHelper.ZodiacRestraint(userTrump.Zodiac, 1);
                Dictionary<short, decimal> restraintList = TrumpHelper.ZodiacRestraint(userTrump.Zodiac, 2);
                foreach (KeyValuePair<short, decimal> keyValuePair in restraintList)
                {
                    isRestraint = keyValuePair.Value.ToShort();
                    break;
                }
            }
            return true;
        }
    }
}