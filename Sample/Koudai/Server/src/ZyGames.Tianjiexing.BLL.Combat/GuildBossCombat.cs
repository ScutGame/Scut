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

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 公会Boss战役
    /// </summary>
    public class GuildBossCombat
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
        private static DictionaryExtend<string, UserGuild> _userGuildList = new DictionaryExtend<string, UserGuild>();
        private static DictionaryExtend<string, BossDictionary> _bossGeneralList = new DictionaryExtend<string, BossDictionary>();
        private string _guildID = string.Empty;
        private static GameActive gameActive = new ShareCacheStruct<GameActive>().FindKey(UserGuild.ActiveID);
        /// <summary>
        /// 加载数据
        /// </summary>
        public static void InitBoss(UserGuild userGuild)
        {
            try
            {
                if (_userGuildList.ContainsKey(userGuild.GuildID))
                {
                    _userGuildList[userGuild.GuildID] = userGuild;
                }
                else
                {
                    _userGuildList.Add(userGuild.GuildID, userGuild);
                }

                CombatGeneral general = CreateBossGeneral(userGuild);
                if (!_bossGeneralList.ContainsKey(userGuild.GuildID))
                {
                    _bossGeneralList.Add(userGuild.GuildID, new BossDictionary { BossGeneral = general });
                }
                else
                {
                    _bossGeneralList[userGuild.GuildID].BossGeneral = general;
                }
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="userGuild"></param>
        public static void Dispose(UserGuild userGuild)
        {
            if (_bossGeneralList.ContainsKey(userGuild.GuildID))
            {
                _bossGeneralList[userGuild.GuildID].Clear();
            }
            if (_userGuildList.ContainsKey(userGuild.GuildID))
            {
                _userGuildList.Remove(userGuild.GuildID);
            }
        }

        public GuildBossCombat(string guildID)
        {
            _guildID = guildID;
        }

        /// <summary>
        /// 初始化BOSS数据
        /// </summary>
        /// <returns></returns>
        private static CombatGeneral CreateBossGeneral(UserGuild guild)
        {
            CombatGeneral boss = null;
            if (guild != null)
            {
                GuildBossInfo bossInfo = guild.GuildBossInfo;
                if (bossInfo != null)
                {
                    var plotNpcInfoList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == gameActive.BossPlotID);
                    if (plotNpcInfoList.Count > 0)
                    {
                        var embattleList = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.PlotNpcID == plotNpcInfoList[0].PlotNpcID);
                        if (embattleList.Count > 0)
                        {
                            boss = (CombatGeneral)MonsterQueue.Create(embattleList[0]);
                            boss.Lv = (short)MathUtils.Subtraction(bossInfo.BossLv, 0, gameActive.BossDefLv);
                            int bossLiftNum = MonsterQueue.BossProperty(embattleList[0].PlotNpcID, AbilityType.ShengMing).ToInt();
                            int lifeNum = boss.LifeNum + (boss.Lv - gameActive.BossDefLv) * bossLiftNum; //ConfigEnvSet.GetInt("BossCombat.IncreaseLiveNum");
                            boss.LifeMaxNum = lifeNum;
                            boss.LifeNum = lifeNum;
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("公会战未配置BOSS:{0}", gameActive.BossPlotID));
                    }
                }
            }
            if (boss == null)
            {
                throw new Exception("Loading guid boss faild.");
            }
            return boss;
        }

        private BossDictionary BossDict
        {
            get
            {
                if (_bossGeneralList.ContainsKey(_guildID))
                {
                    return _bossGeneralList[_guildID];
                }

                throw new Exception("未找到公会Boss活动id:" + _guildID);
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
            //KeyValuePair<string, BossUser>[] tempList = new KeyValuePair<string, BossUser>[userGeneralList.Count];
            //userGeneralList.CopyTo(tempList, 0);
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
            DictionaryExtend<string, BossUser> userGeneralList = BossDict.UserGeneralList;
            if (userGeneralList.ContainsKey(userId))
            {
                return userGeneralList[userId];
            }

            throw new Exception(string.Format("玩家{0}未加入公会Boss活动id:{1}", userId, _guildID));
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
        /// 公会BOSS信息
        /// </summary>
        public CombatGeneral Boss
        {
            get { return BossDict.BossGeneral; }
        }

        public bool HasCombat
        {
            get
            {
                if (_userGuildList.ContainsKey(_guildID))
                {
                    return _userGuildList[_guildID] != null ? true : false;
                }
                return false;
            }
        }
        /// <summary>
        /// 活动信息
        /// </summary>
        public UserGuild UserGuild
        {
            get
            {
                if (_userGuildList.ContainsKey(_guildID))
                {
                    return _userGuildList[_guildID];
                }
                throw new Exception("未找到公会Boss活动id:" + _guildID);
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
            bool isWin = false;
            BossUser bossUser = GetCombatUser(user.UserID);

            ICombatController controller =  new TjxCombatController();
            ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.BossPlot);
            double inspirePercent = bossUser.InspirePercent + bossUser.ReliveInspirePercent;
            plotCombater.SetAttack(new UserEmbattleQueue(user.UserID, user.UseMagicID, inspirePercent,
                                                         CombatType.BossPlot));
            plotCombater.SetDefend(new MonsterQueue(bossGeneral));

            isWin = plotCombater.Doing();
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
                //GameActive active = GameActive;
                UserGuild guild = UserGuild;
                short bossprizeLv = 0;
                int killbossrate = 0;
                if (guild.BossPrize != null)
                {
                    bossprizeLv = (short)guild.BossPrize.KillBossLv;
                    killbossrate = guild.BossPrize.KillBossRate;
                    //击杀奖
                    BossActivePrize bossPrize = UserGuild.BossPrize;
                    string prizeItemMsg = string.Empty;
                    DoTopThreePrize(0, user, bossPrize.KillReward, bossPrize.Items, out prizeItemMsg);
                }

                short lv = MathUtils.Subtraction(bossGeneral.Lv, bossprizeLv, (short)1);
                int killCoin = lv * killbossrate;
                user.GameCoin = MathUtils.Addition(user.GameCoin, killCoin, int.MaxValue);
                // new CacheChat().SystemSend(ChatType.World, string.Format("{0}玩家获得公会Boss战击杀奖，奖励{1}金币", user.NickName, killCoin));
                new TjxChatService(user).SystemGuildSend(ChatType.Guild,
                                                string.Format(LanguageManager.GetLang().St6105_CombatKillReward, user.NickName, killCoin));
                bossCombatProcess.KillGameCoin = killCoin;

                DoKillPrize();


                int tempMinute = gameActive.Minutes - ConfigEnvSet.GetInt("BossCombat.KillTime");
                int subSeconds = (int)new TimeSpan(0, tempMinute, 0).TotalSeconds;
                if (subSeconds - guild.ColdTime < 0)
                {
                    //Boss自动升级
                    guild.GuildBossInfo.UpdateNotify(obj =>
                    {
                        guild.GuildBossInfo.BossLv = MathUtils.Addition(guild.GuildBossInfo.BossLv, (short)1, short.MaxValue);
                        return true;
                    });
                    //guild.Update();
                }
                guild.CombatStatus = CombatStatus.Killed;
                CombatHelper.UpdateGuildBossKill(guild.GuildID); //公会boss已被杀
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
                PlotID = gameActive == null ? 0 : gameActive.BossPlotID,
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
            BossActivePrize bossPrize = UserGuild.BossPrize;
            if (bossPrize == null)
            {
                return;
            }
            int obtainNum = MathUtils.Addition(((int)Math.Ceiling((double)damageNum / bossPrize.ObtainRate)), 0, bossPrize.MaxObtain);
            int maxCoin = user.UserLv * bossPrize.MaxCoin;
            int gameCoin = MathUtils.Addition((int)Math.Ceiling((double)damageNum / bossPrize.CoinRate), 0, maxCoin);

            user.ObtainNum = MathUtils.Addition(user.ObtainNum, obtainNum, int.MaxValue);
            user.GameCoin = MathUtils.Addition(user.GameCoin, gameCoin, int.MaxValue);
            if (gameCoin <= 0)
            {
                gameCoin = 1;
            }
            if (obtainNum <= 0)
            {
                obtainNum = 1;
            }
            bossCombatProcess.ObtainNum = obtainNum;
            bossCombatProcess.GameCoin = gameCoin;
            CountryCombat.Contribution(user.UserID, obtainNum); //公会贡献

            //发到聊天里
            var chatService = new TjxChatService();
            chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().St6105_CombatHarmReward, gameCoin, obtainNum));
        }


        /// <summary>
        /// 处理击杀奖励
        /// </summary>
        private void DoKillPrize()
        {
            BossActivePrize bossPrize = UserGuild.BossPrize;
            if (bossPrize == null)
            {
                return;
            }
            //CacheChat cacheChat = new CacheChat();

            var rankingList = RefreshRanking();
            int length = rankingList.Count > bossPrize.TopObtain ? bossPrize.TopObtain : rankingList.Count;
            int afterLength = rankingList.Count > bossPrize.AfterObtain ? bossPrize.AfterObtain : rankingList.Count;
            //处理排名奖励
            for (int i = 0; i < length; i++)
            {
                string prizeItemMsg = string.Empty;
                GameUser tempUser = new GameDataCacheSet<GameUser>().FindKey(rankingList[i].UserId);
                tempUser.ObtainNum = MathUtils.Addition(tempUser.ObtainNum, bossPrize.TopObtainNum, int.MaxValue);
                //前5名奖励
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
                else if (i == 3)
                {
                    DoTopThreePrize(i + 1, tempUser, bossPrize.Rank4, bossPrize.Items, out prizeItemMsg);
                }
                else if (i == 4)
                {
                    DoTopThreePrize(i + 1, tempUser, bossPrize.Rank5, bossPrize.Items, out prizeItemMsg);
                }
                //tempUser.Update();

                if (!string.IsNullOrEmpty(prizeItemMsg)) prizeItemMsg = "，" + prizeItemMsg;

                new TjxChatService(tempUser).SystemGuildSend(ChatType.Guild, string.Format(LanguageManager.GetLang().St6105_CombatRankmReward,
                     tempUser.NickName,
                     (i + 1),
                     bossPrize.TopObtainNum,
                     prizeItemMsg));

                CountryCombat.Contribution(tempUser.UserID, bossPrize.TopObtainNum); //公会贡献
            }
            //后5名玩家奖励
            for (int i = length; i < afterLength; i++)
            {
                string prizeItemMsg = string.Empty;
                GameUser tempUser = new GameDataCacheSet<GameUser>().FindKey(rankingList[i].UserId);
                tempUser.ObtainNum = MathUtils.Addition(tempUser.ObtainNum, bossPrize.AfterObtainNum, int.MaxValue);
                //tempUser.Update();

                if (!string.IsNullOrEmpty(prizeItemMsg)) prizeItemMsg = "，" + prizeItemMsg;

                new TjxChatService(tempUser).SystemGuildSend(ChatType.Guild, string.Format(LanguageManager.GetLang().St6105_CombatRankmReward,
                    tempUser.NickName,
                    (i + 1),
                    bossPrize.AfterObtainNum,
                    prizeItemMsg));

                CountryCombat.Contribution(tempUser.UserID, bossPrize.AfterObtainNum); //公会贡献
            }
        }

        /// <summary>
        /// 前5名奖励
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
                        //prizeItemMsg = DoBossItem(user, bossItem, randId);
                        break;
                    }
                }
            }
        }

        private string DoBossItem(GameUser user, BossItem bossItem, int randId)
        {
            string prizeItemMsg = string.Empty;
            ////筛选物品
            var itemInfoList = new ConfigCacheSet<ItemBaseInfo>().FindAll(m => m.ItemType == bossItem.Type && m.MedicineLv == bossItem.ItemLv);
            if (itemInfoList.Count > 0)
            {
                for (int i = 0; i < bossItem.Num; i++)
                {
                    ItemBaseInfo itemInfo = itemInfoList[RandomUtils.GetRandom(0, itemInfoList.Count)];
                    if (itemInfo == null) continue;
                    UserItemHelper.AddUserItem(user.UserID, itemInfo.ItemID, 1);
                    if (prizeItemMsg.Length > 0) prizeItemMsg += " ";
                    prizeItemMsg += string.Format(LanguageManager.GetLang().St5405_CombatNum, itemInfo.ItemName, 1);

                }
            }
            return prizeItemMsg;
        }
    }
}