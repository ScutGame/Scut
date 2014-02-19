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
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1458_法宝洗练接口
    /// </summary>
    public class Action1458 : BaseAction
    {
        private int ops;


        public Action1458(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1458, httpGet)
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

            int upitemNum = 0;
            var cacheSet = new GameDataCacheSet<UserTrump>();
            UserTrump userTrump = cacheSet.FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump == null)
            {
                return false;
            }
            WashConsumeInfo consumeInfo = TrumpHelper.GetWashConsumeInfo(userTrump.MatureNum);
            if (consumeInfo == null)
            {
                return false;
            }
            upitemNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, consumeInfo.ItemID);
            if (ops == 1)
            {
                string success = ((double)consumeInfo.SuccessNum * 100).ToString();
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1458_UseBackDaysOrb, consumeInfo.ItemNum, consumeInfo.MatureNum, success);
                return false;
            }
            else if (ops == 2)
            {
                ErrorCode = ops;
                if (upitemNum < consumeInfo.ItemNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1458_BackDaysOrbNotEnough;
                    return false;
                }
                int maxMatrueNum = ConfigEnvSet.GetInt("Trump.MaxMatrueNum");
                if (userTrump.MatureNum >= maxMatrueNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1458_MaxMatrueNumFull;
                    return false;
                }
                UserItemHelper.UseUserItem(ContextUser.UserID, consumeInfo.ItemID, consumeInfo.ItemNum);
                if (RandomUtils.IsHit(consumeInfo.SuccessNum))
                {
                    userTrump.MatureNum = MathUtils.Addition(userTrump.MatureNum, consumeInfo.MatureNum.ToShort(), maxMatrueNum.ToShort());
                    ErrorInfo = LanguageManager.GetLang().St1458_XiLianSuccess;
                    var usergeneral = UserGeneral.GetMainGeneral(ContextUser.UserID);
                    if (usergeneral != null)
                    {
                        usergeneral.RefreshMaxLife();
                    }
                }
                else
                {
                    ErrorInfo = LanguageManager.GetLang().St1458_XiLianFail;
                }
            }
            return true;
        }
    }
}