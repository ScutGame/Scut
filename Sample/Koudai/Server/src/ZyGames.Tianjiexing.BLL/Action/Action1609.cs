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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1609_VIP礼包使用接口
    /// </summary>
    public class Action1609 : BaseAction
    {
        private string userItemID;


        public Action1609(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1609, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref userItemID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(ContextUser.UserID);
            UserItemInfo useritem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
            if (useritem != null)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
                if (itemInfo != null && itemInfo.DemandLv > ContextUser.VipLv)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                    return false;
                }
            }
            ItemUseHelper itemuse = new ItemUseHelper();
            itemuse.UseItem(userItemID, Uid);
            if (itemuse.result && itemuse.content != string.Empty)
            {
                ErrorCode = 0;
            }
            if (!itemuse.result)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
            }
            ErrorInfo = itemuse.content.Trim(',');
            return true;
        }
    }
}