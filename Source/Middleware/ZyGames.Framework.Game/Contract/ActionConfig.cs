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
using System.Net;
using System.Net.Sockets;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Game.Contract
{
    internal class ActionConfig
    {
        private static ActionConfig instance;

        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
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

        static ActionConfig()
        {
            instance = new ActionConfig();
            instance.TypeName = ConfigUtils.GetSetting("Game.Action.TypeName");
            if (string.IsNullOrEmpty(instance.TypeName))
            {
                string assemblyName = ConfigUtils.GetSetting("Game.Action.AssemblyName");
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    instance.TypeName = assemblyName + ".Action.Action{0}," + assemblyName;
                }
            }
            instance.ScriptTypeName = ConfigUtils.GetSetting("Game.Action.Script.TypeName", "Game.Script.Action{0}");
            instance.IpAddress = ConfigUtils.GetSetting("Game.IpAddress");
            if (string.IsNullOrEmpty(instance.IpAddress))
            {
                instance.IpAddress = GetLocalIp();
            }
            instance.Port = ConfigUtils.GetSetting("Game.Port", 9101);
            instance.IgnoreAuthorizeSet = new HashSet<int>();
        }

        public static ActionConfig Current
        {
            get { return instance; }
        }

        public string IpAddress
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        /// 对不采用脚本方式的请求动作支持
        /// </summary>
        public string TypeName
        {
            get;
            private set;
        }

        /// <summary>
        /// CSharp脚本类型名
        /// </summary>
        public string ScriptTypeName
        {
            get;
            private set;
        }

        public HashSet<int> IgnoreAuthorizeSet { get; set; }

    }
}