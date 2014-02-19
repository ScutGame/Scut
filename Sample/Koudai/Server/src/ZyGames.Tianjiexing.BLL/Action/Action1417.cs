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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1417_传承与被传承人列表接口
    /// </summary>
    public class Action1417 : BaseAction
    {
        private HeritageType heritageType;
        private List<UserGeneral> generalArray = new List<UserGeneral>();

        public Action1417(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1417, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(generalArray.Count);
            foreach (var item in generalArray)
            {
                CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(item.CareerID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.GeneralID);
                dsItem.PushIntoStack(item.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(item.HeadID.ToNotNullString());
                dsItem.PushIntoStack(item.GeneralLv);
                dsItem.PushIntoStack(careerInfo == null ? string.Empty : careerInfo.CareerName.ToNotNullString());
                dsItem.PushIntoStack(item.TrainingPower);
                dsItem.PushIntoStack(item.TrainingSoul);
                dsItem.PushIntoStack(item.TrainingIntellect);
                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("HeritageType", ref heritageType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int generalID = 0;
            int heGeneralID = 0;
            short generalLv = 0;
            List<UserGeneral> generalList = new List<UserGeneral>();
            if (ContextUser.HeritageList.Count > 0)
            {
                GeneralHeritage heritageGenral = ContextUser.HeritageList.Find(m => m.Type == HeritageType.IsHeritage);
                if (heritageGenral != null)
                {
                    generalLv = MathUtils.Addition(heritageGenral.GeneralLv, 3.ToShort());
                    heGeneralID = heritageGenral.GeneralID;
                }
            }
            if (heritageType == HeritageType.Heritage)
            {
                generalList = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID,
                                                          u => u.GeneralID != heGeneralID &&
                                                               u.GeneralLv >= generalLv &&
                                                          u.GeneralStatus == GeneralStatus.DuiWuZhong && u.GeneralType != GeneralType.Soul);
            }
            else if (heritageType == HeritageType.IsHeritage)
            {
                int isGeneralID = 0;
                GeneralHeritage IsGenral = ContextUser.HeritageList.Find(m => m.Type == HeritageType.IsHeritage);
                if (IsGenral != null)
                {
                    isGeneralID = IsGenral.GeneralID;
                }
                generalList = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID,
                                                                        u => u.GeneralStatus == GeneralStatus.DuiWuZhong &&
                                                                        u.GeneralID != generalID &&
                                                                        u.GeneralID != isGeneralID &&
                                                                        u.GeneralID != heGeneralID && u.GeneralType != GeneralType.Soul);
            }
            foreach (var userGeneral in generalList)
            {
                if (!EmbattleHelper.IsEmbattleGeneral(userGeneral.UserID, userGeneral.GeneralID))
                {
                    generalArray.Add(userGeneral);
                }
            }
            return true;
        }
    }
}