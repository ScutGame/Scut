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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1452_法宝修炼接口
    /// </summary>
    public class Action1452 : BaseAction
    {
        public Action1452(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1452, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                return true;
            }
        }

        public override bool TakeAction()
        {
            if (!UserHelper.IsOpenFunction(ContextUser.UserID, FunctionEnum.Trump))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_NoFun;
                return false;
            }
            if (UserHelper.IsOpenFunction(ContextUser.UserID, FunctionEnum.TrumpPractice))
            {
                return false;
            }
            if (!TrumpHelper.IsTrumpPractice(ContextUser.UserID))
            {
                return false;
            }
            var cacheTrump = new GameDataCacheSet<UserTrump>();
            UserTrump userTrump = cacheTrump.FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump == null)
            {
                TrumpInfo trumpInfo = new ConfigCacheSet<TrumpInfo>().FindKey(TrumpInfo.CurrTrumpID, 1);
                if (trumpInfo == null)
                {
                    return false;
                }
                StoryTaskInfo[] storyTaskArray = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.TaskType == TaskType.Trump).ToArray();
                foreach (StoryTaskInfo taskInfo in storyTaskArray)
                {
                    UserItemHelper.UseUserItem(ContextUser.UserID, taskInfo.TargetItemID, taskInfo.TargetItemNum);
                }
                userTrump = new UserTrump(ContextUser.UserID, TrumpInfo.CurrTrumpID);
                userTrump.TrumpLv = 1;
                userTrump.WorshipLv = 1;
                userTrump.LiftNum = trumpInfo.MaxLift;
                userTrump.Experience = 0;
                userTrump.MatureNum = trumpInfo.BeseMature;
                userTrump.Zodiac = TrumpHelper.GetZodiacType(ZodiacType.NoZodiac);
                //userTrump.SkillInfo = new List<SkillInfo>();
                //userTrump.PropertyInfo = new List<GeneralProperty>();
                cacheTrump.Add(userTrump);
                cacheTrump.Update();
            }
            var cacheSet = new GameDataCacheSet<UserFunction>();
            UserFunction userFunction = cacheSet.FindKey(ContextUser.UserID, FunctionEnum.TrumpPractice);
            if (userFunction == null)
            {
                UserFunction function = new UserFunction()
                {
                    FunEnum = FunctionEnum.TrumpPractice,
                    UserID = ContextUser.UserID,
                    CreateDate = DateTime.Now,
                };
                cacheSet.Add(function);
                cacheSet.Update();
            }
            var usergeneral = UserGeneral.GetMainGeneral(ContextUser.UserID);
            if (userTrump.LiftNum > 0 && usergeneral != null)
            {
                usergeneral.RefreshMaxLife();
            }
            return true;
        }
    }
}