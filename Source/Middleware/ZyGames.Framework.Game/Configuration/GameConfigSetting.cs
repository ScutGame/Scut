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
using System.Text;
using System.Xml;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 游戏配置类
    /// </summary>
    internal class GameConfigSetting
    {
        private Dictionary<string, string> _gmCommandDict = new Dictionary<string, string>();
        private Dictionary<string, GameLoginSetting> _loginDict = new Dictionary<string, GameLoginSetting>();
        private Dictionary<ChannelType, GameChannel> _sdkChannelDict = new Dictionary<ChannelType, GameChannel>();
        private bool _hasSetting;

        /// <summary>
        /// 是否有配置文件
        /// </summary>
        public bool HasSetting
        {
            get { return _hasSetting; }
        }

        internal void Init(string configFileName)
        {
            if (!File.Exists(configFileName))
            {
                _hasSetting = false;
                return;
            }
            _hasSetting = true;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFileName);

            var gmList = xmlDoc.SelectNodes("//gmCommand/add");
            if (gmList != null)
            {
                _gmCommandDict.Clear();
                foreach (XmlElement command in gmList)
                {
                    string cmd = command.GetAttribute("cmd");
                    string typeName = command.GetAttribute("type");
                    if (!string.IsNullOrEmpty(cmd) &&
                        !_gmCommandDict.ContainsKey(cmd))
                    {
                        _gmCommandDict.Add(cmd, typeName);
                    }
                }
            }

            var loginList = xmlDoc.SelectNodes("//login/add") ?? xmlDoc.SelectNodes("//login/retailList/add");
            if (loginList != null)
            {
                ParseLoginSetting(loginList);
            }

            var sdkChannel = xmlDoc.SelectSingleNode("//sdkChannel");
            if (sdkChannel != null)
            {
                ParseSdkSetting(sdkChannel);
            }
        }

        private void ParseLoginSetting(XmlNodeList loginList)
        {
            _loginDict.Clear();
            foreach (XmlElement item in loginList)
            {
                GameLoginSetting loginSetting = new GameLoginSetting();
                loginSetting.RetailId = item.GetAttribute("id");
                loginSetting.TypeName = item.GetAttribute("type");
                loginSetting.TypeArgs = item.GetAttribute("args");

                if (!_loginDict.ContainsKey(loginSetting.RetailId))
                {
                    _loginDict.Add(loginSetting.RetailId, loginSetting);
                }
            }
        }

        private void ParseSdkSetting(XmlNode sdkChannel)
        {
            _sdkChannelDict.Clear();
            foreach (var node in sdkChannel.ChildNodes)
            {
                XmlElement childNode = node as XmlElement;
                if (childNode == null) continue;

                ChannelType channelType;
                try
                {
                    channelType = MathUtils.ToEnum<ChannelType>(childNode.Name);
                }
                catch (Exception)
                {
                    TraceLog.WriteError("The SDK config node \"{0}\" is not suport", childNode.Name);
                    continue;
                }
                GameChannel gameChannel = new GameChannel(channelType);
                string url = childNode.GetAttribute("url");
                if (!string.IsNullOrEmpty(url))
                {
                    gameChannel.Url = url;
                }
                string service = childNode.GetAttribute("service");
                if (!string.IsNullOrEmpty(service))
                {
                    gameChannel.Service = service;
                }
                string version = childNode.GetAttribute("version");
                if (!string.IsNullOrEmpty(version))
                {
                    gameChannel.Version = version;
                }
                string tokenUrl = childNode.GetAttribute("token_url");
                if (!string.IsNullOrEmpty(tokenUrl))
                {
                    gameChannel.TokenUrl = tokenUrl;
                }
                string ctype = childNode.GetAttribute("type");
                if (!string.IsNullOrEmpty(ctype))
                {
                    gameChannel.CType = ctype;
                }
                string channelId = childNode.GetAttribute("channelId");
                if (!string.IsNullOrEmpty(channelId))
                {
                    gameChannel.ChannelId = channelId;
                }
                foreach (XmlElement item in childNode.ChildNodes)
                {
                    var setting = new GameSdkSetting();
                    setting.RetailId = item.GetAttribute("name");
                    if (channelType == ChannelType.channelUC)
                    {
                        setting.AppId = item.GetAttribute("cpId");
                        setting.AppKey = item.GetAttribute("apiKey");
                    }
                    else
                    {
                        setting.AppId = item.GetAttribute("appId");
                        setting.AppKey = item.GetAttribute("appKey");
                    }
                    setting.AppSecret = item.GetAttribute("appSecret");
                    setting.GameId = item.GetAttribute("gameId");
                    setting.ServerId = item.GetAttribute("serverId");
                    gameChannel.Add(setting);
                }
                if (!_sdkChannelDict.ContainsKey(gameChannel.ChannelType))
                {
                    _sdkChannelDict.Add(gameChannel.ChannelType, gameChannel);
                }
            }
        }

        /// <summary>
        /// 获得GM配置解析的对象类型
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string GetGmCommandType(string cmd)
        {
            return _gmCommandDict.ContainsKey(cmd) ? _gmCommandDict[cmd] : "";
        }

        /// <summary>
        /// 获得渠道登录处理提供类配置
        /// </summary>
        /// <param name="retailId"></param>
        /// <returns></returns>
        public GameLoginSetting GetLoginSetting(string retailId)
        {
            return _loginDict.ContainsKey(retailId) ? _loginDict[retailId] : null;
        }

        /// <summary>
        /// 获得渠道商提供的Sdk配置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameChannel GetChannelSetting(ChannelType type)
        {
            return _sdkChannelDict.ContainsKey(type) ? _sdkChannelDict[type] : null;
        }
    }
}