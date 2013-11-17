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
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Common;
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
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Context.BaseUser"/> class.
		/// </summary>
        protected BaseUser()
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Context.BaseUser"/> class.
		/// </summary>
		/// <param name="access">Access.</param>
        protected BaseUser(AccessLevel access)
            : base(access)
		{
		    SocketSid = string.Empty;
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
        /// 在线间隔小时数
        /// </summary>
        protected int OnlineHourInterval = 8;
		/// <summary>
		/// Gets a value indicating whether this instance is feng jin status.
		/// </summary>
		/// <value><c>true</c> if this instance is feng jin status; otherwise, <c>false</c>.</value>
        public abstract bool IsFengJinStatus { get; }
		/// <summary>
		/// Gets or sets the online date.
		/// </summary>
		/// <value>The online date.</value>
        public abstract DateTime OnlineDate { get; set; }
		/// <summary>
		/// Gets a value indicating whether this instance is inlining.
		/// </summary>
		/// <value><c>true</c> if this instance is inlining; otherwise, <c>false</c>.</value>
        public bool IsInlining
        {
            get
            {
                return MathUtils.DiffDate(OnlineDate).TotalHours > OnlineHourInterval;
            }
        }


        /// <summary>
        /// 远端SessionId
        /// </summary>
        [JsonIgnore]
        public string SocketSid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string RemoteAddress { get; set; }

        /// <summary>
        /// 360渠道Token
        /// </summary>
        [JsonIgnore]
        public string Token360 { get; set; }

        //public abstract int GameId { get; set; }

        //public abstract int ServerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetSessionId();
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
        /// <summary>
        /// 
        /// </summary>
        public virtual void RefleshOnlineDate()
        {
            OnlineDate = DateTime.Now;
        }
    }
}