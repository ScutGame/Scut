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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10004_庄园植物种植接口
    /// </summary>
    public class Action10004 : BaseAction
    {
        private int plantType = 0;
        private int generalID = 0;
        private int plantQualityType = 0;
        private int landPsition = 0;


        public Action10004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10004, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlantType", ref plantType)
                 && httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("PlantQualityType", ref plantQualityType)
                 && httpGet.GetInt("LandPsition", ref landPsition))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            PlantType pType = plantType.ToEnum<PlantType>();
            UserPlantQuality userPlantQuality = new GameDataCacheSet<UserPlantQuality>().FindKey(ContextUser.UserID, generalID, pType);
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userPlantQuality == null)
            {
                return false;
            }
            UserPlant userPlant = new GameDataCacheSet<UserPlant>().FindKey(ContextUser.UserID);
            if (userPlant == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().LoadDataError;
                return false;
            }

            if (pType == PlantType.GameGoin && userPlant.DewNum == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St10004_DewNotEnough;
                return false;
            }

            if (userGeneral != null && pType == PlantType.Experience)
            {
                if (UserHelper.GeneralIsUpLv(ContextUser.UserID, generalID, ContextUser.UserLv, landPsition))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St10004_GeneralNotUserLv;
                    return false;
                }
            }

            var cacheSet = new GameDataCacheSet<UserLand>();
            UserLand land = cacheSet.FindKey(ContextUser.UserID, landPsition);
            if (land == null)
            {
                land = new UserLand();
                land.UserID = ContextUser.UserID;
                land.LandPositon = landPsition;
                land.GeneralID = generalID;
                land.IsBlackLand = 2;
                land.IsRedLand = 2;
                land.IsGain = 1;
                land.PlantQuality = userPlantQuality.PlantQuality;
                land.GainDate = MathUtils.SqlMinDate;
                cacheSet.Add(land);
            }

            if (land.IsGain > 1 && ((DateTime.Now - land.GainDate).TotalSeconds > 28800))
            {
                land.GeneralID = generalID;
                land.PlantType = pType;
                land.IsGain = 1;
                land.PlantQuality = userPlantQuality.PlantQuality;
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            if (pType == PlantType.GameGoin)
            {
                userPlant.DewNum = MathUtils.Subtraction(userPlant.DewNum, 1, 0);
            }
            userPlantQuality.PlantQuality = PlantQualityType.PuTong;

            //日常任务-庄园
            TaskHelper.TriggerDailyTask(Uid, 4008);
            return true;
        }
    }
}