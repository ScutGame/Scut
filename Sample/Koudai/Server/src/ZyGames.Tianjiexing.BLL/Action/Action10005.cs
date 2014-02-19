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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10005_庄园植物品质刷新接口
    /// </summary>
    public class Action10005 : BaseAction
    {
        private int refershID = 0;
        private int ops = 0;
        private int plantType = 0;
        private int generalID = 0;

        public Action10005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10005, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {

            if (httpGet.GetInt("PlantType", ref plantType)
                 && httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("RefershID", ref refershID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            short optype = 0;
            int qualityID = 0;
            int totalNum = 0;
            var plantQualitieArray = new GameDataCacheSet<UserPlantQuality>().FindAll(ContextUser.UserID, u => u.RefreshDate.Date == DateTime.Now.Date);
            if (plantQualitieArray.Count > 0)
            {
                totalNum = plantQualitieArray[0].RefreshNum;
            }
            foreach (UserPlantQuality userPlantQuality in plantQualitieArray)
            {
                if (totalNum < userPlantQuality.RefreshNum)
                {
                    totalNum = userPlantQuality.RefreshNum;
                }
            }

            PlantType pType = (PlantType)Enum.Parse(typeof(PlantType), plantType.ToString());
            if (pType == PlantType.Experience)
            {
                optype = 4;
            }
            else
            {
                optype = 5;
            }
            int refreshNum = ConfigEnvSet.GetInt("UserPlant.QualityRefreshNum");
            UserPlantQuality plantQuality = new GameDataCacheSet<UserPlantQuality>().FindKey(ContextUser.UserID, generalID, pType);
            if (plantQuality == null)
            {
                return false;
            }
            if (plantQuality.PlantQuality == PlantQualityType.Shenhua)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St10005_MaxQualityType;
                return false;
            }

            if (refershID == 1 && ops == 1)
            {
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St10005_Refresh, GetRefreshNum(ContextUser.UserID, totalNum));
                return false;
            }
            else if (refershID == 1 && ops == 2)
            {
                if (ContextUser.GoldNum < GetRefreshNum(ContextUser.UserID, totalNum))
                {
                    ErrorCode = 2;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, GetRefreshNum(ContextUser.UserID, totalNum), int.MaxValue);

                QualityProbabilityInfo probabilityInfo = new ConfigCacheSet<QualityProbabilityInfo>().FindKey(plantQuality.PlantQuality);
                if (RandomUtils.IsHit(probabilityInfo.Light))
                {
                    if (!string.IsNullOrEmpty(plantQuality.UserID) && DateTime.Now.Date == plantQuality.RefreshDate.Date && plantQuality.PlantQuality != PlantQualityType.Shenhua)
                    {
                        qualityID = MathUtils.Addition(Convert.ToInt32(plantQuality.PlantQuality), 1, int.MaxValue);
                    }
                    else
                    {
                        qualityID = 2;
                    }
                    PlantQualityType pQualityType = (PlantQualityType)Enum.Parse(typeof(PlantQualityType), qualityID.ToString());
                    ErrorCode = ErrorCode;
                    ErrorInfo = probabilityInfo.QualityName;
                    if (!string.IsNullOrEmpty(plantQuality.UserID))
                    {
                        UpdatePlantQuailty(plantQuality, pQualityType, totalNum);
                    }
                    else
                    {
                        AppendPlantQuality(pType, pQualityType);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(plantQuality.UserID) && DateTime.Now.Date == plantQuality.RefreshDate.Date && plantQuality.PlantQuality != PlantQualityType.PuTong)
                    {
                        qualityID = MathUtils.Subtraction(Convert.ToInt32(plantQuality.PlantQuality), 1, 0);
                    }
                    else
                    {
                        qualityID = 1;
                    }
                    PlantQualityType pQualityType = (PlantQualityType)Enum.Parse(typeof(PlantQualityType), qualityID.ToString());
                    UpdatePlantQuailty(plantQuality, pQualityType, totalNum);
                }
                UserLogHelper.AppenLandLog(ContextUser.UserID, optype, generalID, 0, GetRefreshNum(ContextUser.UserID, totalNum),
                                          (short)plantQuality.PlantQuality, 0, 0);
            }
            else if (refershID == 2 && ops == 1)
            {
                if (ContextUser.VipLv < 5)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                    return false;
                }
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St10005_Refresh, refreshNum);
                return false;
            }
            else if (refershID == 2 && ops == 2)
            {
                if (ContextUser.GoldNum < refreshNum)
                {
                    ErrorCode = 2;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                PlantQualityType pQualityType = PlantQualityType.Shenhua;
                if (!string.IsNullOrEmpty(plantQuality.UserID))
                {
                    UpdatePlantQuailty(plantQuality, pQualityType, totalNum);
                }
                else
                {
                    AppendPlantQuality(pType, pQualityType);
                }

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, refreshNum, int.MaxValue);

                UserLogHelper.AppenLandLog(ContextUser.UserID, optype, generalID, 0, refreshNum, (short)plantQuality.PlantQuality, 0, 0);
            }

            return true;
        }

        private void UpdatePlantQuailty(UserPlantQuality plantQuality, PlantQualityType pQualityType, int totalNum)
        {
            plantQuality.GeneralID = generalID;
            plantQuality.PlantQuality = pQualityType;
            plantQuality.RefreshDate = DateTime.Now;
            plantQuality.RefreshNum = MathUtils.Addition(totalNum, 1, int.MaxValue);

        }

        private void AppendPlantQuality(PlantType pType, PlantQualityType plantQualityType)
        {
            UserPlantQuality plant = new UserPlantQuality()
                                         {
                                             UserID = ContextUser.UserID,
                                             GeneralID = generalID,
                                             PlantType = pType,
                                             PlantQuality = plantQualityType,
                                             RefreshNum = 1,
                                             RefreshDate = DateTime.Now
                                         };
            new GameDataCacheSet<UserPlantQuality>().Add(plant);
        }

        //刷新所需晶石
        public int GetRefreshNum(string userID, int totalNum)
        {
            //int useGoldMaxNum = ConfigEnvSet.GetInt("RefreshPlantUseGoldMaxNum");
            int refreshGold = 0;
            if (totalNum > 0)
            {
                refreshGold = (MathUtils.Addition(totalNum, 1, int.MaxValue) * 2);
                //if (refreshGold > useGoldMaxNum)
                //{
                //refreshGold = useGoldMaxNum;
                //}
            }
            else
            {
                refreshGold = 2;
            }
            return refreshGold;
        }
    }
}