using System;
using ZyGames.Common;

namespace ZyGames.DirCenter.Model
{
    public class ServerInfo : IComparable<ServerInfo>
    {
        public ServerInfo()
        {
            EnableDate = ConvertHelper.SqlMinDate;
        }

        public int ID
        {
            get;
            set;
        }

        public int GameID
        {
            get;
            set;
        }

        public string ServerName
        {
            get;
            set;
        }

        public string ServerUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 内网地址
        /// </summary>
        public string IntranetAddress
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// 活跃数
        /// </summary>
        public int ActiveNum
        {
            get;
            set;
        }

        public int Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 指向服务器
        /// </summary>
        public int TargetServer
        {
            get;
            set;
        }

        /// <summary>
        /// 是否正式开服
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }

        /// <summary>
        /// 正式开服时间
        /// </summary>
        public DateTime EnableDate
        {
            get;
            set;
        }

        #region IComparable<ServerInfo> 成员

        /// <summary>
        /// 排序规则：权重高到低，活跃数低到高排序
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ServerInfo other)
        {
            int result = other.Weight.CompareTo(Weight);
            if (result == 0)
            {
                //升序
                result = this.ActiveNum.CompareTo(other.ActiveNum);
                if (result == 0)
                {
                    result = other.ID.CompareTo(this.ID);
                }
            }
            return result;
        }

        #endregion
    }
}
