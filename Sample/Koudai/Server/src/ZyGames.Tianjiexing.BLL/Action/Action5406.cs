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
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5406_Boss战伤害排名接口
    /// </summary>
    public class Action5406 : BaseAction
    {
        private const int Top = 10;
        private int _rankingNo;
        private List<BossUser> _bossUserList = new List<BossUser>();
        private int _activeId;
        private int _damageNum;
        private string _nickName = string.Empty;
        private int _userLv = 0;
        private short _isCurr;
        private GameDataCacheSet<GameUser> _cacheSet = new GameDataCacheSet<GameUser>();

        public Action5406(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5406, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_rankingNo);
            PushIntoStack(_bossUserList.Count);
            foreach (BossUser item in _bossUserList)
            {
                GameUser user = _cacheSet.FindKey(item.UserId);
                var dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserId.ToNotNullString());
                dsItem.PushIntoStack(item.NickName.ToNotNullString());
                dsItem.PushIntoStack(item.DamageNum);
                dsItem.PushIntoStack(user == null ? 0.ToShort() : user.UserLv);
                PushIntoStack(dsItem);
            }
            PushIntoStack(_damageNum);
            PushIntoStack(_nickName);
            PushIntoStack(_userLv);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ActiveId", ref _activeId)
                && httpGet.GetWord("IsCurr", ref _isCurr))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (_isCurr == 0)
            {
                var bossFirst = ServerEnvSet.Get(ServerEnvKey.FirstHalfBoss, 0);
                if (!string.IsNullOrEmpty(bossFirst))
                {
                    int total;
                    var tempList = JsonUtils.Deserialize<List<BossUser>>(bossFirst);
                    if (tempList != null && tempList.Count > 0)
                    {
                        _bossUserList = tempList.GetPaging(1, Top, out total);
                        _rankingNo = tempList.FindIndex(m => m.UserId == Uid) + 1;
                        var killUserId = ServerEnvSet.GetInt(ServerEnvKey.KillBossUserID, 0);
                        var killUser = tempList.Find(t => t.UserId == killUserId.ToString());
                        if (killUser != null)
                        {
                            _damageNum = killUser.DamageNum;
                            _nickName = killUser.NickName;
                            var userEntity = new GameDataCacheSet<GameUser>().FindKey(killUserId.ToString());
                            if (userEntity != null)
                            {
                                _userLv = userEntity.UserLv;
                            }
                        }
                    }
                }
            }
            else if (_isCurr == 1)
            {
                BossCombat bossCombat = new BossCombat(_activeId);
                GameActive gameActive = bossCombat.GameActive;
                if (gameActive == null)
                {
                    return true;
                }
                CombatStatus combatStatus = gameActive.CombatStatus;
                if (combatStatus == CombatStatus.Wait || combatStatus == CombatStatus.Combat)
                {
                    int total;
                    var tempList = bossCombat.RefreshRanking();
                    _bossUserList = tempList.GetPaging(1, Top, out total);
                    _rankingNo = tempList.FindIndex(m => m.UserId == Uid) + 1;
                    var killUserId = ServerEnvSet.GetInt(ServerEnvKey.KillBossUserID, 0);
                    var killUser = tempList.Find(t => t.UserId == killUserId.ToString());
                    if (killUser != null)
                    {
                        _damageNum = killUser.DamageNum;
                        _nickName = killUser.NickName;
                        var userEntity = new GameDataCacheSet<GameUser>().FindKey(killUserId.ToString());
                        if (userEntity != null)
                        {
                            _userLv = userEntity.UserLv;
                        }
                    }
                }
            }
            return true;
        }
    }
}