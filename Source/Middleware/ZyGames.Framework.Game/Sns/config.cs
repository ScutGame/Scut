using System.Configuration;
using System.IO;
using System.Web;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Security;

namespace ZyGames.Framework.Game.Sns
{
    public class config
    {
        private static string _connectionString = string.Empty;

        public static string connectionString
        {
            get
            {
                if (_connectionString == string.Empty)
                {
                    string cString = ConfigUtils.GetSetting("Snscenter_ConnectionString");
                    string DataSource = string.Empty;
                    string UserInfo = string.Empty;
                    try
                    {
                        DataSource = ConfigUtils.GetSetting("Snscenter_Server");
                        UserInfo = CryptoHelper.DES_Decrypt(ConfigUtils.GetSetting("Snscenter_Acount"), DES_KEY);
                    }
                    catch
                    {                        
                    }
                    if (string.IsNullOrEmpty(DataSource) || string.IsNullOrEmpty(UserInfo))
                    {
                        string path = HttpContext.Current.Server.MapPath("");
                        ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                        //Assembly assembly = Assembly.GetCallingAssembly();
                        //Uri uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
                        //map.ExeConfigFilename = Path.Combine(uri.LocalPath, "Sns.config");
                        map.ExeConfigFilename = Path.Combine(path, "Sns.config");
                        System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(map, 0);
                        if (configuration != null && configuration.AppSettings.Settings.Count > 0)
                        {
                            DataSource = configuration.AppSettings.Settings["Snscenter_Server"].Value;
                            UserInfo = CryptoHelper.DES_Decrypt(configuration.AppSettings.Settings["Snscenter_Acount"].Value, DES_KEY);
                        }
                    }
                    _connectionString = string.Format(cString, DataSource, UserInfo);
                }
                return _connectionString;
            }
        }

        public static readonly string DES_KEY = "5^1-34E!";
    }
}
