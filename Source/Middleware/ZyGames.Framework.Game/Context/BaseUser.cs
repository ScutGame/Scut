using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Context
{
    /// <summary>
    /// 游戏角色
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class BaseUser : BaseEntity
    {
        protected BaseUser(AccessLevel access)
            : base(access)
        {
        }

        private ContextCacheSet<CacheItem> _userData;

        public ContextCacheSet<CacheItem> UserData
        {
            get
            {
                if (_userData == null)
                {
                    _userData = new ContextCacheSet<CacheItem>(string.Format("__gamecontext_{0}", GetUserId()));
                }
                return _userData;
            }
        }

        /// <summary>
        /// 在线间隔小时数
        /// </summary>
        protected int OnlineHourInterval = 8;

        public abstract bool IsFengJinStatus { get; }

        public abstract DateTime OnlineDate { get; set; }

        public bool IsInlining
        {
            get
            {
                return MathUtils.DiffDate(OnlineDate).TotalHours > OnlineHourInterval;
            }
        }


        /// <summary>
        /// 远端SessionId
        /// </summary>
        [JsonIgnore]
        public Guid SocketSid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string RemoteAddress { get; set; }

        /// <summary>
        /// 360渠道Token
        /// </summary>
        [JsonIgnore]
        public string Token360 { get; set; }

        //public abstract int GameId { get; set; }

        //public abstract int ServerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetSessionId();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract int GetUserId();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetNickName();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetPassportId();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetRetailId();
        /// <summary>
        /// 
        /// </summary>
        public virtual void RefleshOnlineDate()
        {
            OnlineDate = DateTime.Now;
        }
    }
}
