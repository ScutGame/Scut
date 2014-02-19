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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1434_将星佣兵招募接口
    /// </summary>
    public class Action1434 : BaseAction
    {
        private int generalID;


        public Action1434(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1434, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int maxGeneralNum = ContextUser.GeneralMaxNum;
            var userGeneralsList = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong && u.GeneralType != GeneralType.Soul);
            if (userGeneralsList.Count >= MathUtils.Addition(maxGeneralNum, 1, int.MaxValue))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1404_MaxGeneralNumFull;
                return false;
            }
            var generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            if (generalInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (userGeneral == null && GeneralHelper.IsGeneralRecruit(ContextUser.UserID, generalID) == 1)
            {
                userGeneral = new UserGeneral();
                userGeneral.UserID = ContextUser.UserID;
                userGeneral.GeneralID = generalID;
                userGeneral.GeneralName = generalInfo.GeneralName;
                userGeneral.HeadID = generalInfo.HeadID;
                userGeneral.PicturesID = generalInfo.PicturesID;
                userGeneral.GeneralLv = (short)generalInfo.GeneralLv;
                userGeneral.LifeNum = generalInfo.LifeNum;
                userGeneral.GeneralType = GeneralType.YongBing;
                userGeneral.CareerID = generalInfo.CareerID;
                userGeneral.PowerNum = generalInfo.PowerNum;
                userGeneral.SoulNum = generalInfo.SoulNum;
                userGeneral.IntellectNum = generalInfo.IntellectNum;
                userGeneral.TrainingPower = 0;
                userGeneral.TrainingSoul = 0;
                userGeneral.TrainingIntellect = 0;
                userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
                userGeneral.AbilityID = generalInfo.AbilityID;
                userGeneral.Momentum = 0;
                userGeneral.Description = generalInfo.Description;
                userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
                userGeneral.CurrExperience = 0;
                userGeneral.Experience1 = 0;
                userGeneral.Experience2 = 0;
                var cacheSet = new GameDataCacheSet<UserGeneral>();
                cacheSet.Add(userGeneral);
            }
            ErrorCode = 0;
            ErrorInfo = LanguageManager.GetLang().St1434_RecruitmentErfolg;
            return true;
        }
    }
}