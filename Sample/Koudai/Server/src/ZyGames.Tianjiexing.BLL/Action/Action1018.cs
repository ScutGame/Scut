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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1018_新手活动领取阅历和声望接口
    /// </summary>
    public class Action1018 : BaseAction
    {
        private int TakeNum;
        private int ColdTime;


        public Action1018(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1018, httpGet)
        {
            TakeNum = 0;
            ColdTime = 0;
        }

        public override void BuildPacket()
        {
            this.PushIntoStack(TakeNum);
            this.PushIntoStack(ColdTime);

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            int expNum;
            if (NoviceHelper.DailyExpPrize(ContextUser, out expNum))
            {
                ErrorInfo = string.Format(LanguageManager.GetLang().St1018_ExpObtainPrize, expNum);
            }
            return true;
        }
    }
}