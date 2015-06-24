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
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Scut.SMS.Comm;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Build;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Script;

namespace Scut.SMS.Config
{
    /// <summary>
    /// The SCUTServer app setting.
    /// </summary>
    public class AppSetting
    {
        private static AppSetting _setting;
        private static string _fileName;
        public static string DictTypeNameFormat = "System.Collections.Generic.Dictionary`2[[System.String],[{0},{1}]]";

        public static AppSetting Current
        {
            get { return _setting; }
        }

        static AppSetting()
        {
            string configPath = Path.GetTempPath();
            _fileName = Path.Combine(configPath, "ScutSM", "setting.ini");
            _setting = new AppSetting();
            LoadConfig();
        }

        private static void LoadConfig()
        {
            try
            {
                if (File.Exists(_fileName))
                {
                    string data = File.ReadAllText(_fileName, Encoding.UTF8);
                    if (!string.IsNullOrEmpty(data))
                    {
                        _setting = JsonUtils.Deserialize<AppSetting>(data);
                    }
                    LoadAssembly();
                }
            }
            catch (Exception ex)
            {
            }
        }


        public static string LoadAssembly()
        {
            string error = "";
            if (_setting.Entity.IsScript)
            {
                ScriptCompiler.ClearTemp("smstemp");
                var refAsm = new string[] {
                        "NLog.dll",
                        "Newtonsoft.Json.dll",
                        "protobuf-net.dll",
                        "ZyGames.Framework.dll",
                        "ZyGames.Framework.Common.dll",
                        "ZyGames.Framework.Game.dll"
                    };

                AppDomain.CurrentDomain.AppendPrivatePath(Path.Combine(MathUtils.RuntimePath, "smstemp"));
                var files = Directory.GetFiles(_setting.Entity.ScriptPath, "*.cs", SearchOption.AllDirectories);
                var cr = ScriptCompiler.Compile(files, refAsm, "Entity.Model", false, false, "smstemp");
                if (cr != null)
                {
                    var assembly = cr.CompiledAssembly;
                    _setting.Entity.DymincEntity = assembly;
                }
                else
                {
                    error = "Compile fail";
                }
            }
            else
            {
                AppDomain.CurrentDomain.AppendPrivatePath(Path.GetFullPath(_setting.Entity.AssemblyPath));
                _setting.Entity.DymincEntity = AssemblyBuilder.ReadAssembly(_setting.Entity.AssemblyPath, null);
            }
            try
            {
                ProtoBufUtils.LoadProtobufType(_setting.Entity.DymincEntity);
            }
            catch (Exception)
            {
            }
            return error;
        }

        public void Save()
        {
            try
            {
                using (var stream = File.CreateText(_fileName))
                {
                    string data = JsonUtils.Serialize(_setting);
                    data = JsonHelper.FormatJsonToString(data);
                    stream.Write(data);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private AppSetting()
        {
            Entity = new EntitySetting();
            Contract = new ContractSetting();
        }

        public EntitySetting Entity { get; set; }

        public ContractSetting Contract { get; set; }

    }

    public class EntitySetting
    {
        public EntitySetting()
        {
            IsScript = true;
        }
        public bool IsScript { get; set; }
        public string ScriptPath { get; set; }
        public string AssemblyPath { get; set; }

        [JsonIgnore]
        public Assembly DymincEntity { get; set; }
    }

    public enum DBType
    {
        SQL,
        MySql
    }
    public class ContractSetting
    {
        public ContractSetting()
        {
            CaseNameFormat = "Step{0}";
            CaseOutPath = "Case";
        }
        public string Server { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public DBType DBType { get; set; }
        public int Port { get; set; }

        [JsonIgnore]
        public bool IsProxyServer { get { return !string.IsNullOrEmpty(Database); } }

        public string CaseNameFormat { get; set; }
        public string CaseOutPath { get; set; }
    }
}