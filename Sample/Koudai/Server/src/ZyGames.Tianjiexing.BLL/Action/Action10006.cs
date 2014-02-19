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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10006_庄园种植收获接口
    /// </summary>
    public class Action10006 : BaseAction
    {
        private int plantType = 0;
        private int generalID = 0;
        private int landPositon = 0;
        private int gainNum = 0;

        public Action10006(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10006, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(gainNum);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlantType", ref plantType)
                 && httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("LandPositon", ref landPositon))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            short optype = 0;
            short plantQuality = 0;
            PlantType pType = (PlantType)Enum.Parse(typeof(PlantType), plantType.ToString());
            if (pType == PlantType.Experience)
            {
                optype = 6;
            }
            else
            {
                optype = 7;
            }
            short generalLv = 0;
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userGeneral == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().LoadDataError;
                return false;
            }
            generalLv = userGeneral.GeneralLv;

            if (pType == PlantType.Experience)
            {
                generalLv = ContextUser.UserLv;
            }
            else if (pType == PlantType.GameGoin)
            {
                generalLv = userGeneral.GeneralLv;
            }
            int upexpNum = 0;
            int expeNum = 0;
            double addNum = FestivalHelper.SurplusPurchased(ContextUser.UserID, FestivalType.ManorAddition);//活动加成
            UserLand userLand = new GameDataCacheSet<UserLand>().FindKey(ContextUser.UserID, landPositon);
            if (userLand != null)
            {
                if (userLand.IsGain == 2 || userLand.GeneralID == 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().LoadDataError;
                    return false;
                }
                plantQuality = (short)userLand.PlantQuality;
                PlantInfo plantInfo = new ConfigCacheSet<PlantInfo>().FindKey(generalLv, plantType, userLand.PlantQuality);
                if (plantInfo != null)
                {
                    if (userLand.IsGain == 1)
                    {
                        userLand.GeneralID = 0;
                        userLand.PlantType = PlantType.Experience;
                        userLand.IsGain = 2;
                        userLand.GainDate = DateTime.Now;
                        userLand.PlantQuality = PlantQualityType.PuTong;
                    }
                    else
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        return false;
                    }
                    gainNum = plantInfo.GainNum;
                    UserLand land = new GameDataCacheSet<UserLand>().FindKey(ContextUser.UserID, landPositon);
                    if (land != null && land.IsRedLand == 1)
                    {
                        gainNum = MathUtils.Addition(gainNum, (int)(gainNum * 0.2), int.MaxValue);
                    }
                    if (land != null && land.IsBlackLand == 1)
                    {
                        gainNum = MathUtils.Addition(gainNum, (int)(gainNum * 0.4), int.MaxValue);
                    }
                    gainNum = (gainNum * addNum).ToInt();
                    if (pType == PlantType.Experience)
                    {
                        expeNum = MathUtils.Addition(userGeneral.CurrExperience, gainNum, int.MaxValue);

                        GeneralEscalateInfo generalEscalate = new ConfigCacheSet<GeneralEscalateInfo>().FindKey(userGeneral.GeneralLv);
                        if (generalEscalate != null)
                        {
                            upexpNum = generalEscalate.UpExperience;
                        }

                        if (gainNum > 0)
                        {
                            userGeneral.Experience2 = MathUtils.Addition(userGeneral.Experience2, gainNum, int.MaxValue);
                            //userGeneral.CurrExperience = MathUtils.Addition(userGeneral.CurrExperience, gainNum, int.MaxValue);
                            UserHelper.TriggerGeneral(userGeneral, gainNum);
                        }
                    }
                    else if (pType == PlantType.GameGoin)
                    {
                        if (gainNum > 0)
                        {
                            ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, gainNum, int.MaxValue);
                        }

                    }
                }
                UserLogHelper.AppenLandLog(ContextUser.UserID, optype, generalID, landPositon, 0, plantQuality, gainNum, 0);
            }
            
            if (pType == PlantType.Experience && expeNum >= upexpNum)
            {
                var chatService = new TjxChatService();
                string content = string.Format(LanguageManager.GetLang().St10006_UserGeneralUpLv,
                                   userGeneral.GeneralName,
                                   userGeneral.GeneralLv);
                chatService.SystemSendWhisper(ContextUser, content);
            }
            
            return true;
        }
    }
}