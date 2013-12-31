using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using ZyGames.Doudizhu.Bll.Base;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.Sockets;
using ZyGames.Framework.Script;

namespace ZyGames.Doudizhu.Bll
{
    public class GameHostApp : GameSocketHost
    {
        private static GameHostApp _instance;

        static GameHostApp()
        {
            _instance = new GameHostApp();
        }

        public static GameHostApp Current
        {
            get { return _instance; }
        }


        protected override void OnConnectCompleted(object sender, ConnectionEventArgs e)
        {
        }

        protected override void OnReceivedBefore(ConnectionEventArgs e)
        {
            //Console.WriteLine("Url:{0}", Encoding.ASCII.GetString(e.Data));
        }

        protected override void OnRequested(HttpGet httpGet, IGameResponse response)
        {
            try
            {
                var actionId = httpGet.ActionId;
                var uid = httpGet.GetString("uid");
                Console.WriteLine("Action{0} from {1} {2}", actionId, httpGet.RemoteAddress, uid);
                ActionFactory.Request(httpGet, response, GetUser);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("{0}", ex);
            }
        }

        protected override BaseUser GetUser(int userId)
        {
            return new GameDataCacheSet<GameUser>().FindKey(userId.ToNotNullString());
        }

        protected override void OnStartAffer()
        {
            try
            {
                var setting = new EnvironmentSetting();
                setting.ClientDesDeKey = "j6=9=1ac";
                setting.EntityAssembly = Assembly.Load("ZyGames.Doudizhu.Model");
                setting.ScriptStartBeforeHandle += () =>
                {
                    ScriptEngines.AddReferencedAssembly(new string[] {
                        "ZyGames.Doudizhu.Lang.dll",
                        "ZyGames.Doudizhu.Model.dll",
                        "ZyGames.Doudizhu.Bll.dll"
                    });
                    ActionFactory.SetActionIgnoreAuthorize(1012, 9001, 9203);

                    AppstoreClientManager.Current.InitConfig();
                    LoadUnlineUser();
                    InitRanking();
                };
                GameEnvironment.Start(setting);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnStartAffer error:{0}", ex);
            }
        }

        protected override void OnServiceStop()
        {
            GameEnvironment.Stop();
        }

        private void LoadUnlineUser()
        {
            TraceLog.ReleaseWrite("正在加载玩家数据...");
            List<string> userList = new List<string>();
            try
            {
                int loadUnlineDay = ConfigUtils.GetSetting("LoadUnlineDay", "1").ToInt();
                int maxCount = ConfigUtils.GetSetting("MaxLoadCount", "100").ToInt();
                var dbProvider = DbConnectionProvider.CreateDbProvider(DbConfig.Data);
                var command = dbProvider.CreateCommandStruct("GameUser", CommandMode.Inquiry);
                command.Columns = dbProvider.FormatQueryColumn(",", new string[] { "UserID" });
                command.ToIndex = maxCount;
                command.OrderBy = "LoginDate desc";
                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = dbProvider.FormatFilterParam("LoginDate", ">");
                var param = dbProvider.CreateParameter("LoginDate", DateTime.Now.Date.AddDays(-loadUnlineDay));
                command.Filter.AddParam(param);
                command.Parser();

                using (IDataReader reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
                {
                    while (reader.Read())
                    {
                        userList.Add(reader["UserID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("LoadUnlineUser:{0}", ex);
            }
            var cacheSet = new GameDataCacheSet<GameUser>();
            foreach (string userId in userList)
            {
                cacheSet.FindKey(userId);
            }
            TraceLog.ReleaseWrite("正在加载玩家结束");
        }

        /// <summary>
        /// 排行榜加载
        /// </summary>
        private static void InitRanking()
        {
            int timeOut = ConfigUtils.GetSetting("Ranking.timeout", "3600").ToInt();
            RankingFactory.Add(new BeansRanking());
            RankingFactory.Add(new WinRanking());

            RankingFactory.Start(timeOut);
        }
    }
}
