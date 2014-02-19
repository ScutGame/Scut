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


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1215_灵件背包开启接口
    /// </summary>
    public class Action1215 : BaseAction
    {
        private int _ops;
        private short _enableGridNum;


        public Action1215(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1215, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops)
                 && httpGet.GetWord("EnableGridNum", ref _enableGridNum))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ErrorCode = _ops;
            int goldNum = GetOpenGoldNum(_enableGridNum, ContextUser.UserExtend.SparePartGridNum);
            short subGridNum = MathUtils.Subtraction(_enableGridNum, ContextUser.UserExtend.SparePartGridNum);
            if (_ops == 1)
            {
                ErrorInfo = string.Format(LanguageManager.GetLang().St1215_OpenGridNumUseGold, subGridNum, goldNum);
            }
            if (_ops == 2)
            {
                if (goldNum > ContextUser.GoldNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                if (goldNum <= ContextUser.GoldNum)
                {
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, goldNum, int.MaxValue);
                    ContextUser.UserExtend.UpdateNotify(obj => 
                    {
                        ContextUser.UserExtend.SparePartGridNum =
                        MathUtils.Addition(ContextUser.UserExtend.SparePartGridNum, subGridNum, new GameUser().SparePartMaxGridNum);
                        return true;
                    });
                    //ContextUser.Update();
                }
            }
            return true;
        }

        public static int GetOpenGoldNum(int enableNum, int userGridNum)
        {
            int latticeSpar = 0; //开启每个格子的价格
            int Sum = 0;
            int Sub = 0;
            int minGridNum = new GameUser().SparePartMinGridNum;
            int subGridNum = MathUtils.Subtraction(enableNum, minGridNum, 0);

            int minusNum = MathUtils.Subtraction(userGridNum, minGridNum, 0);
            for (int i = 1; i <= subGridNum; i++)
            {
                if (i == 0) latticeSpar = 0;
                if (i == 1) latticeSpar = 2;
                if (i >= 2) latticeSpar = i * 2;
                Sum += latticeSpar;
            }

            for (int j = 0; j <= minusNum; j++)
            {
                if (j == 0) latticeSpar = 0;
                if (j == 1) latticeSpar = 2;
                if (j >= 2) latticeSpar = j * 2;
                Sub += latticeSpar;
            }
            return MathUtils.Subtraction(Sum, Sub, 0);
        }
    }
}