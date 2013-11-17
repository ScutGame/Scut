using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Doudizhu.Bll.Base
{
    /// <summary>
    /// 请求分发管理
    /// </summary>
    public class AppstoreClientManager : IDisposable
    {

        private static AppstoreClientManager _instance;
        static AppstoreClientManager()
        {
            _instance = new AppstoreClientManager();
        }
        /// <summary>
        /// 
        /// </summary>
        public static AppstoreClientManager Current
        {
            get { return _instance; }
        }

        private DateTime preModifyDate;
        private FileSystemWatcher _watcher = new FileSystemWatcher();
        public Hashtable ListAppstore = new Hashtable();
        public Hashtable ListIphone = new Hashtable();
        public Hashtable ListIpad = new Hashtable();
        public Hashtable ListSelf = new Hashtable();
        private string _runtimePath;

        private AppstoreClientManager()
        {
        }

        public void InitConfig()
        {
            string runtimePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            InitConfig(runtimePath);
        }

        public void InitConfig(string runtimePath)
        {
            _runtimePath = Path.Combine(runtimePath, "Config");
            string moduleName = "Appstore.cfg.xml";
            _watcher.Path = _runtimePath;
            //_watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            // 只监控.py文件  
            _watcher.Filter = moduleName;
            //设置是否监听子目录
            _watcher.IncludeSubdirectories = false;
            // 添加事件处理器。  
            _watcher.Changed += OnScriptFileChanged;
            // 开始监控。  
            _watcher.EnableRaisingEvents = true;
            runtimePath = Path.Combine(runtimePath, moduleName);
            ParaseConfig(moduleName);
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
            string fileName = Path.Combine(_runtimePath, moduleName);
            xmlDoc.Load(fileName);
            var childListIphone = xmlDoc.SelectNodes("//configuration//iphone//add");
            if (childListIphone == null)
            {
                return;
            }
            ListAppstore.Clear();
            ListIphone.Clear();
            foreach (var child in childListIphone)
            {
                XmlElement el = child as XmlElement;
                if (el == null)
                {
                    continue;
                }
                ListIphone.Add(el.GetAttribute("key").ToNotNullString(),
              new AppStoreHelper() { Dollar = el.GetAttribute("dollar").ToDecimal(), SilverPiece = el.GetAttribute("gameCoin").ToInt(), RMB = el.GetAttribute("currency").ToInt(), product_id = el.GetAttribute("key").ToNotNullString() });
                ListAppstore.Add(el.GetAttribute("key").ToNotNullString(),
              new AppStoreHelper() { Dollar = el.GetAttribute("dollar").ToDecimal(), SilverPiece = el.GetAttribute("gameCoin").ToInt(), RMB = el.GetAttribute("currency").ToInt(), product_id = el.GetAttribute("key").ToNotNullString() });
            }


            var childListIpad = xmlDoc.SelectNodes("//configuration//ipad//add");
            if (childListIpad == null)
            {
                return;
            }

            ListIpad.Clear();
            foreach (var child in childListIpad)
            {
                XmlElement el = child as XmlElement;
                if (el == null)
                {
                    continue;
                }
                ListIpad.Add(el.GetAttribute("key").ToNotNullString(),
              new AppStoreHelper() { Dollar = el.GetAttribute("dollar").ToDecimal(), SilverPiece = el.GetAttribute("gameCoin").ToInt(), RMB = el.GetAttribute("currency").ToInt(), product_id = el.GetAttribute("key").ToNotNullString() });
               
            }


            var childListSelf = xmlDoc.SelectNodes("//configuration//self//add");
            if (childListSelf == null)
            {
                return;
            }

            ListSelf.Clear();
            foreach (var child in childListSelf)
            {
                XmlElement el = child as XmlElement;
                if (el == null)
                {
                    continue;
                }
                ListSelf.Add(el.GetAttribute("key").ToNotNullString(),
              new AppStoreHelper() { Dollar = el.GetAttribute("dollar").ToDecimal(), SilverPiece = el.GetAttribute("gameCoin").ToInt(), RMB = el.GetAttribute("currency").ToInt(), product_id = el.GetAttribute("key").ToNotNullString() }); ;

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
