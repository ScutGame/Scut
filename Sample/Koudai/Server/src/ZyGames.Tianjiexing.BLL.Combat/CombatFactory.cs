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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    public static class CombatFactory
    {
        /// <summary>
        /// 副本战役
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plotNpcID"></param>
        /// <returns></returns>
        public static ISingleCombat TriggerPlot(GameUser user, int plotNpcID)
        {
            //TrumpAbilityAttack.CombatTrumpLift(user.UserID); //法宝每战斗M次就扣除N点寿命
            ICombatController controller = new TjxCombatController();
            ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.Plot);
            plotCombater.SetAttack(new UserEmbattleQueue(user.UserID, user.UseMagicID));
            plotCombater.SetDefend(new MonsterQueue(plotNpcID));
            return plotCombater;
        }

        /// <summary>
        /// 圣吉塔战役
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plotNpcID"></param>
        /// <returns></returns>
        public static ISingleCombat TriggerSJTPlot(GameUser user, int plotNpcID, double difficultNum)
        {
            ICombatController controller = new TjxCombatController();
            ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.ShengJiTa);
            plotCombater.SetAttack(new UserEmbattleQueue(user.UserID, user.UseMagicID));
            plotCombater.SetDefend(new MonsterQueue(plotNpcID, difficultNum));
            return plotCombater;
        }
        /// <summary>
        /// 考古战役
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plotNpcID"></param>
        /// <returns></returns>
        public static ISingleCombat TriggerArchaeologyPlot(GameUser user, int plotNpcID)
        {
            ICombatController controller = new TjxCombatController();
            ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.KaoGuPlot);
            plotCombater.SetAttack(new UserEmbattleQueue(user.UserID, user.UseMagicID));
            plotCombater.SetDefend(new MonsterQueue(plotNpcID));
            return plotCombater;
        }

        /// <summary>
        /// 国家战
        /// </summary>
        /// <param name="cuser1">发起人</param>
        /// <param name="cuser2"></param>
        /// <returns></returns>
        public static ISingleCombat TriggerTournament(CountryUser cuser1, CountryUser cuser2)
        {
            TrumpAbilityAttack.CombatTrumpLift(cuser1.UserId); //法宝每战斗M次就扣除N点寿命
            ICombatController controller =  new TjxCombatController();
            ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.Country);

            plotCombater.SetAttack(new UserEmbattleQueue(cuser1.UserId, GetMagicId(cuser1.UserId), cuser1.InspirePercent, CombatType.Country));
            plotCombater.SetDefend(new UserEmbattleQueue(cuser2.UserId, GetMagicId(cuser2.UserId), cuser2.InspirePercent, CombatType.Country));
            return plotCombater;
        }
        private static int GetMagicId(string userId)
        {
            var user = new GameDataCacheSet<GameUser>().FindKey(userId);
            if (user != null)
            {
                return user.UseMagicID;
            }
            return 0;
        }

        /// <summary>
        /// 玩家竞技
        /// </summary>
        /// <param name="user1">发起人</param>
        /// <param name="user2"></param>
        /// <returns></returns>
        public static ISingleCombat TriggerTournament(GameUser user1, GameUser user2)
        {
            //TrumpAbilityAttack.CombatTrumpLift(user1.UserID); //法宝每战斗M次就扣除N点寿命
            ICombatController controller =  new TjxCombatController();
            ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.User);
            user1 = UserCacheGlobal.LoadOffline(user1.UserID);
            user2 = UserCacheGlobal.LoadOffline(user2.UserID);

            if (user1 != null && user2 != null)
            {
                plotCombater.SetAttack(new UserEmbattleQueue(user1.UserID, user1.UseMagicID, 0, CombatType.User));
                plotCombater.SetDefend(new UserEmbattleQueue(user2.UserID, user2.UseMagicID, 0, CombatType.User));
            }
            return plotCombater;
        }
    }
}