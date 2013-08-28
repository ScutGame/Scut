using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Pay
{
    /// <summary>
    /// 反馈信息
    /// </summary>
    public class FeedbackInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int ReplyID;

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int GameID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ServerID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReplyContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ReplyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}
