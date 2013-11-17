using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Caching;
using ZyGames.Doudizhu.Bll.Base;
using ZyGames.Doudizhu.Bll.Com.Chat;
using ZyGames.Doudizhu.Bll.Logic;
using ZyGames.Doudizhu.Lang;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.Sql;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Script;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Plugin.PythonScript;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

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
                var actionId = httpGet.GetString("ActionID").ToInt();
                var uid = httpGet.GetString("uid");
                Console.WriteLine("Action{0} from {1} {2}", actionId, httpGet.RemoteAddress, uid);
                ActionFactory.Request(httpGet, response, userId => new GameDataCacheSet<GameUser>().FindKey(userId.ToNotNullString()));

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("{0}", ex);
            }
        }

        protected override void OnStartAffer()
        {
            //时间间隔更新库
            int cacheInterval = 600;
            BaseLog log = null;
            try
            {
                GameEnvironment.ClientDesDeKey = "j6=9=1ac";
                log = new BaseLog();
                var assembly = Assembly.Load("ZyGames.Doudizhu.Model");
                GameEnvironment.Start(cacheInterval, () =>
                {
                    PythonContext pythonContext;
                    PythonScriptManager.Current.TryLoadPython(@"Lib/action.py", out pythonContext);
                    PythonScriptManager.Current.TryLoadPython(@"Lib/lang.py", out pythonContext);
                    PythonScriptManager.Current.TryLoadPython(@"Logic/cardAILogic.py", out pythonContext);

                    AppstoreClientManager.Current.InitConfig();
                    RouteItem routeItem;
                    PythonScriptManager.Current.TryGetAction(1008, out routeItem);
                    PythonScriptManager.Current.TryGetAction(9202, out routeItem);
                    LoadUnlineUser();
                    InitRanking();

                    ////todo 广播
                    //var list = new ShareCacheStruct<GameNotice>().FindAll(
                    //    s => (s.IsBroadcast &&
                    //            (s.ExpiryDate <= MathUtils.SqlMinDate) ||
                    //                (s.ExpiryDate > MathUtils.SqlMinDate && s.ExpiryDate >= DateTime.Now))
                    //          );
                    //foreach (var notice in list)
                    //{
                    //    DdzBroadcastService.Send(notice.Content);
                    //}
                    return true;
                }, 600, assembly);

                //todo test
                //CacheFactory.RemoveToDatabase("ZyGames.Doudizhu.Model.UserNickName_1380003");
                //UserNickName u = new ShareCacheStruct<UserNickName>().FindKey(1380003);
                //if (u == null)
                //{

                //}
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.SaveLog(ex);
                }
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
                command.Filter.Condition = dbProvider.FormatFilterParam("LoginDate", "LoginDate").Replace("=", ">");
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
