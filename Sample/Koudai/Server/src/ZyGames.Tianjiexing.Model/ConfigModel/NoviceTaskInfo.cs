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
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.Model.ConfigModel
{
    /// <summary>
    /// @periodTime:设置生存周期(秒)
    /// @personalName: 映射UserId对应的字段名,默认为"UserId"
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, DbConfig.Config, "NoviceTaskInfo")]
    public class NoviceTaskInfo : GuideData
    {

        /// <summary>
        /// </summary>
        public NoviceTaskInfo()
            : base()
        {
            _PrizeList = new CacheDictionary<int, CacheList<int>>();
        }
        /// <summary>
        /// </summary>
        public NoviceTaskInfo(Int32 ID)
            : base(ID)
        {
            this._ID = ID;
        }

        #region 自动生成属性

        private Int32 _ID;
        /// <summary>
        /// </summary>        
        [ProtoMember(1)]
        [EntityField("ID", IsKey = true)]
        public override Int32 ID
        {
            get
            {
                return _ID;
            }

        }

        private String _Name;
        /// <summary>
        /// </summary>        
        [ProtoMember(2)]
        [EntityField("Name")]
        public override String Name
        {
            get
            {
                return _Name;
            }

        }

        private String _Description;
        /// <summary>
        /// </summary>        
        [ProtoMember(3)]
        [EntityField("Description")]
        public override String Description
        {
            get
            {
                return _Description;
            }

        }

        private Int32 _NextID;
        /// <summary>
        /// </summary>        
        [ProtoMember(4)]
        [EntityField("NextID")]
        public override Int32 NextID
        {
            get
            {
                return _NextID;
            }

        }

        private CacheDictionary<int, CacheList<int>> _PrizeList;
        /// <summary>
        /// </summary>        
        [ProtoMember(5)]
        [EntityField("PrizeList", IsJsonSerialize = true, DbType = ColumnDbType.Text)]
        public CacheDictionary<int, CacheList<int>> PrizeList
        {
            get
            {
                return _PrizeList;
            }
        }

        private Int32 _Type;
        /// <summary>
        /// </summary>        
        [ProtoMember(6)]
        [EntityField("Type")]
        public override Int32 Type
        {
            get
            {
                return _Type;
            }

        }

        private Int32 _SubType;
        /// <summary>
        /// </summary>        
        [ProtoMember(7)]
        [EntityField("SubType")]
        public override Int32 SubType
        {
            get
            {
                return _SubType;
            }

        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "ID": return ID;
                    case "Name": return Name;
                    case "Description": return Description;
                    case "NextID": return NextID;
                    case "PrizeList": return PrizeList;
                    case "Type": return Type;
                    case "SubType": return SubType;
                    default: throw new ArgumentException(string.Format("NoviceTaskInfo index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "ID":
                        _ID = value.ToInt();
                        break;
                    case "Name":
                        _Name = value.ToNotNullString();
                        break;
                    case "Description":
                        _Description = value.ToNotNullString();
                        break;
                    case "NextID":
                        _NextID = value.ToInt();
                        break;
                    case "PrizeList":
                        _PrizeList = ConvertCustomField<CacheDictionary<int, CacheList<int>>>(value, index);
                        break;
                    case "Type":
                        _Type = value.ToInt();
                        break;
                    case "SubType":
                        _SubType = value.ToInt();
                        break;
                    default: throw new ArgumentException(string.Format("NoviceTaskInfo index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion


        public override string Prize
        {
            get { return string.Empty; }
        }
    }
}