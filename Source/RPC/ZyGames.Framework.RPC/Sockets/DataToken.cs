using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.RPC.Sockets
{

    /// <summary>
    /// 
    /// </summary>
    class DataToken
    {
        /// <summary>
        /// 消息体信息
        /// </summary>
        internal byte[] byteArrayForMessage;
        /// <summary>
        /// 头部信息
        /// </summary>
        internal byte[] byteArrayForPrefix;
        /// <summary>
        /// 已处理消息字节数
        /// </summary>
        internal int messageBytesDone;
        /// <summary>
        /// 已处理的头部字节数
        /// </summary>
        internal int prefixBytesDone;
        /// <summary>
        /// 消息字节长度
        /// </summary>
        internal int messageLength;
        /// <summary>
        /// 缓存区的位置
        /// </summary>
        internal int bufferOffset;
        /// <summary>
        /// 
        /// </summary>
        internal int bufferSkip;

        /// <summary>
        /// 
        /// </summary>
        public SocketObject Socket { get; set; }



        internal DataToken(int offset)
        {
            bufferOffset = offset;
            byteArrayForPrefix = new byte[4];
        }


        internal int dataOffset
        {
            get { return bufferOffset + bufferSkip; }
        }

        /// <summary>
        /// 剩余字节
        /// </summary>
        internal int RemainByte
        {
            get { return messageLength - messageBytesDone; }
        }

        /// <summary>
        /// 是否还有完成消息
        /// </summary>
        internal bool IsMessageReady
        {
            get { return messageBytesDone == messageLength; }
        }
        
        /// <summary>
        /// 重置消息
        /// </summary>
        /// <param name="skip"></param>
        internal void Reset(bool skip)
        {
            this.byteArrayForMessage = null;
            byteArrayForPrefix[0] = 0;
            byteArrayForPrefix[1] = 0;
            byteArrayForPrefix[2] = 0;
            byteArrayForPrefix[3] = 0;
            this.messageBytesDone = 0;
            this.prefixBytesDone = 0;
            this.messageLength = 0;
            if (skip)
                this.bufferSkip = 0;
        }
    }
}
