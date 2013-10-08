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

namespace ZyGames.Framework.Game.Command
{
    /// <summary>
    /// GM命令基类
    /// </summary>
    public abstract class BaseCommand
    {
        private const string SectionName = "zyGameBase-GM";

        private static bool EnableGM { get; set; }

        static BaseCommand()
        {
            EnableGM = false;
            string str = ConfigurationManager.AppSettings["EnableGM"];
            if (!string.IsNullOrEmpty(str))
            {
                EnableGM = Convert.ToBoolean(str);
            }
        }

        public static void Run(string userID, string command)
        {
            try
            {
                if (!EnableGM) throw new Exception("GM命令未开启暂不能使用！");

                command = command != null ? command.Trim() : string.Empty;
                string[] paramList = command.Split(new char[] { ' ' });
                if (paramList.Length < 1)
                {
                    throw new Exception("无效参数");
                }

                string cmd = paramList[0].Replace("GM:", "").Trim();
                string[] args = new string[0];
                if (paramList.Length > 1)
                {
                    args = paramList[1].Split(new char[] { ',' });
                }
                BaseCommand baseCommand = CreateCommand(cmd);
                if (baseCommand != null)
                {
                    baseCommand.UserID = userID;
                    baseCommand.ProcessCmd(args);
                }
            }
            catch (Exception ex)
            {
                ErrorFormat(command, ex);
            }
        }

        private static BaseCommand CreateCommand(string cmd)
        {
            CommandCollection cmdList = ((GmSection)ConfigurationManager.GetSection(SectionName)).Command;
            CommandElement cmdElement = cmdList[cmd];
            if (cmdElement == null)
            {
                throw new Exception("[" + cmd + "]找不到处理器");
            }

            return (BaseCommand)Activator.CreateInstance(Type.GetType(cmdElement.TypeName));
        }


        private static void ErrorFormat(string command, Exception ex)
        {
            throw new Exception(string.Format("GM命令:{0}执行失败", command), ex);
        }

        public BaseCommand()
        {
        }

        public string UserID
        {
            get;
            set;
        }

        protected abstract void ProcessCmd(string[] args);

    }
}