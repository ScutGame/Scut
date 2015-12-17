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
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageQueueSection : ConfigSection
    {
        /// <summary>
        /// 
        /// </summary>
        public MessageQueueSection()
        {
            SlaveMessageQueue = ConfigUtils.GetSetting("Slave.MessageQueue", "");
            EnableRedisQueue = ConfigUtils.GetSetting("Cache.enable.redisqueue", true);
            EnableWriteToDb = ConfigUtils.GetSetting("Cache.enable.writetoDb", true);
            DataSyncQueueNum = ConfigUtils.GetSetting("DataSyncQueueNum", 2);
            SqlWaitSyncQueueNum = ConfigUtils.GetSetting("SqlWaitSyncQueueNum", 2);
            SqlSyncInterval = ConfigUtils.GetSetting("Game.Cache.UpdateDbInterval", 60000);//1 min
            SqlSyncQueueNum = ConfigUtils.GetSetting("SqlSyncQueueNum", 1);
        }

        /// <summary>
        /// Slave message queue name
        /// </summary>
        public string SlaveMessageQueue { get; set; }

        /// <summary>
        /// Enable redis queue
        /// </summary>
        public bool EnableRedisQueue { get; set; }

        /// <summary>
        /// Enable write to Db.
        /// </summary>
        public bool EnableWriteToDb { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DataSyncQueueNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SqlWaitSyncQueueNum { get; set; }

        /// <summary>
        /// default 5min
        /// </summary>
        public int SqlSyncInterval { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SqlSyncQueueNum { get; set; }
    }
}
