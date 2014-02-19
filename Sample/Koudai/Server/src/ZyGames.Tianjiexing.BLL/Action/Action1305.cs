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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1305_获取命运水晶接口
    /// </summary>
    public class Action1305 : BaseAction
    {
        private int huntingID = 0;
        private int ops = 0;
        private CrystalInfo crystal = null;
        private List<UserLight> lightArray = new List<UserLight>();
        private const int errorNum = 1000; //一键猎命出错的编号

        public Action1305(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1305, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(crystal == null ? 0 : crystal.CrystalID);
            PushIntoStack(crystal == null ? string.Empty : crystal.HeadID.ToNotNullString());
            PushIntoStack(lightArray.Count);
            foreach (UserLight light in lightArray)
            {
                ProbabilityInfo probability = new ConfigCacheSet<ProbabilityInfo>().FindKey(light.HuntingID) ?? new ProbabilityInfo();
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(light.HuntingID);
                dsItem.PushIntoStack(probability.HuntingName.ToNotNullString());
                dsItem.PushIntoStack(light.IsLight);

                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("HuntingID", ref huntingID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserHelper.GetUserLightOpen(ContextUser.UserID);
            short opType = 0;
            int huntingID2 = 0;
            if (huntingID < 1005)
            {
                huntingID2 = MathUtils.Addition(huntingID, 1, int.MaxValue);
            }
            else
            {
                huntingID2 = 1001; //huntingID;
            }
            UserHelper.ChechDailyRestrain(ContextUser.UserID);

            if (!CrystalHelper.CheckAllowCrystall(ContextUser))
            {
                ErrorCode = 1000;
                ErrorInfo = LanguageManager.GetLang().St1305_FateBackpackFull;
                return false;
            }
            else
            {
                if (!CrystalHelper.CheckAllowCrystall(ContextUser))
                {
                    ErrorCode = errorNum;
                    ErrorInfo = LanguageManager.GetLang().St1305_FateBackpackFull;
                    return false;
                }
            }

            ProbabilityInfo probabilityInfo = null;
            if (ops == 1)
            {
                #region
                opType = 1;
                UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
                probabilityInfo = new ConfigCacheSet<ProbabilityInfo>().FindKey(huntingID); //当前猎命人物的概率
                if (probabilityInfo == null)
                {
                    return false;
                }
                ProbabilityInfo probability1 = new ConfigCacheSet<ProbabilityInfo>().FindKey(huntingID2);
                if (userRestrain != null && userRestrain.Funtion2 >= VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.MianFeiLieMing) && DateTime.Now.Date == userRestrain.RefreshDate.Date)
                {
                    if (probabilityInfo.Price > ContextUser.GameCoin)
                    {
                        ErrorCode = errorNum;
                        ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                        return false;
                    }
                }
                //暑期第三弹
                if (huntingID2 == 1001 && !NoviceHelper.IsGianCrystalPack(ContextUser))
                {
                    ErrorCode = errorNum;
                    ErrorInfo = LanguageManager.GetLang().St1305_BeiBaoBackpackFull;
                    return false;
                }
                var lightCacheSet = new GameDataCacheSet<UserLight>();
                if (huntingID != 1001)
                {
                    UserLight userLight1 = lightCacheSet.FindKey(ContextUser.UserID, huntingID);
                    if (userLight1.IsLight == 2)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        return false;
                    }

                    if (userLight1.IsLight == 1)
                    {
                        userLight1.IsLight = 2;
                        //userLight1.Update();
                    }
                }
                UserLight userLight = new GameDataCacheSet<UserLight>().FindKey(ContextUser.UserID, huntingID2);
                if (RandomUtils.IsHit(probability1.Light))
                {
                    ErrorCode = ErrorCode;
                    ErrorInfo = probability1.HuntingName;

                    if (userLight != null)
                    {
                        userLight.IsLight = 1;
                        //userLight.Update();
                    }
                    else
                    {
                        userLight = new UserLight()
                        {
                            UserID = ContextUser.UserID,
                            HuntingID = huntingID2,
                            IsLight = 1
                        };
                        lightCacheSet.Add(userLight);
                    }
                }

                if (userRestrain != null)
                {
                    if (userRestrain.Funtion2 >= VipHelper.GetVipUseNum(ContextUser.VipLv, RestrainType.MianFeiLieMing) && DateTime.Now.Date == userRestrain.RefreshDate.Date)
                    {
                        ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, probabilityInfo.Price, 0);
                        //ContextUser.Update();
                    }
                    else
                    {
                        userRestrain.Funtion2 = MathUtils.Addition(userRestrain.Funtion2, 1, int.MaxValue);
                        //userRestrain.Update();
                    }
                }
                else
                {
                    ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, probabilityInfo.Price, 0);
                    //ContextUser.Update();
                }

                lightArray = new GameDataCacheSet<UserLight>().FindAll(ContextUser.UserID);
                #endregion
            }
            else if (ops == 2)
            {
                #region
                opType = 2;
                if (ContextUser.VipLv < 5)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                    return false;
                }

                probabilityInfo = new ConfigCacheSet<ProbabilityInfo>().FindKey(huntingID); //当前猎命人物的概率
                if (probabilityInfo == null)
                {
                    return false;
                }

                if (ContextUser.GoldNum < probabilityInfo.Price)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                UserLight uLightInfo = new GameDataCacheSet<UserLight>().FindKey(ContextUser.UserID, probabilityInfo.GoldHunting);
                if (uLightInfo != null && uLightInfo.IsLight == 1)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1305_HuntingIDLight;
                    return false;
                }
                else if (uLightInfo != null && (uLightInfo.IsLight == 2 || uLightInfo.IsLight == 0))
                {
                    uLightInfo.IsLight = 1;
                }
                else
                {
                    uLightInfo = new UserLight()
                    {
                        UserID = ContextUser.UserID,
                        HuntingID = probabilityInfo.GoldHunting,
                        IsLight = 1
                    };
                    new GameDataCacheSet<UserLight>().Add(uLightInfo);
                }

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, probabilityInfo.Price, int.MaxValue);
                lightArray = new GameDataCacheSet<UserLight>().FindAll(ContextUser.UserID);
                #endregion
            }

            //每种品质的概率
            double[] probabilityArray2 = { (double)probabilityInfo.Gray, (double)probabilityInfo.Green, (double)probabilityInfo.Blue, (double)probabilityInfo.Purple, (double)probabilityInfo.Yellow, (double)probabilityInfo.Red };
            int index2 = RandomUtils.GetHitIndex(probabilityArray2);
            CrystalQualityType qualityType = (CrystalQualityType)Enum.Parse(typeof(CrystalQualityType), (index2 + 1).ToString());
            List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(u => u.CrystalQuality == qualityType && u.DemandLv <= ContextUser.UserLv);
            if (crystalArray2.Count > 0)
            {
                int randomNum = RandomUtils.GetRandom(0, crystalArray2.Count);
                crystal = new ConfigCacheSet<CrystalInfo>().FindKey(crystalArray2[randomNum].CrystalID);
                if (crystal != null && crystal.CrystalQuality == CrystalQualityType.Gray)
                {
                    //wuzf修改 8-15 灰色放在临时背包不存DB
                    CrystalHelper.AppendGrayCrystal(ContextUser, crystal.CrystalID);
                }
                else if (crystal != null)
                {
                    var package = UserCrystalPackage.Get(Uid);

                    UserCrystalInfo userCrystal = new UserCrystalInfo()
                    {
                        UserCrystalID = Guid.NewGuid().ToString(),
                        CrystalID = crystal.CrystalID,
                        CrystalLv = 1,
                        CurrExprience = 0,
                        GeneralID = 0,
                        IsSale = 1,
                        CreateDate = DateTime.Now
                    };
                    package.SaveCrystal(userCrystal);
                    UserLogHelper.AppenCtystalLog(ContextUser.UserID, opType, crystal.CrystalID, probabilityInfo.Price, probabilityInfo.Price, null, 1, 0);
                    //高品质聊天通知);
                    if (crystal.CrystalQuality >= CrystalQualityType.Yellow)
                    {
                        var cacheChat = new TjxChatService();
                        string content = string.Format(LanguageManager.GetLang().St1305_HighQualityNotice,
                            ContextUser.NickName,
                           CrystalHelper.GetQualityName(crystal.CrystalQuality),
                            crystal.CrystalName
                            );
                        cacheChat.SystemSend(ChatType.World, content);
                    }
                }
            }

            //日常任务-猎命
            TaskHelper.TriggerDailyTask(Uid, 4009);
            return true;
        }

        private static bool IsGridNumFull(GameUser user)
        {
            int itemid = 5019;
            var userItemsArray = UserItemHelper.GetItems(user.UserID).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
            if (userItemsArray.Count < user.GridNum)
            {
                UserItemHelper.AddUserItem(user.UserID, itemid, 1);
                return true;
            }
            return false;
        }
    }
}