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
using System.Linq;
using System.Text;
using ZyGames.Tianjiexing.Model;
using ZyGames.Base.Model;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 目标被攻击过程
    /// </summary>
    public class TargetProcess
    {
        public TargetProcess()
        {
            DamageStatusList = new List<AbilityEffectStatus>();
        }
        /// <summary>
        /// 目标佣兵
        /// </summary>
        public int GeneralID
        {
            get;
            set;
        }
        /// <summary>
        /// 生命
        /// </summary>
        public int LiveNum
        {
            get;
            set;
        }
        /// <summary>
        /// 当前气势
        /// </summary>
        public short Momentum
        {
            get;
            set;
        }
        /// <summary>
        /// 佣兵站的位置
        /// </summary>
        public short Position
        {
            get;
            set;
        }
        /// <summary>
        /// 阵形角色
        /// </summary>
        public EmbattleRole Role
        {
            get;
            set;
        }
        /// <summary>
        /// 伤害值
        /// </summary>
        public int DamageNum
        {
            get;
            set;
        }
        /// <summary>
        /// 是否闪避
        /// </summary>
        public bool IsShanBi
        {
            get;
            set;
        }
        /// <summary>
        /// 是否爆击
        /// </summary>
        public bool IsBaoji
        {
            get;
            set;
        }
        /// <summary>
        /// 是否格挡
        /// </summary>
        public bool IsGeDang
        {
            get;
            set;
        }
        /// <summary>
        /// 是否反击
        /// </summary>
        public bool IsBack
        {
            get;
            set;
        }

        public bool IsMove
        {
            get;
            set;
        }
        /// <summary>
        /// 反击伤害
        /// </summary>
        public int BackDamageNum
        {
            get;
            set;
        }
        /// <summary>
        /// 佣兵状态
        /// </summary>
        public BattleStatus TargetStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 中招持续效果
        /// </summary>
        public List<AbilityEffectStatus> DamageStatusList
        {
            get;
            set;
        }
    }
}