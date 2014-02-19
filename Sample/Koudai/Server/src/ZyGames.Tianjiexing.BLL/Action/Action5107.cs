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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Com;
using ZyGames.Tianjiexing.Component;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    ///<summary>
    ///5107_竞技战斗详情接口
    ///</summary>
    public class Action5107 : BaseAction
    {
        private static int codeTime = ConfigEnvSet.GetInt("UserSport.CodeTime");
        private string toUserID = string.Empty;
        private string userSportsID = string.Empty;
        private SportsCombat _uSportsCombat = null;
        private CombatProcessContainer _combatProcessList = new CombatProcessContainer();
        private string sportsPrizeStr = string.Empty;
        private int _userTalPriority = 0;
        private int _npcTalPriority = 0;
        private GameUser toGameUser;
        private UserMail userMail = new UserMail(Guid.NewGuid());
        private ConfigCacheSet<GeneralInfo> _cacheSetGeneral = new ConfigCacheSet<GeneralInfo>();
        private List<SelfAbilityEffect> _selfAbilityEffectList = new List<SelfAbilityEffect>();
        private List<SelfAbilityEffect> _defSelfAbilityEffectList = new List<SelfAbilityEffect>();
        public Action5107(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5107, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_uSportsCombat == null ? LanguageManager.GetLang().shortInt : _uSportsCombat.IsWin.ToShort());
            this.PushIntoStack(_uSportsCombat == null ? 0 : _uSportsCombat.RewardGoin);
            this.PushIntoStack(_uSportsCombat == null ? 0 : _uSportsCombat.RewardObtian);
            //攻方阵形
            this.PushIntoStack(_combatProcessList.AttackList.Count);
            foreach (CombatEmbattle combatEmbattle in _combatProcessList.AttackList)
            {
                var general = _cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatEmbattle.GeneralID);
                dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                //dsItem.PushIntoStack(combatEmbattle.BattleHead.ToNotNullString());
                dsItem.PushIntoStack(general == null ? string.Empty : general.BattleHeadID.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                dsItem.PushIntoStack(combatEmbattle.LiveNum);
                dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                dsItem.PushIntoStack(combatEmbattle.AbilityID);
                dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                // 增加佣兵品质
                dsItem.PushShortIntoStack(general == null ? 0 : general.GeneralQuality.ToShort());
                this.PushIntoStack(dsItem);
            }
            //防方阵形
            this.PushIntoStack(_combatProcessList.DefenseList.Count);
            foreach (CombatEmbattle combatEmbattle in _combatProcessList.DefenseList)
            {
                var general = _cacheSetGeneral.FindKey(combatEmbattle.GeneralID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatEmbattle.GeneralID);
                dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(general == null ? string.Empty : general.BattleHeadID.ToNotNullString());
                //dsItem.PushIntoStack(combatEmbattle.BattleHead.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                dsItem.PushIntoStack(combatEmbattle.LiveNum);
                dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                dsItem.PushIntoStack(combatEmbattle.AbilityID);
                dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                // 增加佣兵品质
                dsItem.PushShortIntoStack(general == null ? 0 : general.GeneralQuality.ToShort());
                this.PushIntoStack(dsItem);
            }
            //战斗过程
            this.PushIntoStack(_combatProcessList.ProcessList.Count);
            foreach (CombatProcess combatProcess in _combatProcessList.ProcessList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatProcess.GeneralID);
                dsItem.PushIntoStack(combatProcess.LiveNum);
                dsItem.PushIntoStack(combatProcess.Momentum);
                dsItem.PushIntoStack(combatProcess.AttackTaget.ToShort());
                dsItem.PushIntoStack(combatProcess.AttackUnit.ToShort());
                dsItem.PushIntoStack(combatProcess.AbilityProperty.ToShort());
                dsItem.PushIntoStack(combatProcess.AttStatus.ToShort());
                dsItem.PushIntoStack(combatProcess.DamageNum);
                dsItem.PushIntoStack(combatProcess.AttEffectID.ToNotNullString());
                dsItem.PushIntoStack(combatProcess.TargetEffectID.ToNotNullString());
                dsItem.PushIntoStack(combatProcess.IsMove.ToShort());
                dsItem.PushIntoStack(combatProcess.Position.ToShort());
                dsItem.PushIntoStack(combatProcess.Role.ToShort());

                dsItem.PushIntoStack(combatProcess.DamageStatusList.Count);
                foreach (AbilityEffectStatus effectStatus in combatProcess.DamageStatusList)
                {
                    DataStruct dsItem2 = new DataStruct();
                    dsItem2.PushIntoStack(effectStatus.AbilityType.ToShort());
                    dsItem2.PushIntoStack(effectStatus.DamageNum);
                    dsItem2.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                    dsItem.PushIntoStack(dsItem2);
                }

                dsItem.PushIntoStack(combatProcess.TargetList.Count);
                foreach (TargetProcess targetProcess in combatProcess.TargetList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(targetProcess.GeneralID);
                    dsItem1.PushIntoStack(targetProcess.LiveNum);
                    dsItem1.PushIntoStack(targetProcess.Momentum);
                    dsItem1.PushIntoStack(targetProcess.DamageNum);
                    dsItem1.PushIntoStack(targetProcess.IsShanBi.ToShort());
                    dsItem1.PushIntoStack(targetProcess.IsGeDang.ToShort());
                    dsItem1.PushIntoStack(targetProcess.IsBack.ToShort());
                    dsItem1.PushIntoStack(targetProcess.IsMove.ToShort());
                    dsItem1.PushIntoStack(targetProcess.BackDamageNum);
                    dsItem1.PushIntoStack(targetProcess.TargetStatus.ToShort());
                    dsItem1.PushIntoStack(targetProcess.Position.ToShort());
                    dsItem1.PushIntoStack(targetProcess.Role.ToShort());
                    //目标中招效果
                    dsItem1.PushIntoStack(targetProcess.DamageStatusList.Count);
                    foreach (AbilityEffectStatus effectStatus in targetProcess.DamageStatusList)
                    {
                        DataStruct dsItem12 = new DataStruct();
                        dsItem12.PushIntoStack(effectStatus.AbilityType.ToShort());
                        dsItem12.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                        dsItem1.PushIntoStack(dsItem12);
                    }
                    dsItem1.PushIntoStack(targetProcess.IsBaoji.ToShort());
                    dsItem1.PushIntoStack(targetProcess.TrumpStatusList.Count);
                    foreach (var item in targetProcess.TrumpStatusList)
                    {
                        DataStruct dsItem13 = new DataStruct();
                        dsItem13.PushIntoStack((short)item.AbilityID);
                        dsItem13.PushIntoStack(item.Num);
                        dsItem1.PushIntoStack(dsItem13);
                    }

                    dsItem.PushIntoStack(dsItem1);
                }
                dsItem.PushIntoStack(combatProcess.TrumpStatusList.Count);
                foreach (var item in combatProcess.TrumpStatusList)
                {
                    DataStruct dsItem14 = new DataStruct();
                    dsItem14.PushIntoStack((short)item.AbilityID);
                    dsItem14.PushIntoStack(item.Num);
                    dsItem.PushIntoStack(dsItem14);
                }
                dsItem.PushIntoStack(combatProcess.FntHeadID.ToNotNullString());
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(_selfAbilityEffectList.Count);
            foreach (var selfAbilityEffect in _selfAbilityEffectList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(selfAbilityEffect.GeneralID);
                dsItem.PushIntoStack(selfAbilityEffect.EffectID1.ToNotNullString());
                dsItem.PushIntoStack(selfAbilityEffect.FntHeadID.ToNotNullString());
                dsItem.PushIntoStack(selfAbilityEffect.IsIncrease ? 1.ToShort() : 0.ToShort());
                dsItem.PushIntoStack(selfAbilityEffect.Position);
                dsItem.PushIntoStack(selfAbilityEffect.Role.ToInt());
                PushIntoStack(dsItem);
            }
            PushIntoStack(_userTalPriority);
            PushIntoStack(_npcTalPriority);
            PushIntoStack(sportsPrizeStr);


        }

        public override bool GetUrlElement()
        {
            httpGet.GetString("ToUserID", ref toUserID);
            httpGet.GetString("UserSportsID", ref userSportsID);
            return true;
        }

        public override bool TakeAction()
        {
            
            int sportsIndex;
            if (int.TryParse(userSportsID, out sportsIndex))
            {
                var tempList = ContextUser.GetSportsCombat();
                if (tempList.Count > 0 && sportsIndex < tempList.Count)
                {
                    sportsIndex = MathUtils.Addition(sportsIndex, 1);
                    _uSportsCombat = tempList[tempList.Count - sportsIndex] ?? new SportsCombat();
                    _combatProcessList = _uSportsCombat.CombatProcess ?? new CombatProcessContainer();

                    _userTalPriority = CombatHelper.TotalPriorityNum(ContextUser.UserID, 0);

                    if (_combatProcessList.DefenseList.Count > 0)
                    {
                        _npcTalPriority = CombatHelper.TotalPriorityNum(_combatProcessList.DefenseList[0].UserID, 0);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(toUserID))
            {
                UserHelper.ChechDailyRestrain(ContextUser.UserID);
                if (ContextUser.UserID == toUserID)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                if (GetChallGeNum(ContextUser.UserID) <= 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St5107_ChallGeNumFull;
                    return false;
                }

                toGameUser = UserCacheGlobal.LoadOffline(toUserID);
                if (ContextUser.UseMagicID == 0 || toGameUser == null || toGameUser.UseMagicID == 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St4004_NoUseMagic;
                    return false;
                }
                RankingHelper.SportSRewardGain(toUserID, toGameUser.RankID);   //被挑战者的竞技排名
                //todo 竞技场冷却时间清除
                //var userQueueCache = new GameDataCacheSet<UserQueue>();
                //var queueArray = userQueueCache.FindAll(Uid, m => m.QueueType == QueueType.JingJiTiaoZhan);
                //if (queueArray.Count > 0 && queueArray[0].DoRefresh() > 0)
                //{
                //    ErrorCode = LanguageManager.GetLang().ErrorCode;
                //    ErrorInfo = LanguageManager.GetLang().St5107_Colding;
                //    return false;
                //}
                //if (queueArray.Count > 0)
                //{
                //    var userQueue = queueArray[0];
                //    userQueue.ColdTime = codeTime;
                //    userQueue.TotalColdTime = codeTime;
                //    userQueue.Timing = DateTime.Now;
                //}
                //else
                //{
                //    UserQueue queue = new UserQueue()
                //    {
                //        QueueID = Guid.NewGuid().ToString(),
                //        QueueType = QueueType.JingJiTiaoZhan,
                //        QueueName = QueueType.JingJiTiaoZhan.ToString(),
                //        UserID = ContextUser.UserID,
                //        ColdTime = codeTime,
                //        IsSuspend = false,
                //        TotalColdTime = codeTime,
                //        Timing = DateTime.Now
                //    };
                //    userQueueCache.Add(queue);
                //    //queue.Append();
                //}
                //次数限制修改
                int sportNum = 0;
                UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
                if (dailyRestrain != null)
                {
                    sportNum = MathUtils.Addition(dailyRestrain.Funtion9, 1, int.MaxValue);
                    dailyRestrain.Funtion9 = sportNum;
                    //dailyRestrain.Update();
                }
                else
                {
                    sportNum = 1;
                }
                TriggerSportCombat(ContextUser, toGameUser, sportNum, ref sportsPrizeStr,userMail);

                var tempList = ContextUser.GetSportsCombat();
                if (tempList.Count > 0)
                {
                    _uSportsCombat = tempList[tempList.Count - 1] ?? new SportsCombat();
                    _combatProcessList = _uSportsCombat.CombatProcess ?? new CombatProcessContainer();
                }
                //日常任务-竞技
                TaskHelper.TriggerDailyTask(Uid, 4006);
                _userTalPriority = CombatHelper.TotalPriorityNum(ContextUser.UserID, 0);
                _npcTalPriority = CombatHelper.TotalPriorityNum(toUserID, 0);
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }

            userMail.GameCoin = _uSportsCombat.RewardGoin;
            userMail.Obtion = _uSportsCombat.RewardObtian;
            var tjxMailService = new TjxMailService(toGameUser);
            tjxMailService.Send(userMail);

            _selfAbilityEffectList = UserAbilityHelper.GetSelfAbilityEffectList(ContextUser.UserID, 0);
            _defSelfAbilityEffectList = UserAbilityHelper.GetSelfAbilityEffectList(toUserID, 1);
            _selfAbilityEffectList.AddRange(_defSelfAbilityEffectList);
            return true;
        }

        /// <summary>
        /// 增加仇敌
        /// </summary>
        /// <param name="user"></param>
        /// <param name="toUser"></param>
        private static void AddFoe(GameUser user, GameUser toUser)
        {
            var cacheSet = new ShareCacheStruct<UserFriends>();
            var friends = cacheSet.Find(s => s.FriendID == user.UserID && s.UserID == toUser.UserID && s.FriendType == FriendType.Friend);
            var choudiFriends = cacheSet.Find(s => s.FriendID == user.UserID && s.UserID == toUser.UserID && s.FriendType == FriendType.ChouDi);
            if (friends == null && choudiFriends == null)
            {
                var list = cacheSet.FindAll(s => s.UserID == toUser.UserID && s.FriendType == FriendType.ChouDi);
                if (list.Count >= 50)
                {
                    list.QuickSort((x, y) =>
                    {
                        if (x == null && y == null) return 0;
                        if (x != null && y == null) return 1;
                        if (x == null) return -1;
                        return (x.FightTime).CompareTo(y.FightTime);
                    });
                    int count = list.Count - 49 + 1;
                    for (int i = 0; i < count; i++)
                    {
                        cacheSet.Delete(list[i]);
                    }
                }
                var userFriends = new UserFriends();
                userFriends.UserID = toUser.UserID;
                userFriends.FriendID = user.UserID;
                userFriends.FightTime = DateTime.Now;
                userFriends.FriendType = FriendType.ChouDi;
                userFriends.ChatTime = DateTime.Now;
                cacheSet.Add(userFriends);
            }

        }
        private static void TriggerSportCombat(GameUser user, GameUser toUser, int sportNum, ref string sportsPrizeString,UserMail userMail)
        {
            CombatProcessContainer combatProcessList = null;
            int rewardGoin = 0;
            int rewardObtion = 0; //荣誉值
            int experence = 0; //经验
            int winNum = 0;
            //原因：gameuser加了连胜字段
            winNum = user.VictoryNum;

            int rankIndex;
            int torankIndex;
            Ranking<UserRank> ranking = RankingFactory.Get<UserRank>(CombatRanking.RankingKey);
            if (!ranking.TryGetRankNo(m => m.UserID == user.UserID, out rankIndex) || !ranking.TryGetRankNo(m => m.UserID == toUser.UserID, out torankIndex))
            {
                return;
            }
            UserRank userRank = ranking.Find(s => s.UserID == user.UserID);
            UserRank toUserRank = ranking.Find(s => s.UserID == toUser.UserID);
            if (userRank == null || toUserRank == null) return;

            //修改wuzf，两个地方调用Doing()方法
            ISingleCombat sportCombat = CombatFactory.TriggerTournament(user, toUser);
            if (sportCombat == null) return;
            bool isWin = sportCombat.Doing();

            if (isWin)
            {
                winNum = MathUtils.Addition(winNum, 1, int.MaxValue);
                userRank.VictoryNum = winNum;
                toUserRank.VictoryNum = 0;
                toUser.VictoryNum = 0;
                new GameDataCacheSet<GameUser>().UpdateSelf(toUser.PersonalId);
                rewardGoin = (user.UserLv * 40); //GetRewardGameCoin(user, user.UserLv);
                experence = (user.UserLv * 10);
                rewardObtion = 10;
                AddFoe(user, toUser);
            }
            else
            {
                winNum = 0;
                userRank.VictoryNum = 0;
                rewardGoin = (user.UserLv * 20);
                if (!rewardGoin.IsValid())
                {
                    rewardGoin = 0;
                }
                experence = (user.UserLv * 5);
                rewardObtion = 5;
            }


            //公会贡献
            UserHelper.Contribution(user.UserID, rewardObtion);
            if (!string.IsNullOrEmpty(user.MercenariesID))
            {
                GuildMemberLog.AddLog(user.MercenariesID, new MemberLog
                {
                    UserID = user.UserID,
                    IdolID = 0,
                    LogType = 1,
                    GainObtion = rewardObtion,
                    Experience = rewardObtion,
                    GainAura = 0,
                    InsertDate = DateTime.Now,
                });
            }
            user.VictoryNum = winNum;
            new GameDataCacheSet<GameUser>().UpdateSelf(user.PersonalId);
            user.ExpNum = MathUtils.Addition(user.ExpNum, rewardObtion, int.MaxValue);
            user.GameCoin = MathUtils.Addition(user.GameCoin, rewardGoin, int.MaxValue);
            GeneralHelper.UserGeneralExp(user.UserID, 0, experence);

            int _rankTopId = 0;
            int _ranktoTopID = 0;
            if (isWin && rankIndex > torankIndex)
            {
                _rankTopId = toUserRank.RankId;
                _ranktoTopID = userRank.RankId;
            }
            else
            {
                _rankTopId = userRank.RankId;
                _ranktoTopID = toUserRank.RankId;
            }

            combatProcessList = (CombatProcessContainer)sportCombat.GetProcessResult();
            user.SportsCombatQueue.Enqueue(new SportsCombat()
            {
                ToUser = toUser.UserID,
                ToUserName = toUser.NickName,
                TopID = _rankTopId,
                IsWin = isWin,
                RewardGoin = rewardGoin,
                RewardObtian = rewardObtion,
                SportsNum = sportNum,
                WinNum = winNum,
                CombatDate = DateTime.Now,
                CombatProcess = combatProcessList,
                IsSelf = true,
                RankStatus = GetRankStatus(isWin, userRank, toUserRank)
            });

            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(toUser.UserID) ?? new UserDailyRestrain();
            toUser.SportsCombatQueue.Enqueue(new SportsCombat()
            {
                ToUser = user.UserID,
                ToUserName = user.NickName,
                TopID = _ranktoTopID,
                IsWin = !isWin,
                RewardGoin = 0,
                RewardObtian = 0,
                SportsNum = dailyRestrain.Funtion9,
                WinNum = toUserRank.VictoryNum,
                CombatDate = DateTime.Now,
                CombatProcess = combatProcessList,
                IsSelf = false,
                RankStatus = GetRankStatus(!isWin, toUserRank, userRank)
            });

            //日志
            UserCombatLog log = new UserCombatLog()
            {
                CombatLogID = Guid.NewGuid().ToString(),
                UserID = user.UserID,
                CityID = 0,
                PlotID = 0,
                NpcID = 0,
                CombatType = CombatType.User,
                HostileUser = toUser.UserID,
                IsWin = isWin,
                CombatProcess = JsonUtils.Serialize(combatProcessList),
                CreateDate = DateTime.Now
            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);

            string prizeContent = string.Empty;
            MysteryHelper.IsTriggerMyStery(user, MysteryType.Jingjichang, out prizeContent);
            MysteryInfo mysteryInfo = new ConfigCacheSet<MysteryInfo>().FindKey(MysteryType.Jingjichang);
            if (!string.IsNullOrEmpty(prizeContent) && mysteryInfo != null)
            {
                var prompt = string.Empty;
                var broadcast = string.Empty;
                if (isWin)
                {
                    prompt = mysteryInfo.WinPrompt;
                    broadcast = mysteryInfo.WinBroadcast;
                }
                else
                {
                    prompt = mysteryInfo.Prompt;
                    broadcast = mysteryInfo.Broadcast;
                }
                sportsPrizeString = string.Format(prompt, toUser.NickName, prizeContent);
                string broadContent = string.Format(broadcast, toUser.NickName, prizeContent);
                var broadcastService = new TjxBroadcastService(null);
                var msg = broadcastService.Create(NoticeType.System, broadContent);
                broadcastService.Send(msg);
            }
            //sportsPrizeString = SportsPrizeHelper.DoPrize(user.UserID, isWin, toUser.NickName);
            //var userMail = new UserMail(Guid.NewGuid());
            var mailText = string.Format(LanguageManager.GetLang().SportsRankLetterForWin, user.NickName);

            if (isWin)
            {
                if (rankIndex > torankIndex)
                {
                    //SportsRankPrize(rankIndex, user, torankIndex);
                    //SportsRankPrize(torankIndex, toUser, rankIndex);
                    ranking.TryMove(rankIndex, torankIndex);
                    //user.RankID = torankIndex;
                    //user.RankDate = DateTime.Now;
                    //toUser.RankID = rankIndex;
                    //toUser.RankDate = DateTime.Now;
                    new GameDataCacheSet<GameUser>().UpdateSelf(user.PersonalId);
                    new GameDataCacheSet<GameUser>().UpdateSelf(toUser.PersonalId);
                    mailText = string.Format(LanguageManager.GetLang().SportsRankLetterForFailure, user.NickName, rankIndex);
                }
                else
                {
                    mailText = string.Format(LanguageManager.GetLang().SportsRankLetterForFailureRank, user.NickName, rankIndex);
                }
                UserHelper.SprostSystemChat(userRank, toUserRank);
                NoviceHelper.SportCombatFestival(user, winNum, userRank.VictoryNum);
            }
            userMail.Content = mailText;
            var userid = toUser.UserID.ToInt();
            userMail.ToUserID = userid;
            userMail.UserId = userid;
            userMail.MailType = (int)MailType.Fight;
            userMail.ToUserName = toUser.NickName;
            userMail.FromUserName = LanguageManager.GetLang().St_SystemMailTitle;
            userMail.FromUserId = LanguageManager.GetLang().SystemUserId;
            userMail.SendDate = DateTime.Now;
            userMail.Title = string.Empty;
            userMail.CounterattackUserID = user.UserID.ToInt();
            userMail.CombatProcess = JsonUtils.Serialize(combatProcessList);
            userMail.IsWin = isWin;
            
            
        }

        private static short GetRankStatus(bool isWin, UserRank userRank, UserRank toUserRank)
        {
            //0：不变 1：上升 2：下降
            if (isWin && userRank.RankId > toUserRank.RankId)
            {
                return (short)1;
            }
            else if (!isWin && userRank.RankId < toUserRank.RankId)
            {
                return (short)2;
            }
            else
            {
                return (short)0;
            }
        }

        /// <summary>
        /// 连胜金币奖励
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int GetRewardGameCoin(GameUser user, short userLv)
        {
            int rewardGameCoin = 0;
            IList<SportsCombat> toUserCombat = user.GetSportsCombat();
            if (toUserCombat.Count > 0)
            {
                var temp = toUserCombat[toUserCombat.Count - 1];
                if (temp.WinNum > 10)
                {
                    rewardGameCoin = (int)(userLv * 40 * 1.2);
                }
                else if (temp.WinNum > 20)
                {
                    rewardGameCoin = (int)(userLv * 40 * 1.5);
                }
                else if (temp.WinNum > 50)
                {
                    rewardGameCoin = (int)(userLv * 40 * 2);
                }
                else if (temp.WinNum > 100)
                {
                    rewardGameCoin = (int)(userLv * 40 * 3);
                }
                else
                {
                    rewardGameCoin = (int)(userLv * 40);
                }
            }
            else
            {
                rewardGameCoin = (int)(userLv * 40);
            }
            if (rewardGameCoin.IsValid())
            {
                return rewardGameCoin;
            }
            throw new ArgumentOutOfRangeException(string.Format("User:{0}连胜金币奖励溢出:{1}", user.UserID, rewardGameCoin));
        }

        ///<summary>
        /// 剩余次数
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int GetChallGeNum(string userID)
        {
            int InitialNum = VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.JingJiChangTiaoZhan);
            int totalNum = 0;
            int AddNum = 0;
            int currNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.RefreshDate.Date == DateTime.Now.Date)
            {
                currNum = dailyRestrain.Funtion9;
            }

            UserChallengeNum userChallenge = new GameDataCacheSet<UserChallengeNum>().FindKey(userID);
            if (userChallenge != null && DateTime.Now.Date == userChallenge.InsertDate.Date)
            {
                AddNum = userChallenge.ChallengeNum;
            }

            totalNum = MathUtils.Addition(InitialNum, AddNum, int.MaxValue);
            totalNum = MathUtils.Subtraction(totalNum, currNum, 0);
            return totalNum;
        }

    }
}