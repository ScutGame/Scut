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
namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 
    /// </summary>
    public enum MessageError
    {
        /// <summary>
        /// 
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 系统错误
        /// </summary>
        SystemError = 10000
    }

    /// <summary>
    /// 消息头
    /// </summary>
    public class MessageHead
    {
        /// <summary>
        /// default st
        /// </summary>
        public const string DefaultSt = "st";

        /// <summary>
        /// 
        /// </summary>
        public MessageHead()
        {
            ErrorInfo = string.Empty;
            St = DefaultSt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        public MessageHead(int action, int errorCode = 0, string errorInfo = "")
            : this(0, action, DefaultSt, errorCode, errorInfo)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="action"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        public MessageHead(int msgId, int action, int errorCode = 0, string errorInfo = "")
            : this(msgId, action, DefaultSt, errorCode, errorInfo)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="action"></param>
        /// <param name="st"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorInfo"></param>
        public MessageHead(int msgId, int action, string st = DefaultSt, int errorCode = 0, string errorInfo = "")
        {
            MsgId = msgId;
            Action = action;
            St = st;
            ErrorCode = errorCode;
            ErrorInfo = errorInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetSystemError()
        {
            ErrorCode = (int)MessageError.SystemError;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return ErrorCode == 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Faild
        {
            get { return ErrorCode == (int)MessageError.SystemError; }
        }
        /// <summary>
        /// 消息包总字节
        /// </summary>
        public long PacketLength
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gzip压缩包的长度
        /// </summary>
        public long GzipLength { get; internal set; }

        /// <summary>
        /// 消息体总字节
        /// </summary>
        public long TotalLength
        {
            get;
            internal set;
        }
        /// <summary>
        /// Push:固定下发0,R-R:下发请求的MsgId
        /// </summary>
        public int MsgId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Action
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ErrorInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string St
        {
            get;
            set;
        }

        /// <summary>
        /// 是否包括Gzip压缩头部长度信息
        /// </summary>
        public bool HasGzip { get; set; }

        /// <summary>
        /// 客户端版本，0：旧版本
        /// </summary>
        public int ClientVersion { get; set; }
    }
}