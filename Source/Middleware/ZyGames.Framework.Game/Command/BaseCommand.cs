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
using System.Configuration;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Configuration;

namespace ZyGames.Framework.Game.Command
{
    /// <summary>
    /// GM命令基类
    /// </summary>
    public abstract class BaseCommand
    {
        private const string SectionName = "zyGameBase-GM";

        private static bool EnableGM;

        static BaseCommand()
        {
            EnableGM = ConfigUtils.GetSetting("EnableGM", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static bool Check(string command)
        {
            return command.StartsWith("gm:", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 运行GM命令解析
        /// </summary>
        /// <param name="userId">发送GM命令者</param>
        /// <param name="command">GM命令</param>
        /// <param name="assemblyName">指定解析的程序集</param>
        public static void Run(string userId, string command, string assemblyName = "")
        {
            try
            {
                if (!EnableGM)
                {
                    return;
                }

                command = command != null ? command.Trim() : string.Empty;
                string[] paramList = command.Split(new char[] { ' ' });
                if (paramList.Length < 1)
                {
                    return;
                }

                string cmd = paramList[0].Trim();
                if (cmd.Length > 3)
                {
                    cmd = cmd.Substring(3, cmd.Length - 3);
                }

                string[] args = new string[0];
                if (paramList.Length > 1)
                {
                    args = paramList[1].Split(new char[] { ',' });
                }
                BaseCommand baseCommand = null;
                if (string.IsNullOrEmpty(assemblyName))
                {
                    baseCommand = new ScriptCommand(cmd);
                }
                else
                {
                    baseCommand = CreateCommand(cmd, assemblyName);
                }
                if (baseCommand == null)
                {
                    return;
                }
                baseCommand.UserID = userId;
                baseCommand.ProcessCmd(args);
            }
            catch (Exception ex)
            {
                ErrorFormat(command, ex);
            }
        }

        private static BaseCommand CreateCommand(string cmd, string assemblyName)
        {
            string typeName = "";
            if (ZyGameBaseConfigManager.GameSetting.HasSetting)
            {
                typeName = ZyGameBaseConfigManager.GameSetting.GetGmCommandType(cmd);
                typeName = !string.IsNullOrEmpty(typeName) ? typeName : cmd + "Command";
            }
            else
            {
                CommandCollection cmdList = ((GmSection) ConfigurationManager.GetSection(SectionName)).Command;
                CommandElement cmdElement = cmdList[cmd];
                typeName = cmdElement != null ? cmdElement.TypeName : cmd + "Command";
            }

            if (typeName.IndexOf(",") == -1)
            {
                typeName = string.Format("{0}.{1},{0}", assemblyName, typeName);
            }
            var type = Type.GetType(typeName, false, true);
            if (type != null)
            {
                return type.CreateInstance<BaseCommand>();
            }
            return null;
        }


        private static void ErrorFormat(string command, Exception ex)
        {
            TraceLog.WriteError("GM命令:{0}执行失败\r\nException:{1}", command, ex);
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Command.BaseCommand"/> class.
		/// </summary>
        protected BaseCommand()
        {
        }
		/// <summary>
		/// Gets or sets the user I.
		/// </summary>
		/// <value>The user I.</value>
        public string UserID
        {
            get;
            set;
        }
		/// <summary>
		/// Processes the cmd.
		/// </summary>
		/// <param name="args">Arguments.</param>
        protected abstract void ProcessCmd(string[] args);

    }
}