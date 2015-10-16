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
using System.Collections.Concurrent;
using System.Text;
using System.Web;
using NLog;

namespace ZyGames.Framework.Common.Log
{
    internal class LogHelper
    {
        private static bool _isinit;
        private static bool _logInfoEnable;
        private static bool _logErrorEnable;
        private static bool _logWarnEnable;
        private static bool _logComplementEnable;
        private static bool _logDubugEnable;
        private static bool _logFatalEnabled;
        private static Logger _logger;
        private static ConcurrentDictionary<string, Logger> _customLoggers;

        static LogHelper()
        {
            _customLoggers = new ConcurrentDictionary<string, Logger>();
            _isinit = false;
            _logInfoEnable = false;
            _logErrorEnable = false;
            _logWarnEnable = false;
            _logComplementEnable = false;
            _logDubugEnable = false;
            _logFatalEnabled = false;
            _logger = LogManager.GetCurrentClassLogger();

            if (!_isinit)
            {
                _isinit = true;
                SetConfig();
            }
        }
        public static void SetConfig()
        {
            _logInfoEnable = _logger.IsInfoEnabled;
            _logErrorEnable = _logger.IsErrorEnabled;
            _logWarnEnable = _logger.IsWarnEnabled;
            _logComplementEnable = _logger.IsTraceEnabled;
            _logFatalEnabled = _logger.IsFatalEnabled;
            _logDubugEnable = _logger.IsDebugEnabled;
        }
        public static void WriteInfo(string info)
        {
            if (LogHelper._logInfoEnable)
            {
                LogHelper._logger.Info(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteDebug(string info)
        {
            if (LogHelper._logDubugEnable)
            {
                LogHelper._logger.Debug(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteError(string info)
        {
            if (LogHelper._logErrorEnable)
            {
                LogHelper._logger.Error(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteException(string info, Exception ex)
        {
            if (LogHelper._logErrorEnable)
            {
                LogHelper._logger.Error(LogHelper.BuildMessage(info, ex));
            }
        }
        public static void WriteWarn(string info)
        {
            if (LogHelper._logWarnEnable)
            {
                LogHelper._logger.Warn(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteWarn(string info, Exception ex)
        {
            if (LogHelper._logWarnEnable)
            {
                LogHelper._logger.Warn(LogHelper.BuildMessage(info, ex));
            }
        }
        public static void WriteFatal(string info)
        {
            if (LogHelper._logFatalEnabled)
            {
                LogHelper._logger.Fatal(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteComplement(string info)
        {
            WriteTo("", info);
        }
        public static void WriteComplement(string info, Exception ex)
        {
            WriteTo("", info, ex);
        }

        public static void WriteTo(string name, string info, Exception ex = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "Complement";
            }
            var lazy = new Lazy<Logger>(() => LogManager.GetLogger(name));
            Logger customLog = _customLoggers.GetOrAdd(name, lazy.Value);
            if (customLog != null)
            {
                customLog.Log(LogLevel.Trace, LogHelper.BuildMessage(info, ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message)
        {
            WriteLine(LogLevel.Info, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public static void WriteLine(LogLevel level, string message)
        {
            _logger.Log(level, message);
        }

        private static string BuildMessage(string info)
        {
            return LogHelper.BuildMessage(info, null);
        }
        private static string BuildMessage(string info, Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            HttpContext current = HttpContext.Current;
            try
            {
                stringBuilder.AppendFormat("Time:{0}-{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), info);
                if (current != null)
                {
                    HttpRequest request = null;
                    try
                    {
                        //IIS 7.0 集成模式不能用
                        request = current.Request;
                    }
                    catch
                    {
                    }

                    if (request != null)
                    {
                        stringBuilder.AppendFormat("Url:{0}\r\n", current.Request.Url);
                        if (null != current.Request.UrlReferrer)
                        {
                            stringBuilder.AppendFormat("UrlReferrer:{0}\r\n", current.Request.UrlReferrer);
                        }
                        stringBuilder.AppendFormat("UserHostAddress:{0}\r\n", current.Request.UserHostAddress);
                    }
                }
                if (ex != null)
                {
                    stringBuilder.AppendFormat("Exception:{0}\r\n", ex.ToString());
                }
            }
            catch (Exception error)
            {
                stringBuilder.AppendLine(info + ", Exception:\r\n" + error);
            }
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }
    }
}