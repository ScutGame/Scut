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
using System.Configuration;
using ZyGames.Framework.Collection;
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
            else
            {
                ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings[connectKey];
                if (connSetting != null)
                {
                    string connectionString = connSetting.ConnectionString;
                    try
                    {
                        Type type = TryGetProviderType(connSetting.ProviderName);
                        dbBaseProvider = CreateDbProvider(type, connectionString);
                        dbProviders.TryAdd(connectKey, dbBaseProvider);
                    }
                    catch
                    {
                        TraceLog.WriteError("ProviderName:{0} instance failed.", connSetting.ProviderName);
                    }
                }
                else
                {
                    TraceLog.WriteError("Connection:{0} config is not exist.", connectKey);
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
                Type type = TryGetProviderType(schema.ConnectionProviderType);
                return CreateDbProvider(type, schema.ConnectionString);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbBaseProvider CreateDbProvider(Type type, string connectionString)
        {
            if (type == null)
            {
                type = typeof(SqlDataProvider);
            }
            return type.CreateInstance<DbBaseProvider>(connectionString);
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