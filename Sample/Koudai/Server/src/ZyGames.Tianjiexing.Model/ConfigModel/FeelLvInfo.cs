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
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model
{
    /// <summary>
    /// 礼物配置表
    /// </summary>
    [Serializable, ProtoContract, EntityTable(AccessLevel.ReadOnly, DbConfig.Config, "FeelLvInfo")]
    public class FeelLvInfo : ShareEntity
    {

        public FeelLvInfo()
            : base(AccessLevel.ReadOnly)
        {
            Property = new CacheList<GeneralProperty>(0, true);
        }

        //protected override void BindChangeEvent()
        //{
        //    Property.BindParentChangeEvent(this);
        //}

        #region auto-generated Property

        private Int16 _FeelLv;
        /// <summary>
        /// 礼物ID
        /// </summary>
        [EntityField("FeelLv", IsKey = true)]
        public Int16 FeelLv
        {
            get
            {
                return _FeelLv;
            }
            private set
            {
                SetChange("FeelLv", value);
            }
        }

        private int _Experience;
        /// <summary>
        /// 礼物名称
        /// </summary>
        [EntityField("Experience")]
        public int Experience
        {
            get
            {
                return _Experience;
            }
            private set
            {
                SetChange("Experience", value);
            }
        }
        private CacheList<GeneralProperty> _Property;
        /// <summary>
        /// 礼物品质
        /// </summary>
        [EntityField("Property", IsJsonSerialize = true)]
        public CacheList<GeneralProperty> Property
        {
            get
            {
                return _Property;
            }
            private set
            {
                SetChange("Property", value);
            }
        }


        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "FeelLv": return FeelLv;
                    case "Experience": return Experience;
                    case "Property": return Property;
                    default: throw new ArgumentException(string.Format("FeelLvInfo index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "FeelLv":
                        _FeelLv = value.ToShort();
                        break;
                    case "Experience":
                        _Experience = value.ToInt();
                        break;
                    case "Property":
                        _Property =ConvertCustomField<CacheList<GeneralProperty>>(value, index); 
                        break;
                    default: throw new ArgumentException(string.Format("FeelLvInfo index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        protected override int GetIdentityId()
        {
            return DefIdentityId;
        }
        #endregion
    }
}