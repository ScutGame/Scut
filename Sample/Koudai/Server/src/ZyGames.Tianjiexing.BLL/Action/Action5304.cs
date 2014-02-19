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

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5304_公会战进入战场接口
    /// </summary>
    public class Action5304 : BaseAction
    {
        private int Ops;
        private int Version;
        private string GuildName1 = "";
        private int MemberNum1 = 0;
        private string GuildName2 = "";
        private int MemberNum2 = 0;
        private int PerWinNum = 0;
        private int PerObtainNum = 0;
        private int PerGameCoin = 0;
        private int GuildWinNum = 0;
        private int GuildFailNum = 0;


        public Action5304(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5304, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(GuildName1);
            this.PushIntoStack(MemberNum1);
            this.PushIntoStack(GuildName2);
            this.PushIntoStack(MemberNum2);
            this.PushIntoStack(PerWinNum);
            this.PushIntoStack(PerObtainNum);
            this.PushIntoStack(PerGameCoin);
            this.PushIntoStack(GuildWinNum);
            this.PushIntoStack(GuildFailNum);
            this.PushIntoStack(Version);
            //this.PushIntoStack(dsItemCollection.Length);
            //foreach (var item in dsItemCollection)
            //{
            //    DataStruct dsItem = new DataStruct();
            //    dsItem.PushIntoStack(UserName1);
            //    dsItem.PushIntoStack(UserName2);
            //    dsItem.PushIntoStack(IsWin);
            //    dsItem.PushIntoStack(KillNum);
            //    dsItem.PushIntoStack(ObtainNum);
            //    dsItem.PushIntoStack(GameCoin);
            //    dsItem.PushIntoStack(GuildShiqi);
            //    dsItem.PushIntoStack(dsItem1Collection.Length);
            //    foreach (var item in dsItem1Collection)
            //    {
            //        DataStruct dsItem1 = new DataStruct();
            //        dsItem1.PushIntoStack(AttGeneralID);
            //        dsItem1.PushIntoStack(AttGeneralName);
            //        dsItem1.PushIntoStack(AttGeneralHeadID);
            //        dsItem1.PushIntoStack(AttPosition);
            //        dsItem1.PushIntoStack(LiveNum);
            //        dsItem1.PushIntoStack(LiveMaxNum);

            //        dsItem.PushIntoStack(dsItem1);
            //    }
            //    dsItem.PushIntoStack(dsItem1Collection.Length);
            //    foreach (var item in dsItem1Collection)
            //    {
            //        DataStruct dsItem1 = new DataStruct();
            //        dsItem1.PushIntoStack(DefGeneralID);
            //        dsItem1.PushIntoStack(DefGeneralName);
            //        dsItem1.PushIntoStack(DefGeneralHeadID);
            //        dsItem1.PushIntoStack(DefPosition);
            //        dsItem1.PushIntoStack(LiveNum);
            //        dsItem1.PushIntoStack(LiveMaxNum);

            //        dsItem.PushIntoStack(dsItem1);
            //    }
            //    dsItem.PushIntoStack(dsItem1Collection.Length);
            //    foreach (var item in dsItem1Collection)
            //    {
            //        DataStruct dsItem1 = new DataStruct();
            //        dsItem1.PushIntoStack(AttGeneralID);
            //        dsItem1.PushIntoStack(AttGeneralLiveNum);
            //        dsItem1.PushIntoStack(AttGeneralQishi);
            //        dsItem1.PushIntoStack(AttackTaget);
            //        dsItem1.PushIntoStack(AttackType);
            //        dsItem1.PushIntoStack(AbilityProperty);
            //        dsItem1.PushIntoStack(AttGeneralStatus);
            //        dsItem1.PushIntoStack(BackDamage);
            //        dsItem1.PushIntoStack(AttEffectID);
            //        dsItem1.PushIntoStack(TargetEffectID);
            //        dsItem1.PushIntoStack(IsMove);
            //        dsItem1.PushIntoStack(Position);
            //        dsItem1.PushIntoStack(dsItem12Collection.Length);
            //        foreach (var item in dsItem12Collection)
            //        {
            //            DataStruct dsItem12 = new DataStruct();
            //            dsItem12.PushIntoStack(GeneralEffect);
            //            dsItem12.PushIntoStack(ConDamageNum);

            //            dsItem1.PushIntoStack(dsItem12);
            //        }
            //        dsItem1.PushIntoStack(dsItem12Collection.Length);
            //        foreach (var item in dsItem12Collection)
            //        {
            //            DataStruct dsItem12 = new DataStruct();
            //            dsItem12.PushIntoStack(TargetGeneralID);
            //            dsItem12.PushIntoStack(TargetGeneralLiveNum);
            //            dsItem12.PushIntoStack(TargetGeneralQishi);
            //            dsItem12.PushIntoStack(TargetDamageNum);
            //            dsItem12.PushIntoStack(IsShanBi);
            //            dsItem12.PushIntoStack(IsGeDang);
            //            dsItem12.PushIntoStack(IsFangji);
            //            dsItem12.PushIntoStack(IsMove);
            //            dsItem12.PushIntoStack(FangjiDamageNum);
            //            dsItem12.PushIntoStack(TargetStatus);
            //            dsItem12.PushIntoStack(Position);

            //            dsItem1.PushIntoStack(dsItem12);
            //        }

            //        dsItem.PushIntoStack(dsItem1);
            //    }

            //    this.PushIntoStack(dsItem);
            //}

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref Ops)
                 && httpGet.GetInt("Version", ref Version))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            return true;
        }
    }
}