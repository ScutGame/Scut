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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1254_附魔符详情接口
    /// </summary>
    public class Action1254 : BaseAction
    {
        private string userEnchantID;
        private string toUserID;
        private int currExp;
        private int upExp;
        private decimal finalMature;
        private UserEnchantInfo userEnchantInfo = null;
        private EnchantInfo enchant = null;
        private EnchantLvInfo enchantLvInfo = null;

        public Action1254(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1254, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(enchant == null ? 0 : enchant.EnchantID);
            this.PushIntoStack(enchant == null ? string.Empty : enchant.EnchantName.ToNotNullString());
            this.PushIntoStack(enchant == null ? string.Empty : enchant.HeadID.ToNotNullString());
            this.PushIntoStack(userEnchantInfo == null ? (short)0 : (short)userEnchantInfo.EnchantLv);
            this.PushIntoStack(enchant == null ? (short)0 : (short)enchant.ColorType);
            PushIntoStack(currExp);
            PushIntoStack(upExp);
            this.PushIntoStack(userEnchantInfo == null ? (short)0 : (short)userEnchantInfo.MaxMature);
            this.PushIntoStack(TrumpHelper.GetTransformData(finalMature).ToNotNullString());
            this.PushIntoStack(enchantLvInfo == null ? 0 : enchantLvInfo.CoinPrice);
            this.PushIntoStack(enchantLvInfo == null ? 0 : enchantLvInfo.MoJingPrice);
            this.PushIntoStack(enchant == null ? (short)0 : (short)enchant.AbilityType);
            this.PushIntoStack(enchantLvInfo == null ? string.Empty :TrumpHelper.GetTransformData(enchantLvInfo.Num).ToNotNullString());
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserEnchantID", ref userEnchantID))
            {
                httpGet.GetString("ToUserID", ref toUserID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            string puUserID;
            if (string.IsNullOrEmpty(toUserID))
            {
                puUserID = ContextUser.UserID;
            }
            else
            {
                puUserID = toUserID;
                UserCacheGlobal.LoadOffline(puUserID);
            }
            var package = UserEnchant.Get(puUserID);
            if (package != null)
            {
                userEnchantInfo = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
                if (userEnchantInfo != null)
                {
                    finalMature = CombatHelper.EnchantFinalNum(userEnchantInfo);
                    currExp = userEnchantInfo.CurrExprience;
                    enchant = new ConfigCacheSet<EnchantInfo>().FindKey(userEnchantInfo.EnchantID);
                    if (enchant != null)
                    {
                        enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(enchant.EnchantID, userEnchantInfo.EnchantLv);
                        short uplv = MathUtils.Addition(userEnchantInfo.EnchantLv, (short)1, (short)GameConfigSet.MaxEnchantLv);
                        EnchantLvInfo enchantLv = new ConfigCacheSet<EnchantLvInfo>().FindKey(userEnchantInfo.EnchantID, uplv);
                        upExp = enchantLv == null ? 0 : enchantLv.Experience;
                    }
                }
            }
            return true;
        }
    }
}