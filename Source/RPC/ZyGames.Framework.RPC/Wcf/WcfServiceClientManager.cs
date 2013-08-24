using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Wcf
{

    /// <summary>
    /// 请求分发管理
    /// </summary>
    public class WcfServiceClientManager : IDisposable
    {
        /// <summary>
        /// 倍率
        /// </summary>
        private const int MultipleRate = 1000;
        private static WcfServiceClientManager _instance;
        static WcfServiceClientManager()
        {
            _instance = new WcfServiceClientManager();
        }
        /// <summary>
        /// 
        /// </summary>
        public static WcfServiceClientManager Current
        {
            get { return _instance; }
        }

        private DateTime preModifyDate;
        private FileSystemWatcher _watcher = new FileSystemWatcher();
        private ConcurrentDictionary<int, WcfServiceClient> _pools;
        private string _path;
        private Action<string, byte[]> _receivedCallback;

        private WcfServiceClientManager()
        {
            _pools = new ConcurrentDictionary<int, WcfServiceClient>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="receivedCallback"></param>
        /// <param name="fileName"></param>
        public void InitConfig(string path, Action<string, byte[]> receivedCallback, string fileName = null)
        {
            lock (_instance)
            {
                _path = path;
                _receivedCallback = receivedCallback;
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException("path");
                }
                fileName = fileName ?? "ServerConnection.cfg.xml";
                ParaseConfig(Path.Combine(path, fileName));

                _watcher.Path = path;
                _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
                // 只监控.py文件  
                _watcher.Filter = fileName;
                //设置是否监听子目录
                _watcher.IncludeSubdirectories = false;
                // 添加事件处理器。  
                _watcher.Changed += OnScriptFileChanged;
                // 开始监控。  
                _watcher.EnableRaisingEvents = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public WcfServiceClient Get(int gameId, int serverId)
        {
            WcfServiceClient client;
            int identityId = ToIdentityId(gameId, serverId);
            if (_pools != null && _pools.TryGetValue(identityId, out client))
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
                    ParaseConfig(Path.Combine(_path, moduleName));
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
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(moduleName);
                var childList = xmlDoc.SelectNodes("//connection");
                if (childList == null || _pools == null)
                {
                    return;
                }
                HashSet<int> keys = new HashSet<int>();
                var er = _pools.Keys.GetEnumerator();
                while (er.MoveNext())
                {
                    keys.Add(er.Current);
                }

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
                    int timeout = el.GetAttribute("connectTimeout").ToInt();
                    int receiveTimeout = el.GetAttribute("receiveTimeout").ToInt();
                    int inactivityTimeout = el.GetAttribute("inactivityTimeout").ToInt();
                    int identityId = ToIdentityId(gameid, serverid);
                    //过滤Key
                    keys.Remove(identityId);
                    TraceLog.ReleaseWrite("Load wcfclient config:{0},ip:{1}:{2}", identityId, ip, port);
                    WcfServiceClient client;
                    try
                    {
                        if (!_pools.TryGetValue(identityId, out client))
                        {
                            client = new WcfServiceClient(identityId);
                            BindWcfConection(ip, port, timeout, client, receiveTimeout, inactivityTimeout);
                            _pools.TryAdd(identityId, client);
                        }
                        else if (client.Ip != ip ||
                            client.Port != port ||
                            (timeout > 0 && client.ConnectTimeout.TotalSeconds != (double)timeout))
                        {
                            BindWcfConection(ip, port, timeout, client, receiveTimeout, inactivityTimeout);
                        }
                    }
                    catch (Exception se)
                    {
                        TraceLog.WriteError("Binding wcfclient-{0} error:{0}", identityId, se);
                    }
                }

                //remove key
                foreach (var key in keys)
                {
                    WcfServiceClient client;
                    if (_pools.TryRemove(key, out client))
                    {
                        client.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ParaseConfig error:{0}", ex);
            }
        }

        private void BindWcfConection(string ip, int port, int timeout, WcfServiceClient client, int receiveTimeout, int inactivityTimeout)
        {
            if (timeout > 0 && receiveTimeout > 0 && inactivityTimeout > 0)
            {
                client.Bind(ip, port, _receivedCallback, timeout, new TimeSpan(0, 0, receiveTimeout), inactivityTimeout);
            }
            else if (timeout > 0 && inactivityTimeout > 0)
            {
                client.Bind(ip, port, _receivedCallback, timeout, TimeSpan.MaxValue, inactivityTimeout);
            }
            else if (timeout > 0)
            {
                client.Bind(ip, port, _receivedCallback, timeout);
            }
            else
            {
                client.Bind(ip, port, _receivedCallback);
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
