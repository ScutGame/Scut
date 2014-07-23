using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 玩家信息[:BaseUser]
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(DbConfig.Data)]
    public class GameUser : BaseUser
    {

        /// <summary>
        /// </summary>
        public GameUser()
        {
            Property = new UserProperty();
        }

        public GameUser(int userid)
            :this()
        {
            UserId = userid;
        }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityField(IsKey = true)]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityField]
        public string Pid { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityField]
        public string NickName { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityField]
        public string HeadIcon { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(5)]
        [EntityField]
        public bool Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(6)]
        [EntityField]
        public string RetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(7)]
        [EntityField]
        public int PayGold { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(8)]
        [EntityField]
        public int GiftGold { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(9)]
        [EntityField]
        public int ExtGold { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(10)]
        [EntityField]
        public int UseGold { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(11)]
        [EntityField]
        public int GameCoin { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(12)]
        [EntityField]
        public short UserLv { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(13)]
        [EntityField]
        public int VipLv { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(14)]
        [EntityField]
        public int RankId { get; set; }

        /// <summary>
        /// 玩家状态[Enum<UserStatus>]
        /// </summary>        
        [ProtoMember(15)]
        [EntityField]
        public UserStatus UserStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(16)]
        [EntityField]
        public bool MsgState { get; set; }

        /// <summary>
        /// 玩家状态[Enum<MobileType>]
        /// </summary>        
        [ProtoMember(17)]
        [EntityField]
        public MobileType MobileType { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(18)]
        [EntityField]
        public short ScreenX { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(19)]
        [EntityField]
        public short ScreenY { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(20)]
        [EntityField]
        public short ClientAppVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(21)]
        [EntityField]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(22)]
        [EntityField]
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(23)]
        [EntityField]
        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(24)]
        [EntityField]
        public int WinNum { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(25)]
        [EntityField]
        public int FailNum { get; set; }

        /// <summary>
        /// 积分
        /// </summary>        
        [ProtoMember(26)]
        [EntityField]
        public int ScoreNum { get; set; }

        /// <summary>
        /// 称号ID
        /// </summary>        
        [ProtoMember(27)]
        [EntityField]
        public int TitleId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>        
        [ProtoMember(28)]
        [EntityField]
        public string RealName { get; set; }

        /// <summary>
        /// 生日
        /// </summary>        
        [ProtoMember(29)]
        [EntityField]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 爱好
        /// </summary>        
        [ProtoMember(30)]
        [EntityField]
        public string Hobby { get; set; }

        /// <summary>
        /// 职业
        /// </summary>        
        [ProtoMember(31)]
        [EntityField]
        public string Profession { get; set; }
    
        
        protected override int GetIdentityId()
        {
            return UserId;
        }        
        

        public override int GetUserId()
        {
            return UserId;
        }

        public override string GetNickName()
        {
            return NickName;
        }

        public override string GetPassportId()
        {
            return Pid;
        }

        public override string GetRetailId()
        {
            return RetailId;
        }

        public override bool IsFengJinStatus
        {
            get { return UserStatus == UserStatus.FengJin; }
        }

        /// <summary>
        /// 玩家会话ID
        /// </summary>
        [ProtoMember(100)]
        public string SessionID { get; set; }

        /// <summary>
        /// 玩家请求时间,排除常刷新接口
        /// </summary>
        [ProtoMember(101)]
        public override DateTime OnlineDate { get; set; }

        /// <summary>
        /// 玩家属性
        /// </summary>
        [ProtoMember(102)]
        public UserProperty Property { get; set; }

	}
}