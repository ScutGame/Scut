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
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model
{
    [Serializable, ProtoContract, EntityTable(AccessLevel.ReadOnly, DbConfig.Config, "ServerCombatPrize")]
    public class ServerCombatPrize : ShareEntity
    {
        public ServerCombatPrize()
            : base(AccessLevel.ReadOnly)
        {
        }

        public int _CombatType;
        /// <summary>
        /// 报名类型
        /// </summary>
        [EntityField("CombatType", IsKey = true)]
        public int CombatType
        {
            get { return _CombatType; }
            private set { SetChange("CombatType", value); }
        }

        public int _Stage;
        /// <summary>
        /// 阶段
        /// </summary>
        [EntityField("Stage", IsKey = true)]
        public int Stage
        {
            get { return _Stage; }
            private set { SetChange("Stage", value); }
        }

        public int _GameCoins;
        /// <summary>
        /// 金币
        /// </summary>
        [EntityField("GameCoins")]
        public int GameCoins
        {
            get { return _GameCoins; }
            private set { SetChange("GameCoins", value); }
        }

        public int _ObtainNum;
        /// <summary>
        /// 声望
        /// </summary>
        [EntityField("ObtainNum")]
        public int ObtainNum
        {
            get { return _ObtainNum; }
            private set { SetChange("ObtainNum", value); }
        }

        private CacheList<PrizeInfo> _Reward;
        /// <summary>
        /// 
        /// </summary>
        [EntityField("Reward", IsJsonSerialize = true)]
        public CacheList<PrizeInfo> Reward
        {
            get
            {
                return _Reward;
            }
            set
            {
                SetChange("Reward", value);
            }
        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "CombatType": return CombatType;
                    case "Stage": return Stage;
                    case "GameCoins": return GameCoins;
                    case "ObtainNum": return ObtainNum;
                    case "Reward": return Reward;
                    default: throw new ArgumentException(string.Format("ServerCombatPrize index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "CombatType":
                        this._CombatType = value.ToInt();
                        break;
                    case "Stage":
                        this._Stage = value.ToInt();
                        break;
                    case "GameCoins":
                        this._GameCoins = value.ToInt();
                        break;
                    case "ObtainNum":
                        this._ObtainNum = value.ToInt();
                        break;
                    case "Reward":
                        this._Reward = value as CacheList<PrizeInfo>;
                        break;
                    default: throw new ArgumentException(string.Format("ServerCombatPrize index[{0}] isn't exist.", index));
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