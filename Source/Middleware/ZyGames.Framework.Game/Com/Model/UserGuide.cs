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
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 引导状态
    /// </summary>
    public enum GuideStatus
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Complate,
        /// <summary>
        /// 
        /// </summary>
        Closed
    }
    /// <summary>
    /// 引导进度项
    /// </summary>
    [Serializable, ProtoContract]
    public class GuideProgressItem : EntityChangeEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public GuideProgressItem()
            : base(false)
        {
        }

        private int _guideId;

        /// <summary>
        /// 
        /// </summary>    
        [ProtoMember(1)]
        public int GuideId
        {
            get { return _guideId; }
            set
            {
                if (!Equals(_guideId, value))
                {
                    _guideId = value;
                    BindAndNotify(_guideId);
                }
            }
        }

        private GuideStatus _status;

        /// <summary>
        /// 
        /// </summary>    
        [ProtoMember(2)]
        public GuideStatus Status
        {
            get { return _status; }
            set
            {
                if (!Equals(_status, value))
                {
                    _status = value;
                    BindAndNotify(_status);
                }
            }
        }
    }

    /// <summary>
    /// 玩家引导记录
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class UserGuide : BaseEntity
    {

        /// <summary>
        /// </summary>
        protected UserGuide()
            : base(AccessLevel.ReadWrite)
        {
        }

        /// <summary>
        /// </summary>        
        [ProtoMember(1)]
        public abstract Int32 UserId { get; set; }

        /// <summary>
        /// 是否结束引导
        /// </summary>        
        [ProtoMember(2)]
        public abstract Boolean IsClose
        {
            get;
            set;
        }

        /// <summary>
        /// 当前引导ID
        /// </summary>        
        [ProtoMember(3)]
        public abstract Int32 CurrGuideId
        {
            get;
            set;
        }

        /// <summary>
        /// 引导进度
        /// </summary>        
        [ProtoMember(4)]
        public virtual CacheList<GuideProgressItem> GuideProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 完成时间
        /// </summary>        
        [ProtoMember(5)]
        public virtual DateTime CloseDate
        {
            get;
            set;
        }

    }
}