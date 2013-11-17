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
using ZyGames.Framework.Game.Combat;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// </summary>
    /// <example>
    /// ICombatController controller =  new TjxCombatController();
    /// ISingleCombat combat = controller.GetSingleCombat(null);
    /// if (combat.Doing())
    /// {
    ///     combat.GetProcessResult();
    /// }
    /// </example>
    public class TjxCombatController : ICombatController
    {
        #region ICombatController 成员

        public IManyOneCombat GetManyOneCombat(object args)
        {
            return null;//new BossCombat(args);
        }

        public IMultiCombat GetMultiCombat(object args)
        {
            return null;// new MorePlotCombat(args);
        }

        public ISingleCombat GetSingleCombat(object args)
        {
            return new PlotCombat(args);
        }

        #endregion
    }
}