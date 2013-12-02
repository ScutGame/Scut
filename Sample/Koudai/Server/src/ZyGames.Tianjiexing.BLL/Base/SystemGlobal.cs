using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.Sql;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.SocketServer;
using ZyGames.Tianjiexing.BLL.Task;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 系统全局运行环境
    /// </summary>
    public static class SystemGlobal
    {
        private static readonly object thisLock = new object();
        private static int maxCount = ConfigUtils.GetSetting("MaxLoadCount", "100").ToInt();
        private const int LoadDay = 1;
        //private static Timer _timer;
        private static int cacheOverdueTime = ConfigUtils.GetSetting("CacheOverdueTime", "4").ToInt();
        private static bool _isRunning;

        public static bool IsRunning
        {
            get { return _isRunning; }
        }

        public static void CloseRunState()
        {
            lock (thisLock)
            {
                _isRunning = false;
            }
        }

        static SystemGlobal()
        {
            _isRunning = false;
            //long periodTime = 600 * 1000;
            //_timer = new Timer(RemoveUser, null, 60000, periodTime);
        }

        public static void Run()
        {
            AppstoreClientManager.Current.InitConfig();
            var dispatch = TaskDispatch.StartTask();
            dispatch.Add(new FightCombatTask());
            //dispatch.Start(););)

            lock (thisLock)
            {
                _isRunning = false;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            RegisterCacheSet();
            ConfigCacheGlobal.Load();
            LoadGlobalData();
            LoadUser();

            GameUser.Callback = new AsyncDataChangeCallback(UserHelper.TriggerUserCallback);
            //UserGeneral.EscalateHandle += UserHelper.TriggerGeneral;
            new GameActiveCenter(null);
            new GuildGameActiveCenter(null);

            InitRanking();
            stopwatch.Stop();
            new BaseLog().SaveLog("系统全局运行环境加载所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");

            lock (thisLock)
            {
                _isRunning = true;
            }
        }

        private static void InitRanking()
        {
            int timeOut = ConfigUtils.GetSetting("Ranking.timeout", "3600").ToInt();
            //RankingFactory.Add(new CombatNumRanking());
            //RankingFactory.Add(new GameCoinRanking());
            //RankingFactory.Add(new ObtainRanking());
            RankingFactory.Add(new UserLvRanking());
            RankingFactory.Add(new CombatRanking());
            RankingFactory.Add(new ShengJiTaRanking());
            //圣吉塔排行     
            //int intervalTimes = ConfigEnvSet.GetInt("Rank.SJT") / 3600;
            //RankingFactory.Add(new ShengJiTaRanking(intervalTimes));
            
            RankingFactory.Start(timeOut);
            var a = 1;
        }

        private static void RegisterCacheSet()
        {
            //todo 注释掉，由底层自动注册
        }

        private static void InitProtoBufType()
        {
            try
            {
                var assembly = Assembly.Load("ZyGames.Tianjiexing.Model");
                ProtoBufUtils.LoadProtobufType(assembly);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Init protobuf class error", ex);
            }
        }


        private static void LoadUser()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var userList = GetLoadUser(LoadDay, maxCount);
            new BaseLog().SaveLog("系统加载当天用户数:" + userList.Count + "/最大:" + maxCount);
            foreach (string userId in userList)
            {
                UserCacheGlobal.LoadOffline(userId);
            }
            stopwatch.Stop();
            new BaseLog().SaveLog("系统加载当天用户所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");
        }

        public static List<string> GetLoadUser(int days, int maxCount)
        {
            var dbProvider = DbConnectionProvider.CreateDbProvider(DbConfig.Data);

            var command = dbProvider.CreateCommandStruct("GameUser", CommandMode.Inquiry, "UserID");
            command.OrderBy = "LoginTime desc";
            command.Filter = dbProvider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("LoginTime", ">");
            command.Filter.AddParam("LoginTime", DateTime.Now.AddDays(-days));
            command.Parser();

            List<string> userList = new List<string>();
            using (IDataReader reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    userList.Add(reader["UserID"].ToString());
                }
            }
            return userList;
        }

        public static void LoadGlobalData()
        {
            int periodTime = GameEnvironment.CacheGlobalPeriod;
            new BaseLog().SaveLog("系统加载单服配置开始...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int capacity = int.MaxValue;
            //todo Load
            var dbFilter = new DbDataFilter(capacity);
            new ShareCacheStruct<ServerEnvSet>().AutoLoad(dbFilter);
            new ShareCacheStruct<GameNotice>().AutoLoad(dbFilter);
            new ShareCacheStruct<DailyRestrainSet>().AutoLoad(dbFilter);
            new ShareCacheStruct<UserFriends>().AutoLoad(dbFilter);
            new ShareCacheStruct<UserGuild>().AutoLoad(dbFilter);
            new ShareCacheStruct<GuildMember>().AutoLoad(dbFilter);
            new ShareCacheStruct<UserApply>().AutoLoad(dbFilter);
            new ShareCacheStruct<GuildIdol>().AutoLoad(dbFilter);
            new ShareCacheStruct<GuildMemberLog>().AutoLoad(dbFilter);
            new ShareCacheStruct<GameActive>().AutoLoad(dbFilter);
            new ShareCacheStruct<FestivalInfo>().AutoLoad(dbFilter);
            // new ShareCacheStruct<FestivalRestrain>().AutoLoad(dbFilter);
            new ShareCacheStruct<PetRunPool>().AutoLoad(dbFilter);
            new ShareCacheStruct<UserTakePrize>().AutoLoad(dbFilter);
            new ShareCacheStruct<ServerFight>().AutoLoad(dbFilter);
            new ShareCacheStruct<ServerFightGroup>().AutoLoad(dbFilter);
            new ShareCacheStruct<MemberGroup>().AutoLoad(dbFilter);
            //new ShareCacheStruct<UserMail>().AutoLoad(dbFilter);
            stopwatch.Stop();
            new BaseLog().SaveLog("系统加载单服配置所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");

        }

        //private static void LoadServerCombatUser()
        //{
        //    try
        //    {
        //        new BaseLog().SaveLog("系统跨服战用户加载开始...");
        //        Stopwatch stopwatch = new Stopwatch();
        //        stopwatch.Start();
        //        int periodTime = GameEnvironment.CacheGlobalPeriod;
        //        int capacity = int.MaxValue;
        //        int FastID = ServerCombatHelper.GetFastID();
        //        DbDataFilter filter = new DbDataFilter(capacity);
        //        filter.Condition = "FastID = @FastID";
        //        filter.Parameters.Add("@FastID", FastID);
        //        var cacheSet = new ShareCacheStruct<UserServerCombatApply>();
        //        cacheSet.AutoLoad(filter, periodTime, false);
        //        List<UserServerCombatApply> applyList = cacheSet.FindAll();
        //        new BaseLog().SaveLog(string.Format("系统跨服战加载用户数{0}", applyList.Count));
        //        foreach (var item in applyList)
        //        {
        //            UserCacheGlobal.LoadOffline(item.UserID);
        //        }
        //        stopwatch.Stop();
        //        new BaseLog().SaveLog(string.Format("系统跨服战用户加载时间:{0}ms", stopwatch.Elapsed.TotalMilliseconds));

        //    }
        //    catch (Exception ex)
        //    {
        //        new BaseLog().SaveLog("跨服战服务器出错", ex);
        //    }
        //}

        public static void Stop()
        {
            CountryCombat.Stop();
            GameActiveCenter.Stop();
            GuildGameActiveCenter.Stop();
        }
    }
}