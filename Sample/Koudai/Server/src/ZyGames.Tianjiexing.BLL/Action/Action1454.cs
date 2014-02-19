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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1454_法宝详细信息接口
    /// </summary>
    public class Action1454 : BaseAction
    {
        private short trumpLv;
        private string growth;
        private short matureType;
        private int currLiftNum;
        private short powerNum;
        private short soulNum;
        private short intelligenceNum;
        private int upLiftNum;
        private short upPower;
        private short upSoul;
        private short upIntelligence;
        private UserTrump userTrump = null;
        private WorshipInfo[] worshipInfoInfoArray = new WorshipInfo[0];
        private TrumpInfo[] trumpInfoArray = new TrumpInfo[0];
        private int totalNum = 0;
        private int propertycount;
        private int skcount = 0;
        private short worshipLv;

        public Action1454(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1454, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(trumpLv);
            this.PushIntoStack(growth.ToNotNullString());
            this.PushIntoStack((short)matureType);
            this.PushIntoStack(currLiftNum);
            this.PushIntoStack((short)powerNum);
            this.PushIntoStack((short)soulNum);
            this.PushIntoStack((short)intelligenceNum);
            this.PushIntoStack(upLiftNum);
            this.PushIntoStack((short)upPower);
            this.PushIntoStack((short)upSoul);
            this.PushIntoStack((short)upIntelligence);
            this.PushIntoStack(worshipInfoInfoArray.Length);
            foreach (var item in worshipInfoInfoArray)
            {
                short isOpen = 0;
                GeneralProperty gProperty = null;
                if (userTrump != null && userTrump.PropertyInfo.Count > propertycount)
                {
                    gProperty = userTrump.PropertyInfo[propertycount];
                }
                if (worshipLv >= item.WorshipLv)
                {
                    isOpen = 1;
                }
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(gProperty == null ? (short)AbilityType.Empty : (short)gProperty.AbilityType);
                dsItem.PushIntoStack(gProperty == null ? (short)0 : (short)gProperty.AbilityLv);
                dsItem.PushIntoStack(gProperty == null ? AbilityType.Empty.ToNotNullString() : gProperty.AbilityValue.ToNotNullString());
                dsItem.PushIntoStack(isOpen);
                dsItem.PushIntoStack(totalNum > propertycount ? (short)1 : (short)0);
                this.PushIntoStack(dsItem);
                propertycount++;
            }
            this.PushIntoStack(trumpInfoArray.Length);
            for (int i = 0; i < trumpInfoArray.Length; i++)
            {
                AbilityInfo abilityInfo = TrumpHelper.GetAbilityInfo(ContextUser.UserID, i); ;
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(abilityInfo == null ? (short)0 : (short)abilityInfo.AbilityID);
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityName.ToNotNullString());
                dsItem.PushIntoStack(abilityInfo == null ? string.Empty : abilityInfo.AbilityDesc.ToNotNullString());
                dsItem.PushIntoStack(skcount > i ? (short)1 : (short)0);

                this.PushIntoStack(dsItem);
            }
            PushIntoStack(trumpLv >= GameConfigSet.MaxTrumpLv ? (short)1 : (short)0);
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
            userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null)
            {
                worshipLv = userTrump.WorshipLv;
                growth = userTrump.MatureNum.ToString();
                trumpLv = userTrump.TrumpLv;
                matureType = TrumpHelper.GetEnumMatureType(userTrump.MatureNum).ToShort();
                TrumpInfo trumpInfo = new ConfigCacheSet<TrumpInfo>().FindKey(TrumpInfo.CurrTrumpID, userTrump.TrumpLv);
                if (trumpInfo != null && trumpInfo.Property.Count > 0)
                {
                    currLiftNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.ShengMing).ToInt();
                    powerNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.PowerNum);
                    soulNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.SoulNum);
                    intelligenceNum = TrumpHelper.GetTrumpProperty(trumpInfo, AbilityType.IntelligenceNum);
                    upLiftNum = (int)Math.Floor(currLiftNum * TrumpHelper.GetMatureNum(userTrump.MatureNum));
                    upPower = (short)Math.Floor(powerNum * TrumpHelper.GetMatureNum(userTrump.MatureNum));
                    upSoul = (short)Math.Floor(soulNum * TrumpHelper.GetMatureNum(userTrump.MatureNum));
                    upIntelligence = (short)Math.Floor(intelligenceNum * TrumpHelper.GetMatureNum(userTrump.MatureNum));
                }
                totalNum = userTrump.PropertyInfo.Count;
                skcount = userTrump.SkillInfo.Count;
            }
            trumpInfoArray = new ConfigCacheSet<TrumpInfo>().FindAll(m => m.SkillID > 0 && m.TrumpID == TrumpInfo.CurrTrumpID).ToArray();
            worshipInfoInfoArray = new ConfigCacheSet<WorshipInfo>().FindAll(m => m.IsOpen && m.TrumpID == TrumpInfo.CurrTrumpID).ToArray();
            return true;
        }
    }
}