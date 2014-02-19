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
using ServiceStack.ServiceInterface.ServiceModel;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1451_法宝修炼界面接口
    /// </summary>
    public class Action1451 : BaseAction
    {
        private int lifeNum = 0;
        private short powerNum;
        private short soulNum;
        private short intelligenceNum;
        private short isPractice;
        private short iscomplete;
        private StoryTaskInfo[] storyTaskArray = new StoryTaskInfo[0];

        public Action1451(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1451, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(lifeNum);
            this.PushIntoStack((short)powerNum);
            this.PushIntoStack((short)soulNum);
            this.PushIntoStack((short)intelligenceNum);
            this.PushIntoStack((short)isPractice);
            this.PushIntoStack(storyTaskArray.Length);
            foreach (var task in storyTaskArray)
            {
                int collectNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, task.TargetItemID);
                iscomplete = IsComplete(ContextUser, task);
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(task.PlotID);
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(task.TargetItemID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(task.TaskID);
                dsItem.PushIntoStack(task.TaskName.ToNotNullString());
                dsItem.PushIntoStack(task.TaskDescp.ToNotNullString());
                dsItem.PushIntoStack(plotInfo == null ? 0 : plotInfo.CityID);
                dsItem.PushIntoStack(task.PlotID);
                dsItem.PushIntoStack(task.TargetItemID);
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.ItemName.ToNotNullString());
                dsItem.PushIntoStack(itemInfo == null ? string.Empty : itemInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(collectNum);
                dsItem.PushIntoStack(task.TargetItemNum);
                dsItem.PushIntoStack((short)iscomplete);

                this.PushIntoStack(dsItem);
            }

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
            short trumpLv = 1;
            TrumpInfo trumpInfo = new ConfigCacheSet<TrumpInfo>().FindKey(TrumpInfo.CurrTrumpID, trumpLv);
            if (trumpInfo != null && trumpInfo.Property.Count > 0)
            {
                lifeNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.ShengMing).ToInt();
                powerNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.PowerNum);
                soulNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.SoulNum);
                intelligenceNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.IntelligenceNum);
            }
            storyTaskArray = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.TaskType == TaskType.Trump).ToArray();
            if (TrumpHelper.IsTrumpPractice(ContextUser.UserID))
            {
                isPractice = 1;
            }
            else
            {
                iscomplete = 0;
            }
            return true;
        }

        public static short IsComplete(GameUser user, StoryTaskInfo taskInfo)
        {
            if (user.UserLv >= taskInfo.TaskLv)
            {
                int collectNum = TrumpHelper.GetUserItemNum(user.UserID, taskInfo.TargetItemID);
                if (collectNum >= taskInfo.TargetItemNum)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 2;
        }
    }
}