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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// session's user
    /// </summary>
    public class SessionUser : IUser
    {
        /// <summary>
        /// 
        /// </summary>
        public SessionUser()
        {
            OnlineInterval = new TimeSpan(0, 1, 0);
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
        public string PassportId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOnlining { get { return MathUtils.DiffDate(OnlineDate) < OnlineInterval; } }

        /// <summary>
        /// 
        /// </summary>
        public DateTime OnlineDate { get; set; }


        /// <summary>
        /// 在线间隔
        /// </summary>
        public TimeSpan OnlineInterval { get; set; }

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
    }
}
