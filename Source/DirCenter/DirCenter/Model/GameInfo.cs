using System;
using ZyGames.Common;

namespace ZyGames.DirCenter.Model
{
    public class GameInfo
    {
        public GameInfo()
        {
            ReleaseDate = ConvertHelper.SqlMinDate;
            Currency = "";
            GameWord = "";
            AgentsID = "";
            PayStyle = "";
            SocketServer = "";
        }

        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 代币名称（晶石，元宝）
        /// </summary>
        public string Currency
        {
            get;
            set;
        }

        /// <summary>
        /// 倍率，1元=n代币
        /// </summary>
        public decimal Multiple
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏拼音缩写
        /// </summary>
        public string GameWord
        {
            get;
            set;
        }

        /// <summary>
        /// 代理商编号官网：0000;触控：2001
        /// </summary>
        public string AgentsID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否正式发布
        /// </summary>
        public bool IsRelease
        {
            get;
            set;
        }

        /// <summary>
        /// 正式发布时间（年月日时分）
        /// </summary>
        public DateTime ReleaseDate
        {
            get;
            set;
        }

        /// <summary>
        /// 充值页面样式
        /// </summary>
        public string PayStyle
        {
            get;
            set;
        }

        /// <summary>
        /// SOCKET游戏解析域名
        /// </summary>
        public string SocketServer
        {
            get;
            set;
        }

        /// <summary>
        /// SOCKET端口
        /// </summary>
        public int SocketPort
        {
            get;
            set;
        }

    }
}
