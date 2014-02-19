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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.ConfigModel;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model.DataModel;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 12004_抽奖接口
    /// </summary>
    public class Action12004 : BaseAction
    {
        private int ops;
        private short postion;
        private int hasNextBox = 0;
        

        public Action12004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action12004, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack((short)postion);
            PushIntoStack(hasNextBox);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            DialHelper.CheckDialNum(ContextUser.UserID);
            UserDial userDial = new GameDataCacheSet<UserDial>().FindKey(ContextUser.UserID);
            if (userDial == null || string.IsNullOrEmpty(userDial.UserItemID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (ops == 1)
            {
                string errContent = string.Empty;
                if (UserPackHelper.PackIsFull(ContextUser, BackpackType.BeiBao, 0, out errContent))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = errContent;
                    return false;
                }
                int itemid = UserItemHelper.GetUserItemInfoID(ContextUser.UserID, userDial.UserItemID);
                var itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemid);
                if (itemInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                    return false;
                }
                // 根据宝箱取得该宝箱对应的钥匙ID
                int itemNum = UserItemHelper.CheckItemNum(ContextUser.UserID, itemInfo.EffectNum);
                if (itemNum <= 0)
                {
                    ItemBaseInfo itemKey = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemInfo.EffectNum);
                    if (itemKey == null)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                        return false;
                    }
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St12004_ChestKeyNotEnough, itemKey.ItemName);
                    return false;
                }
                UserItemHelper.UseUserItem(ContextUser.UserID, itemid, 1);
                UserItemHelper.UseUserItem(ContextUser.UserID, itemInfo.EffectNum, 1);
                postion = DialHelper.ChestDialPrizePostion(ContextUser);
                postion = MathUtils.Addition(postion, (short)1);

                // 是否还可以继续使用
                //var package = UserItemPackage.Get(ContextUser.UserID);
                bool hasBox = UserItemHelper.IsEnoughBeiBaoItem(ContextUser.UserID, itemid, 1);
                bool hasKey = UserItemHelper.IsEnoughBeiBaoItem(ContextUser.UserID, itemInfo.EffectNum, 1);
                // 如果钥匙和宝箱的数各 >= 1
                if (hasBox && hasKey)
                {
                    hasNextBox = 1;
                }

            }

            //int useGold = 0;
            //if (ops == 1)
            //{
            //    //if (!DialHelper.IsDialFree(ContextUser.UserID))
            //    //{
            //    //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    //    ErrorInfo = LanguageManager.GetLang().St12004_FreeNotEnough;
            //    //    return false;
            //    //}
            //}
            //else if (ops == 2)
            //{
            //    useGold = GameConfigSet.SweepstakesRequiredGold;
            //    ErrorCode = ops;
            //    ErrorInfo = string.Format(LanguageManager.GetLang().St12004_SpendSparDraw, useGold, 1);
            //    return false;
            //}
            //else if (ops == 3)
            //{
            //    if (DialHelper.IsDialFree(ContextUser.UserID))
            //    {
            //        ErrorCode = LanguageManager.GetLang().ErrorCode;
            //        ErrorInfo = LanguageManager.GetLang().St12004_FreeNumEnough;
            //        return false;
            //    }
            //    useGold = GameConfigSet.SweepstakesRequiredGold;
            //    if (ContextUser.GoldNum < useGold)
            //    {
            //        ErrorCode = LanguageManager.GetLang().ErrorCode;
            //        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
            //        return false;
            //    }
            //    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
            //}
            //else if (ops == 4)
            //{
            //    useGold = GameConfigSet.FiveRequiredGold;
            //    ErrorCode = ops;
            //    ErrorInfo = string.Format(LanguageManager.GetLang().St12004_SpendSparDraw, useGold, 5);
            //    return false;
            //}
            //else if (ops == 5)
            //{
            //    if (DialHelper.IsDialFree(ContextUser.UserID))
            //    {
            //        ErrorCode = LanguageManager.GetLang().ErrorCode;
            //        ErrorInfo = LanguageManager.GetLang().St12004_FreeNumEnough;
            //        return false;
            //    }
            //    useGold = GameConfigSet.FiveRequiredGold;
            //    if (ContextUser.GoldNum < useGold)
            //    {
            //        ErrorCode = LanguageManager.GetLang().ErrorCode;
            //        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
            //        return false;
            //    }
            //    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
            //}
            //ErrorCode = ops;
            //postion = DialHelper.DialPrizePostion(ContextUser.UserID, ops);
            //postion = MathUtils.Addition(postion, (short)1);
            return true;
        }
    }
}