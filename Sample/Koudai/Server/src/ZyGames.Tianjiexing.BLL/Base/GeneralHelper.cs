using System;
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Tianjiexing.Model.ConfigModel;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class GeneralHelper
    {
        public static short _currMaxLv = GameConfigSet.CurrMaxLv.ToShort(); //玩家最大等级

        /// <summary>
        /// 将星佣兵完成任务后奖励物品名称
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public static string PrizeItemName(PrizeInfo prize)
        {
            string name = string.Empty;
            switch (prize.Type)
            {
                case RewardType.CrystalId:
                    CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
                    name = crystal == null ? string.Empty : crystal.CrystalName;
                    break;
                case RewardType.Item:
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
                    name = itemInfo == null ? string.Empty : itemInfo.ItemName;
                    break;
                case RewardType.Spare:
                    SparePartInfo sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(prize.ItemID);
                    name = sparePartInfo == null ? string.Empty : sparePartInfo.Name;
                    break;
                case RewardType.CrystalType:
                    name = string.Format(LanguageManager.GetLang().St_Crystal,
                                         CrystalHelper.GetQualityName(prize.CrystalType.ToEnum<CrystalQualityType>()),
                                         string.Empty);
                    break;

            }
            return name;
        }

        /// <summary>
        /// 将星佣兵是否可招募
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static short IsGeneralRecruit(string userID, int generalID)
        {
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (general != null)
            {
                return 2;
            }
            var storyList = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.GeneralID == generalID && m.TaskType == TaskType.General);
            foreach (var info in storyList)
            {
                UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(userID, info.TaskID);
                if (userTask != null && userTask.TaskState == TaskState.Close)
                {
                    continue;
                }
                else
                {
                    return 0;
                }
            }
            return 1;
        }

        /// <summary>
        /// 任务是否完成
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public static short IsComplete(GameUser user, StoryTaskInfo taskInfo)
        {
            UserTask userTask = new GameDataCacheSet<UserTask>().FindKey(user.UserID, taskInfo.TaskID);
            if (userTask != null && userTask.TaskState == TaskState.Close)
            {
                return 3;
            }
            if (taskInfo.TaskLv > user.UserLv)
            {
                return 0;
            }
            if (user.UserLv >= taskInfo.TaskLv)
            {
                int collectNum = TrumpHelper.GetUserItemNum(user.UserID, taskInfo.TargetItemID);
                if (collectNum >= taskInfo.TargetItemNum)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 替换佣兵功能开启相关数值
        /// </summary>
        /// <param name="user"></param>
        public static void OpenReplaceGeneral(GameUser user)
        {
            var cachePrize = new ShareCacheStruct<UserTakePrize>();
            string generalPrize = ConfigEnvSet.GetString("General.ReplaceGeneralPrize");
            string content = LanguageManager.GetLang().St1901_OpenGeneralReplace;
            List<PrizeInfo> prizeList = new List<PrizeInfo>();
            if (!string.IsNullOrEmpty(generalPrize))
            {
                prizeList = JsonUtils.Deserialize<List<PrizeInfo>>(generalPrize);
            }
            foreach (PrizeInfo info in prizeList)
            {
                var takePrize = GetUserTake(info, user.UserID);
                cachePrize.Add(takePrize);
            }
            var chatService = new TjxChatService(user);
            chatService.SystemSendWhisper(user, content);
        }

        /// <summary>
        /// 检查替换佣兵功能是否开启
        /// </summary>
        /// <param name="user"></param>
        public static void StotyTaskFunction(GameUser user)
        {
            var storyTaskList = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.FunctionEnum == FunctionEnum.ReplaceGeneral && m.TaskType == TaskType.Master);
            if (storyTaskList.Count > 0)
            {
                var userTask = new GameDataCacheSet<UserTask>().FindKey(user.UserID, storyTaskList[0].TaskID);
                var userFunction = new GameDataCacheSet<UserFunction>().FindKey(user.UserID, FunctionEnum.ReplaceGeneral);
                if (userTask != null && userFunction == null)
                {
                    TaskHelper.EnableFunction(user, FunctionEnum.ReplaceGeneral);
                }
            }
        }

        /// <summary>
        /// 替换佣兵位置
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="magicID"></param>
        /// <returns></returns>
        public static short ReplacePostion(string userID, int magicID)
        {
            short replacePostion = 0;
            UserMagic userMagic = new GameDataCacheSet<UserMagic>().FindKey(userID, magicID);
            if (userMagic != null)
            {
                MagicLvInfo magicLv = new ConfigCacheSet<MagicLvInfo>().FindKey(userMagic.MagicID, userMagic.MagicLv);
                if (magicLv != null && magicLv.ReplacePostion > 0)
                {
                    replacePostion = magicLv.ReplacePostion.ToShort();
                }
            }
            return replacePostion;
        }

        /// <summary>
        /// 奖励配置
        /// </summary>
        /// <param name="user"></param>
        public static UserTakePrize GetUserTake(PrizeInfo prize, string userID)
        {
            UserTakePrize userPrize = new UserTakePrize();
            userPrize.CreateDate = DateTime.Now;
            userPrize.ID = Guid.NewGuid().ToString();
            userPrize.UserID = Convert.ToInt32(userID);
            userPrize.MailContent = LanguageManager.GetLang().St_SummerThreeGameCoinNotice.Substring(0, 5);
            userPrize.IsTasked = false;
            userPrize.TaskDate = MathUtils.SqlMinDate;
            userPrize.ItemPackage = string.Empty;
            userPrize.SparePackage = string.Empty;
            userPrize.CrystalPackage = string.Empty;
            userPrize.OpUserID = 10000;
            switch (prize.Type)
            {
                case RewardType.GameGoin:
                    userPrize.GameCoin = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_GameCoin, prize.Num) + ",";
                    break;
                case RewardType.Obtion:
                    userPrize.ObtainNum = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_ObtionNum, prize.Num) + ",";
                    break;
                case RewardType.ExpNum:
                    userPrize.ExpNum = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_ExpNum, prize.Num) + ",";
                    break;
                case RewardType.EnergyNum:
                    userPrize.EnergyNum = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_EnergyNum, prize.Num) + ",";
                    break;
                case RewardType.Experience:
                    break;
                case RewardType.Gold:
                    userPrize.Gold = prize.Num;
                    userPrize.MailContent += string.Format(LanguageManager.GetLang().St_GiftGoldNum, prize.Num) + ",";
                    break;
                case RewardType.Item:
                    userPrize.ItemPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(prize.ItemID);
                    if (itemInfo != null)
                    {
                        userPrize.MailContent += string.Format("{0}*{1}", itemInfo.ItemName, prize.Num) + ",";
                    }
                    break;
                case RewardType.CrystalType:
                    //List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(CrystalInfo.Index_CrystalQuality, prize.CrystalType);
                    //userPrize.CrystalPackage = string.Format("{0}={1}={2}", itemID, prize.UserLv, itemNum);
                    break;
                case RewardType.CrystalId:

                    userPrize.CrystalPackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(prize.ItemID);
                    if (crystal != null)
                    {
                        userPrize.MailContent += string.Format("{0}*{1}", crystal.CrystalName, prize.Num) + ",";
                    }
                    break;
                case RewardType.Spare:
                    userPrize.SparePackage = string.Format("{0}={1}={2}", prize.ItemID, prize.UserLv, prize.Num);
                    SparePartInfo spare = new ConfigCacheSet<SparePartInfo>().FindKey(prize.ItemID);
                    if (spare != null)
                    {
                        userPrize.MailContent += string.Format("{0}*{1}", spare.Name, prize.Num) + ",";
                    }
                    break;
                default:
                    break;
            }
            userPrize.MailContent = userPrize.MailContent.TrimEnd(',');
            return userPrize;
        }

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
        /// <summary>
        /// 是否招募突破 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <param name="recruitNum">还需多少灵魂可招募或突破</param>
        /// <returns></returns>
        public static short IsGeneralRecruit(string userID, int generalID, out int recruitNum, out int demandNum)
        {
            short isRecruit = 0;
            recruitNum = 0;
            demandNum = 0;
            var general = new ConfigCacheSet<GeneralInfo>().Find(s => s.SoulID == generalID);
            if (general != null)
            {
                demandNum = general.DemandNum;
                recruitNum = demandNum;
                var soulGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
                var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, general.GeneralID);
                if (soulGeneral == null)
                {
                    demandNum = general.DemandNum;
                    recruitNum = demandNum;
                    return isRecruit;
                }
                if (userGeneral == null)
                {
                    if (soulGeneral.AtmanNum >= recruitNum)
                    {
                        recruitNum = 0;
                        isRecruit = 1;
                    }
                    else
                    {
                        recruitNum = MathUtils.Subtraction(general.DemandNum, soulGeneral.AtmanNum);
                    }
                }
                else
                {
                    if (soulGeneral.AtmanNum >= recruitNum)
                    {
                        recruitNum = 0;
                        isRecruit = 3;
                    }
                    else
                    {
                        recruitNum = MathUtils.Subtraction(general.DemandNum, soulGeneral.AtmanNum);
                        isRecruit = 2;
                    }
                }
            }
            return isRecruit;
        }

        /// <summary>
        /// 招募剩余次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="freenum"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int SurplusNum(string userID, int freenum, RecruitType type)
        {
            int surNum = 0;
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null && dailyRestrain.RefreshDate.Date == DateTime.Now.Date && dailyRestrain.UserExtend != null)
            {
                if (type == RecruitType.ShiLiTiaoYi)
                {
                    surNum = dailyRestrain.UserExtend.ShiLiTiaoYi;
                }
                else if (type == RecruitType.BaiLiTiaoYi)
                {
                    surNum = dailyRestrain.UserExtend.BaiLiTiaoYi;
                }
                else if (type == RecruitType.Golden)
                {
                    surNum = dailyRestrain.UserExtend.Golden;
                }
            }
            return MathUtils.Subtraction(freenum, surNum);
        }

        /// <summary>
        /// 修改招募次数
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="freenum"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void UpdateDailyRecruitNum(string userID, RecruitType type)
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);
            if (dailyRestrain != null)
            {
                if (dailyRestrain.UserExtend == null)
                {
                    dailyRestrain.UserExtend = new DailyUserExtend();
                }
                if (dailyRestrain.RefreshDate.Date != DateTime.Now.Date)
                {
                    dailyRestrain.UserExtend.ShiLiTiaoYi = 0;
                    dailyRestrain.UserExtend.BaiLiTiaoYi = 0;
                    dailyRestrain.UserExtend.Golden = 0;
                    dailyRestrain.RefreshDate = DateTime.Now;
                }
                if (type == RecruitType.ShiLiTiaoYi)
                {
                    dailyRestrain.UserExtend.UpdateNotify(
                        obj =>
                        {
                            dailyRestrain.UserExtend.ShiLiTiaoYi = MathUtils.Addition(dailyRestrain.UserExtend.ShiLiTiaoYi, 1);
                            return true;
                        });

                }
                else if (type == RecruitType.BaiLiTiaoYi)
                {
                    dailyRestrain.UserExtend.UpdateNotify(
                       obj =>
                       {
                           dailyRestrain.UserExtend.BaiLiTiaoYi = MathUtils.Addition(dailyRestrain.UserExtend.BaiLiTiaoYi, 1);
                           return true;
                       });
                }
                else if (type == RecruitType.Golden)
                {
                    dailyRestrain.UserExtend.UpdateNotify(
                       obj =>
                       {
                           dailyRestrain.UserExtend.Golden = MathUtils.Addition(dailyRestrain.UserExtend.Golden, 1);
                           return true;
                       });
                }
            }
        }

        /// <summary>
        /// 是否有冷却时间
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recruitRule"></param>
        /// <returns></returns>
        public static bool GeneralRecruitColdTime(string userID, RecruitType recruitType)
        {
            QueueType queueType = QueueType.ShiLiTiaoYi;
            if (recruitType == RecruitType.ShiLiTiaoYi)
            {
                queueType = QueueType.ShiLiTiaoYi;
            }
            else if (recruitType == RecruitType.BaiLiTiaoYi)
            {
                queueType = QueueType.BaiLiTiaoYi;
            }
            else if (recruitType == RecruitType.Golden)
            {
                queueType = QueueType.Golden;
            }
            UserQueue userQueue = new GameDataCacheSet<UserQueue>().Find(userID, s => s.QueueType == queueType);
            if (userQueue != null && userQueue.DoRefresh() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 佣兵招募冷却时间
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="recruitRule"></param>
        /// <returns></returns>
        public static void UpdateRecruitColdTime(string userID, RecruitRule recruitRule)
        {
            QueueType queueType = QueueType.ShiLiTiaoYi;
            if (recruitRule.RecruitType == RecruitType.ShiLiTiaoYi.ToInt())
            {
                queueType = QueueType.ShiLiTiaoYi;
            }
            else if (recruitRule.RecruitType == RecruitType.BaiLiTiaoYi.ToInt())
            {
                queueType = QueueType.BaiLiTiaoYi;
            }
            else if (recruitRule.RecruitType == RecruitType.Golden.ToInt())
            {
                queueType = QueueType.Golden;
            }
            var cacheSet = new GameDataCacheSet<UserQueue>();
            UserQueue userQueue = cacheSet.Find(userID, s => s.QueueType == queueType);
            if (userQueue == null)
            {
                userQueue = new UserQueue();
                userQueue.QueueID = Guid.NewGuid().ToString();
                userQueue.UserID = userID;
                userQueue.QueueType = queueType;
                cacheSet.Add(userQueue, userID.ToInt());
            }
            userQueue.QueueName = queueType.ToString();
            userQueue.Timing = DateTime.Now;
            userQueue.TotalColdTime = recruitRule.CodeTime;
            userQueue.ColdTime = recruitRule.CodeTime;
            userQueue.StrengNum = 1;
            userQueue.IsSuspend = false;
        }

        /// <summary>
        /// 队列剩余时间
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int UserQueueCodeTime(string userID, RecruitType type)
        {
            QueueType queueType = QueueType.Nomal;
            if (type == RecruitType.ShiLiTiaoYi)
            {
                queueType = QueueType.ShiLiTiaoYi;
            }
            else if (type == RecruitType.BaiLiTiaoYi)
            {
                queueType = QueueType.BaiLiTiaoYi;
            }
            else if (type == RecruitType.Golden)
            {
                queueType = QueueType.Golden;
            }
            UserQueue userQueue = new GameDataCacheSet<UserQueue>().Find(userID, s => s.QueueType == queueType);
            if (userQueue != null)
            {
                return userQueue.DoRefresh();
            }
            return 0;
        }

        /// <summary>
        /// 传承类型信息
        /// </summary>
        /// <returns></returns>
        public static OpsInfo HeritageOpsInfo(int opsID)
        {
            OpsInfo opsInfos = null;
            if (!string.IsNullOrEmpty(GameConfigSet.HeritageList))
            {
                var opsInfosList = JsonUtils.Deserialize<List<OpsInfo>>(GameConfigSet.HeritageList);
                if (opsInfosList.Count > 0 && opsID <= 0)
                {
                    opsInfos = opsInfosList[0];
                }
                else
                {
                    opsInfos = opsInfosList.Find(m => m.Type == opsID);
                }
            }
            return opsInfos;
        }

        /// <summary>
        /// 传承人或被传承人、传承类型选择
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalHeritagesList"></param>
        /// <param name="type"></param>
        public static void HeritageGeneral(GameUser user, int type)
        {
            if (user.HeritageList.Count == 0)
            {
                return;
            }
            GeneralHeritage heritage = user.HeritageList.Find(m => m.Type == HeritageType.Heritage);
            GeneralHeritage hGernal = user.HeritageList.Find(m => m.Type == HeritageType.IsHeritage);
            if (heritage == null || hGernal == null)
            {
                return;
            }
            var cacheSet = new GameDataCacheSet<UserGeneral>();
            UserGeneral ugeneral = cacheSet.FindKey(user.UserID, heritage.GeneralID);
            UserGeneral general = cacheSet.FindKey(user.UserID, hGernal.GeneralID);
            if (ugeneral == null || general == null)
            {
                return;
            }
            heritage.GeneralLv = ugeneral.GeneralLv;
            OpsInfo info = HeritageOpsInfo(type);
            if (info != null)
            {
                int generLv = (heritage.GeneralLv * info.Num).ToFloorInt(); //().ToShort();
                if (generLv < general.GeneralLv)
                {
                    generLv = general.GeneralLv;
                }
                hGernal.GeneralLv = generLv.ToShort();
                hGernal.opsType = type;
            }
        }

        /// <summary>
        /// 佣兵加经验
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="experience"></param>
        public static void UserGeneralExp(string userID, int generalID, int experience)
        {
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, generalID);
            if (userGeneral != null)
            {
                UserHelper.TriggerGeneral(userGeneral, experience);
            }
            else
            {
                UserMagic userMagic = new GameDataCacheSet<UserMagic>().Find(userID,
                                                                             s =>
                                                                             s.IsEnabled &&
                                                                             s.MagicType == MagicType.MoFaZhen);
                if (userMagic != null)
                {
                    List<UserEmbattle> userEmbattleArray = new GameDataCacheSet<UserEmbattle>().FindAll(userID, m => m.MagicID == userMagic.MagicID);
                    foreach (UserEmbattle embattle in userEmbattleArray)
                    {
                        UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(userID, embattle.GeneralID);
                        if (general != null)
                        {
                            UserHelper.TriggerGeneral(general, experience);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 佣兵升级加升级属性
        /// </summary>
        /// <param name="general"></param>
        public static void GeneralUpgradeproperty(UserGeneral general)
        {
            GeneralInfo generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(general.GeneralID);
            if (generalInfo != null && generalInfo.Mature.Count > 0)
            {
                foreach (var mature in generalInfo.Mature)
                {
                    UpdateGeneralPro(general, mature.AbilityType, mature.AbilityValue);
                }
            }
        }

        /// <summary>
        /// 升级加属性
        /// </summary>
        /// <param name="general"></param>
        /// <param name="property"></param>
        public static void UpdateGeneralPro(UserGeneral general, AbilityType type, decimal num)
        {
            if (type == AbilityType.Potential)
            {
                general.Potential = MathUtils.Addition(general.Potential, num).ToInt();
            }
            var property = general.Attribute.Find(s => s.AbilityType == type);
            if (property != null)
            {
                general.Attribute.UpdateNotify(obj =>
                {
                    property.AbilityValue = MathUtils.Addition(property.AbilityValue, num);
                    return true;
                });
                //property.UpdateNotify(obj =>
                //{
                //    property.AbilityValue = num;
                //    return true;
                //});
            }
            else
            {
                property = new GeneralProperty();
                property.AbilityType = type;
                property.AbilityValue = num;
                general.Attribute.Add(property);
            }
        }


        /// <summary>
        /// 佣兵升级
        /// </summary>
        /// <param name="userGeneral"></param>
        public static void GeneralUp(UserGeneral userGeneral)
        {

            var cacheSetGeneralEscalate = new ConfigCacheSet<GeneralEscalateInfo>();
            var GeneralEscalateList = cacheSetGeneralEscalate.FindAll(s => s.GeneralType == GeneralType.YongBing && s.UpExperience <= userGeneral.CurrExperience);
            short generalLv = userGeneral.GeneralLv;
            GeneralEscalateInfo generalEscalateInfo = new GeneralEscalateInfo();
            GeneralEscalateList.ForEach(item =>
            {
                if (item.GeneralLv > generalLv)
                {
                    generalLv = item.GeneralLv;
                    generalEscalateInfo = item;
                }
            });
            if (generalLv > userGeneral.GeneralLv)
            {
                userGeneral.GeneralLv = generalLv;

                userGeneral.CurrExperience = MathUtils.Subtraction(userGeneral.CurrExperience, generalEscalateInfo.UpExperience);

            }
        }

        /// <summary>
        /// 招募佣兵或招募灵魂
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalInfo"></param>
        /// <param name="type"></param>
        public static void UpdateUserGeneral(string userID, GeneralInfo generalInfo, GeneralType type, int num)
        {
            var cacheSet = new GameDataCacheSet<UserGeneral>();
            UserGeneral userGeneral = null;
            if (type == GeneralType.YongBing)
            {
                userGeneral = cacheSet.FindKey(userID, generalInfo.GeneralID);
                if (userGeneral == null)
                {
                    CreateUserGeneral(userID, generalInfo, type, num);
                }
                else
                {
                    UpdateUserGeneral(userID, generalInfo, GeneralType.Soul, num);
                }
            }
            else if (type == GeneralType.Soul)
            {
                userGeneral = cacheSet.FindKey(userID, generalInfo.SoulID);
                if (userGeneral == null)
                {
                    CreateUserGeneral(userID, generalInfo, type, num);
                }
                else
                {
                    userGeneral.AtmanNum = MathUtils.Addition(userGeneral.AtmanNum, num);
                }
            }
        }

        /// <summary>
        /// 创建佣兵或佣兵灵魂
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="general"></param>
        /// <param name="type"></param>
        /// <param name="num"></param>
        public static void CreateUserGeneral(string userID, GeneralInfo general, GeneralType type, int num)
        {
            var cacheSet = new GameDataCacheSet<UserGeneral>();
            int generalID = 0;
            CareerInfo careerInfo = null;
            if (type == GeneralType.YongBing)
            {
                generalID = general.GeneralID;
                careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(general.CareerID);
            }
            else if (type == GeneralType.Soul)
            {
                generalID = general.SoulID;
            }
            if (generalID > 0)
            {
                UserGeneral userGeneral = new UserGeneral();
                userGeneral.UserID = userID;
                userGeneral.GeneralID = generalID;
                userGeneral.GeneralName = general.GeneralName;
                userGeneral.HeadID = general.HeadID;
                userGeneral.PicturesID = general.PicturesID;
                userGeneral.GeneralLv = 1;
                userGeneral.GeneralType = type;
                userGeneral.CareerID = general.CareerID;
                userGeneral.PowerNum = general.PowerNum;
                userGeneral.SoulNum = general.SoulNum;
                userGeneral.IntellectNum = general.IntellectNum;
                userGeneral.TrainingPower = 0;
                userGeneral.TrainingSoul = 0;
                userGeneral.TrainingIntellect = 0;
                userGeneral.AbilityID = general.AbilityID;
                userGeneral.Momentum = 0;
                userGeneral.Description = string.Empty;
                userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
                userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
                userGeneral.CurrExperience = 0;
                userGeneral.Experience1 = 0;
                userGeneral.Experience2 = 0;
                if (careerInfo != null)
                {
                    userGeneral.LifeNum = MathUtils.Addition(general.LifeNum, careerInfo.LifeIncreaseNum * (MathUtils.Subtraction(careerInfo.Lv, (short)1, (short)0)), int.MaxValue);
                }
                if (type == GeneralType.Soul)
                {
                    userGeneral.AtmanNum = num;
                }
                userGeneral.HeritageType = HeritageType.Normal;
                userGeneral.AbilityNum = 3;
                cacheSet.Add(userGeneral);
            }
        }

        /// <summary>
        /// 传承佣兵身上是否有装备
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static bool IsGeneralEqu(string userID, int generalID)
        {
            var package = UserItemPackage.Get(userID);
            if (package != null)
            {
                var useritemList = package.ItemPackage.FindAll(s => !s.IsRemove && s.GeneralID == generalID);
                if (useritemList.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 传承佣兵身上是否有水晶
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static bool IsGeneralCrystal(string userID, int generalID)
        {
            var package = UserCrystalPackage.Get(userID);
            if (package != null)
            {
                var usercrystalList = package.CrystalPackage.FindAll(s => s.GeneralID == generalID);
                if (usercrystalList.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 传承佣兵身上是否有魂技
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public static bool IsGeneralAbility(string userID, int generalID)
        {
            var package = new GameDataCacheSet<UserAbility>().FindKey(userID);
            if (package != null)
            {
                var generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
                int abilityID = generalInfo == null ? 0 : generalInfo.AbilityID;
                var usercrystalList = package.AbilityList.FindAll(s => s.GeneralID == generalID && s.AbilityID != abilityID);
                if (usercrystalList.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 首次招募佣兵是否完成
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static bool IsFirstRecruit(GameUser user, RecruitType rType)
        {
            if (rType == RecruitType.BaiLiTiaoYi && user.UserExtend != null && user.UserExtend.BaiLanse)
            {
                return true;
            }
            if (rType == RecruitType.Golden && user.UserExtend != null && user.UserExtend.GoldenZise)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 完成首次招募佣兵
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rType"></param>
        public static void FirstRecruitComplete(GameUser user, RecruitType rType)
        {
            if (user.UserExtend == null)
            {
                user.UserExtend = new GameUserExtend();
            }
            if (rType == RecruitType.BaiLiTiaoYi)
            {
                user.UserExtend.UpdateNotify(obj =>
                {
                    user.UserExtend.BaiLanse = true;
                    return true;
                });
            }

            if (rType == RecruitType.Golden)
            {
                user.UserExtend.UpdateNotify(obj =>
                {
                    user.UserExtend.GoldenZise = true;
                    return true;
                });
            }
        }
    }
}