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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class EnchantHelper
    {
        /// <summary>
        /// 当前佣兵身上的武器
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        /// <returns></returns>
        public static UserItemInfo GetGeneralWeapon(string userID, int generalID)
        {
            UserItemInfo useritem = null;
            var package = UserItemPackage.Get(userID);
            if (package != null)
            {
                UserItemInfo[] userItemArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.GeneralID == generalID && m.Equparts == EquParts.WuQi).ToArray();
                if (userItemArray.Length > 0)
                {
                    useritem = userItemArray[0];
                }
            }
            return useritem;
        }

        /// <summary>
        /// 佣兵武器开启的附魔格子
        /// </summary>
        /// <param name="itemLv"></param>
        /// <returns></returns>
        public static short EnchantOpenGridNum(short itemLv)
        {
            short gridNum = 0;
            var mosaicInfos = new ConfigCacheSet<MosaicInfo>().FindAll(m => m.DemandLv <= itemLv);
            mosaicInfos.QuickSort((x, y) =>
            {
                int result = 0;
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                result = y.DemandLv.CompareTo(x.DemandLv);
                return result;
            });
            if (mosaicInfos.Count > 0)
            {
                gridNum = mosaicInfos[0].Position.ToShort();
            }
            return gridNum;
        }

        /// <summary>
        /// 附魔符升级
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="generalID"></param>
        public static void CheckEnchantEscalate(string userID, string userEnchantID)
        {
            var package = UserEnchant.Get(userID);
            UserEnchantInfo uEInfo1 = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
            if (uEInfo1 != null)
            {
                while (uEInfo1.EnchantLv <= GameConfigSet.MaxEnchantLv)
                {
                    short upLv = MathUtils.Addition(uEInfo1.EnchantLv, (short)1);
                    EnchantLvInfo enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(uEInfo1.EnchantID, upLv);
                    if (enchantLvInfo != null && uEInfo1.CurrExprience >= enchantLvInfo.Experience)
                    {
                        uEInfo1.EnchantLv = MathUtils.Addition(uEInfo1.EnchantLv, (short)1);
                        package.SaveEnchant(uEInfo1);
                    }
                    else
                    {
                        break;
                    }
                }
                if (uEInfo1.EnchantLv >= GameConfigSet.MaxEnchantLv)
                {
                    EnchantLvInfo enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(uEInfo1.EnchantID, GameConfigSet.MaxEnchantLv);
                    if (enchantLvInfo != null)
                    {
                        uEInfo1.EnchantLv = GameConfigSet.MaxEnchantLv.ToShort();
                        uEInfo1.CurrExprience = enchantLvInfo.Experience;
                        package.SaveEnchant(uEInfo1);
                    }
                }
            }
        }

        /// <summary>
        /// 附魔符培养列表
        /// </summary>
        /// <returns></returns>
        public static List<EnchantCulTure> EnchantCultureList()
        {
            List<EnchantCulTure> enchantList = new List<EnchantCulTure>();
            string[] cultureArray = ConfigEnvSet.GetString("Enchant.EnchantCulture").Split(',');
            foreach (var str in cultureArray)
            {
                EnchantCulTure enchantCulTure = new EnchantCulTure();
                string[] strCulture = str.Split('=');
                if (strCulture.Length > 4)
                {
                    enchantCulTure.CultureType = strCulture[0].ToEnum<EnchantCultureType>();
                    enchantCulTure.GoldNum = strCulture[1].ToInt();
                    enchantCulTure.MoJingNum = strCulture[2].ToInt();
                    enchantCulTure.UpMature = strCulture[3].ToShort();
                    enchantCulTure.SuccessNum = strCulture[4].ToDecimal();
                    enchantList.Add(enchantCulTure);
                }
            }
            return enchantList;
        }

        /// <summary>
        /// 开启附魔符格子的价格
        /// </summary>
        /// <param name="gNum">开启格子位置</param>
        /// <param name="gridNum">已开启的格子数</param>
        /// <returns></returns>
        public static int EnchantOpenGoldNum(int gNum, int gridNum)
        {
            int latticeSpar = 0;
            int sum = 0;
            int sub = 0;
            int openGold = GameConfigSet.OpenGoldNum;
            int minGridNum = GameConfigSet.BasePackageNum;
            int subGridNum = MathUtils.Subtraction(gNum, minGridNum);

            int minusNum = MathUtils.Subtraction(gridNum, minGridNum);
            for (int i = 1; i <= subGridNum; i++)
            {
                if (i == 0) latticeSpar = 0;
                if (i == 1) latticeSpar = openGold;
                if (i >= 2) latticeSpar = i * openGold;
                sum += latticeSpar;
            }

            for (int j = 0; j <= minusNum; j++)
            {
                if (j == 0) latticeSpar = 0;
                if (j == 1) latticeSpar = openGold;
                if (j >= 2) latticeSpar = j * openGold;
                sub += latticeSpar;
            }
            return MathUtils.Subtraction(sum, sub);
        }

        /// <summary>
        /// 是否符合当前凹槽的附魔符
        /// </summary>
        /// <param name="postion"></param>
        /// <param name="colorType"></param>
        /// <returns></returns>
        public static bool IsMosaicColor(int postion, ColorType colorType)
        {
            MosaicInfo mosaicInfo = new ConfigCacheSet<MosaicInfo>().FindKey(postion);
            if (mosaicInfo != null)
            {
                var strPostion = mosaicInfo.MosaicColor.Split(',');
                foreach (string str in strPostion)
                {
                    if (str.ToEnum<ColorType>() == colorType)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string EnchantAbilityStr(int enchantID, short enchantLv)
        {
            return CombatHelper.EnchantAbilityNum(enchantID, enchantLv).ToString();
        }

        /// <summary>
        /// 开启附魔符格子
        /// </summary>
        /// <param name="user"></param>
        public static void EnchantFunctionOpen(GameUser user)
        {
            if (user != null && user.UserExtend != null && user.UserExtend.EnchantGridNum < GameConfigSet.BasePackageNum)
            {
                user.UserExtend.UpdateNotify(obj =>
                {
                    user.UserExtend.EnchantGridNum = GameConfigSet.BasePackageNum;
                    return true;
                });
            }
        }

        /// <summary>
        /// 获取附魔符
        /// </summary>
        /// <param name="enchantID"></param>
        /// <returns></returns>
        public static UserEnchantInfo GetUserEnchantInfo(int enchantID)
        {
            UserEnchantInfo enchant = new UserEnchantInfo();
            EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchantID);
            if (enchantInfo != null)
            {
                enchant.UserEnchantID = Guid.NewGuid().ToString();
                enchant.EnchantID = enchantID;
                enchant.EnchantLv = 1;
                enchant.CurrExprience = 0;
                enchant.Position = 0;
                enchant.UserItemID = string.Empty;
                enchant.MaxMature = (short)RandomUtils.GetRandom(enchantInfo.BeseNum, enchantInfo.MaxNum);
            }
            return enchant;
        }

        /// <summary>
        /// 获取oa补偿附魔符
        /// </summary>
        /// <param name="enchantID"></param>
        /// <returns></returns>
        public static UserEnchantInfo GetUserEnchantInfo(int enchantID, short enchantLv)
        {
            UserEnchantInfo enchant = new UserEnchantInfo();
            var enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(enchantID, enchantLv);
            EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchantID);
            if (enchantInfo != null && enchantLvInfo != null)
            {
                enchant.UserEnchantID = Guid.NewGuid().ToString();
                enchant.EnchantID = enchantID;
                enchant.EnchantLv = enchantLv;
                enchant.CurrExprience = enchantLvInfo.Experience;
                enchant.Position = 0;
                enchant.UserItemID = string.Empty;
                enchant.MaxMature = (short)RandomUtils.GetRandom(enchantInfo.BeseNum, enchantInfo.MaxNum);
            }
            return enchant;
        }

        /// <summary>
        /// 附魔符背包是否已满
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static bool IsEnchantPackage(string userID)
        {
            int enchantNum = 0;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (user != null && user.UserExtend != null)
            {
                enchantNum = user.UserExtend.EnchantGridNum;
            }
            UserFunction function = new GameDataCacheSet<UserFunction>().FindKey(userID, FunctionEnum.Enchant);
            if (function == null)
            {
                return false;
            }
            var package = UserEnchant.Get(userID);
            if (package != null)
            {
                var enchantList = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID));
                if (enchantNum > enchantList.Count)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取附魔符
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="enchantID"></param>
        /// <param name="enchantLv"></param>
        public static void AppendEnchat(string userid, int enchantID, short enchantLv)
        {
            var package = UserEnchant.Get(userid);
            UserEnchantInfo enchant = GetUserEnchantInfo(enchantID, enchantLv);
            if (package != null && enchant != null)
            {
                package.SaveEnchant(enchant);
            }
        }
    }
}