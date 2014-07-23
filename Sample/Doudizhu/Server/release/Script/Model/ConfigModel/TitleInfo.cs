using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;


namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 称号配置
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, DbConfig.Config)]
    public class TitleInfo : ShareEntity
    {

        /// <summary>
        /// </summary>
        public TitleInfo()
            : base(true)
        {
            
        }        

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(true)]
        public int Id{ get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public string Name{ get; set; }

        /// <summary>
        /// 达到值
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public int TargetNum { get; set; }

	}
}