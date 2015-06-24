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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptSection : ConfigSection
    {
        /// <summary>
        /// 
        /// </summary>
        public ScriptSection()
        {
            ScriptChangedDelay = ConfigUtils.GetSetting("ScriptChangedDelay", 1000);
            RuntimePath = MathUtils.RuntimePath;
            RuntimePrivateBinPath = MathUtils.RuntimeBinPath;
            if (string.IsNullOrEmpty(RuntimePath))
            {
                RuntimePrivateBinPath = RuntimePath;
            }
            ScriptRelativePath = ConfigUtils.GetSetting("ScriptRelativePath");
            ScriptIsDebug = ConfigUtils.GetSetting("Script_IsDebug", false);

            ModelScriptPath = ConfigUtils.GetSetting("ModelRootPath", "Model");
            CSharpScriptPath = ConfigUtils.GetSetting("CSharpRootPath", "Script");//兼容旧版本在"Script"目录下，新版本使用"CsScript"
            ScriptMainProgram = ConfigUtils.GetSetting("ScriptMainProgram");//调整入口程序命名
            if (string.IsNullOrEmpty(ScriptMainProgram))
            {
                ScriptMainProgram = ConfigUtils.GetSetting("ScriptMainClass", "MainClass.cs");
            }
            ScriptMainTypeName = ConfigUtils.GetSetting("ScriptMainTypeName", "Game.Script.MainClass");

            //Py setting
            DisablePython = ConfigUtils.GetSetting("Python_Disable", true);
            PythonIsDebug = ConfigUtils.GetSetting("Python_IsDebug", ScriptIsDebug);
            PythonScriptPath = ConfigUtils.GetSetting("PythonRootPath", "PyScript");
            PythonReferenceLibFile = Path.Combine(RuntimePath,
                ScriptRelativePath,
                PythonScriptPath,
                ConfigUtils.GetSetting("ReferenceLibFile", "Lib/ReferenceLib.py"));

            //Lua setting
            DisableLua = ConfigUtils.GetSetting("Lua_Disable", true);
            LuaScriptPath = ConfigUtils.GetSetting("LuaRootPath", "LuaScript");
            

            SysAssemblyReferences = ConfigUtils.GetSetting("ScriptSysAsmReferences", "").Split(';');
            AssemblyReferences = ConfigUtils.GetSetting("ScriptAsmReferences", "").Split(';');
        }

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
        /// 
        /// </summary>
        public string[] AssemblyReferences { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] SysAssemblyReferences { get; set; }
    }
}
