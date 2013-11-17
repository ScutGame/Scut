using System;
using System.Data;
using System.Collections;
using ZyGames.GameCache;
using System.Data.SqlClient;
using ZyGames.Core.Data;
using System.Collections.Generic;
using ZyGames.DirCenter.Model;
using ZyGames.Common;

namespace ZyGames.DirCenter.CacheData
{
    public class CacheGame : BaseCache
    {
        private static Object thisLock = new Object();
        private static readonly string CacheKey = "GameConfig";

        public CacheGame()
            : base(CacheKey)
        {
            this.cachekey = CacheKey;
        }

        protected override bool InitCache()
        {
            try
            {
                Dictionary<int, GameInfo> list = new Dictionary<int, GameInfo>();
                string sql = "SELECT [GameID],[GameName],[Currency],[Multiple],[GameWord],[AgentsID],[IsRelease],[ReleaseDate],[PayStyle],[SocketServer],[SocketPort] FROM GameInfo ORDER BY GameID";
                using (SqlDataReader reader = SqlHelper.ExecuteReader(config.connectionString, CommandType.Text, sql))
                {
                    while (reader.Read())
                    {
                        var gameInfo = new GameInfo()
                        {
                            ID = ConvertHelper.ToInt(reader["GameID"]),
                            Name = ConvertHelper.ToString(reader["GameName"]),
                            Currency = ConvertHelper.ToString(reader["Currency"]),
                            Multiple = ConvertHelper.ToDecimal(reader["Multiple"]),
                            AgentsID = ConvertHelper.ToString(reader["AgentsID"]),
                            IsRelease = ConvertHelper.ToBool(reader["IsRelease"]),
                            ReleaseDate = ConvertHelper.ToDateTime(reader["ReleaseDate"]),
                            PayStyle = ConvertHelper.ToString(reader["PayStyle"]),
                            GameWord = Convert.ToString(reader["GameWord"]),
                            SocketServer = ConvertHelper.ToString(reader["SocketServer"]),
                            SocketPort = Convert.ToInt32(reader["SocketPort"])
                        };
                        list.Add(gameInfo.ID, gameInfo);
                    }
                }

                this.addCache(list);
                return true;
            }
            catch (Exception ex)
            {
                this.SaveLog(ex);
                return false;
            }
        }

        public bool AddToCache(GameInfo gameInfo)
        {
            lock (thisLock)
            {
                try
                {
                    Dictionary<int, GameInfo> list = (Dictionary<int, GameInfo>)this.getCache();
                    if (!list.ContainsKey(gameInfo.ID))
                    {
                        CommandHelper command = new CommandHelper("GameInfo", EditType.Insert);
                        command.AddParameter("GameID", SqlDbType.Int, gameInfo.ID);
                        command.AddParameter("GameName", SqlDbType.VarChar, gameInfo.Name);
                        command.AddParameter("Currency", SqlDbType.VarChar, gameInfo.Currency);
                        command.AddParameter("Multiple", SqlDbType.Decimal, gameInfo.Multiple);
                        command.AddParameter("AgentsID", SqlDbType.VarChar, gameInfo.AgentsID);
                        command.AddParameter("IsRelease", SqlDbType.Bit, gameInfo.IsRelease);
                        command.AddParameter("ReleaseDate", SqlDbType.DateTime, gameInfo.ReleaseDate);
                        command.AddParameter("PayStyle", SqlDbType.VarChar, gameInfo.PayStyle);
                        command.AddParameter("GameWord", SqlDbType.VarChar, gameInfo.GameWord);
                        command.AddParameter("SocketServer", SqlDbType.VarChar, gameInfo.SocketServer);
                        command.AddParameter("SocketPort", SqlDbType.Int, gameInfo.SocketPort);
                        command.Parser();
                        SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);

                        list.Add(gameInfo.ID, gameInfo);
                    }
                    else
                    {
                        CommandHelper command = new CommandHelper("GameInfo", EditType.Update);
                        command.AddParameter("GameName", SqlDbType.VarChar, gameInfo.Name);
                        command.AddParameter("Currency", SqlDbType.VarChar, gameInfo.Currency);
                        command.AddParameter("Multiple", SqlDbType.Decimal, gameInfo.Multiple);
                        command.AddParameter("AgentsID", SqlDbType.VarChar, gameInfo.AgentsID);
                        command.AddParameter("IsRelease", SqlDbType.Bit, gameInfo.IsRelease);
                        command.AddParameter("ReleaseDate", SqlDbType.DateTime, gameInfo.ReleaseDate);
                        command.AddParameter("PayStyle", SqlDbType.VarChar, gameInfo.PayStyle);
                        command.AddParameter("GameWord", SqlDbType.VarChar, gameInfo.GameWord);
                        command.AddParameter("SocketServer", SqlDbType.VarChar, gameInfo.SocketServer);
                        command.AddParameter("SocketPort", SqlDbType.Int, gameInfo.SocketPort);
                        command.Filter = new CommandFilter();
                        command.Filter.Condition = "GameID=@GameID";
                        command.Filter.AddParam("@GameID", SqlDbType.Int, 0, gameInfo.ID);
                        command.Parser();
                        SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);

                        list[gameInfo.ID] = gameInfo;
                        return true;
                    }
                    this.addCache(list);
                    return true;
                }
                catch (Exception ex)
                {
                    this.SaveLog(ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取游戏列表
        /// </summary>
        /// <returns></returns>
        public GameInfo[] GetGame()
        {
            List<GameInfo> gameArr = new List<GameInfo>();
            Dictionary<int, GameInfo> list = (Dictionary<int, GameInfo>)this.getCache();
            foreach (KeyValuePair<int, GameInfo> keyPair in list)
            {
                gameArr.Add(keyPair.Value as GameInfo);
            }
            return gameArr.ToArray();
        }

        //孙德尧 2012-5-23
        /// <summary>
        /// 获取游戏名称
        /// </summary>
        /// <returns></returns>
        public string GetGameName(int gameID)
        {
            List<GameInfo> gameArr = new List<GameInfo>();
            Dictionary<int, GameInfo> list = (Dictionary<int, GameInfo>)this.getCache();
            foreach (KeyValuePair<int, GameInfo> keyPair in list)
            {
                if ((keyPair.Value as GameInfo).ID == gameID)
                {
                    return (keyPair.Value as GameInfo).Name;
                }

            }
            return null;
        }

        /// <summary>
        /// 获取游戏
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public GameInfo GetGame(int gameID)
        {
            Dictionary<int, GameInfo> list = (Dictionary<int, GameInfo>)this.getCache();
            return list[gameID] as GameInfo;
        }


        internal void RemoveGame(int gameID)
        {
            try
            {
                CommandHelper command = new CommandHelper("GameInfo", EditType.Delete);
                command.Filter = new CommandFilter();
                command.Filter.Condition = "GameID=@GameID";
                command.Filter.AddParam("@GameID", SqlDbType.Int, 0, gameID);
                command.Parser();
                SqlHelper.ExecuteNonQuery(config.connectionString, CommandType.Text, command.Sql, command.Parameters);
                Dictionary<int, GameInfo> list = (Dictionary<int, GameInfo>)this.getCache();
                lock (thisLock)
                {
                    if (list.ContainsKey(gameID))
                    {
                        list.Remove(gameID);
                    }
                    this.addCache(list);
                }
            }
            catch (Exception ex)
            {
                this.SaveLog(ex);
            }
        }

        /// <summary>
        /// 官方游戏列表
        /// </summary>
        /// <returns></returns>
        internal GameInfo[] GetOfficialGame()
        {
            List<GameInfo> gameArr = new List<GameInfo>();
            Dictionary<int, GameInfo> list = (Dictionary<int, GameInfo>)this.getCache();
            foreach (KeyValuePair<int, GameInfo> keyPair in list)
            {
                var gameInfo = keyPair.Value as GameInfo;
                if (gameInfo.IsRelease && (string.IsNullOrEmpty(gameInfo.AgentsID) || "0000".Equals(gameInfo.AgentsID)))
                {
                    gameArr.Add(gameInfo);
                }
            }
            return gameArr.ToArray();
        }

    }
}
