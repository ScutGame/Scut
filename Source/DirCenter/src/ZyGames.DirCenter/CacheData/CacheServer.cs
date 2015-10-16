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
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ZyGames.DirCenter.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;

namespace ZyGames.DirCenter.CacheData
{
    public class CacheServer : MemoryCacheStruct<ServerInfo>
    {
        private string GenerateKey(int gameId, int serverId)
        {
            return string.Format("{0}_{1}", gameId, serverId);
        }

        protected override bool InitCache()
        {
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Inquiry);
                command.Columns = "ID,GameID,ServerName,BaseUrl,ActiveNum,Weight,isEnable,TargetServer,EnableDate,IntranetAddress";
                command.OrderBy = "GameID asc, ID asc";
                command.Parser();

                using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
                {
                    LoadServer(reader);
                }

                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("InitCache {0} error:{1}", ContainerKey, ex);
                return false;
            }
        }

        private void LoadServer(IDataReader reader)
        {
            while (reader.Read())
            {
                var servierInfo = new ServerInfo
                {
                    ID = MathUtils.ToInt(reader["ID"]),
                    GameID = MathUtils.ToInt(reader["GameID"]),
                    TargetServer = MathUtils.ToInt(reader["TargetServer"]),
                    ServerName = MathUtils.ToNotNullString(reader["ServerName"]),
                    ServerUrl = MathUtils.ToNotNullString(reader["BaseUrl"]),
                    ActiveNum = MathUtils.ToInt(reader["ActiveNum"]),
                    Weight = MathUtils.ToInt(reader["Weight"]),
                    IsEnable = MathUtils.ToBool(reader["isEnable"]),
                    EnableDate = MathUtils.ToDateTime(reader["EnableDate"]),
                    IntranetAddress = MathUtils.ToNotNullString(reader["IntranetAddress"]),
                    Status = string.Empty
                };
                if (!string.IsNullOrEmpty(reader["ActiveNum"].ToString()))
                {
                    servierInfo.ActiveNum = MathUtils.ToInt(reader["ActiveNum"]);
                }
                string key = GenerateKey(servierInfo.GameID, servierInfo.ID);
                AddOrUpdate(key, servierInfo);
            }
        }


        public bool AddToCache(ServerInfo serverInfo)
        {
            string key = GenerateKey(serverInfo.GameID, serverInfo.ID);
            try
            {
                //判断是否重名
                ServerInfo tempServer = Find(p => p.GameID == serverInfo.GameID && Equals(p.ServerName, serverInfo.ServerName));
                if (tempServer == null)
                {
                    tempServer = new ServerInfo();
                    tempServer.ID = serverInfo.ID;
                    tempServer.GameID = serverInfo.GameID;
                    tempServer.TargetServer = serverInfo.TargetServer;
                    tempServer.ServerName = serverInfo.ServerName;
                    tempServer.ServerUrl = serverInfo.ServerUrl;
                    tempServer.ActiveNum = serverInfo.ActiveNum;
                    tempServer.Status = serverInfo.Status;
                    tempServer.EnableDate = serverInfo.EnableDate;
                    tempServer.IntranetAddress = serverInfo.IntranetAddress;

                    var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                    var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Insert);

                    command.AddParameter("ID", serverInfo.ID);
                    command.AddParameter("GameID", serverInfo.GameID);
                    command.AddParameter("TargetServer", serverInfo.TargetServer);
                    command.AddParameter("ServerName", serverInfo.ServerName);
                    command.AddParameter("BaseUrl", serverInfo.ServerUrl);
                    command.AddParameter("ActiveNum", serverInfo.ActiveNum);
                    command.AddParameter("EnableDate", serverInfo.EnableDate);
                    command.AddParameter("IntranetAddress", serverInfo.IntranetAddress);
                    command.Parser();
                    dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                    AddOrUpdate(key, tempServer);
                }
                else
                {
                    tempServer.TargetServer = serverInfo.TargetServer;
                    tempServer.ActiveNum = serverInfo.ActiveNum;
                    tempServer.Status = serverInfo.Status;
                }
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("AddToCache {0}_{1} error:{2}", ContainerKey, key, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="gameId"></param>
        /// <param name="serverName"></param>
        /// <param name="targetServer"></param>
        /// <param name="serverUrl"></param>
        /// <param name="status"></param>
        /// <param name="weight"></param>
        /// <param name="intranetAddress"></param>
        public void SetServer(int serverId, int gameId, string serverName, int targetServer, string serverUrl, string status, int weight, string intranetAddress)
        {
            string key = GenerateKey(gameId, serverId);
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Modify);
                command.AddParameter("ServerName", serverName);
                command.AddParameter("BaseUrl", serverUrl);
                command.AddParameter("IntranetAddress", intranetAddress);
                command.AddParameter("TargetServer", targetServer);
                if (weight > 0)
                {
                    command.AddParameter("Weight", weight);
                }
                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = string.Format("{0} AND {1}",
                    command.Filter.FormatExpression("ID"),
                    command.Filter.FormatExpression("GameID"));
                command.Filter.AddParam("ID", serverId);
                command.Filter.AddParam("GameID", gameId);
                command.Parser();
                dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);

                ServerInfo serverInfo;
                if (TryGet(key, out serverInfo))
                {
                    serverInfo.ServerName = serverName;
                    serverInfo.ServerUrl = serverUrl;
                    serverInfo.TargetServer = targetServer;
                    serverInfo.IntranetAddress = intranetAddress;
                    serverInfo.Status = status;
                    if (weight > 0)
                    {
                        serverInfo.Weight = weight;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SetServer {0}_{1} error:{2}", ContainerKey, key, ex);
            }
        }

        public void SetActiveNum(int serverId, int gameId, int activeNum)
        {
            string key = GenerateKey(gameId, serverId);
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Modify);
                command.AddParameter("ActiveNum", activeNum);

                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = string.Format("{0} AND {1}",
                    command.Filter.FormatExpression("ID"),
                    command.Filter.FormatExpression("GameID"));
                command.Filter.AddParam("ID", serverId);
                command.Filter.AddParam("GameID", gameId);
                command.Parser();
                dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                ServerInfo serverInfo;
                if (TryGet(key, out serverInfo))
                {
                    serverInfo.ActiveNum = activeNum;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SetActiveNum {0}_{1} error:{2}", ContainerKey, key, ex);
            }
        }

        /// <summary>
        /// 游戏客户端调用
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public List<ServerInfo> GetClientServerList(int gameId)
        {
            return GetServers(gameId, true, true);
        }

        /// <summary>
        /// 官网调用
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public List<ServerInfo> GetOfficialServerList(int gameId)
        {
            return GetServers(gameId, false, true);
        }

        public List<ServerInfo> GetServerList(int gameId, bool isSort)
        {
            return GetServers(gameId, isSort, false);
        }

        public List<ServerInfo> GetServers(int gameId, bool isSort, bool isEnable)
        {
            var list = FindAll(p => p.GameID == gameId && (!isEnable || p.IsEnable));
            if (isSort)
            {
                list.Sort();
            }
            return list;
        }

        public ServerInfo GetServers(int gameId, int serverId)
        {
            return Find(p => p.GameID == gameId && p.ID == serverId);
        }


        internal void RemoveServer(int gameId, int serverId)
        {
            string key = GenerateKey(gameId, serverId);
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Delete);
                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = string.Format("{0} AND {1}",
                    command.Filter.FormatExpression("ID"),
                    command.Filter.FormatExpression("GameID"));
                command.Filter.AddParam("ID", serverId);
                command.Filter.AddParam("GameID", gameId);
                command.Parser();
                dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                TryRemove(key);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("RemoveServer {0}_{1} error:{2}", ContainerKey, key, ex);
            }
        }

        internal bool ReloadServer(int gameID)
        {
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Inquiry);
                command.Columns = "ID,GameID,ServerName,BaseUrl,ActiveNum,Weight,isEnable,TargetServer,EnableDate,IntranetAddress";
                command.OrderBy = "GameID asc, ID asc";
                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = command.Filter.FormatExpression("GameID");
                command.Filter.AddParam("GameID", gameID);
                command.Parser();

                using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
                {
                    LoadServer(reader);
                }
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ReloadServer {0}_{1} error:{2}", ContainerKey, gameID, ex);
                return false;
            }
        }

        internal void EnableServer(int gameId, int serverId, bool isEnable)
        {
            string key = GenerateKey(gameId, serverId);
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Modify);
                command.AddParameter("IsEnable", isEnable);

                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = string.Format("{0} AND {1}",
                    command.Filter.FormatExpression("ID"),
                    command.Filter.FormatExpression("GameID"));
                command.Filter.AddParam("ID", serverId);
                command.Filter.AddParam("GameID", gameId);
                command.Parser();
                dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                ServerInfo serverInfo;
                if (TryGet(key, out serverInfo))
                {
                    serverInfo.IsEnable = isEnable;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("EnableServer {0}_{1} error:{2}", ContainerKey, key, ex);
            }
        }


        internal void ServerStatus(int gameId, int serverId, int Status)
        {
            string key = GenerateKey(gameId, serverId);
            try
            {
                ServerInfo serverInfo;
                if (TryGet(key, out serverInfo))
                {
                    serverInfo.Status = Status.ToString();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ServerStatus {0}_{1} error:{2}", ContainerKey, key, ex);
            }
        }

        public void SetServerEnableDate(int gameId, int serverId, DateTime enableDate)
        {
            string key = GenerateKey(gameId, serverId);
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("ServerInfo", CommandMode.Modify);
                command.AddParameter("EnableDate", enableDate);

                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = string.Format("{0} AND {1}",
                    command.Filter.FormatExpression("ID"),
                    command.Filter.FormatExpression("GameID"));
                command.Filter.AddParam("ID", serverId);
                command.Filter.AddParam("GameID", gameId);
                command.Parser();
                dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                ServerInfo serverInfo;
                if (TryGet(key, out serverInfo))
                {
                    serverInfo.EnableDate = enableDate;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("SetServerEnableDate {0}_{1} error:{2}", ContainerKey, key, ex);
            }

        }
    }
}