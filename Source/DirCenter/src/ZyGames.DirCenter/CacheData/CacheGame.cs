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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;
using System.Collections.Generic;
using ZyGames.DirCenter.Model;

namespace ZyGames.DirCenter.CacheData
{
    public class CacheGame : MemoryCacheStruct<GameInfo>
    {
        protected override bool InitCache()
        {
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("GameInfo", CommandMode.Inquiry);
                command.Columns = "GameID,GameName,Currency,Multiple,GameWord,AgentsID,IsRelease,ReleaseDate,PayStyle,SocketServer,SocketPort";
                command.OrderBy = "GameID";
                command.Parser();
                using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
                {
                    while (reader.Read())
                    {
                        var gameInfo = new GameInfo()
                        {
                            ID = MathUtils.ToInt(reader["GameID"]),
                            Name = MathUtils.ToNotNullString(reader["GameName"]),
                            Currency = MathUtils.ToNotNullString(reader["Currency"]),
                            Multiple = MathUtils.ToDecimal(reader["Multiple"]),
                            AgentsID = MathUtils.ToNotNullString(reader["AgentsID"]),
                            IsRelease = MathUtils.ToBool(reader["IsRelease"]),
                            ReleaseDate = MathUtils.ToDateTime(reader["ReleaseDate"]),
                            PayStyle = MathUtils.ToNotNullString(reader["PayStyle"]),
                            GameWord = MathUtils.ToNotNullString(reader["GameWord"]),
                            SocketServer = MathUtils.ToNotNullString(reader["SocketServer"]),
                            SocketPort = MathUtils.ToInt(reader["SocketPort"])
                        };
                        AddOrUpdate(gameInfo.ID.ToString(), gameInfo);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("InitCache {0} error:{1}", ContainerKey, ex);
                return true;
            }
        }

        public bool AddToCache(GameInfo gameInfo)
        {

            var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
            string key = gameInfo.ID.ToString();
            try
            {
                GameInfo info;
                if (!TryGet(key, out info))
                {
                    var command = dbProvider.CreateCommandStruct("GameInfo", CommandMode.Insert);
                    command.AddParameter("GameID", gameInfo.ID);
                    command.AddParameter("GameName", gameInfo.Name);
                    command.AddParameter("Currency", gameInfo.Currency);
                    command.AddParameter("Multiple", gameInfo.Multiple);
                    command.AddParameter("AgentsID", gameInfo.AgentsID);
                    command.AddParameter("IsRelease", gameInfo.IsRelease);
                    command.AddParameter("ReleaseDate", gameInfo.ReleaseDate);
                    command.AddParameter("PayStyle", gameInfo.PayStyle);
                    command.AddParameter("GameWord", gameInfo.GameWord);
                    command.AddParameter("SocketServer", gameInfo.SocketServer);
                    command.AddParameter("SocketPort", gameInfo.SocketPort);
                    command.Parser();
                    dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);

                    TryAdd(key, gameInfo);
                }
                else
                {
                    var command = dbProvider.CreateCommandStruct("GameInfo", CommandMode.Modify);
                    command.AddParameter("GameName", gameInfo.Name);
                    command.AddParameter("Currency", gameInfo.Currency);
                    command.AddParameter("Multiple", gameInfo.Multiple);
                    command.AddParameter("AgentsID", gameInfo.AgentsID);
                    command.AddParameter("IsRelease", gameInfo.IsRelease);
                    command.AddParameter("ReleaseDate", gameInfo.ReleaseDate);
                    command.AddParameter("PayStyle", gameInfo.PayStyle);
                    command.AddParameter("GameWord", gameInfo.GameWord);
                    command.AddParameter("SocketServer", gameInfo.SocketServer);
                    command.AddParameter("SocketPort", gameInfo.SocketPort);
                    command.Filter = dbProvider.CreateCommandFilter();
                    command.Filter.Condition = dbProvider.FormatFilterParam("GameID");
                    command.Filter.AddParam("GameID", gameInfo.ID);
                    command.Parser();
                    dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);

                    info.Name = gameInfo.Name;
                    info.Currency = gameInfo.Currency;
                    info.Multiple = gameInfo.Multiple;
                    info.AgentsID = gameInfo.AgentsID;
                    info.IsRelease = gameInfo.IsRelease;
                    info.ReleaseDate = gameInfo.ReleaseDate;
                    info.PayStyle = gameInfo.PayStyle;
                    info.GameWord = gameInfo.GameWord;
                    info.SocketServer = gameInfo.SocketServer;
                    info.SocketPort = gameInfo.SocketPort;
                }
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("AddToCache {0} error:{1}", ContainerKey, ex);
            }
            return false;
        }

        /// <summary>
        /// 获取游戏列表
        /// </summary>
        /// <returns></returns>
        public List<GameInfo> GetGame()
        {
            return FindAll(p => true);
        }

        /// <summary>
        /// 获取游戏名称
        /// </summary>
        /// <returns></returns>
        public string GetGameName(int gameID)
        {
            GameInfo gameInfo;
            return TryGet(gameID.ToString(), out gameInfo) ? gameInfo.Name : null;
        }

        /// <summary>
        /// 获取游戏
        /// </summary>
        /// <param name="gameID"></param>
        /// <returns></returns>
        public GameInfo GetGame(int gameID)
        {
            GameInfo gameInfo;
            if (TryGet(gameID.ToString(), out gameInfo))
            {
                return gameInfo;
            }
            return null;
        }


        internal void RemoveGame(int gameID)
        {
            try
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider("DirData");
                var command = dbProvider.CreateCommandStruct("GameInfo", CommandMode.Delete);
                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = dbProvider.FormatFilterParam("GameID");
                command.Filter.AddParam("GameID", gameID);
                command.Parser();
                dbProvider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                TryRemove(gameID.ToString());
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("RemoveGame {0}_{1} error:{2}", ContainerKey, gameID, ex);
            }
        }

        /// <summary>
        /// 官方游戏列表
        /// </summary>
        /// <returns></returns>
        internal List<GameInfo> GetOfficialGame()
        {
            return FindAll(g => g.IsRelease && (string.IsNullOrEmpty(g.AgentsID) || "0000".Equals(g.AgentsID)));
        }

    }
}