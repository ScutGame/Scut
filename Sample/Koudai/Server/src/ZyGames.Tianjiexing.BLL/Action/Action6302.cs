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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;
using System;



namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 参加工会晨练
    /// </summary>
    public class Action6302 : BaseAction
    {
        private int ColdTime = 0;
        private GuildQuestion Question = null;
        private int Level = 0;
        private int Status = 0;
        private int QuestionNo = 0;
        private int QueueStatus = 0;
        private int IsAuto = 0;

        public Action6302(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6302, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(Status);
            PushIntoStack(ColdTime);
            PushIntoStack(Level);
            PushIntoStack(QuestionNo);
            PushIntoStack(Question == null ? string.Empty : Question.Question);
            PushIntoStack(Question == null ? string.Empty : Question.Option_A);
            PushIntoStack(Question == null ? string.Empty : Question.Option_B);
            PushIntoStack(Question == null ? string.Empty : Question.Option_C);
            PushIntoStack(Question == null ? string.Empty : Question.Option_D);
            PushIntoStack(QueueStatus);
            PushIntoStack(IsAuto);

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseIsOpen;
                return false;
            }
            GameActive gameactive = new ShareCacheStruct<GameActive>().FindKey(11);
            if (gameactive == null || !gameactive.State)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseIsOpen;
                return false;
            }
            gameactive.BeginTime = gameactive.EnablePeriod.ToDateTime(DateTime.MinValue);
            gameactive.EndTime = gameactive.BeginTime.AddMinutes(gameactive.Minutes);
            DateTime readytime = gameactive.BeginTime.AddMinutes(0 - gameactive.WaitMinutes);
            if (gameactive.BeginTime > DateTime.Now)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseIsOpen;
                return false;
            }
            if (gameactive.EndTime < DateTime.Now.AddSeconds(2))
            {
                ContextUser.UserLocation = Location.City;
                Status = 3;
                ColdTime = 10;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseClose;
                return true;
            }
            if (guild.GuildExercise == null)
            {
                ContextUser.UserLocation = Location.City;
                Status = 3;
                ColdTime = 10;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseClose;
                return true;
            }
            if (guild.GuildExercise.UserList.Count == 0)
            {
                ContextUser.UserLocation = Location.City;
                Status = 3;
                ColdTime = 10;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseClose;
                return true;
            }
            ExerciseUser exUser = guild.GuildExercise.UserList.Find(u => u.UserID == ContextUser.UserID);
            if (exUser == null)
            {
                ContextUser.UserLocation = Location.City;
                Status = 3;
                ColdTime = 10;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseClose;
                return true;
            }

            Question = GuildExerciseHelper.GetQuestion(guild, gameactive, exUser, ref ColdTime);
            if (Question == null)
            {
                Question = null;
            }
            if (Question != null)
            {
                Level = guild.GuildExercise.Level;
                QuestionNo = guild.GuildExercise.QuestionNo;
            }

            if (guild.GuildExercise.QuestionNo - exUser.QuestionNo > 5)
            {

                ContextUser.UserLocation = Location.City;
                Status = 3;
                ColdTime = 10;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseTimeOut;
                return true;
            }
            if (guild.GuildExercise.CheckAllAnswer == 0)
            {
                if (guild.GuildExercise.QuestionNo != exUser.QuestionNo)
                {
                    exUser.QuestionStatus = GuildQuestionStatus.ToAnswer;
                }
            }
            else
            {
                exUser.QuestionStatus = GuildQuestionStatus.ToNext;
            }
            Status = guild.GuildExercise.Status;
            QueueStatus = (int)exUser.QuestionStatus;
            IsAuto = exUser.Status == GuildExerciseStatus.All ? 1 : 0;
            return true;
        }
    }
}