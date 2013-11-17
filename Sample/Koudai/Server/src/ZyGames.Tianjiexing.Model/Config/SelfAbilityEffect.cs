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
using System.Runtime.Serialization;
using ProtoBuf;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Event;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 战斗日志
    /// </summary>
    [Serializable, ProtoContract]
    public class SelfAbilityEffect
    {
        /// <summary>
        /// 佣兵ID
        /// </summary>
        public Int32 GeneralID { get; set; }
        /// <summary>
        /// 增减属性True:增false:增
        /// </summary>
        [ProtoMember(1)]
        public Boolean IsIncrease
        {
            get;
            set;
        }
        /// <summary>
        /// 文字光效图
        /// </summary>
        [ProtoMember(2)]
        public String FntHeadID
        {
            get;
            set;
        }
        /// <summary>
        /// 光效图片
        /// </summary>
        [ProtoMember(3)]
        public String EffectID1
        {
            get;
            set;
        }
        /// <summary>
        /// 0：攻击，1：防御	
        /// </summary>
        [ProtoMember(4)]
        public Int16 Role { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        [ProtoMember(5)]
        public Int32 Position { get; set; }
    }
}