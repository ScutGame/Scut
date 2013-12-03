using System;
using ProtoBuf;
using ZyGames.Framework.Model;


namespace GameNotice.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(CacheType.Entity, "GameData")]
    public class Notice :  ShareEntity
    {

        /// <summary>
        /// </summary>
        public Notice()
            : base(false)
        {
        }        
        /// <summary>
        /// </summary>
        public Notice(int id)
            : this()
        {
            Id = id;
        }
    
        [ProtoMember(1)]
        [EntityField(true)]
        public int Id { get; private set; }
     
        [ProtoMember(2)]
        [EntityField]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public DateTime CreateDate { get; set; }
    
	}
}