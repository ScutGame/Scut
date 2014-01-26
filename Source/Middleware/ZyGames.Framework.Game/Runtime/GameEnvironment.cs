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
using System.Reflection;
using System.Threading;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Model;
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
        /// <summary>
        /// The python script task cache key.
        /// </summary>
        public static readonly string PythonScriptTaskCacheKey = "__PythonScript_Task";

        private static int _isRunning;

        //static GameEnvironment()
        //{
        //    _setting = new EnvironmentSetting();
        //    _setting.EnableCacheListener = true;
        //    _setting.ProductDesEnKey = "BF3856AD";
        //    _setting.ClientDesDeKey = "n7=7=7dk";
        //    _setting.ProductSignKey = ConfigUtils.GetSetting("Product.SignKey", "");
        //    _setting.ProductCode = ConfigUtils.GetSetting("Product.Code", 1);
        //    _setting.ProductName = ConfigUtils.GetSetting("Product.Name", "Game");
        //    _setting.ProductServerId = ConfigUtils.GetSetting("Product.ServerId", 1);
        //    _setting.CacheGlobalPeriod = ConfigUtils.GetSetting("Cache.global.period", 3 * 86400); //72 hour
        //    _setting.CacheUserPeriod = ConfigUtils.GetSetting("Cache.user.period", 86400); //24 hour
        //}

        private static EnvironmentSetting _setting;
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
        public static int ProductCode { get { return _setting.ProductCode; } }

        /// <summary>
        /// 产品名称
        /// </summary>
        public static string ProductName { get { return _setting.ProductName; } }

        /// <summary>
        /// 游戏服代码
        /// </summary>
        public static int ProductServerId { get { return _setting.ProductServerId; } }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsRunning
        {
            get { return _isRunning == 1; }
        }

        /// <summary>
        /// Initialize entity cache.
        /// </summary>
        public static void InitializeCache()
        {
            CacheFactory.Initialize(new CacheSetting());
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
            _setting = setting;
            DbConnectionProvider.Initialize();
            EntitySchemaSet.CacheGlobalPeriod = _setting.CacheGlobalPeriod;
            EntitySchemaSet.CacheUserPeriod = _setting.CacheUserPeriod;
            if (_setting.EntityAssembly != null)
            {
                ProtoBufUtils.LoadProtobufType(_setting.EntityAssembly);
                EntitySchemaSet.LoadAssembly(_setting.EntityAssembly);
            }
            EntitySchemaSet.StartCheckTableTimer();
            LoadGameEntitySchema();
            ZyGameBaseConfigManager.Intialize();
            CacheFactory.Initialize(cacheSetting);
            Global = new ContextCacheSet<CacheItem>("__gameenvironment_global");
            //init script.
            ScriptEngines.AddReferencedAssembly("ZyGames.Framework.Game.dll");
            ScriptEngines.RegisterModelChangedBefore(OnModelChangeBefore);
            ScriptEngines.RegisterModelChangedAfter(OnModelChangeAtfer);
            _setting.OnScriptStartBefore();
            ScriptEngines.Initialize();
            Interlocked.Exchange(ref _isRunning, 1);
        }

        private static void OnModelChangeBefore(Assembly assembly)
        {
            try
            {
                Interlocked.Exchange(ref _isRunning, 0);
                TraceLog.ReleaseWrite("Updating model script...");
                CacheFactory.UpdateNotify(true);
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    while (!CacheFactory.CheckCompleted())
                    {
                        Thread.Sleep(100);
                    }
                });
                System.Threading.Tasks.Task.WaitAll(task);
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
                ProtoBufUtils.Initialize();
                ProtoBufUtils.LoadProtobufType(assembly);
                EntitySchemaSet.LoadAssembly(assembly);
                Language.Reset();
                TraceLog.ReleaseWrite("Update Model script success.");
                Interlocked.Exchange(ref _isRunning, 1);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnModelChangeAtfer error:{0}", ex);
            }
        }


        private static void LoadGameEntitySchema()
        {
            SchemaTable schema;
            if (EntitySchemaSet.TryGet<SensitiveWord>(out schema))
            {
                schema.ConnectionProviderType = ConfigManger.Provider.ConnectionSetting.ProviderTypeName;
                schema.ConnectionString = ConfigManger.Provider.ConnectionString;
            }
        }

        /// <summary>
        /// The game service stop.
        /// </summary>
        public static void Stop()
        {
            CacheFactory.UpdateNotify(true);
            CacheFactory.Dispose();
            Interlocked.Exchange(ref _isRunning, 0);
        }

    }
}