using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Config;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.MySql;
using ZyGames.Framework.Data.Sql;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Profile
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum OperateMode
    {
        /// <summary>
        /// 
        /// </summary>
        Add = 1,
        /// <summary>
        /// 
        /// </summary>
        Modify = 2,
        /// <summary>
        /// 
        /// </summary>
        Remove = 4,
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ProfileStorageMode
    {
        /// <summary>
        /// 
        /// </summary>
        File = 1,
        /// <summary>
        /// 
        /// </summary>
        Sql,
        /// <summary>
        /// 
        /// </summary>
        MySql
    }

    /// <summary>
    /// Profile manager, moniter Action assess and MQ io
    /// </summary>
    public static class ProfileManager
    {
        private static long _counter;
        private static SyncTimer _collectTimer;
        private static SyncTimer _operateTimer;
        private static DateTime _refreshTime;
        private static ProfileSummary _summary;
        private static string ConnectKey = "profileLog";
        private static string MessageQueueTableName = "MessageQueue";
        private static string[] MessageQueueColumns = new[]
        {
            "Id",
            "Time",
            "TotalAutoChanged","AutoChangedCount","AutoChangedPerSecond",
            "TotalPost","TotalPostObject","PostCount","PostObjectCount","PostPerSecond",
            "TotalProcess","TotalProcessObject","ProcessCount","ProcessObjectCount","ProcessPerSecond",
            "NoProcessKey",
            "TotalPostSql","TotalProcessSql","TotalFailSql","WaitSyncEntity","PostSql","ProcessSql","ProcessSqlPerSecond" //sql
        };
        private static StringBuilder _operateLog = new StringBuilder();
        private static readonly object rootSync = new object();
        private const int ColumnScale = 2;

        /// <summary>
        /// Enity object time of MQ IO, [key]: entity type name
        /// </summary>
        private static DictionaryExtend<string, EntityProfileCollection> _entityObjectCollection;
        //收集每个对象更新的次数
        private static DictionaryExtend<string, SqlProfileCollection> _sqlCollection;

        static ProfileManager()
        {
            _summary = new ProfileSummary();
            _entityObjectCollection = new DictionaryExtend<string, EntityProfileCollection>();
            _sqlCollection = new DictionaryExtend<string, SqlProfileCollection>();
        }

        /// <summary>
        /// Moniter is running
        /// </summary>
        public static bool IsEnable { get { return GetSection().ProfileEnableCollect; } }

        /// <summary>
        /// Open trace log
        /// </summary>
        public static bool IsOpenWriteLog { get { return GetSection().ProfileEnableCollect; } }
        /// <summary>
        /// 
        /// </summary>
        public static string WorkingLogPath { get { return GetSection().ProfileLogPath; } }
        /// <summary>
        /// 
        /// </summary>
        public static ProfileStorageMode StorageMode { get; private set; }

        private static ProfileSection GetSection()
        {
            return ConfigManager.Configger.GetFirstOrAddConfig<ProfileSection>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            _refreshTime = DateTime.Now;
            int intervalSecond = GetSection().ProfileCollectInterval;

            _operateTimer = new SyncTimer(obj => WriteToStorage(), 1000, 1000);
            _operateTimer.Start();
            _collectTimer = new SyncTimer(obj => Collect(), 100, intervalSecond * 1000);
            _collectTimer.Start();
            StorageMode = ProfileStorageMode.File;

            if (WorkingLogPath.ToLower().StartsWith("mysql:"))
            {
                StorageMode = ProfileStorageMode.MySql;
                CheckLogTable(typeof(MySqlDataProvider).Name, WorkingLogPath.Substring(6));
            }
            else if (WorkingLogPath.ToLower().StartsWith("sql:"))
            {
                StorageMode = ProfileStorageMode.Sql;
                CheckLogTable(typeof(SqlDataProvider).Name, WorkingLogPath.Substring(4));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            _collectTimer.Stop();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="key"></param>
        public static void ChangeEntityByAutoOfMessageQueueTimes(string typeName, string key)
        {
            if (!IsEnable) return;
            var obj = _entityObjectCollection.GetOrAdd(typeName, t => new EntityProfileCollection());
            obj.ChangeAutoTimes.Countor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="key">entity key</param>
        /// <param name="mode"></param>
        public static void PostEntityOfMessageQueueTimes(string typeName, string key, OperateMode mode)
        {
            if (!IsEnable) return;
            var obj = _entityObjectCollection.GetOrAdd(typeName, t => new EntityProfileCollection());
            AppendOperateLog(MessageQueueTableName + "#Post", typeName, mode, key);
            obj.PostTimes.Countor();
            obj.PostKeyCountor(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="keys"></param>
        /// <param name="mode"></param>
        public static void ProcessEntityOfMessageQueueTimes(string typeName, IEnumerable<string> keys, OperateMode mode)
        {
            if (!IsEnable) return;
            var obj = _entityObjectCollection.GetOrAdd(typeName, t => new EntityProfileCollection());
            foreach (var key in keys)
            {
                AppendOperateLog(MessageQueueTableName + "#Process", typeName, mode, key);
                obj.ProcessTimes.Countor();
                obj.ProcessKeyCountor(key);
            }
        }

        /// <summary>
        /// 收集Sql执行失败计数
        /// </summary>
        public static void ProcessFailSqlOfMessageQueueTimes(string tableName, int count)
        {
            if (!IsEnable) return;
            var obj = _sqlCollection.GetOrAdd(tableName ?? "unknow", t => new SqlProfileCollection());
            Interlocked.Add(ref obj.TotalFailCount, count);
        }

        /// <summary>
        /// 收集Sql提交计数
        /// </summary>
        public static void PostSqlOfMessageQueueTimes(string tableName, int count)
        {
            if (!IsEnable) return;
            var obj = _sqlCollection.GetOrAdd(tableName ?? "unknow", t => new SqlProfileCollection());
            obj.PostTimes.Countor(count);
        }

        /// <summary>
        /// 收集Sql执行计数
        /// </summary>
        public static void ProcessSqlOfMessageQueueTimes(string tableName)
        {
            if (!IsEnable) return;
            var obj = _sqlCollection.GetOrAdd(tableName ?? "unknow", t => new SqlProfileCollection());
            obj.ProcessTimes.Countor();
        }

        /// <summary>
        /// 收集Sql等待同步计数
        /// </summary>
        public static void WaitSyncSqlOfMessageQueueTimes(string tableName, int count)
        {
            if (!IsEnable) return;
            var obj = _sqlCollection.GetOrAdd(tableName ?? "unknow", t => new SqlProfileCollection());
            Interlocked.Add(ref obj.WaitSyncCount, count);
        }

        /// <summary>
        /// 
        /// </summary>
        public static ProfileSummary Collect()
        {
            if (!IsEnable) return null;

            Interlocked.Increment(ref _counter);
            var summary = new ProfileSummary();
            summary.Id = _counter;
            summary.IntervalSecond = DateTime.Now.Subtract(_refreshTime).TotalSeconds;
            _refreshTime = DateTime.Now;

            foreach (var pair in _entityObjectCollection)
            {
                var entityName = pair.Key;
                var obj = pair.Value;
                long postObjectCount;
                long processObjectCount;
                obj.Reset(out postObjectCount, out processObjectCount);

                var changeAutoMqSummary = new EntitySummary()
                {
                    TotalCount = obj.ChangeAutoTimes.Total,
                    Count = obj.ChangeAutoTimes.Reset()
                };

                var postMqSummary = new EntitySummary()
                {
                    TotalCount = obj.PostTimes.Total,
                    Count = obj.PostTimes.Reset(),
                    TotalObjectCount = obj.TotalPostObjectCount,
                    ObjectCount = postObjectCount
                };
                var processMqSummary = new EntitySummary()
                {
                    TotalCount = obj.ProcessTimes.Total,
                    Count = obj.ProcessTimes.Reset(),
                    TotalObjectCount = obj.TotalProcessObjectCount,
                    ObjectCount = processObjectCount
                };
                summary.ChangeAutoMQ[entityName] = changeAutoMqSummary;
                summary.PostMQ[entityName] = postMqSummary;
                summary.ProcessMQ[entityName] = processMqSummary;
                var noProcessKeys = obj.PopNoProcessKeys().ToList();
                obj.TotalNoProcessObjectCount += noProcessKeys.Count;
                summary.TotalNoProcess += obj.TotalNoProcessObjectCount;
                if (noProcessKeys.Count > 0)
                {
                    //Write error log
                    TraceLog.WriteError("Entity operation is not saved, \"{0}\" keys:{1}", entityName, string.Join(";", noProcessKeys));
                }
            }
            foreach (var pair in _sqlCollection)
            {
                var obj = pair.Value;
                summary.PostSqlCount += obj.PostTimes.Reset();
                summary.ProcessSqlCount += obj.ProcessTimes.Reset();
                summary.TotalPostSqlCount += obj.PostTimes.Total;
                summary.TotalProcessSqlCount += obj.ProcessTimes.Total;
                summary.TotalProcessFailSqlCount += obj.TotalFailCount;
            }
            WriteProfileSummaryLog(summary);
            return _summary = summary;
        }

        #region private method
        private static void WriteToStorage()
        {
            //Only write file
            if (!IsEnable || _operateLog.Length == 0) return;

            StringBuilder log;
            lock (rootSync)
            {
                if (_operateLog.Length == 0) return;
                log = Interlocked.Exchange(ref _operateLog, new StringBuilder());
            }

            string path = WorkingLogPath ?? "";
            path = Path.Combine((string.IsNullOrEmpty(path) ? MathUtils.RuntimePath : path), "Profile");
            string dayPath = DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string detailPath = Path.Combine(path, MessageQueueTableName, "Storage");
            if (!Directory.Exists(detailPath))
            {
                Directory.CreateDirectory(detailPath);
            }
            //content {Time:xxx MessageQueue [operateMode] entityName keys:}
            string fileName = Path.Combine(detailPath, dayPath + ".log");
            File.AppendAllText(fileName, log.ToString(), Encoding.UTF8);
        }

        private static void CheckLogTable(string dbProviderName, string connectionString)
        {
            var dbProvider = DbConnectionProvider.CreateDbProvider(ConnectKey, dbProviderName, connectionString);
            DbColumn[] columns;
            if (!dbProvider.CheckTable(MessageQueueTableName, out columns))
            {
                int number = 0;
                columns = MessageQueueColumns.Select(name =>
                 {
                     number++;
                     switch (name)
                     {
                         case "Id":
                             return new DbColumn
                             {
                                 Id = number,
                                 Name = "Id",
                                 Type = typeof(long),
                                 Isnullable = false,
                                 IsKey = true,
                                 IsUnique = false,
                                 DbType = ColumnDbType.Varchar.ToString(),
                                 IsIdentity = true,
                                 IdentityNo = 1
                             };
                         default:
                             return new DbColumn
                             {
                                 Id = number,
                                 Name = name,
                                 Type = typeof(decimal),
                                 Scale = ColumnScale,
                                 Isnullable = false,
                                 DbType = ColumnDbType.Varchar.ToString()
                             };
                     }
                 }).ToArray();
                dbProvider.CreateTable(MessageQueueTableName, columns);
            }
        }

        private static void AppendOperateLog(string category, string typeName, OperateMode mode, string key)
        {
            if (!IsOpenWriteLog) return;

            lock (rootSync)
            {
                //{Time:xxx type [operateMode] entityName keys:}
                _operateLog.AppendFormat("Time:{0} {1} [{2}] {3} key:{4}",
                    MathUtils.UnixEpochTimeSpan.TotalSeconds,
                    category,
                    (string.Join("|",
                        new[]{(mode.HasFlag(OperateMode.Add) ? OperateMode.Add.ToString() : ""),
                    (mode.HasFlag(OperateMode.Modify) ? OperateMode.Modify.ToString() : ""),
                    (mode.HasFlag(OperateMode.Remove) ? OperateMode.Remove.ToString() : "")}.Where(t => !string.IsNullOrEmpty(t)))
                    ),
                    typeName,
                    key
                  );
                _operateLog.AppendLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void WriteProfileSummaryLog(ProfileSummary summary)
        {
            string path = WorkingLogPath ?? "";
            switch (StorageMode)
            {
                case ProfileStorageMode.File:
                    path = Path.Combine((string.IsNullOrEmpty(path) ? MathUtils.RuntimePath : path), "Profile");
                    WriteFileLog(summary, path);
                    break;
                case ProfileStorageMode.Sql:
                    WriteSqlLog(summary, typeof(SqlDataProvider).Name, path.Substring(4));
                    break;
                case ProfileStorageMode.MySql:
                    WriteSqlLog(summary, typeof(MySqlDataProvider).Name, path.Substring(6));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void WriteSqlLog(ProfileSummary summary, string dbProviderName, string connectionString)
        {
            var dbProvider = DbConnectionProvider.CreateDbProvider(ConnectKey, dbProviderName, connectionString);
            var command = dbProvider.CreateCommandStruct(MessageQueueTableName, CommandMode.Insert);

            foreach (var pair in GetDetailValues(summary))
            {
                if (pair.Key.ToLower() == MessageQueueColumns[0].ToLower())
                {
                    //ignore "id"
                    continue;
                }
                command.AddParameter(pair.Key, pair.Value);
            }
            dbProvider.ExecuteQuery(command);
        }


        private static void WriteFileLog(ProfileSummary summary, string path)
        {
            string dayPath = DateTime.Now.ToString("yyyyMMdd");
            //1.total summary
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string detailPath = Path.Combine(path, MessageQueueTableName, "Performance");
            if (!Directory.Exists(detailPath))
            {
                Directory.CreateDirectory(detailPath);
            }
            string fileName = Path.Combine(path, "summary.log");
            File.WriteAllText(fileName, FormatTotalContent(summary));
            //entity MQ write log detail
            string detailName = Path.Combine(detailPath, dayPath + ".log");
            if (!File.Exists(detailName))
            {
                File.AppendAllText(detailName, string.Join(",", MessageQueueColumns));
            }
            File.AppendAllText(detailName, FormatDetailContent(summary));
        }


        private static string FormatTotalContent(ProfileSummary summary)
        {
            var log = new StringBuilder();
            foreach (var pair in GetDetailValues(summary))
            {
                log.AppendFormat("{0}:\t{1},", pair.Key, pair.Value);
                log.AppendLine();
            }
            return log.ToString().TrimEnd(',');
        }

        private static string FormatDetailContent(ProfileSummary summary)
        {
            var log = new StringBuilder();
            log.AppendLine();
            foreach (var pair in GetDetailValues(summary))
            {
                log.AppendFormat("{0},", pair.Value);
            }
            return log.ToString().TrimEnd(',');
        }

        #endregion

        private static IEnumerable<KeyValuePair<string, object>> GetDetailValues(ProfileSummary summary)
        {
            var changeAutoCount = summary.ChangeAutoMQ.Sum(t => t.Value.Count);
            var changeAutoPerSecond = Math.Round(changeAutoCount / summary.IntervalSecond, ColumnScale);//2位小数
            var postCount = summary.PostMQ.Sum(t => t.Value.Count);
            var postObjectCount = summary.PostMQ.Sum(t => t.Value.ObjectCount);
            var postPerSecond = Math.Round(postCount / summary.IntervalSecond, ColumnScale);
            var processCount = summary.ProcessMQ.Sum(t => t.Value.Count);
            var processObjectCount = summary.ProcessMQ.Sum(t => t.Value.ObjectCount);
            var processPerSecond = Math.Round(processCount / summary.IntervalSecond, ColumnScale);

            //sql
            var processSqlPerSecond = Math.Round(summary.ProcessSqlCount / summary.IntervalSecond, ColumnScale);


            return new[]
            {
                new KeyValuePair<string, object>("Id", summary.Id), 
                new KeyValuePair<string, object>("Time", MathUtils.UnixEpochTimeSpan.TotalSeconds), 
                new KeyValuePair<string, object>("TotalAutoChanged", summary.ChangeAutoMQ.Sum(t => t.Value.TotalCount)), 
                new KeyValuePair<string, object>("AutoChangedCount", changeAutoCount), 
                new KeyValuePair<string, object>("AutoChangedPerSecond", changeAutoPerSecond), 
                new KeyValuePair<string, object>("TotalPost", summary.PostMQ.Sum(t => t.Value.TotalCount)), 
                new KeyValuePair<string, object>("TotalPostObject", summary.PostMQ.Sum(t => t.Value.TotalObjectCount)), 
                new KeyValuePair<string, object>("PostCount", postCount), 
                new KeyValuePair<string, object>("PostObjectCount", postObjectCount), 
                new KeyValuePair<string, object>("PostPerSecond", postPerSecond), 
                new KeyValuePair<string, object>("TotalProcess", summary.ProcessMQ.Sum(t => t.Value.TotalCount)), 
                new KeyValuePair<string, object>("TotalProcessObject", summary.ProcessMQ.Sum(t => t.Value.TotalObjectCount)), 
                new KeyValuePair<string, object>("ProcessCount", processCount), 
                new KeyValuePair<string, object>("ProcessObjectCount", processObjectCount), 
                new KeyValuePair<string, object>("ProcessPerSecond", processPerSecond), 
                new KeyValuePair<string, object>("NoProcessKey", summary.TotalNoProcess), 

                //sql
                new KeyValuePair<string, object>("TotalPostSql", summary.TotalPostSqlCount), 
                new KeyValuePair<string, object>("TotalProcessSql", summary.TotalProcessSqlCount), 
                new KeyValuePair<string, object>("TotalFailSql", summary.TotalProcessFailSqlCount), 
                new KeyValuePair<string, object>("WaitSyncEntity", summary.WaitSyncEntityCount), 
                new KeyValuePair<string, object>("PostSql", summary.PostSqlCount), 
                new KeyValuePair<string, object>("ProcessSql", summary.ProcessSqlCount), 
                new KeyValuePair<string, object>("ProcessSqlPerSecond", processSqlPerSecond), 
            };

        }

    }
}
