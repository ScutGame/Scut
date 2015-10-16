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
using System.Linq;
using System.Text;

namespace ScutServerManager.Config
{
    public static class DefaultConfig
    {
        public const int GameCode = 0;
        public const int ServerCode = 0;
        public const int SocketPort = 9101;
        public const string RedisHost = "localhost";
        public const string ReadOnlyHost = "localhost";
        public const int RedisDb = 0;
        public const int MaxWritePoolSize = 100;
        public const int MaxReadPoolSize = 100;
        public const int ConnectTimeout = 0;
        public const int PoolTimeOut = 0;

        public const string GameName = "";
        public const string GameSignKey = "";

        public const string HttpHost = "";
        public const int HttpPort = 80;
        public const string HttpName = "Service.aspx";
        public const int HttpTimeout = 120000;
        public const int SocketMaxConnections = 10000;
        public const int SocketBacklog = 1000;
        public const int SocketMaxAcceptOps = 1000;
        public const int SocketBufferSize = 8192;

        public const bool ActionEnableGZip = true;
        public const int ActionGZipOutLength = 10240;
        public const string ActionTypeName = "";
        public const string ActionAssemblyName = "";
        public const string ActionScriptTypeName = "Game.Script.Action{0}";
        public const bool ScriptIsDebug = false;
        public const bool PythonDisable = true;
        public const bool LuaDisable = true;
        public const string ScriptRelativePath = "";
        public const string CSharpRootPath = "Script";
        public const string PythonRootPath = "PyScript";
        public const string LuaRootPath = "LuaScript";
        public const string ScriptMainClass = "MainClass.cs";
        public const string ScriptMainTypeName = "Game.Script.MainClass";
        public const string ModelEntityAssemblyName = "";
        public const string ScriptDecodeFuncTypeName = "";
        public const string RemoteScriptTypeName = "Game.Script.Remote.{0}"; 


        public const int CacheGlobalPeriod = 259200;
        public const int CacheUserPeriod = 86400;
        public const int CacheUpdateDbInterval = 300000;
        public const int CacheUpdateInterval = 600;
        public const int CacheExpiredInterval = 600;
        public const bool CacheEnableWritetoDb = true;
        public const int DataSyncQueueNum = 2;
        public const int SqlWaitSyncQueueNum = 2;
        public const int SqlSyncQueueNum = 2;
        public const StorageMode  CacheSerializer = StorageMode.Protobuf;

        public const string LogTableNameFormat = "log_$date{0}";
        public const int LogPriorBuildMonth = 3;

        public const string DataSource = "localhost";
        public const string MysqlCharset = "gbk";
        public const int MysqlPort = 3306;

        public const int SqlPort = 1433;

        public const PublishType DefPublishType = PublishType.Release;
        public const bool EnableGm = false;
        public const string SnsAccountPrefixChar = "Z";
        public const string LanguageTypeName = "Game.src.Locale.DefaultLanguage";
        public const string ProductDesEnKey = "BF3856AD";
        public const string ClientDesDeKey = "n7=7=7dk";

    }
}