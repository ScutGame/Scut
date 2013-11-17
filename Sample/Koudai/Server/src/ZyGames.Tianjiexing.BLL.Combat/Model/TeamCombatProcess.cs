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
using ZyGames.Core.ExientionType;

namespace ZyGames.Tianjiexing.BLL.Combat.Model
{
    /// <summary>
    /// 多人副本组队
    /// </summary>
    public class TeamCombatProcess
    {
        public bool IsWin
        {
            get;
            set;
        }

        /// <summary>
        /// 队伍ID
        /// </summary>
        public int TeamID
        {
            get;
            set;
        }

        /// <summary>
        /// 阵型位置
        /// </summary>
        public int Position
        {
            get;
            set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 副本Id
        /// </summary>
        public int PlotID
        {
            get;
            set;
        }

        /// <summary>
        /// 副本npc
        /// </summary>
        public int PlotNpcID
        {
            get;
            set;
        }

        /// <summary>
        /// 战斗日志
        /// </summary>
        public CombatProcessContainer ProcessContainer
        {
            get;
            set;
        }
    }
}