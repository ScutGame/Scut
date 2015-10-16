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
using System.Diagnostics;
using System.Text;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Timing
{
    /// <summary>
    /// Watch code process run time.
    /// </summary>
    public class RunTimeWatch : IDisposable
    {
        private static string MessageTip = "{0} {1}ms{2}";
        private string _message;
        private Stopwatch _watch;
        private long preTime;
        private StringBuilder log = new StringBuilder();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static RunTimeWatch StartNew(string message)
        {
            return new RunTimeWatch(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public RunTimeWatch(string message)
        {
            this._message = message;
            _watch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Check of tag use time.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="timeout"></param>
        public void Check(string tag, int timeout = 0)
        {
            var ms = _watch.ElapsedMilliseconds;
            var ts = ms - preTime;
            preTime = ms;
            if (timeout == 0 || (timeout > 0 && ts > timeout))
            {
                log.AppendLine();
                log.AppendFormat(">>{0} time:{1}ms run:{2}ms", tag, preTime, ts);
            }
        }
        /// <summary>
        /// Write to log.
        /// </summary>
        public void Flush(bool error = false, int timeout = 0)
        {
            var time = _watch.ElapsedMilliseconds;
            if (error && (timeout == 0 || time > timeout))
            {
                TraceLog.WriteError(MessageTip, _message, time, log);
            }
            else if (timeout == 0 || time > timeout)
            {
                TraceLog.ReleaseWriteDebug(MessageTip, _message, time, log);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Elapsed
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _watch.Stop();
            Elapsed = _watch.Elapsed;
            Flush();
            _watch = null;
        }

    }
}