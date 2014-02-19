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
using System.Diagnostics;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model;
using System;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6106_Boss战伤害排名接口
    /// </summary>
    public class Action6106 : BaseAction
    {
        private const int Top = 10;
        private int _rankingNo;
        private List<BossUser> _bossUserList = new List<BossUser>();
        private int _activeId;


        public Action6106(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6106, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_rankingNo);
            PushIntoStack(_bossUserList.Count);
            foreach (BossUser item in _bossUserList)
            {
                var dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserId.ToNotNullString());
                dsItem.PushIntoStack(item.NickName.ToNotNullString());
                dsItem.PushIntoStack(item.DamageNum);

                PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ActiveId", ref _activeId))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (!string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                GuildBossCombat bossCombat = new GuildBossCombat(ContextUser.MercenariesID);
                UserGuild guild = bossCombat.UserGuild;
                if (guild != null)
                {
                    GameActive gameActive = new ShareCacheStruct<GameActive>().FindKey(_activeId);
                    CombatStatus combatStatus = guild.CombatStatus;
                    if (combatStatus == CombatStatus.Wait || combatStatus == CombatStatus.Combat)
                    {
                        int total;
                        var tempList = bossCombat.RefreshRanking();
                        _bossUserList = tempList.GetPaging(1, Top, out total);
                        _rankingNo = tempList.FindIndex( m => m.UserId == Uid) + 1;
                    }
                }
                stopwatch.Stop();
                new BaseLog().SaveLog("公会boss伤害排名所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");
            }
            return true;
        }
    }
}