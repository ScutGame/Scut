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
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 11002_完成探险答题接口
    /// </summary>
    public class Action11002 : BaseAction
    {
        private int questionID = 0;
        private int isRemove = 0;
        private int codeTime = 0;
        private int answerID = 0;
        private int rewardNumlv = 0;
        private List<RewardShow> rewardShowArray = new List<RewardShow>();

        public Action11002(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action11002, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(codeTime);
            PushIntoStack(rewardShowArray.Count);
            foreach (RewardShow show in rewardShowArray)
            {
                DataStruct dsItem = new DataStruct();

                rewardNumlv = 0;
                if (show.RewardType == RewardType.GameGoin || show.RewardType == RewardType.Experience)
                {
                    rewardNumlv = (show.RewardNum * ContextUser.UserLv);
                }
                else
                {
                    rewardNumlv = show.RewardNum;
                }
                dsItem.PushIntoStack((short)show.RewardType);
                dsItem.PushIntoStack(rewardNumlv);
                PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("QuestionID", ref questionID)
                && httpGet.GetInt("AnswerID", ref answerID)
                 && httpGet.GetInt("IsRemove", ref isRemove))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //List<UserQueue> queueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.TianXianStrong);

            //if (queueArray.Count > 0)
            //{
            //    int codeDate = queueArray[0].DoRefresh();
            //    if (codeDate > 0)
            //    {
            //        ErrorCode = LanguageManager.GetLang().ErrorCode;
            //        ErrorInfo = LanguageManager.GetLang().St11002_Colding;
            //        return false;
            //    }
            //}

            UserExpedition userExpedition = new GameDataCacheSet<UserExpedition>().FindKey(ContextUser.UserID);
            if (userExpedition != null && userExpedition.ExpeditionNum >= 10 && userExpedition.InsertDate.Date == DateTime.Now.Date)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St11002_ExpeditionFull;
                return false;
            }

            if (userExpedition != null && userExpedition.DoRefresh() > 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St11002_Colding;
                return false;
            }

            int sumGold = MathUtils.Addition((GetExpCodeTime(ContextUser.UserID) / 60), 1);
            if (isRemove == 1)
            {
                if (ContextUser.GoldNum < sumGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                codeTime = 0;

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, sumGold);
            }
            else if (isRemove == 2)
            {
                codeTime = GetExpCodeTime(ContextUser.UserID);

                //if (queueArray.Count > 0)
                //{
                //    queueArray[0].ColdTime = codeTime;
                //    queueArray[0].TotalColdTime = codeTime;
                //    queueArray[0].Timing = DateTime.Now;
                //    //queueArray[0].Update();
                //}
                //else
                //{
                //    UserQueue queue = new UserQueue()
                //    {
                //        QueueID = Guid.NewGuid().ToString(),
                //        QueueType = QueueType.TianXianStrong,
                //        QueueName = QueueType.TianXianStrong.ToString(),
                //        UserID = ContextUser.UserID,
                //        ColdTime = codeTime,
                //        IsSuspend = false,
                //        TotalColdTime = codeTime,
                //        Timing = DateTime.Now,
                //    };
                //    new GameDataCacheSet<UserQueue>().Add(queue, ContextUser.UserID.ToInt());
                //}
            }

            if (userExpedition == null)
            {
                userExpedition = new UserExpedition()
                 {
                     UserID = ContextUser.UserID,
                 };
                new GameDataCacheSet<UserExpedition>().Add(userExpedition, ContextUser.UserID.ToInt());
            }
            if (DateTime.Now.Date == userExpedition.InsertDate.Date)
            {
                userExpedition.ExpeditionNum = MathUtils.Addition(userExpedition.ExpeditionNum, 1, int.MaxValue);
            }
            else
            {
                userExpedition.ExpeditionNum = 1;
            }
            userExpedition.CodeTime = codeTime;
            userExpedition.InsertDate = DateTime.Now;

            ExpeditionInfo expInfo = new ConfigCacheSet<ExpeditionInfo>().FindKey(questionID);
            if (expInfo != null)
            {
                if (answerID == 1)
                {
                    rewardShowArray = expInfo.RewardNum1.ToList();
                }
                else if (answerID == 2)
                {
                    rewardShowArray = expInfo.RewardNum2.ToList();
                }
                GuildMember member = new ShareCacheStruct<GuildMember>().FindKey(ContextUser.MercenariesID, ContextUser.UserID);

                foreach (RewardShow show in rewardShowArray)
                {
                    if (show.RewardType == RewardType.Obtion)
                    {
                        if (member != null)
                        {
                            GuildMemberLog.AddLog(member.GuildID, new MemberLog()
                                                                      {
                                                                          UserID = ContextUser.UserID,
                                                                          LogType = 1,
                                                                          IdolID = 0,
                                                                          GainObtion = show.RewardNum,
                                                                          GainAura = 0,
                                                                          Experience = show.RewardNum,
                                                                          InsertDate = DateTime.Now
                                                                      });
                            UserHelper.Contribution(ContextUser.UserID, show.RewardNum);
                        }
                        ContextUser.ObtainNum = MathUtils.Addition(ContextUser.ObtainNum, show.RewardNum, int.MaxValue);
                    }
                    else if (show.RewardType == RewardType.GameGoin)
                    {
                        ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, (show.RewardNum * ContextUser.UserLv), int.MaxValue);
                    }
                    else if (show.RewardType == RewardType.ExpNum)
                    {
                        ContextUser.ExpNum = MathUtils.Addition(ContextUser.ExpNum, show.RewardNum, int.MaxValue);
                    }
                    else if (show.RewardType == RewardType.Experience)
                    {
                        UserHelper.UserGeneralExp(ContextUser.UserID, (show.RewardNum * ContextUser.UserLv));
                    }
                    else if (show.RewardType == RewardType.EnergyNum)
                    {
                        ContextUser.EnergyNum = MathUtils.Addition(ContextUser.EnergyNum, (short)show.RewardNum, short.MaxValue);
                    }
                    else if (show.RewardType == RewardType.Gold)
                    {
                        ContextUser.GiftGold = MathUtils.Addition(ContextUser.GiftGold, show.RewardNum, int.MaxValue);
                    }
                }
                string prizeContent = string.Empty;
                MysteryHelper.IsTriggerMyStery(ContextUser, MysteryType.Meiritanxian, out prizeContent);
                MysteryInfo mysteryInfo = new ConfigCacheSet<MysteryInfo>().FindKey(MysteryType.Meiritanxian);
                if (!string.IsNullOrEmpty(prizeContent) && mysteryInfo != null)
                {
                    string prompt = string.Format(mysteryInfo.Prompt, prizeContent);
                    string broadContent = string.Format(mysteryInfo.Broadcast, ContextUser.NickName, prizeContent);
                    var broadcastService = new TjxBroadcastService(null);
                    var msg = broadcastService.Create(NoticeType.System, broadContent);
                    broadcastService.Send(msg);
                    ErrorCode = 1;
                    ErrorInfo = prompt;
                }
            }
            return true;
        }

        public int GetExpCodeTime(string userID)
        {
            int coldTime = 0;
            UserExpedition userExp = new GameDataCacheSet<UserExpedition>().FindKey(userID);
            if (userExp != null && DateTime.Now.Date == userExp.InsertDate.Date && userExp.ExpeditionNum > 0)
            {
                if (userExp.ExpeditionNum >= 9)
                {
                    coldTime = 0;
                }
                else
                {
                    coldTime = MathUtils.Addition((userExp.ExpeditionNum * 60), 20, int.MaxValue);
                }
            }
            else
            {
                coldTime = 20;
            }
            return coldTime;
        }

        public int GetExpRewardNum(RewardType rewardType, int BaseNum, int userLv)
        {
            int rewardNum = 0;
            if (rewardType == RewardType.Experience)
            {
                rewardNum = BaseNum * userLv;
            }
            else if (rewardType == RewardType.GameGoin)
            {
                rewardNum = BaseNum * userLv;
            }
            else
            {
                rewardNum = BaseNum;
            }
            return rewardNum;
        }
    }
}