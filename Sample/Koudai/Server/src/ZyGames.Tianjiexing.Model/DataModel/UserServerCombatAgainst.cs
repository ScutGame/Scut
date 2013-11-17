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
    /// 玩家分组表
    /// </summary>
    [Serializable, ProtoContract, EntityTable(CacheType.Entity, DbConfig.Data, "UserServerCombatAgainst")]
    public class UserServerCombatAgainst : ShareEntity
    {
        public UserServerCombatAgainst()
            : base(AccessLevel.ReadWrite)
        { }

        public UserServerCombatAgainst(String ID)
            : this()
        {
            this.ID = ID;
        }

        private string _ID;
        /// <summary>
        ///  对阵ID
        /// </summary>
        [EntityField("ID", IsKey = true)]
        [ProtoMember(1)]
        public string ID
        {
            get { return _ID; }
            set { SetChange("ID", value); }
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

        private int _CombatType;
        /// <summary>
        /// 类型
        /// </summary>
        [EntityField("CombatType")]
        [ProtoMember(3)]
        public int CombatType
        {
            get { return _CombatType; }
            set { SetChange("CombatType", value); }
        }

        private ServerCombatStage _Stage;
        /// <summary>
        /// 阶段
        /// </summary>
        [EntityField("Stage")]
        [ProtoMember(4)]
        public ServerCombatStage Stage
        {
            get { return _Stage; }
            set { SetChange("Stage", value); }
        }

        private int _No;
        /// <summary>
        /// 编号
        /// </summary>
        [EntityField("No")]
        [ProtoMember(5)]
        public int No
        {
            get { return _No; }
            set { SetChange("No", value); }
        }
        private int _Round;
        /// <summary>
        /// 轮次
        /// </summary>
        [EntityField("Round")]
        [ProtoMember(6)]
        public int Round
        {
            get { return _Round; }
            set { SetChange("Round", value); }
        }

        private string _APlayID;
        /// <summary>
        /// 玩家ID
        /// </summary>
        [EntityField("APlayID")]
        [ProtoMember(7)]
        public string APlayID
        {
            get { return _APlayID; }
            set { SetChange("APlayID", value); }
        }

        private int _AServerID;
        /// <summary>
        /// 玩家所在服ID
        /// </summary>
        [EntityField("AServerID")]
        [ProtoMember(8)]
        public int AServerID
        {
            get { return _AServerID; }
            set { SetChange("AServerID", value); }
        }

        private int _Awin;
        /// <summary>
        /// 获胜次数
        /// </summary>
        [EntityField("Awin")]
        [ProtoMember(9)]
        public int Awin
        {
            get { return _Awin; }
            set { SetChange("Awin", value); }
        }

        private string _BPlayID;
        /// <summary>
        /// 玩家ID
        /// </summary>
        [EntityField("BPlayID")]
        [ProtoMember(10)]
        public string BPlayID
        {
            get { return _BPlayID; }
            set { SetChange("BPlayID", value); }
        }

        private int _BServerID;
        /// <summary>
        /// 玩家所在服
        /// </summary>
        [EntityField("BServerID")]
        [ProtoMember(11)]
        public int BServerID
        {
            get { return _BServerID; }
            set { SetChange("BServerID", value); }
        }

        private int _Bwin;
        /// <summary>
        /// 获胜次数
        /// </summary>
        [EntityField("Bwin")]
        [ProtoMember(12)]
        public int Bwin
        {
            get { return _Bwin; }
            set { SetChange("Bwin", value); }
        }

        /// <summary>
        /// 结果0没有结果
        /// </summary>
        public int Result
        {
            get
            {
                if (Awin == 0 && Bwin == 0)
                    return 0;
                switch (Stage)
                {
                    case ServerCombatStage.Close:
                    case ServerCombatStage.Apply:
                        return 0;
                    case ServerCombatStage.serverkonckout:
                    case ServerCombatStage.finalskonckout:
                        return Awin == 0 ? 2 : 1;
                    case ServerCombatStage.finals_32:
                    case ServerCombatStage.finals_16:
                    case ServerCombatStage.quarter_final:
                    case ServerCombatStage.semi_final:
                    case ServerCombatStage.final:
                        if (Awin == 3) return 1;
                        if (Bwin == 3) return 2;
                        return 0;
                    case ServerCombatStage.champion:
                    default:
                        return 0;
                }
            }
        }


        public UserServerCombatApply applyA
        {
            get { return new ShareCacheStruct<UserServerCombatApply>().FindKey(APlayID, FastID, AServerID); }
        }

        public UserServerCombatApply applyB
        {
            get { return new ShareCacheStruct<UserServerCombatApply>().FindKey(BPlayID, FastID, BServerID); }
        }



        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "ID": return ID;
                    case "Stage": return Stage;
                    case "FastID": return FastID;
                    case "No": return No;
                    case "Round": return Round;
                    case "APlayID": return APlayID;
                    case "AServerID": return AServerID;
                    case "Awin": return Awin;
                    case "BPlayID": return BPlayID;
                    case "BServerID": return BServerID;
                    case "Bwin": return Bwin;
                    case "CombatType": return CombatType;
                    default: throw new ArgumentException(string.Format("UserServerCombatAgainst index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "ID":
                        this._ID = value.ToNotNullString();
                        break;
                    case "CombatType":
                        this._CombatType = value.ToInt();
                        break;
                    case "FastID":
                        this._FastID = value.ToInt();
                        break;
                    case "Stage":
                        this._Stage = value.ToEnum<ServerCombatStage>();
                        break;
                    case "No":
                        this._No = value.ToInt();
                        break;
                    case "Round":
                        this._Round = value.ToInt();
                        break;
                    case "APlayID":
                        this._APlayID = value.ToNotNullString();
                        break;
                    case "AServerID":
                        this._AServerID = value.ToInt();
                        break;
                    case "Awin":
                        this._Awin = value.ToInt();
                        break;
                    case "BPlayID":
                        this._BPlayID = value.ToNotNullString();
                        break;
                    case "BServerID":
                        this._BServerID = value.ToInt();
                        break;
                    case "Bwin":
                        this._Bwin = value.ToInt();
                        break;
                    default: throw new ArgumentException(string.Format("UserServerCombatAgainst index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        protected override int GetIdentityId()
        {
            return DefIdentityId.ToInt();
        }
    }
}