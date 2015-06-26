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
using System;
using System.Threading;
using ProtoBuf;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存周期
    /// </summary>
    [ProtoContract, Serializable]
    internal class CachePeriod
    {
        private DateTime _preAccessTime;
        private DateTime _accessTime;
        private int _preAccessCounter;
        private int _accessCounter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodTime"></param>
        public CachePeriod(int periodTime)
        {
            _preAccessTime = DateTime.Now;
            _accessTime = DateTime.Now;
            PeriodTime = periodTime;
        }

        private int _periodTime;

        /// <summary>
        /// 过期时间，单位秒
        /// </summary>
        public int PeriodTime
        {
            get
            {
                return _periodTime;
            }
            set
            {
                _periodTime = value;
                if (_periodTime <= 0)
                {
                    IsPersistence = true;
                }
            }
        }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsPeriod
        {
            get { return !IsPersistence && MathUtils.DiffDate(_accessTime).TotalSeconds > PeriodTime; }
        }

        /// <summary>
        /// 是否持久(周期)
        /// </summary>
        public bool IsPersistence
        {
            get;
            private set;
        }

        /// <summary>
        /// 访问计数频率
        /// </summary>
        public double CounterFrequency
        {
            get
            {
                
                int counter = MathUtils.Subtraction(_accessCounter, _preAccessCounter);
                double timeSpan = MathUtils.DiffDate(_accessTime, _preAccessTime).TotalSeconds;
                if (timeSpan > 0)
                {
                    return counter / timeSpan;
                }
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetCounter()
        {
            _preAccessCounter = 0;
            _accessCounter = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshAccessTime()
        {
            _preAccessTime = _accessTime;
            _accessTime = DateTime.Now;
            _preAccessCounter = _accessCounter;
            Interlocked.Increment(ref _accessCounter);
        }

    }
}