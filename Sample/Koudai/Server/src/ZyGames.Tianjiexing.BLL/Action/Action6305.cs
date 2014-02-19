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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;
using System;
using ZyGames.Framework.Common;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 答题接口
    /// </summary>
    public class Action6305 : BaseAction
    {
        private int Answer = 0;
        private int QuestionNo = 0;
        private int ops = 0;
        private int baseexpen = 12;
        private int Result = 0;

        public Action6305(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6305, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(Result);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("QuestionNo", ref QuestionNo))
            {
                httpGet.GetInt("ops", ref ops);
                httpGet.GetInt("Answer", ref Answer);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ErrorCode = ops;
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            GameActive gameactive = new ShareCacheStruct<GameActive>().FindKey(11);
            if (gameactive == null || !gameactive.State)
            {
                return false;
            }
            gameactive.BeginTime = gameactive.EnablePeriod.ToDateTime(DateTime.MinValue);
            gameactive.EndTime = gameactive.BeginTime.AddMinutes(gameactive.Minutes);
            DateTime readytime = gameactive.BeginTime.AddMinutes(0 - gameactive.WaitMinutes);
            if (readytime > DateTime.Now)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseNoOpen;
                return false;
            }
            if (guild.GuildExercise == null)
            {
                return false;
            }

            ExerciseUser exUser = guild.GuildExercise.UserList.Find(u => u.UserID == ContextUser.UserID);
            if (exUser == null)
            {
                return false;
            }
            if (QuestionNo != guild.GuildExercise.QuestionNo)
            {
                return false;
            }

            if (exUser.QuestionNo == guild.GuildExercise.QuestionNo)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6305_GuildExerciseISAnswer;
                return false;
            }

            //自动答题
            if (exUser.Status == GuildExerciseStatus.All)
            {
                ops = 4;
            }
            if (ops == 0)
            {
                exUser.QuestionNo = QuestionNo;
                GuildQuestion Question = new ConfigCacheSet<GuildQuestion>().FindKey(guild.GuildExercise.QuestionID);
                if (Question != null && Question.Answer == Answer)
                {
                    exUser.AnswerStatus = true;
                }
                else
                {
                    exUser.AnswerStatus = false;
                }
            }
            int expen = 0;
            if (ops == 1)//晶石询问
            {
                expen = baseexpen + exUser.GameConisCount * 2;
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St6305_GuildExerciseGoldAnswer, expen);
                return false;
            }
            if (ops == 2)//晶石确认
            {
                expen = baseexpen + exUser.GameConisCount * 2;
                //晶石不足
                if (ContextUser.GoldNum < expen)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                exUser.QuestionNo = QuestionNo;
                exUser.AnswerStatus = true;
                exUser.GameConisCount = MathUtils.Addition(exUser.GameConisCount, 1);
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, expen, int.MaxValue);
                //ContextUser.Update();
            }
            if (ops == 3)//自动答题询问
            {
                int expenAll = 200;
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St6305_GuildExerciseAutoAnswer, expenAll);
                return false;
            }
            if (ops == 4)//自动答题
            {
                if (exUser.Status != GuildExerciseStatus.All)
                {
                    int expenAll = 200;
                    //晶石不足
                    if (ContextUser.GoldNum < expenAll)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                    exUser.Status = GuildExerciseStatus.All;
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, expenAll, int.MaxValue);
                    //ContextUser.Update();
                }
                exUser.QuestionNo = QuestionNo;
                exUser.AnswerStatus = true;
            }
            Result = exUser.AnswerStatus ? 1 : 0;
            //奖励
            if (QuestionNo == guild.GuildExercise.QuestionNo
                && exUser.AnswerStatus)
            {
                int Expen = 0;
                int Experience = 0;
                GuildExerciseHelper.DoPrize(guild, gameactive, exUser, ContextUser, ref Expen, ref Experience);
                ErrorInfo = string.Format(LanguageManager.GetLang().St6305_GuildExerciseAnswerSuss, Expen, Experience);
                //答对提示
            }
            else
            {
                ErrorInfo = LanguageManager.GetLang().St6305_GuildExerciseAnswerFail;
                new TjxChatService(ContextUser).SystemGuildSend(ChatType.Guild,
                                                    string.Format(LanguageManager.GetLang().St6305_GuildExerciseGuildChat, ContextUser.NickName));
            }
            exUser.QuestionStatus = GuildQuestionStatus.WaitForResults;
            //guild.Update();
            return true;
        }
    }
}