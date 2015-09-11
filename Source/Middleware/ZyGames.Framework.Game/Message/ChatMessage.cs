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
using ProtoBuf;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadWrite, CacheType.Queue, false)]
    public class ChatMessage : ShareEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public ChatMessage()
            : base(AccessLevel.ReadWrite)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1001)]
        public virtual int Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1002)]
        public virtual string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1003)]
        public virtual DateTime SendDate { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        [ProtoMember(1004)]
        public virtual int FromUserID { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        [ProtoMember(1005)]
        public virtual int ToUserID { get; set; }

		/// <summary>
		/// Gets the identity identifier.
		/// </summary>
		/// <returns>The identity identifier.</returns>
        protected override int GetIdentityId()
        {
            return DefIdentityId;
        }
		/// <summary>
		/// 对象索引器属性
		/// </summary>
		/// <returns></returns>
		/// <param name="index">Index.</param>
        protected override object this[string index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}