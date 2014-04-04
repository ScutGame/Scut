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
using System.Text;
using System.Threading;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;

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
        public static readonly string CSharpDirName;
        public static readonly string PythonDirName;
        private static string _runtimeBinPath;
        private static bool _scriptIsDebug;
        private static int ScriptChangedDelay = 1000;
        private static string ReferenceLibFile;
        private static string ScriptMainClass;
        private static string ScriptMainTypeName;
        private static string _runtimePath;

        /// <summary>
        /// Disable python script.
        /// </summary>
        public static bool DisablePython
        {
            get { return _disablePython; }
        }

        /// <summary>
        /// Is error
        /// </summary>
        public static bool IsError { get; private set; }

        static ScriptEngines()
        {
            //init object.
            _pythonOptions = new Dictionary<string, object>();
            _watcherDict = new DictionaryExtend<string, FileWatcherInfo>();
            _scriptCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
            _changedFiles = new HashSet<string>();

            //init runtime path.
            _runtimePath = MathUtils.RuntimePath;
            _runtimeBinPath = MathUtils.RuntimeBinPath;
            if (string.IsNullOrEmpty(_runtimeBinPath))
            {
                _runtimeBinPath = _runtimePath;
            }
            _relativeDirName = ConfigUtils.GetSetting("ScriptRelativePath", "");
            _disablePython = ConfigUtils.GetSetting("Python_Disable", false);
            _scriptIsDebug = ConfigUtils.GetSetting("Script_IsDebug", false);
            SetPythonDebug = ConfigUtils.GetSetting("Python_IsDebug", _scriptIsDebug);
            CSharpDirName = ConfigUtils.GetSetting("CSharpRootPath", "Script");
            ScriptMainClass = ConfigUtils.GetSetting("ScriptMainClass", Path.Combine(CSharpDirName, "MainClass.cs"));
            ScriptMainTypeName = ConfigUtils.GetSetting("ScriptMainTypeName", "");

            //init script dir.
            var directorys = new[] 
            { 
                string.Format("{0};{1}", ModelDirName, false),
                string.Format("{0};{1};{2}", CSharpDirName, false, ModelDirName)
            };
            foreach (string temp in directorys)
            {
                var arr = temp.Split(';');
                string dirName = arr[0];
                bool isMemory = arr.Length > 1 ? arr[1].ToBool() : false;
                string[] refArr = arr.Length > 2 ? arr[2].Split(',') : new string[0];
                _watcherDict[dirName] = new FileWatcherInfo()
                {
                    Path = Path.Combine(_runtimePath, _relativeDirName, dirName),
                    Filter = "*.cs",
                    CompileLevel = dirName == ModelDirName ? 9 : 3,
                    IsInMemory = isMemory,
                    ReferenceKeys = refArr
                };
            }
            if (!_disablePython)
            {
                PythonDirName = ConfigUtils.GetSetting("PythonRootPath", "PyScript");
                ReferenceLibFile = Path.Combine(_runtimePath, _relativeDirName, PythonDirName, ConfigUtils.GetSetting("ReferenceLibFile", "Lib/ReferenceLib.py"));
                string path = Path.GetDirectoryName(ReferenceLibFile);
                if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                _watcherDict[PythonDirName] = new FileWatcherInfo()
                {
                    IsPython = true,
                    CompileLevel = 0,
                    Path = Path.Combine(_runtimePath, _relativeDirName, PythonDirName),
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
        /// Add system reference
        /// </summary>
        /// <param name="assemblys"></param>
        public static void AddSysReferencedAssembly(params string[] assemblys)
        {
            foreach (var assembly in assemblys)
            {
                if (!string.IsNullOrEmpty(assembly) && !_referencedAssemblies.Contains(assembly))
                {
                    _referencedAssemblies.Add(assembly);
                }
            }
        }
        /// <summary>
        /// 添加CSharp脚本动态引用DLL
        /// </summary>
        /// <param name="assemblys"></param>
        public static void AddReferencedAssembly(params string[] assemblys)
        {
            foreach (var ass in assemblys)
            {
                if (string.IsNullOrEmpty(ass)) continue;
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
        public static bool RunMainClass(out dynamic instance, params string[] args)
        {
            string scriptCode = GetScriptCode(ScriptMainClass);
            instance = Execute(scriptCode, ScriptMainTypeName);
            if (instance != null)
            {
                instance.Start(args);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get model entity assembly.
        /// </summary>
        /// <returns></returns>
        public static Assembly GetEntityAssembly()
        {
            return _watcherDict.ContainsKey(ModelDirName)
                ? _watcherDict[ModelDirName].Assembly
                : null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            try
            {
                ScriptCompiler.ClearTemp();
                AppDomain.CurrentDomain.AppendPrivatePath(ScriptCompiler.ScriptPath);
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
            catch (Exception er)
            {
                IsError = true;
                throw er;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public static void Dispose()
        {
            _changeWatchingTimer.Dispose();
            foreach (var pair in _watcherDict)
            {
                if (pair.Value.Watcher != null)
                {
                    pair.Value.Watcher.Dispose();
                }
            }
        }

        private static void TickCallback(object state)
        {
            try
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
                    var srciptList = _scriptCodeCache.Where(pair => pair.Value.GroupName == name);
                    var filenames = srciptList.Select(t => t.Value.FileName).ToArray();
                    UpdateAssembly(name, filenames, changeDir.Value);
                    foreach (var pair in srciptList)
                    {
                        pair.Value.ObjType = null;
                    }
                }
                if (paris.Count > 0)
                {
                    InitPythonRuntime(true);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("TickCallback error:{0}", ex);
            }
        }

        private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!string.Equals(e.FullPath, ReferenceLibFile, StringComparison.CurrentCultureIgnoreCase) &&
                    CheckFileChanged(e.FullPath))
                {
                    _changedFiles.Add(e.FullPath);
                    _changeWatchingTimer.Change(ScriptChangedDelay, Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("watcher_Changed error:{0}", ex);
            }
        }

        private static void InitScriptRuntime()
        {
            try
            {
                InitCSharpRuntime();
                InitPythonRuntime();
            }
            catch (Exception ex)
            {
                IsError = true;
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
                    TraceLog.ReleaseWrite("CSharp {0} script successfully compiled.", name);
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

        /// <summary>
        /// update py reference head file.
        /// </summary>
        private static void UpdatePythonReferenceLib()
        {
            if (DisablePython) return;

            StringBuilder pyCode = new StringBuilder();
            pyCode.AppendLine(@"import clr, sys");
            pyCode.AppendLine(@"clr.AddReference('ZyGames.Framework.Common')");
            pyCode.AppendLine(@"clr.AddReference('ZyGames.Framework')");
            pyCode.AppendLine(@"clr.AddReference('ZyGames.Framework.Game')");
            var assmeblyNames = _watcherDict.Where(p => !p.Value.IsPython && p.Value.Assembly != null)
                .Select(p => p.Value.Assembly.GetName().Name);
            foreach (var assmeblyName in assmeblyNames)
            {
                pyCode.AppendFormat(@"clr.AddReference('{0}')", assmeblyName);
                pyCode.AppendLine();
            }

            string scriptCode = GetScriptCode(ReferenceLibFile);
            ScriptFileInfo scriptInfo = _scriptCodeCache[scriptCode];
            if (scriptInfo == null || scriptInfo.TryEnterLock())
            {
                try
                {
                    using (var sw = File.CreateText(ReferenceLibFile))
                    {
                        sw.Write(pyCode.ToString());
                        sw.Flush();
                        sw.Close();
                    }
                }
                finally
                {
                    if (scriptInfo != null)
                    {
                        scriptInfo.ExitLock();
                    }
                }
            }

        }

        private static void InitPythonRuntime(bool isUpdate = false)
        {
            if (DisablePython)
            {
                return;
            }
            _scriptEngine = Python.CreateEngine(_pythonOptions);
            _scriptEngine.Runtime.LoadAssembly(typeof(string).Assembly);
            _scriptEngine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());

            var pythonPairs = _watcherDict.Where(pair => pair.Value.IsPython).ToList();
            List<string> searchPaths = new List<string>();
            searchPaths.Add("*");
            //load python path
            string path = Environment.GetEnvironmentVariable("IRONPYTHONPATH");
            if (string.IsNullOrEmpty(path))
            {
                TraceLog.WriteError("The ENV:\"IRONPYTHONPATH\" is not be setting.");
            }
            if (!string.IsNullOrEmpty(path))
            {
                string[] items = path.Split(';');
                searchPaths.AddRange(items.Where(p => p.Length > 0));
            }
            foreach (var pair in pythonPairs)
            {
                string pythonPath = pair.Value.Path;
                if (Directory.Exists(pythonPath))
                {
                    searchPaths.Add(pythonPath);
                    var dirList = Directory.GetDirectories(pythonPath, "*", SearchOption.AllDirectories);
                    searchPaths.AddRange(dirList);
                }
            }
            TraceLog.ReleaseWrite("The py path:{0}", string.Join(@";", searchPaths));
            _scriptEngine.SetSearchPaths(searchPaths.ToArray());

            UpdatePythonReferenceLib();
            //load py script
            foreach (var pair in pythonPairs)
            {
                string pythonPath = pair.Value.Path;
                if (Directory.Exists(pythonPath))
                {
                    var files = Directory.GetFiles(pythonPath, pair.Value.Filter, SearchOption.AllDirectories);
                    foreach (var fileName in files)
                    {
                        LoadScript(fileName, isUpdate);
                    }
                }
            }
        }

        private static bool CheckFileChanged(string fileName)
        {
			//todo modify
            return true;
            string scriptCode = GetScriptCode(fileName);
            var script = _scriptCodeCache[scriptCode];
            if (script != null && script.TryEnterLock())
            {
                try
                {
                    string hashcode = GetFileHashCode(fileName);
                    return !string.IsNullOrEmpty(hashcode) && script.HashCode != hashcode;
                }
                finally
                {
                    script.ExitLock();
                }
            }
            return false;
        }
        /// <summary>
        /// 加载脚本对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isUpdate"></param>
        public static ScriptFileInfo LoadScript(string fileName, bool isUpdate = false)
        {
            ScriptFileInfo scriptFileInfo = null;
            string scriptCode = GetScriptCode(fileName);
            if (!isUpdate && _scriptCodeCache.ContainsKey(scriptCode))
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
            string scriptCode = "";
            typeName = typeName ?? "";
            int index = typeName.IndexOf(CSharpDirName + ".", StringComparison.CurrentCultureIgnoreCase);
            if (index > -1)
            {
                scriptCode = typeName.Substring(index) + ".cs";
            }
            else
            {
                var arr = typeName.Split(',')[0].Split('.');
                scriptCode = arr[arr.Length - 1] + ".cs";
            }
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
                if (scriptInfo.ObjType == null)
                {
                    var item = _watcherDict[scriptInfo.GroupName];
                    if (item != null && item.Assembly != null && !item.IsUpdating())
                    {
                        if (string.IsNullOrEmpty(typeName))
                        {
                            typeName = ("Game.Script." + Path.GetFileNameWithoutExtension(scriptInfo.FileName)).Split(',')[0];
                        }
                        scriptInfo.ObjType = item.Assembly.GetType(typeName, false, true);
                    }
                }
                return scriptInfo.ObjType != null ? scriptInfo.ObjType.CreateInstance(args) : null;
            }
            throw new NotSupportedException("Not supported script type:" + scriptInfo.GetType().FullName);
        }
        /// <summary>
        /// ExecutePython
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public static dynamic ExecutePython(string scriptCode)
        {
            if (_scriptEngine == null) return null;
            var scope = _scriptEngine.CreateScope();
            var compileCode = CompilePythonCode(scriptCode);
            if (scriptCode != null)
            {
                compileCode.Execute(scope);
                return scope;
            }
            return null;
        }
        /// <summary>
        /// ExecuteCSharp
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static dynamic ExecuteCSharpSource(string[] scriptCode, string[] refAssemblies, string typeName, params object[] args)
        {
            var result = ScriptCompiler.CompileSource(scriptCode, refAssemblies, "DynamicCode", _scriptIsDebug, true);
            if (result != null)
            {
                var type = result.CompiledAssembly.GetType(typeName, false, true);
                if (type != null) return type.CreateInstance(args);
            }
            return null;
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
                    //load parent class propertys.
                    if (assm != null)
                    {
                        ProtoBufUtils.LoadProtobufType(assm);
                        if (watcherInfo.Assembly == null)
                        {
                            //first
                            EntitySchemaSet.LoadAssembly(assm);
                        }
                    }
                    return assm;
                }
                var result = ScriptCompiler.Compile(fileNames, refAssemblies.ToArray(), assemblyName, _scriptIsDebug, inMemory, ScriptCompiler.ScriptAssemblyTemp);
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

        /// <summary>
        /// full filename,return relative to "Script" path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetScriptCode(string fileName)
        {
            string codeString = "";
            var arr = (fileName ?? "").Split('\\', '/', '.');

            var rootArr = Path.Combine(_runtimePath, _relativeDirName).Split('\\', '/', '.');
            bool issame = true;
            for (int i = 0; i < arr.Length; i++)
            {
                string str = arr[i];
                if (issame && rootArr.Length > i && string.Equals(str, rootArr[i], StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                issame = false;
                if (codeString.Length > 0) codeString += ".";
                codeString += str;
            }
            return (codeString ?? "default").ToLower();
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
                var scriptSource = _scriptEngine.CreateScriptSourceFromFile(fileName);
                return scriptSource.Compile();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CompilePython script:{0} error:{1}", fileName, ex);
                return null;
            }
        }

        private static CompiledCode CompilePythonCode(string scriptCode)
        {
            try
            {
                if (_scriptEngine == null) return null;
                var scriptSource = _scriptEngine.CreateScriptSourceFromString(scriptCode, SourceCodeKind.AutoDetect);
                return scriptSource.Compile();
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CompilePython script:{0} error:{1}", scriptCode, ex);
                return null;
            }
        }
    }
}