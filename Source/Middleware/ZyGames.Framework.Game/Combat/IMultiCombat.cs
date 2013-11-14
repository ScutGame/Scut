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
using ZyGames.Framework.Game.Model;

namespace ZyGames.Framework.Game.Combat
{
    /// <summary>
    /// 多对多战斗
    /// </summary>
    public interface IMultiCombat
    {
        /// <summary>
        /// 加入攻击方阵列
        /// </summary>
		/// <param name="combatGrid"></param>
        void AppendAttack(EmbattleQueue combatGrid);

        /// <summary>
        /// 加入防守方阵列
        /// </summary>
		/// <param name="combatGrid"></param>
        void AppendDefend(EmbattleQueue combatGrid);

        /// <summary>
        /// 交战
        /// </summary>
        /// <returns>返回胜利或失败</returns>
        bool Doing();

        /// <summary>
        /// 交战过程
        /// </summary>
        /// <returns></returns>
        object GetProcessResult();

        /// <summary>
        /// 回合数
        /// </summary>
        int BoutNum { get; }
    }
}