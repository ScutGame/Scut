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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1252_佣兵武器附魔符列表接口
    /// </summary>
    public class Action1252 : BaseAction
    {
        private int generalID;
        private string toUserID;
        private string generalName;
        private short careerID;
        private string userItemID;
        private string itemName;
        private string headID;
        private short openNum;
        private UserEnchantInfo[] enchantArray = new UserEnchantInfo[0];

        public Action1252(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1252, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(generalName.ToNotNullString());
            PushIntoStack(careerID);
            PushIntoStack(userItemID.ToNotNullString());
            PushIntoStack(itemName.ToNotNullString());
            PushIntoStack(headID.ToNotNullString());
            PushIntoStack((short)openNum);
            PushIntoStack(enchantArray.Length);
            foreach (var enchant in enchantArray)
            {
                EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchant.EnchantID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)enchant.Position);
                dsItem.PushIntoStack(enchant.UserEnchantID.ToNotNullString());
                dsItem.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.EnchantName.ToNotNullString());
                dsItem.PushIntoStack(enchantInfo == null ? string.Empty : enchantInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack((short)enchant.EnchantLv);
                dsItem.PushIntoStack(enchantInfo == null ? (short)0 : (short)enchantInfo.ColorType);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID))
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
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(puUserID, generalID);
            if (general == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            generalName = general.GeneralName;
            careerID = general.CareerID;
            var package = UserEnchant.Get(puUserID);
            UserItemInfo useritem = EnchantHelper.GetGeneralWeapon(general.UserID, general.GeneralID);
            if (useritem != null)
            {
                userItemID = useritem.UserItemID;
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
                if (itemInfo != null)
                {
                    itemName = itemInfo.ItemName;
                    headID = itemInfo.HeadID;
                }
                openNum = EnchantHelper.EnchantOpenGridNum(useritem.ItemLv);
                enchantArray = package.EnchantPackage.FindAll(m => m.UserItemID == useritem.UserItemID).ToArray();
            }

            return true;
        }
    }
}