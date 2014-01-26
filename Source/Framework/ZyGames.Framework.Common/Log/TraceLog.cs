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

namespace ZyGames.Framework.Common.Log
{
    /// <summary>
    /// 日志操作类
    /// </summary>
    /// <example>
    /// <code>
    /// public class Program
    /// {
    ///     static void Main()
    ///     {
    ///         TraceLog.ReleaseWrite("Hello {0}", "Kim")
    ///         TraceLog.WriteError("Error {0}", new Exception("error test."))
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class TraceLog
    {
        private static BaseLog log = new BaseLog();

        /// <summary>
        /// 只在编译器的DEBUG下输出到Debug目录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Write(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveDebugLog(str);
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveDebugLog(str);
#endif
        }

        /// <summary>
        /// 只在编译器的DEBUG下输出到Info目录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void WriteInfo(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveLog(str);
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveLog(str);
#endif
        }

        /// <summary>
        /// 记录出错日志到Error目录下
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void WriteError(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveLog(new Exception(str));
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveLog(new Exception(str));
#endif
        }

        /// <summary>
        /// 记录出错日志到Warn目录下
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void WriteWarn(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveWarnLog(str);
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveWarnLog(str);
#endif
        }
        /// <summary>
        /// 记录出错日志到Complement目录下
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void WriteComplement(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveComplementLog(str);
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveComplementLog(str);
#endif
        }

        /// <summary>
        /// 在DEBUG和Release下输出到Info目录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void ReleaseWrite(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveLog(str);
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveLog(str);
#endif
        }
        /// <summary>
        /// 记录出错日志到Debug目录下
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void ReleaseWriteDebug(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveDebugLog(str);
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveDebugLog(str);
#endif
        }
        /// <summary>
        /// 记录出错日志到Fatal目录下
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void ReleaseWriteFatal(string message, params object[] args)
        {
            string str = message;
            if (args.Length > 0)
            {
                str = string.Format("Trace>>" + message, args);
            }
#if DEBUG
            log.SaveDebugLog(str);
            Trace.WriteLine(str);
            Console.WriteLine(str);
#else
            log.SaveFatalLog(str);
#endif
        }
    }
}