using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Model;

namespace GameRanking.Model
{
    /// <summary>
    /// 玩家配置集合
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(CacheType.Entity, "ConnData")]
    public class UserSet : ShareEntity
    {

        private readonly static object SyncRoot = new object();

        /// <summary>
        /// 生成Id
        /// </summary>
        /// <returns></returns>
        public static int GenerateId()
        {
            lock (SyncRoot)
            {
                string userCode = "USER_CODE";
                var userCache = new ShareCacheStruct<UserSet>();
                UserSet userSet = userCache.FindKey(userCode);
                if (userSet == null)
                {
                    userSet = new UserSet() { Code = userCode, CurrUserId = 1000 };
                    userCache.Add(userSet);
                }
                userSet.CurrUserId++;
                userCache.Update();
                return userSet.CurrUserId;
            }
        }

        public UserSet()
            : base(false)
        {
        }

        /// <summary>
        /// 编号代码
        /// </summary>
        [ProtoMember(1)]
        [EntityField(true)]
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 当前玩家编号
        /// </summary>
        [ProtoMember(2)]
        [EntityField]
        public int CurrUserId
        {
            get;
            set;
        }
    }
}