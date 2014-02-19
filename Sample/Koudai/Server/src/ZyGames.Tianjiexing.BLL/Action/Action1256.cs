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
using System.Data;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1256_合成附魔符接口
    /// </summary>
    public class Action1256 : BaseAction
    {
        private string userEnchantID1;
        private string userEnchantID2;
        private int ops;


        public Action1256(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1256, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                httpGet.GetString("UserEnchantID1", ref userEnchantID1);
                httpGet.GetString("UserEnchantID2", ref userEnchantID2);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var syntheList = new CacheList<SynthesisInfo>();
            var package = UserEnchant.Get(ContextUser.UserID);
            if (ops == 1)
            {
                UserEnchantInfo userEnchantinfo1 = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID1);
                UserEnchantInfo userEnchantinfo2 = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID2);
                if (userEnchantinfo1 == null || userEnchantinfo2 == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1256_EnchantNotEnough;
                    return false;
                }

                if (userEnchantinfo1.EnchantLv >= GameConfigSet.MaxEnchantLv)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1256_OutMaxEnchantLv;
                    return false;
                }
                int entExprience1 = 0;
                int entExprience2 = 0;
                EnchantInfo enchant1 = new ConfigCacheSet<EnchantInfo>().FindKey(userEnchantinfo1.EnchantID);
                EnchantInfo enchant2 = new ConfigCacheSet<EnchantInfo>().FindKey(userEnchantinfo2.EnchantID);
                if (enchant1 != null && enchant2 != null)
                {
                    entExprience1 = enchant1.Experience;
                    entExprience2 = enchant2.Experience;
                    if (userEnchantinfo1.EnchantLv > userEnchantinfo2.EnchantLv ||
                        (userEnchantinfo1.EnchantLv == userEnchantinfo2.EnchantLv &&
                            userEnchantinfo1.CurrExprience >= userEnchantinfo2.CurrExprience))
                    {
                        UpdateEnchant(userEnchantID1, userEnchantID2, entExprience2);
                        syntheList.Add(new SynthesisInfo() { DemandID = userEnchantinfo2.EnchantID, Num = userEnchantinfo2.CurrExprience });
                        UserLogHelper.AppenEnchantLog(ContextUser.UserID, 4, userEnchantinfo1, syntheList);
                    }
                    else
                    {
                        UpdateEnchant(userEnchantID2, userEnchantID1, entExprience1);
                        syntheList.Add(new SynthesisInfo() { DemandID = userEnchantinfo1.EnchantID, Num = userEnchantinfo1.CurrExprience });
                        UserLogHelper.AppenEnchantLog(ContextUser.UserID, 4, userEnchantinfo2, syntheList);
                    }
                }
            }
            else if (ops == 2)
            {
                int experience = 0;
                var enchantArray = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID));
                if (enchantArray.Count == 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1256_EnchantNotEnough;
                    return false;
                }
                if (enchantArray.Count == 1)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1256_EnchantNumNotEnough;
                    return false;
                }
                enchantArray.QuickSort((x, y) =>
                {
                    int result = 0;
                    if (x == null && y == null) return 0;
                    if (x != null && y == null) return 1;
                    if (x == null) return -1;
                    result = y.EnchantLv.CompareTo(x.EnchantLv);
                    if (result == 0)
                    {
                        result = y.CurrExprience.CompareTo(x.CurrExprience);
                    }
                    return result;
                });

                UserEnchantInfo uEnchantInfo =
                    package.EnchantPackage.Find(m => m.UserEnchantID == enchantArray[0].UserEnchantID);
                if (uEnchantInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1262_EnchantSynthesisNotEnough;
                    return false;
                }
                EnchantLvInfo enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(uEnchantInfo.EnchantID,
                                                                                          GameConfigSet.MaxEnchantLv);
                int maxExprience = enchantLvInfo == null ? 0 : enchantLvInfo.Experience;

                foreach (var info in enchantArray)
                {
                    EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(info.EnchantID);
                    if (enchantInfo != null)
                    {
                        experience = enchantInfo.Experience;
                    }
                    if (uEnchantInfo.UserEnchantID != info.UserEnchantID)
                    {
                        if (uEnchantInfo.CurrExprience >= maxExprience)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1256_OutMaxEnchantLv;
                            return false;
                        }
                        syntheList.Add(new SynthesisInfo() { DemandID = info.EnchantID, Num = info.CurrExprience });
                        UpdateEnchant(uEnchantInfo.UserEnchantID, info.UserEnchantID, experience);
                    }
                }
                UserLogHelper.AppenEnchantLog(ContextUser.UserID, 4, uEnchantInfo, syntheList);
            }
            return true;
        }

        /// <summary>
        /// 合成附魔符
        /// </summary>
        /// <param name="_userEntID1"></param>
        /// <param name="_userEntID2"></param>
        /// <param name="entExp"></param>
        private void UpdateEnchant(string _userEntID1, string _userEntID2, int entExp)
        {
            var package = UserEnchant.Get(Uid);
            UserEnchantInfo uEInfo1 = package.EnchantPackage.Find(m => m.UserEnchantID == _userEntID1);
            UserEnchantInfo uEInfo2 = package.EnchantPackage.Find(m => m.UserEnchantID == _userEntID2);
            if (uEInfo1 != null && uEInfo2 != null)
            {
                int experience = MathUtils.Addition(uEInfo2.CurrExprience, entExp);
                uEInfo1.CurrExprience = MathUtils.Addition(uEInfo1.CurrExprience, experience);
                package.SaveEnchant(uEInfo1);
                EnchantHelper.CheckEnchantEscalate(ContextUser.UserID, _userEntID1);
                package.RemoveEnchant(uEInfo2);
            }
        }
    }
}