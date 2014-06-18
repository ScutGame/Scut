using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 任务、成就状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 未完成
        /// </summary>
        Unfinished =1,

        /// <summary>
        /// 完成
        /// </summary>
        Complete,

        /// <summary>
        /// 领取
        /// </summary>
        Receive,
    }
}
