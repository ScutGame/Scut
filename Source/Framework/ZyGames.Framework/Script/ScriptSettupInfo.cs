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
using System.Reflection;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Config;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public delegate string ScriptDecodeHandle(string source, string ext);

    /// <summary>
    /// Script settup info
    /// </summary>
    [Serializable]
    public class ScriptSettupInfo
    {
        /// <summary>
        /// init
        /// </summary>
        public ScriptSettupInfo()
        {
            ScriptSection section = ConfigManager.Configger.GetFirstOrAddConfig<ScriptSection>();

            //init runtime path.
            ScriptChangedDelay = section.ScriptChangedDelay;
            RuntimePath = section.RuntimePath;
            RuntimePrivateBinPath = section.RuntimePrivateBinPath;
            ScriptRelativePath = section.ScriptRelativePath;
            ScriptIsDebug = section.ScriptIsDebug;

            ModelScriptPath = section.ModelScriptPath;
            CSharpScriptPath = section.CSharpScriptPath;
            ScriptMainProgram = section.ScriptMainProgram;
            ScriptMainTypeName = section.ScriptMainTypeName;

            //Py setting
            DisablePython = section.DisablePython;
            PythonIsDebug = section.PythonIsDebug;
            PythonScriptPath = section.PythonScriptPath;
            PythonReferenceLibFile = section.PythonReferenceLibFile;

            //Lua setting
            DisableLua = section.DisableLua;
            LuaScriptPath = section.LuaScriptPath;

            ReferencedAssemblyNames = new List<string>{
                Path.Combine(RuntimePrivateBinPath, "NLog.dll"),
                Path.Combine(RuntimePrivateBinPath, "Newtonsoft.Json.dll"), 
                Path.Combine(RuntimePrivateBinPath, "protobuf-net.dll"),
                Path.Combine(RuntimePrivateBinPath, "ServiceStack.Redis.dll"),
                Path.Combine(RuntimePrivateBinPath, "ZyGames.Framework.Common.dll"),
                Path.Combine(RuntimePrivateBinPath, "ZyGames.Framework.dll")
            };
        }

        /// <summary>
        /// 是否取消执行脚本
        /// </summary>
        public bool IsCancelRunning { get; set; }

        /// <summary>
        /// 脚本文件改变延迟时间(ms)
        /// </summary>
        public int ScriptChangedDelay { get; set; }

        /// <summary>
        /// 脚本当前运行目录
        /// </summary>
        public string RuntimePath { get; set; }

        /// <summary>
        /// 脚本运行的Dll目录，Web程序是在Bin目录下
        /// </summary>
        public string RuntimePrivateBinPath { get; set; }
        /// <summary>
        /// 脚本在当前运行目录的相对位置
        /// </summary>
        public string ScriptRelativePath { get; set; }
        /// <summary>
        /// 脚本入口程序
        /// </summary>
        public string ScriptMainProgram { get; set; }
        /// <summary>
        /// 脚本入口程序类型，C#脚本时配置
        /// </summary>
        public string ScriptMainTypeName { get; set; }

        /// <summary>
        /// C# Model实体脚本路径
        /// </summary>
        public string ModelScriptPath { get; set; }

        /// <summary>
        /// C#脚本路径
        /// </summary>
        public string CSharpScriptPath { get; set; }

        /// <summary>
        /// 脚本是否可调试
        /// </summary>
        public bool ScriptIsDebug { get; set; }
        /// <summary>
        /// Py脚本是否启用
        /// </summary>
        public bool DisablePython { get; set; }
        /// <summary>
        /// Py是否可调试
        /// </summary>
        public bool PythonIsDebug { get; set; }

        /// <summary>
        /// Py脚本目录
        /// </summary>
        public string PythonScriptPath { get; set; }
        /// <summary>
        /// Py脚本包含的Import头信息
        /// </summary>
        public string PythonReferenceLibFile { get; set; }
        /// <summary>
        /// Lua脚本是否开启
        /// </summary>
        public bool DisableLua { get; set; }
        /// <summary>
        /// Lua脚本目录
        /// </summary>
        public string LuaScriptPath { get; set; }

        /// <summary>
        /// 脚本引用的程序集名
        /// </summary>
        public List<string> ReferencedAssemblyNames { get; set; }

        /// <summary>
        /// Register model script has changed before event.
        /// </summary>
        public Action<Assembly> ModelChangedBefore { get; set; }

        /// <summary>
        /// Register model script has changed after event.
        /// </summary>
        public Action<Assembly> ModelChangedAfter { get; set; }

        /// <summary>
        /// 脚本解码回调方法
        /// </summary>
        public event ScriptDecodeHandle DecodeCallback;

        internal virtual string OnDecodeCallback(string source, string ext)
        {
            ScriptDecodeHandle handler = DecodeCallback;
            if (handler != null) return handler(source, ext);
            return source;
        }
    }
}
