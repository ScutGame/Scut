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
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// script loaded delegate
    /// </summary>
    public delegate void ScriptLoaded(string type, string[] files);

    /// <summary>
    /// 脚本对象引擎
    /// </summary>
    public class ScriptEngines
    {
        private static ScriptRuntimeDomain _runtimeDomain;
        private static ScriptSettupInfo _settupInfo;
        private static List<FileSystemWatcher> _watcherList;
        private static HashSet<string> _changedFiles;
        private static Timer _changeWatchingTimer;
        private static int _isCompiling = 0;
        /// <summary>
        /// Script loaded event
        /// </summary>
        public static event ScriptLoaded OnLoaded;

        private static void DoScriptLoaded(string type, string[] files)
        {
            ScriptLoaded handler = OnLoaded;
            if (handler != null) handler(type, files);
        }

        /// <summary>
        /// Is compiling
        /// </summary>
        public static bool IsCompiling { get { return _isCompiling == 1; } }
        /// <summary>
        /// Is error
        /// </summary>
        public static bool IsError { get; private set; }

        /// <summary>
        /// Settup info
        /// </summary>
        public static ScriptSettupInfo SettupInfo
        {
            get { return _settupInfo; }
        }

        static ScriptEngines()
        {
            _changedFiles = new HashSet<string>();
            _settupInfo = new ScriptSettupInfo();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            try
            {
                ScriptCompiler.ClearScriptRuntimeTemp();
                ConfigManager.ConfigReloaded += OnScriptSettingReLoad;
                var scope = InitScriptRuntimeScope();
                if (scope != null)
                {
                    InitScriptListener(scope.WatcherPaths);
                }
            }
            catch (Exception er)
            {
                IsError = true;
                TraceLog.WriteError("Script init error:{0}.", er);
                throw er;
            }
        }

        private static void OnScriptSettingReLoad(object sender, ConfigReloadedEventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {

            }
        }

        private static void InitScriptListener(IEnumerable<string> paths)
        {
            _changeWatchingTimer = new Timer(DoWatcherChanged, null, Timeout.Infinite, Timeout.Infinite);
            _watcherList = new List<FileSystemWatcher>();
            foreach (var pathInfo in paths)
            {
                string[] args = pathInfo.Split(';');
                string path = args[0];
                string filter = args.Length > 0 ? args[1] : "*.*";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var watcher = new FileSystemWatcher(path, filter);
                watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                watcher.Created += new FileSystemEventHandler(watcher_Changed);
                watcher.Deleted += new FileSystemEventHandler(watcher_Changed);
                watcher.Error += watcher_Error;
                watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                _watcherList.Add(watcher);
            }
        }

        private static void watcher_Error(object sender, ErrorEventArgs e)
        {
            TraceLog.WriteError("Script file has changed error:{0}", e.GetException().ToString());
        }

        private static ScriptRuntimeScope InitScriptRuntimeScope()
        {
            //star compile
            if (Interlocked.Exchange(ref _isCompiling, 1) == 0)
            {
                ScriptRuntimeDomain runtimeDomain = null;
                try
                {
                    string runtimePath = MathUtils.RuntimePath ?? MathUtils.RuntimeBinPath;
                    AppDomain.CurrentDomain.AppendPrivatePath(ScriptCompiler.ScriptPath);
                    runtimeDomain = new ScriptRuntimeDomain(typeof(ScriptRuntimeDomain).Name, new[] { _settupInfo.RuntimePrivateBinPath, ScriptCompiler.ScriptPath });
                    foreach (var assemblyName in _settupInfo.ReferencedAssemblyNames)
                    {
                        //排除System的dll
                        if (string.IsNullOrEmpty(assemblyName) ||
                            !Path.IsPathRooted(assemblyName)) continue;
                        string key = Path.GetFileNameWithoutExtension(assemblyName);
                        runtimeDomain.LoadAssembly(key, assemblyName);
                    }
                    var scope = runtimeDomain.CreateScope(_settupInfo);
                    //ignore error, allow model is empty.
                    if (scope == null)
                    {
                        if (_runtimeDomain == null) _runtimeDomain = runtimeDomain;
                        return scope;
                    }

                    //update befor
                    bool isFirstRun = _runtimeDomain == null;
                    if (!isFirstRun && _settupInfo.ModelChangedBefore != null)
                    {
                        if (_runtimeDomain.Scope.ModelAssembly != null) _settupInfo.ModelChangedBefore(_runtimeDomain.Scope.ModelAssembly);
                        TimeListener.Clear();
                        if (_runtimeDomain.MainInstance != null) _runtimeDomain.MainInstance.Stop();
                    }
                    runtimeDomain.MainInstance = runtimeDomain.Scope.Execute(_settupInfo.ScriptMainProgram, _settupInfo.ScriptMainTypeName) as IMainScript;
                    if (_runtimeDomain != null)
                    {
                        //unload pre-domain
                        _runtimeDomain.Dispose();
                    }
                    _runtimeDomain = runtimeDomain;
                    EntitySchemaSet.EntityAssembly = scope.ModelAssembly;
                    //update after
                    if (!isFirstRun && _settupInfo.ModelChangedAfter != null && scope.ModelAssembly != null)
                    {
                        _settupInfo.ModelChangedAfter(scope.ModelAssembly);
                    }
                    else if (scope.ModelAssembly != null)
                    {
                        ProtoBufUtils.LoadProtobufType(scope.ModelAssembly);
                        EntitySchemaSet.LoadAssembly(scope.ModelAssembly);
                    }
                    PrintCompiledMessage();
                    //replace runtime 
                    if (!isFirstRun && runtimeDomain.MainInstance != null)
                    {
                        runtimeDomain.MainInstance.ReStart();
                    }
                    return scope;
                }
                finally
                {
                    Interlocked.Exchange(ref _isCompiling, 0);
                }
            }
            else
            {
                TraceLog.WriteLine("{1} {0} has not compiled in other thread.", "model", DateTime.Now.ToString("HH:mm:ss"));
            }
            return null;
        }

        private static void PrintCompiledMessage(string message = "script", string assemblyName = "")
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                TraceLog.WriteLine("{1} {0} compiled successfully, the assemblyName:{2}.", message, DateTime.Now.ToString("HH:mm:ss"), assemblyName);
            }
            else
            {
                TraceLog.WriteLine("{1} {0} compiled successfully.", message, DateTime.Now.ToString("HH:mm:ss"));
            }
        }

        private static void DoWatcherChanged(object state)
        {
            try
            {
                if (IsCompiling)
                {
                    //延迟处理
                    _changeWatchingTimer.Change(_settupInfo.ScriptChangedDelay, Timeout.Infinite);
                    return;
                }
                HashSet<string> tmp = new HashSet<string>();
                var changedFiles = Interlocked.Exchange<HashSet<string>>(ref _changedFiles, tmp);
                _changeWatchingTimer.Change(Timeout.Infinite, Timeout.Infinite);

                HashSet<string> localChangedFiles = new HashSet<string>();
                foreach (var file in changedFiles)
                {
                    //check file changed for md5 encode.
                    if (_runtimeDomain.Scope.VerifyScriptHashCode(file))
                    {
                        continue;
                    }
                    localChangedFiles.Add(file);
                }
                TraceLog.WriteLine("Update script file count:{0}.", localChangedFiles.Count);
                if (localChangedFiles.Count == 0)
                {
                    return;
                }
                bool hasModelFile = false;
                //以文件类型分组
                var changeGroup = localChangedFiles.GroupBy(t =>
                {
                    if (!hasModelFile && _runtimeDomain.Scope.IsModelScript(t))
                    {
                        hasModelFile = true;
                    }
                    return Path.GetExtension(t);
                })
                .OrderBy(t => t.Key);

                bool isLoop = true;
                foreach (var group in changeGroup)
                {
                    if (!isLoop)
                    {
                        break;
                    }

                    string ext = group.Key.ToLower();
                    switch (ext)
                    {
                        case ".cs":
                            //star compile
                            if (hasModelFile)
                            {
                                TraceLog.WriteLine("{1} {0} compile start...", "model script", DateTime.Now.ToString("HH:mm:ss"));
                                var scope = InitScriptRuntimeScope();
                                PrintCompiledMessage("model script", scope != null && scope.ModelAssembly != null ? scope.ModelAssembly.FullName : "null");
                                isLoop = false;
                            }
                            else
                            {
                                if (Interlocked.Exchange(ref _isCompiling, 1) == 0)
                                {
                                    try
                                    {
                                        TraceLog.WriteLine("{1} {0} compile start...", "csharp script", DateTime.Now.ToString("HH:mm:ss"));
                                        _runtimeDomain.Scope.InitCsharp();
                                        PrintCompiledMessage("csharp script");
                                    }
                                    finally
                                    {
                                        Interlocked.Exchange(ref _isCompiling, 0);
                                    }
                                }
                                else
                                {
                                    TraceLog.WriteLine("{1} {0} has not compiled in other thread.", "csharp script", DateTime.Now.ToString("HH:mm:ss"));
                                }
                            }

                            break;
                        case ".py":
                            _runtimeDomain.Scope.InitPython(group.ToArray());
                            PrintCompiledMessage("python script");
                            break;
                        case ".lua":
                            _runtimeDomain.Scope.InitLua();
                            PrintCompiledMessage("lua script");
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Script type \"{0}\" not supported.", ext));
                    }
                    DoScriptLoaded(ext, group.ToArray());
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("DoWatcherChanged error:{0}", ex);
            }
        }

        private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (!IsCompiling)
                {
                    _changedFiles.Add(e.FullPath);
                    _changeWatchingTimer.Change(_settupInfo.ScriptChangedDelay, Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("watcher_Changed error:{0}", ex);
            }
        }



        /// <summary>
        /// Register model script has changed before event.
        /// </summary>
        /// <param name="callback"></param>
        public static void RegisterModelChangedBefore(Action<Assembly> callback)
        {
            _settupInfo.ModelChangedBefore += callback;
        }

        /// <summary>
        /// Register model script has changed after event.
        /// </summary>
        /// <param name="callback"></param>
        public static void RegisterModelChangedAfter(Action<Assembly> callback)
        {
            _settupInfo.ModelChangedAfter += callback;
        }

        /// <summary>
        /// Add system reference
        /// </summary>
        /// <param name="assemblys"></param>
        public static void AddSysReferencedAssembly(params string[] assemblys)
        {
            foreach (var assembly in assemblys)
            {
                if (!string.IsNullOrEmpty(assembly))
                {
                    _settupInfo.ReferencedAssemblyNames.Add(assembly);
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
                var assembly = ass.Split('/', '\\').Length == 1 ? Path.Combine(_settupInfo.RuntimePrivateBinPath, ass) : ass;
                if (!string.IsNullOrEmpty(assembly))
                {
                    _settupInfo.ReferencedAssemblyNames.Add(assembly);
                }
            }
        }

        /// <summary>
        /// Get model entity assembly.
        /// </summary>
        /// <returns></returns>
        public static Assembly GetEntityAssembly()
        {
            return _runtimeDomain.Scope != null ? _runtimeDomain.Scope.ModelAssembly : null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static ScriptRuntimeScope RuntimeScope
        {
            get
            {
                if (IsCompiling)
                {
                    throw new Exception("Script is compiling.");
                }
                return _runtimeDomain.Scope;
            }
        }

        /// <summary>
        /// Run main class.
        /// </summary>
        public static bool RunMainProgram(params string[] args)
        {
            if (_runtimeDomain == null)
            {
                return false;
            }
            _settupInfo.IsCancelRunning = false;
            //string scriptCode = _settupInfo.ScriptMainProgram;
            //_runtimeDomain.MainInstance = Execute(scriptCode, _settupInfo.ScriptMainTypeName);
            if (_runtimeDomain.MainInstance != null)
            {
                ((dynamic)_runtimeDomain.MainInstance).Start(args);
                return true;
            }
            TraceLog.WriteLine("Error>>Main script instance is not init.");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CancelRunMain()
        {
            _settupInfo.IsCancelRunning = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RunMainWait()
        {
            RunMainAsync(_settupInfo).Wait();
        }

        private static async Task RunMainAsync(ScriptSettupInfo settupInfo)
        {
            while (!settupInfo.IsCancelRunning)
            {
                await Task.Delay(1000);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IMainScript GetCurrentMainScript()
        {
            if (_runtimeDomain == null || _runtimeDomain.MainInstance == null)
            {
                throw new Exception("Not found main script instance, becouse of script has be compiled fail.");
            }
            return _runtimeDomain.MainInstance;
        }

        /// <summary>
        /// Request of main class.
        /// </summary>
        public static void RequestMainProgram(HttpContext context)
        {
            if (_runtimeDomain != null && _runtimeDomain.MainInstance != null)
            {
                ((dynamic)_runtimeDomain.MainInstance).Request(context);
            }
        }

        /// <summary>
        /// stop main class.
        /// </summary>
        public static void StopMainProgram()
        {
            if (_runtimeDomain != null && _runtimeDomain.MainInstance != null)
            {
                ((dynamic)_runtimeDomain.MainInstance).Stop();
            }
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
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.Execute(scriptCode, typeName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static dynamic ExecuteCSharp<T>()
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecuteCSharp(typeof(T).FullName);
        }

        /// <summary>
        /// Process csharp script.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static dynamic ExecuteCSharp(string typeName, params object[] args)
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecuteCSharp(typeName, args);
        }

        /// <summary>
        /// ExecutePython
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public static dynamic ExecutePython(string scriptCode)
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecutePython(scriptCode);
        }

        /// <summary>
        /// 向Lua注册NET的方法
        /// </summary>
        /// <param name="funName"></param>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static void LuaRegister(string funName, object obj, MethodBase method)
        {
            _runtimeDomain.Scope.LuaRegister(funName, obj, method);
        }

        /// <summary>
        /// 向Lua注册NET的方法,方法需要加LuaMethod属性
        /// </summary>
        /// <param name="args"></param>
        public static void LuaRegisterObject(params object[] args)
        {
            _runtimeDomain.Scope.LuaRegisterObject(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ExecuteLua(string funcName, params object[] args)
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecuteLua(funcName, args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaMethod"></param>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ExecuteLua(string luaMethod, string funcName, params object[] args)
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecuteLua(luaMethod, funcName, args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="luaMethod"></param>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ExecuteLuaSource(string[] sources, string luaMethod, string funcName, params object[] args)
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecuteLuaSource(sources, luaMethod, funcName, args);
        }

        /// <summary>
        /// ExecuteCSharp
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static dynamic ExecuteCSharpSource(string[] sources, string[] refAssemblies, string typeName, params object[] args)
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecuteCSharpSource(sources, refAssemblies, typeName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="codeKind"></param>
        /// <returns></returns>
        public static ScriptScope ExecutePythonSource(string sources, string[] refAssemblies, SourceCodeKind codeKind = SourceCodeKind.Statements)
        {
            if (IsCompiling) return null;
            return _runtimeDomain.Scope.ExecutePythonSource(sources, refAssemblies, codeKind);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public static void Dispose()
        {
            _changeWatchingTimer.Dispose();
            _runtimeDomain.Dispose();
        }

    }
}