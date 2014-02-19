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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10003_庄园种植植物品质列表接口
    /// </summary>
    public class Action10003 : BaseAction
    {
        private int plantType = 0;
        private int generalID = 0;
        private int landPostion = 0;
        private int rewardNum = 0;
        private short plantQualityType = 0;
        private UserGeneral userGeneral = null;
        private PlantInfo plantInfo = null;

        public Action10003(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10003, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(userGeneral == null ? string.Empty : userGeneral.GeneralName.ToNotNullString());
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.GeneralLv);
            PushIntoStack(rewardNum);
            PushIntoStack(plantQualityType);

        }

        public override bool GetUrlElement()
        {

            if (httpGet.GetInt("PlantType", ref plantType)
                 && httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("LandPostion", ref landPostion))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            PlantType pType = (PlantType)Enum.Parse(typeof(PlantType), plantType.ToString());
            UserPlantQuality userPlantQuality = new GameDataCacheSet<UserPlantQuality>().FindKey(ContextUser.UserID, generalID, pType);
            if (userPlantQuality != null)
            {
                plantQualityType = (short)userPlantQuality.PlantQuality;
            }
            else
            {
                UserPlantQuality plant = new UserPlantQuality()
                {
                    UserID = ContextUser.UserID,
                    GeneralID = generalID,
                    PlantQuality = PlantQualityType.PuTong,
                    PlantType = pType,
                    RefreshNum = 0,
                    RefreshDate = DateTime.Now,
                };
                new GameDataCacheSet<UserPlantQuality>().Add(plant);
                plantQualityType = 1;
            }
            PlantQualityType qualityType = (PlantQualityType)Enum.Parse(typeof(PlantQualityType), plantQualityType.ToString());
            short generalLv = 0;
            userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userGeneral == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (pType == PlantType.Experience)
            {
                generalLv = ContextUser.UserLv;
            }
            else if (pType == PlantType.GameGoin)
            {
                generalLv = userGeneral.GeneralLv;
            }
            plantInfo = new ConfigCacheSet<PlantInfo>().FindKey(generalLv, plantType, qualityType);
            double addNum = FestivalHelper.SurplusPurchased(ContextUser.UserID, FestivalType.ManorAddition);//活动加成
            rewardNum = plantInfo.GainNum;
            UserLand land = new GameDataCacheSet<UserLand>().FindKey(ContextUser.UserID, landPostion);
            if (land != null && land.IsRedLand == 1)
            {
                rewardNum = MathUtils.Addition(rewardNum, (int)(rewardNum * 0.2), int.MaxValue);
            }
            if (land != null && land.IsBlackLand == 1)
            {
                rewardNum = MathUtils.Addition(rewardNum, (int)(rewardNum * 0.4), int.MaxValue);
            }
            rewardNum = (rewardNum * addNum).ToInt();
            return true;
        }
    }
}