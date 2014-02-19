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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1213_装备镶嵌与卸下接口
    /// </summary>
    public class Action1213 : BaseAction
    {
        private int _ops;
        private string _sparepartID;
        private string _userItemID;
        private short _position;


        public Action1213(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1213, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops)
                 && httpGet.GetString("SparepartID", ref _sparepartID))
            {
                httpGet.GetString("UserItemID", ref _userItemID);
                httpGet.GetWord("Position", ref _position);

                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            ErrorCode = _ops;
            //1：镶嵌 2：卸下 3：出售
            if (_ops == 1)
            {
                if (string.IsNullOrEmpty(_userItemID))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                UserSparePart[] sparePartsArray = ContextUser.SparePartList.FindAll(m => m.UserItemID.Equals(_userItemID)).ToArray();
                if (sparePartsArray.Length > 0)
                {
                    //原因：装备上镶嵌超出开启位置数量的灵件
                    if (ContextUser.UserExtend != null)
                    {
                        if (_position > ContextUser.UserExtend.MaxLayerNum)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1213_GridNumNotEnough;
                            return false;
                        }
                        if (sparePartsArray.Length >= ContextUser.UserExtend.MaxLayerNum)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1213_OpenNumNotEnough;
                            return false;
                        }
                    }
                    foreach (UserSparePart part in sparePartsArray)
                    {
                        if (part.Position == _position)
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1213_GridPotionFull;
                            return false;
                        }
                    }
                }

                var sparePart = ContextUser.SparePartList.Find(m => m.UserSparepartID.Equals(_sparepartID));
                if (sparePart != null && string.IsNullOrEmpty(sparePart.UserItemID))
                {
                    sparePart.UpdateNotify(obj =>
                    {
                        sparePart.UserItemID = _userItemID;
                        sparePart.SetPosition(_position);
                        return true;
                    });
                    UserLogHelper.AppendSparePartLog(ContextUser.UserID, sparePart, 3);
                    //ContextUser.UpdateSparePart();

                    var package = UserItemPackage.Get(Uid);
                    UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(sparePart.UserItemID));
                    if (userItem != null && userItem.ItemStatus.Equals(ItemStatus.YongBing))
                    {
                        var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(Uid, userItem.GeneralID);
                        if (userGeneral != null) userGeneral.RefreshMaxLife();
                    }
                }
            }
            else if (_ops == 2)
            {
                int currNum = ContextUser.SparePartList.FindAll(m => string.IsNullOrEmpty(m.UserItemID)).Count;
                if (currNum >= ContextUser.UserExtend.SparePartGridNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1213_GridNumFull;
                    return false;
                }
                var sparePart = ContextUser.SparePartList.Find(m => m.UserSparepartID.Equals(_sparepartID));
                if (sparePart != null && !string.IsNullOrEmpty(sparePart.UserItemID))
                {
                    var package = UserItemPackage.Get(Uid);
                    UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(sparePart.UserItemID));

                    sparePart.UpdateNotify(obj =>
                    {
                        sparePart.SetPosition(0);
                        sparePart.UserItemID = string.Empty;
                        return true;
                    });
                    UserLogHelper.AppendSparePartLog(ContextUser.UserID, sparePart, 3);
                    if (userItem != null && userItem.ItemStatus.Equals(ItemStatus.YongBing))
                    {
                        var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(Uid, userItem.GeneralID);
                        if (userGeneral != null) userGeneral.RefreshMaxLife();
                    }
                }
            }
            else if (_ops == 3)
            {
                var sparePart = ContextUser.SparePartList.Find(m => m.UserSparepartID.Equals(_sparepartID));
                if (sparePart != null)
                {
                    var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePart.SparePartId) ?? new SparePartInfo();
                    ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, sparePartInfo.CoinPrice);
                    ContextUser.UserExtend.UpdateNotify(obj =>
                        {
                            ContextUser.UserExtend.LingshiNum = MathUtils.Addition(ContextUser.UserExtend.LingshiNum, sparePartInfo.LingshiPrice);
                            return true;
                        });
                    UserLogHelper.AppendSparePartLog(ContextUser.UserID, sparePart, 2);
                    ContextUser.RemoveSparePart(sparePart);
                    //ContextUser.Update();

                }
            }
            return true;
        }
    }
}