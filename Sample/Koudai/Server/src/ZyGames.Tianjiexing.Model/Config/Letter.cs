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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// [临时缓存]玩家聊天表
    /// </summary>
    [Serializable, ProtoContract]
    public class Letter
    {
        /// <summary>
        /// 自增，主键
        /// </summary>
        [ProtoMember(1)]
        public string LetterID
        {
            get;
            set;
        }

        /// <summary>
        /// 发件人ID
        /// </summary>
        [ProtoMember(2)]
        public string FromUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 收件人ID 0：发给所有人
        /// </summary>
        [ProtoMember(3)]
        public string ToUserID
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [ProtoMember(4)]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 发送内容
        /// </summary>
        [ProtoMember(5)]
        public string Content
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public ChatType LetterType
        {
            get;
            set;
        }

        /// <summary>
        /// 已读状态
        /// </summary>
        [ProtoMember(7)]
        public Int16 IsRead
        {
            get;
            set;
        }

        /// <summary>
        /// 发送时间
        /// </summary>
        [ProtoMember(8)]
        public DateTime SendDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(9)]
        public string FromUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(10)]
        public string ToUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 发送内容(原始)
        /// </summary>
        [ProtoMember(11)]
        public string Content2
        {
            get;
            set;
        }
    }
}