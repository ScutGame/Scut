using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Core.Data;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ZyGames.Core.Util;
using ZyGames.Core.Web;
using ZyGames.OA.WatchService.BLL.Plugin;
using ZyGames.OA.WatchService.BLL.Tools;
using ZyGames.SimpleManager.Model;
using ZyGames.SimpleManager.Service;

namespace ZyGames.OA.WatchService.BLL.Watch
{
    /// <summary>
    /// 周一做完成备份，其它差异备份，保留两周的数据
    /// </summary>
    public class DbBackupWatch : BaseWatch
    {
        private static string connectionFormat = "Data Source={0};Database=master;{1}; Pooling=true;Max Pool Size=100;Min Pool Size=0;";
        private static string ConnectionString = ConfigHelper.GetSetting("DbBackup_ConnectionString", connectionFormat);
        private static readonly object thisLock = new object();
        private static string Timming = ConfigHelper.GetSetting("DbBackup_Timing");
        private static string DbBackupPath = ConfigHelper.GetSetting("DbBackup_Path");
        private static int SaveDay = Convert.ToInt32(ConfigHelper.GetSetting("DbBackup_SaveDay"));
        private static int CommandTimeout = Convert.ToInt32(ConfigHelper.GetSetting("DbBackup_CommandTimeout"));
        private static string[] DbBackupFilter = new string[0];
        private static string DbBackupServer = ConfigHelper.GetSetting("DbBackup_Server");
        private static string DbBackupAcount = ConfigHelper.GetConnectionString("DbBackup_Acount");
        private static string serverUrl = ConfigHelper.GetSetting("OAPlan_Server");

        private static string _netWorkPath = ConfigHelper.GetSetting("DbBackup_NetWork_Path");
        private static string _netWorkUser = ConfigHelper.GetSetting("DbBackup_NetWork_User");
        private static string _netWorkPassword = ConfigHelper.GetConnectionString("DbBackup_NetWork_Password");
        private static bool _ignoreLog;
        private static bool IsNetBackup { get; set; }
        private static DayOfWeek FullWeek { get; set; }

        static DbBackupWatch()
        {
            ConnectionString = string.Format(ConnectionString, DbBackupServer, DbBackupAcount);
            string tempStr = ConfigHelper.GetSetting("DbBackup_Filter");
            if (tempStr.Trim().Length > 0) DbBackupFilter = tempStr.Split('|');
            IsNetBackup = string.IsNullOrEmpty(_netWorkPath) ? false : true;
            string week = ConfigHelper.GetSetting("DbBackup_FullWeek");
            if (week.Length > 0)
            {
                FullWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), week);
            }
            else
            {
                FullWeek = DayOfWeek.Monday;
            }
            _ignoreLog = Convert.ToBoolean(ConfigHelper.GetSetting("DbBackup_IgnoreLog", "true"));
        }


        public DbBackupWatch()
            : base(Timming)
        {
        }

        //public void Test()
        //{
        //    try
        //    {
        //        string dbName = "CkTjx5Data";
        //        string path = IsNetBackup ? _netWorkPath : DbBackupPath;
        //        if (!OpenNetAddress(path, _netWorkUser, _netWorkPassword))
        //        {
        //            Logger.SaveLog("网络路径" + path + "找开失败");
        //            return;
        //        }
        //        BackupDatabase(path, dbName);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        protected override bool DoProcess(object obj)
        {
            try
            {
                string path = IsNetBackup ? _netWorkPath : DbBackupPath;
                Logger.SaveLog("DbBackupWatch start...");
                if (IsNetBackup)
                {
                    if (!OpenNetAddress(path, _netWorkUser, _netWorkPassword))
                    {
                        Logger.SaveLog("Network path " + path + " Unable to connect.");
                        string planName = string.Format("{0}数据库自动备份", GetServerIP());
                        OaSimplePlanHelper.PostDataToServer(planName, "网络路径" + path + "无法连接");
                        return false;
                    }

                    Logger.SaveLog("Is to clean up the database backup...");
                    ClearBackupDb(path);
                    Logger.SaveLog("The end of the clean database backup.");
                    BackupAll(path);
                }
                else
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    lock (thisLock)
                    {
                        Logger.SaveLog("Is to clean up the database backup...");
                        ClearBackupDb(path);
                        Logger.SaveLog("The end of the clean database backup.");
                        BackupAll(path);

                    }
                }
                Logger.SaveLog("DbBackupWatch end.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
                return false;
            }
        }

        private void BackupAll(string path)
        {
            int successNum = 0;
            int faildNum = 0;
            string sql = "SELECT Name FROM Master..SysDatabases where name not in('AdventureWorks','AdventureWorksDW','master','model','msdb','tempdb','ReportServer','ReportServerTempDB','distribution') ORDER BY Name ";
            List<string> dbList = new List<string>();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    string dbName = reader["Name"].ToString();
                    if (_ignoreLog && dbName.ToLower().IndexOf("log") != -1) continue;
                    if (DbBackupFilter.Length > 0)
                    {
                        HashSet<string> filterSet = new HashSet<string>(DbBackupFilter);
                        if (filterSet.Contains(dbName)) continue;
                    }
                    dbList.Add(dbName);
                }
            }
            Logger.SaveLog(string.Format("Database backup count {0} to start...", dbList.Count));

            foreach (string dbName in dbList)
            {
                if (BackupDatabase(path, dbName))
                {
                    Logger.SaveLog(string.Format("Database {0} backup success", dbName));
                    successNum++;
                }
                else
                {
                    Logger.SaveLog(string.Format("Database {0} backup failed", dbName));
                    faildNum++;
                }

            }
            Logger.SaveLog(string.Format("The database backups end, the success of {0} failed {1}", successNum, faildNum));

            PostDataToServer(GetServerIP(), successNum, faildNum);
        }

        /// <summary>
        /// 提交数据到远程
        /// </summary>
        public void PostDataToServer(string ip, int successNum, int faildNum)
        {
            string postUrl = string.Format("{0}?type=dbbackup&ip={1}&successNum={2}&faildNum={3}", serverUrl, ip, successNum, faildNum);
            HttpHelper.GetReponseText(postUrl);
        }

        private bool OpenNetAddress(string backupPath, string user, string passwort)
        {
            bool result = false;
            string paramStr = string.Format(@"net use {0} {2} /user:{1}", backupPath, user, passwort);
            //GetCmdShell(paramStr);
            SqlParameter param = new SqlParameter("@command", SqlDbType.NVarChar, 1000);
            param.Value = paramStr;

            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "xp_cmdshell", param))
                {
                    if (reader.Read())
                    {
                        if (reader[0].ToString().StartsWith("命令成功完成")) result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
            }
            return result;
        }

        private bool BackupDatabase(string path, string dbName)
        {
            try
            {

                if (string.IsNullOrEmpty(dbName))
                {
                    return false;
                }
                string fileName = string.Empty;
                string currDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                string filePath = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string sql = string.Empty;
                if (DateTime.Now.DayOfWeek == FullWeek)
                {
                    fileName = string.Format("{0}_{1}_full.bak", dbName, currDate);
                    filePath = Path.Combine(filePath, fileName);
                    sql = string.Format("backup database {0} to disk='{1}' WITH NOFORMAT, NOINIT, NAME = N'{0}-完整 数据库 备份', SKIP, NOREWIND, NOUNLOAD", dbName, filePath);

                }
                else
                {
                    fileName = string.Format("{0}_{1}_diff.bak", dbName, currDate);
                    filePath = Path.Combine(filePath, fileName);
                    sql = string.Format("backup database {0} to disk='{1}' WITH  DIFFERENTIAL , NOFORMAT, NOINIT, NAME = N'{0}-差异 数据库 备份', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", dbName, filePath);
                }
                ExecuteNonQuery(sql);
                //SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql);
                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
            }

            return false;
        }

        private static void ExecuteNonQuery(string commandText)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                try
                {
                    cmd.CommandText = commandText;
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    if (CommandTimeout > 0)
                    {
                        cmd.CommandTimeout = CommandTimeout;
                    }
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        private static void ClearBackupDb(string path)
        {
            DateTime expectDate = DateTime.Now.AddDays(-SaveDay);
            DirectoryInfo rootDir = new DirectoryInfo(path);
            DirectoryInfo[] items = rootDir.GetDirectories();
            foreach (DirectoryInfo item in items)
            {
                DateTime dirDate;
                if (DateTime.TryParse(item.Name, out dirDate))
                {
                    if (dirDate < expectDate)
                    {
                        item.Delete(true);
                    }
                }
            }
        }

        /// <summary>
        /// 利用 xp_cmdshell 命令返回 dos 命令结果
        /// </summary>
        /// <param name="command">dos 命令</param>
        /// <returns>DataSet 类型</returns>
        DataSet GetCmdShell(string command)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "xp_cmdshell";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@command", SqlDbType.NVarChar, 1000);
                param.Value = command;

                //添加 DOS 命令
                cmd.Parameters.Add(param);

                SqlDataReader reader;
                cn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                int fieldCount = reader.FieldCount;

                //添加表的字段名称和类型
                for (int i = 0; i < fieldCount; i++)
                    dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));

                object[] values = new object[fieldCount];
                while (reader.Read())
                {
                    //循环添加每一行记录到 DataTable
                    reader.GetValues(values);
                    dt.LoadDataRow(values, true);
                }

                reader.Close();
                cmd.Dispose();
                dt.EndLoadData();
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
            }

            ds.Tables.Add(dt);

            return ds;

        }
    }
}
