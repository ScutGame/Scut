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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1257_附魔符培养界面接口
    /// </summary>
    public class Action1257 : BaseAction
    {
        private string userEnchantID;
        private string abilityNum;
        private decimal matureNum;
        private decimal maxMature;
        private UserEnchantInfo userEnchant = null;
        private EnchantInfo enchantInfo = null;
        private List<EnchantCulTure> enchantList = new List<EnchantCulTure>();

        public Action1257(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1257, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.EnchantName.ToNotNullString());
            this.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.HeadID.ToNotNullString());
            this.PushIntoStack(userEnchant == null ? (short)0 : (short)userEnchant.EnchantLv);
            this.PushIntoStack(enchantInfo == null ? (short)0 : (short)enchantInfo.ColorType);
            this.PushIntoStack(matureNum.ToNotNullString());
            this.PushIntoStack(TrumpHelper.GetTransformData(maxMature).ToNotNullString());
            this.PushIntoStack(enchantInfo == null ? (short)0 : (short)enchantInfo.AbilityType);
            this.PushIntoStack(abilityNum.ToNotNullString());
            this.PushIntoStack(enchantList.Count);
            foreach (var culTure in enchantList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)culTure.CultureType);
                dsItem.PushIntoStack(culTure.MoJingNum);
                dsItem.PushIntoStack(culTure.GoldNum);
                dsItem.PushIntoStack((int)culTure.UpMature);
                dsItem.PushIntoStack(culTure.SuccessNum.ToNotNullString());
                dsItem.PushIntoStack(IsMeet(ContextUser, culTure) ? (short)1 : (short)0);
                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserEnchantID", ref userEnchantID))
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
                userEnchant = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
                if (userEnchant != null)
                {
                    matureNum = (decimal)userEnchant.MaxMature / GameConfigSet.MaxEnchantMature;
                    maxMature = CombatHelper.EnchantFinalNum(userEnchant);
                    enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(userEnchant.EnchantID);
                    EnchantLvInfo enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(userEnchant.EnchantID,
                                                                                              userEnchant.EnchantLv);
                    if (enchantLvInfo != null)
                    {
                        abilityNum = TrumpHelper.GetTransformData(enchantLvInfo.Num);
                    }
                }
            }
            enchantList = EnchantHelper.EnchantCultureList();
            return true;
        }

        /// <summary>
        /// 是否满足培养条件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="culTure"></param>
        /// <returns></returns>
        public static bool IsMeet(GameUser user, EnchantCulTure culTure)
        {
            if ((culTure.CultureType == EnchantCultureType.Ordinary && user.UserExtend != null && user.UserExtend.MoJingNum >= culTure.MoJingNum)
                || (user.GoldNum >= culTure.GoldNum && culTure.CultureType != EnchantCultureType.Ordinary))
            {
                return true;
            }
            return false;
        }
    }
}