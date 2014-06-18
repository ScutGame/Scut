using System.Configuration;

namespace ZyGames.Doudizhu.Model
{
    public class DbConfig
    {
        public const string Config = "DoudizhuConfig";
        public const string Data = "DoudizhuData";
        public const string Log = "DoudizhuLog";
        public const int GlobalPeriodTime = 0;
        public const int PeriodTime = 0;
        public const string PersonalName = "UserId";

        public static string ConfigConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[Config].ConnectionString;
            }
        }

        public static string DataConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[Data].ConnectionString;
            }
        }

        public static string LogConnectString
        {
            get
            {   
                return ConfigurationManager.ConnectionStrings[Log].ConnectionString;
            }
        }

    }
}
