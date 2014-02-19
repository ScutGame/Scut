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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.ConfigModel;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 1402_酒馆佣兵列表接口
    /// </summary>
    public class Action1402 : BaseAction
    {
        private List<RecruitRule> recruitList = new List<RecruitRule>();

        public Action1402(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1402, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(ContextUser.GoldNum);
            PushIntoStack(ContextUser.GameCoin);
            PushIntoStack(recruitList.Count);
            foreach (RecruitRule rule in recruitList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)rule.RecruitType);
                dsItem.PushIntoStack(GetGeneralQuality(rule.GeneralQuality).ToNotNullString());
                dsItem.PushIntoStack(GeneralHelper.SurplusNum(ContextUser.UserID, rule.FreeNum, rule.RecruitType.ToEnum<RecruitType>()));
                dsItem.PushIntoStack(rule.FreeNum);
                dsItem.PushIntoStack(rule.GoldNum);
                dsItem.PushIntoStack(GeneralHelper.UserQueueCodeTime(ContextUser.UserID, rule.RecruitType.ToEnum<RecruitType>()));

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            recruitList = new ConfigCacheSet<RecruitRule>().FindAll();
            return true;
        }

        public override bool TakeAction()
        {
            if (true)
            {
                return true;
            }
        }

        public static string GetGeneralQuality(CacheList<RecruitInfo> _generalQuality)
        {
            string genquality = string.Empty;
            foreach (var quality in _generalQuality)
            {
                genquality += quality.Quality.ToInt() + ",";
            }
            return genquality.TrimEnd(',');
        }
    }
}