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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1431_将星佣兵列表接口
    /// </summary>
    public class Action1431 : BaseAction
    {
        private string _firstName;
        private string _headID;
        private int _lifeNum;
        private short _careerID;
        private string _careerName;
        private short _powerNum;
        private short _soulNum;
        private short _intellectNum;
        private string _abilityName;
        private string _abilityDesc;
        private short _isRecruit;
        private short iscomplete;
        private GeneralInfo[] _generalArray = new GeneralInfo[0];
        private StoryTaskInfo[] _storyTaskArray = new StoryTaskInfo[0];


        public Action1431(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1431, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_generalArray.Length);
            foreach (var general in _generalArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(general.GeneralID);
                dsItem.PushIntoStack(general.GeneralName.ToNotNullString());
                this.PushIntoStack(dsItem);
            }
            this.PushIntoStack(_firstName.ToNotNullString());
            this.PushIntoStack(_headID.ToNotNullString());
            this.PushIntoStack(_lifeNum);
            this.PushIntoStack((short)_careerID);
            this.PushIntoStack(_careerName.ToNotNullString());
            this.PushIntoStack((short)_powerNum);
            this.PushIntoStack((short)_soulNum);
            this.PushIntoStack((short)_intellectNum);
            this.PushIntoStack(_abilityName.ToNotNullString());
            this.PushIntoStack(_abilityDesc.ToNotNullString());
            this.PushIntoStack((short)_isRecruit);
            this.PushIntoStack(_storyTaskArray.Length);
            foreach (var task in _storyTaskArray)
            {
                int collectNum = TrumpHelper.GetUserItemNum(ContextUser.UserID, task.TargetItemID);
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(task.PlotID);
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(task.TargetItemID);
                iscomplete = GeneralHelper.IsComplete(ContextUser, task);
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
                dsItem.PushIntoStack(task.Reward.Count);
                foreach (var prize in task.Reward)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack((short)prize.Type);
                    dsItem1.PushIntoStack(GeneralHelper.PrizeItemName(prize).ToNotNullString());
                    dsItem1.PushIntoStack(prize.Num);
                    dsItem.PushIntoStack(dsItem1);
                }

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
            _generalArray = new ConfigCacheSet<GeneralInfo>().FindAll().ToArray();
            if (_generalArray.Length > 0)
            {
                UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, _generalArray[0].GeneralID);
                GeneralInfo generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(_generalArray[0].GeneralID);
                if (generalInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                _firstName = generalInfo.GeneralName;
                _headID = generalInfo.HeadID;
                _lifeNum = generalInfo.LifeNum;
                _careerID = generalInfo.CareerID;
                CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(generalInfo.CareerID);
                if (careerInfo != null)
                {
                    _careerName = careerInfo.CareerName;
                }
                _powerNum = generalInfo.PowerNum;
                _soulNum = generalInfo.SoulNum;
                _intellectNum = generalInfo.IntellectNum;
                int abilityId = generalInfo.AbilityID;
                _isRecruit = GeneralHelper.IsGeneralRecruit(ContextUser.UserID, _generalArray[0].GeneralID);
                if (general != null)
                {
                    _lifeNum = general.LifeNum;
                    abilityId = general.AbilityID;
                    _powerNum = general.PowerNum;
                    _soulNum = general.SoulNum;
                    _intellectNum = general.IntellectNum;
                    _isRecruit = 2;
                }
                AbilityInfo abilityInfo = new ConfigCacheSet<AbilityInfo>().FindKey(abilityId);
                if (abilityInfo != null)
                {
                    _abilityName = abilityInfo.AbilityName;
                    _abilityDesc = abilityInfo.AbilityDesc;
                }
                _storyTaskArray = new ConfigCacheSet<StoryTaskInfo>().FindAll(m => m.TaskType == TaskType.General && m.GeneralID == generalInfo.GeneralID).ToArray();
            }

            return true;
        }


    }
}