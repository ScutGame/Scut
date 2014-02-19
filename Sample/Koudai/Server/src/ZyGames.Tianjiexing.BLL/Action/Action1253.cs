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
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1253_背包附魔符列表接口
    /// </summary>
    public class Action1253 : BaseAction
    {
        private int pageIndex;
        private int pageSize;
        private string toUserID;
        private int pageCount;
        private int goldNum;
        private int moshiNum;
        private int packNum;
        private UserEnchantInfo[] enchantArray = new UserEnchantInfo[0];

        public Action1253(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1253, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(pageCount);
            this.PushIntoStack(goldNum);
            this.PushIntoStack(moshiNum);
            this.PushIntoStack(packNum);
            this.PushIntoStack(enchantArray.Length);
            foreach (var enchant in enchantArray)
            {
                EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchant.EnchantID);
                DataStruct dsItem = new DataStruct();
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
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize))
            {
                httpGet.GetString("ToUserID", ref toUserID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            GameUser user = null;
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
            user = new GameDataCacheSet<GameUser>().FindKey(puUserID);
            if (user != null)
            {
                goldNum = user.GoldNum;
                if (user.UserExtend != null)
                {
                    moshiNum = user.UserExtend.MoJingNum;
                    packNum = user.UserExtend.EnchantGridNum;
                }
            }
            var package = UserEnchant.Get(puUserID);
            if (package != null && string.IsNullOrEmpty(toUserID))
            {
                var enchantsLList = package.EnchantPackage.FindAll(m => string.IsNullOrEmpty(m.UserItemID));
                enchantArray = enchantsLList.GetPaging(pageIndex, pageSize, out pageCount).ToArray();
            }
            return true;
        }
    }
}