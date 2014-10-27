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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataConfigger : IConfigger
    {
        //static DataConfigger()
        //{
        //    ConfigFile = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "AppServer.config");
        //}

        /// <summary>
        /// 
        /// </summary>
        public string ConfigFile { get; set; }

        protected bool IsDependenced;
        private FileSystemWatcher _watcher;
        private HashSet<string> _changedFiles = new HashSet<string>();
        private Timer _excuteTimer;
        private List<ConfigSection> _dataList = new List<ConfigSection>();
        protected int _dueChangeTime = 500;

        /// <summary>
        /// 
        /// </summary>
        public void Install()
        {
            if (!string.IsNullOrEmpty(ConfigFile) && File.Exists(ConfigFile))
            {
                IsDependenced = true;
                InitDependenceFile();
            }
            LoadConfigData();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitDependenceFile()
        {
            _excuteTimer = new Timer(OnExcute, null, Timeout.Infinite, Timeout.Infinite);
            string path = Path.GetDirectoryName(ConfigFile) ?? "";
            string file = Path.GetFileName(ConfigFile) ?? "*.config";
            _watcher = new FileSystemWatcher(path, file);
            _watcher.Changed += new FileSystemEventHandler(OnWatcherChanged);
            _watcher.Created += new FileSystemEventHandler(OnWatcherChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnWatcherChanged);
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            var e = new ConfigReloadedEventArgs();
            DoClearData();
            LoadConfigData();
            ConfigManager.OnConfigReloaded(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DoClearData()
        {
            _dataList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetConfig<T>() where T : ConfigSection
        {
            return _dataList.OfType<T>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<ConfigSection> GetAllConfig()
        {
            return _dataList.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList<T> GetConnetion<T>(string name) where T : ConnetionSection
        {
            return _dataList.Where(data => data is T && ((T) data).Name == name).Cast<T>().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeData"></param>
        protected void AddNodeData(ConfigSection nodeData)
        {
            _dataList.Add(nodeData);
        }


        /// <summary>
        /// 
        /// </summary>
        protected abstract void LoadConfigData();

        private void OnWatcherChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                _changedFiles.Add(e.FullPath);
                _excuteTimer.Change(_dueChangeTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("XmlDataConfigger changed error:{0}", ex);
            }
        }
        private void OnExcute(object state)
        {
            try
            {
                //Repetitive loading process
                var tempFile = Interlocked.Exchange(ref _changedFiles, new HashSet<string>());
                foreach (var fileName in tempFile)
                {
                    var e = new ConfigChangedEventArgs() { FileName = fileName };
                    DoClearData();
                    LoadConfigData();
                    ConfigManager.OnConfigChanged(this, e);
                    break;
                }
                //stop timer
                _excuteTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("XmlDataConfigger excute error:{0}", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //释放非托管资源 
            if (disposing)
            {
                if (_watcher != null) _watcher.Dispose();
                if (_watcher != null) _excuteTimer.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
}
