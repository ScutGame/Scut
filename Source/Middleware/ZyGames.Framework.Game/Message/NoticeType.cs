using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// 公告类型
    /// </summary>
    public enum NoticeType
    {
        /// <summary>
        /// 系统广播
        /// </summary>
        System = 1,
        /// <summary>
        /// 游戏内部广播
        /// </summary>
        Game,
        /// <summary>
        /// 玩家广播
        /// </summary>
        User
    }
}
