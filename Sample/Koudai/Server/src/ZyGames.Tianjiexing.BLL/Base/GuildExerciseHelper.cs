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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class GuildExerciseHelper
    {
        private static readonly int QuestionWaitTime = 30;

        private static int _length = 0;

        private static int Length
        {
            get
            {
                if (_length == 0)
                {
                    _length = new ConfigCacheSet<GuildQuestion>().FindAll().Count;
                }
                return _length;
            }
        }

        public static GuildQuestion GetQuestion(UserGuild userGuild, GameActive gameActive, ExerciseUser exUser, ref int ColdTime)
        {

            DateTime dt = DateTime.Now;
            DateTime beginTime = gameActive.BeginTime.AddMinutes(gameActive.WaitMinutes);
            if (dt < gameActive.BeginTime.AddMinutes(gameActive.WaitMinutes))
            {
                ColdTime = (int)(beginTime - dt).TotalSeconds;
                userGuild.GuildExercise.UpdateNotify(obj =>
                    {
                        userGuild.GuildExercise.Status = 1;
                        return true;
                    });
                return null;
            }

            GuildQuestion Question = new GuildQuestion();
            if (userGuild.GuildExercise.QuesTime != null
                && dt < userGuild.GuildExercise.QuesTime.AddSeconds(QuestionWaitTime)
                && userGuild.GuildExercise.QuestionID != 0)
            {
                Question = new ConfigCacheSet<GuildQuestion>().FindKey(userGuild.GuildExercise.QuestionID);
            }
            else
            {
                int questionid = GetID(userGuild.GuildExercise.QuestionIDList.ToList());
                userGuild.GuildExercise.QuestionID = questionid;
                Question = new ConfigCacheSet<GuildQuestion>().FindKey(userGuild.GuildExercise.QuestionID);
                userGuild.GuildExercise.UpdateNotify(obj =>
                {
                    if (userGuild.GuildExercise.QuesTime < beginTime)
                        userGuild.GuildExercise.QuesTime = beginTime;
                    int count = ((int)(dt - userGuild.GuildExercise.QuesTime).TotalSeconds) / QuestionWaitTime;
                    if (count <= 0)
                    {
                        count = 0;
                    }
                    userGuild.GuildExercise.QuestionNo = MathUtils.Addition(userGuild.GuildExercise.QuestionNo, (count <= 1 ? 1 : count), int.MaxValue);
                    userGuild.GuildExercise.QuesTime = beginTime.AddSeconds((userGuild.GuildExercise.QuestionNo - 1) * QuestionWaitTime);
                    userGuild.GuildExercise.Status = 2; //已开始活动
                    userGuild.GuildExercise.CheckAllAnswer = 0;
                    return true;
                });//userGuild.Update();
                exUser.QuestionStatus = GuildQuestionStatus.ToAnswer;
            }
            ColdTime = QuestionWaitTime - (int)(dt - userGuild.GuildExercise.QuesTime).TotalSeconds;

            if (userGuild.GuildExercise.CheckAllAnswer == 0)
            {
                ColdTime -= 10;
            }

            if (ColdTime <= 0)
            {
                ColdTime = 0;
                return null;
            }
            return Question;


        }

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <param name="intList"></param>
        /// <returns></returns>
        private static int GetID(List<int> intList)
        {
            for (int i = 0; i < 100; i++)
            {
                int id = RandomUtils.GetRandom(1, Length);
                if (!intList.Contains(id))
                {
                    intList.Add(id);
                    return id;
                }
            }
            return RandomUtils.GetRandom(1, Length);
        }

        /// <summary>
        /// 答题奖励
        /// </summary>
        /// <param name="userGuild"></param>
        /// <param name="gameActive"></param>
        /// <param name="exuser"></param>
        /// <param name="user"></param>
        public static void DoPrize(UserGuild userGuild, GameActive gameActive, ExerciseUser exuser, GameUser user, ref int expNum, ref int Experience)
        {
            DateTime dt = DateTime.Now;
            if (dt < gameActive.BeginTime)
            {
                userGuild.GuildExercise.Status = 0;
                return;
            }
            if (exuser.Status == GuildExerciseStatus.All ||
                (userGuild.GuildExercise.QuestionNo == exuser.QuestionNo
                && exuser.AnswerStatus))
            {
                GuildExercisePrize prize = new ConfigCacheSet<GuildExercisePrize>().FindKey(userGuild.GuildExercise.Level);
                if (prize == null)
                    return;
                decimal precent = GetExercisePrecent(userGuild);
                expNum = (int)Math.Floor((decimal)prize.ExpNum * precent);
                Experience = (int)Math.Floor((decimal)prize.Experience * precent);
                user.ExpNum = MathUtils.Addition(user.ExpNum, expNum, int.MaxValue);
                UserHelper.UserGeneralExp(user.UserID, Experience);
            }
        }

        /// <summary>
        /// 奖励加成
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        public static decimal GetExercisePrecent(UserGuild guild)
        {
            decimal precent = 0.05M;
            int count = 0;
            foreach (ExerciseUser user in guild.GuildExercise.UserList)
            {
                if (user.QuestionNo != guild.GuildExercise.QuestionNo)
                {
                    if (guild.GuildExercise.QuestionNo - user.QuestionNo >= 5)
                    {
                        continue;
                    }
                }
                count++;
            }
            count = count - 1;
            if (count <= 0)
                count = 0;
            return (decimal)1 + (decimal)count * precent;
        }
    }
}