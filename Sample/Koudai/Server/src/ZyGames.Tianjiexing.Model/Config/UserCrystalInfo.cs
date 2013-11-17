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
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 命运水晶表
    /// </summary>
    [Serializable, ProtoContract]
    public class UserCrystalInfo : JsonEntity, IComparable<UserCrystalInfo>
    {
        [ProtoMember(1)]
        public string UserCrystalID
        {
            get;
            set;
        }

        /// <summary>
        /// 佣兵ID
        /// </summary>
        [ProtoMember(2)]
        public int GeneralID
        {
            get;
            set;
        }

        /// <summary>
        /// 水晶ID
        /// </summary>
        [ProtoMember(3)]
        public Int32 CrystalID
        {
            get;
            set;
        }

        public CrystalQualityType CrystalQuality
        {
            get
            {
                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(CrystalID);
                if (crystalInfo != null)
                {
                    return crystalInfo.CrystalQuality;
                }
                return CrystalQualityType.Gray;
            }
        }

        public AbilityType AbilityType
        {
            get
            {
                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(CrystalID);
                if (crystalInfo != null)
                {
                    return crystalInfo.AbilityID;
                }
                return AbilityType.ShengMing;
            }
        }

        /// <summary>
        /// 水晶等级
        /// </summary>
        [ProtoMember(4)]
        public Int16 CrystalLv
        {
            get;
            set;
        }

        /// <summary>
        /// 当前经验
        /// </summary>
        [ProtoMember(5)]
        public int CurrExprience
        {
            get;
            set;
        }

        /// <summary>
        /// 位置
        /// </summary>
        [ProtoMember(6)]
        public Int16 Position
        {
            get;
            set;
        }

        /// <summary>
        /// 是否卖出 1:可以卖出 2：不可卖出
        /// </summary>
        [ProtoMember(7)]
        public Int16 IsSale
        {
            get;
            set;
        }

        /// <summary>
        /// 获得时间
        /// </summary>
        [ProtoMember(8)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 是否可拾取  1：可拾取 2：不可拾取
        /// </summary>
        public int IsTelegrams
        {
            get
            {
                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(CrystalID);
                if (crystalInfo != null)
                {
                    return crystalInfo.IsTelegrams;
                }
                return 2;
            }
        }

        /// <summary>
        /// 命运水晶是否已满 已满false;未满true;
        /// </summary>
        public static bool CheckFull(string userID, int num)
        {
            GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(userID);
            var package = UserCrystalPackage.Get(userID);
            UserCrystalInfo[] crystalsArray =
                package.CrystalPackage.FindAll(m => m.IsSale == 2 && m.GeneralID.Equals(0)).ToArray();
            //原因：当num大于0且crystalsArray.Length + num = userInfo.CrystalNum是提示背包已满
            if (num > 0)
            {
                if (crystalsArray.Length + num > userInfo.CrystalNum) return false;
            }
            else
            {
                if (crystalsArray.Length >= userInfo.CrystalNum) return false;
            }
            return true;
        }

        public int CompareTo(UserCrystalInfo item)
        {
            int result = 0;
            if (this == null && item == null) return 0;
            if (this != null && item == null) return 1;
            if (this == null && item != null) return -1;

            result = ((int)item.CrystalQuality).CompareTo((int)CrystalQuality);
            if (result == 0)
            {
                result = item.CrystalLv.CompareTo(item.CrystalLv);
                if (result == 0)
                {
                    result = item.CurrExprience.CompareTo(item.CurrExprience);
                }
            }

            return result;
        }
    }
}