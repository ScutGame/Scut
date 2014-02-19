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
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1107_仓库物品放入取出接口
    /// </summary>
    public class Action1107 : BaseAction
    {
        private string userItemID = string.Empty;
        private int ops = 0;


        public Action1107(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1107, httpGet)
        {

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref userItemID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            var package = UserItemPackage.Get(Uid);
            UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
            if (userItem == null)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                return false;
            }
            if (ops == 1)
            {
                var ckUserItemArray = UserItemHelper.GetItems(Uid).FindAll(m => m.ItemStatus == ItemStatus.CangKu);
                if (ckUserItemArray.Count >= ContextUser.WarehouseNum)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St1107_WarehouseNumFull;
                    return false;
                }
                UserItemHelper.MergerUserItem(ContextUser.UserID, userItemID, ItemStatus.CangKu);
            }
            else if (ops == 2)
            {
                var bbUserItemArray = UserItemHelper.GetItems(Uid).FindAll(m => m.ItemStatus == ItemStatus.BeiBao);
                if (bbUserItemArray.Count >= ContextUser.GridNum)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St1107_GridNumFull;
                    return false;
                }
                UserItemHelper.MergerUserItem(ContextUser.UserID, userItemID, ItemStatus.BeiBao);
            }
            return true;
        }
    }
}