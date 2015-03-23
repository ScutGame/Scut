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
        /// <param name="roleUser"></param>
        public SessionUser(BaseUser roleUser)
        {
            OnlineInterval = new TimeSpan(0, 1, 0);
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
        public bool IsOnlining { get { return MathUtils.DiffDate(OnlineDate) > OnlineInterval; } }

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
