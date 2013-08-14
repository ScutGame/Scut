using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 媒体礼包新手卡
    /// </summary>
    public abstract class GiftNoviceCard : BaseEntity
    {
        public abstract string CardNo
        {
            get;
            set;
        }

        public abstract string GiftType
        {
            get;
            set;
        }

        public abstract int UserId
        {
            get;
            set;
        }

        public abstract DateTime ActivateDate
        {
            get;
            set;
        }

        public abstract bool IsInvalid
        {
            get;
            set;
        }

        public abstract string CreateIp
        {
            get;
            set;
        }

        public abstract DateTime CreateDate
        {
            get;
            set;
        }
    }
}
