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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1416_传承界面接口
    /// </summary>
    public class Action1416 : BaseAction
    {
        private int ops;
        private string heritageName;
        private short heritageLv;
        private string disGeneralName;
        private List<GeneralHeritage> heritageList = new List<GeneralHeritage>();
        private List<OpsInfo> opsInfoList = new List<OpsInfo>();

        public Action1416(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1416, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(heritageList.Count);
            foreach (var item in heritageList)
            {
                UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, item.GeneralID);
                GeneralInfo general = new ConfigCacheSet<GeneralInfo>().FindKey(item.GeneralID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)item.Type);
                dsItem.PushIntoStack(userGeneral == null ? 0 : userGeneral.GeneralID);
                dsItem.PushIntoStack(userGeneral == null ? string.Empty : ObjectExtend.ToNotNullString(userGeneral.GeneralName));
                dsItem.PushIntoStack(general == null ? string.Empty : ObjectExtend.ToNotNullString(general.BattleHeadID));
                dsItem.PushIntoStack(item.GeneralLv);
                dsItem.PushIntoStack(item.PowerNum);
                dsItem.PushIntoStack(item.SoulNum);
                dsItem.PushIntoStack(item.IntellectNum);
                dsItem.PushIntoStack(userGeneral == null ? (short)0 : (short)userGeneral.GeneralQuality);
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(opsInfoList.Count);
            foreach (var item in opsInfoList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.Type);
                dsItem.PushIntoStack(item.VipLv);
                dsItem.PushIntoStack(item.UseGold);
                dsItem.PushIntoStack(item.ItemID);
                dsItem.PushIntoStack(item.ItemNum);
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(heritageName.ToNotNullString());
            this.PushIntoStack(heritageLv);
            this.PushIntoStack(disGeneralName.ToNotNullString());

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (ContextUser.HeritageList.Count > 0)
            {
                heritageList = ContextUser.HeritageList.ToList();
            }
            var heritage = heritageList.Find(s => s.Type == HeritageType.Heritage);

            if (heritage != null)
            {
                UserGeneral uGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, heritage.GeneralID);
                disGeneralName = uGeneral == null ? string.Empty : uGeneral.GeneralName;
                heritageLv = heritage.GeneralLv;
            }
            var heritageGeneral = heritageList.Find(s => s.Type == HeritageType.IsHeritage);
            if (heritageGeneral != null)
            {
                UserGeneral uGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, heritageGeneral.GeneralID);
                heritageName = uGeneral == null ? string.Empty : uGeneral.GeneralName;
            }

            GeneralHelper.HeritageGeneral(ContextUser, ops);
            if (!string.IsNullOrEmpty(GameConfigSet.HeritageList))
            {
                opsInfoList = JsonUtils.Deserialize<List<OpsInfo>>(GameConfigSet.HeritageList);
                var opsInfo = opsInfoList.Find(s => s.Type == ops);
                if (opsInfo != null && ContextUser.VipLv < opsInfo.VipLv)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                    return false;
                }
            }
            return true;
        }
    }
}