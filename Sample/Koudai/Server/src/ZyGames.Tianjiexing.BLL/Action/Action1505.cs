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
using System.Collections.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1505_魂技配置信息下发接口
    /// </summary>
    public class Action1505 : BaseAction
    {
        private List<AbilityInfo> abilityList = new List<AbilityInfo>();


        public Action1505(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1505, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(abilityList.Count);
            foreach (AbilityInfo abilityInfo in abilityList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(abilityInfo.AbilityID);
                dsItem.PushIntoStack(abilityInfo.AbilityName.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.AttackUnit.ToShort());
                dsItem.PushIntoStack(abilityInfo.AttackTaget.ToShort());
                dsItem.PushIntoStack(abilityInfo.EffectCount.ToShort());
                dsItem.PushIntoStack(abilityInfo.IsMove ? (short)1 : (short)0);
                dsItem.PushIntoStack(abilityInfo.EffectDesc.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.AbilityDesc.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.EffectID1.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.EffectID2.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.MaxHeadID.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo.FntHeadID.ToNotNullString());
                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            //if (httpGet.GetInt("AbilityID", ref _abilityId))
            //{
            //    return true;
            //}
            return true;
        }

        public override bool TakeAction()
        {
            //int currVersion = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.Ability).CurVersion;
            abilityList = new ConfigCacheSet<AbilityInfo>().FindAll();
            return true;
        }
    }
}