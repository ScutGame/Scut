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


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1409_佣兵属性培养接口
    /// </summary>
    public class Action1409 : BaseAction
    {
        private int generalID;
        private CultureType cultureType;
        private short newPower = 0;
        private short newSoul = 0;
        private short newIntellect = 0;
        private Random random = new Random();

        public Action1409(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1409, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(newPower);
            PushIntoStack(newSoul);
            PushIntoStack(newIntellect);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetEnum<CultureType>("CultureType", ref cultureType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserGeneral userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userGeneral == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            int useGoldNum = 0;

            int jiaQiang = ConfigEnvSet.GetInt("User.JiaQiangTraining");
            int ZuanShi = ConfigEnvSet.GetInt("User.ZuanShiTraining");
            int BaiJin = ConfigEnvSet.GetInt("User.BaiJinTraining");
            int ZhiZun = ConfigEnvSet.GetInt("User.ZhiZunTraining");

            int maxTrainingNum = MathUtils.Addition((userGeneral.GeneralLv * 2), 20, int.MaxValue); //培养上限

            int trainingPower = (int)userGeneral.TrainingPower;
            int trainingSoul = (int)userGeneral.TrainingSoul;
            int trainingIntellect = (int)userGeneral.TrainingIntellect;

            if (trainingPower >= maxTrainingNum && trainingSoul >= maxTrainingNum && trainingIntellect >= maxTrainingNum)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St1409_maxTrainingNum;
                return false;
            }

            if (CultureType.PuTong == cultureType)
            {
                newPower = (short)random.Next(1, MathUtils.Addition(trainingPower, 5, maxTrainingNum));
                newSoul = (short)random.Next(1, MathUtils.Addition(trainingSoul, 5, maxTrainingNum));
                newIntellect = (short)random.Next(1, MathUtils.Addition(trainingIntellect, 5, maxTrainingNum));
                if (ContextUser.GameCoin < UserHelper.GetCultureMoney(ContextUser.UserID, generalID))
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St_GameCoinNotEnough;
                    return false;
                }
                ContextUser.GameCoin = MathUtils.Subtraction(ContextUser.GameCoin, UserHelper.GetCultureMoney(ContextUser.UserID, generalID), 0);
            }
            else if (CultureType.JiaQiang == cultureType)
            {
                newPower = (short)random.Next(3, MathUtils.Addition(trainingPower, 6, maxTrainingNum));
                newSoul = (short)random.Next(3, MathUtils.Addition(trainingSoul, 6, maxTrainingNum));
                newIntellect = (short)random.Next(3, MathUtils.Addition(trainingIntellect, 6, maxTrainingNum));
                useGoldNum = jiaQiang;
            }
            else if (CultureType.BaiJin == cultureType)
            {
                newPower = (short)random.Next(5, MathUtils.Addition(trainingPower, 7, maxTrainingNum));
                newSoul = (short)random.Next(5, MathUtils.Addition(trainingSoul, 7, maxTrainingNum));
                newIntellect = (short)random.Next(5, MathUtils.Addition(trainingIntellect, 7, maxTrainingNum));
                useGoldNum = BaiJin;
            }
            else if (CultureType.ZuanShi == cultureType)
            {
                newPower = (short)random.Next(7, MathUtils.Addition(trainingPower, 8, maxTrainingNum));
                newSoul = (short)random.Next(7, MathUtils.Addition(trainingSoul, 8, maxTrainingNum));
                newIntellect = (short)random.Next(7, MathUtils.Addition(trainingIntellect, 8, maxTrainingNum));
                useGoldNum = ZuanShi;
            }
            else if (CultureType.ZhiZun == cultureType)
            {

                newPower = (short)MathUtils.Addition(trainingPower, 5, maxTrainingNum);
                newSoul = (short)MathUtils.Addition(trainingSoul, 5, maxTrainingNum);
                newIntellect = (short)MathUtils.Addition(trainingIntellect, 5, maxTrainingNum);
                useGoldNum = ZhiZun;
            }
            if (useGoldNum > ContextUser.GoldNum)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                return false;
            }
            ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGoldNum, int.MaxValue);
            ContextUser.GeneralID = generalID;
            ContextUser.TrainingPower = newPower;
            ContextUser.TrainingSoul = newSoul;
            ContextUser.TrainingIntellect = newIntellect;

            //日常任务-培养
            TaskHelper.TriggerDailyTask(Uid, 4002);

            return true;
        }
    }
}