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
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1013_新手活动在线得精力领取接口
    /// </summary>
    public class Action1013 : BaseAction
    {
        private int _takeNum;
        private int _coldTime;
        private int ops = 0;

        public Action1013(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1013, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_takeNum);
            PushIntoStack(_coldTime);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            if (ops == 1)
            {
                short onlineEnergy;
                if (NoviceHelper.OnlinePrize(ContextUser, out _takeNum, out _coldTime, out onlineEnergy))
                {
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1013_JingliPrize, onlineEnergy);
                }
            }
            else if (ops == 2)
            {
                if (NoviceHelper.DailyEnergy(ContextUser.UserID))
                {
                    UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
                    if (dailyRestrain != null)
                    {
                        short dailyEnergyNum = (short)ConfigEnvSet.GetInt("DailyEnergyNum");

                        if (dailyRestrain.Funtion15.Date != DateTime.Now)
                        {
                            dailyRestrain.Funtion15 = DateTime.Now;
                            //dailyRestrain.Update();
                            ContextUser.SurplusEnergy = dailyEnergyNum;
                            //ContextUser.Update();
                            ErrorInfo = string.Format(LanguageManager.GetLang().St1013_DailyJingliPrize, dailyEnergyNum);
                        }

                    }
                }
            }
            else if (ops == 3)
            {
                int festivalEnergy = NoviceHelper.AugustSecondWeekEnergy(ContextUser);
                ErrorInfo = string.Format(LanguageManager.GetLang().st_FestivalInfoReward, LanguageManager.GetLang().St_AugustSecondWeek, string.Format(LanguageManager.GetLang().St_EnergyNum, festivalEnergy));
            }
            return true;
        }
    }
}