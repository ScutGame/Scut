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
using System.Diagnostics;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 6104_Boss战血量刷新接口
    /// </summary>
    public class Action6104 : BaseAction
    {
        private int currLifeNum;
        private int maxLifeNum;
        private int _activeId;

        public Action6104(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6104, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(currLifeNum);
            this.PushIntoStack(maxLifeNum);

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
                UserGuild _guild = bossCombat.UserGuild;
                if (_guild != null)
                {
                    CombatGeneral boss = bossCombat.Boss;
                    if (boss != null)
                    {
                        currLifeNum = boss.LifeNum;
                        maxLifeNum = boss.LifeMaxNum;
                    }
                }
                stopwatch.Stop();
                new BaseLog().SaveLog("公会boss刷新血量所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");
            }
            return true;
        }
    }
}