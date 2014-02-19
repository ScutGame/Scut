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
    /// 7007_神秘商店晶石刷新接口
    /// </summary>
    public class Action7007 : BaseAction
    {
        private int ops = 0;


        public Action7007(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action7007, httpGet)
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
            int sparRefresh = ConfigEnvSet.GetInt("MysteryShops.SparRefresh");
            if (ops == 1)
            {
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St7007_UseSparRefreshGold, sparRefresh);
                return false;
            }
            else if (ops == 2)
            {
                ErrorCode = ops;
                if (ContextUser.GoldNum < sparRefresh)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                UserHelper.RefrshShopsSparData(ContextUser, true);

                if (ContextUser.GoldNum >= sparRefresh)
                {
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, sparRefresh, int.MaxValue);
                    //ContextUser.Update();
                }
            }
            return true;
        }
    }
}