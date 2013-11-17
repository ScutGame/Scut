using System;
using ZyGames.GamesReportService;

namespace ZyGames.OA.BLL.Remote.Ddz
{
    public class DbConfig : BaseDbConfig
    {
        private const string ckrptServerKey = "ckddz_rpt_dbserver";
        private const string rptServerKey = "ddz_rpt_dbserver";
        private const string ck_Name = "Ck";
        private const string dataName = "Ddz{0}Data";
        private const string logName = "Ddz{0}Log";
        private const string configName = "DdzConfig";

        public static BaseDbConfig GetPay()
        {
            return new DbConfig(rptServerKey, PayDbName, 0);
        }

        public static BaseDbConfig GetConfig(int gameId)
        {
            string dbName = GetDbName(gameId, 0, ConnctType.Config);

            if (!string.IsNullOrEmpty(dbName))
            {
                return new DbConfig(gameId, 0, dbName);
            }
            throw new Exception(string.Format("读取游戏{0}数据配置出错！", gameId));
        }

        public static BaseDbConfig GetData(int gameId, int serverId)
        {
            string dbName = GetDbName(gameId, serverId, ConnctType.Data);

            if (!string.IsNullOrEmpty(dbName))
            {
                return new DbConfig(gameId, serverId, dbName);
            }
            throw new Exception(string.Format("读取游戏{0}数据配置出错！", gameId));
        }

        public static BaseDbConfig GetLog(int gameId, int serverId)
        {
            string dbName = GetDbName(gameId, serverId, ConnctType.Log);

            if (!string.IsNullOrEmpty(dbName))
            {
                return new DbConfig(gameId, serverId, dbName);
            }
            throw new Exception(string.Format("读取游戏{0}数据配置出错！", gameId));
        }

        public DbConfig(int gameId, int serverId, string dbName)
            : base(gameId, serverId, dbName)
        {
        }

        public DbConfig(string rptServerKey, string dbName, int serverId)
            : base(rptServerKey, dbName, serverId)
        {
        }

    }
}
