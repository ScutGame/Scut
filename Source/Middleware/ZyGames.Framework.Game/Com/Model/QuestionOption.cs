using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Event;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 问题选项
    /// </summary>
    public class QuestionOption : CacheItemChangeEvent
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }
    }
}
