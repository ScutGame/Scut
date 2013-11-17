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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 普通物理攻击
    /// </summary>
    public class CombatPhysicalAttack : CombatBaseAttack
    {
        public CombatPhysicalAttack(CombatGeneral general)
            : base(general)
        {
        }

        public override void Attack()
        {
            TargetProcess targetProcess = new TargetProcess();
            targetProcess.GeneralID = TagetGeneral.GeneralID;
            targetProcess.Position = TagetGeneral.Position.ToShort();
            targetProcess.Role = TagetRole;
            targetProcess.LiveNum = TagetGeneral.LifeNum;
            targetProcess.Momentum = TagetGeneral.Momentum;

            ProccessExtendAttack();
            int damageNum = GetPhysicalDamageNum(TagetGeneral);
            decimal hitPercent = 0;
            bool isHit = General.TriggerHitPercent(TagetGeneral, out hitPercent);
            ProcessLog.AddPercent(1, hitPercent);
            if (isHit)
            {
                
                targetProcess.IsShanBi = false;
                //普通攻击\防御加气势
                General.Momentum = MathUtils.Addition(General.Momentum, MomentumAttack, short.MaxValue);
                if (!TagetGeneral.IsJueduifangyuStatus)
                {
                    TagetGeneral.Momentum = MathUtils.Addition(TagetGeneral.Momentum, MomentumDefense, short.MaxValue);
                }
                DoTargetDefense(damageNum, targetProcess, false);
                //攻击加血
                decimal attackLife = TrumpAbilityAttack.GetEffect(General, AbilityType.AttackLife);
                if (attackLife > 0)
                {
                    ProcessLog.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.AttackLife, -(int)(damageNum * attackLife)));
                    General.LifeNum = (int)Math.Floor(MathUtils.Addition(General.LifeNum, damageNum * attackLife, General.LifeMaxNum));
                }

                //反伤
                decimal fangShang = TrumpAbilityAttack.GetEffect(TagetGeneral, AbilityType.FanShang);
                if (fangShang > 0)
                {
                    int num = (int)(damageNum * fangShang);
                    targetProcess.TrumpStatusList.Add(TrumpAbilityAttack.GetSkillprocess(AbilityType.FanShang, num));
                    General.LifeNum = (int)Math.Floor(MathUtils.Subtraction(General.LifeNum, damageNum * fangShang));
                }
            }
            else
            {
                targetProcess.IsShanBi = true;
                TagetGeneral.TriggerShanBi();
            }
            targetProcess.Momentum = TagetGeneral.Momentum;
            targetProcess.TargetStatus = TagetGeneral.BattleStatus;
            ProcessLog.Momentum = General.Momentum;
            ProcessLog.LiveNum = General.LifeNum;
            ProcessLog.AttStatus = General.BattleStatus;
            ProcessLog.IsMove = General.IsMove;
            ProcessLog.TargetList.Add(targetProcess);
        }


        protected override AttackTaget GetAttackTaget()
        {
            return AttackTaget.Enemy;
        }

        protected override AttackUnit GetAttackUnit()
        {
            return AttackUnit.Single;
        }
    }
}