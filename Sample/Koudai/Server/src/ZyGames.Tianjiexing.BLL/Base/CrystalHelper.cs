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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Lang;


namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 命运水晶帮助
    /// </summary>
    public class CrystalHelper
    {
        /// <summary>
        /// 猎命的格子容量（14个）
        /// </summary>
        private static int _crystalGridNum = ConfigEnvSet.GetInt("UserCrystal.CrystalGridNum");

        /// <summary>
        /// 检查是否能猎命
        /// </summary>
        /// <returns></returns>
        public static bool CheckAllowCrystall(GameUser user)
        {
            var package = UserCrystalPackage.Get(user.UserID);
            int currNum = 0;
            if (package != null)
            {
                currNum = package.CrystalPackage.FindAll(m => m.IsSale == 1).Count;
                currNum += user.GrayCrystalList == null ? 0 : user.GrayCrystalList.Count;
            }
            return currNum < _crystalGridNum;
        }

        /// <summary>
        /// 增加灰色水晶
        /// </summary>
        /// <param name="user"></param>
        /// <param name="crystalID"></param>
        public static void AppendGrayCrystal(GameUser user, int crystalID)
        {
            if (user == null) return;
            if (user.GrayCrystalList == null) user.GrayCrystalList = new CacheList<GrayCrystal>();

            CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystalID);
            if (crystalInfo == null) return;

            if (CheckAllowCrystall(user))
            {
                user.GrayCrystalList.Add(new GrayCrystal()
                {
                    UserCrystalID = Guid.NewGuid().ToString(),
                    CrystalID = crystalID,
                    SalePrice = crystalInfo.Price,
                    CreateDate = DateTime.Now,
                });
                user.UserExtend.GrayCrystalNum = MathUtils.Addition(user.UserExtend.GrayCrystalNum, 1);
            }
        }

        /// <summary>
        /// 卖出灰色水晶
        /// </summary>
        /// <param name="user"></param>
        /// <param name="crystalId">空卖出全部</param>
        /// <param name="saleNum">卖出水晶数</param>
        public static void SellGrayCrystal(GameUser user, string crystalId, out int saleNum)
        {
            int salePrice = 0;
            saleNum = 0;
            if (!string.IsNullOrEmpty(crystalId))
            {
                GrayCrystal crystal = user.GrayCrystalList.Find(m => m.UserCrystalID.Equals(crystalId)) ?? new GrayCrystal();
                salePrice += crystal.SalePrice;
                if (salePrice > 0) saleNum = 1;
                user.GrayCrystalList.Remove(crystal);

            }
            else
            {
                saleNum = user.GrayCrystalList.Count;
                user.GrayCrystalList.Foreach(m =>
                {
                    salePrice += ((GrayCrystal)m).SalePrice;
                    return true;
                });
                user.GrayCrystalList.Clear();
            }
            int totalNum = saleNum;
            user.GameCoin = MathUtils.Addition(user.GameCoin, salePrice, int.MaxValue);
            user.UserExtend.UpdateNotify(obj =>
                {
                    user.UserExtend.GrayCrystalNum = MathUtils.Subtraction(user.UserExtend.GrayCrystalNum, totalNum);
                    return true;
                });

        }

        /// <summary>
        /// 全部灰色水晶
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<GrayCrystal> GetGrayCrystalList(GameUser user)
        {
            return user.GrayCrystalList.ToList();
        }


        /// <summary>
        /// 获取猎命水晶页面的列表    
        /// </summary>
        /// <param name="user"></param>
        /// <param name="allowSale">允许一键卖出</param>
        /// <param name="allowTake">允许一键拾取</param>
        /// <returns></returns>
        public static List<GrayCrystal> GetNotSaleCrystalNum(GameUser user, out bool allowSale, out bool allowTake)
        {
            allowSale = false;
            allowTake = false;
            var crystalsList = new List<GrayCrystal>();
            var tempList = GetGrayCrystalList(user);
            if (tempList.Count > 0)
            {
                allowSale = true;
                crystalsList.AddRange(tempList);
            }
            var package = UserCrystalPackage.Get(user.UserID);
            UserCrystalInfo[] userCrystalsArray = package.CrystalPackage.FindAll(m => m.IsSale == 1).ToArray();
            //UserCrystal[] userCrystalsArray = UserCrystal.FindAll(UserCrystal.Index_UserID, m => m.IsSale == 1, user.UserID);
            if (userCrystalsArray.Length > 0)
            {
                allowTake = true;
            }
            foreach (UserCrystalInfo userCrystal in userCrystalsArray)
            {
                crystalsList.Add(new GrayCrystal()
                {
                    UserCrystalID = userCrystal.UserCrystalID,
                    CrystalID = userCrystal.CrystalID,
                    CreateDate = userCrystal.CreateDate
                });
            }
            return crystalsList;
        }

        /// <summary>
        /// 命格背包是否已满
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsCrystalNumFull(GameUser user)
        {
            var package = UserCrystalPackage.Get(user.UserID);
            UserCrystalInfo[] crystalArray = package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID == 0).ToArray();
            if (user.CrystalNum <= crystalArray.Length) return true;
            return false;
        }

        /// <summary>
        /// 获取命运水晶品质名称
        /// </summary>
        /// <param name="qualityType"></param>
        /// <returns></returns>
        public static string GetQualityName(CrystalQualityType qualityType)
        {
            string result = string.Empty;
            switch (qualityType)
            {
                case CrystalQualityType.Gray:
                    result = LanguageManager.GetLang().Color_Gray;
                    break;
                case CrystalQualityType.Green:
                    result = LanguageManager.GetLang().Color_Green;
                    break;
                case CrystalQualityType.Blue:
                    result = LanguageManager.GetLang().Color_Blue;
                    break;
                case CrystalQualityType.PurPle:
                    result = LanguageManager.GetLang().Color_PurPle;
                    break;
                case CrystalQualityType.Yellow:
                    result = LanguageManager.GetLang().Color_Yellow;
                    break;
                case CrystalQualityType.Orange:
                    result = LanguageManager.GetLang().Color_Orange;
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获得水晶发送世界聊天
        /// </summary>
        /// <param name="CrystalID"></param>
        /// <param name="userInfo"></param>
        public static void SendChat(int CrystalID, GameUser userInfo)
        {
            CrystalInfo crystal = new ConfigCacheSet<CrystalInfo>().FindKey(CrystalID);
            if (crystal == null)
                return;
            string chatcontent = string.Empty;
            if (crystal.CrystalQuality == CrystalQualityType.Orange)
            {
                chatcontent = LanguageManager.GetLang().St1305_GainQualityNotice;
            }
            else
            {
                chatcontent = LanguageManager.GetLang().St1305_HighQualityNotice;
            }
            string content = string.Format(chatcontent,
                            userInfo.NickName,
                            GetQualityName(crystal.CrystalQuality),
                            crystal.CrystalName
                            );
            new TjxChatService().SystemSend(ChatType.World, content);
        }

        /// <summary>
        /// 获得一个水晶
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="crystalID"></param>
        /// <param name="crystalLv"></param>
        public static void AppendCrystal(string userID, int crystalID, short crystalLv)
        {
            CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystalID);
            var crystalLvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(crystalID, crystalLv);
            var package = UserCrystalPackage.Get(userID);
            if (package == null || crystalInfo == null || crystalLvInfo == null)
            {
                return;
            }
            UserCrystalInfo ucrystalInfo = new UserCrystalInfo();
            ucrystalInfo.UserCrystalID = Guid.NewGuid().ToString();
            ucrystalInfo.CrystalID = crystalID;
            ucrystalInfo.CrystalID = crystalInfo.CrystalID;
            ucrystalInfo.CrystalLv = crystalLv;
            ucrystalInfo.Position = 0;
            ucrystalInfo.CurrExprience = crystalLvInfo.UpExperience;
            ucrystalInfo.CreateDate = DateTime.Now;
            package.SaveCrystal(ucrystalInfo);
        }

        public static string AkeyHuntingLife(GameUser user)
        {
            bool allowSale;
            bool allowTake;
            var grayCrystalArray = GetNotSaleCrystalNum(user, out allowSale, out allowTake);
            int crystalCount = MathUtils.Subtraction(_crystalGridNum, grayCrystalArray.Count);
            int crystallenght = 0;
            while (crystallenght < crystalCount)
            {
                string errStr = string.Empty;
                HuntingLife(user, out errStr);
                if (!string.IsNullOrEmpty(errStr))
                {
                    return errStr;
                }
                int saleNum = 0;
                SellGrayCrystal(user, null, out saleNum);
                if (saleNum <= 0)
                {
                    crystallenght = MathUtils.Addition(crystallenght, 1);
                }
            }
            return LanguageManager.GetLang().St1305_FateBackpackFull;
        }

        public static int UserLightLit(string userID)
        {
            int hunLight = 1001;
            var userLightArray = new GameDataCacheSet<UserLight>().FindAll(userID, s => s.IsLight == 1);
            if (userLightArray.Count > 0)
            {
                hunLight = userLightArray[userLightArray.Count - 1].HuntingID;
            }
            return hunLight;
        }

        /// <summary>
        /// 下一位猎命人物
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int UserNextLight(string userID, int hunlighID)
        {
            int hunLighID2 = 1001;
            if (hunlighID < 1005)
            {
                hunLighID2 = MathUtils.Addition(hunlighID, 1, int.MaxValue);
            }
            return hunLighID2;
        }

        public static string HuntingLife(GameUser user, out string errStr)
        {
            #region
            errStr = string.Empty;
            int huntingID = UserLightLit(user.UserID);
            int huntingID2 = UserNextLight(user.UserID, huntingID);
            UserDailyRestrain userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.UserID);
            var probabilityInfo = new ConfigCacheSet<ProbabilityInfo>().FindKey(huntingID); //当前猎命人物的概率
            if (probabilityInfo == null)
            {
                return errStr;
            }
            ProbabilityInfo probability1 = new ConfigCacheSet<ProbabilityInfo>().FindKey(huntingID2);
            if (userRestrain != null && userRestrain.Funtion2 >= VipHelper.GetVipUseNum(user.VipLv, RestrainType.MianFeiLieMing) && DateTime.Now.Date == userRestrain.RefreshDate.Date)
            {
                if (probabilityInfo.Price > user.GameCoin)
                {
                    errStr = LanguageManager.GetLang().St_GameCoinNotEnough;
                    return errStr;
                }
            }
            //暑期第三弹
            if (huntingID2 == 1001 && !NoviceHelper.IsGianCrystalPack(user))
            {
                errStr = LanguageManager.GetLang().St1305_BeiBaoBackpackFull;
                return errStr;
            }
            var lightCacheSet = new GameDataCacheSet<UserLight>();
            if (huntingID != 1001)
            {
                UserLight userLight1 = lightCacheSet.FindKey(user.UserID, huntingID);
                if (userLight1.IsLight == 2)
                {
                    return string.Empty;
                }
                if (userLight1.IsLight == 1)
                {
                    userLight1.IsLight = 2;
                }
            }
            UserLight userLight = new GameDataCacheSet<UserLight>().FindKey(user.UserID, huntingID2);
            if (RandomUtils.IsHit(probability1.Light))
            {
                if (userLight != null)
                {
                    userLight.IsLight = 1;
                    if (userLight.HuntingID == 1005)
                    {
                        errStr = LanguageManager.GetLang().St1305_HuntingIDLight;
                    }
                }
                else
                {
                    userLight = new UserLight()
                    {
                        UserID = user.UserID,
                        HuntingID = huntingID2,
                        IsLight = 1
                    };
                    lightCacheSet.Add(userLight);
                }
            }

            if (userRestrain != null)
            {
                if (userRestrain.Funtion2 >= VipHelper.GetVipUseNum(user.VipLv, RestrainType.MianFeiLieMing) && DateTime.Now.Date == userRestrain.RefreshDate.Date)
                {
                    user.GameCoin = MathUtils.Subtraction(user.GameCoin, probabilityInfo.Price, 0);
                }
                else
                {
                    userRestrain.Funtion2 = MathUtils.Addition(userRestrain.Funtion2, 1, int.MaxValue);
                }
            }
            else
            {
                user.GameCoin = MathUtils.Subtraction(user.GameCoin, probabilityInfo.Price, 0);
            }

            //每种品质的概率
            double[] probabilityArray2 = { (double)probabilityInfo.Gray, (double)probabilityInfo.Green, (double)probabilityInfo.Blue, (double)probabilityInfo.Purple, (double)probabilityInfo.Yellow, (double)probabilityInfo.Red };
            int index2 = RandomUtils.GetHitIndex(probabilityArray2);
            CrystalQualityType qualityType = (CrystalQualityType)Enum.Parse(typeof(CrystalQualityType), (index2 + 1).ToString());
            List<CrystalInfo> crystalArray2 = new ConfigCacheSet<CrystalInfo>().FindAll(u => u.CrystalQuality == qualityType && u.DemandLv <= user.UserLv);
            if (crystalArray2.Count > 0)
            {
                int randomNum = RandomUtils.GetRandom(0, crystalArray2.Count);
                var crystal = new ConfigCacheSet<CrystalInfo>().FindKey(crystalArray2[randomNum].CrystalID);
                if (crystal != null && crystal.CrystalQuality == CrystalQualityType.Gray)
                {
                    //wuzf修改 8-15 灰色放在临时背包不存DB
                    CrystalHelper.AppendGrayCrystal(user, crystal.CrystalID);
                }
                else if (crystal != null)
                {
                    var package = UserCrystalPackage.Get(user.UserID);

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
                    UserLogHelper.AppenCtystalLog(user.UserID, 5, crystal.CrystalID, probabilityInfo.Price, 0, null, 1, 0);
                    //高品质聊天通知);
                    if (crystal.CrystalQuality >= CrystalQualityType.Yellow)
                    {
                        var cacheChat = new TjxChatService();
                        string content = string.Format(LanguageManager.GetLang().St1305_HighQualityNotice,
                            user.NickName,
                           CrystalHelper.GetQualityName(crystal.CrystalQuality),
                            crystal.CrystalName
                            );
                        cacheChat.SystemSend(ChatType.World, content);
                    }
                }
            }
            return errStr;
            #endregion
        }
    }
}