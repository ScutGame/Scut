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
using ZyGames.Framework.SyncThreading;

namespace ZyGames.Framework.MSMQ
{
    /// <summary>
    ///消息队列添加类
    /// </summary>
    public class ActionMSMQ : BaseMsmqAction
    {
        private static readonly object syncRoot = new object();
        private const int MessageQueueDefNum = 1;
        private static ActionMSMQ instance;
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static ActionMSMQ Instance()
        {
            SyncHelper.SyncFun(syncRoot,
                () => instance == null,
                () =>
                {
                    instance = new ActionMSMQ();
                });
            return instance;
        }

        private ActionMSMQ()
            : base(AppConfig.SqlCmdMessageQueuePath, AppConfig.SqlCmdMessageQueueNum)
        {

        }
        /// <summary>
        /// 创建队列的构造函数，传入队列路径名称
        /// </summary>
        /// <param name="_MessagePath"></param>
        public ActionMSMQ(string _MessagePath)
            : base(_MessagePath, MessageQueueDefNum)
        {
        }
    }
}