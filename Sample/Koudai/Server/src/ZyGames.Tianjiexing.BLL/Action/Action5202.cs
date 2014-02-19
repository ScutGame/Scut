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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


using ZyGames.Tianjiexing.BLL.Combat;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 领土战鼓舞接口
    /// </summary>
    public class Action5202 : BaseAction
    {
        private const int ExpNum = 200;
        private const int GlodNum = 20;
        private int ops = 0;
        private double inspirePercent = 0;

        public Action5202(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5202, httpGet)
        {

        }

        public override bool TakeAction()
        {
            this.ErrorCode = ops;
            CountryCombat countryCombat = new CountryCombat(ContextUser);
            if (ops == 1)
            {
                this.ErrorInfo = string.Format(LanguageManager.GetLang().St5202_InspireTip, ExpNum);
            }
            else if (ops == 2)
            {
                if (ContextUser.ExpNum < ExpNum)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_ExpNumNotEnough;
                    return false;
                }
                if (!countryCombat.Inspire(false, out inspirePercent))
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_InspireFailed;
                }
                ContextUser.ExpNum = MathUtils.Subtraction(ContextUser.ExpNum, ExpNum, 0);
                //ContextUser.Update();
            }
            else if (ops == 3)
            {
                if (ContextUser.GoldNum < GlodNum)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                this.ErrorInfo = string.Format(LanguageManager.GetLang().St5202_InspireGoldTip, GlodNum);
            }
            else if (ops == 4)
            {
                if (ContextUser.VipLv < CountryCombat.CombatInspireVipLv)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                    return false;
                }
                if (ContextUser.GoldNum < GlodNum)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                if (!countryCombat.Inspire(true, out inspirePercent))
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_InspireFailed;
                }
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, GlodNum, int.MaxValue);
                //ContextUser.Update();
            }
            return true;
        }

        public override void BuildPacket()
        {
            this.PushIntoStack((inspirePercent * 100).ToInt());
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops, 1, 4))
            {
                return true;
            }
            return false;
        }
    }
}