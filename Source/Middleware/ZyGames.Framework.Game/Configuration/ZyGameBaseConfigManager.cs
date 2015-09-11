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
using System.Web.Caching;
using System.Xml;
using ZyGames.Framework.Common.Timing;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 游戏配置管理类
    /// </summary>
    public static class ZyGameBaseConfigManager
    {
        private static readonly object thisLock = new object();
        private const string KeyName = "zyGameBaseBll";
        private static ZyGameBaseBllSection zyGameBaseBll;
        private static GameConfigSetting _gameConfigSetting = new GameConfigSetting();
        private static string _configFileName;

        /// <summary>
        /// 游戏配置
        /// </summary>
        internal static GameConfigSetting GameSetting
        {
            get
            {
                return _gameConfigSetting;
            }
        }

        private static ZyGameBaseBllSection BaseConfig
        {
            get
            {
                if (zyGameBaseBll == null)
                {
                    lock (thisLock)
                    {
                        if (zyGameBaseBll == null)
                        {
                            zyGameBaseBll = (ZyGameBaseBllSection)ConfigurationManager.GetSection(KeyName);
                        }
                    }
                }
                return zyGameBaseBll;
            }
        }

        /// <summary>
        /// 获取渠道登录处理提供类的配置
        /// </summary>
        /// <returns></returns>
        public static LoginElement GetLogin()
        {
            return BaseConfig != null ? BaseConfig.Login : null;
        }

        /// <summary>
        /// 获取战斗处理配置
        /// </summary>
        /// <returns></returns>
        public static CombatElement GetCombat()
        {
            return BaseConfig != null ? BaseConfig.Combat : null;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void Intialize()
        {
            string runtimePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            _configFileName = Path.Combine(runtimePath, "Game.config.xml");
            CacheListener routeListener = new CacheListener("__GAME_CONFIG", 0, (key, value, reason) =>
            {
				if (reason == CacheRemovedReason.Changed)
                {
                    _gameConfigSetting.Init(_configFileName);
                }
            }, _configFileName);
            routeListener.Start();

            _gameConfigSetting.Init(_configFileName);
        }

    }
}