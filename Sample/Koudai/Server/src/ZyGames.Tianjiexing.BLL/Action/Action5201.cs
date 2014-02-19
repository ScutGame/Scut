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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

using ZyGames.Tianjiexing.BLL.Combat;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 领土战列表接口
    /// </summary>
    public class Action5201 : BaseAction
    {
        private int ops = 0;
        private int version = 0;
        private int coldTime;
        private List<CountryCombatProcess> processList = new List<CountryCombatProcess>();
        private CountryUser countryUser;
        private CountryUser firstUser;
        private int _mscoreNum;
        private int _hscoreNum;

        public Action5201(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5201, httpGet)
        {

        }

        public override bool TakeAction()
        {
            if (ContextUser.CountryID == CountryType.None)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St5201_NoJoinCountry;
                return false;
            }
            CountryCombat countryCombat = new CountryCombat(ContextUser);
            if (countryCombat.GameActive == null ||
                countryCombat.GameActive.RefreshStatus() == CombatStatus.NoStart ||
                countryCombat.GameActive.RefreshStatus() == CombatStatus.Over)
            {
                if (ContextUser.UserStatus == UserStatus.CountryCombat)
                {
                    ContextUser.UserStatus = UserStatus.Normal;
                    //ContextUser.Update();
                }
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St5201_CombatNoStart;
                return false;
            }
            //wuzf 检查战斗状态
            if (ContextUser.UserStatus != UserStatus.CountryCombat || UserHelper.IsUserEmbattle(ContextUser.UserID, ContextUser.UseMagicID))
            {
                countryCombat.Exit();
                //ErrorCode = LanguageManager.GetLang().ErrorCode;
                //ErrorInfo = LanguageManager.GetLang().St5201_NoJoinCountry;
                //return false;
            }

            if (ops == 1)
            {
                processList = countryCombat.GetCombatProcess(true);
            }
            else if (ops == 2)
            {
                processList = countryCombat.GetCombatProcess(false);
            }
            coldTime = countryCombat.GameActive.ColdTime;
            countryUser = countryCombat.GetCountryUser();
            firstUser = CountryCombat.FistCountryUser ?? new CountryUser();
            CountryGroup mogemaGroup;
            CountryGroup hashideGroup;
            if (CountryCombat.TryGroup(CountryType.M, out mogemaGroup) && CountryCombat.TryGroup(CountryType.H, out hashideGroup))
            {
                _mscoreNum = mogemaGroup.Score;
                _hscoreNum = hashideGroup.Score;
            }
            if (coldTime == 0)
            {

            }
            return true;
        }

        public override void BuildPacket()
        {
            this.PushIntoStack(coldTime);
            this.PushIntoStack(firstUser.UserName.ToNotNullString());
            this.PushIntoStack(firstUser.CurrWinNum);
            this.PushIntoStack(_mscoreNum);
            this.PushIntoStack(_hscoreNum);
            this.PushIntoStack(countryUser.MaxWinNum);
            this.PushIntoStack(countryUser.CurrWinNum);
            this.PushIntoStack(countryUser.WinCount);
            this.PushIntoStack(countryUser.FailCount);
            this.PushIntoStack(countryUser.ObtainNum);
            this.PushIntoStack(countryUser.GameCoin);
            this.PushIntoStack(processList.Count > 0 ? processList[processList.Count - 1].Version : 0);

            this.PushIntoStack(processList.Count);
            foreach (CountryCombatProcess countryProcess in processList)
            {
                #region 战报
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(countryProcess.WinUserName.ToNotNullString());
                ds.PushIntoStack(countryProcess.FailUserName.ToNotNullString());
                ds.PushIntoStack(countryProcess.KillNum);
                ds.PushIntoStack(countryProcess.WinGameCoin);
                ds.PushIntoStack(countryProcess.WinObtainNum);
                ds.PushIntoStack(countryProcess.FailGameCoin);
                ds.PushIntoStack(countryProcess.FailObtainNum);

                //攻方阵形
                ds.PushIntoStack(countryProcess.ProcessContainer.AttackList.Count);
                foreach (CombatEmbattle combatEmbattle in countryProcess.ProcessContainer.AttackList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(combatEmbattle.GeneralID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                    dsItem.PushIntoStack(combatEmbattle.LiveNum);
                    dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                    dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.AbilityID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                    dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                    ds.PushIntoStack(dsItem);
                }
                //防方阵形
                ds.PushIntoStack(countryProcess.ProcessContainer.DefenseList.Count);
                foreach (CombatEmbattle combatEmbattle in countryProcess.ProcessContainer.DefenseList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(combatEmbattle.GeneralID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralName.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.HeadID.ToNotNullString());
                    dsItem.PushIntoStack(combatEmbattle.Position.ToShort());
                    dsItem.PushIntoStack(combatEmbattle.LiveNum);
                    dsItem.PushIntoStack(combatEmbattle.LiveMaxNum);
                    dsItem.PushIntoStack(combatEmbattle.MomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.MaxMomentumNum);
                    dsItem.PushIntoStack(combatEmbattle.AbilityID);
                    dsItem.PushIntoStack(combatEmbattle.GeneralLv);
                    dsItem.PushIntoStack(combatEmbattle.IsWait ? (short)1 : (short)0);
                    ds.PushIntoStack(dsItem);
                }
                //战斗过程
                ds.PushIntoStack(countryProcess.ProcessContainer.ProcessList.Count);
                foreach (CombatProcess combatProcess in countryProcess.ProcessContainer.ProcessList)
                {
                    DataStruct dsItem = new DataStruct();
                    dsItem.PushIntoStack(combatProcess.GeneralID);
                    dsItem.PushIntoStack(combatProcess.LiveNum);
                    dsItem.PushIntoStack(combatProcess.Momentum);
                    dsItem.PushIntoStack(combatProcess.AttackTaget.ToShort());
                    dsItem.PushIntoStack(combatProcess.AttackUnit.ToShort());
                    dsItem.PushIntoStack(combatProcess.AbilityProperty.ToShort());
                    dsItem.PushIntoStack(combatProcess.AttStatus.ToShort());
                    dsItem.PushIntoStack(combatProcess.DamageNum);
                    dsItem.PushIntoStack(combatProcess.AttEffectID.ToNotNullString());
                    dsItem.PushIntoStack(combatProcess.TargetEffectID.ToNotNullString());
                    dsItem.PushIntoStack(combatProcess.IsMove.ToShort());
                    dsItem.PushIntoStack(combatProcess.Position.ToShort());
                    dsItem.PushIntoStack(combatProcess.Role.ToShort());

                    dsItem.PushIntoStack(combatProcess.DamageStatusList.Count);
                    foreach (AbilityEffectStatus effectStatus in combatProcess.DamageStatusList)
                    {
                        DataStruct dsItem1 = new DataStruct();
                        dsItem1.PushIntoStack(effectStatus.AbilityType.ToShort());
                        dsItem1.PushIntoStack(effectStatus.DamageNum);
                        dsItem1.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                        dsItem.PushIntoStack(dsItem1);
                    }

                    dsItem.PushIntoStack(combatProcess.TargetList.Count);
                    foreach (TargetProcess targetProcess in combatProcess.TargetList)
                    {
                        DataStruct dsItem1 = new DataStruct();
                        dsItem1.PushIntoStack(targetProcess.GeneralID);
                        dsItem1.PushIntoStack(targetProcess.LiveNum);
                        dsItem1.PushIntoStack(targetProcess.Momentum);
                        dsItem1.PushIntoStack(targetProcess.DamageNum);
                        dsItem1.PushIntoStack(targetProcess.IsShanBi.ToShort());
                        dsItem1.PushIntoStack(targetProcess.IsGeDang.ToShort());
                        dsItem1.PushIntoStack(targetProcess.IsBack.ToShort());
                        dsItem1.PushIntoStack(targetProcess.IsMove.ToShort());
                        dsItem1.PushIntoStack(targetProcess.BackDamageNum);
                        dsItem1.PushIntoStack(targetProcess.TargetStatus.ToShort());
                        dsItem1.PushIntoStack(targetProcess.Position.ToShort());
                        dsItem1.PushIntoStack(targetProcess.Role.ToShort());
                        //目标中招效果
                        dsItem1.PushIntoStack(targetProcess.DamageStatusList.Count);
                        foreach (AbilityEffectStatus effectStatus in targetProcess.DamageStatusList)
                        {
                            DataStruct dsItem12 = new DataStruct();
                            dsItem12.PushIntoStack(effectStatus.AbilityType.ToShort());
                            dsItem12.PushIntoStack(effectStatus.IsIncrease ? 1 : 0);

                            dsItem1.PushIntoStack(dsItem12);
                        }
                        dsItem1.PushIntoStack(targetProcess.IsBaoji.ToShort());
                        dsItem1.PushIntoStack(targetProcess.TrumpStatusList.Count);
                        foreach (var item in targetProcess.TrumpStatusList)
                        {
                            DataStruct dsItem13 = new DataStruct();
                            dsItem13.PushIntoStack((short)item.AbilityID);
                            dsItem13.PushIntoStack(item.Num);
                            dsItem1.PushIntoStack(dsItem13);
                        }
                        dsItem.PushIntoStack(dsItem1);
                    }
                    dsItem.PushIntoStack(combatProcess.TrumpStatusList.Count);
                    foreach (var item in combatProcess.TrumpStatusList)
                    {
                        DataStruct dsItem14 = new DataStruct();
                        dsItem14.PushIntoStack((short)item.AbilityID);
                        dsItem14.PushIntoStack(item.Num);
                        dsItem.PushIntoStack(dsItem14);
                    }
                    ds.PushIntoStack(dsItem);
                }

                ds.PushIntoStack(countryProcess.FaildKillNum);
                this.PushIntoStack(ds);
                //战报结束
                #endregion
            }

            this.PushIntoStack((countryUser.InspirePercent * 100).ToInt());
            this.PushIntoStack(countryUser.UserExpNum);
            this.PushIntoStack(countryUser.Status);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops, 1, 2) &&
                httpGet.GetInt("Version", ref version))
            {
                return true;
            }
            return false;
        }
    }
}