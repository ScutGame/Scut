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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// 脚本对象引擎
    /// </summary>
    public class ScriptEngines
    {
        private static string _runtimePath;
        private static string _runtimeBinPath;
        private static string _pythonRootPath;
        private static string _csharpRootPath;
        private static Timer _changeWatchingTimer;
        private static FileSystemWatcher _pyfileWatcher;
        private static FileSystemWatcher _csfileWatcher;
        private static DictionaryExtend<string, ScriptFileInfo> _scriptCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
        private static HashSet<string> changedFiles = new HashSet<string>();
        private static HashSet<string> referencedAssemblies = new HashSet<string>();
        private static ScriptEngine _scriptEngine;
        private static Assembly _csharpAssembly;
        private static Dictionary<string, Object> _pythonOptions = new Dictionary<string, object>();
        private static bool _disablePython;

        static ScriptEngines()
        {
            _runtimePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            _runtimeBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (string.IsNullOrEmpty(_runtimeBinPath))
            {
                _runtimeBinPath = _runtimePath;
            }
            _disablePython = ConfigUtils.GetSetting("Python_Disable", false);
            _pythonRootPath = Path.Combine(_runtimePath, ConfigUtils.GetSetting("PythonRootPath", "PyScript"));
            _csharpRootPath = Path.Combine(_runtimePath, ConfigUtils.GetSetting("CSharpRootPath", "Script"));
            SetPythonDebug = ConfigUtils.GetSetting("Python_IsDebug", false);
        }

        /// <summary>
        /// 设置Python脚本是否可调试
        /// </summary>
        public static bool SetPythonDebug
        {
            set { SetPythonOptions("Debug", value); }
        }

        /// <summary>
        /// 设置Python运行时参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public static void SetPythonOptions(string param, object value)
        {
            _pythonOptions[param] = value;
        }

        /// <summary>
        /// 添加CSharp脚本动态引用DLL
        /// </summary>
        /// <param name="assemblys"></param>
        public static void AddReferencedAssembly(params string[] assemblys)
        {
            foreach (var assembly in assemblys)
            {
                if (!string.IsNullOrEmpty(assembly) && !referencedAssemblies.Contains(assembly))
                {
                    referencedAssemblies.Add(assembly);
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            _changeWatchingTimer = new Timer(TickCallback, null, Timeout.Infinite, Timeout.Infinite);

            if (!_disablePython && Directory.Exists(_pythonRootPath))
            {
                _pyfileWatcher = new FileSystemWatcher(_pythonRootPath, "*.py");
                _pyfileWatcher.Changed += new FileSystemEventHandler(watcher_Changed);
                _pyfileWatcher.Created += new FileSystemEventHandler(watcher_Changed);
                _pyfileWatcher.Deleted += new FileSystemEventHandler(watcher_Changed);
                _pyfileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                _pyfileWatcher.IncludeSubdirectories = true;
                _pyfileWatcher.EnableRaisingEvents = true;
            }

            if (Directory.Exists(_csharpRootPath))
            {
                _csfileWatcher = new FileSystemWatcher(_csharpRootPath, "*.cs");
                _csfileWatcher.Changed += new FileSystemEventHandler(watcher_Changed);
                _csfileWatcher.Created += new FileSystemEventHandler(watcher_Changed);
                _csfileWatcher.Deleted += new FileSystemEventHandler(watcher_Changed);
                _csfileWatcher.NotifyFilter = NotifyFilters.LastWrite;
                _csfileWatcher.IncludeSubdirectories = true;
                _csfileWatcher.EnableRaisingEvents = true;
            }
            InitScriptRuntime();
        }

        private static void TickCallback(object state)
        {
            HashSet<string> tmp = new HashSet<string>();
            var localChangedFiles = Interlocked.Exchange<HashSet<string>>(ref changedFiles, tmp);
            foreach (var fileName in localChangedFiles)
            {
                LoadScript(fileName);
            }
            lock (_scriptCodeCache)
            {
                _csharpAssembly = null;
            }
            GenerateCsharpScriptAssembly();
        }

        private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            changedFiles.Add(e.FullPath);
            _changeWatchingTimer.Change(500, Timeout.Infinite);
        }

        private static void InitScriptRuntime()
        {
            try
            {
                if (!_disablePython)
                {
                    InitPythonRuntime();
                }
                InitCSharpRuntime();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("InitScriptRuntime error:{0}", ex);
            }
        }

        private static void InitCSharpRuntime()
        {
            if (Directory.Exists(_csharpRootPath))
            {
                var files = Directory.GetFiles(_csharpRootPath, "*.cs", SearchOption.AllDirectories);
                foreach (var fileName in files)
                {
                    LoadScript(fileName);
                }

                GenerateCsharpScriptAssembly();
            }
        }

        private static void InitPythonRuntime()
        {
            _scriptEngine = Python.CreateEngine(_pythonOptions);
            _scriptEngine.Runtime.LoadAssembly(typeof(string).Assembly);
            _scriptEngine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());

            List<string> paths = new List<string>();
            paths.Add("*");
            if (Directory.Exists(_pythonRootPath))
            {
                paths.Add(_pythonRootPath);
                var dirList = Directory.GetDirectories(_pythonRootPath, "*", SearchOption.AllDirectories);
                foreach (var dir in dirList)
                {
                    paths.Add(dir);
                }
            }
            string path = Environment.GetEnvironmentVariable("IRONPYTHONPATH");
            if (string.IsNullOrEmpty(path))
            {
                TraceLog.WriteWarn("The ENV:\"IRONPYTHONPATH\" is not be setting.");
            }
            if (!string.IsNullOrEmpty(path))
            {
                string[] items = path.Split(';');
                foreach (string p in items)
                {
                    if (p.Length > 0)
                    {
                        paths.Add(p);
                    }
                }
            }
            TraceLog.ReleaseWrite("The py path:{0}", string.Join(@";", paths));
            _scriptEngine.SetSearchPaths(paths.ToArray());
            if (Directory.Exists(_pythonRootPath))
            {
                var files = Directory.GetFiles(_pythonRootPath, "*.py", SearchOption.AllDirectories);
                foreach (var fileName in files)
                {
                    LoadScript(fileName);
                }
            }
        }

        /// <summary>
        /// 加载脚本对象
        /// </summary>
        /// <param name="fileName"></param>
        public static ScriptFileInfo LoadScript(string fileName)
        {
            ScriptFileInfo scriptFileInfo = null;
            string scriptCode = GetScriptCode(fileName);
            if (_scriptCodeCache.ContainsKey(scriptCode))
            {
                var old = _scriptCodeCache[scriptCode];
                if (!File.Exists(fileName) ||
                    old.HashCode == GetFileHashCode(fileName))
                {
                    return old;
                }
            }
            scriptFileInfo = CreateScriptFile(fileName);
            if (scriptFileInfo != null)
            {
                _scriptCodeCache[scriptCode] = scriptFileInfo;
            }
            return scriptFileInfo;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="scriptCode">脚本标识</param>
        /// <param name="typeName">csharp脚本指定对象类型</param>
        /// <param name="args">csharp脚本指定类型构造函数的参数</param>
        /// <returns>csharp脚本返回指定typeName实例对象；python脚本返回ScriptCode对象</returns>
        public static dynamic Execute(string scriptCode, string typeName, params object[] args)
        {
            scriptCode = GetScriptCode(scriptCode);
            ScriptFileInfo scriptInfo = _scriptCodeCache[scriptCode];
            if (scriptInfo != null)
            {
                return Execute(scriptInfo, typeName, args);
            }
            return null;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="scriptInfo">ScriptFileInfo对象</param>
        /// <param name="typeName">csharp脚本指定对象类型</param>
        /// <param name="args">csharp脚本指定类型构造函数的参数</param>
        /// <returns>csharp脚本返回指定typeName实例对象；python脚本返回ScriptCode对象</returns>
        public static dynamic Execute(ScriptFileInfo scriptInfo, string typeName, params object[] args)
        {
            if (scriptInfo is PythonFileInfo)
            {
                if (_scriptEngine == null) return null;
                var scope = _scriptEngine.CreateScope();
                ((PythonFileInfo)scriptInfo).CompiledCode.Execute(scope);
                return scope;
            }
            if (scriptInfo is CSharpFileInfo)
            {
                if (_csharpAssembly != null)
                {
                    typeName = (typeName ?? "Game.Script." + Path.GetFileNameWithoutExtension(scriptInfo.FileName)).Split(',')[0];
                    var type = _csharpAssembly.GetType(typeName, false, true);
                    if (type != null)
                    {
                        return type.CreateInstance(args);
                    }
                }
                return null;
            }
            throw new NotSupportedException("Not supported script type:" + scriptInfo.GetType().FullName);
        }

        /// <summary>
        /// 生成CSharp脚本程序集
        /// </summary>
        public static void GenerateCsharpScriptAssembly()
        {
            lock (_scriptCodeCache)
            {
                if (_csharpAssembly == null)
                {
                    var list = _scriptCodeCache.Where(pair => pair.Value.Type == ScriptType.Csharp).ToList();
                    var fileNames = new string[list.Count];
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        fileNames[i] = list[i].Value.FileName;
                    }
                    if (fileNames.Length > 0)
                    {
                        _csharpAssembly = CompileCSharp(fileNames);
                    }
                }
            }
        }

        /// <summary>
        /// 创建脚本文件信息对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static ScriptFileInfo CreateScriptFile(string fileName)
        {
            ScriptFileInfo scriptFileInfo = null;
            if (!File.Exists(fileName))
            {
                return scriptFileInfo;
            }
            FileInfo fi = new FileInfo(fileName);
            string fileCode = GetScriptCode(fileName);
            string fileHash = GetFileHashCode(fileName);
            if (fi.Extension == ".py")
            {
                CompiledCode compiledCode = CompilePython(fileName);
                if (compiledCode != null)
                {
                    scriptFileInfo = new PythonFileInfo(fileCode, fileName, compiledCode);
                    scriptFileInfo.HashCode = fileHash;
                }
            }
            else if (fi.Extension == ".cs")
            {
                scriptFileInfo = new CSharpFileInfo(fileCode, fileName);
                scriptFileInfo.HashCode = fileHash;
            }
            else
            {
                TraceLog.WriteError("Not supported \"{0}\" file type.", fileName);
            }
            return scriptFileInfo;
        }

        private static string GetScriptCode(string fileName)
        {
            return (Path.GetFileName(fileName) ?? "").ToLower();
        }

        private static string GetFileHashCode(string fileName)
        {
            return CryptoHelper.ToFileMd5Hash(fileName);
        }

        private static Assembly CompileCSharp(string[] fileNames)
        {
            try
            {

                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                //compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "Microsoft.Dynamic.dll"));
                //compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "Microsoft.Scripting.dll"));
                compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "NLog.dll"));
                compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "Newtonsoft.Json.dll"));
                compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "protobuf-net.dll"));
                compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "ServiceStack.Redis.dll"));
                compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "ZyGames.Framework.Common.dll"));
                compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, "ZyGames.Framework.dll"));

                foreach (var assembly in referencedAssemblies)
                {
                    if (!compilerParameters.ReferencedAssemblies.Contains(assembly))
                    {
                        compilerParameters.ReferencedAssemblies.Add(Path.Combine(_runtimeBinPath, assembly));
                    }
                }
                compilerParameters.GenerateExecutable = false;
                compilerParameters.GenerateInMemory = true;
                compilerParameters.IncludeDebugInformation = true;
                CompilerResults cr = provider.CompileAssemblyFromFile(compilerParameters, fileNames);
                if (cr.Errors.HasErrors)
                {
                    string errStr = "Script Compile error：\r\n";
                    foreach (CompilerError err in cr.Errors)
                    {
                        errStr += "File:" + err.FileName + "\r\nLine:" + err.Line + "\r\nMessage:" + err.ErrorText;
                    }

                    TraceLog.WriteError(errStr);
                    return null;
                }
                return cr.CompiledAssembly;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CompileCSharp script error:{2}", ex);
                return null;
            }
        }

        private static CompiledCode CompilePython(string fileName)
        {
            try
            {
                if (_scriptEngine == null) return null;
                return _scriptEngine.CreateScriptSourceFromFile(fileName).Compile();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CompilePython script:{0} error:{1}", fileName, ex);
                return null;
            }
        }

    }
}