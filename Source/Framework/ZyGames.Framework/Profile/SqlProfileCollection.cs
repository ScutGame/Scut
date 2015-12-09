using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZyGames.Framework.Profile
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlProfileCollection
    {

        /// <summary>
        /// 
        /// </summary>
        public SqlProfileCollection()
        {
            PostTimes = new ProfileLog();
            ProcessTimes = new ProfileLog();
        }

        /// <summary>
        /// Sql命令处理失败计数
        /// </summary>
        public long TotalFailCount;

        /// <summary>
        /// 延迟等待同步数
        /// </summary>
        public long WaitSyncCount;

        /// <summary>
        /// Post times
        /// </summary>
        public ProfileLog PostTimes { get; private set; }
        /// <summary>
        /// process times
        /// </summary>
        public ProfileLog ProcessTimes { get; private set; }

    }
}
