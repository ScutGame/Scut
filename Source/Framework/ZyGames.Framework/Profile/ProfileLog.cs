using System;
using System.Threading;

namespace ZyGames.Framework.Profile
{
    /// <summary>
    /// Profile log entity
    /// </summary>
    public class ProfileLog
    {
        /// <summary>
        /// init
        /// </summary>
        public ProfileLog()
        {
            Reset();
        }

        private long _total;

        /// <summary>
        /// Current count
        /// </summary>
        private long _current;

        /// <summary>
        /// Current max count
        /// </summary>
        private long _max;

        /// <summary>
        /// Total count
        /// </summary>
        public long Total { get { return _total; } }


        /// <summary>
        /// Reset times
        /// </summary>
        /// <returns>return current count of reset before</returns>
        public long Reset()
        {
            var val = Interlocked.Exchange(ref _current, 0);
            _max = val > _max ? val : _max;
            return val;
        }

        /// <summary>
        /// times
        /// </summary>
        public void Countor()
        {
            Interlocked.Increment(ref _total);
            Interlocked.Increment(ref _current);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        public void Countor(int count)
        {
            Interlocked.Add(ref _total, count);
            Interlocked.Add(ref _current, count);
        }
    }
}
