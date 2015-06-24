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
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 公告消息
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadWrite, CacheType.Queue, false)]
    public class NoticeMessage : ShareEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public NoticeMessage()
            : this(0, 0)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="noticeType"></param>
        /// <param name="versionId"></param>
        public NoticeMessage(NoticeType noticeType, int versionId)
            :base(false)
        {
            _id = Guid.NewGuid();
            _noticeType = noticeType;
            VersionId = versionId;
        }
        /// <summary>
        /// 
        /// </summary>
        public int VersionId { get; private set; }

        private Guid _id;
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1001)]
        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        private NoticeType _noticeType;

        /// <summary>
        /// 类型
        /// </summary>
        [ProtoMember(1002)]
        public virtual NoticeType NoticeType
        {
            get { return _noticeType; }
            set { _noticeType = value; }
        }

        private string _title;

        /// <summary>
        /// 标题
        /// </summary>
        [ProtoMember(1003)]
        public virtual string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _content;

        /// <summary>
        /// 内容
        /// </summary>
        [ProtoMember(1004)]
        public virtual string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private DateTime _expiryDate;

        /// <summary>
        /// 过期时间
        /// </summary>
        [ProtoMember(1006)]
        public virtual DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set { _expiryDate = value; }
        }

        private string _sender;

        /// <summary>
        /// 发送人
        /// </summary>
        [ProtoMember(1007)]
        public virtual string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        private DateTime _sendDate;

        /// <summary>
        /// 发送时间
        /// </summary>
        [ProtoMember(1008)]
        public virtual DateTime SendDate
        {
            get { return _sendDate; }
            set { _sendDate = value; }
        }

        /// <summary>
        /// 播放次数
        /// </summary>
        [ProtoMember(1009)]
        public int PlayTimes { get; set; }

        /// <summary>
        /// 目标扩展ID
        /// </summary>
        [ProtoMember(1010)]
        public object TargetId { get; set; }
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