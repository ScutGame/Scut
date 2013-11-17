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
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 世界Boss战役
    /// </summary>
    public class BossCombat
    {
        class BossDictionary
        {
            public BossDictionary()
            {
                UserGeneralList = new DictionaryExtend<string, BossUser>();
            }
            public CombatGeneral BossGeneral { get; set; }
            public DictionaryExtend<string, BossUser> UserGeneralList { get; set; }

            public void Exit(string userId)
            {
                if (UserGeneralList.ContainsKey(userId))
                {
                    UserGeneralList.Remove(userId);
                }
            }

            public void Clear()
            {
                BossGeneral = null;
                UserGeneralList.Clear();
            }
        }
        private static readonly object thisLock = new object();
        private static DictionaryExtend<int, GameActive> _gameActiveList = new DictionaryExtend<int, GameActive>();
        private static DictionaryExtend<int, BossDictionary> _bossGeneralList = new DictionaryExtend<int, BossDictionary>();
        private int _activeId;

        /// <summary>
        /// 加载数据
        /// </summary>
        public static void InitBoss(GameActive gameActive)
        {
            if (_gameActiveList.ContainsKey(gameActive.ActiveId))
            {
                _gameActiveList[gameActive.ActiveId] = gameActive;
            }
            else
            {
                _gameActiveList.Add(gameActive.ActiveId, gameActive);
            }

            CombatGeneral general = CreateBossGeneral(gameActive);
            if (!_bossGeneralList.ContainsKey(gameActive.ActiveId))
            {
                _bossGeneralList.Add(gameActive.ActiveId, new BossDictionary { BossGeneral = general });
            }
            else
            {
                _bossGeneralList[gameActive.ActiveId].BossGeneral = general;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="gameActive"></param>
        public static void Dispose(GameActive gameActive)
        {
            if (_bossGeneralList.ContainsKey(gameActive.ActiveId))
            {
                _bossGeneralList[gameActive.ActiveId].Clear();
            }
        }

        public BossCombat(int activeId)
        {
            _activeId = activeId;
        }

        /// <summary>
        /// 初始化BOSS数据
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        private static CombatGeneral CreateBossGeneral(GameActive active)
        {
            CombatGeneral boss = null;
            if (active != null)
            {
                var plotNpcInfoList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == active.BossPlotID);
                if (plotNpcInfoList.Count > 0)
                {
                    var embattleList = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.PlotNpcID == plotNpcInfoList[0].PlotNpcID);
                    if (embattleList.Count > 0)
                    {
                        int npcId = plotNpcInfoList[0].PlotNpcID;
                        boss = (CombatGeneral)MonsterQueue.Create(embattleList[0]);
                        boss.Lv = (short)MathUtils.Subtraction(active.BossLv, 0, active.BossDefLv);
                        int bossLiftNum = MonsterQueue.BossProperty(embattleList[0].PlotNpcID, AbilityType.ShengMing).ToInt();
                        int lv = (boss.Lv - active.BossDefLv);
                        int lifeNum = boss.LifeNum + lv * bossLiftNum; //ConfigEnvSet.GetInt("BossCombat.IncreaseLiveNum");
                        boss.LifeMaxNum = lifeNum;
                        boss.LifeNum = lifeNum;
                        if (lv > 0)
                        {
                            boss.PowerNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.PowerNum)).ToInt(), boss.PowerNum).ToInt();
                            boss.SoulNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.SoulNum).ToInt()), boss.SoulNum).ToInt();
                            boss.IntellectNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.IntelligenceNum).ToInt()), boss.IntellectNum).ToInt();
                            boss.PhyAttackNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.WuLiGongJi).ToInt()), boss.PhyAttackNum);
                            boss.AbiAttackNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.HunJiGongJi).ToInt()), boss.AbiAttackNum);
                            boss.MagAttackNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.MoFaGongJi).ToInt()), boss.MagAttackNum);
                            boss.PhyDefenseNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.WuLiFangYu).ToInt()), boss.PhyDefenseNum);
                            boss.AbiDefenseNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.HunJiFangYu).ToInt()), boss.AbiDefenseNum);
                            boss.MagDefenseNum = MathUtils.Addition((lv * MonsterQueue.BossProperty(npcId, AbilityType.MoFaFangYu).ToInt()), boss.MagDefenseNum);
                        }
                        TraceLog.WriteComplement("世界BOSS属性值---血量:{0}、上限血量:{1}、力量:{2}、魂力:{3}、智力:{4}、物理攻击:{5}、魂技攻击:{6}、魔法攻击：{7}、物理防御：{8}、魂技防御：{9}、魔法防御：{10}", boss.LifeNum, boss.LifeMaxNum, boss.PowerNum, boss.SoulNum, boss.IntellectNum, (boss.ExtraAttack.WuliNum + boss.PhyAttackNum), (boss.ExtraAttack.HunjiNum + boss.AbiAttackNum), (boss.ExtraAttack.MofaNum + boss.MagAttackNum), (boss.ExtraDefense.WuliNum + boss.PhyDefenseNum), (boss.ExtraDefense.HunjiNum + boss.AbiDefenseNum), (boss.ExtraDefense.MofaNum + boss.MagDefenseNum));
                    }
                }
                else
                {
                    throw new Exception(string.Format("公会战未配置BOSS:{0}", active.BossPlotID));
                }
            }
            if (boss == null)
            {
                throw new Exception("Loading boss faild.");
            }
            return boss;
        }

        private BossDictionary BossDict
        {
            get
            {
                if (_bossGeneralList.ContainsKey(_activeId))
                {
                    return _bossGeneralList[_activeId];
                }
                return null;
                //throw new Exception("未找到Boss活动id:" + _activeId);
            }
        }
        /// <summary>
        /// 获得参战的玩家列表
        /// </summary>
        /// <returns></returns>
        public List<BossUser> GetCombatUser()
        {
            List<BossUser> list = new List<BossUser>();
            var userGeneralList = BossDict.UserGeneralList;
            foreach (KeyValuePair<string, BossUser> keyPair in userGeneralList)
            {
                BossUser cuser = keyPair.Value;
                list.Add(cuser);
            }

            return list;
        }

        /// <summary>
        /// 获得参战的玩家
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BossUser GetCombatUser(string userId)
        {
            var userGeneralList = BossDict.UserGeneralList;
            if (userGeneralList.ContainsKey(userId))
            {
                return userGeneralList[userId];
            }

            throw new Exception(string.Format("玩家{0}未加入Boss活动id:{1}", userId, _activeId));
        }

        /// <summary>
        /// 刷新排名
        /// </summary>
        /// <returns></returns>
        public List<BossUser> RefreshRanking()
        {
            List<BossUser> buserList = GetCombatUser();
            buserList.QuickSort((a, b) => a.CompareTo(b));
            return buserList;
        }

        /// <summary>
        /// 世界BOSS信息
        /// </summary>
        public CombatGeneral Boss
        {
            get { return BossDict.BossGeneral; }
        }

        /// <summary>
        /// 活动信息
        /// </summary>
        public GameActive GameActive
        {
            get
            {
                if (_gameActiveList.ContainsKey(_activeId))
                {
                    return _gameActiveList[_activeId];
                }
                return null;
                //throw new Exception("未找到Boss活动id:" + _activeId);
            }
        }

        /// <summary>
        /// 增加到参战队列
        /// </summary>
        /// <param name="user"></param>
        public void Append(GameUser user)
        {
            var userGeneralList = BossDict.UserGeneralList;
            if (!userGeneralList.ContainsKey(user.UserID))
            {
                userGeneralList.Add(user.UserID, new BossUser
                {
                    UserId = user.UserID,
                    NickName = user.NickName
                });
            }
        }

        /// <summary>
        /// 取出参战队列
        /// </summary>
        /// <param name="userId"></param>
        public void Exit(string userId)
        {
            BossDict.Exit(userId);
        }

        /// <summary>
        /// 鼓舞加成
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isUseGold"></param>
        /// <param name="percent">鼓舞后的值</param>
        /// <returns></returns>
        public bool Inspire(string userId, bool isUseGold, out double percent)
        {
            bool result = false;
            double num = 0;
            var userGeneralList = BossDict.UserGeneralList;
            if (userGeneralList.ContainsKey(userId))
            {
                BossUser bossUser = userGeneralList[userId];
                if (bossUser.InspirePercent >= 1)
                {
                    num = bossUser.InspirePercent;
                }
                else if (isUseGold || RandomUtils.IsHit(CountryCombat.InspirePercent))
                {
                    result = true;
                    bossUser.InspirePercent = MathUtils.Addition(bossUser.InspirePercent, CountryCombat.InspireIncrease, 1);
                }

                num = bossUser.InspirePercent;
            }
            percent = num;
            return result;
        }

        /// <summary>
        /// 触发战斗
        /// </summary>
        /// <param name="user"></param>
        /// <param name="bossCombatProcess"></param>
        /// <returns></returns>
        public bool Combat(GameUser user, BossCombatProcess bossCombatProcess)
        {
            CombatGeneral bossGeneral = Boss;
            bossGeneral.LossLifeNum = 0;

            BossUser bossUser = GetCombatUser(user.UserID);
            ICombatController controller =  new TjxCombatController();
            ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.BossPlot);
            double inspirePercent = bossUser.InspirePercent + bossUser.ReliveInspirePercent;
            plotCombater.SetAttack(new UserEmbattleQueue(user.UserID, user.UseMagicID, inspirePercent, CombatType.BossPlot));
            plotCombater.SetDefend(new MonsterQueue(Boss));

            bool isWin = plotCombater.Doing();
            bossCombatProcess.ProcessContainer = (CombatProcessContainer)plotCombater.GetProcessResult();
            bossUser.CombatNum += 1;
            bossUser.DamageNum = MathUtils.Addition(bossUser.DamageNum, bossGeneral.LossLifeNum, int.MaxValue);
            bossCombatProcess.LiveNum = bossGeneral.LifeNum;
            bossCombatProcess.DamageNum = bossGeneral.LossLifeNum;
            bossCombatProcess.IsWin = isWin;
            bossCombatProcess.UserId = user.UserID;

            DoDamagePrize(user, bossGeneral.LossLifeNum, bossCombatProcess);
            if (isWin && bossGeneral.IsOver)
            {
                GameActive active = GameActive;
                short lv = MathUtils.Subtraction(bossGeneral.Lv, (short)active.BossPrize.KillBossLv, (short)1);
                int killCoin = lv * active.BossPrize.KillBossRate;
                user.GameCoin = MathUtils.Addition(user.GameCoin, killCoin, int.MaxValue);

                new TjxChatService().SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St5405_CombatKillReward, user.NickName, killCoin));
                TraceLog.ReleaseWriteDebug(string.Format(LanguageManager.GetLang().St5405_CombatKillReward, user.NickName, killCoin));
                bossCombatProcess.KillGameCoin = killCoin;

                ServerEnvSet.Set(ServerEnvKey.KillBossUserID, user.UserID);
                DoKillPrize(user);

                int tempMinute = active.Minutes - ConfigEnvSet.GetInt("BossCombat.KillTime");
                if (new TimeSpan(0, tempMinute, 0).TotalSeconds - active.ColdTime < 0)
                {
                    //Boss自动升级
                    active.BossLv = MathUtils.Addition(active.BossLv, (short)1);
                    //active.Update();
                }
                active.CombatStatus = CombatStatus.Killed;
                CombatHelper.BossKillDate(); //增加boss被杀时间
            }
            else
            {
                bossUser.IsRelive = true;
                bossUser.ReliveBeginDate = DateTime.Now;
            }
            //日志
            var log = new UserCombatLog
            {
                CombatLogID = Guid.NewGuid().ToString(),
                UserID = user.UserID,
                CityID = user.CityID,
                PlotID = GameActive == null ? 0 : GameActive.BossPlotID,
                NpcID = bossGeneral.GeneralID,
                CombatType = CombatType.BossPlot,
                HostileUser = string.Empty,
                IsWin = isWin,
                CombatProcess = JsonUtils.Serialize(bossCombatProcess),
                CreateDate = DateTime.Now

            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);

            //user.Update();

            return isWin;

        }

        /// <summary>
        /// 处理伤害奖励
        /// </summary>
        private void DoDamagePrize(GameUser user, int damageNum, BossCombatProcess bossCombatProcess)
        {
            BossActivePrize bossPrize = GameActive.BossPrize;
            if (bossPrize == null)
            {
                return;
            }
            int obtainNum = MathUtils.Addition(((int)Math.Ceiling((double)damageNum / bossPrize.ObtainRate)), 0, bossPrize.MaxObtain);
            int maxCoin = user.UserLv * bossPrize.MaxCoin;
            int gameCoin = MathUtils.Addition((int)Math.Ceiling((double)damageNum / bossPrize.CoinRate), 0, maxCoin);
            if (gameCoin <= 0)
            {
                gameCoin = 1;
            }
            if (obtainNum <= 0)
            {
                obtainNum = 1;
            }
            //user.ObtainNum = MathUtils.Addition(user.ObtainNum, obtainNum, int.MaxValue);
            user.GameCoin = MathUtils.Addition(user.GameCoin, gameCoin, int.MaxValue);
            user.ExpNum = MathUtils.Addition(user.ExpNum, obtainNum, int.MaxValue);

            bossCombatProcess.ObtainNum = obtainNum;
            bossCombatProcess.GameCoin = gameCoin;
            CountryCombat.Contribution(user.UserID, obtainNum); //公会贡献

            //发到聊天里
            var chatService = new TjxChatService();
            chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().St5405_CombatHarmReward, gameCoin, obtainNum));
            TraceLog.ReleaseWriteDebug(string.Format(LanguageManager.GetLang().St5405_CombatHarmReward, gameCoin, obtainNum));
        }


        /// <summary>
        /// 处理击杀奖励
        /// </summary>
        private void DoKillPrize(GameUser user)
        {
            BossActivePrize bossPrize = GameActive.BossPrize;
            if (bossPrize == null)
            {
                return;
            }
            TjxChatService chatService = new TjxChatService();

            var rankingList = RefreshRanking();
            ServerEnvSet.Set(ServerEnvKey.FirstHalfBoss, rankingList.ToJson());
            int length = rankingList.Count > bossPrize.TopObtain ? bossPrize.TopObtain : rankingList.Count;
            //处理排名奖励
            for (int i = 0; i < length; i++)
            {
                string prizeItemMsg = string.Empty;
                GameUser tempUser = new GameDataCacheSet<GameUser>().FindKey(rankingList[i].UserId);
                tempUser.GameCoin = MathUtils.Addition(tempUser.GameCoin, bossPrize.TopObtainNum, int.MaxValue);
                //前3名奖励
                if (i == 0)
                {
                    DoTopThreePrize(i + 1, tempUser, bossPrize.Rank1, bossPrize.Items, out prizeItemMsg);
                }
                else if (i == 1)
                {
                    DoTopThreePrize(i + 1, tempUser, bossPrize.Rank2, bossPrize.Items, out prizeItemMsg);

                }
                else if (i == 2)
                {
                    DoTopThreePrize(i + 1, tempUser, bossPrize.Rank3, bossPrize.Items, out prizeItemMsg);
                }
                //tempUser.Update();

                if (!string.IsNullOrEmpty(prizeItemMsg)) prizeItemMsg = "，" + prizeItemMsg;

                chatService.SystemSend(ChatType.World, string.Format(LanguageManager.GetLang().St5405_CombatRankmReward,
                    tempUser.NickName,
                    (i + 1),
                    bossPrize.TopObtainNum,
                    prizeItemMsg));
                TraceLog.ReleaseWriteDebug(string.Format(LanguageManager.GetLang().St5405_CombatRankmReward,
                    tempUser.NickName,
                    (i + 1),
                    bossPrize.TopObtainNum,
                    prizeItemMsg));
                // CountryCombat.Contribution(tempUser.UserID, bossPrize.TopObtainNum); //公会贡献
            }
        }

        /// <summary>
        /// 前3名奖励
        /// </summary>
        /// <param name="randId"></param>
        /// <param name="user"></param>
        /// <param name="itemList"></param>
        /// <param name="bossItemList"></param>
        /// <param name="prizeItemMsg"></param>
        private void DoTopThreePrize(int randId, GameUser user, int[] itemList, CacheList<CacheList<BossItem>> bossItemList, out  string prizeItemMsg)
        {
            prizeItemMsg = string.Empty;
            foreach (int itemIndex in itemList)
            {
                if (itemIndex >= bossItemList.Count) continue;
                var tempItems = bossItemList[itemIndex];
                foreach (BossItem bossItem in tempItems)
                {
                    if (user.UserLv >= bossItem.UserLv)
                    {
                        string tempStr = DoBossItem(user, bossItem, randId);
                        if (prizeItemMsg.Length > 0 && tempStr.Length > 0) prizeItemMsg += "，";
                        prizeItemMsg += tempStr;
                        break;
                    }
                }
            }
        }

        private string DoBossItem(GameUser user, BossItem bossItem, int randId)
        {
            string prizeItemMsg = string.Empty;
            //筛选物品
            var itemInfoList = new ConfigCacheSet<ItemBaseInfo>().FindAll(m =>
            {
                bool result = false;
                if (m.ItemType == bossItem.Type)
                {
                    if (m.ItemID == bossItem.ItemId)
                    {
                        return true;
                    }
                    if (m.ItemType == ItemType.TuZhi)
                    {
                        result = m.MedicineLv <= bossItem.ItemLv;
                    }
                    else
                    {
                        if (bossItem.ItemLv > 0 && bossItem.Quality > 0)
                        {
                            result = m.DemandLv == bossItem.ItemLv && (short)m.QualityType == bossItem.Quality;
                        }
                        else if (bossItem.ItemLv > 0)
                        {
                            result = m.DemandLv == bossItem.ItemLv;
                        }
                        else if (bossItem.Quality > 0)
                        {
                            result = (short)m.QualityType == bossItem.Quality;
                        }
                    }
                }
                return result;
            });

            if (itemInfoList.Count > 0)
            {
                for (int i = 0; i < bossItem.Num; i++)
                {
                    ItemBaseInfo itemInfo = itemInfoList[RandomUtils.GetRandom(0, itemInfoList.Count)];
                    if (itemInfo == null) continue;
                    UserItemHelper.AddUserItem(user.UserID, itemInfo.ItemID, 1);
                    if (prizeItemMsg.Length > 0) prizeItemMsg += " ";
                    prizeItemMsg += string.Format("{0}*{1}", itemInfo.ItemName, 1);
                }

            }
            return prizeItemMsg;
        }
    }
}