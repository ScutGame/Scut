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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Data;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultAppConfigger : DefaultDataConfigger
    {
        /// <summary>
        /// init
        /// </summary>
        public DefaultAppConfigger()
        {
            ConfigFile = Path.Combine(MathUtils.RuntimePath, "GameServer.exe.config");
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void LoadConfigData()
        {
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("connectionStrings");
            var er = ConfigurationManager.ConnectionStrings.GetEnumerator();
            while (er.MoveNext())
            {
                var connSetting = er.Current as ConnectionStringSettings;
                if (connSetting == null) continue;
                AddNodeData(new ConnectionSection(connSetting.Name, connSetting.ProviderName, connSetting.ConnectionString));
            }
            var setting = GameEnvironment.Setting;
            setting.Reset();
            MessageStructure.EnableGzip = setting.ActionEnableGZip;
            MessageStructure.EnableGzipMinByte = setting.ActionGZipOutLength;
            base.LoadConfigData();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppServerConfigger : DataConfigger
    {
        /// <summary>
        /// init
        /// </summary>
        public AppServerConfigger()
        {
            ConfigFile = Path.Combine(MathUtils.RuntimePath, "AppServer.config");
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void LoadConfigData()
        {

        }
    }
}
