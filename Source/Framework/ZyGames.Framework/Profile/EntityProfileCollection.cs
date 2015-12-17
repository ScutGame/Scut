using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZyGames.Framework.Profile
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityProfileCollection
    {
        private HashSet<string> _postKeys;
        private HashSet<string> _processKeys;
        /// <summary>
        /// 第二次未被处理的Key，若有则记录异常日志
        /// </summary>
        private HashSet<string> _secondNoProcessKeys;
        /// <summary>
        /// 第一次未被处理的Key
        /// </summary>
        private HashSet<string> _noProcessKeys;

        /// <summary>
        /// 
        /// </summary>
        public EntityProfileCollection()
        {
            _postKeys = new HashSet<string>();
            _processKeys = new HashSet<string>();
            _secondNoProcessKeys = new HashSet<string>();
            _noProcessKeys = new HashSet<string>();
            ChangeAutoTimes = new ProfileLog();
            PostTimes = new ProfileLog();
            ProcessTimes = new ProfileLog();
        }

        /// <summary>
        /// Total count
        /// </summary>
        public long TotalPostObjectCount;

        /// <summary>
        /// Total count
        /// </summary>
        public long TotalProcessObjectCount;

        /// <summary>
        /// 
        /// </summary>
        public long TotalNoProcessObjectCount;

        /// <summary>
        /// change auto times
        /// </summary>
        public ProfileLog ChangeAutoTimes { get; private set; }

        /// <summary>
        /// post times
        /// </summary>
        public ProfileLog PostTimes { get; private set; }

        /// <summary>
        /// process times
        /// </summary>
        public ProfileLog ProcessTimes { get; private set; }

        internal void PostKeyCountor(string key)
        {
            _postKeys.Add(key);
        }

        internal void ProcessKeyCountor(string key)
        {
            _processKeys.Add(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postObjectCount"></param>
        /// <param name="processObjectCount"></param>
        public void Reset(out long postObjectCount, out long processObjectCount)
        {
            var postKeys = Interlocked.Exchange(ref _postKeys, new HashSet<string>());
            var processKeys = Interlocked.Exchange(ref _processKeys, new HashSet<string>());
            postObjectCount = postKeys.Count;
            processObjectCount = processKeys.Count;
            TotalPostObjectCount += postObjectCount;
            TotalProcessObjectCount += processObjectCount;

            var preKeys = Interlocked.Exchange(ref _noProcessKeys, new HashSet<string>());
            //上次未处理的Key
            foreach (var preKey in preKeys)
            {
                _secondNoProcessKeys.Add(preKey);
            }
            _secondNoProcessKeys.RemoveWhere(processKeys.Contains);

            postKeys.RemoveWhere(processKeys.Contains);
            foreach (var key in postKeys)
            {
                _noProcessKeys.Add(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> PopNoProcessKeys()
        {
            return Interlocked.Exchange(ref _secondNoProcessKeys, new HashSet<string>());
        }
    }
}
