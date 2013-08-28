using System;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 新手引导配置
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class GuideData : ShareEntity
    {
        /// <summary>
        /// </summary>
        protected GuideData()
            : base(AccessLevel.ReadOnly)
        {
        }
        /// <summary>
        /// </summary>
        protected GuideData(Int32 id)
            : this()
        {
            this._ID = id;
        }

        private Int32 _ID;

        /// <summary>
        /// 引导配置ID
        /// </summary>       
        [ProtoMember(1)]
        public abstract Int32 ID { get; }

        /// <summary>
        /// 引导类型
        /// </summary>        
        public abstract Int32 Type { get; }

        /// <summary>
        /// 引导子类型
        /// </summary>        
        public abstract Int32 SubType { get; }

        /// <summary>
        /// 下一引导
        /// </summary>
        public abstract Int32 NextID { get; }

        /// <summary>
        /// 引导名称
        /// </summary>     
        public abstract String Name { get; }

        /// <summary>
        /// 引导描述
        /// </summary>       
        public abstract String Description { get; }

        /// <summary>
        /// 完成获取奖励
        /// </summary>     
        public abstract String Prize { get; }


    }
}
