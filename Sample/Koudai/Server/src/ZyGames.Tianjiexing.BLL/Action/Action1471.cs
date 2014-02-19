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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1471_法宝属相易相接口
    /// </summary>
    public class Action1471 : BaseAction
    {
        private int ops;


        public Action1471(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1471, httpGet)
        {

        }

        public override void BuildPacket()
        {

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
            int useGold = ConfigEnvSet.GetInt("TrumpZodiac.ChangeZodiacUseGold");
            if (ops == 1)
            {
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1471_ChangeZodiac, useGold);
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum < useGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                var cacheSet = new GameDataCacheSet<UserTrump>();
                UserTrump userTrump = cacheSet.FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
                if (userTrump != null)
                {
                    userTrump.Zodiac = TrumpHelper.GetZodiacType(userTrump.Zodiac);
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold, int.MaxValue);
                }
                ErrorCode = ops;
            }
            return true;
        }
    }
}