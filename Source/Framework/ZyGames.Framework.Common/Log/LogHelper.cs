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
using System.Text;
using System.Web;
using NLog;

namespace ZyGames.Framework.Common.Log
{
    internal class LogHelper
    {
        private static bool isinit;
        private static bool LogInfoEnable;
        private static bool LogErrorEnable;
        private static bool LogWarnEnable;
        private static bool LogComplementEnable;
        private static bool LogDubugEnable;
        private static bool LogFatalEnabled;
        private static Logger logger;
        static LogHelper()
        {
            LogHelper.isinit = false;
            LogHelper.LogInfoEnable = false;
            LogHelper.LogErrorEnable = false;
            LogHelper.LogWarnEnable = false;
            LogHelper.LogComplementEnable = false;
            LogHelper.LogDubugEnable = false;
            LogHelper.LogFatalEnabled = false;
            LogHelper.logger = LogManager.GetCurrentClassLogger();
            if (!LogHelper.isinit)
            {
                LogHelper.isinit = true;
                LogHelper.SetConfig();
            }
        }
        public static void SetConfig()
        {
            LogHelper.LogInfoEnable = LogHelper.logger.IsInfoEnabled;
            LogHelper.LogErrorEnable = LogHelper.logger.IsErrorEnabled;
            LogHelper.LogWarnEnable = LogHelper.logger.IsWarnEnabled;
            LogHelper.LogComplementEnable = LogHelper.logger.IsTraceEnabled;
            LogHelper.LogFatalEnabled = LogHelper.logger.IsFatalEnabled;
            LogHelper.LogDubugEnable = LogHelper.logger.IsDebugEnabled;
        }
        public static void WriteInfo(string info)
        {
            if (LogHelper.LogInfoEnable)
            {
                LogHelper.logger.Info(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteDebug(string info)
        {
            if (LogHelper.LogDubugEnable)
            {
                LogHelper.logger.Debug(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteError(string info)
        {
            if (LogHelper.LogErrorEnable)
            {
                LogHelper.logger.Error(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteException(string info, Exception ex)
        {
            if (LogHelper.LogErrorEnable)
            {
                LogHelper.logger.Error(LogHelper.BuildMessage(info, ex));
            }
        }
        public static void WriteWarn(string info)
        {
            if (LogHelper.LogWarnEnable)
            {
                LogHelper.logger.Warn(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteWarn(string info, Exception ex)
        {
            if (LogHelper.LogWarnEnable)
            {
                LogHelper.logger.Warn(LogHelper.BuildMessage(info, ex));
            }
        }
        public static void WriteFatal(string info)
        {
            if (LogHelper.LogFatalEnabled)
            {
                LogHelper.logger.Fatal(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteComplement(string info)
        {
            if (LogHelper.LogComplementEnable)
            {
                LogHelper.logger.Trace(LogHelper.BuildMessage(info));
            }
        }
        public static void WriteComplement(string info, Exception ex)
        {
            if (LogHelper.LogComplementEnable)
            {
                LogHelper.logger.Trace(LogHelper.BuildMessage(info, ex));
            }
        }
        private static string BuildMessage(string info)
        {
            return LogHelper.BuildMessage(info, null);
        }
        private static string BuildMessage(string info, Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            HttpContext current = HttpContext.Current;
            stringBuilder.AppendFormat("Time:{0}-{1}\r\n", DateTime.Now, info);
            if (current != null)
            {
                stringBuilder.AppendFormat("Url:{0}\r\n", current.Request.Url);
                if (null != current.Request.UrlReferrer)
                {
                    stringBuilder.AppendFormat("UrlReferrer:{0}\r\n", current.Request.UrlReferrer);
                }
                stringBuilder.AppendFormat("UserHostAddress:{0}\r\n", current.Request.UserHostAddress);
            }
            if (ex != null)
            {
                stringBuilder.AppendFormat("Exception:{0}\r\n", ex.ToString());
            }
            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }
    }
}