using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.Game.Context
{
    /// <summary>
    /// IUser
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// user's token
        /// </summary>
        string Token { get; set; }
        /// <summary>
        /// is lock status
        /// </summary>
        bool IsLock { get; }

 
        /// <summary>
        /// is online
        /// </summary>
        bool IsOnlining { get; }

        /// <summary>
        /// get userid
        /// </summary>
        /// <returns></returns>
       int GetUserId();
        /// <summary>
        /// get passport
        /// </summary>
        /// <returns></returns>
        string GetPassportId();
        /// <summary>
        /// reflesh date
        /// </summary>
        void RefleshOnlineDate();

    }
}
