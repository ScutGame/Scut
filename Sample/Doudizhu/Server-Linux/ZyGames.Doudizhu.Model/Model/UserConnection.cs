using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Model;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 用户连接
    /// </summary>
    public class UserConnection : MemoryEntity
    {
        public UserConnection()
        {
            LocalHost = "";
        }

        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LocalHost { get; set; }
    }
}
