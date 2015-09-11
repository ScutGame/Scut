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
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Command
{
    /// <summary>
    /// 鑴氭湰鎵ц鍛戒护
    /// </summary>
    /// <example>
    /// <code>
    /// # py demo:
    /// ############
    /// def processCmd(userId, args):
    ///     #鍦ㄨ繖閲屽鐞嗗懡浠?
    ///     pass 
    /// </code>
    /// </example>
    public class ScriptCommand : BaseCommand
    {
        private readonly string _cmd;

        ///<summary>
        ///</summary>
        public ScriptCommand(string cmd)
        {
            _cmd = cmd;
        }
        /// <summary>
        /// Processes the cmd.
        /// </summary>
        /// <param name="args">Arguments.</param>
        protected override void ProcessCmd(string[] args)
        {
            string routeName = string.Format("Gm.{0}", _cmd);
            dynamic scriptScope = ScriptEngines.Execute(routeName, null);
            if (scriptScope != null)
            {
                try
                {
                    scriptScope.processCmd(UserID, args);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Gm:{0} process error:{1}", _cmd, ex);
                }
            }
        }
    }
}