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
    /// 获取所有玩家结果接口
    /// </summary>
    public class Action6303 : BaseAction
    {
        private int QuestionNo = 0;

        public Action6303(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6303, httpGet)
        {
        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("QuestionNo", ref QuestionNo))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
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
            if (gameactive.EndTime < DateTime.Now)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseClose;
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


            if ((DateTime.Now - guild.GuildExercise.QuesTime).TotalSeconds < 20)
            {
                return false;
            }
            if (guild.GuildExercise.CheckAllAnswer == 0)
            {
                checkAnswer(guild);
                if (guild.GuildExercise.CheckAllAnswer == 1)
                {
                    if (guild.GuildExercise.Level < 20)
                        guild.GuildExercise.Level = MathUtils.Addition(guild.GuildExercise.Level, 1);
                }
                if (guild.GuildExercise.CheckAllAnswer == 2)
                {
                    guild.GuildExercise.Level = guild.GuildExercise.Level = 1;
                }
                //guild.Update();
            }
            if (guild.GuildExercise.CheckAllAnswer == 1)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6303_GuildExerciseAllAnswerTrue;
            }
            if (guild.GuildExercise.CheckAllAnswer == 2)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6303_GuildExerciseAllAnswerFalse;
            }
            return true;
        }

        /// <summary>
        /// 检查答题状态
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        private bool checkAnswer(UserGuild guild)
        {
            if (guild.GuildExercise.CheckAllAnswer != 0)
                return false;
            foreach (ExerciseUser user in guild.GuildExercise.UserList)
            {
                if (user.Status == GuildExerciseStatus.All)
                {
                    continue;
                }
                if (user.QuestionNo != guild.GuildExercise.QuestionNo)
                {
                    if (guild.GuildExercise.QuestionNo - user.QuestionNo >= 5)
                    {
                        continue;
                    }
                    else
                    {
                        guild.GuildExercise.CheckAllAnswer = 2;
                        return false;
                    }
                }
                if (!user.AnswerStatus)
                {
                    guild.GuildExercise.CheckAllAnswer = 2;
                    return false;
                }
            }
            guild.GuildExercise.CheckAllAnswer = 1;
            return true;
        }
    }
}