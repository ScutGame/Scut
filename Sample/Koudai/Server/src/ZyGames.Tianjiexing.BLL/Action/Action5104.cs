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
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5104_增加挑战次数接口
    /// </summary>
    public class Action5104 : BaseAction
    {
        private int ops = 0;


        public Action5104(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5104, httpGet)
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
            UserChallengeNum userchallenge = new GameDataCacheSet<UserChallengeNum>().FindKey(ContextUser.UserID);

            int openGold = GetChallengeNum(ContextUser.UserID);
            if (ops == 1)
            {
                this.ErrorCode = 1;
                this.ErrorInfo = openGold.ToString();
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum >= openGold)
                {
                    this.ErrorCode = 2;
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, openGold, int.MaxValue);
                    int num = 0;

                    if (userchallenge == null)
                    {
                        num = 1;
                        UserChallengeNum userChallengeNum = new UserChallengeNum()
                        {
                            UserID = ContextUser.UserID,
                            ChallengeNum = 1,
                            InsertDate = DateTime.Now
                        };
                        new GameDataCacheSet<UserChallengeNum>().Add(userChallengeNum);
                    }
                    else
                    {
                        if (userchallenge.InsertDate.Date == DateTime.Now.Date)
                        {
                            num = MathUtils.Addition(userchallenge.ChallengeNum, 1, int.MaxValue);
                            userchallenge.ChallengeNum = num;

                        }
                        else
                        {
                            userchallenge.ChallengeNum = 1;
                            num = 1;
                        }
                        userchallenge.InsertDate = DateTime.Now;
                    }
                    UserLogHelper.AppenUseGoldLog(ContextUser.UserID, 4, 0, num, openGold,
                                                ContextUser.GoldNum,
                                                MathUtils.Addition(ContextUser.GoldNum, openGold, int.MaxValue));
                }
                else
                {
                    //todo 客户端跳转充值页面的特殊值 10
                    this.ErrorCode = LanguageManager.GetLang().RechargeError;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
            }
            else
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 增加挑战次数话费晶石
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int GetChallengeNum(string userID)
        {
            int openGold = 0;
            UserChallengeNum challengeNum = new GameDataCacheSet<UserChallengeNum>().FindKey(userID);
            if (challengeNum != null && DateTime.Now.Date == challengeNum.InsertDate.Date)
            {
                openGold = (MathUtils.Addition(challengeNum.ChallengeNum, 1, int.MaxValue) * 2);
            }
            else
            {
                openGold = 2;
            }
            return openGold;
        }
    }
}