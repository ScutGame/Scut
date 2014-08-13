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
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Microsoft.Scripting.Hosting;
using ServiceStack.Text;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// The environment configuration information.
    /// </summary>
    [Serializable]
    public class EnvironmentSetting
    {
        private static readonly string productDesEnKey;
        private static readonly string clientDesDeKey;
        private static readonly string productSignKey;
        private static readonly int productCode;
        private static readonly string productName;
        private static readonly int productServerId;
        private static readonly string gameIpAddress;
        private static readonly int gamePort;
        private static readonly int cacheGlobalPeriod;
        private static readonly int cacheUserPeriod;
        private static readonly string[] scriptSysAsmReferences;
        private static readonly string[] scriptAsmReferences;
        private static readonly bool enableActionGZip;
        private static readonly int actionGZipOutLength;
        private static readonly string actionTypeName;
        private static readonly string scriptTypeName;
        private static readonly string entityAssemblyName;
        private static readonly string decodeFuncTypeName;
        private static readonly string remoteTypeName;
        private static ICacheSerializer serializer;

        static EnvironmentSetting()
        {
            productDesEnKey = ConfigUtils.GetSetting("Product.DesEnKey", "BF3856AD");
            clientDesDeKey = ConfigUtils.GetSetting("Product.ClientDesDeKey", "n7=7=7dk");
            productSignKey = ConfigUtils.GetSetting("Product.SignKey", "");
            productCode = ConfigUtils.GetSetting("Product.Code", 1);
            productName = ConfigUtils.GetSetting("Product.Name", "Game");
            productServerId = ConfigUtils.GetSetting("Product.ServerId", 1);
            gameIpAddress = ConfigUtils.GetSetting("Game.IpAddress");
            if (string.IsNullOrEmpty(gameIpAddress))
            {
                gameIpAddress = GetLocalIp();
            }
            gamePort = ConfigUtils.GetSetting("Game.Port", 9101);
            cacheGlobalPeriod = ConfigUtils.GetSetting("Cache.global.period", 3 * 86400); //72 hour
            cacheUserPeriod = ConfigUtils.GetSetting("Cache.user.period", 86400); //24 hour

            scriptSysAsmReferences = ConfigUtils.GetSetting("ScriptSysAsmReferences", "").Split(';');
            scriptAsmReferences = ConfigUtils.GetSetting("ScriptAsmReferences", "").Split(';');
            enableActionGZip = ConfigUtils.GetSetting("Game.Action.EnableGZip", true);
            actionGZipOutLength = ConfigUtils.GetSetting("Game.Action.GZipOutLength", 10240);//10k

            actionTypeName = ConfigUtils.GetSetting("Game.Action.TypeName");
            if (string.IsNullOrEmpty(actionTypeName))
            {
                string assemblyName = ConfigUtils.GetSetting("Game.Action.AssemblyName", "GameServer.CsScript");
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    actionTypeName = assemblyName + ".Action.Action{0}," + assemblyName;
                }
            }
            scriptTypeName = ConfigUtils.GetSetting("Game.Action.Script.TypeName", "Game.Script.Action{0}");
            entityAssemblyName = ConfigUtils.GetSetting("Game.Entity.AssemblyName");
            decodeFuncTypeName = ConfigUtils.GetSetting("Game.Script.DecodeFunc.TypeName", "");

            remoteTypeName = ConfigUtils.GetSetting("Game.Remote.Script.TypeName", "Game.Script.Remote.{0}");
            LoadDecodeFunc();
            InitSerializer();
        }

        private static void InitSerializer()
        {
            string type = ConfigUtils.GetSetting("Cache.Serializer", "Protobuf");
            if (string.Equals(type, "json", StringComparison.OrdinalIgnoreCase))
            {
                serializer = new JsonCacheSerializer(Encoding.UTF8);
            }
            else
            {
                serializer = new ProtobufCacheSerializer();
            }
        }

        private static string GetLocalIp()
        {
            string localIp = "";
            IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
            foreach (var ipAddress in addressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ipAddress.ToString();
                    break;
                }
            }
            return localIp;
        }
        /// <summary>
        /// Object Initialization.
        /// </summary>
        public EnvironmentSetting()
        {
            ProductDesEnKey = productDesEnKey;
            ClientDesDeKey = clientDesDeKey;
            ProductSignKey = productSignKey;
            ProductCode = productCode;
            ProductName = productName;
            ProductServerId = productServerId;
            CacheGlobalPeriod = cacheGlobalPeriod;
            CacheUserPeriod = cacheUserPeriod;

            ScriptSysAsmReferences = scriptSysAsmReferences;
            ScriptAsmReferences = scriptAsmReferences;
            ActionEnableGZip = enableActionGZip;
            ActionGZipOutLength = actionGZipOutLength;
            GamePort = gamePort;
            GameIpAddress = gameIpAddress;
            ActionTypeName = actionTypeName;
            ScriptTypeName = scriptTypeName;
            RemoteTypeName = remoteTypeName;
            try
            {
                if (!string.IsNullOrEmpty(entityAssemblyName))
                {
                    EntityAssembly = Assembly.LoadFrom(entityAssemblyName);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Load entity assembly error:\"{0}\" {1}", entityAssemblyName, ex);
            }
            ActionDispatcher = new ScutActionDispatcher();
            Serializer = serializer;
        }

        private static dynamic _scriptDecodeTarget;
        private static void LoadDecodeFunc()
        {
            try
            {
                if (string.IsNullOrEmpty(decodeFuncTypeName)) return;
                var type = Type.GetType(decodeFuncTypeName, true, true);
                _scriptDecodeTarget = type.CreateInstance();
                ScriptEngines.SettupInfo.DecodeCallback += DecodeCallback;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Load DecodeFunc type error:\"{0}\" {1}", decodeFuncTypeName, ex);
            }
        }

        private static string DecodeCallback(string source, string ext)
        {
            if (_scriptDecodeTarget == null)
                return "";
            return _scriptDecodeTarget.DecodeCallback(source, ext);
        }


        /// <summary>
        /// Request signature key.
        /// </summary>
        public string ProductSignKey { get; set; }

        /// <summary>
        /// Des encryption key account password.
        /// </summary>
        public string ProductDesEnKey { get; set; }

        /// <summary>
        /// Des decryption for client password.
        /// </summary>
        public string ClientDesDeKey { get; set; }

        /// <summary>
        /// Global cache lifecycle.
        /// </summary>
        public int CacheGlobalPeriod { get; set; }

        /// <summary>
        /// Game players cache lifecycle.
        /// </summary>
        public int CacheUserPeriod { get; set; }

        /// <summary>
        /// Product code.
        /// </summary>
        public int ProductCode { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product server id.
        /// </summary>
        public int ProductServerId { get; set; }

        /// <summary>
        /// The entity assembly.
        /// </summary>
        public Assembly EntityAssembly { get; set; }

        /// <summary>
        /// Script use system assembly reference.
        /// </summary>
        public string[] ScriptSysAsmReferences { get; set; }

        /// <summary>
        /// Script use other assembly reference.
        /// </summary>
        public string[] ScriptAsmReferences { get; set; }

        /// <summary>
        /// enable gzip
        /// </summary>
        public bool ActionEnableGZip { get; set; }

        /// <summary>
        /// stream out length use gzip.
        /// </summary>
        public int ActionGZipOutLength { get; set; }

        /// <summary>
        /// Action type name.
        /// </summary>
        public string ActionTypeName { get; set; }

        /// <summary>
        /// CSharp script type name.
        /// </summary>
        public string ScriptTypeName { get; set; }

        /// <summary>
        /// Remote type name.
        /// </summary>
        public string RemoteTypeName { get; set; }
        /// <summary>
        /// local ip
        /// </summary>
        public string GameIpAddress
        {
            get;
            private set;
        }

        /// <summary>
        /// socket port
        /// </summary>
        public int GamePort
        {
            get;
            private set;
        }

        /// <summary>
        /// Action repeater
        /// </summary>
        public IActionDispatcher ActionDispatcher { get; set; }

        public ICacheSerializer Serializer { get; set; }
        ///// <summary>
        ///// Before starting the script engine process.
        ///// </summary>
        //public event Action ScriptStartBeforeHandle;

        //internal void OnScriptStartBefore()
        //{
        //    if (ScriptStartBeforeHandle != null)
        //    {
        //        ScriptStartBeforeHandle();
        //    }
        //}

    }
}