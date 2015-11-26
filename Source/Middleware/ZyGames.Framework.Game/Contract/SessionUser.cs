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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// session's user
    /// </summary>
    [ProtoContract]
    public class SessionUser : IUser
    {
        /// <summary>
        /// 
        /// </summary>
        public SessionUser()
        {
            OnlineInterval = new TimeSpan(0, 1, 0);
            RefleshOnlineDate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleUser"></param>
        public SessionUser(BaseUser roleUser)
            : this()
        {
            PassportId = roleUser.GetPassportId();
            UserId = roleUser.GetUserId();
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(2)]
        public string PassportId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(3)]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOnlining { get { return MathUtils.DiffDate(OnlineDate) < OnlineInterval; } }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(4)]
        public DateTime OnlineDate { get; set; }


        /// <summary>
        /// 在线间隔
        /// </summary>
        [ProtoMember(5)]
        public TimeSpan OnlineInterval { get; set; }

        /// <summary>
        /// 被替换掉
        /// </summary>
        [ProtoMember(6)]
        public bool IsReplaced { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetUserId()
        {
            return UserId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetPassportId()
        {
            return PassportId;
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefleshOnlineDate()
        {
            OnlineDate = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetExpired(DateTime time)
        {
            OnlineDate = time;
        }
    }
}
