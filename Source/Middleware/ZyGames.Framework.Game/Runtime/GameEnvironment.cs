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
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Reflect;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Model;
using ZyGames.Framework.Redis;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// Runtime event handler.
    /// </summary>
    public delegate void RuntimeEventHandler();

    /// <summary>
    /// 游戏运行环境
    /// </summary>
    public static class GameEnvironment
    {
        static GameEnvironment()
        {
            ConfigManager.ConfigReloaded += OnConfigReloaded;
        }

        private static void OnConfigReloaded(object sender, ConfigReloadedEventArgs e)
        {
            try
            {
                _setting.Reset();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("GameEnvironment reload error:{0}", ex);
            }
        }

        /// <summary>
        /// The python script task cache key.
        /// </summary>
        public static readonly string PythonScriptTaskCacheKey = "__PythonScript_Task";

        private static int _isRunning;

        private static EnvironmentSetting _setting = new EnvironmentSetting();
        ///<summary>
        /// The environment configuration information.
        ///</summary>
        public static EnvironmentSetting Setting
        {
            get
            {
                return _setting;
            }
        }
        /// <summary>
        /// 全局变量集合
        /// </summary>
        public static ContextCacheSet<CacheItem> Global
        {
            get;
            private set;
        }

        /// <summary>
        /// 产品代码
        /// </summary>
        public static int ProductCode { get { return _setting != null ? _setting.ProductCode : 0; } }

        /// <summary>
        /// 产品名称
        /// </summary>
        public static string ProductName { get { return _setting != null ? _setting.ProductName : ""; } }

        /// <summary>
        /// 游戏服代码
        /// </summary>
        public static int ProductServerId { get { return _setting != null ? _setting.ProductServerId : 0; } }


        private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// is close server
        /// </summary>
        public static bool IsCanceled
        {
            get
            {
                return cancellationTokenSource.Token.IsCancellationRequested;
            }
            set
            {
                if (value)
                {
                    cancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsRunning
        {
            get { return _isRunning == 1; }
            set { Interlocked.Exchange(ref _isRunning, value ? 1 : 0); }
        }

        /// <summary>
        /// Initialize entity cache.
        /// </summary>
        public static void InitializeCache(ICacheSerializer serializer)
        {
            CacheFactory.Initialize(new CacheSetting(), serializer);
        }

        /// <summary>
        /// The game service start.
        /// </summary>
        /// <param name="setting">Environment setting.</param>
        public static void Start(EnvironmentSetting setting)
        {
            CacheSetting cacheSetting = new CacheSetting();
            cacheSetting.ChangedHandle += EntitySyncManger.OnChange;
            Start(setting, cacheSetting);
        }

        /// <summary>
        /// The game service start.
        /// </summary>
        /// <param name="setting">Environment setting.</param>
        /// <param name="cacheSetting">Cache setting.</param>
        public static void Start(EnvironmentSetting setting, CacheSetting cacheSetting)
        {
            if (IsRunning) return;

            TraceLog.WriteLine("{0} Server is starting...", DateTime.Now.ToString("HH:mm:ss"));
            _setting = setting;
            if (!RedisConnectionPool.Ping("127.0.0.1"))
            {
                string error = string.Format("Error: NIC is not connected or no network.");
                TraceLog.WriteLine(error);
                TraceLog.WriteError(error);
                return;
            }
            RedisConnectionPool.Initialize(_setting.Serializer);
            if (!RedisConnectionPool.CheckConnect())
            {
                string error = string.Format("Error: the redis server is not started.");
                TraceLog.WriteLine(error);
                TraceLog.WriteError(error);
                return;
            }
            TraceLog.WriteLine("{0} Redis server connect successfully.", DateTime.Now.ToString("HH:mm:ss"));

            DbConnectionProvider.Initialize();
            TraceLog.WriteLine("{0} DB server connect successfully.", DateTime.Now.ToString("HH:mm:ss"));

            EntitySchemaSet.CacheGlobalPeriod = _setting.CacheGlobalPeriod;
            EntitySchemaSet.CacheUserPeriod = _setting.CacheUserPeriod;
            if (_setting.EntityAssembly != null)
            {
                ProtoBufUtils.LoadProtobufType(_setting.EntityAssembly);
                EntitySchemaSet.LoadAssembly(_setting.EntityAssembly);
            }

            ZyGameBaseConfigManager.Intialize();
            //init script.
            if (_setting.ScriptSysAsmReferences.Length > 0)
            {
                ScriptEngines.AddSysReferencedAssembly(_setting.ScriptSysAsmReferences);
            }
            ScriptEngines.AddReferencedAssembly("ZyGames.Framework.Game.dll");
            if (_setting.ScriptAsmReferences.Length > 0)
            {
                ScriptEngines.AddReferencedAssembly(_setting.ScriptAsmReferences);
            }
            ScriptEngines.RegisterModelChangedBefore(OnModelChangeBefore);
            ScriptEngines.RegisterModelChangedAfter(OnModelChangeAtfer);
            ScriptEngines.OnLoaded += OnScriptLoaded;
            ScriptEngines.Initialize();
            Language.SetLang();
            CacheFactory.Initialize(cacheSetting, _setting.Serializer);
            EntitySchemaSet.StartCheckTableTimer();
            Global = new ContextCacheSet<CacheItem>("__gameenvironment_global");
        }

        private static void OnScriptLoaded(string type, string[] files)
        {
            if (".cs".Equals(type))
            {
                Language.Reset();
            }
        }


        private const int CheckTimeout = 60000;
        private static void OnModelChangeBefore(Assembly assembly)
        {
            try
            {
                IsRunning = false;
                TraceLog.ReleaseWrite("Wait for the update before Model script...");
                CacheFactory.UpdateNotify(true);
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    int time = CheckTimeout / 100;
                    try
                    {
                        while (time > 0 && !CacheFactory.CheckCompleted())
                        {
                            Thread.Sleep(100);
                            time--;
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
                if (System.Threading.Tasks.Task.WaitAll(new[] { task }, CheckTimeout))
                {
                    TraceLog.ReleaseWrite("Update before Model script OK.");
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnModelChangeBefore error:{0}", ex);
            }
        }

        private static void OnModelChangeAtfer(Assembly assembly)
        {
            if (assembly == null) return;
            try
            {
                TypeAccessor.Init();
                RedisConnectionPool.Initialize(_setting.Serializer);
                DbConnectionProvider.Initialize();
                ProtoBufUtils.Initialize();
                ProtoBufUtils.LoadProtobufType(assembly);
                EntitySchemaSet.Init();
                EntitySchemaSet.LoadAssembly(assembly);
                EntitySchemaSet.InitSchema(typeof (SensitiveWord));
                Language.Reset();
                CacheFactory.ResetCache();
                TraceLog.ReleaseWrite("Update Model script success.");
                IsRunning = true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnModelChangeAtfer error:{0}", ex);
            }
        }


        //private static void LoadGameEntitySchema()
        //{
        //    SchemaTable schema;
        //    if (EntitySchemaSet.TryGet<SensitiveWord>(out schema))
        //    {
        //        schema.ConnectionProviderType = ConfigManger.Provider.ConnectionSetting.ProviderTypeName;
        //        schema.ConnectionString = ConfigManger.Provider.ConnectionString;
        //    }
        //}

        /// <summary>
        /// The game service stop.
        /// </summary>
        public static void Stop()
        {
            IsRunning = false;
            CacheFactory.UpdateNotify(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task WaitStop()
        {
            while (!DataSyncQueueManager.IsRunCompleted)
            {
                await System.Threading.Tasks.Task.Delay(1000);
            }
            Console.WriteLine("Waiting exit.");
        }

    }
}