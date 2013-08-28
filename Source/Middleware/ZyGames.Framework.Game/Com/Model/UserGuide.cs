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
                _guideId = value;
                NotifyByModify();
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
                _status = value;
                NotifyByModify();
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
        public abstract Int32 UserId { get; }

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
