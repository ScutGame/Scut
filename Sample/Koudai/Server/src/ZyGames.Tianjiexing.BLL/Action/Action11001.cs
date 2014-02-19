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
using System.Collections.Generic;
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;



namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 11001_探险答题接口
    /// </summary>
    public class Action11001 : BaseAction
    {
        private int answerID1 = 0;
        private int answerID2 = 0;
        private int isEnd = 0;
        private ExpeditionInfo expInfo = null;
        private int codeTime = 0;
        private int useGold = 0;

        public Action11001(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action11001, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(expInfo == null ? 0 : expInfo.ExpID);
            PushIntoStack(expInfo == null ? string.Empty : expInfo.ExpName.ToNotNullString());
            PushIntoStack(answerID1);
            PushIntoStack(expInfo == null ? string.Empty : expInfo.Answer1.ToNotNullString());
            PushIntoStack(answerID2);
            PushIntoStack(expInfo == null ? string.Empty : expInfo.Answer2.ToNotNullString());
            PushIntoStack(isEnd);
            PushIntoStack(codeTime);
            PushIntoStack(useGold);
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            int question = 0;
            int index = 0;
            answerID1 = 1;
            answerID2 = 2;
            int expID = 0;
            int usegoldTime = GetExpCodeTime(ContextUser.UserID);
            useGold = MathUtils.Addition((usegoldTime / 60), 1, int.MaxValue);
            UserExpedition userExp = new GameDataCacheSet<UserExpedition>().FindKey(ContextUser.UserID);

            if (userExp != null && DateTime.Now.Date == userExp.InsertDate.Date)
            {
                if (userExp.ExpeditionNum >= 10)
                {
                    isEnd = 1;
                }
                else
                {
                    isEnd = 2;
                }
                question = MathUtils.Addition(userExp.ExpeditionNum, 1, int.MaxValue);
                expID = userExp.ExpID;
                codeTime = userExp.DoRefresh();
            }
            else
            {
                isEnd = 2;
                question = 1;
            }

            List<ExpeditionInfo> expeditionArray = new ConfigCacheSet<ExpeditionInfo>().FindAll(m => m.GroupID == question);
            if (expeditionArray.Count > 0)
            {
                if (IsAnswer(expeditionArray, expID))
                {
                    expInfo = new ConfigCacheSet<ExpeditionInfo>().FindKey(expID);
                }
                else
                {
                    index = RandomUtils.GetRandom(0, expeditionArray.Count);
                    expInfo = new ConfigCacheSet<ExpeditionInfo>().FindKey(expeditionArray[index].ExpID);
                    if (userExp != null)
                    {
                        userExp.ExpID = expeditionArray[index].ExpID;
                    }
                }
            }
            return true;
        }

        public int GetExpCodeTime(string userID)
        {
            int coldTime = 0;
            UserExpedition userExp = new GameDataCacheSet<UserExpedition>().FindKey(userID);
            if (userExp == null)
            {
                return coldTime;
            }
            if (DateTime.Now.Date == userExp.InsertDate.Date)
            {
                coldTime = MathUtils.Addition((userExp.ExpeditionNum * 60), 20, int.MaxValue);
            }
            return coldTime;
        }

        public static bool IsAnswer(List<ExpeditionInfo> expeditionArray, int expID)
        {
            foreach (ExpeditionInfo expeditionInfo in expeditionArray)
            {
                if (expeditionInfo.ExpID == expID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}