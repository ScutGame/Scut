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
using System.Reflection;
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
        private static ScriptEngine _scriptEngine;
        private static Timer _changeWatchingTimer;
        private static bool _disablePython;
        private static HashSet<string> _referencedAssemblies;
        private static DictionaryExtend<string, FileWatcherInfo> _watcherDict;
        private static DictionaryExtend<string, ScriptFileInfo> _scriptCodeCache;
        private static Dictionary<string, Object> _pythonOptions;
        private static HashSet<string> _changedFiles;
        private static string _relativeDirName;
        private const string ModelDirName = "Model";
        private static string _runtimeBinPath;
        private static bool _scriptIsDebug;


        static ScriptEngines()
        {
            //init object.
            _pythonOptions = new Dictionary<string, object>();
            _watcherDict = new DictionaryExtend<string, FileWatcherInfo>();
            _scriptCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
            _changedFiles = new HashSet<string>();

            //init runtime path.
            var runtimePath = MathUtils.RuntimePath;
            _runtimeBinPath = MathUtils.RuntimeBinPath;
            if (string.IsNullOrEmpty(_runtimeBinPath))
            {
                _runtimeBinPath = runtimePath;
            }
            _relativeDirName = ConfigUtils.GetSetting("ScriptRelativePath", "");
            _disablePython = ConfigUtils.GetSetting("Python_Disable", false);
            _scriptIsDebug = ConfigUtils.GetSetting("Script_IsDebug", false);
            SetPythonDebug = ConfigUtils.GetSetting("Python_IsDebug", _scriptIsDebug);

            //init script dir.
            var directorys = new[] 
            { 
                string.Format("{0};{1}", ModelDirName, false),
                string.Format("{0};{1};{2}", ConfigUtils.GetSetting("CSharpRootPath", "Script"), true, ModelDirName)
            };
            foreach (string temp in directorys)
            {
                var arr = temp.Split(';');
                string dirName = arr[0];
                bool isMemory = arr.Length > 1 ? arr[1].ToBool() : false;
                string[] refArr = arr.Length > 2 ? arr[2].Split(',') : new string[0];
                _watcherDict[dirName] = new FileWatcherInfo()
                {
                    Path = Path.Combine(runtimePath, _relativeDirName, dirName),
                    Filter = "*.cs",
                    CompileLevel = dirName == ModelDirName ? 9 : 0,
                    IsInMemory = isMemory,
                    ReferenceKeys = refArr
                };
            }
            if (!_disablePython)
            {
                string pythonDir = ConfigUtils.GetSetting("PythonRootPath", "PyScript");
                _watcherDict[pythonDir] = new FileWatcherInfo()
                {
                    IsPython = true,
                    Path = Path.Combine(runtimePath, _relativeDirName, pythonDir),
                    Filter = "*.py"
                };
            }

            //init Assemblies
            _referencedAssemblies = new HashSet<string>(new[]
            {
                Path.Combine(_runtimeBinPath, "NLog.dll"),
                Path.Combine(_runtimeBinPath, "Newtonsoft.Json.dll"), 
                Path.Combine(_runtimeBinPath, "protobuf-net.dll"),
                Path.Combine(_runtimeBinPath, "ServiceStack.Redis.dll"),
                Path.Combine(_runtimeBinPath, "ZyGames.Framework.Common.dll"),
                Path.Combine(_runtimeBinPath, "ZyGames.Framework.dll")
            });
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
        /// Register model script has changed before event.
        /// </summary>
        /// <param name="callback"></param>
        public static void RegisterModelChangedBefore(Action<Assembly> callback)
        {
            _watcherDict[ModelDirName].ChangedBefore += callback;
        }

        /// <summary>
        /// Register model script has changed after event.
        /// </summary>
        /// <param name="callback"></param>
        public static void RegisterModelChangedAfter(Action<Assembly> callback)
        {
            _watcherDict[ModelDirName].ChangedAfter += callback;
        }

        /// <summary>
        /// 添加CSharp脚本动态引用DLL
        /// </summary>
        /// <param name="assemblys"></param>
        public static void AddReferencedAssembly(params string[] assemblys)
        {
            foreach (var ass in assemblys)
            {
                var assembly = ass.Split('/', '\\').Length == 1 ? Path.Combine(_runtimeBinPath, ass) : ass;
                if (!string.IsNullOrEmpty(assembly) && !_referencedAssemblies.Contains(assembly))
                {
                    _referencedAssemblies.Add(assembly);
                }
            }
        }

        /// <summary>
        /// Run main class.
        /// </summary>
        public static bool RunMainClass(string mainClass, out dynamic instance, params string[] refAssemblies)
        {
            instance = null;
            string runtimePath = MathUtils.RuntimePath;
            string mainFile = Path.Combine(runtimePath, _relativeDirName, mainClass + ".cs");
            if (!File.Exists(mainFile))
            {
                throw new Exception("Not found file \"" + mainFile + "\".");
            }
            ScriptCompiler.ClearTemp();
            var tempAssemblies = new List<string>(_referencedAssemblies);
            if(refAssemblies.Length > 0)
            {
                tempAssemblies.AddRange(refAssemblies);
            }
            var compileResult = ScriptCompiler.Compile(new string[] { mainFile }, tempAssemblies.ToArray(), mainClass, _scriptIsDebug, true);
            if (compileResult != null)
            {
                var assembly = compileResult.CompiledAssembly;
                var mainType = assembly.GetTypes().Where(t => t.Name == mainClass).FirstOrDefault();
                instance = mainType.CreateInstance();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            AppDomain.CurrentDomain.AppendPrivatePath(Path.Combine(".", ScriptCompiler.ScriptAssemblyTemp));
            _changeWatchingTimer = new Timer(TickCallback, null, Timeout.Infinite, Timeout.Infinite);
            foreach (var pair in _watcherDict)
            {
                var item = pair.Value;
                if (!Directory.Exists(item.Path))
                {
                    Directory.CreateDirectory(item.Path);
                }
                item.Watcher = new FileSystemWatcher(item.Path, item.Filter);
                item.Watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                item.Watcher.Created += new FileSystemEventHandler(watcher_Changed);
                item.Watcher.Deleted += new FileSystemEventHandler(watcher_Changed);
                item.Watcher.NotifyFilter = NotifyFilters.LastWrite;
                item.Watcher.IncludeSubdirectories = true;
                item.Watcher.EnableRaisingEvents = true;
            }

            InitScriptRuntime();
        }

        private static void TickCallback(object state)
        {
            HashSet<string> tmp = new HashSet<string>();
            var localChangedFiles = Interlocked.Exchange<HashSet<string>>(ref _changedFiles, tmp);
            var changeDirs = new Dictionary<string, FileWatcherInfo>();
            foreach (var fileName in localChangedFiles)
            {
                var script = LoadScript(fileName);
                if (script != null &&
                    script is CSharpFileInfo &&
                    !string.IsNullOrEmpty(script.GroupName) &&
                    _watcherDict.ContainsKey(script.GroupName))
                {
                    changeDirs[script.GroupName] = _watcherDict[script.GroupName];
                }
            }

            var paris = changeDirs.OrderByDescending(t => t.Value.CompileLevel).ToList();
            foreach (var changeDir in paris)
            {
                string name = changeDir.Key;
                var filenames = _scriptCodeCache.Where(pair => pair.Value.GroupName == name).Select(t => t.Value.FileName).ToArray();
                UpdateAssembly(name, filenames, changeDir.Value);
            }
        }

        private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            _changedFiles.Add(e.FullPath);
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
            var csharpPairs = _watcherDict.Where(pair => !pair.Value.IsPython)
                .OrderByDescending(t => t.Value.CompileLevel).ToList();
            foreach (var pair in csharpPairs)
            {
                string name = pair.Key;
                if (Directory.Exists(pair.Value.Path))
                {
                    var files = Directory.GetFiles(pair.Value.Path, pair.Value.Filter, SearchOption.AllDirectories);
                    foreach (var fileName in files)
                    {
                        LoadScript(fileName);
                    }
                    UpdateAssembly(name, files, pair.Value);
                }
            }
        }

        private static void UpdateAssembly(string name, string[] filenames, FileWatcherInfo watcherInfo)
        {
            string assemblyName = string.Format("DynamicScripts.{0}", name);
            var assembly = GenerateCsharpScriptAssembly(name, filenames, assemblyName, watcherInfo);
            var obj = _watcherDict[name];
            if (obj != null && assembly != null)
            {
                obj.Assembly = assembly;
                var pairs = _watcherDict.Where(pair => !pair.Value.IsPython && pair.Value.ReferenceKeys.Contains(name)).ToList();
                foreach (var pair in pairs)
                {
                    string refName = pair.Key;
                    var files = _scriptCodeCache.Where(p => p.Value.GroupName == refName).Select(t => t.Value.FileName).ToArray();
                    UpdateAssembly(refName, files, pair.Value);
                }
            }
        }

        private static void InitPythonRuntime()
        {
            _scriptEngine = Python.CreateEngine(_pythonOptions);
            _scriptEngine.Runtime.LoadAssembly(typeof(string).Assembly);
            _scriptEngine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());

            List<string> paths = new List<string>();
            paths.Add("*");
            var pythonPairs = _watcherDict.Where(pair => pair.Value.IsPython).ToList();
            foreach (var pair in pythonPairs)
            {
                string pythonPath = pair.Value.Path;
                if (Directory.Exists(pythonPath))
                {
                    paths.Add(pythonPath);
                    var dirList = Directory.GetDirectories(pythonPath, "*", SearchOption.AllDirectories);
                    paths.AddRange(dirList);

                    var files = Directory.GetFiles(pythonPath, pair.Value.Filter, SearchOption.AllDirectories);
                    foreach (var fileName in files)
                    {
                        LoadScript(fileName);
                    }
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
        /// Process csharp script.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static dynamic ExecuteCSharp(string typeName, params object[] args)
        {
            var arr = typeName.Split(',')[0].Split('.');
            string scriptCode = arr[arr.Length - 1] + ".cs";
            return Execute(scriptCode, typeName, args);
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
                var item = _watcherDict[scriptInfo.GroupName];
                if (item != null && item.Assembly != null)
                {
                    typeName = (typeName ?? "Game.Script." + Path.GetFileNameWithoutExtension(scriptInfo.FileName)).Split(',')[0];
                    var type = item.Assembly.GetType(typeName, false, true);
                    if (type != null)
                    {
                        return type.CreateInstance(args);
                    }
                }
                return null;
                //if (_dynamicAssembly != null)
                //{
                //    typeName = (typeName ?? "Game.Script." + Path.GetFileNameWithoutExtension(scriptInfo.FileName)).Split(',')[0];
                //    var type = _dynamicAssembly.GetType(typeName, false, true);
                //    if (type != null)
                //    {
                //        return type.CreateInstance(args);
                //    }
                //}
                //return null;
            }
            throw new NotSupportedException("Not supported script type:" + scriptInfo.GetType().FullName);
        }

        /// <summary>
        /// 生成CSharp脚本程序集
        /// </summary>
        private static Assembly GenerateCsharpScriptAssembly(string name, string[] fileNames, string assemblyName, FileWatcherInfo watcherInfo)
        {
            if (fileNames.Length > 0)
            {
                var refAssemblies = _referencedAssemblies.ToList();
                foreach (var refKey in watcherInfo.ReferenceKeys)
                {
                    string assmPath = _watcherDict[refKey].AssemblyOutPath;
                    if (!string.IsNullOrEmpty(assmPath))
                    {
                        refAssemblies.Add(assmPath);
                    }
                }
                bool inMemory = watcherInfo.IsInMemory;
                if (name == ModelDirName)
                {
                    string pathToAssembly;
                    var assm = ScriptCompiler.InjectionCompile(fileNames, refAssemblies.ToArray(), assemblyName, _scriptIsDebug, inMemory, out pathToAssembly);
                    watcherInfo.AssemblyOutPath = pathToAssembly;
                    return assm;
                }
                var result = ScriptCompiler.Compile(fileNames, refAssemblies.ToArray(), assemblyName, _scriptIsDebug, inMemory);
                if (result != null)
                {
                    watcherInfo.AssemblyOutPath = result.PathToAssembly;
                    return result.CompiledAssembly;
                }
            }
            return null;
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

            var watchDir = _watcherDict.Where(pair => fileName.StartsWith(pair.Value.Path)).FirstOrDefault();

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
                    scriptFileInfo.GroupName = watchDir.Key;
                }
            }
            else if (fi.Extension == ".cs")
            {
                scriptFileInfo = new CSharpFileInfo(fileCode, fileName);
                scriptFileInfo.HashCode = fileHash;
                scriptFileInfo.GroupName = watchDir.Key;
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