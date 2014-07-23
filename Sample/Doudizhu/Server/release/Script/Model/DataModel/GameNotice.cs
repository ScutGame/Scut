using System;
using System.Runtime.Serialization;
using ProtoBuf;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, ProtoContract, EntityTable(CacheType.Entity, DbConfig.Data, OrderColumn = "IsTop desc,CreateDate desc")]
    public class GameNotice : ShareEntity
    {


        public GameNotice()
            : base(false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        [EntityField(true)]
        public String NoticeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(2)]
        [EntityField]
        public String Title{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(3)]
        [EntityField]
        public String Content{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(4)]
        [EntityField]
        public DateTime ExpiryDate{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(5)]
        [EntityField]
        public Boolean IsTop{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(6)]
        [EntityField]
        public Boolean IsBroadcast{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(7)]
        [EntityField]
        public String Creater{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(8)]
        [EntityField]
        public DateTime CreateDate{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(9)]
        [EntityField]
        public int NoticeType{ get; set; }

    }
}