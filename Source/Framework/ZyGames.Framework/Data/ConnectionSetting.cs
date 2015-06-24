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
using System;
using System.Collections.Concurrent;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data.MySql;
using ZyGames.Framework.Data.Sql;

namespace ZyGames.Framework.Data
{
    /// <summary>
    /// 数据库级别
    /// </summary>
    public enum DbLevel
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKown = 0,
        /// <summary>
        /// 配置
        /// </summary>
        Config,
        /// <summary>
        /// 游戏
        /// </summary>
        Game,
        /// <summary>
        /// 日志
        /// </summary>
        Log,
        /// <summary>
        /// 本机的Sql数据服务
        /// </summary>
        LocalSql,
        /// <summary>
        /// 本机的MySql数据服务
        /// </summary>
        LocalMySql,
    }
    /// <summary>
    /// 数据提供者类型
    /// </summary>
    public enum DbProviderType
    {
        /// <summary>
        /// 
        /// </summary>
        Unkown = 0,
        /// <summary>
        /// 
        /// </summary>
        MsSql = 1,
        /// <summary>
        /// 
        /// </summary>
        MySql
    }
    /// <summary>
    /// Connection setting.
    /// </summary>
    public class ConnectionSetting
    {
        private static ConcurrentDictionary<string, ConnectionSetting> _settings = new ConcurrentDictionary<string, ConnectionSetting>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerTypeName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ConnectionSetting Create(string providerName, string providerTypeName, string connectionString)
        {
            string key = MathUtils.ToHexMd5Hash(providerTypeName + connectionString);
            ConnectionSetting setting;
            if (_settings.TryGetValue(key, out setting))
            {
                return setting;
            }
            setting = new ConnectionSetting(providerName, providerTypeName, connectionString);
            key = MathUtils.ToHexMd5Hash(setting.ProviderTypeName + setting.ConnectionString);
            _settings[key] = setting;
            return setting;
        }

        /// <summary>
        /// clear setting
        /// </summary>
        public static void ClearSetting()
        {
            _settings.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionSetting"/> class.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerTypeName">provider type name.</param>
        /// <param name="connectionString">Connection string.</param>
        private ConnectionSetting(string providerName, string providerTypeName, string connectionString)
        {
            ProviderName = providerName;
            ProviderTypeName = providerTypeName;
            ProviderType = DbProviderType.Unkown;
            if (typeof(SqlDataProvider).Name.IsEquals(ProviderTypeName, true))
            {
                ProviderType = DbProviderType.MsSql;
            }
            else if (typeof(MySqlDataProvider).Name.IsEquals(ProviderTypeName, true))
            {
                ProviderType = DbProviderType.MySql;
            }
            ConnectionString = connectionString ?? "";
            Initializer();
        }

        void Initializer()
        {
            string[] items = ConnectionString.Split(';');
            foreach (var item in items)
            {
                string[] keyValue = item.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim().ToLower();
                    string val = keyValue[1].Trim();
                    if ("server".Equals(key, StringComparison.Ordinal) ||
                        "data source".Equals(key, StringComparison.Ordinal))
                    {
                        DataSource = val;
                        continue;
                    }
                    if ("charset".Equals(key, StringComparison.Ordinal))
                    {
                        CharSet = val;
                        continue;
                    }
                    if ("database".Equals(key, StringComparison.Ordinal) ||
                        "initial catalog".Equals(key, StringComparison.Ordinal))
                    {
                        DatabaseName = val;
                        DbLevel = DbLevel.UnKown;
                        val = val.ToLower();
                        if (val.IndexOf("config", StringComparison.Ordinal) != -1)
                        {
                            DbLevel = DbLevel.Config;
                        }
                        else if (val.IndexOf("data", StringComparison.Ordinal) != -1)
                        {
                            DbLevel = DbLevel.Game;
                        }
                        else if (val.IndexOf("log", StringComparison.Ordinal) != -1)
                        {
                            DbLevel = DbLevel.Log;
                        }
                        continue;
                    }
                }
            }

            if ("localsqlserver".Equals(ProviderName.ToLower(), StringComparison.Ordinal))
            {
                ProviderTypeName = "SqlDataProvider";
                DbLevel = DbLevel.LocalSql;
            }
            else if ("localmysqlserver".Equals(ProviderName.ToLower(), StringComparison.Ordinal))
            {
                ProviderTypeName = "MySqlDataProvider";
                DbLevel = DbLevel.LocalMySql;
            }

            if (ProviderType == DbProviderType.MySql)
            {
                if (string.IsNullOrEmpty(CharSet))
                {
                    CharSet = "gbk";
                    string charset = string.Format("CharSet={0};", CharSet);
                    ConnectionString += ConnectionString.EndsWith(";") ? charset : ";" + charset;
                }
            }
        }

        /// <summary>
        /// config name
        /// </summary>
        public string ProviderName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DbProviderType ProviderType { get; private set; }

        /// <summary>
        /// Provider type name.
        /// </summary>
        public string ProviderTypeName { get; private set; }

        /// <summary>
        /// mysql connect charset.
        /// </summary>
        public string CharSet { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DbLevel DbLevel { get; private set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public string DataSource
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the name of the data base.
        /// </summary>
        /// <value>The name of the data base.</value>
        public string DatabaseName
        {
            get;
            private set;
        }
    }
}