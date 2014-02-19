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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1464_法宝附加属性祭祀接口
    /// </summary>
    public class Action1464 : BaseAction
    {
        public Action1464(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1464, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                return true;
            }
        }

        public override bool TakeAction()
        {
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump == null)
            {
                return false;
            }
            if (userTrump.WorshipLv >= GameConfigSet.MaxWorshipLv)
            {
                return false;
            }
            short uplv = (short)MathUtils.Addition((int)userTrump.WorshipLv, 1, GameConfigSet.MaxWorshipLv);
            WorshipInfo worshipInfo = new ConfigCacheSet<WorshipInfo>().FindKey(TrumpInfo.CurrTrumpID, uplv);
            if (worshipInfo == null)
            {
                return false;
            }

            if (ContextUser.GameCoin < worshipInfo.GameCoin)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                return false;
            }
            if (ContextUser.ObtainNum < worshipInfo.ObtainNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_ObtainNumNotEnough;
                return false;
            }
            int upitemNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, worshipInfo.ItemID);
            if (upitemNum < worshipInfo.ItemNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1462_ItemNumNotEnough;
                return false;
            }
            ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, worshipInfo.GameCoin, 0);
            ContextUser.ObtainNum = MathUtils.Subtraction(ContextUser.ObtainNum, worshipInfo.ObtainNum, 0);
            UserItemHelper.UseUserItem(ContextUser.UserID, worshipInfo.ItemID, worshipInfo.ItemNum);
            ErrorCode = 0;
            if (RandomUtils.IsHit(worshipInfo.SuccessNum))
            {
                userTrump.WorshipLv = MathUtils.Addition(userTrump.WorshipLv, (short)1, GameConfigSet.MaxWorshipLv);
                ErrorInfo = LanguageManager.GetLang().St1464_WorshipSuccess;
            }
            else
            {
                ErrorInfo = LanguageManager.GetLang().St1464_WorshipFail;
            }
            return true;
        }
    }
}