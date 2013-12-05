/*********************************************************
 * Copyright (C) 2010
 * Developer：WUZF
 * Createtime：2013/10/25 11:44:46
 * Description：
 * 
 * History：
 * 
 * *******************************************************/

using System;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Command
{
    /// <summary>
    /// 脚本执行命令
    /// </summary>
    /// <example>
    /// <code>
    /// # py demo:
    /// ############
    /// def processCmd(userId, args):
    ///     #在这里处理命令
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
            string routeName = string.Format("Gm\\{0}.py", _cmd);
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