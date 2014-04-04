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
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model.ConfigModel
{
    /// <summary>
    /// 招募规则信息表
    /// </summary>
    [Serializable, ProtoContract, EntityTable(AccessLevel.ReadOnly, DbConfig.Config, "RecruitRule")]
    public class RecruitRule : ShareEntity
    {
        public RecruitRule()
            : base(AccessLevel.ReadOnly)
        {
            GeneralQuality = new CacheList<RecruitInfo>(0, true);
        }

        #region auto-generated Property
        private Int32 _RecruitType;

        /// <summary>
        /// 参照RecruitType枚举
        /// </summary>
        [EntityField("RecruitType", IsKey = true)]
        public Int32 RecruitType
        {
            get
            {
                return _RecruitType;
            }
            private set
            {
                SetChange("RecruitType", value);
            }
        }

        private CacheList<RecruitInfo> _GeneralQuality;

        /// <summary>
        ///参照GeneralQuality枚举
        /// </summary>
        [EntityField("GeneralQuality", IsJsonSerialize = true)]
        public CacheList<RecruitInfo> GeneralQuality
        {
            get
            {
                return _GeneralQuality;
            }
            private set
            {
                SetChange("GeneralQuality", value);
            }
        }

        private Int32 _CodeTime;

        /// <summary>
        /// 冷却时间 秒
        /// </summary>
        [EntityField("CodeTime")]
        public Int32 CodeTime
        {
            get
            {
                return _CodeTime;
            }
            private set
            {
                SetChange("CodeTime", value);
            }
        }

        private Int32 _GoldNum;

        /// <summary>
        /// 消除所需晶石
        /// </summary>
        [EntityField("GoldNum")]
        public Int32 GoldNum
        {
            get
            {
                return _GoldNum;
            }
            private set
            {
                SetChange("GoldNum", value);
            }
        }


        private Int32 _FreeNum;

        /// <summary>
        /// 免费次数
        /// </summary>
        [EntityField("FreeNum")]
        public Int32 FreeNum
        {
            get
            {
                return _FreeNum;
            }
            private set
            {
                SetChange("FreeNum", value);
            }
        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "RecruitType": return RecruitType;
                    case "GeneralQuality": return GeneralQuality;
                    case "CodeTime": return CodeTime;
                    case "GoldNum": return GoldNum;
                    case "FreeNum": return FreeNum;
                    default: throw new ArgumentException(string.Format("RecruitRule index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "RecruitType":
                        _RecruitType = value.ToInt();
                        break;
                    case "GeneralQuality":
                        _GeneralQuality = ConvertCustomField<CacheList<RecruitInfo>>(value, index);
                        break;
                    case "CodeTime":
                        _CodeTime = value.ToInt();
                        break;
                    case "GoldNum":
                        _GoldNum = value.ToInt();
                        break;
                    case "FreeNum":
                        _FreeNum = value.ToInt();
                        break;
                    default: throw new ArgumentException(string.Format("RecruitRule index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }
        #endregion

        protected override int GetIdentityId()
        {
            return DefIdentityId;
        }

        public override int CompareTo(ShareEntity other)
        {
            return ((int)this.RecruitType).CompareTo((int)((RecruitRule)other).RecruitType);
        }

    }
}