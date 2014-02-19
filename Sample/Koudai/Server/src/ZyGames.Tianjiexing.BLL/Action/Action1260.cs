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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1260_附魔背包格子开启接口
    /// </summary>
    public class Action1260 : BaseAction
    {
        private int ops;
        private int latticeNum;


        public Action1260(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1260, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops)
                 && httpGet.GetInt("LatticeNum", ref latticeNum))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (ContextUser.UserExtend != null && latticeNum > ContextUser.UserExtend.EnchantGridNum)
            {
                int openGold = EnchantHelper.EnchantOpenGoldNum(latticeNum, ContextUser.UserExtend.EnchantGridNum);
                int subGridNum = MathUtils.Subtraction(latticeNum, ContextUser.UserExtend.EnchantGridNum);
                if (ops == 1)
                {
                    ErrorCode = ops;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1260_UseGoldOpenPackage, openGold, subGridNum);
                    return false;
                }
                else if (ops == 2)
                {
                    if (ContextUser.GoldNum >= openGold)
                    {
                        ErrorCode = ops;
                        int goldNum = ContextUser.GoldNum;
                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, openGold);
                        ContextUser.UserExtend.UpdateNotify(obj =>
                        {
                            ContextUser.UserExtend.EnchantGridNum = (short)latticeNum;
                            return true;
                        });

                        UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 8, latticeNum, subGridNum, openGold, ContextUser.GoldNum, goldNum);
                    }
                    else
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                }
                else
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
            }
            return true;
        }
    }
}