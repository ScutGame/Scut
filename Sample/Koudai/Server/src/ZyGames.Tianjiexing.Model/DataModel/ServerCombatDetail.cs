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
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;
using ProtoBuf;
using System.Runtime.Serialization;
using ZyGames.Tianjiexing.Model.Config;
using Newtonsoft.Json;
namespace ZyGames.Tianjiexing.Model
{
    [Serializable, ProtoContract, JsonObject, EntityTable(CacheType.Entity, DbConfig.Data, "ServerCombatDetail")]
    public class ServerCombatDetail : ShareEntity
    {

        private string _AgainstID;

        public ServerCombatDetail()
            : base(AccessLevel.ReadWrite)
        {
        }

        /// <summary>
        /// 对阵ID
        /// </summary>
        [EntityField("AgainstID", IsKey = true)]
        [ProtoMember(1)]
        public string AgainstID
        {
            get { return _AgainstID; }
            set { SetChange("AgainstID", value); }
        }

        private int _Round;
        /// <summary>
        /// 轮次
        /// </summary>
        [EntityField("Round", IsKey = true)]
        [ProtoMember(2)]
        public int Round
        {
            get { return _Round; }
            set { SetChange("Round", value); }
        }

        private bool _Win;
        /// <summary>
        /// 轮次
        /// </summary>
        [EntityField("Win")]
        [ProtoMember(3)]
        public bool Win
        {
            get { return _Win; }
            set { SetChange("Win", value); }
        }
        private int _FastID;

        [ProtoMember(4)]
        [EntityField("FastID")]
        public int FastID
        {
            get { return _FastID; }
            set { SetChange("FastID", value); }
        }

        private CombatProcessContainer _CombatProcessContainer;
        /// <summary>
        /// 战斗详情
        /// </summary>
        [ProtoMember(5)]
        [EntityField("CombatProcessContainer", IsJsonSerialize = true)]
        public CombatProcessContainer CombatProcessContainer
        {
            get { return _CombatProcessContainer; }
            set { SetChange("CombatProcessContainer", value); }
        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "AgainstID": return AgainstID;
                    case "Round": return Round;
                    case "Win": return Win;
                    case "FastID": return FastID;
                    case "CombatProcessContainer": return CombatProcessContainer;
                    default: throw new ArgumentException(string.Format("ServerCombatDetail index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "AgainstID":
                        this._AgainstID = value.ToNotNullString();
                        break;
                    case "Round":
                        this._Round = value.ToInt();
                        break;
                    case "Win":
                        this._Win = value.ToBool();
                        break;
                    case "FastID":
                        this._FastID = value.ToInt();
                        break;
                    case "CombatProcessContainer":
                        this._CombatProcessContainer = value as CombatProcessContainer;
                        break;
                    default: throw new ArgumentException(string.Format("ServerCombatDetail index[{0}] isn't exist.", index));
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