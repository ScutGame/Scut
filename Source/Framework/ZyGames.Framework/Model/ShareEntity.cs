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

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// 共享实体数据基类
    /// </summary>
    [ProtoContract, Serializable]
    public abstract class ShareEntity : AbstractEntity, IComparable<ShareEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        protected ShareEntity()
            : base(false)
        {

        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Model.ShareEntity"/> class.
		/// </summary>
        /// <param name="isReadonly">If set to <c>true</c> is readonly. no used</param>
        protected ShareEntity(bool isReadonly)
            : base(isReadonly)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="access">no used</param>
        protected ShareEntity(AccessLevel access)
            : base(access)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected internal override int GetIdentityId()
        {
            return DefIdentityId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual int CompareTo(ShareEntity other)
        {
            return base.CompareTo(other);
        }
    }
}