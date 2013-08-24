using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.Wcf;

namespace ZyGames.Framework.Game.SocketServer
{
    /// <summary>
    /// 请求分发管理
    /// </summary>
    public class WcfClientManager : IDisposable
    {
        /// <summary>
        /// 倍率
        /// </summary>
        private const int MultipleRate = 1000;
        private static WcfClientManager _instance;
        static WcfClientManager()
        {
            _instance = new WcfClientManager();
        }
        /// <summary>
        /// 
        /// </summary>
        public static WcfClientManager Current
        {
            get { return _instance; }
        }

        private DateTime preModifyDate;
        private FileSystemWatcher _watcher = new FileSystemWatcher();
        private ConcurrentDictionary<int, WcfServiceClient> _pools;

        private WcfClientManager()
        {
            _pools = new ConcurrentDictionary<int, WcfServiceClient>();
        }

        public void InitConfig(string path)
        {
            string moduleName = "ServerConnection.config";
            _watcher.Path = path;
            //_watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            // 只监控.py文件  
            _watcher.Filter = moduleName;
            //设置是否监听子目录
            _watcher.IncludeSubdirectories = false;
            // 添加事件处理器。  
            _watcher.Changed += OnScriptFileChanged;
            // 开始监控。  
            _watcher.EnableRaisingEvents = true;
            path = Path.Combine(path, moduleName);
            ParaseConfig(path);
        }

        public WcfServiceClient Get(int gameId, int serverId)
        {
            WcfServiceClient client;
            int identityId = ToIdentityId(gameId, serverId);
            if (_pools.TryGetValue(identityId, out client))
            {
                return client;
            }
            return null;
        }

        private static int ToIdentityId(int gameId, int serverId)
        {
            return gameId * MultipleRate + serverId;
        }

        private void OnScriptFileChanged(object sender, FileSystemEventArgs e)
        {
            string moduleName = e.Name;
            try
            {
                TimeSpan ts = DateTime.Now - preModifyDate;
                if (ts.TotalSeconds > 1)
                {
                    preModifyDate = DateTime.Now;
                    ParaseConfig(moduleName);
                    TraceLog.ReleaseWrite("server connection load success");
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Load server connection:{0} error:{1}", moduleName, ex);
            }
        }

        private void ParaseConfig(string moduleName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(moduleName);
            var childList = xmlDoc.SelectNodes("//connection");
            if (childList == null)
            {
                return;
            }
            _pools.Clear();

            foreach (var child in childList)
            {
                XmlElement el = child as XmlElement;
                if (el == null)
                {
                    continue;
                }
                int gameid = el.GetAttribute("gameid").ToInt();
                int serverid = el.GetAttribute("serverid").ToInt();
                string ip = el.GetAttribute("ip");
                int port = el.GetAttribute("port").ToInt();
                int identityId = ToIdentityId(gameid, serverid);
                WcfServiceClient oldValue;
                if (_pools.TryRemove(identityId, out oldValue))
                {
                    oldValue.Dispose();
                }
                var client = new WcfServiceClient(ip, port, identityId);
                _pools.TryAdd(identityId, client);
            }
        }


        /// <summary>
        /// 释放
        /// </summary>
        public static void Dispose()
        {
            _instance.DoDispose(true);
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                _watcher.Dispose();
                _pools = null;
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }
    }
}
