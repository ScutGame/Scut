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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;

using System.Diagnostics;
using ZyGames.Tianjiexing.Model.Config;
using BaseLog = ZyGames.Tianjiexing.BLL.Base.BaseLog;
using ZyGames.Tianjiexing.Model.DataModel;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component.Chat;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1008_用户角色详情接口
    /// </summary>
    public class Action1008 : BaseAction
    {
        private string headID = string.Empty;
        private int lifeNum = 0;
        private int lifeMaxNum = 0;
        private short maxEnergyNum = 0;
        private short careerID = 0;
        private int sex = 0;
        private int generalID = 0;
        private string guildID = string.Empty;
        private UserGeneral userGeneralInfo = null;
        private GeneralEscalateInfo escalateInfo = null;
        private int _itemLiveNum;
        private int _itemLiveMaxNum;
        private short genlv = 0;
        private int pictureTime = 0;
        private string pictureID = string.Empty;
        private List<BlessingInfo> _blessingList = new List<BlessingInfo>();
        private int demandGold = 0;
        private short isHelper = 0;
        private int plotstatucID;
        private int mercenarySeq;
        private string cardUserID = string.Empty;
        private int battleNum = 0;
        private int totalBattle = 0;
        private int rstore;
        private int totalRstore = 0;
        private int _honourNum = 0;
        private int _nextHonourNum = 0;
        private int _talPriority = 0;
        private List<UserFunction> functionList = new List<UserFunction>();
        private int unReadCount;
        public Action1008(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1008, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(ContextUser.CityID);
            PushIntoStack(ContextUser.PointX);
            PushIntoStack(ContextUser.PointY);
            PushIntoStack(generalID);
            PushIntoStack(guildID.ToNotNullString());
            PushIntoStack(ObjectExtend.ToNotNullString(ContextUser.NickName));
            PushIntoStack(genlv);
            PushIntoStack(careerID);
            PushIntoStack(sex);
            PushIntoStack(headID.ToNotNullString());
            PushIntoStack(ContextUser.GoldNum);
            PushIntoStack(ContextUser.GameCoin);
            PushIntoStack(lifeNum);
            PushIntoStack(lifeMaxNum);
            PushIntoStack((ContextUser.EnergyNum).ToShort());
            PushIntoStack(maxEnergyNum);
            PushIntoStack(userGeneralInfo == null ? 0 : userGeneralInfo.CurrExperience);
            PushIntoStack(escalateInfo == null ? 0 : escalateInfo.UpExperience);
            PushIntoStack((short)ContextUser.VipLv);
            PushIntoStack((short)ContextUser.CountryID);

            //加状态
            PushIntoStack(_itemLiveNum);
            PushIntoStack(_itemLiveMaxNum);
            PushIntoStack(_blessingList.Count);
            foreach (var blessing in _blessingList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(blessing.BlessingType);
                dsItem.PushIntoStack(blessing.BlessingNum);
                dsItem.PushIntoStack(blessing.PropDate);
                dsItem.PushIntoStack(string.Empty);
                dsItem.PushIntoStack(blessing.PropDesc.ToNotNullString());
                PushIntoStack(dsItem);
            }
            PushIntoStack((short)ContextUser.UserLocation);
            PushIntoStack(ContextUser.ExpNum);
            PushIntoStack((short)ContextUser.UserStatus);
            PushIntoStack(ContextUser.SweepPool == null ? 0 : ContextUser.SweepPool.PlotID);
            PushIntoStack(ContextUser.IsUseupItem ? (short)1 : (short)0);
            PushIntoStack(pictureID.ToNotNullString());
            PushIntoStack(pictureTime);
            PushIntoStack(demandGold);
            PushIntoStack(ContextUser.SurplusEnergy);
            PushIntoStack(isHelper);
            PushIntoStack(plotstatucID);
            PushIntoStack(mercenarySeq);
            PushIntoStack(cardUserID.ToNotNullString());
            PushIntoStack(0);
            this.PushIntoStack(battleNum);
            this.PushIntoStack(totalBattle);
            this.PushIntoStack(rstore);
            this.PushIntoStack(totalRstore);
            PushIntoStack(_honourNum);
            PushIntoStack(_nextHonourNum);
            PushIntoStack(ContextUser.CombatNum);
            PushIntoStack(_talPriority);
            PushIntoStack(ContextUser.IsLv ? 1.ToShort() : 0.ToShort());
            this.PushIntoStack(functionList.Count);
            foreach (UserFunction item in functionList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.FunEnum.ToShort());
                PushIntoStack(dsItem);
            }

            if (ContextUser.OpenFun != null && ContextUser.OpenFun.Count > 0)
            {
                var OpenFun = ContextUser.OpenFun;
                this.PushIntoStack(OpenFun.Count);
                foreach (UserFunction item in OpenFun)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(item.FunEnum.ToShort());
                    PushIntoStack(dsItem);
                }
                ContextUser.OpenFun.Clear();
            }
            else
            {
                PushIntoStack(0);
            }

            PushIntoStack(ContextUser.WizardNum);
            // PushIntoStack(unReadCount);
            // 未读的信件数目
            PushIntoStack(unReadCount);

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            short currMaxLv = ConfigEnvSet.GetInt("User.CurrMaxLv").ToShort();
            //NoviceHelper.CheckFestival(ContextUser);

            NoviceHelper.GetFunctionEnum(Uid); //默认开启金币，精力大作战功能
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                UserHelper.ChecheDailyContribution(ContextUser.MercenariesID, ContextUser.UserID);
            }
            PaymentService.Trigger(ContextUser);
            if (ContextUser.UserLv > currMaxLv)
            {
                genlv = currMaxLv;
            }
            else
            {
                genlv = ContextUser.UserLv;
            }
            var cacheSetGeneralEscalate = new ConfigCacheSet<GeneralEscalateInfo>();
            GeneralEscalateHelper.AddUserLv(ContextUser, 0);
            _honourNum = ContextUser.HonourNum;
            int lv = ContextUser.UserLv;
            lv = lv < 0 ? 1 : lv + 1;
            var generalEscalate =
                cacheSetGeneralEscalate.Find(s => s.GeneralType == GeneralType.YongHu && s.GeneralLv == lv);
            if (generalEscalate != null)
            {
                _nextHonourNum = generalEscalate.UpExperience;
            }
            guildID = ContextUser.MercenariesID;
            var userGeneralList = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID);
            if (userGeneralList.Count == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }

            userGeneralInfo = userGeneralList[0]; //UserGeneral.GetMainGeneral(ContextUser.UserID);
            if (userGeneralInfo != null)
            {
                //wuzf 去掉刷新血量，其它改变血量接口有触发刷新
                //userGeneralInfo.RefreshMaxLife();
                generalID = userGeneralInfo.GeneralID;
                //careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(userGeneralInfo.CareerID);
                headID = userGeneralInfo.HeadID; //ContextUser.Sex ? careerInfo.HeadID2 : careerInfo.HeadID;
                escalateInfo = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(genlv);
                lifeNum = userGeneralInfo.LifeNum;
                careerID = userGeneralInfo.CareerID;
            }
            lifeMaxNum = UserHelper.GetMaxLife(ContextUser.UserID, generalID);
            maxEnergyNum = (short)ConfigEnvSet.GetInt("User.MaxEnergyNum");
            sex = ContextUser.Sex ? 1 : 0;

            //道具图标
            _blessingList = UserHelper.BlessingInfoList(ContextUser);
            //变身卡图标
            List<UserProps> userPropsList = new GameDataCacheSet<UserProps>().FindAll(ContextUser.UserID, u => u.PropType == 3 && u.ItemID != 5200 && u.ItemID != 7003);
            if (userPropsList.Count > 0)
            {
                UserProps props = userPropsList[0];
                int pTime = props.DoRefresh();
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(props.ItemID);
                if (itemInfo != null && pTime > pictureTime)
                {
                    pictureID = itemInfo.PictrueID;
                    pictureTime = pTime;
                }
            }
            //兼容客户端上已版本血量图标
            List<UserProps> userPropsList2 = new GameDataCacheSet<UserProps>().FindAll(ContextUser.UserID, u => u.PropType == 1);
            if (userPropsList2.Count > 0)
            {
                UserProps props = userPropsList2[0];
                int pTime = props.DoRefresh();
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(props.ItemID);
                if (itemInfo != null && pTime > pictureTime)
                {
                    _itemLiveNum = props.SurplusNum;
                    _itemLiveMaxNum = itemInfo.EffectNum;
                }
            }


            //加量,领土战不能加血wuzf)
            if (ContextUser.UserStatus != UserStatus.CountryCombat)
            {
                UserHelper.GetGeneralLife(ContextUser.UserID);
            }
          


            //精力恢复
            List<UserQueue> energyQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.EnergyHuiFu);
            if (energyQueueArray.Count > 0)
            {
                UserQueue energyQueue = energyQueueArray[0];
                short energyMaxNum = (short)ConfigEnvSet.GetInt("User.MaxEnergyNum");
                int restorationDate = ConfigEnvSet.GetInt("UserQueue.EnergyRestorationDate"); //半小时
                int restorationNum = ConfigEnvSet.GetInt("UserQueue.EnergyRestorationNum"); //恢复5点

                if (energyQueue.Timing > DateTime.Now)
                {
                    energyQueue.Timing = DateTime.Now;
                }
                //原因：玩家满精力时，精力恢复累加
                int timeCount = (int)(DateTime.Now - energyQueue.Timing).TotalSeconds / restorationDate;
                if (timeCount > 0)
                {
                    short energyNum = (short)(timeCount * restorationNum);
                    if (ContextUser.EnergyNum < energyMaxNum)
                    {
                        ContextUser.EnergyNum = MathUtils.Addition(ContextUser.EnergyNum, energyNum, energyMaxNum);
                    }
                    energyQueue.Timing = DateTime.Now;
                }
                else
                {
                    if (energyMaxNum > ContextUser.EnergyNum)
                    {
                        rstore = (int)(energyQueue.Timing.AddSeconds(restorationDate) - DateTime.Now).TotalSeconds;
                        totalRstore =
                            (energyQueue.Timing.AddSeconds((energyMaxNum - ContextUser.EnergyNum) * restorationDate) -
                             DateTime.Now).TotalSeconds.ToInt();
                    }
                }

            }
            else
            {
                UserQueue queue = new UserQueue()
                {
                    QueueID = Guid.NewGuid().ToString(),
                    UserID = ContextUser.UserID,
                    QueueType = QueueType.EnergyHuiFu,
                    QueueName = QueueType.EnergyHuiFu.ToString(),
                    Timing = DateTime.Now,
                    ColdTime = 0,
                    TotalColdTime = 0,
                    IsSuspend = false,
                    StrengNum = 0
                };
                new GameDataCacheSet<UserQueue>().Add(queue);
            }


            VipLvInfo lvInfo = new ConfigCacheSet<VipLvInfo>().FindKey(MathUtils.Addition(ContextUser.VipLv, 1, int.MaxValue));
            if (lvInfo != null)
            {
                demandGold = MathUtils.Subtraction(lvInfo.PayGold, ContextUser.PayGold, 0);
                demandGold = MathUtils.Subtraction(demandGold, ContextUser.ExtGold, 0);
            }
            UserHelper.GetGameUserCombat(ContextUser.UserID);

            if ((int)(DateTime.Now - ContextUser.DailyLoginTime).TotalSeconds <= 5 && ContextUser.UserLv > 10)
            {
                isHelper = 1;
            }
            FestivalHelper.DoFestival(ContextUser);
            if (ContextUser.UserExtend != null)
            {
                if ((plotstatucID > 0 || ContextUser.TempEnergyNum == 0) && !ContextUser.UserExtend.IsBoss)
                {
                    ContextUser.TempEnergyNum = 1;
                }

                if ((plotstatucID == 0 && ContextUser.TempEnergyNum == 0) || ContextUser.UserExtend.IsBoss)
                {
                    ContextUser.UserExtend.UpdateNotify(obj =>
                    {
                        ContextUser.UserExtend.PlotStatusID = 0;
                        ContextUser.UserExtend.PlotNpcID = -1;
                        ContextUser.UserExtend.MercenarySeq = 0;
                        ContextUser.UserExtend.IsBoss = false;
                        return true;
                    });
                }

                plotstatucID = ContextUser.UserExtend.PlotStatusID;
                mercenarySeq = ContextUser.UserExtend.MercenarySeq;
                cardUserID = ContextUser.UserExtend.CardUserID;
            }

            //公会晨练结束，退出公会晨练
            int activeID = 11;
            GameActive active = new ShareCacheStruct<GameActive>().FindKey(activeID);
            if (active != null)
            {
                DateTime stratTime = active.BeginTime;
                DateTime endTime = active.BeginTime.AddMinutes(active.Minutes);
                if (ContextUser.UserLocation == Location.GuildExercise && (DateTime.Now < stratTime || DateTime.Now > endTime))
                {
                    ContextUser.UserLocation = Location.Guid;
                }
            }
            DateTime nextDate;
            FightStage stage = GuildFightCombat.GetStage(out nextDate);
            //公会战结束后
            if (stage == FightStage.Apply && ContextUser.UserStatus == UserStatus.FightCombat)
            {
                ContextUser.UserStatus = UserStatus.Normal;
            }
            battleNum = EmbattleHelper.CurrEmbattle(ContextUser.UserID, true).Count;
            totalBattle = EmbattleHelper.CurrEmbattle(ContextUser.UserID, false).Count;
            var userEmbattleList = EmbattleHelper.CurrEmbattle(ContextUser.UserID, true);
            foreach (var userEmbattle in userEmbattleList)
            {
                _talPriority = MathUtils.Addition(_talPriority, PriorityHelper.GeneralTotalPriority(ContextUser.UserID, userEmbattle.GeneralID));
            }
            functionList = new GameDataCacheSet<UserFunction>().FindAll(ContextUser.UserID);

            // 精灵祝福
            if (ContextUser != null)
            {
                if (MathUtils.SqlMinDate == ContextUser.WizardDate)  // 玩家第一次进入
                {
                    ContextUser.WizardDate = DateTime.Now;
                    ContextUser.WizardNum = 1;
                }
                else
                {

                    double diffHours = (DateTime.Now - ContextUser.WizardDate).TotalHours;
                    if (diffHours >= 1)
                    {
                        ContextUser.WizardNum = MathUtils.Addition(ContextUser.WizardNum, Convert.ToInt32(diffHours), 3);
                        ContextUser.WizardDate = DateTime.Now;
                    }
                }
            }

            // 未读信件的数量
            TjxMailService tjxMailService = new TjxMailService(ContextUser);
            tjxMailService.GetMail(out unReadCount);
            return true;
        }
    }
}