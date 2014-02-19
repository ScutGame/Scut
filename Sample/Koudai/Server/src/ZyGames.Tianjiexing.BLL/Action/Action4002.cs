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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 4002_副本地图加载接口
    /// </summary>
    public class Action4002 : BaseAction
    {
        private int plotID;
        private PlotInfo plotInfo;
        private int _ops;
        private bool _isOverCombat;
        private List<PlotNPCInfo> npcList = new List<PlotNPCInfo>();
        private int _plotNum = 0;
        public Action4002(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action4002, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(plotInfo.PlotName.ToNotNullString());
            PushIntoStack(plotInfo.BgScene.ToNotNullString());
            PushIntoStack(plotInfo.FgScene.ToNotNullString());
            PushIntoStack(npcList.Count);
            foreach (PlotNPCInfo item in npcList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(item.PlotNpcID);
                ds.PushIntoStack(item.NpcSeqNo);
                ds.PushIntoStack(item.NpcName.ToNotNullString());
                ds.PushIntoStack(item.HeadID.ToNotNullString());
                ds.PushIntoStack(item.NpcTip.ToNotNullString());
                ds.PushIntoStack(item.PointX);
                ds.PushIntoStack(item.PointY);
                bool isPlayStory = false;
                var userPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, plotID);
                if (userPlot == null || userPlot.PlotStatus != PlotStatus.Completed)
                {
                    isPlayStory = true;
                }
                ds.PushIntoStack(isPlayStory ? item.PreStoryCode.ToNotNullString() : string.Empty);
                ds.PushIntoStack(isPlayStory ? item.AftStoryCode.ToNotNullString() : string.Empty);
                PushIntoStack(ds);
            }
            PushIntoStack(_isOverCombat ? (short)1 : (short)0);
            PushIntoStack(_plotNum);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PlotID", ref plotID))
            {
                httpGet.GetInt("Ops", ref _ops);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (ContextUser.UserStatus == UserStatus.CountryCombat)
            {
                CountryCombat countryCombat = new CountryCombat(ContextUser);
                if (countryCombat.GameActive != null && countryCombat.GameActive.RefreshStatus() == CombatStatus.Combat)
                {
                    countryCombat.Exit();
                }
            }

            if (ContextUser.EnergyNum <= 0 && ContextUser.SurplusEnergy <= 0)
            {
                ErrorCode = 3;
                ErrorInfo = LanguageManager.GetLang().St_EnergyNotEnough;
                return false;
            }
            //if (_ops != 1 && UserHelper.IsPromptBlood(ContextUser.UserID))
            //{
            //    ErrorCode = 1;
            //    ErrorInfo = LanguageManager.GetLang().St4002_PromptBlood;
            //    return false;
            //}
            //if (_ops != 1 && UserItemHelper.CheckItemOut(ContextUser, ItemStatus.BeiBao))
            //{
            //    ErrorCode = 1;
            //    ErrorInfo = LanguageManager.GetLang().St_User_BeiBaoMsg;
            //    return false;
            //}

            UserHelper.UserGeneralPromptBlood(ContextUser);//佣兵自动使用绷带补血

            plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotID);
           
            if (plotInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().LoadDataError;
                return false;
            }
            int challengeNum = PlotHelper.GetPlotChallengeNum(UserId.ToString(), plotInfo.PlotID);
            if (plotInfo.PlotType == PlotType.Elite && challengeNum >= plotInfo.ChallengeNum)
            {
                ErrorCode = 4;
                ErrorInfo = LanguageManager.GetLang().St4002_IsPlotEliteNotChallengeNum;
                return false;
            }
            
            if ((challengeNum >= plotInfo.ChallengeNum && ContextUser.VipLv < 3))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4002_NotChallengeNum;
                return false;
            }

            int num = challengeNum;
            int challengeDefSoldNum = ConfigEnvSet.GetInt("UserPlot.ChallengeDefSoldNum");
            int challengeDoubleSoldNum = ConfigEnvSet.GetInt("UserPlot.ChallengeDoubleSoldNum");
            if (_ops != 1 && num >= plotInfo.ChallengeNum && PlotHelper.GetPlotIsOne(UserId.ToString(), plotInfo.PlotID))
            {
                ErrorCode = 1;
                _plotNum = challengeDefSoldNum;
                return false;
            }
            else
            {
                if (_ops != 1 && num >= plotInfo.ChallengeNum && PlotHelper.GetPlotIsOne(UserId.ToString(), plotInfo.PlotID) == false)
                {
                    int openNum = num - plotInfo.ChallengeNum + 1;
                    _plotNum = challengeDoubleSoldNum * openNum + challengeDefSoldNum;
                }
            }

            if (_ops != 1 && num >= plotInfo.ChallengeNum)
            {
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St4002_IsPlotNum, _plotNum);
                return false;
            }
            if (_ops == 1 && num >= plotInfo.ChallengeNum)
            {

                if (ContextUser.GoldNum < _plotNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                ContextUser.UseGold = MathUtils.Subtraction(ContextUser.UseGold, _plotNum);
            }
            if (_ops == 1 && num >= plotInfo.ChallengeNum && PlotHelper.GetPlotIsOne(UserId.ToString(), plotInfo.PlotID) == false)
            {
                int openNum = num - plotInfo.ChallengeNum + 1;
                _plotNum = challengeDoubleSoldNum * openNum + challengeDefSoldNum; ;
                if (ContextUser.GoldNum < _plotNum)
                {
                    ErrorCode = 2;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, _plotNum);
            }
            if (_ops != 1 && !string.IsNullOrEmpty(plotInfo.EnchantID) && plotInfo.EnchantProbability > 0 && EnchantHelper.IsEnchantPackage(ContextUser.UserID))
            {
                ErrorCode = 1;
                ErrorInfo = LanguageManager.GetLang().St4002_EnchantPackageFull;
                return false;
            }
            //var cacheSetUserPlot = new GameDataCacheSet<UserPlotPackage>();
            //var userPlotPack = cacheSetUserPlot.FindKey(ContextUser.UserID);
            //var userPlot = userPlotPack != null ? userPlotPack.PlotPackage.Find(s => s.PlotID == npcInfo.PlotID) : null;
            var userPlot = UserPlotHelper.GetUserPlotInfo(ContextUser.UserID, plotID);
            if (userPlot != null && userPlot.PlotNum >= plotInfo.ChallengeNum)
            {
                if (plotInfo.PlotType == PlotType.Elite)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St4002_EliteUsed;
                    return false;
                }
                else if (plotInfo.PlotType == PlotType.HeroPlot)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St4002_HeroPlotNum;
                    return false;
                }
            }
            npcList = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == plotID);
            //if (ContextUser.EnergyNum < (npcList.Count * PlotInfo.BattleEnergyNum))
            //{
            //    ErrorCode = 3;
            //    ErrorInfo = LanguageManager.GetLang().St_EnergyNotEnough;
            //    return false;
            //}
            ContextUser.InPlotDate = DateTime.Now;
            //if (ContextUser.UserExtend == null || ContextUser.UserExtend.PlotStatusID == 0 || ContextUser.UserExtend.MercenarySeq == 0)
            //{
            //    ContextUser.TempEnergyNum = PlotInfo.BattleEnergyNum;
            //    //ContextUser.RemoveEnergyNum(PlotInfo.BattleEnergyNum);
            //}
            //ContextUser.Update();
            if (ContextUser.UserExtend != null && ContextUser.UserExtend.PlotStatusID <= 0)
            {
                ContextUser.UserExtend.UpdateNotify(obj =>
                {
                    ContextUser.UserExtend.PlotStatusID = plotID;
                    ContextUser.UserExtend.PlotNpcID = -1;
                    ContextUser.UserExtend.MercenarySeq = 1;
                    ContextUser.UserExtend.IsBoss = false;
                    return true;
                });
            }
           
            _isOverCombat = userPlot != null && userPlot.PlotStatus == PlotStatus.Completed;
            ContextUser.TempEnergyNum = PlotInfo.BattleEnergyNum;
            return true;
        }
    }
}