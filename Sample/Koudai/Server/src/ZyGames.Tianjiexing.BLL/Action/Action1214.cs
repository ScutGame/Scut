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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1214_灵件属性洗涤接口
    /// </summary>
    public class Action1214 : BaseAction
    {
        private string _sparepartID;
        private int _ops;
        private string _lockList;
        private UserSparePart _sparePart = new UserSparePart();
        private string[] _partPropertys = new string[0];

        public Action1214(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1214, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_sparePart.Propertys.Count);
            for (int i = 0; i < _sparePart.Propertys.Count; i++)
            {
                var property = _sparePart.Propertys[i];
                short proPos = MathUtils.Addition(property.ValueIndex, (short)1, short.MaxValue);
                short isEnable = 0;
                if (property.IsEnable) isEnable = 1;

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(property.AbilityType.ToShort());
                dsItem.PushIntoStack(property.Num.ToNotNullString());
                dsItem.PushIntoStack(property.HitMinValue.ToNotNullString());
                dsItem.PushIntoStack(property.HitMaxValue.ToNotNullString());
                dsItem.PushIntoStack(isEnable);
                dsItem.PushIntoStack(proPos);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("SparepartID", ref _sparepartID)
                 && httpGet.GetInt("Ops", ref _ops))
            {
                httpGet.GetString("LockList", ref _lockList);
                if (_lockList.TrimEnd().Length > 0) _partPropertys = _lockList.Trim().Split(',');
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ErrorCode = _ops;
            var sparePart = ContextUser.SparePartList.Find(m => m.UserSparepartID.Equals(_sparepartID));
            var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePart.SparePartId);

            var reset = sparePartInfo.GetSparePartReset(_partPropertys.Length);
            if (_ops == 1)
            {
                ErrorInfo = string.Format(LanguageManager.GetLang().St1214_ResetUseLingshi, reset.Lingshi, reset.Coin);
            }
            else if (_ops == 2)
            {
                if (reset.Lingshi > ContextUser.UserExtend.LingshiNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_LingshiNumNotEnough;
                    return false;
                }
                if (reset.Coin > ContextUser.GameCoin)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                    return false;
                }
                if (reset.Lingshi <= ContextUser.UserExtend.LingshiNum)
                {
                    ResetProperty();
                    ContextUser.UserExtend.UpdateNotify(obj => 
                        {
                            ContextUser.UserExtend.LingshiNum = MathUtils.Subtraction(ContextUser.UserExtend.LingshiNum, reset.Lingshi, 0);
                            return true;
                        });
                    ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, reset.Coin, 0);
                    //ContextUser.Update();
                }
                UserLogHelper.AppendSparePartLog(ContextUser.UserID, sparePart, 4);
            }
            else if (_ops == 3)
            {
                ErrorInfo = string.Format(LanguageManager.GetLang().St1214_ResetUseGold, reset.Gold, reset.Coin);
            }
            else if (_ops == 4)
            {
                if (reset.Gold > ContextUser.GoldNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                if (reset.Coin > ContextUser.GameCoin)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                    return false;
                }
                if (reset.Gold <= ContextUser.GoldNum && reset.Coin <= ContextUser.GameCoin)
                {
                    ResetProperty();
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, reset.Gold);
                    ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, reset.Coin);
                    //ContextUser.Update();
                }
                UserLogHelper.AppendSparePartLog(ContextUser.UserID, sparePart, 5);
            }
            return true;
        }

        private void ResetProperty()
        {
            //洗涤属性
            var sparePart = ContextUser.SparePartList.Find(m => m.UserSparepartID.Equals(_sparepartID));
            if (sparePart != null)
            {
                var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePart.SparePartId) ?? new SparePartInfo();
                sparePart.UpdateNotify(obj =>
                {
                    for (int i = 0; i < sparePart.Propertys.Count; i++)
                    {
                        if (!sparePart.Propertys[i].IsEnable) continue;
                        if (_partPropertys.Length > 0 && Array.Exists(_partPropertys, m => m.ToInt() - 1 == i)) continue;

                        sparePart.Propertys[i] = UserSparePart.RandomProperty(sparePartInfo, false, sparePart.Propertys);
                        var package = UserItemPackage.Get(Uid);
                        UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(sparePart.UserItemID));
                        if (userItem != null && userItem.ItemStatus.Equals(ItemStatus.YongBing))
                        {
                            var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(Uid, userItem.GeneralID);
                            if (userGeneral != null) userGeneral.RefreshMaxLife();
                        }
                    }
                    return true;
                });
                _sparePart = ContextUser.SparePartList.Find(m => m.UserSparepartID.Equals(_sparepartID));
                //ContextUser.UpdateSparePart();
            }
        }

    }
}