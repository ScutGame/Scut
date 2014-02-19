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
using ZyGames.Framework.Common.Log;
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
    public class Action6301 : BaseAction
    {
        public Action6301(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6301, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            ContextUser.UserLocation = Location.City;
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
            if (gameactive.BeginTime > DateTime.Now)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseNoOpen;
                return false;
            }
            if (gameactive.BeginTime.AddMinutes(gameactive.WaitMinutes) < DateTime.Now && gameactive.EndTime > DateTime.Now)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St6301_GuildExerciseIsOpen;
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
                TraceLog.ReleaseWriteFatal("重新刷了6301接口1");
                guild.GuildExercise = new GuildExercise();
            }
            if (guild.GuildExercise.QuestionNo > 0)
            {
                TraceLog.ReleaseWriteFatal("重新刷了6301接口2");
                guild.GuildExercise = new GuildExercise();
            }
            if (guild.GuildExercise.UserList.Find(u => u.UserID == ContextUser.UserID) == null)
            {
                ExerciseUser exUser = new ExerciseUser();
                exUser.AnswerStatus = false;
                exUser.QuestionNo = 0;
                exUser.Status = GuildExerciseStatus.Default;
                exUser.UserID = ContextUser.UserID;
                guild.GuildExercise.UpdateNotify(obj =>
                    {
                        guild.GuildExercise.UserList.Add(exUser);
                        return true;
                    });
                //guild.Update();                
            }
            ContextUser.UserLocation = Location.GuildExercise;
            //ContextUser.Update();
            return true;
        }
    }
}