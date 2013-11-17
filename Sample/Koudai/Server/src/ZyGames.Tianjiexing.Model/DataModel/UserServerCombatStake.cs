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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;
using ProtoBuf;
using System.Runtime.Serialization;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Tianjiexing.Model
{
    /// <summary>
    /// 玩家跨服战下注表
    /// </summary>
    [Serializable, ProtoContract, EntityTable(CacheType.Entity, DbConfig.Data, "UserServerCombatStake")]
    public class UserServerCombatStake : ShareEntity
    {
        public UserServerCombatStake()
            : base(AccessLevel.ReadWrite)
        { }

        public UserServerCombatStake(string UserID, int ServerID, string AgainstID)
            : this()
        {
            this.UserID = UserID;
            this.ServerID = ServerID;
            this.AgainstID = AgainstID;
        }

        private string _UserID;
        /// <summary>
        ///  玩家ID
        /// </summary>
        [ProtoMember(1)]
        [EntityField("UserID", IsKey = true)]
        public string UserID
        {
            get { return _UserID; }
            set { SetChange("UserID", value); }
        }

        private int _FastID;
        /// <summary>
        /// 活动ID
        /// </summary>
        [EntityField("FastID")]
        [ProtoMember(2)]
        public int FastID
        {
            get { return _FastID; }
            set { SetChange("FastID", value); }
        }

        private int _ServerID;
        /// <summary>
        /// 活动ID
        /// </summary>
        [EntityField("ServerID", IsKey = true)]
        [ProtoMember(3)]
        public int ServerID
        {
            get { return _ServerID; }
            set { SetChange("ServerID", value); }
        }

        private string _AgainstID;
        /// <summary>
        /// 对阵ID
        /// </summary>
        [EntityField("AgainstID", IsKey = true)]
        [ProtoMember(4)]
        public string AgainstID
        {
            get { return _AgainstID; }
            set { SetChange("AgainstID", value); }
        }

        private int _StakeUserID;
        /// <summary>
        /// 下注对象
        /// </summary>
        [EntityField("StakeUserID")]
        [ProtoMember(5)]
        public int StakeUserID
        {
            get { return _StakeUserID; }
            set { SetChange("StakeUserID", value); }
        }

        private int _StakeType;
        /// <summary>
        /// 下注类型
        /// </summary>
        [EntityField("StakeType")]
        [ProtoMember(6)]
        public int StakeType
        {
            get { return _StakeType; }
            set { SetChange("StakeType", value); }
        }

        private bool _IsGet;
        /// <summary>
        /// 是否领取
        /// </summary>
        [EntityField("IsGet")]
        [ProtoMember(7)]
        public bool IsGet
        {
            get { return _IsGet; }
            set { SetChange("IsGet", value); }
        }

        private DateTime _opTime;
        /// <summary>
        /// 下注时间
        /// </summary>
        [EntityField("opTime")]
        [ProtoMember(8)]
        public DateTime opTime
        {
            get { return _opTime; }
            set { SetChange("opTime", value); }
        }

        public ServerCombatStakeInfo StakeInfo
        {
            get { return ServerCombatStakeInfo.GetStakeInfo(this.StakeType); }
        }



        /// <summary>
        /// 结果
        /// </summary>
        public int Result
        {
            get
            {
                if (aginst.Result == 0)
                    return 0;
                if (StakeUserID == aginst.Result)
                    return 1;
                return 2;
            }
        }

        public UserServerCombatAgainst aginst
        {
            get
            {
                return new ShareCacheStruct<UserServerCombatAgainst>().FindKey(AgainstID);
            }
        }




        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "UserID": return UserID;
                    case "FastID": return FastID;
                    case "ServerID": return ServerID;
                    case "AgainstID": return AgainstID;
                    case "StakeUserID": return StakeUserID;
                    case "StakeType": return StakeType;
                    case "IsGet": return IsGet;
                    case "opTime": return opTime;
                    default: throw new ArgumentException(string.Format("UserServerCombatStake index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "UserID":
                        this._UserID = value.ToNotNullString();
                        break;
                    case "FastID":
                        this._FastID = value.ToInt();
                        break;
                    case "ServerID":
                        this._ServerID = value.ToInt();
                        break;
                    case "AgainstID":
                        this._AgainstID = value.ToNotNullString();
                        break;
                    case "StakeUserID":
                        this._StakeUserID = value.ToInt();
                        break;
                    case "StakeType":
                        this._StakeType = value.ToInt();
                        break;
                    case "IsGet":
                        this._IsGet = value.ToBool();
                        break;
                    case "opTime":
                        this._opTime = value.ToDateTime();
                        break;
                    default: throw new ArgumentException(string.Format("UserServerCombatStake index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        protected override int GetIdentityId()
        {
            return DefIdentityId.ToInt();
        }
    }

    /// <summary>
    /// 下注配置
    /// </summary>
    public class ServerCombatStakeInfo
    {
        public int StakeType
        {
            get;
            private set;
        }
        public int StakeNum
        {
            get;
            private set;
        }
        public static List<ServerCombatStakeInfo> StakeInfo
        {
            get;
            set;
        }

        static ServerCombatStakeInfo()
        {
            StakeInfo = new List<ServerCombatStakeInfo>();
            StakeInfo.Add(new ServerCombatStakeInfo() { StakeType = 1, StakeNum = 10000 });
            StakeInfo.Add(new ServerCombatStakeInfo() { StakeType = 2, StakeNum = 20000 });
            StakeInfo.Add(new ServerCombatStakeInfo() { StakeType = 3, StakeNum = 50000 });
        }

        public static ServerCombatStakeInfo GetStakeInfo(int StakeType)
        {
            foreach (var item in StakeInfo)
            {
                if (item.StakeType == StakeType)
                    return item;
            }
            return null;
        }



    }

}