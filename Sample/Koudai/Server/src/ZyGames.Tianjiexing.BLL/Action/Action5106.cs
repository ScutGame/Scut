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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5106_排名奖励领取接口
    /// </summary>
    public class Action5106 : BaseAction
    {
        public Action5106(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5106, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
            if (RankingHelper.IsGainSportsReward(ContextUser.UserID) && dailyRestrain != null && dailyRestrain.UserExtend != null)
            {
                var envSet = ServerEnvSet.Get(ServerEnvKey.JingJiChangReward, "");
                SportsRewardInfo sportsInfo = new ConfigCacheSet<SportsRewardInfo>().FindKey(dailyRestrain.UserExtend.UserRankID);
                {                if (envSet != null && sportsInfo != null)

                    if (envSet.ToDateTime().Date > dailyRestrain.Funtion11.Date && dailyRestrain.UserExtend.UserRankID > 0)
                    {

                        dailyRestrain.Funtion11 = DateTime.Now;
                        dailyRestrain.UserExtend.UserRankID = 0;
                        //dailyRestrain.Update();

                        ContextUser.ObtainNum = MathUtils.Addition(ContextUser.ObtainNum, sportsInfo.RewardObtian, int.MaxValue);
                        ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, sportsInfo.RewardGoin, int.MaxValue);
                        //ContextUser.Update();
                        UserHelper.Contribution(ContextUser.UserID, sportsInfo.RewardObtian);
                        ErrorInfo = string.Format(LanguageManager.GetLang().St5106_JingJiChangRankReward, sportsInfo.RewardObtian, sportsInfo.RewardGoin);
                    }
                }
            }
            return true;
        }
    }
}