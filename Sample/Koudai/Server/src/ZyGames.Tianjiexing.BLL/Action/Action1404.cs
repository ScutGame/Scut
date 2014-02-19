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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.ConfigModel;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1404_佣兵邀请接口
    /// </summary>
    public class Action1404 : BaseAction
    {
        private RecruitType recruitType;
        private int soulID;
        private int currSoulID;
        private int gainNum;
        private GeneralInfo general = null;
        private GeneralType generalType = 0;

        public Action1404(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1404, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(general == null ? string.Empty : general.GeneralName.ToNotNullString());
            this.PushIntoStack(general == null ? string.Empty : general.PicturesID.ToNotNullString());
            this.PushIntoStack(general == null ? (short)0 : general.GeneralLv);
            this.PushIntoStack(generalType.ToShort());
            this.PushIntoStack(general == null ? 0 : general.LifeNum);
            this.PushIntoStack(general == null ? (short)0 : (short)general.PowerNum);
            this.PushIntoStack(general == null ? (short)0 : (short)general.SoulNum);
            this.PushIntoStack(general == null ? (short)0 : (short)general.IntellectNum);
            this.PushIntoStack(currSoulID);
            this.PushIntoStack(gainNum);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("RecruitType", ref recruitType))
            {
                httpGet.GetInt("SoulID", ref soulID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cacheSet = new GameDataCacheSet<UserGeneral>();
            if (recruitType == RecruitType.SoulRecruit)
            {
                general = new ConfigCacheSet<GeneralInfo>().FindKey(soulID);
                if (general == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                UserGeneral userGeneral = cacheSet.FindKey(ContextUser.UserID, soulID);
                if (soulID == 0 || userGeneral != null)
                {
                    UserItemHelper.AddUserItem(ContextUser.UserID, general.ItemID, 1);
                }
                else
                {
                    userGeneral = new UserGeneral();
                    userGeneral.UserID = ContextUser.UserID;
                    cacheSet.Add(userGeneral);
                    userGeneral.GeneralID = general.GeneralID;
                    userGeneral.GeneralName = general.GeneralName;
                    userGeneral.HeadID = general.HeadID;
                    userGeneral.PicturesID = general.PicturesID;
                    userGeneral.GeneralLv = (short)general.GeneralLv;
                    userGeneral.LifeNum = general.LifeNum;
                    userGeneral.GeneralType = GeneralType.YongBing;
                    userGeneral.CareerID = general.CareerID;
                    userGeneral.PowerNum = general.PowerNum;
                    userGeneral.SoulNum = general.SoulNum;
                    userGeneral.IntellectNum = general.IntellectNum;
                    userGeneral.TrainingPower = 0;
                    userGeneral.TrainingSoul = 0;
                    userGeneral.TrainingIntellect = 0;
                    userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
                    userGeneral.AbilityID = general.AbilityID;
                    userGeneral.Momentum = 0;
                    userGeneral.Description = general.Description;
                    userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
                    userGeneral.CurrExperience = 0;
                    userGeneral.Experience1 = 0;
                    userGeneral.Experience2 = 0;
                }
            }
            else
            {
                RecruitRule recruitRule = new ConfigCacheSet<RecruitRule>().FindKey(recruitType);
                if (recruitRule == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                int surplusNum = GeneralHelper.SurplusNum(ContextUser.UserID, recruitRule.FreeNum,
                                                          recruitRule.RecruitType.ToEnum<RecruitType>());
                if (surplusNum > 0 && !GeneralHelper.GeneralRecruitColdTime(ContextUser.UserID, recruitType))
                {
                    GeneralHelper.UpdateDailyRecruitNum(ContextUser.UserID, recruitType);
                    GeneralHelper.UpdateRecruitColdTime(ContextUser.UserID, recruitRule);
                }
                else
                {
                    if (ContextUser.GoldNum < recruitRule.GoldNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, recruitRule.GoldNum);
                }

                CacheList<RecruitInfo> recruitInfos = recruitRule.GeneralQuality;
                double[] probability = new double[recruitInfos.Count];
                for (int i = 0; i < recruitInfos.Count; i++)
                {
                    probability[i] = (double)recruitInfos[i].Probability;
                }
                int index2 = RandomUtils.GetHitIndex(probability);
                GeneralQuality quality = recruitInfos[index2].Quality;
                var generalList = new ConfigCacheSet<GeneralInfo>().FindAll(s => s.GeneralQuality == quality);
                if (generalList.Count > 0)
                {
                    int indexradom = RandomUtils.GetRandom(0, generalList.Count);
                    if (indexradom < 0 || indexradom >= generalList.Count)
                    {
                        return false;
                    }
                    general = generalList[indexradom];
                    
                    UserGeneral userGeneral = cacheSet.FindKey(ContextUser.UserID, general.GeneralID);
                    
                    if (userGeneral != null && userGeneral.GeneralStatus != GeneralStatus.YinCang)
                    {
                        currSoulID = general.SoulID;
                        gainNum = general.DemandNum;
                        userGeneral = cacheSet.FindKey(ContextUser.UserID, currSoulID);
                        if (userGeneral != null)
                        {
                            userGeneral.AtmanNum = MathUtils.Addition(userGeneral.AtmanNum, gainNum);
                            generalType = GeneralType.Soul;
                        }
                        else
                        {
                            userGeneral = new UserGeneral();
                            userGeneral.UserID = ContextUser.UserID;
                            userGeneral.GeneralID = currSoulID;
                            generalType = GeneralType.Soul;
                            cacheSet.Add(userGeneral);
                            UserAbilityHelper.AddUserAbility(general.AbilityID, ContextUser.UserID.ToInt(), general.GeneralID,1);
                        }
                    }
                    else if (userGeneral == null)
                    {
                        userGeneral = new UserGeneral();
                        userGeneral.UserID = ContextUser.UserID;
                        userGeneral.GeneralID = general.GeneralID;
                        generalType = GeneralType.YongBing;
                        cacheSet.Add(userGeneral);
                        UserAbilityHelper.AddUserAbility(general.AbilityID, ContextUser.UserID.ToInt(), general.GeneralID,1);
                    }
                    userGeneral.GeneralName = general.GeneralName;
                    userGeneral.HeadID = general.HeadID;
                    userGeneral.PicturesID = general.PicturesID;
                    userGeneral.GeneralLv = (short)general.GeneralLv;
                    userGeneral.LifeNum = general.LifeNum;
                    userGeneral.GeneralType = generalType;
                    userGeneral.CareerID = general.CareerID;
                    userGeneral.PowerNum = general.PowerNum;
                    userGeneral.SoulNum = general.SoulNum;
                    userGeneral.IntellectNum = general.IntellectNum;
                    userGeneral.TrainingPower = 0;
                    userGeneral.TrainingSoul = 0;
                    userGeneral.TrainingIntellect = 0;
                    userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
                    userGeneral.AbilityID = general.AbilityID;
                    userGeneral.Momentum = 0;
                    userGeneral.Description = general.Description;
                    userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
                    userGeneral.CurrExperience = 0;
                    userGeneral.Experience1 = 0;
                    userGeneral.Experience2 = 0;

                    //玩家抽取到蓝色和紫色佣兵时，给予系统频道提示
                    //if (recruitType != RecruitType.SoulRecruit)
                    //{
                    //    if (general.GeneralQuality.ToEnum<GeneralQuality>() == GeneralQuality.Blue || general.GeneralQuality.ToEnum<GeneralQuality>() == GeneralQuality.Purple)
                    //    {    
                    //        string content = string.Empty;
                    //        content = string.Format(LanguageManager.GetLang().St_UserGetGeneralQuality, ContextUser.NickName,recruitType,
                    //                            general.GeneralQuality.ToEnum<GeneralQuality>(), general.GeneralName);
                    //        new TjxChatService().SystemSend(ChatType.System, content);
                    //    }
                    //}
                }
            }
            return true;
        }
    }
}