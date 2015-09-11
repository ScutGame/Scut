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

namespace ZyGames.Framework.Common.Threading
{
    /// <summary>
    /// Stopwatch class
    /// Used with WindowsCE and Silverlight which don't have Stopwatch
    /// </summary>
    internal class Stopwatch
    {
        private long _elapsed;
        private bool _isRunning;
        private long _startTimeStamp;

        public Stopwatch()
        {
            Reset();
        }

        private long GetElapsedDateTimeTicks()
        {
            long rawElapsedTicks = GetRawElapsedTicks();
            return rawElapsedTicks;
        }

        private long GetRawElapsedTicks()
        {
            long elapsed = _elapsed;
            if (_isRunning)
            {
                long ticks = GetTimestamp() - _startTimeStamp;
                elapsed += ticks;
            }
            return elapsed;
        }

        public static long GetTimestamp()
        {
            return DateTime.UtcNow.Ticks;
        }

        public void Reset()
        {
           _elapsed = 0L;
           _isRunning = false;
           _startTimeStamp = 0L;
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _startTimeStamp = GetTimestamp();
                _isRunning = true;
            }
        }

        public static Stopwatch StartNew()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        public void Stop()
        {
            if (_isRunning)
            {
                long ticks = GetTimestamp() - _startTimeStamp;
                _elapsed += ticks;
                _isRunning = false;
            }
        }

        // Properties
        public TimeSpan Elapsed
        {
            get
            {
                return new TimeSpan(GetElapsedDateTimeTicks());
            }
        }

        public long ElapsedMilliseconds
        {
            get
            {
                return (GetElapsedDateTimeTicks() / 0x2710L);
            }
        }

        public long ElapsedTicks
        {
            get
            {
                return GetRawElapsedTicks();
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }
    }
}