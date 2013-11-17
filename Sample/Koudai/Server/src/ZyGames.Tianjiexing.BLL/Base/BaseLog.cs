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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;


namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// BaseLog 的摘要说明
    /// 保存日志到文本的接口类
    /// </summary>
    public class BaseLog
    {
        /// <summary>
        /// 文件夹名称
        /// </summary>
        protected string MFolderName = string.Empty;

        /// <summary>
        /// 缺省构造器
        /// </summary>
        public BaseLog()
        {
            //
        }
        /// <summary>
        /// 接收日志目录信息构造器
        /// </summary>
        /// <param name="folderName">日志目录信息</param>
        public BaseLog(string folderName)
        {
            this.MFolderName = folderName;
        }
        /// <summary>
        /// 记录一些基本信息（非异常日志）
        /// </summary>
        /// <param name="message">记录的消息内容</param>
        public void SaveLog(String message)
        {
            TraceLog.ReleaseWrite(string.Format("{0}消息：{1}", this.MFolderName, message));
        }

        public void SaveDebugLog(String message)
        {
            TraceLog.ReleaseWriteDebug(string.Format("{0}消息：{1}", this.MFolderName, message));
        }
        /// <summary>
        /// 记录异常日志信息
        /// </summary>
        /// <param name="aExObj">异常对象</param>
        public void SaveLog(Exception aExObj)
        {
            if (aExObj == null)
            {
                TraceLog.WriteError(string.Format("{0}发生异常：{1}", this.MFolderName, "aExObj Is Null"), aExObj);
            }
            else
            {
                TraceLog.WriteError(string.Format("{0}发生异常：{1}", this.MFolderName, aExObj.Message), aExObj);
            }
        }
        /// <summary>
        /// 记录异常日志信息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="aExObj">异常对象</param>
        public void SaveLog(string message, Exception aExObj)
        {
            if (aExObj == null)
            {
                TraceLog.WriteError(string.Format("{0}；消息：{1}发生异常：{2}", this.MFolderName, message, "aExObj Is Null"), aExObj);
            }
            else
            {
                TraceLog.WriteError(string.Format("{0}；消息：{1}发生异常：{2}", this.MFolderName, message, aExObj.Message), aExObj);
            }
        }
    }
}