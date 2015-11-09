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
using System.Configuration;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ConfigReloadedEventArgs : EventArgs
    {

    }

    /// <summary>
    /// Config manage
    /// </summary>
    public sealed class ConfigManager
    {
        private static readonly object syncRoot = new object();
        private static HashSet<IConfigger> _configgerSet;
        private static IConfigger _configger;

        static ConfigManager()
        {
            _configgerSet = new HashSet<IConfigger>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<ConfigChangedEventArgs> ConfigChanged;

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<ConfigReloadedEventArgs> ConfigReloaded;


        /// <summary>
        /// Get current object.
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        public static IConfigger Configger
        {
            get
            {
                if (_configger == null)
                {
                    return GetConfigger<DefaultDataConfigger>();
                }
                return _configger;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool Intialize()
        {
            var configger = GetConfigger<DefaultDataConfigger>();
            var er = ConfigurationManager.ConnectionStrings.GetEnumerator();
            while (er.MoveNext())
            {
                var connSetting = er.Current as ConnectionStringSettings;
                if (connSetting == null) continue;

                configger.Add(new ConnectionSection(connSetting.Name, connSetting.ProviderName, connSetting.ConnectionString));
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName">custom cection node name</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static bool Intialize(string sectionName)
        {
            lock (syncRoot)
            {
                var section = ConfigurationManager.GetSection(sectionName);
                if (section is IConfigger)
                {
                    var instance = section as IConfigger;
                    instance.Install();
                    _configgerSet.Add(instance);
                    _configger = instance;
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetConfigger<T>() where T : IConfigger
        {
            return (T)GetConfigger(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IConfigger GetConfigger(Type type)
        {
            lock (syncRoot)
            {
                foreach (IConfigger configger in _configgerSet)
                {
                    if (configger.GetType() == type)
                    {
                        _configger = configger;
                        return configger;
                    }
                }
                var instance = type.CreateInstance<IConfigger>();
                instance.Install();
                _configgerSet.Add(instance);
                _configger = instance;
                return instance;
            }
        }

        internal static void OnConfigChanged(object sender, ConfigChangedEventArgs e)
        {
            EventHandler<ConfigChangedEventArgs> handler = ConfigChanged;
            if (handler != null) handler(sender, e);
        }

        internal static void OnConfigReloaded(object sender, ConfigReloadedEventArgs e)
        {
            EventHandler<ConfigReloadedEventArgs> handler = ConfigReloaded;
            if (handler != null) handler(sender, e);
        }
    }
}
