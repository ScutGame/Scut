using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Pay
{
    internal class ConfigManger
    {
        private static string _connectionString = string.Empty;
        
        public static string connectionString
        {
            get
            {

                if (_connectionString == string.Empty)
                {
                    string cString = ConfigUtils.GetSetting("PayDB_ConnectionString");
                    string dataSource = string.Empty;
                    string userInfo = string.Empty;
                    try
                    {
                        dataSource = ConfigUtils.GetSetting("PayDB_Server");
                        userInfo = ConfigUtils.GetSetting("PayDB_Acount");
                        if (!string.IsNullOrEmpty(userInfo))
                        {
                            userInfo = CryptoHelper.DES_Decrypt(userInfo, GameEnvironment.ProductDesEnKey);
                        }
                    }
                    catch
                    {
                    }
                    if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(userInfo))
                    {
                        _connectionString = string.Format(cString, dataSource, userInfo);
                    }
                }
                return _connectionString;
            }
        }



    }
}
