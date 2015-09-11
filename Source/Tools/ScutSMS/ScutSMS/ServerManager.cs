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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Text;
using System.Linq;
using Scut.SMS.Comm;
using Scut.SMS.Config;
using ScutServerManager.Config;
using ServiceStack.Redis;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Config;
using ZyGames.Framework.Data;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Redis;
using DbProviderType = ScutServerManager.Config.DbProviderType;
using JsonUtils = ZyGames.Framework.Common.Serialization.JsonUtils;

namespace Scut.SMS
{
    public class ServerManager
    {
        public ServerManager()
        {
            _redisSearchPatternList = new List<string>();
            RedisSearchTop = 10;
            RedisSearchPattern = "";
            ReLoadRedis();
        }

        public RedisSetting RedisConfig { get; private set; }
        private string _redisSearchPattern;
        private List<string> _redisSearchPatternList;
        public string RedisSearchPattern
        {
            get { return _redisSearchPattern; }
            set
            {
                _redisSearchPattern = value;
                _redisSearchPatternList.Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    var tempList = _redisSearchPattern.Split(new[] { ";", "\r\n" }, StringSplitOptions.None);
                    foreach (var temp in tempList)
                    {
                        if (string.IsNullOrEmpty(temp)) continue;
                        var key = string.Format(temp.StartsWith("~") ? "{0}*" : "*{0}*", temp.TrimStart('~').Trim('*'));
                        _redisSearchPatternList.Add(key);
                    }
                }
            }
        }

        public int RedisSearchTop { get; set; }

        #region Methond

        public void ReLoadRedis()
        {
            try
            {
                RedisConfig = RedisSettingFactory.Load();

                ICacheSerializer serializer = RedisConfig.Serializer == StorageMode.Protobuf
                    ? (ICacheSerializer)new ProtobufCacheSerializer()
                    : new JsonCacheSerializer(Encoding.UTF8);

                RedisConnectionPool.Initialize(new RedisPoolSetting()
                {
                    Host = RedisConfig.Host,
                    ReadOnlyHost = RedisConfig.Host,
                    MaxWritePoolSize = 2,
                    MaxReadPoolSize = 2,
                    DbIndex = RedisConfig.Db

                }, serializer);

                var _redisSection = ConfigManager.Configger.GetFirstOrAddConfig<RedisSection>();
                if (_redisSection == null)
                {
                    
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ReLoadRedis error:{0}", ex);
            }
        }

        public bool TryRedisInfo(out string str)
        {
            string msg = "";
            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                msg = string.Format("Redis[{0}] Info:\r\n", this.RedisConfig.ReadOnlyHost);
                client.Info.ToList().OrderBy(p => p.Key).ToList().ForEach(o =>
                {
                    msg += "\t" + o.Key + ":\t" + o.Value + "\r\n";
                });
                result = true;
            });
            str = msg;
            return result;
        }

        public bool TryExecute(Func<RedisClient, bool> func)
        {
            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                result = func(client);
            });
            return result;
        }

        public bool TryRenameKey(string fromKey, string toKey)
        {
            return TryRenameKey(new[] { new KeyValuePair<string, string>(fromKey, toKey) });
        }

        public bool TryRenameKey(IEnumerable<KeyValuePair<string, string>> keyPairs)
        {
            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                foreach (var pair in keyPairs)
                {
                    client.RenameKey(pair.Key, pair.Value);
                }
                result = true;
            });
            return result;
        }

        public bool TrySearchRedisKeys(out List<string> keys, int top = 0)
        {
            string pattern = "*";
            if (_redisSearchPatternList.Count == 1)
            {
                pattern = _redisSearchPatternList[0];
            }
            List<string> list = null;
            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                if (top > 0)
                {
                    list = _redisSearchPatternList.Count > 1
                    ? SearchKeys(client, _redisSearchPatternList).OrderBy(k => k).Take(top).ToList()
                    : client.SearchKeys(pattern).OrderBy(k => k).Take(top).ToList();
                }
                else
                {
                    list = _redisSearchPatternList.Count > 1
                    ? SearchKeys(client, _redisSearchPatternList).OrderBy(k => k).ToList()
                    : client.SearchKeys(pattern).OrderBy(k => k).ToList();
                }
                result = true;
            });
            keys = list;
            return result;
        }

        private List<string> SearchKeys(RedisClient redisClient, IEnumerable<string> patterns)
        {
            var list = new List<string>();
            foreach (var pattern in patterns)
            {
                list.AddRange(redisClient.SearchKeys(pattern));
            }
            return list;
        }

        public bool TryGetRedisKeyValue(string key, out int error, out string msg)
        {
            bool result = false;
            int e = 0;
            string str = "";
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                result = GetRedisKeyValue(client, key, out  e, out  str);
            });
            error = e;
            msg = str;
            return result;
        }

        private bool GetRedisKeyValue(RedisClient client, string key, out int error, out string msg)
        {
            error = 0;
            msg = "";
            int count = 10;
            if (key.StartsWith("__GLOBAL_CHANGE_KEYS_NEW") || key.StartsWith("__QUEUE_REDIS_SYNC"))
            {
                byte[][] keyBytes = client.HKeys(key);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Count:{0:D}", keyBytes.Length);
                sb.AppendLine("[");
                foreach (var buffer in keyBytes)
                {
                    sb.AppendFormat("    {0}", Encoding.UTF8.GetString(buffer));
                    sb.AppendLine();
                }
                sb.AppendLine("]");
                msg = sb.ToString();
            }
            else if (key.StartsWith("__GLOBAL_CHANGE_KEYS"))
            {
                byte[][] buffers = client.ZRange(key, 0, int.MaxValue);
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Count:{0:D}", buffers.Length);
                sb.AppendLine("[");
                foreach (var buffer in buffers)
                {
                    var temp = ProtoBufUtils.Deserialize<KeyValuePair<string, byte[]>>(buffer);
                    sb.AppendFormat("    {0}", temp.Key);
                    sb.AppendLine();
                }
                sb.AppendLine("]");
                msg = sb.ToString();
            }
            else if (key.StartsWith("__GLOBAL_SESSIONS"))
            {
                byte[] data = client.Get<byte[]>(key) ?? new byte[0];
                var dict = ProtoBufUtils.Deserialize<ConcurrentDictionary<Guid, GameSession>>(data);
                msg = "Count:" + dict.Count + "\r\n";
                msg += JsonHelper.FormatJsonToString(JsonUtils.Serialize(dict.Values.ToList()));
            }
            else if (key.StartsWith("__GLOBAL_SQL_CHANGE_KEYS_NEW") || key.StartsWith("__QUEUE_SQL_SYNC_WAIT"))
            {
                byte[][] keyBytes = client.HKeys(key);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Count:" + keyBytes.Length);
                sb.AppendLine("[");
                foreach (var buffer in keyBytes)
                {
                    sb.AppendFormat("    {0}", Encoding.UTF8.GetString(buffer));
                    sb.AppendLine();
                }
                sb.AppendLine("]");
                msg = sb.ToString();
            }
            else if (key.StartsWith("__GLOBAL_SQL_CHANGE_KEYS"))
            {
                byte[] data = client.Get<byte[]>(key) ?? new byte[0];
                var dict = ProtoBufUtils.Deserialize<Dictionary<string, bool>>(data) ??
                           new Dictionary<string, bool>();
                msg = "Count:" + dict.Count + "\r\n";
                msg += JsonHelper.FormatJsonToString(JsonUtils.Serialize(dict.Keys.ToList()));
            }
            else if (key.StartsWith("EntityPrimaryKey_"))
            {
                var val = client.Get<long>(key);
                msg = string.Format("{0}:\t{1}", key, val);
            }
            else if (key.StartsWith("__GLOBAL_SQL_STATEMENT") || key.StartsWith("__QUEUE_SQL_SYNC"))
            {
                //sql
                StringBuilder sb = new StringBuilder();
                byte[][] buffers = client.ZRange(key, 0, int.MaxValue);
                int index = 0;
                foreach (var buffer in buffers)
                {
                    if (index >= count)
                    {
                        break;
                    }
                    var temp = ProtoBufUtils.Deserialize(buffer, typeof(SqlStatement));
                    sb.AppendLine(JsonHelper.FormatJsonToString(JsonUtils.Serialize(temp)));
                    index++;
                }
                msg = string.Format("Count:{0}/{1:D}\r\n", buffers.Length > count ? count : buffers.Length, buffers.Length);
                msg += sb.ToString();
            }
            else if (key.StartsWith("$"))
            {
                string[] arr = key.TrimStart(new char[]{'$'}).Split(new char[]{'_'});
                string typeName = arr[0];
                string keyCode = (arr.Length > 1) ? arr[1] : "";
                StringBuilder stringBuilder5 = new StringBuilder();
                if (string.IsNullOrEmpty(keyCode))
                {
                    byte[][] array10 = client.HKeys(key);
                    byte[][] array11 = client.HMGet(key, array10);
                    stringBuilder5.AppendFormat("【{0}】 Count:{1:D}", typeName, array10.Length);
                    stringBuilder5.AppendLine("");
                    int num4 = 0;
                    byte[][] array2 = array10;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        byte[] bytes3 = array2[i];
                        string arg = "null";
                        Assembly dymincEntity = AppSetting.Current.Entity.DymincEntity;
                        if (dymincEntity != null)
                        {
                            Type type = dymincEntity.GetType(typeName, false, true);
                            try
                            {
                                byte[] data5 = array11[num4];
                                object obj = ProtoBufUtils.Deserialize(data5, type);
                                if (obj != null)
                                {
                                    arg = JsonUtils.SerializeCustom(obj);
                                }
                            }
                            catch
                            {
                            }
                        }
                        stringBuilder5.AppendFormat("┗{0}\t{1}\t{2}", num4 + 1, Encoding.UTF8.GetString(bytes3), arg);
                        stringBuilder5.AppendLine();
                        num4++;
                    }
                }
                else
                {
                    stringBuilder5.AppendFormat("【{0}】Key:{1}", typeName, keyCode);
                    stringBuilder5.AppendLine("");
                    string arg2 = "null";
                    Assembly dymincEntity2 = AppSetting.Current.Entity.DymincEntity;
                    if (dymincEntity2 != null)
                    {
                        Type type2 = dymincEntity2.GetType(typeName, false, true);
                        try
                        {
                            byte[] data6 = client.HGet("$" + typeName, Encoding.UTF8.GetBytes(keyCode));
                            object obj2 = ProtoBufUtils.Deserialize(data6, type2);
                            if (obj2 != null)
                            {
                                arg2 = JsonUtils.SerializeCustom(obj2);
                            }
                        }
                        catch
                        {
                        }
                    }
                    stringBuilder5.AppendFormat("Value:{0}", arg2);
                    stringBuilder5.AppendLine();
                }
                stringBuilder5.AppendLine("");
                msg = stringBuilder5.ToString();

            }
            else
            {

                string formatType = AppSetting.DictTypeNameFormat;
                string typeName = key.Split('_')[0];
                var dymincEntity = AppSetting.Current.Entity.DymincEntity;
                if (dymincEntity == null)
                {
                    error = 102;
                    msg = "Not generating entity assembly!";
                    return false;
                }
                string asmName = dymincEntity.GetName().Name;
                string result = "";
                var buffer = client.Get(key) ?? new byte[0];
                var type = dymincEntity.GetType(typeName, false, true);

                if (key.EndsWith(":remove"))
                {
                    //dictionary
                    var dictType = dymincEntity.GetType(string.Format(formatType, type.FullName, asmName));
                    var dict = ProtoBufUtils.Deserialize(buffer, dictType);
                    result = JsonHelper.FormatJsonToString(JsonUtils.Serialize(dict));
                }
                else if ("String".Equals(typeName))
                {
                    result = Encoding.UTF8.GetString(buffer);
                }
                else
                {
                    var refType = Type.GetType(string.Format(formatType, type.FullName, asmName));
                    dynamic refObject = null;
                    try
                    {
                        refObject = ProtoBufUtils.Deserialize(buffer, refType);
                    }
                    catch
                    {
                    }
                    if (refObject != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Count:" + refObject.Count);
                        sb.AppendLine("[");
                        try
                        {
                            foreach (var pair in refObject)
                            {
                                object entity = pair.Value;
                                sb.AppendFormat("{0},", JsonHelper.FormatJsonToString(JsonUtils.Serialize(entity)));
                                sb.AppendLine();
                            }
                        }
                        catch (Exception er)
                        {
                            sb.AppendLine(er.Message);
                        }
                        sb.AppendLine("]");
                        result = sb.ToString();
                    }
                    else
                    {
                        refType = type;
                        try
                        {
                            refObject = ProtoBufUtils.Deserialize(buffer, refType);
                        }
                        catch
                        {
                        }
                        if (refObject != null)
                        {
                            result = JsonHelper.FormatJsonToString(JsonUtils.Serialize(refObject));
                        }
                        else
                        {
                            error = 100;
                            msg = string.Format("Not found match type:{0} to deserialize.", typeName);
                            return false;
                        }
                    }
                }
                msg = result;
            }
            return true;
        }

        public bool TryRemoveRedisKey(string key)
        {
            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                result = client.Remove(key);
            });
            return result;
        }


        public bool TryRemoveAllRedisKey()
        {
            bool result = false;
            RedisConnectionPool.ProcessReadOnly(client =>
            {
                client.FlushDb();
                result = true;
            });
            return result;
        }

        public static T EnumParse<T>(string value, int defalut = 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                return (T)Enum.ToObject(typeof(T), defalut);
            }
            if (value is string)
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            return (T)Enum.ToObject(typeof(T), value);
        }

        public string ToXml(GameSetting setting)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "", ""));
            var root = doc.CreateElement("configuration");
            doc.AppendChild(root);

            XmlNode appSettings = doc.CreateElement("appSettings");
            root.AppendChild(appSettings);
            UpdateAppSettings(setting, appSettings, doc);
            XmlNode connectionStrings = doc.CreateElement("connectionStrings");
            root.AppendChild(connectionStrings);
            UpdateConnectionStrings(setting, connectionStrings, doc);

            using (var ms = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(ms, null);
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
                StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                ms.Position = 0;
                string xml = sr.ReadToEnd();
                return xml;
            }
        }

        public GameSetting LoadFile(string fileName)
        {
            GameSetting setting = null;
            if (!File.Exists(fileName))
            {
                return setting;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            if (doc.SelectSingleNode("/configuration") == null)
            {
                return setting;
            }

            var appSettings = doc.SelectNodes("//appSettings/add[@key]");
            var connectionStrings = doc.SelectNodes("//connectionStrings/add[@name]");
            setting = new GameSetting();
            if (appSettings != null)
            {
                foreach (XmlElement child in appSettings)
                {
                    string name = child.GetAttribute("key");
                    string value = child.GetAttribute("value");
                    SetAppSetting(setting, name, value);
                }
            }
            if (connectionStrings != null)
            {
                foreach (XmlElement child in connectionStrings)
                {
                    string name = child.GetAttribute("name");
                    string providerName = child.GetAttribute("providerName");
                    string connectionString = child.GetAttribute("connectionString");
                    SetConnectionString(setting, name, providerName, connectionString);
                }
            }
            setting.IsModify = false;
            return setting;
        }

        public void SaveFile(GameSetting setting, string fileName)
        {
            if (!File.Exists(fileName))
            {
                using (var fs = File.Create(fileName))
                {
                    var data = Encoding.UTF8.GetBytes(@"<?xml version=""1.0""?>
<configuration />");
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            var root = doc.SelectSingleNode("/*");
            XmlNode appSettings = doc.SelectSingleNode("//appSettings");
            if (appSettings == null)
            {
                appSettings = doc.CreateElement("appSettings");
                root.AppendChild(appSettings);
            }
            UpdateAppSettings(setting, appSettings, doc);
            XmlNode connectionStrings = doc.SelectSingleNode("//connectionStrings");
            if (connectionStrings == null)
            {
                connectionStrings = doc.CreateElement("connectionStrings");
                root.AppendChild(connectionStrings);
            }
            UpdateConnectionStrings(setting, connectionStrings, doc);
            doc.Save(fileName);
            setting.IsModify = false;
        }

        private void SetConnectionString(GameSetting setting, string name, string providerName, string connectionString)
        {
            ConnectionString conn = null;
            if (name == GameSetting.DbSnscenter)
            {
                conn = setting.SnsCenter;
            }
            else if (name == GameSetting.DbPaycenter)
            {
                conn = setting.PayCenter;
            }
            else
            {
                conn = setting.Connections.Where(t => t.Name == name).FirstOrDefault();
                if (conn == null)
                {
                    conn = new ConnectionString() { Name = name };
                    setting.Connections.Add(conn);
                }
            }
            conn.ProviderName = EnumParse<DbProviderType>(providerName);
            var attrs = connectionString.Split(';');
            string extendAttr = conn.Extend ?? "";
            foreach (var item in attrs)
            {
                string[] keyValue = item.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim().Replace(" ", "").ToLower();
                    string value = keyValue[1].Trim();
                    switch (key)
                    {
                        case "server":
                        case "datasource":
                            string[] arr = value.Split(',');
                            conn.DataSource = arr[0];
                            if (arr.Length > 1)
                            {
                                conn.Port = arr[1].ToInt();
                            }
                            break;
                        case "charset":
                            conn.Charset = value;
                            break;
                        case "database":
                        case "initialcatalog":
                            conn.Database = value;
                            break;
                        case "uid":
                        case "userid":
                            conn.Uid = value;
                            break;
                        case "pwd":
                        case "password":
                            conn.Pwd = value;
                            break;
                        case "port":
                            conn.Port = value.ToInt();
                            break;
                        default:
                            extendAttr += string.Format("{0}={1};", keyValue[0].Trim(), value);
                            break;
                    }
                }
            }
            conn.Extend = extendAttr;
        }

        private void UpdateConnectionStrings(GameSetting setting, XmlNode connectionStrings, XmlDocument parent)
        {
            var list = new List<ConnectionString>(setting.Connections);
            list.Insert(0, setting.SnsCenter);
            list.Insert(1, setting.PayCenter);
            connectionStrings.RemoveAll();
            foreach (var connection in list)
            {
                var elem = parent.CreateElement("add");
                var keyAttr = parent.CreateAttribute("name");
                keyAttr.Value = connection.Name;
                var providerNameAttr = parent.CreateAttribute("providerName");
                providerNameAttr.Value = connection.ProviderName.ToString();
                var connectionStringAttr = parent.CreateAttribute("connectionString");
                connectionStringAttr.Value = connection.FormatString();
                elem.Attributes.Append(keyAttr);
                elem.Attributes.Append(providerNameAttr);
                elem.Attributes.Append(connectionStringAttr);
                connectionStrings.AppendChild(elem);
            }
        }

        private void UpdateAppSettings(GameSetting setting, XmlNode appSettings, XmlDocument parent)
        {
            SaveKeyValue(appSettings, parent, "Product.Code", () => setting.GameCode, DefaultConfig.GameCode, true);
            SaveKeyValue(appSettings, parent, "Product.Name", () => setting.GameName, DefaultConfig.GameName);
            SaveKeyValue(appSettings, parent, "Product.ServerId", () => setting.GameServerCode, DefaultConfig.ServerCode, true);
            SaveKeyValue(appSettings, parent, "Product.SignKey", () => setting.GameSignKey, DefaultConfig.GameSignKey);
            SaveKeyValue(appSettings, parent, "Product.DesEnKey", () => setting.ProductDesEnKey, DefaultConfig.ProductDesEnKey);
            SaveKeyValue(appSettings, parent, "Product.ClientDesDeKey", () => setting.ClientDesDeKey, DefaultConfig.ClientDesDeKey);
            SaveKeyValue(appSettings, parent, "Game.Port", () => setting.SocketPort, DefaultConfig.SocketPort, true);
            SaveKeyValue(appSettings, parent, "Redis.Host", () => setting.Redis.Host, DefaultConfig.RedisHost, true);
            SaveKeyValue(appSettings, parent, "Redis.ReadHost", () => setting.Redis.ReadOnlyHost, DefaultConfig.ReadOnlyHost, true);
            SaveKeyValue(appSettings, parent, "Redis.Db", () => setting.Redis.Db, DefaultConfig.RedisDb, true);
            SaveKeyValue(appSettings, parent, "Redis.ConnectTimeout", () => setting.Redis.ConnectTimeout, DefaultConfig.ConnectTimeout);
            SaveKeyValue(appSettings, parent, "Redis.PoolTimeOut", () => setting.Redis.PoolTimeOut, DefaultConfig.PoolTimeOut);
            SaveKeyValue(appSettings, parent, "Redis.Pool.MaxWritePoolSize", () => setting.Redis.MaxWritePoolSize, DefaultConfig.MaxWritePoolSize);
            SaveKeyValue(appSettings, parent, "Redis.Pool.MaxReadPoolSize", () => setting.Redis.MaxReadPoolSize, DefaultConfig.MaxReadPoolSize);

            SaveKeyValue(appSettings, parent, "Game.Http.Host", () => setting.HttpHost, DefaultConfig.HttpHost, true);
            SaveKeyValue(appSettings, parent, "Game.Http.Port", () => setting.HttpPort, DefaultConfig.HttpPort, true);
            SaveKeyValue(appSettings, parent, "Game.Http.Name", () => setting.HttpName, DefaultConfig.HttpName);
            SaveKeyValue(appSettings, parent, "Game.Http.Timeout", () => setting.HttpTimeout, DefaultConfig.HttpTimeout);
            SaveKeyValue(appSettings, parent, "MaxConnections", () => setting.SocketMaxConnections, DefaultConfig.SocketMaxConnections);
            SaveKeyValue(appSettings, parent, "Backlog", () => setting.SocketBacklog, DefaultConfig.SocketBacklog);
            SaveKeyValue(appSettings, parent, "MaxAcceptOps", () => setting.SocketMaxAcceptOps, DefaultConfig.SocketMaxAcceptOps);
            SaveKeyValue(appSettings, parent, "BufferSize", () => setting.SocketBufferSize, DefaultConfig.SocketBufferSize);

            SaveKeyValue(appSettings, parent, "Game.Action.EnableGZip", () => setting.ActionEnableGZip, DefaultConfig.ActionEnableGZip);
            SaveKeyValue(appSettings, parent, "Game.Action.GZipOutLength", () => setting.ActionGZipOutLength, DefaultConfig.ActionGZipOutLength);
            SaveKeyValue(appSettings, parent, "Game.Action.TypeName", () => setting.ActionTypeName, DefaultConfig.ActionTypeName);
            SaveKeyValue(appSettings, parent, "Game.Action.AssemblyName", () => setting.ActionAssemblyName, DefaultConfig.ActionAssemblyName);
            SaveKeyValue(appSettings, parent, "Game.Action.Script.TypeName", () => setting.ActionScriptTypeName, DefaultConfig.ActionScriptTypeName);
            SaveKeyValue(appSettings, parent, "Python_Disable", () => setting.PythonDisable, DefaultConfig.PythonDisable);
            SaveKeyValue(appSettings, parent, "Script_IsDebug", () => setting.ScriptIsDebug, DefaultConfig.ScriptIsDebug, true);
            SaveKeyValue(appSettings, parent, "ScriptRelativePath", () => setting.ScriptRelativePath, DefaultConfig.ScriptRelativePath);
            SaveKeyValue(appSettings, parent, "CSharpRootPath", () => setting.CSharpRootPath, DefaultConfig.CSharpRootPath);
            SaveKeyValue(appSettings, parent, "PythonRootPath", () => setting.PythonRootPath, DefaultConfig.PythonRootPath);

            SaveKeyValue(appSettings, parent, "Lua_Disable", () => setting.LuaDisable, DefaultConfig.LuaDisable);
            SaveKeyValue(appSettings, parent, "LuaRootPath", () => setting.LuaRootPath, DefaultConfig.LuaRootPath);

            SaveKeyValue(appSettings, parent, "ScriptMainClass", () => setting.ScriptMainClass, DefaultConfig.ScriptMainClass, true);
            SaveKeyValue(appSettings, parent, "ScriptMainTypeName", () => setting.ScriptMainTypeName, DefaultConfig.ScriptMainTypeName, true);
            SaveKeyValue(appSettings, parent, "ScriptSysAsmReferences", () => string.Join(";", setting.ScriptSysAsmReferences), "");
            SaveKeyValue(appSettings, parent, "ScriptAsmReferences", () => string.Join(";", setting.ScriptAsmReferences), "");

            SaveKeyValue(appSettings, parent, "Game.Entity.AssemblyName", () => setting.ModelEntityAssemblyName, DefaultConfig.ModelEntityAssemblyName);
            SaveKeyValue(appSettings, parent, "Game.Script.DecodeFunc.TypeName", () => setting.ScriptDecodeFuncTypeName, DefaultConfig.ScriptDecodeFuncTypeName, true);
            SaveKeyValue(appSettings, parent, "Game.Remote.Script.TypeName", () => setting.RemoteScriptTypeName, DefaultConfig.RemoteScriptTypeName, true);

            SaveKeyValue(appSettings, parent, "Cache.global.period", () => setting.CacheGlobalPeriod, DefaultConfig.CacheGlobalPeriod);
            SaveKeyValue(appSettings, parent, "Cache.user.period", () => setting.CacheUserPeriod, DefaultConfig.CacheUserPeriod);
            SaveKeyValue(appSettings, parent, "Game.Cache.UpdateDbInterval", () => setting.CacheUpdateDbInterval, DefaultConfig.CacheUpdateDbInterval);
            SaveKeyValue(appSettings, parent, "Cache.update.interval", () => setting.CacheUpdateInterval, DefaultConfig.CacheUpdateInterval);
            SaveKeyValue(appSettings, parent, "Cache.expired.interval", () => setting.CacheExpiredInterval, DefaultConfig.CacheExpiredInterval);
            SaveKeyValue(appSettings, parent, "Cache.enable.writetoDb", () => setting.CacheEnableWritetoDb, DefaultConfig.CacheEnableWritetoDb);
            SaveKeyValue(appSettings, parent, "Cache.Serializer", () => setting.CacheSerializer, DefaultConfig.CacheSerializer);
            SaveKeyValue(appSettings, parent, "DataSyncQueueNum", () => setting.DataSyncQueueNum, DefaultConfig.DataSyncQueueNum);
            SaveKeyValue(appSettings, parent, "SqlWaitSyncQueueNum", () => setting.SqlWaitSyncQueueNum, DefaultConfig.SqlWaitSyncQueueNum);
            SaveKeyValue(appSettings, parent, "SqlSyncQueueNum", () => setting.SqlSyncQueueNum, DefaultConfig.SqlSyncQueueNum);


            SaveKeyValue(appSettings, parent, "Log.TableName.Format", () => setting.LogTableNameFormat, DefaultConfig.LogTableNameFormat);
            SaveKeyValue(appSettings, parent, "Log.PriorBuild.Month", () => setting.LogPriorBuildMonth, DefaultConfig.LogPriorBuildMonth);
            SaveKeyValue(appSettings, parent, "PublishType", () => setting.PublishType, DefaultConfig.DefPublishType);
            SaveKeyValue(appSettings, parent, "EnableGM", () => setting.EnableGm, DefaultConfig.EnableGm);
            SaveKeyValue(appSettings, parent, "Sns.PreAccount", () => setting.SnsAccountPrefixChar, DefaultConfig.SnsAccountPrefixChar);
            SaveKeyValue(appSettings, parent, "Game.Language.TypeName", () => setting.LanguageTypeName, DefaultConfig.LanguageTypeName);
        }

        private void SetAppSetting(GameSetting setting, string name, string value)
        {
            switch (name)
            {
                case "Product.Code":
                    setting.GameCode = Convert.ToInt32(value);
                    break;
                case "Product.Name":
                    setting.GameName = Convert.ToString(value);
                    break;
                case "Product.ServerId":
                    setting.GameServerCode = Convert.ToInt32(value);
                    break;
                case "Product.SignKey":
                    setting.GameSignKey = value;
                    break;
                case "Product.DesEnKey":
                    setting.ProductDesEnKey = value;
                    break;
                case "Product.ClientDesDeKey":
                    setting.ClientDesDeKey = value;
                    break;
                case "Game.Port":
                    setting.SocketPort = Convert.ToInt32(value);
                    break;
                case "Redis.Host":
                    setting.Redis.Host = value;
                    break;
                case "Redis.ReadHost":
                    setting.Redis.ReadOnlyHost = value;
                    break;
                case "Redis.Db":
                    setting.Redis.Db = Convert.ToInt32(value);
                    break;
                case "Redis.MaxWritePoolSize":
                    setting.Redis.MaxWritePoolSize = Convert.ToInt32(value);
                    break;
                case "Redis.MaxReadPoolSize":
                    setting.Redis.MaxReadPoolSize = Convert.ToInt32(value);
                    break;
                case "Redis.ConnectTimeout":
                    setting.Redis.ConnectTimeout = Convert.ToInt32(value);
                    break;
                case "Redis.PoolTimeOut":
                    setting.Redis.PoolTimeOut = Convert.ToInt32(value);
                    break;

                case "Game.Http.Host":
                    setting.HttpHost = value;
                    break;
                case "Game.Http.Port":
                    setting.HttpPort = Convert.ToInt32(value);
                    break;
                case "Game.Http.Name":
                    setting.HttpName = value;
                    break;
                case "Game.Http.Timeout":
                    setting.HttpTimeout = Convert.ToInt32(value);
                    break;
                case "MaxConnections":
                    setting.SocketMaxConnections = Convert.ToInt32(value);
                    break;
                case "Backlog":
                    setting.SocketBacklog = Convert.ToInt32(value);
                    break;
                case "MaxAcceptOps":
                    setting.SocketMaxAcceptOps = Convert.ToInt32(value);
                    break;
                case "BufferSize":
                    setting.SocketBufferSize = Convert.ToInt32(value);
                    break;
                case "Game.Action.EnableGZip":
                    setting.ActionEnableGZip = Convert.ToBoolean(value);
                    break;
                case "Game.Action.GZipOutLength":
                    setting.ActionGZipOutLength = Convert.ToInt32(value);
                    break;
                case "Game.Action.TypeName":
                    setting.ActionTypeName = value;
                    break;
                case "Game.Action.AssemblyName":
                    setting.ActionAssemblyName = value;
                    break;
                case "Game.Action.Script.TypeName":
                    setting.ActionScriptTypeName = value;
                    break;
                case "Python_Disable":
                    setting.PythonDisable = Convert.ToBoolean(value);
                    break;
                case "Script_IsDebug":
                    setting.ScriptIsDebug = Convert.ToBoolean(value);
                    break;
                case "CSharpRootPath":
                    setting.CSharpRootPath = value;
                    break;
                case "PythonRootPath":
                    setting.PythonRootPath = value;
                    break;
                case "Lua_Disable":
                    setting.LuaDisable = Convert.ToBoolean(value);
                    break;
                case "LuaRootPath":
                    setting.LuaRootPath = value;
                    break;

                case "ScriptMainClass":
                    setting.ScriptMainClass = value;
                    break;
                case "ScriptMainTypeName":
                    setting.ScriptMainTypeName = value;
                    break;
                case "ScriptSysAsmReferences":
                    setting.ScriptSysAsmReferences = value.Split(';');
                    break;
                case "ScriptAsmReferences":
                    setting.ScriptAsmReferences = value.Split(';');
                    break;
                case "Game.Entity.AssemblyName":
                    setting.ModelEntityAssemblyName = value;
                    break;
                case "Game.Script.DecodeFunc.TypeName":
                    setting.ScriptDecodeFuncTypeName = value;
                    break;
                case "Game.Remote.Script.TypeName":
                    setting.RemoteScriptTypeName = value;
                    break;

                case "Cache.global.period":
                    setting.CacheGlobalPeriod = Convert.ToInt32(value);
                    break;
                case "Cache.user.period":
                    setting.CacheUserPeriod = Convert.ToInt32(value);
                    break;
                case "Game.Cache.UpdateDbInterval":
                    setting.CacheUpdateDbInterval = Convert.ToInt32(value);
                    break;
                case "Cache.update.interval":
                    setting.CacheUpdateInterval = Convert.ToInt32(value);
                    break;
                case "Cache.expired.interval":
                    setting.CacheExpiredInterval = Convert.ToInt32(value);
                    break;
                case "Cache.enable.writetoDb":
                    setting.CacheEnableWritetoDb = Convert.ToBoolean(value);
                    break;
                case "Cache.Serializer":
                    setting.CacheSerializer = value.ToEnum<StorageMode>();
                    break;
                case "DataSyncQueueNum":
                    setting.DataSyncQueueNum = Convert.ToInt32(value);
                    break;
                case "SqlWaitSyncQueueNum":
                    setting.SqlWaitSyncQueueNum = Convert.ToInt32(value);
                    break;
                case "SqlSyncQueueNum":
                    setting.SqlSyncQueueNum = Convert.ToInt32(value);
                    break;

                case "Log.TableName.Format":
                    setting.LogTableNameFormat = value;
                    break;
                case "Log.PriorBuild.Month":
                    setting.LogPriorBuildMonth = Convert.ToInt32(value);
                    break;
                case "PublishType":
                    setting.PublishType = EnumParse<PublishType>(value);
                    break;
                case "EnableGM":
                    setting.EnableGm = Convert.ToBoolean(value);
                    break;
                case "Sns.PreAccount":
                    setting.SnsAccountPrefixChar = value;
                    break;
                case "Game.Language.TypeName":
                    setting.LanguageTypeName = value;
                    break;
                default:
                    break;
            }
        }

        private void SaveKeyValue(XmlNode appSettings, XmlDocument parent, string name, Func<object> valueFunc, object defaultVal, bool isWrite = false)
        {
            string value = valueFunc().ToString();
            var elem = (XmlElement)appSettings.SelectSingleNode("add[@key=\"" + name + "\"]");
            if (!isWrite && Equals(value, defaultVal.ToString()))
            {
                if (elem != null)
                {
                    appSettings.RemoveChild(elem);
                }
                return;
            }
            if (elem == null)
            {
                elem = parent.CreateElement("add");
                var keyAttr = parent.CreateAttribute("key");
                keyAttr.Value = name;
                var valueAttr = parent.CreateAttribute("value");
                valueAttr.Value = value;
                elem.Attributes.Append(keyAttr);
                elem.Attributes.Append(valueAttr);
                appSettings.AppendChild(elem);
            }
            else
            {
                elem.SetAttribute("value", value);
            }
        }

        #endregion


        internal bool TryBackupRedis(string type)
        {
            bool b = TryExecute(client =>
            {
                switch (type)
                {
                    case "AOF":
                        client.BgRewriteAof();
                        break;
                    case "RDB":
                        client.BgSave();
                        break;
                    default:
                        return false;
                }
                return true;
            });
            return b;
        }

        private const string HistoryTable = "Temp_EntityHistory";
        public bool TryMoveKeyToDb(ConnectionString setting, IEnumerable<string> keys, out int success)
        {
            success = 0;
            return false;
            //结构调用， 不使用；
            int num = 0;
            if (!setting.HasConfig())
            {
                throw new ArgumentNullException("database connectionstring is empty.");
            }
            var proveder = DbConnectionProvider.CreateDbProvider(setting.Name, setting.ProviderName.ToString(), setting.FormatString());

            bool b = TryExecute(client =>
            {
                bool isError = false;
                foreach (var key in keys)
                {
                    byte[] buffer = ProtoBufUtils.Serialize(client.HGetAll(key));
                    //var buffer = client.Get<byte[]>(key);
                    if (buffer != null)
                    {
                        var command = proveder.CreateCommandStruct(HistoryTable, CommandMode.ModifyInsert);
                        command.AddParameter("Key", key);
                        command.AddParameter("Value", buffer);
                        command.Filter = proveder.CreateCommandFilter();
                        command.Filter.Condition = proveder.FormatFilterParam("Key");
                        command.Filter.AddParam("F_Key", key);
                        command.Parser();
                        if (proveder.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0)
                        {
                            num++;
                        }
                        else
                        {
                            isError = true;
                        }
                    }
                }
                if (!isError)
                {
                    client.RemoveAll(keys);
                }
                return !isError;
            });
            success = num;
            return b;
        }
    }
}