using System.Configuration;
using ZyGames.Core.Extensions;

namespace ZyGames.MSMQ.Model
{
    /// <summary>
    /// 消息管理器的消息配置对象
    /// </summary>
    public class MessageConfig : ConfigurationSection
    {
        public bool IsErrorQueue { get; set; }

        /// <summary>
        /// 消息管理器中文名称
        /// <remarks>如：花粉消息管理器，牧场消息管理器，君临天下消息管理器</remarks>
        /// </summary>
        [ConfigurationProperty("managerName", IsRequired = true)]
        public string ManagerName
        {
            get
            {
                return (string)base["managerName"];
            }
            set
            {
                base["managerName"] = value;
            }
        }

        /// <summary>
        /// 消息管理启用多线程总数
        /// <remarks>默认开启5个线程</remarks>
        /// </summary>
        [ConfigurationProperty("managerThreadNumber", IsRequired = true)]
        public int ManagerThreadNumber
        {
            get
            {
                return base["managerThreadNumber"].ToString().ToInt32(5);
            }
            set
            {
                base["managerThreadNumber"] = value;
            }
        }

        /// <summary>
        /// 消息管理器处理消息的超时时间（以秒为单位）
        /// </summary>
        [ConfigurationProperty("managerThreadTimeOut", IsRequired = true)]
        public int ManagerThreadTimeOut
        {
            get
            {
                return base["managerThreadTimeOut"].ToString().ToInt32(5);
            }
            set
            {
                base["managerThreadTimeOut"] = value;
            }
        }

        /// <summary>
        /// 管理的消息队列存放路径
        /// </summary>
        [ConfigurationProperty("managerMessagePath", IsRequired = true)]
        public string ManagerMessagePath
        {
            get
            {
                return (string)base["managerMessagePath"];
            }
            set
            {
                base["managerMessagePath"] = value;
            }
        }

        /// <summary>
        /// 管理的消息队列出错存放路径
        /// </summary>
        [ConfigurationProperty("errorPath", IsRequired = true)]
        public string ManagerErrorPath
        {
            get
            {
                return (string)base["errorPath"];
            }
            set
            {
                base["errorPath"] = value;
            }
        }
    }
}
