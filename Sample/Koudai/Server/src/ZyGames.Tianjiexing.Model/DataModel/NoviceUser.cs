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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Model;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.DataModel
{
    /// <summary>
    /// @periodTime:设置生存周期(秒)
    /// @personalName: 映射UserId对应的字段名,默认为"UserId"
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(DbConfig.Data, "NoviceUser", DbConfig.PeriodTime, DbConfig.PersonalName)]
    public class NoviceUser : UserGuide
    {

        /// <summary>
        /// </summary>
        public NoviceUser() 
            : base()
        {
            GuideProgress = new CacheList<GuideProgressItem>();
        }
        /// <summary>
        /// </summary>
        public NoviceUser(Int32 UserId)
            : this()
        {
            _UserId = UserId;
        }

        #region 自动生成属性

        private Int32 _UserId;
        /// <summary>
        /// </summary>        
        [ProtoMember(1)]
        [EntityField("UserId", IsKey = true)]
        public override Int32 UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                SetChange("UserId", value);
            }

        }

        private Boolean _IsClose;
        /// <summary>
        /// </summary>        
        [ProtoMember(2)]
        [EntityField("IsClose")]
        public override Boolean IsClose
        {
            get
            {
                return _IsClose;
            }
            set
            {
                SetChange("IsClose", value);
            }
        }

        private Int32 _CurrGuideId;
        /// <summary>
        /// </summary>        
        [ProtoMember(3)]
        [EntityField("CurrGuideId")]
        public override Int32 CurrGuideId
        {
            get
            {
                return _CurrGuideId;
            }
            set
            {
                SetChange("CurrGuideId", value);
            }
        }

        private DateTime _CloseDate;
        /// <summary>
        /// </summary>        
        [ProtoMember(4)]
        [EntityField("CloseDate")]
        public override DateTime CloseDate
        {
            get
            {
                return _CloseDate;
            }
            set
            {
                SetChange("CloseDate", value);
            }
        }

        private CacheList<GuideProgressItem> _GuideProgress;
        /// <summary>
        /// </summary>        
        [ProtoMember(5)]
        [EntityField("GuideProgress", IsJsonSerialize = true, DbType = ColumnDbType.Text)]
        public override CacheList<GuideProgressItem> GuideProgress
        {
            get
            {
                return _GuideProgress;
            }
            set
            {
                SetChange("GuideProgress", value);
            }
        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "UserId": return UserId;
                    case "IsClose": return IsClose;
                    case "CurrGuideId": return CurrGuideId;
                    case "CloseDate": return CloseDate;
                    case "GuideProgress": return GuideProgress;
                    default: throw new ArgumentException(string.Format("NoviceUser index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "UserId":
                        _UserId = value.ToInt();
                        break;
                    case "IsClose":
                        _IsClose = value.ToBool();
                        break;
                    case "CurrGuideId":
                        _CurrGuideId = value.ToInt();
                        break;
                    case "CloseDate":
                        _CloseDate = value.ToDateTime();
                        break;
                    case "GuideProgress":
                        _GuideProgress = ConvertCustomField<CacheList<GuideProgressItem>>(value, index);
                        break;
                    default: throw new ArgumentException(string.Format("NoviceUser index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion

        protected override int GetIdentityId()
        {
            //设置玩家的UserID
            //若要做为全局使用设置类绑定的自定义属性[EntityTable(CacheType.Entity, DbConfig.Config, @TableName, @PeriodTime)]
            return UserId;
        }
    }
}