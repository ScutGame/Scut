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
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Configuration;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data.Sql;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Data
{

    /// <summary>
    /// 数据连接提供类
    /// </summary>
    public sealed class DbConnectionProvider
    {
        private static ConcurrentDictionary<string, DbBaseProvider> dbProviders = new ConcurrentDictionary<string, DbBaseProvider>();

        /// <summary>
        /// 初始化DB连接
        /// </summary>
        public static void Initialize()
        {
            DbBaseProvider dbBaseProvider = null;
            var er = ConfigurationManager.ConnectionStrings.GetEnumerator();
            while (er.MoveNext())
            {
                ConnectionStringSettings connSetting = er.Current as ConnectionStringSettings;
                if (connSetting == null) continue;
                var setting = ConnectionSetting.Create(connSetting.Name, connSetting.ProviderName, connSetting.ConnectionString.Trim());
                if (setting.DbLevel == DbLevel.LocalMysql || setting.DbLevel == DbLevel.LocalSql)
                {
                    continue;
                }
                dbBaseProvider = CreateDbProvider(setting);
                try
                {
                    dbBaseProvider.CheckConnect();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Not connect to the database server \"{0}\" database \"{1}\".", dbBaseProvider.ConnectionSetting.DataSource, dbBaseProvider.ConnectionSetting.DatabaseName), ex);
                }
                dbProviders.TryAdd(connSetting.Name, dbBaseProvider);
            }

        }

        /// <summary>
        /// 查找库连接
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, DbBaseProvider> Find(DbLevel dbLevel)
        {
            return dbProviders.Where(pair => pair.Value.ConnectionSetting.DbLevel == dbLevel).FirstOrDefault();
        }

        /// <summary>
        /// 取第一个
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, DbBaseProvider> FindFirst()
        {
            var providers = dbProviders.Where(
                pair => pair.Value.ConnectionSetting.DbLevel != DbLevel.LocalSql &&
                    pair.Value.ConnectionSetting.DbLevel != DbLevel.LocalMysql)
                .ToList();
            return providers.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectKey"></param>
        /// <returns></returns>
        public static DbBaseProvider CreateDbProvider(string connectKey)
        {
            DbBaseProvider dbBaseProvider = null;
            if (string.IsNullOrEmpty(connectKey))
            {
                return dbBaseProvider;
            }
            if (dbProviders.TryGetValue(connectKey, out dbBaseProvider))
            {
                return dbBaseProvider;
            }

            ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings[connectKey];
            if (connSetting != null)
            {
                string connectionString = connSetting.ConnectionString;
                try
                {
                    dbBaseProvider = CreateDbProvider(connSetting.Name, connSetting.ProviderName, connectionString);
                    dbProviders.TryAdd(connectKey, dbBaseProvider);
                }
                catch
                {
                    TraceLog.WriteError("ProviderName:{0} instance failed.", connSetting.ProviderName);
                }
            }

            return dbBaseProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static DbBaseProvider CreateDbProvider(SchemaTable schema)
        {
            if (!string.IsNullOrEmpty(schema.ConnectKey))
            {
                return CreateDbProvider(schema.ConnectKey);
            }
            if (!string.IsNullOrEmpty(schema.ConnectionString))
            {
                return CreateDbProvider(schema.ConnectKey, schema.ConnectionProviderType, schema.ConnectionString);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providerTypeName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbBaseProvider CreateDbProvider(string name, string providerTypeName, string connectionString)
        {
            var setting = ConnectionSetting.Create(name, providerTypeName, connectionString);
            return CreateDbProvider(setting);
        }

        private static DbBaseProvider CreateDbProvider(ConnectionSetting setting)
        {
            Type type = TryGetProviderType(setting.ProviderTypeName);
            if (type == null)
            {
                type = typeof(SqlDataProvider);
            }
            return type.CreateInstance<DbBaseProvider>(setting);
        }

        private static Type TryGetProviderType(string providerTypeName)
        {
            Type type;
            if (!string.IsNullOrEmpty(providerTypeName))
            {
                if (providerTypeName.IndexOf(",") != -1)
                {
                    type = Type.GetType(providerTypeName);
                }
                else
                {
                    string typeName = string.Empty;
                    if (providerTypeName.StartsWith("mysql", StringComparison.CurrentCultureIgnoreCase))
                    {
                        typeName = string.Format("ZyGames.Framework.Data.MySql.{0},ZyGames.Framework", providerTypeName);
                    }
                    else
                    {
                        typeName = string.Format("ZyGames.Framework.Data.Sql.{0},ZyGames.Framework", providerTypeName);
                    }
                    type = Type.GetType(typeName, false, true);
                }
            }
            else
            {
                type = typeof(SqlDataProvider);
            }
            return type;
        }

    }
}