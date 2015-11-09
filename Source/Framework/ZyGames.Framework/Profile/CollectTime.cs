using System;
using System.Threading;

namespace ZyGames.Framework.Profile
{
    /// <summary>
    /// Collect time struct
    /// </summary>
    public struct CollectTime
    {
        /// <summary>
        /// 
        /// </summary>
        public long Count;
        /// <summary>
        /// 
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// Reset time and count
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref Count, 0);
            Time = DateTime.Now;
        }
    }
}