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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1073_开启队列接口
    /// </summary>
    public class Action1703 : BaseAction
    {
        private int _ops;

        public Action1703(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1703, httpGet)
        {

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            int equUseGold;
            int openGold = ConfigEnvSet.GetInt("Queue.OpenGold");
            int openMaxNum = ConfigEnvSet.GetInt("Queue.OpenMaxNum");
            if (ContextUser.QueueNum >= openMaxNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1703_QueueNumFull;
                return false;
            }
            if (ContextUser.QueueNum > 1)
            {
                if (!VipHelper.GetVipOpenFun(ContextUser.VipLv, ExpandType.KaiQiQiangHuaDuiLie))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnoughNotFuntion;
                    return false;
                }
                equUseGold = (openGold * 2);
            }
            else
            {
                equUseGold = openGold;
            }

            if (_ops == 1)
            {
                ErrorCode = _ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1703_UseGold, equUseGold);
                return false;
            }
            if (_ops == 2)
            {
                if (ContextUser.GoldNum >= equUseGold)
                {
                    ErrorCode = _ops;
                    
                        if (ContextUser.QueueNum < 3)
                        {
                            ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, equUseGold, int.MaxValue);
                            ContextUser.QueueNum = MathUtils.Addition(ContextUser.QueueNum, (short)1, short.MaxValue);
                            //ContextUser.Update();
                        }
                    

                }
                else
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
            }
            return true;
        }
    }
}