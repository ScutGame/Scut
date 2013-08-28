using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Com
{
    /// <summary>
    /// 中间件配置信息
    /// </summary>
    public class ComConfig
    {
        /// <summary>
        /// 中件件唯一标识
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 中间件子类类名
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///  中间件参数
        /// </summary>
        public object[] Params { get; set; }
    }
}
