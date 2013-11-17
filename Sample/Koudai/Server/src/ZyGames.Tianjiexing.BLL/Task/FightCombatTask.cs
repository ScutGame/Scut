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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Task
{
    /// <summary>
    /// 公会城市争斗战
    /// </summary>
    public class FightCombatTask : BaseTask
    {
        public FightCombatTask()
            : base(30000)
        {
            TaskName = "ServerCombatTask";
        }
        protected override void DoExecute(object obj)
        {
            try
            {
                DateTime nextDate;
                FightStage stage = GuildFightCombat.GetStage(out nextDate);
                GuildFightCombat.GetCombatPrize();
                GuildFightCombat.ChangStage(stage);
                if (stage == FightStage.quarter_final || stage == FightStage.semi_final || stage == FightStage.final)
                {
                    GuildFightCombat.ServerFinal(stage);
                    new BaseLog().SaveLog("公会争斗战正在运行！");
                }
                GuildFightCombat.RemoveGuildFight();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("GuildFightCombatTaskError-->{0}", ex);
            }
        }
    }
}