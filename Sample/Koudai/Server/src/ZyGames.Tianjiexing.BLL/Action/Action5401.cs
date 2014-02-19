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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5401_Boss战参加接口
    /// </summary>
    public class Action5401 : BaseAction
    {
        private int _pageIndex;
        private int _pageSize;
        private GameActive _gameActive;
        private List<BossUser> _bossUserList = new List<BossUser>();
        private int _regNum;
        private double _inspirePercent;
        private int _reLiveNum = 0;
        private double _reliveInspirePercent;
        private int _activeId;
        private const int GlodNum = 20;
        private const int InspirePercent = 20;
        private const int BackGoldNum = 10;
        private int _combatNum;
        private int _damageNum;
        private int _codeTime;
        private int _bossLiftNum;
        private int _bossMaxLift;


        public Action5401(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5401, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_gameActive == null ? 0 : _gameActive.BossPlotID);
            PushIntoStack(_gameActive == null ? (short)0 : _gameActive.BossLv);
            PushIntoStack(_gameActive == null ? 0 : _gameActive.ColdTime);
            PushIntoStack(_regNum);
            PushIntoStack(_gameActive == null ? (short)0 : _gameActive.CombatStatus.ToShort());
            PushIntoStack((_inspirePercent * 100).ToInt());
            PushIntoStack(_reLiveNum);


            PushIntoStack(_bossUserList.Count);
            foreach (BossUser bossUser in _bossUserList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(bossUser.NickName.ToNotNullString());
                UserGeneral userGeneral = UserGeneral.GetMainGeneral(bossUser.UserId);
                dsItem.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.HeadID.ToNotNullString());
                dsItem.PushIntoStack(bossUser.IsRelive ? 0 : 1);
                dsItem.PushIntoStack(bossUser.CodeTime);
                dsItem.PushIntoStack(NoviceHelper.IsWingFestivalInfo(bossUser.UserId) ? (short)1 : (short)0);
                dsItem.PushIntoStack(0);
                PushIntoStack(dsItem);
            }

            PushIntoStack((_reliveInspirePercent * 100).ToInt());
            PushIntoStack(GlodNum);
            PushIntoStack(InspirePercent);
            PushIntoStack(BackGoldNum);
            PushIntoStack(_combatNum);
            PushIntoStack(_damageNum);
            PushIntoStack(_codeTime);
            PushIntoStack(_bossLiftNum);
            PushIntoStack(_bossMaxLift);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref _pageIndex)
                && httpGet.GetInt("PageSize", ref _pageSize)
                && httpGet.GetInt("ActiveId", ref _activeId))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            if (CombatHelper.IsBossKill(_activeId))
            {
                this.ErrorCode = 1;
                this.ErrorInfo = LanguageManager.GetLang().St5405_BossKilled;
                return false;
            }
            BossCombat bossCombat = new BossCombat(_activeId);
            _gameActive = bossCombat.GameActive;
            CombatStatus combatStatus = _gameActive.RefreshStatus();
            if (combatStatus == CombatStatus.Wait || combatStatus == CombatStatus.Combat)
            {
                bossCombat.Append(ContextUser);
                BossUser bossUser = bossCombat.GetCombatUser(Uid);
                if (bossUser != null)
                {
                    _inspirePercent = bossUser.InspirePercent;
                    _reliveInspirePercent = bossUser.ReliveInspirePercent;
                    _reLiveNum = bossUser.ReliveNum;
                    _combatNum = bossUser.CombatNum;
                    _damageNum = bossUser.DamageNum;
                    _codeTime = bossUser.CodeTime;
                }
            }
            List<BossUser> userList = bossCombat.GetCombatUser();
            _regNum = userList.Count;
            int recordCount = 0;
            _bossUserList = userList.GetPaging(_pageIndex, _pageSize, out recordCount);

            CombatGeneral boss = bossCombat.Boss;
            if (boss != null)
            {
                _bossLiftNum = boss.LifeNum;
                _bossMaxLift = boss.LifeMaxNum;
            }
            return true;
        }
    }
}