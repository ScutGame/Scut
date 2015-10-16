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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Context
{
    /// <summary>
    /// 游戏角色
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class BaseUser : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected BaseUser()
        {
        }
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="access">Access.</param>
        protected BaseUser(AccessLevel access)
            : base(access)
        {
        }

        private ContextCacheSet<CacheItem> _userData;
        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <value>The user data.</value>
        public ContextCacheSet<CacheItem> UserData
        {
            get
            {
                if (_userData == null)
                {
                    _userData = new ContextCacheSet<CacheItem>(string.Format("__gamecontext_{0}", GetUserId()));
                }
                return _userData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsLock { get; }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("no use")]
        public virtual DateTime OnlineDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract int GetUserId();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetNickName();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetPassportId();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetRetailId();
    }
}