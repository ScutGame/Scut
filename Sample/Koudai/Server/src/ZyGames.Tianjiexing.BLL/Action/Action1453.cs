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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1453_法宝界面接口
    /// </summary>
    public class Action1453 : BaseAction
    {
        private short currLife;
        private short maxLift;
        private int currExp;
        private int maxExp;


        public Action1453(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1453, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack((short)currLife);
            this.PushIntoStack((short)maxLift);
            this.PushIntoStack(currExp);
            this.PushIntoStack(maxExp);

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
            UserTrump trump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (trump != null)
            {
                currLife = trump.LiftNum.ToShort();
                currExp = trump.Experience;
                short uplv = MathUtils.Addition(trump.TrumpLv, (short)1, (short)GameConfigSet.MaxTrumpLv);
                TrumpInfo trumpInfo = new ConfigCacheSet<TrumpInfo>().FindKey(TrumpInfo.CurrTrumpID, uplv);
                if (trumpInfo != null)
                {
                    maxLift = trumpInfo.MaxLift;
                    maxExp = trumpInfo.Experience;
                }
            }
            return true;
        }
    }
}