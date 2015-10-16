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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ScutServerManager.Config
{
    public enum PublishType
    {
        Debug,
        Release
    }

    public enum StorageMode
    {
        Protobuf,
        Json
    }

    public class GameSetting
    {
        public const string DbPaycenter = "PayCenter";
        public const string DbSnscenter = "SnsCenter";
        //private const string CatBasic = "Basic";
        private const string CatOptions = "Basic Options";
        private const string CatDbConfig = "Basic Db";
        private const string CatService = "Service";
        private const string CatScript = "Script";
        private const string CatCache = "Cache";
        private const string CatLog = "Log";
        private const string CatMiddleware = "Middleware";

        private const string CatNormal = "Basic";
        private const string CatNormalScript = "Basic Script";

        public GameSetting()
        {
            IsModify = true;
            Redis = new RedisSetting();
            PayCenter = new ConnectionString() { Name = DbPaycenter, Database = "PayDB" };
            SnsCenter = new ConnectionString() { Name = DbSnscenter, Database = "SnsCenter" };
            Connections = new List<ConnectionString>();

            GameCode = DefaultConfig.GameCode;
            GameServerCode = DefaultConfig.ServerCode;
            SocketPort = DefaultConfig.SocketPort;
            GameName = DefaultConfig.GameName;
            GameSignKey = DefaultConfig.GameSignKey;

            HttpHost = DefaultConfig.HttpHost;
            HttpPort = DefaultConfig.HttpPort;
            HttpName = DefaultConfig.HttpName;
            HttpTimeout = DefaultConfig.HttpTimeout;
            SocketMaxConnections = DefaultConfig.SocketMaxConnections;
            SocketBacklog = DefaultConfig.SocketBacklog;
            SocketMaxAcceptOps = DefaultConfig.SocketMaxAcceptOps;
            SocketBufferSize = DefaultConfig.SocketBufferSize;

            ActionEnableGZip = DefaultConfig.ActionEnableGZip;
            ActionGZipOutLength = DefaultConfig.ActionGZipOutLength;
            ActionTypeName = DefaultConfig.ActionTypeName;
            ActionAssemblyName = DefaultConfig.ActionAssemblyName;
            ActionScriptTypeName = DefaultConfig.ActionScriptTypeName;
            PythonDisable = DefaultConfig.PythonDisable;
            ScriptIsDebug = DefaultConfig.ScriptIsDebug;
            LuaDisable = DefaultConfig.LuaDisable;

            //Scut lib 6.1.5.6 modify
            ScriptRelativePath = "Script";
            CSharpRootPath = "CsScript";
            LuaRootPath = DefaultConfig.LuaRootPath;
            PythonRootPath = DefaultConfig.PythonRootPath;
            ScriptMainClass = DefaultConfig.ScriptMainClass;
            ScriptMainTypeName = DefaultConfig.ScriptMainTypeName;
            ScriptAsmReferences = new string[0];
            ScriptSysAsmReferences = new string[0];
            ModelEntityAssemblyName = DefaultConfig.ModelEntityAssemblyName;
            ScriptDecodeFuncTypeName = DefaultConfig.ScriptDecodeFuncTypeName;
            RemoteScriptTypeName = DefaultConfig.RemoteScriptTypeName;

            CacheGlobalPeriod = DefaultConfig.CacheGlobalPeriod;
            CacheUserPeriod = DefaultConfig.CacheUserPeriod;
            CacheUpdateDbInterval = DefaultConfig.CacheUpdateDbInterval;
            CacheUpdateInterval = DefaultConfig.CacheUpdateInterval;
            CacheExpiredInterval = DefaultConfig.CacheExpiredInterval;
            CacheEnableWritetoDb = DefaultConfig.CacheEnableWritetoDb;
            DataSyncQueueNum = DefaultConfig.DataSyncQueueNum;
            SqlWaitSyncQueueNum = DefaultConfig.SqlWaitSyncQueueNum;
            SqlSyncQueueNum = DefaultConfig.SqlSyncQueueNum;
            CacheSerializer = DefaultConfig.CacheSerializer;

            LogTableNameFormat = DefaultConfig.LogTableNameFormat;
            LogPriorBuildMonth = DefaultConfig.LogPriorBuildMonth;

            PublishType = DefaultConfig.DefPublishType;
            EnableGm = DefaultConfig.EnableGm;
            SnsAccountPrefixChar = DefaultConfig.SnsAccountPrefixChar;
            LanguageTypeName = DefaultConfig.LanguageTypeName;
            ProductDesEnKey = DefaultConfig.ProductDesEnKey;
            ClientDesDeKey = DefaultConfig.ClientDesDeKey;

        }


        [BrowsableAttribute(false)]
        public bool IsModify { get; set; }

        #region base config
        [CategoryAttribute(CatNormal),
        DefaultValue(DefaultConfig.GameCode),
        DescriptionAttribute("code of the game product.")]
        public int GameCode { get; set; }

        [CategoryAttribute(CatNormal),
        DefaultValue(DefaultConfig.ServerCode),
        DescriptionAttribute("code of the game server.")]
        public int GameServerCode { get; set; }

        [CategoryAttribute(CatNormal),
        DescriptionAttribute("Redis connection setting.")]
        public RedisSetting Redis { get; set; }

        #endregion

        #region Options

        [CategoryAttribute(CatOptions),
        DefaultValue(DefaultConfig.GameName),
        DescriptionAttribute("game name.")]
        public string GameName { get; set; }

        [CategoryAttribute(CatOptions),
        DefaultValue(DefaultConfig.GameSignKey),
        DescriptionAttribute("Receiving a request signature key parameters.")]
        public string GameSignKey { get; set; }

        [CategoryAttribute(CatOptions),
        DefaultValue(DefaultConfig.EnableGm),
        DescriptionAttribute("Enable GM command.")]
        public bool EnableGm { get; set; }

        [CategoryAttribute(CatOptions),
        DefaultValue(DefaultConfig.DefPublishType),
        DescriptionAttribute("Open server error info switch.")]
        public PublishType PublishType { get; set; }

        [CategoryAttribute(CatOptions),
        DefaultValue(DefaultConfig.LanguageTypeName),
        DescriptionAttribute("Language type name, ex:Game.src.Locale.SimplifiedLanguage.")]
        public string LanguageTypeName { get; set; }

        [CategoryAttribute(CatOptions),
        DefaultValue(DefaultConfig.ProductDesEnKey),
        DescriptionAttribute("Plus the user's password to do symmetric decryption key .")]
        public string ProductDesEnKey { get; set; }

        [CategoryAttribute(CatOptions),
        DefaultValue(DefaultConfig.ClientDesDeKey),
        DescriptionAttribute("Client requests a password do symmetric decryption key.")]
        public string ClientDesDeKey { get; set; }
        #endregion

        #region DBConfig
        [CategoryAttribute(CatDbConfig),
        DescriptionAttribute("Connection of the PayCenter db.")]
        public ConnectionString PayCenter { get; set; }

        [CategoryAttribute(CatDbConfig),
        DescriptionAttribute("Connection of the SnsCenter db.")]
        public ConnectionString SnsCenter { get; set; }

        [CategoryAttribute(CatDbConfig),
        DescriptionAttribute("Connection of game use db.")]
        public List<ConnectionString> Connections { get; set; }

        #endregion

        #region Service

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.HttpHost),
        DescriptionAttribute("http request Ip or Dns host.")]
        public string HttpHost { get; set; }

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.HttpPort),
        DescriptionAttribute("http request port.")]
        public int HttpPort { get; set; }

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.HttpName),
        DescriptionAttribute("http request app name.")]
        public string HttpName { get; set; }

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.HttpTimeout),
        DescriptionAttribute("http request timeout,Unit:ms.")]
        public int HttpTimeout { get; set; }

        [CategoryAttribute(CatNormal),
        DefaultValue(DefaultConfig.SocketPort),
        DescriptionAttribute("Socket listening port of the game server.")]
        public int SocketPort { get; set; }

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.SocketMaxConnections),
        DescriptionAttribute("socket max connections.")]
        public int SocketMaxConnections { get; set; }

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.SocketBacklog),
        DescriptionAttribute("socket backlog.")]
        public int SocketBacklog { get; set; }

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.SocketMaxAcceptOps),
        DescriptionAttribute("socket Max accept connection.")]
        public int SocketMaxAcceptOps { get; set; }

        [CategoryAttribute(CatService),
        DefaultValue(DefaultConfig.SocketBufferSize),
        DescriptionAttribute("socket buffer size, default 8k.")]
        public int SocketBufferSize { get; set; }

        #endregion

        #region Script

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ActionEnableGZip),
        DescriptionAttribute("action response is enable GZip.")]
        public bool ActionEnableGZip { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ActionGZipOutLength),
        DescriptionAttribute("action out length(10k) use GZip.")]
        public int ActionGZipOutLength { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ActionTypeName),
        DescriptionAttribute("action type name format string, ex:\"DemoGame.Script.Action.Action{0},DemoGame.Script\".")]
        public string ActionTypeName { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ActionAssemblyName),
        DescriptionAttribute("action assembly name, ex:\"DemoGame.Script\".")]
        public string ActionAssemblyName { get; set; }

        [CategoryAttribute(CatNormalScript),
        DefaultValue(DefaultConfig.ActionScriptTypeName),
        DescriptionAttribute("action script type name format string, ex:\"DemoGame.Script.Action.Action{0}\".")]
        public string ActionScriptTypeName { get; set; }

        [CategoryAttribute(CatNormalScript),
        DefaultValue(DefaultConfig.PythonDisable),
        DescriptionAttribute("Python script disable switch.")]
        public bool PythonDisable { get; set; }

        [CategoryAttribute(CatNormalScript),
        DefaultValue(DefaultConfig.LuaDisable),
        DescriptionAttribute("Lua script disable switch.")]
        public bool LuaDisable { get; set; }


        [CategoryAttribute(CatNormalScript),
        DefaultValue(DefaultConfig.ScriptIsDebug),
        DescriptionAttribute("Python script is debug switch.")]
        public bool ScriptIsDebug { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ScriptRelativePath),
        DescriptionAttribute("Script relative path.")]
        public string ScriptRelativePath { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.CSharpRootPath),
        DescriptionAttribute("CSharp script listening root path.")]
        public string CSharpRootPath { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.PythonRootPath),
        DescriptionAttribute("Python script listening root path.")]
        public string PythonRootPath { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.LuaRootPath),
        DescriptionAttribute("Lua script listening root path.")]
        public string LuaRootPath { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ScriptMainClass),
        DescriptionAttribute("CSharp script main class file.")]
        public string ScriptMainClass { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ScriptMainTypeName),
        DescriptionAttribute("CSharp script main class type.")]
        public string ScriptMainTypeName { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ModelEntityAssemblyName),
        DescriptionAttribute("Model entity assembly name.")]
        public string ModelEntityAssemblyName { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.ScriptDecodeFuncTypeName),
        DescriptionAttribute("Script DecodeFunc type name. ex:ScutSecurity.ScriptDes,ScutSecurity")]
        public string ScriptDecodeFuncTypeName { get; set; }

        [CategoryAttribute(CatScript),
        DefaultValue(DefaultConfig.RemoteScriptTypeName),
        DescriptionAttribute("Remote script type name.")]
        public string RemoteScriptTypeName { get; set; }

        [CategoryAttribute(CatScript),
        DescriptionAttribute("CSharp script assmebly references, use ';' split.")]
        public string[] ScriptAsmReferences { get; set; }

        [CategoryAttribute(CatScript),
        DescriptionAttribute("CSharp script system assmebly references, use ';' split.")]
        public string[] ScriptSysAsmReferences { get; set; }
        #endregion

        #region Cache
        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.CacheGlobalPeriod),
        DescriptionAttribute("Global cache lifecycle, unit:second(3 day).")]
        public int CacheGlobalPeriod { get; set; }

        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.CacheUserPeriod),
        DescriptionAttribute("User cache lifecycle, unit:second(24 hour).")]
        public int CacheUserPeriod { get; set; }

        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.CacheUpdateDbInterval),
        DescriptionAttribute("Update cache to the database interval, unit:ms(5 Minute).")]
        public int CacheUpdateDbInterval { get; set; }

        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.CacheUpdateInterval),
        DescriptionAttribute("Update the cache item has be changed interval, unit:second(10 Minute).")]
        public int CacheUpdateInterval { get; set; }

        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.CacheExpiredInterval),
        DescriptionAttribute("Cache cleanup interval expired, unit:second(10 Minute).")]
        public int CacheExpiredInterval { get; set; }

        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.CacheEnableWritetoDb),
        DescriptionAttribute("Cache enable write to Db.")]
        public bool CacheEnableWritetoDb { get; set; }


        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.DataSyncQueueNum),
        DescriptionAttribute("Cache data sync to redis queue num.")]
        public int DataSyncQueueNum { get; set; }

        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.SqlWaitSyncQueueNum),
        DescriptionAttribute("Cache data wait generate Sql queue num.")]
        public int SqlWaitSyncQueueNum { get; set; }

        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.SqlSyncQueueNum),
        DescriptionAttribute("Cache data sync to db queue num.")]
        public int SqlSyncQueueNum { get; set; }


        [CategoryAttribute(CatCache),
        DefaultValue(DefaultConfig.CacheSerializer),
        DescriptionAttribute("Cache serialize to redis model(protobuf/json).")]
        public StorageMode CacheSerializer { get; set; }
        #endregion

        #region Log

        [CategoryAttribute(CatLog),
        DefaultValue(DefaultConfig.LogTableNameFormat),
        DescriptionAttribute("Log table format string of db, ex:log_201401UserLoginLog($date=yyyyMM).")]
        public string LogTableNameFormat { get; set; }

        [CategoryAttribute(CatLog),
        DefaultValue(DefaultConfig.LogPriorBuildMonth),
        DescriptionAttribute("How many months of pre-built log table.")]
        public int LogPriorBuildMonth { get; set; }

        #endregion

        #region Middleware

        [CategoryAttribute(CatMiddleware),
        DefaultValue("Z"),
        DescriptionAttribute("SNS Center account format string prefix char, ex:Z10000,Z10001.")]
        public string SnsAccountPrefixChar { get; set; }

        #endregion


    }
}