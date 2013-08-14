using System;
using ZyGames.Framework.Model;
using ZyGames.Framework.Event;

namespace ZyGames.Framework.Game.Task
{
    public enum TaskStatus
    {
        /// <summary>
        /// 不可接
        /// </summary>
        NoTake = 0,
        /// <summary>
        /// 可接
        /// </summary>
        AllowTake,
        /// <summary>
        /// 已接
        /// </summary>
        Taked,
        /// <summary>
        /// 完成
        /// </summary>
        Completed,
        /// <summary>
        /// 结束
        /// </summary>
        Close,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable
    }

    /// <summary>
    /// 游戏任务信息
    /// </summary>
    public interface ITaskItem
    {
        int UserID { get; set; }

        int TaskID { get; set; }

        TaskStatus Status { get; set; }

        DateTime CreateDate { get; set; }
    }
}
