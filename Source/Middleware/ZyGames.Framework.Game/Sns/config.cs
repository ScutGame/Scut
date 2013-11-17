/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System.Configuration;
using System.IO;
using System.Web;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Sns
{
	/// <summary>
	/// Config.
	/// </summary>
    public class config
    {
        private static string _connectionString = string.Empty;
		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
        public static string connectionString
        {
            get
            {
                string DES_KEY = GameEnvironment.ProductDesEnKey;
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

    }
}