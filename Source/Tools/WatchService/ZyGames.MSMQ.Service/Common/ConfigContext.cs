using System.Configuration;
using ZyGames.Core.Util;
using ZyGames.Core.Extensions;
using ZyGames.MSMQ.Model;

namespace ZyGames.MSMQ.Service.Common
{
    public class ConfigContext
    {
        private static ConfigContext _context = null;
        private static object obj = new object();

        /// <summary>
        /// 系统消息配置项信息
        /// </summary>
        public MessageConfig SysMessageConfig { get; set; }

        /// <summary>
        /// CIT产品数据库连接串
        /// </summary>
        public string ConnctionString { get; set; }

        /// <summary>
        /// 重新执行的机会数
        /// </summary>
        public int ReDoExecuteCount { get; set; }


        #region 私有构造器
        /// <summary>
        /// 私有构造器
        /// </summary>
        private ConfigContext()
        {
            try
            {
                this.Init();
            }
            catch 
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 自定义的相关配置项信息初始化
        /// </summary>
        private void Init()
        {
            try
            {
                SysMessageConfig = (MessageConfig)ConfigurationManager.GetSection("messageConfig");
                ConnctionString = ConfigHelper.GetSetting("messageConnection");
                ReDoExecuteCount = ConfigHelper.GetSetting("messageReDoExecuteCount").ToInt32(1);
            }
            catch 
            {
                throw;
            }
        }

        #region 返回唯一配置实例
        /// <summary>
        /// 返回唯一配置实例
        /// </summary>
        /// <returns></returns>
        public static ConfigContext GetInstance()
        {
            try
            {
                if (_context == null)
                {
                    lock (obj)
                    {
                        if (_context == null)
                        {
                            _context = new ConfigContext();
                        }
                    }
                }
            }
            catch 
            {
                throw;
            }
            return _context;
        }
        #endregion


    }
}
