using System;
using System.Data;
using ZyGames.GameCache;
using System.Data.SqlClient;
using ZyGames.Core.Data;
using System.Collections.Generic;
using ZyGames.DirCenter.Model;
using ZyGames.Common;

namespace ZyGames.DirCenter.CacheData
{
    public class CacheServer : BaseCache
    {
        class ServerList : Dictionary<int, ServerInfo>
        {
            public bool Contains(string serverName, out ServerInfo serverInfo)
            {
                serverInfo = new ServerInfo();
                foreach (KeyValuePair<int, ServerInfo> item in this)
                {
                    if (item.Value != null && item.Value.ServerName.Trim().Equals(serverName.Trim()))
                    {
                        serverInfo = item.Value;
                        return true;
                    }
                }
                return false;
            }
            public ServerInfo[] ToList(bool isSort)
            {
                return ToList(isSort, false);
            }

            public ServerInfo[] ToList(bool isSort, bool isEnable)
            {
                List<ServerInfo> list = new List<ServerInfo>();
                foreach (KeyValuePair<int, ServerInfo> item in this)
                {
                    //过滤未启用的 wuzf 2012-08-08
                    if (isEnable && !item.Value.IsEnable)
                    {
                        continue;
                    }
                    if (item.Value != null)
                    {
                        list.Add(item.Value);
                    }
                }
                if (isSort)
                {
                    list.Sort();
                }
                return list.ToArray();
            }
        }

        private static Object thisLock = new Object();
        private static readonly string CacheKey = "ServerDirConfig";

        public CacheServer()
            : base(CacheKey)
        {
            cachekey = CacheKey;
        }

        protected override bool InitCache()
        {
            try
            {
                string sql = "SELECT [ID],[GameID],[ServerName],[BaseUrl],[ActiveNum],Weight,isEnable,[TargetServer],EnableDate,IntranetAddress FROM ServerInfo ORDER BY GameID asc, ID asc";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(config.connectionString, CommandType.Text, sql))
                {
                    var serverDict = new Dictionary<int, ServerList>();
                    LoadServer(reader, serverDict);
                    addCache(serverDict);
                }

                return true;
            }
            catch (Exception ex)
            {
                SaveLog(ex);
                return false;
            }
        }

        private static void LoadServer(SqlDataReader reader, Dictionary<int, ServerList> serverDict)
        {
            while (reader.Read())
            {
                var servierInfo = new ServerInfo
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    GameID = Convert.ToInt32(reader["GameID"]),
                    TargetServer = Convert.ToInt32(reader["TargetServer"]),
                    ServerName = Convert.ToString(reader["ServerName"]),
                    ServerUrl = Convert.ToString(reader["BaseUrl"]),
                    ActiveNum = Convert.ToInt32(reader["ActiveNum"]),
                    Weight = ConvertHelper.ToInt(reader["Weight"]),
                    IsEnable = ConvertHelper.ToBool(reader["isEnable"]),
                    EnableDate = ConvertHelper.ToDateTime(reader["EnableDate"]),
                    IntranetAddress = Convert.ToString(reader["IntranetAddress"]),
                    Status = string.Empty
                };
                if (!string.IsNullOrEmpty(reader["ActiveNum"].ToString()))
                {
                    servierInfo.ActiveNum = Convert.ToInt32(reader["ActiveNum"].ToString());
                }
                if (serverDict.ContainsKey(servierInfo.GameID))
                {
                    serverDict[servierInfo.GameID].Add(servierInfo.ID, servierInfo);
                }
                else
                {
                    ServerList list = new ServerList();
                    list.Add(servierInfo.ID, servierInfo);
                    serverDict.Add(servierInfo.GameID, list);
                }
            }
        }


        public bool AddToCache(ServerInfo serverInfo)
        {
            try
            {
                lock (thisLock)
                {
                    Dictionary<int, ServerList> serverDict = (Dictionary<int, ServerList>)getCache();
                    ServerList serverList = new ServerList();
                    int gameId = serverInfo.GameID;
                    if (serverDict.ContainsKey(gameId))
                    {
                        serverList = serverDict[gameId];
                    }
                    //判断是否重名
                    ServerInfo tempServer;
                    if (!serverList.Contains(serverInfo.ServerName, out tempServer))
                    {
                        tempServer.ID = serverInfo.ID;
                        tempServer.GameID = serverInfo.GameID;
                        tempServer.TargetServer = serverInfo.TargetServer;
                        tempServer.ServerName = serverInfo.ServerName;
                        tempServer.ServerUrl = serverInfo.ServerUrl;
                        tempServer.ActiveNum = serverInfo.ActiveNum;
                        tempServer.Status = serverInfo.Status;
                        tempServer.EnableDate = serverInfo.EnableDate;
                        tempServer.IntranetAddress = serverInfo.IntranetAddress;

                        CommandHelper command = new CommandHelper("ServerInfo", EditType.Insert);
                        command.AddParameter("ID", SqlDbType.Int, serverInfo.ID);
                        command.AddParameter("GameID", SqlDbType.Int, serverInfo.GameID);
                        command.AddParameter("TargetServer", SqlDbType.Int, serverInfo.TargetServer);
                        command.AddParameter("ServerName", SqlDbType.VarChar, serverInfo.ServerName);
                        command.AddParameter("BaseUrl", SqlDbType.VarChar, serverInfo.ServerUrl);
                        command.AddParameter("ActiveNum", SqlDbType.Int, serverInfo.ActiveNum);
                        command.AddParameter("EnableDate", SqlDbType.DateTime, serverInfo.EnableDate);
                        command.AddParameter("IntranetAddress", SqlDbType.VarChar, serverInfo.IntranetAddress);
                        command.Parser();

                        SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);

                        serverList.Add(tempServer.ID, tempServer);
                    }
                    else
                    {
                        tempServer.TargetServer = serverInfo.TargetServer;
                        tempServer.ActiveNum = serverInfo.ActiveNum;
                        tempServer.Status = serverInfo.Status;
                        serverList[tempServer.ID] = tempServer;
                    }
                    if (serverDict.ContainsKey(gameId))
                    {
                        serverDict[gameId] = serverList;
                    }
                    else
                    {
                        serverDict.Add(gameId, serverList);
                    }
                    addCache(serverDict);
                }
                return true;
            }
            catch (Exception ex)
            {
                SaveLog(ex);
                return false;
            }
        }

        public void SetServer(int serverId, int gameId, string serverName, int TargetServer, string serverUrl, string status, int weight, string intranetAddress)
        {
            try
            {
                CommandHelper command = new CommandHelper("ServerInfo", EditType.Update);
                command.AddParameter("ServerName", SqlDbType.VarChar, serverName);
                command.AddParameter("BaseUrl", SqlDbType.VarChar, serverUrl);
                command.AddParameter("IntranetAddress", SqlDbType.VarChar, intranetAddress);
                command.AddParameter("TargetServer", SqlDbType.Int, TargetServer);
                if (weight > 0)
                {
                    command.AddParameter("Weight", SqlDbType.Int, weight);
                }
                //command.AddParameter("ActiveNum", SqlDbType.Int, serverInfo.ActiveNum);
                command.Filter = new CommandFilter();
                command.Filter.Condition = "ID=@ServerID and GameID=@GameID";
                command.Filter.AddParam("@ServerID", SqlDbType.Int, 0, serverId);
                command.Filter.AddParam("@GameID", SqlDbType.Int, 0, gameId);
                command.Parser();
                SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);


                lock (thisLock)
                {
                    Dictionary<int, ServerList> serverDict = (Dictionary<int, ServerList>)getCache();
                    if (serverDict.ContainsKey(gameId))
                    {
                        ServerList serverList = serverDict[gameId];
                        if (serverList.ContainsKey(serverId))
                        {
                            ServerInfo cacheServer = serverList[serverId];
                            cacheServer.ServerName = serverName;
                            cacheServer.ServerUrl = serverUrl;
                            cacheServer.TargetServer = TargetServer;
                            cacheServer.IntranetAddress = intranetAddress;
                            cacheServer.Status = status;
                            if (weight > 0)
                            {
                                cacheServer.Weight = weight;
                            }
                            serverList[serverId] = cacheServer;
                        }
                        serverDict[gameId] = serverList;
                    }
                    addCache(serverDict);
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }

        public void SetActiveNum(int serverId, int gameId, int activeNum)
        {
            try
            {
                CommandHelper command = new CommandHelper("ServerInfo", EditType.Update);
                command.AddParameter("ActiveNum", SqlDbType.Int, activeNum);
                command.Filter = new CommandFilter();
                command.Filter.Condition = "ID=@ServerID and GameID=@GameID";
                command.Filter.AddParam("@ServerID", SqlDbType.Int, 0, serverId);
                command.Filter.AddParam("@GameID", SqlDbType.Int, 0, gameId);
                command.Parser();
                SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);

                lock (thisLock)
                {
                    var serverDict = (Dictionary<int, ServerList>)getCache();
                    if (serverDict.ContainsKey(gameId))
                    {
                        ServerList serverList = serverDict[gameId];
                        if (serverList.ContainsKey(serverId))
                        {
                            ServerInfo cacheServer = serverList[serverId];
                            cacheServer.ActiveNum = activeNum;
                            serverList[serverId] = cacheServer;
                        }
                        serverDict[gameId] = serverList;
                    }
                    addCache(serverDict);
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }

        /// <summary>
        /// 游戏客户端调用
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public ServerInfo[] GetClientServerList(int gameId)
        {
            return GetServers(gameId, true, true);
        }

        /// <summary>
        /// 官网调用
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public ServerInfo[] GetOfficialServerList(int gameId)
        {
            return GetServers(gameId, false, true);
        }

        public ServerInfo[] GetServerList(int gameId, bool isSort)
        {
            return GetServers(gameId, isSort, false);
        }

        public ServerInfo[] GetServers(int gameId, bool isSort, bool isEnable)
        {
            var serverDict = (Dictionary<int, ServerList>)getCache();
            if (serverDict.ContainsKey(gameId))
            {
                return serverDict[gameId].ToList(isSort, isEnable);
            }
            return new ServerInfo[0];
        }

        /// <summary>
        /// 获取游戏名称孙德尧 2012-5-23
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public string GetServerName(int gameId, int serverId)
        {
            var serverDict = (Dictionary<int, ServerList>)getCache();
            if (serverDict.ContainsKey(gameId))
            {
                var serverList = serverDict[gameId];
                if (serverList.ContainsKey(serverId)) return serverList[serverId].ServerName;
            }
            return null;
        }


        internal void RemoveServer(int gameId, int serverId)
        {
            try
            {
                CommandHelper command = new CommandHelper("ServerInfo", EditType.Delete);
                command.Filter = new CommandFilter();
                command.Filter.Condition = "ID=@ServerID and GameID=@GameID";
                command.Filter.AddParam("@ServerID", SqlDbType.Int, 0, serverId);
                command.Filter.AddParam("@GameID", SqlDbType.Int, 0, gameId);
                command.Parser();
                SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);
                lock (thisLock)
                {
                    var serverDict = (Dictionary<int, ServerList>)getCache();
                    if (serverDict.ContainsKey(gameId))
                    {
                        ServerList serverList = serverDict[gameId];
                        if (serverList.ContainsKey(serverId))
                        {
                            serverList.Remove(serverId);
                        }
                        serverDict[gameId] = serverList;
                    }
                    addCache(serverDict);
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }

        internal bool ReloadServer(int gameID)
        {
            try
            {
                lock (thisLock)
                {
                    var serverDict = (Dictionary<int, ServerList>)getCache();
                    if (serverDict.ContainsKey(gameID))
                    {
                        serverDict.Remove(gameID);
                    }

                    string sql = "SELECT [ID],[GameID],[ServerName],[BaseUrl],[ActiveNum],[Weight],isEnable,[TargetServer],EnableDate,IntranetAddress FROM ServerInfo where GameID=@GameID ORDER BY GameID asc,ID asc";
                    SqlParameter[] paramList = new[]
                    {
                        SqlParamHelper.MakeInParam("@GameID", SqlDbType.Int, 0, gameID)
                    };
                    using (SqlDataReader reader = SqlHelper.ExecuteReader(config.connectionString, CommandType.Text, sql, paramList))
                    {
                        LoadServer(reader, serverDict);
                        addCache(serverDict);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SaveLog(ex);
                return false;
            }
        }

        internal void EnableServer(int gameId, int serverId, bool isEnable)
        {
            try
            {
                CommandHelper command = new CommandHelper("ServerInfo", EditType.Update);
                command.AddParameter("IsEnable", SqlDbType.Bit, isEnable);
                command.Filter = new CommandFilter();
                command.Filter.Condition = "ID=@ServerID and GameID=@GameID";
                command.Filter.AddParam("@ServerID", SqlDbType.Int, 0, serverId);
                command.Filter.AddParam("@GameID", SqlDbType.Int, 0, gameId);
                command.Parser();
                SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);
                lock (thisLock)
                {
                    var serverDict = (Dictionary<int, ServerList>)getCache();
                    if (serverDict.ContainsKey(gameId))
                    {
                        ServerList serverList = serverDict[gameId];
                        if (serverList.ContainsKey(serverId))
                        {
                            ServerInfo cacheServer = serverList[serverId];
                            cacheServer.IsEnable = isEnable;
                            serverList[serverId] = cacheServer;
                        }
                        serverDict[gameId] = serverList;
                    }
                    addCache(serverDict);
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }


        internal void ServerStatus(int gameId, int serverId, int Status)
        {
            try
            {
               
                lock (thisLock)
                {
                    var serverDict = (Dictionary<int, ServerList>)getCache();
                    if (serverDict.ContainsKey(gameId))
                    {
                        ServerList serverList = serverDict[gameId];
                        if (serverList.ContainsKey(serverId))
                        {
                            ServerInfo cacheServer = serverList[serverId];
                            cacheServer.Status = Status.ToString();
                            serverList[serverId] = cacheServer;
                        }
                        serverDict[gameId] = serverList;
                    }
                    addCache(serverDict);
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }

        public void SetServerEnableDate(int gameId, int serverId, DateTime enableDate)
        {
            try
            {
                CommandHelper command = new CommandHelper("ServerInfo", EditType.Update);
                command.AddParameter("EnableDate", SqlDbType.DateTime, enableDate);
                command.Filter = new CommandFilter();
                command.Filter.Condition = "ID=@ServerID and GameID=@GameID";
                command.Filter.AddParam("@ServerID", SqlDbType.Int, 0, serverId);
                command.Filter.AddParam("@GameID", SqlDbType.Int, 0, gameId);
                command.Parser();
                SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);
                lock (thisLock)
                {
                    var serverDict = (Dictionary<int, ServerList>)getCache();
                    if (serverDict.ContainsKey(gameId))
                    {
                        ServerList serverList = serverDict[gameId];
                        if (serverList.ContainsKey(serverId))
                        {
                            ServerInfo cacheServer = serverList[serverId];
                            cacheServer.EnableDate = enableDate;
                            serverList[serverId] = cacheServer;
                        }
                        serverDict[gameId] = serverList;
                    }
                    addCache(serverDict);
                }
            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }
    }
}
