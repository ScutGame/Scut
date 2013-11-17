using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using ZyGames.Core.Util;
using ZyGames.Core.Web;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.OA.WatchService.BLL.Tools;
using ZyGames.SimpleManager.Model;
using ZyGames.SimpleManager.Service;
using ZyGames.SimpleManager.Service.Common;

namespace ZyGames.OA.WatchService.BLL.Watch
{
    /// <summary>
    /// ÓÎÏ·µÇÂ¼¼àÊÓ
    /// </summary>
    public class GameLoginWatch : BaseWatch
    {
        private class ServerInfo
        {
            public int GameID { get; set; }
            public string GameName { get; set; }
            public int ID { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string BaseUrl { get; set; }
            public int Weight { get; set; }
        }
        private const string serverUrl = "http://dir.36you.net/Service.aspx";

        private static int WatchInterval = Int32.Parse(ConfigurationManager.AppSettings["GameLogin_Interval"]);
        private static string GameLogin_Games;
        private static string GameLogin_Id;
        private static string GameLogin_pwd;
        private static Dictionary<int, string> gameDict = new Dictionary<int, string>();
        private static int errorTimes = 0;
        private const int ContinuousTimes = 3;


        static GameLoginWatch()
        {
            try
            {
                GameLogin_Games = ConfigHelper.GetSetting("GameLogin_Games");
                GameLogin_Id = ConfigHelper.GetSetting("GameLogin_Id");
                GameLogin_pwd = ConfigHelper.GetConnectionString("GameLogin_pwd");
                GameLogin_pwd = new ZyGames.DesSecurity.DESAlgorithmNew().EncodePwd(GameLogin_pwd, "n7=7=7dk");
                GameLogin_pwd = HttpUtility.UrlEncode(GameLogin_pwd, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Logger.SaveLog("The login check configuration error", ex);
            }

            string[] gameArry = GameLogin_Games.Split(',');
            foreach (var item in gameArry)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var itemArray = item.Split('=');
                if (itemArray.Length != 2) continue;
                int gameId = Convert.ToInt32(itemArray[0]);
                string gameName = itemArray[1];
                if (!gameDict.ContainsKey(gameId))
                {
                    gameDict.Add(gameId, gameName);
                }
            }

        }

        public GameLoginWatch()
        {
            Interval = WatchInterval;
        }

        protected override bool DoProcess(object obj)
        {
            try
            {
                foreach (var game in gameDict)
                {
                    RequestGame(game.Key, game.Value);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
                return false;
            }
        }

        private void RequestGame(int gameId, string gameName)
        {
            try
            {
                var serverList = new List<ServerInfo>();
                StringBuilder requestParams = new StringBuilder();
                requestParams.AppendFormat("{0}={1}", "ActionID", 1001);
                requestParams.AppendFormat("&{0}={1}", "GameID", gameId);

                Message msg = new Message();
                using (MessageReader msgReader = MessageReader.Create(serverUrl, requestParams.ToString(), msg))
                {
                    if (msgReader == null) return;

                    if (msg.ErrorCode != 10000)
                    {
                        int recordCount = msgReader.RecordCount();

                        for (int i = 0; i < recordCount; i++)
                        {
                            var server = new ServerInfo();
                            msgReader.RecordStart();
                            server.GameID = gameId;
                            server.GameName = gameName;
                            server.ID = msgReader.ReadInt();
                            server.Name = msgReader.ReadString();
                            server.Status = msgReader.ReadString();
                            server.BaseUrl = msgReader.ReadString();
                            server.Weight = msgReader.ReadInt();

                            msgReader.RecordEnd();
                            serverList.Add(server);
                        }
                    }
                    else
                    {
                        Logger.SaveLog(string.Format("Game login monitor >> games {0} service list error", gameName), new Exception(msg.ErrorInfo));
                    }
                }
                foreach (var serverInfo in serverList)
                {
                    ThreadPool.QueueUserWorkItem(RequestServer, serverInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveLog(string.Format("The Game Login surveillance >> read {0} the game hours service list error:", gameName), ex);
                SendToMail(string.Format("¶ÁÈ¡·Ö·þÁÐ±íÒì³£:{0}", ex.Message));
            }
        }

        private void RequestServer(object state)
        {
            if (state is ServerInfo)
            {
                var server = state as ServerInfo;
                //Logger.SaveLog(string.Format("ÓÎÏ·µÇÂ¼¼àÊÓ>>{0}[{1}]ÇëÇó¿ªÊ¼", server.GameName, server.Name));
                StringBuilder requestParams = new StringBuilder();
                requestParams.AppendFormat("{0}={1}", "ActionID", 1004);
                requestParams.AppendFormat("&{0}={1}", "Sid", "");
                requestParams.AppendFormat("&{0}={1}", "Uid", "");
                requestParams.AppendFormat("&{0}={1}", "MobileType", 1);
                requestParams.AppendFormat("&{0}={1}", "Pid", GameLogin_Id);
                requestParams.AppendFormat("&{0}={1}", "Pwd", GameLogin_pwd);
                requestParams.AppendFormat("&{0}={1}", "DeviceID", HttpUtility.UrlEncode("00-00-00-00-00-4A", Encoding.UTF8));
                requestParams.AppendFormat("&{0}={1}", "GameType", server.GameID);
                requestParams.AppendFormat("&{0}={1}", "ServerID", server.ID);
                requestParams.AppendFormat("&{0}={1}", "RetailID", "0000");
                requestParams.AppendFormat("&{0}={1}", "RetailUser", "");

                string errorInfo = string.Empty;
                Message msg = new Message();
                using (MessageReader msgReader = MessageReader.Create(server.BaseUrl, requestParams.ToString(), msg))
                {
                    if (msgReader == null) return;

                    if (msg.ErrorCode != 0)
                    {
                        errorInfo = string.Format("ÓÎÏ·µÇÂ¼¼àÊÓ>>{0}[{4}·þ-{1}]µÇÂ¼³ö´í£¬Error:{2}-{3}", server.GameName, server.Name, msg.ErrorCode, msg.ErrorInfo, server.ID);
                        Logger.SaveLog(new Exception(errorInfo));

                        
                        //Modify post trace
                        string planName = string.Format("¼àÊÓÓÎÏ·£º{0}£¬·þ£º{1}µÇÂ¼Ê§°Ü", server.GameName, server.Name);
                        string planValue = string.Format("{0}:{1}", msg.ErrorCode, msg.ErrorInfo);
                        OaSimplePlanHelper.PostDataToServer(planName, planValue);
                    }
                    else
                    {
                        Logger.SaveLog(string.Format("Game login surveillance >> {0} {2} dress - {1}] successful login", server.GameName, server.Name, server.ID));
                    }
                }

                if (!string.IsNullOrEmpty(errorInfo))
                {
                    SendToMail(errorInfo);
                }
            }
        }

        private static object thisLock = new object();
        private static void SendToMail(string content)
        {
            if (errorTimes > ContinuousTimes)
            {
                try
                {
                    lock (thisLock)
                    {
                        errorTimes = 0;
                    }
                    Mail139Helper.SendMail("ÓÎÏ·µÇÂ¼¼àÊÓ", content, ConfigContext.GetInstance().SendTo139Mail, true);
                }
                catch (Exception ex)
                {
                    Logger.SaveLog("Log in to check mail error", ex);
                }
            }
            else
            {
                lock (thisLock)
                {
                    errorTimes++;
                }
            }
        }
    }
}
