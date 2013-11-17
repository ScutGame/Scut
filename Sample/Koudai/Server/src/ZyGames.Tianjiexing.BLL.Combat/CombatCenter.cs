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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 战斗控制中心
    /// </summary>
    public class CombatCenter
    {
        private CombatProcess _processLog;
        private TargetHandle _targerHandle;
        private CombatGeneral _attrGeneral;
        private readonly EmbattleRole _targerRole;
        private EmbattleRole _role;
        public CombatCenter(EmbattleRole role, CombatGeneral general, EmbattleRole targerRole, TargetHandle targetHandle)
        {
            _role = role;
            _attrGeneral = general;
            _targerRole = targerRole;
            _targerHandle = targetHandle;
            _processLog = new CombatProcess();
        }

        public CombatProcess GetProcess()
        {
            return _processLog;
        }
       
        /// <summary>
        /// 判断是否有持续伤害[未完成]
        /// 判断是普通还是技能功能
        /// 计算功击伤害
        /// 触发主动属性几率（爆击、必杀）
        /// 触发被动属性几率（闪避、格挡）
        /// 计算双方气势
        /// </summary>
        /// <returns>False:结束战斗</returns>
        public bool HasDoing()
        {
            //1.判断攻击方式
            CombatBaseAttack baseAttack = CombatBaseAttack.CheckAttack(_attrGeneral);
            //2.取攻击目标和取攻击单位
            if (baseAttack.BeforeAttack())
            {
                CombatGeneral[] generalList = baseAttack.GetTargetRoleUnit(_targerHandle, _role);
                
                if (generalList.Length == 0)
                {
                    return false;
                }
                foreach (CombatGeneral tagetGeneral in generalList)
                {
                    
                    baseAttack.TagetGeneral = tagetGeneral;
                    baseAttack.Attack();
                    IGeneral replaceTagetGeneral;
                    if (tagetGeneral.IsOver && _targerHandle(_targerRole).ReplaceWaitGeneral(tagetGeneral.Position, out replaceTagetGeneral))
                    {
                        ReplaceGeneralPostion(tagetGeneral.UserID, replaceTagetGeneral);
                        baseAttack.TargetProcess.TrumpStatusList.Add(new SkillInfo() { AbilityID = (int)AbilityType.ReplaceGeneral, Num = 0 });
                    }
                }
            }
            baseAttack.AfterAttack();

            IGeneral replaceGeneral;
            if (_attrGeneral.IsOver && _targerHandle(_role).ReplaceWaitGeneral(_attrGeneral.Position, out replaceGeneral))
            {
                ReplaceGeneralPostion(_attrGeneral.UserID, replaceGeneral);
                baseAttack.ProcessLog.TrumpStatusList.Add(new SkillInfo() { AbilityID = (int)AbilityType.ReplaceGeneral, Num = 0 });
            }
            //回档最后一轮
            _processLog = baseAttack.ProcessLog;
            _processLog.Role = _role;

            return true;
        }

        private static void ReplaceGeneralPostion(string userID, IGeneral replaceGeneral)
        {
            var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(userID, replaceGeneral.GeneralID);
            if (userGeneral != null)
            {
                userGeneral.ReplacePosition = replaceGeneral.Position;
            }
        }
    }
}