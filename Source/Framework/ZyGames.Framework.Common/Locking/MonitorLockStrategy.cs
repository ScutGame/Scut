/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Locking
{
    /// <summary>
    /// Monitor锁策略
    /// </summary>
    public class MonitorLockStrategy : IMonitorStrategy
    {
        private readonly object _syncRoot = new object();
        private readonly int _timeOut;
        /// <summary>
        /// 
        /// </summary>
        public MonitorLockStrategy()
            : this(1000)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeOut"></param>
        public MonitorLockStrategy(int timeOut)
        {
            _timeOut = timeOut > 0 ? timeOut : 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool TryEnterLock(LockCallback handle)
        {
            using (var l = Lock())
            {
                if (l != null && l.TryEnterLock())
                {
                    if (handle != null)
                    {
                        handle();
                    }
                }
                else
                {
                    TraceLog.WriteError("Monitor lock timeout:{0}", _timeOut);
                }
                return l != null && l.IsLocked;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILocking Lock()
        {
            return new MonitorSlim(_syncRoot, _timeOut);
        }
    }
}