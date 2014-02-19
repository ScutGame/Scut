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
using ZyGames.Framework.Cache;
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
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1258_附魔符培养接口
    /// </summary>
    public class Action1258 : BaseAction
    {
        private string userEnchantID;
        private EnchantCultureType cultureType;


        public Action1258(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1258, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserEnchantID", ref userEnchantID)
                 && httpGet.GetEnum("CultureType", ref cultureType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserEnchant.Get(ContextUser.UserID);
            if (package != null)
            {
                var userEnchant = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
                if (userEnchant != null)
                {
                    if (userEnchant.MaxMature >= GameConfigSet.MaxEnchantMature)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1258_OutMaxEnchantMature;
                        return false;
                    }
                    List<EnchantCulTure> culTuresList = EnchantHelper.EnchantCultureList();
                    EnchantCulTure enchantCulTure = culTuresList.Find(m => m.CultureType == cultureType);
                    if (enchantCulTure != null)
                    {
                        if (cultureType == EnchantCultureType.Ordinary)
                        {
                            if (ContextUser.UserExtend == null || ContextUser.UserExtend.MoJingNum < enchantCulTure.MoJingNum)
                            {
                                ErrorCode = LanguageManager.GetLang().ErrorCode;
                                ErrorInfo = LanguageManager.GetLang().St1258_MagicCrystalNotEnough;
                                return false;
                            }
                        }
                        else
                        {
                            if (ContextUser.GoldNum < enchantCulTure.GoldNum)
                            {
                                ErrorCode = LanguageManager.GetLang().ErrorCode;
                                ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                                return false;
                            }
                        }
                        string content = string.Empty;
                        if (RandomUtils.IsHit(enchantCulTure.SuccessNum))
                        {
                            if (cultureType == EnchantCultureType.Ordinary)
                            {
                                ContextUser.UserExtend.MoJingNum = MathUtils.Subtraction(ContextUser.UserExtend.MoJingNum, enchantCulTure.MoJingNum);
                                content = string.Format(LanguageManager.GetLang().St1258_ConsumeMagicCrystalUpEnhance, enchantCulTure.MoJingNum, enchantCulTure.UpMature);
                            }
                            else
                            {
                                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, enchantCulTure.GoldNum);
                                content = string.Format(LanguageManager.GetLang().St1258_ConsumeGoldNumUpEnhance, enchantCulTure.GoldNum, enchantCulTure.UpMature);
                            }
                            short maxMatureNum = MathUtils.Addition(userEnchant.MaxMature, enchantCulTure.UpMature, (short)GameConfigSet.MaxEnchantMature);
                            userEnchant.UpdateNotify(obj =>
                            {
                                userEnchant.MaxMature = maxMatureNum;
                                return true;
                            });
                            UserLogHelper.AppenEnchantLog(ContextUser.UserID, 3, userEnchant, new CacheList<SynthesisInfo>());
                        }
                        else
                        {
                            if (cultureType == EnchantCultureType.Ordinary)
                            {
                                ContextUser.UserExtend.MoJingNum = MathUtils.Subtraction(ContextUser.UserExtend.MoJingNum, enchantCulTure.MoJingNum);
                                content = string.Format(LanguageManager.GetLang().St1258_EnhanceCultureFailedMagicCrystal, enchantCulTure.MoJingNum);
                            }
                            else
                            {
                                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, enchantCulTure.GoldNum);
                                content = string.Format(LanguageManager.GetLang().St1258_EnhanceCultureFailedGold, enchantCulTure.GoldNum);
                            }
                        }
                        ErrorCode = 0;
                        ErrorInfo = content;
                    }
                }
            }
            return true;
        }
    }
}