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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Event;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 玩家佣兵魂技
    /// </summary>
    [Serializable, ProtoContract]
    public class Ability : EntityChangeEvent, IComparable<Ability>
    {
        public Ability()
            : base(false)
        {
        }

        private Int32 _AbilityID;

        /// <summary>
        /// 魂技ID
        /// </summary>
        [ProtoMember(1)]
        public Int32 AbilityID
        {
            get { return _AbilityID; }
            set
            {
                _AbilityID = value;
                NotifyByModify();
            }
        }

        private Int32 _GeneralID;
        /// <summary>
        /// 创建时间
        /// </summary>
        [ProtoMember(2)]
        public Int32 GeneralID
        {
            get { return _GeneralID; }
            set { _GeneralID = value; NotifyByModify(); }
        }

        private int _AbilityLv;

        /// <summary>
        /// 魂技等级
        /// </summary>
        [ProtoMember(3)]
        public Int32 AbilityLv
        {
            get { return _AbilityLv; }
            set { _AbilityLv = value; NotifyByModify(); }
        }
        private int _experienceNum;
        /// <summary>
        /// 当前经验
        /// </summary>
        [ProtoMember(4)]
        public Int32 ExperienceNum
        {
            get { return _experienceNum; }
            set { _experienceNum = value; NotifyByModify(); }
        }

        private String _UserItemID;

        /// <summary>
        /// 唯一值
        /// </summary>
        [ProtoMember(5)]
        public String UserItemID
        {
            get { return _UserItemID; }
            set
            {
                _UserItemID = value;
                NotifyByModify();
            }
        }

        private Int32 _goupxperienceNum;
        /// <summary>
        /// 升级后经验
        /// </summary>
        [ProtoMember(6)]
        public Int32 GoupExperienceNum
        {
            get { return _goupxperienceNum; }
            set { _goupxperienceNum = value; NotifyByModify(); }
        }
        private Int32 _Position;
        /// <summary>
        /// 位置
        /// </summary>
        [ProtoMember(7)]
        public Int32 Position
        {
            get { return _Position; }
            set { _Position = value; NotifyByModify(); }
        }

        public Int32 AbilityQuality
        {
            get
            {
                AbilityInfo ability = null;
                ability = new ShareCacheStruct<AbilityInfo>().FindKey(AbilityID);

                if (ability != null)
                {
                    return ability.AbilityQuality;
                }
                return 1;
            }
        }

        #region IComparable<Ability> 成员

        public int CompareTo(Ability ability)
        {
            int result = 0;
            //  TODO:
            return result;
        }

        #endregion
    }
}