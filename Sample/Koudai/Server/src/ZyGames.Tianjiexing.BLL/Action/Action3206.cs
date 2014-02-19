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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3206_宠物拦截接口
    /// </summary>
    public class Action3206 : BaseAction
    {
        private string _userId;
        private int _petId;
        private bool isWin;
        private int _interceptGameCoin;
        private int _interceptObtainNum;
        private CombatProcessContainer combatProcessList = new CombatProcessContainer();


        public Action3206(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3206, httpGet)
        {

        }
        public override void BuildPacket()
        {
            PushIntoStack(isWin ? (short)1 : (short)0);
            PushIntoStack(_interceptGameCoin);
            PushIntoStack(_interceptObtainNum);
            //攻方阵形
            this.PushIntoStack(combatProcessList.AttackList.Count);
            foreach (CombatEmbattle combatEmbattle in combatProcessList.AttackList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatEmbattle.GeneralID);
                dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                dsItem.PushIntoStack(combatEmbattle.LiveNum);
                dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                dsItem.PushIntoStack(combatEmbattle.AbilityID);
                dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                this.PushIntoStack(dsItem);
            }
            //防方阵形
            this.PushIntoStack(combatProcessList.DefenseList.Count);
            foreach (CombatEmbattle combatEmbattle in combatProcessList.DefenseList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(combatEmbattle.GeneralID);
                dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                dsItem.PushIntoStack(combatEmbattle.LiveNum);
                dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                dsItem.PushIntoStack(combatEmbattle.AbilityID);
                dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                this.PushIntoStack(dsItem);
            }
            //战斗过程
            this.PushIntoStack(combatProcessList.ProcessList.Count);
            foreach (CombatProcess combatProcess in combatProcessList.ProcessList)
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
                this.PushIntoStack(dsItem);
            }
        }


        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserId", ref _userId)
                 && httpGet.GetInt("PetId", ref _petId))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (_userId.Equals(Uid))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St3206_PetInterceptError;
                return false;
            }
            if (new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid) != null)
            {
                var userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);
                int maxNum = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.PetIntercept).MaxNum;
                if (userRestrain.UserExtend != null && userRestrain.UserExtend.PetIntercept >= maxNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St3206_PetInterceptTimesOut;
                    return false;
                }
            }

            var petRunPool = new ShareCacheStruct<PetRunPool>().FindKey(_userId);
            if (petRunPool == null || !petRunPool.IsRunning)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St3206_PetInterceptFaild;
                return false;
            }
            if (!string.IsNullOrEmpty(petRunPool.FriendID) && petRunPool.FriendID.Equals(Uid))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St3206_PetFriendError;
                return false;
            }

            if (!string.IsNullOrEmpty(petRunPool.InterceptUser))
            {
                string[] InterceptUserList = petRunPool.InterceptUser.Split(',');
                foreach (string intercept in InterceptUserList)
                {
                    if (intercept == ContextUser.UserID)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St3206_PetInterceptFull;
                        return false;
                    }
                }
                petRunPool.InterceptUser = petRunPool.InterceptUser + ContextUser.UserID + ",";
            }
            else
            {
                petRunPool.InterceptUser = ContextUser.UserID + ",";
            }
            //petRunPool.Update();
            var chatService = new TjxChatService();
            string toUserId = petRunPool.UserID;
            if (!string.IsNullOrEmpty(petRunPool.FriendID))
            {
                toUserId = petRunPool.FriendID;
            }
            GameUser toGameUser = UserCacheGlobal.LoadOffline(toUserId);
            ISingleCombat combatProxy = CombatFactory.TriggerTournament(ContextUser, toGameUser);
            isWin = combatProxy.Doing();
            combatProcessList = (CombatProcessContainer)combatProxy.GetProcessResult();
            if (isWin)
            {
                _interceptGameCoin = petRunPool.InterceptGameCoin;
                _interceptObtainNum = petRunPool.InterceptObtainNum;

                petRunPool.GameCoin = MathUtils.Subtraction(petRunPool.GameCoin, _interceptGameCoin, 0);
                petRunPool.ObtainNum = MathUtils.Subtraction(petRunPool.ObtainNum, _interceptObtainNum, 0);
                //petRunPool.Update();

                ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, _interceptGameCoin);
                ContextUser.ObtainNum = MathUtils.Addition(ContextUser.ObtainNum, _interceptObtainNum);
                //ContextUser.Update();
                var user = UserCacheGlobal.LoadOffline(petRunPool.UserID) ?? new GameUser();
                chatService.SystemSendWhisper(user, string.Format(LanguageManager.GetLang().Chat_PetWasBlocked,
                    (new ConfigCacheSet<PetInfo>().FindKey(petRunPool.PetID) ?? new PetInfo()).PetName, ContextUser.NickName, _interceptGameCoin, _interceptObtainNum
                    ));

                chatService.SystemSendWhisper(ContextUser, string.Format(LanguageManager.GetLang().Chat_PetInterceptSucess,
                    ContextUser.NickName,
                    user.NickName,
                    (new ConfigCacheSet<PetInfo>().FindKey(petRunPool.PetID) ?? new PetInfo()).PetName,
                    _interceptGameCoin,
                    _interceptObtainNum));

            }
            //日志
            UserCombatLog log = new UserCombatLog();
            log.CombatLogID = Guid.NewGuid().ToString();
            log.UserID = Uid;
            log.CityID = ContextUser.CityID;
            log.PlotID = 0;
            log.NpcID = 0;
            log.CombatType = CombatType.PetRun;
            log.HostileUser = toUserId;
            log.IsWin = isWin;
            log.CombatProcess = JsonUtils.Serialize(combatProcessList);
            log.PrizeItem = new CacheList<PrizeItemInfo>();
            log.CreateDate = DateTime.Now;

            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);

            if (new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid) != null)
            {
                var restrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);

                restrain.UserExtend.UpdateNotify(obj =>
                                                   {
                                                       restrain.UserExtend.PetIntercept = MathUtils.Addition(restrain.UserExtend.PetIntercept, 1);
                                                       return true;
                                                   });
                //restrain.Update();
            }
            return true;
        }
    }
}