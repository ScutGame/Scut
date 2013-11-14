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
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 新手引导配置
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class GuideData : ShareEntity
    {
        /// <summary>
        /// </summary>
        protected GuideData()
            : base(AccessLevel.ReadOnly)
        {
        }
        /// <summary>
        /// </summary>
        protected GuideData(Int32 id)
            : this()
        {
            this._ID = id;
        }

        private Int32 _ID;

        /// <summary>
        /// 引导配置ID
        /// </summary>       
        [ProtoMember(1)]
        public abstract Int32 ID { get; }

        /// <summary>
        /// 引导类型
        /// </summary>        
        public abstract Int32 Type { get; }

        /// <summary>
        /// 引导子类型
        /// </summary>        
        public abstract Int32 SubType { get; }

        /// <summary>
        /// 下一引导
        /// </summary>
        public abstract Int32 NextID { get; }

        /// <summary>
        /// 引导名称
        /// </summary>     
        public abstract String Name { get; }

        /// <summary>
        /// 引导描述
        /// </summary>       
        public abstract String Description { get; }

        /// <summary>
        /// 完成获取奖励
        /// </summary>     
        public abstract String Prize { get; }


    }
}