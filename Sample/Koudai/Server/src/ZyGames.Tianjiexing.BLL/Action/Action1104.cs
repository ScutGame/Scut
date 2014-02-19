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
    /// 1104_背包格子开启接口
    /// </summary>
    public class Action1104 : BaseAction
    {
        private int ops = 0;
        private int latticeNum = 0;

        public Action1104(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1104, httpGet)
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
            if (latticeNum > ContextUser.GridNum)
            {
                int openGold = GetOpenGoldNum(latticeNum, ContextUser.GridNum);
                int UserGridNum = ContextUser.GridNum;
                int OpenGridNum = MathUtils.Addition(UserGridNum, latticeNum, 48);//开启的格子数
                int subGridNum = MathUtils.Subtraction(latticeNum, ContextUser.GridNum, 0);
                if (ops == 1)
                {
                    //开启第3个格子所需的晶石120
                    this.ErrorCode = 1;
                    this.ErrorInfo = string.Format(LanguageManager.GetLang().St1104_UseGold, subGridNum, openGold);
                    return false;
                }
                else if (ops == 2)
                {
                    if (ContextUser.GoldNum >= openGold)
                    {
                        this.ErrorCode = 2;

                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, openGold, int.MaxValue);
                        ContextUser.GridNum = latticeNum;
                        //ContextUser.Update();
                        UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 5, latticeNum, OpenGridNum, openGold,
                                                ContextUser.GoldNum,
                                                MathUtils.Addition(ContextUser.GoldNum, openGold, int.MaxValue));
                    }
                    else
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                }
                else
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
            }
            return true;
        }

        public static int GetOpenGoldNum(int gNum, int userGridNum)
        {
            int latticeSpar = 0; //开启每个格子的价格
            int sum = 0;
            int sub = 0;
            int minGridNum = ConfigEnvSet.GetInt("User.GridMinNum"); //物品背包初始开启格子数值
            int subGridNum = MathUtils.Subtraction(gNum, minGridNum);

            int minusNum = MathUtils.Subtraction(userGridNum, minGridNum);
            for (int i = 1; i <= subGridNum; i++)
            {
                if (i == 0) latticeSpar = 0;
                if (i == 1) latticeSpar = 2;
                if (i >= 2) latticeSpar = i * 2;
                sum += latticeSpar;
            }

            for (int j = 0; j <= minusNum; j++)
            {
                if (j == 0) latticeSpar = 0;
                if (j == 1) latticeSpar = 2;
                if (j >= 2) latticeSpar = j * 2;
                sub += latticeSpar;
            }
            return MathUtils.Subtraction(sum, sub); 
        }
    }
}