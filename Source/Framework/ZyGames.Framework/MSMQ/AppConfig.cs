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
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.MSMQ
{
    /// <summary>
    /// 服务器环境配置类
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// 当前游戏类型
        /// </summary>
        public static short CurGameType { get; set; }

        static AppConfig()
        {
            try
            {
                SqlCmdMessageQueuePath = ConfigUtils.GetSetting("MessageQueuePath", @".\private$\GameDefaultCmdSql");
                SqlCmdMessageQueueNum = ConfigUtils.GetSetting("MessageQueueNum", 10);
            }
            catch (Exception ex)
            {
#if DEBUG
                Trace.WriteLine(string.Format("服务器环境配置异常:{0}", ex.ToString()));
#else
                TraceLog.WriteError("服务器环境配置异常:{0}", ex);
#endif
            }
        }

        /// <summary>
        /// 获得消息队列中，执行数据库脚本的队列路径与名称
        /// </summary>
        public static string SqlCmdMessageQueuePath { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public static int SqlCmdMessageQueueNum { get; private set; }

    }
}