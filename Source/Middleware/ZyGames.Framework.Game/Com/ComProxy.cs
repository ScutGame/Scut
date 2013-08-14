using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Com
{
    /// <summary>
    /// 中间件操作其它
    /// </summary>
    public abstract class ComProxy
    {
        /// <summary>
        /// 
        /// </summary>
        public ComConfig Config
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual T CreateInstance<T>(params object[] args)
        {
            return Activator.CreateInstance<T>();
        }
    }
}
