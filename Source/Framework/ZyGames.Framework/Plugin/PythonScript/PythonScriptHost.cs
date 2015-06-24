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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Scripting.Hosting;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Plugin.PythonScript
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <code>
    /// </code>
    /// </example>
    [Obsolete("使用ZyGames.Framework.Script.ScriptEngines替代")]
    public static class PythonScriptHost
    {
        private static readonly object syncRoot = new object();
        private static ScriptRuntime _pyRuntime;
        private static ScriptEngine _pyEngine;
        private static ConcurrentDictionary<string, dynamic> _pythonScopes = new ConcurrentDictionary<string, dynamic>();

        private static FileSystemWatcher _pythonWatcher = new FileSystemWatcher();

        /// <summary>
        /// Python自带类库路径
        /// </summary>
        public static string LibPath
        {
            get;
            set;
        }

        /// <summary>
        /// 运行时路径
        /// </summary>
        public static string RuntimePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pythonRuntimePath">运行时相对路径</param>
        /// <param name="pythonLibPath">自带类库路径</param>
        /// <param name="isDebug">是否使用调试模式</param>
        public static void Initialize(string pythonRuntimePath, string pythonLibPath, bool isDebug)
        {
            Dictionary<string, Object> options = new Dictionary<string, object>();
            options["Debug"] = isDebug;
            Initialize(pythonRuntimePath, pythonLibPath, options);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pythonRuntimePath">运行时相对路径</param>
        /// <param name="pythonLibPath">自带类库路径</param>
        /// <param name="options"></param>
        public static void Initialize(string pythonRuntimePath, string pythonLibPath, Dictionary<string, object> options)
        {
			try
			{
	            RuntimePath = (pythonRuntimePath??"").Replace(@"\", "/");
	            LibPath = pythonLibPath;
                var assm = Assembly.LoadFrom(Path.Combine(ZyGames.Framework.Common.MathUtils.RuntimeBinPath, "IronPython.dll"));
                var type = assm.GetType("IronPython.Hosting.Python", false, true);
                if (type == null)
                {
                    throw new Exception("Not found Python class in IronPython.dll");
                }

	            _pyEngine = options == null || options.Count == 0
	                ?  type.InvokeMember("CreateEngine", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new object[0]) as ScriptEngine
	                :  type.InvokeMember("CreateEngine", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new[] { options }) as ScriptEngine;
	            _pyRuntime = _pyEngine.Runtime;
	            SetPythonSearchPath(RuntimePath);

	            RuntimePathWatcher(RuntimePath, true);
				
			}
			catch(Exception ex)
			{
				TraceLog.WriteError ("PythonScriptHost Initialize path:{0} error:{1}", pythonRuntimePath, ex);
			}
        }

        /// <summary>
        /// 
        /// </summary>
        public static ScriptEngine Engine
        {
            get
            {
                return _pyEngine;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static dynamic CreateScope()
        {
            return _pyEngine.CreateScope();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void SetPythonSearchPath(string path)
        {
            path = (path ?? "").Replace(@"\", "/");
            if (!path.EndsWith("/"))
            {
                path = path + "/";
            }
            DirectoryInfo rootDir = new DirectoryInfo(path);
            var itemDirs = rootDir.GetDirectories("*", SearchOption.AllDirectories);
            var searchPaths = new List<string>();
            searchPaths.Add("*");
            searchPaths.Add(LibPath);
            searchPaths.Add(rootDir.FullName);
            foreach (var itemDir in itemDirs)
            {
                searchPaths.Add(itemDir.FullName);
            }
            TraceLog.ReleaseWrite("Python search path:{0}\r\n{1}", path, string.Join(";\r\n", searchPaths));
            SetSearchPaths(searchPaths);
        }

        /// <summary>
        /// 设置查找路径
        /// </summary>
        public static void SetSearchPaths(ICollection<string> paths)
        {
            if (_pyEngine == null)
            {
                throw new Exception("PythonScriptHost is not initialized.");
            }
            _pyEngine.SetSearchPaths(paths);
        }

        /// <summary>
        /// 获取脚本Scope
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static bool GetScriptScope(string moduleName, out dynamic scope)
        {
            moduleName = (moduleName ?? "").Replace(@"\", "/");

            if (_pythonScopes.TryGetValue(moduleName, out scope))
            {
                return true;
            }
            if (TryReLoadScript(moduleName, out scope))
            {
                dynamic old;
                if (_pythonScopes.TryGetValue(moduleName, out old))
                {
                    return _pythonScopes.TryUpdate(moduleName, scope, old);
                }
                return _pythonScopes.TryAdd(moduleName, scope);
            }

            return false;
        }

        /// <summary>
        /// 移除脚本
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static bool TryRemoveScript(string moduleName)
        {
            moduleName = (moduleName ?? "").Replace(@"\", "/");
            dynamic scope;
            return _pythonScopes.TryRemove(moduleName, out scope);
        }

        /// <summary>
        /// 尝试重新加载脚本
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static bool TryReLoadScript(string moduleName, out dynamic scope)
        {
            moduleName = (moduleName ?? "").Replace(@"\", "/");
            scope = null;
            string fileName = Path.Combine(RuntimePath, moduleName);
            try
            {
                lock (syncRoot)
                {
                    if (_pyRuntime != null)
                    {
                        //使用UseFile加载相同的文件时，Scope不会改变；
                        //scope = _pyRuntime.UseFile(fileName);
                        scope = _pyRuntime.ExecuteFile(fileName);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Python script import \"{0}\" error\r\n{1}", fileName, ex);
            }
            return false;
        }

        private static void RuntimePathWatcher(string runtimePath, bool isInclude)
        {
            _pythonWatcher.Path = runtimePath;
            _pythonWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // 只监控.py文件  
            _pythonWatcher.Filter = "*.py";
            //设置是否监听子目录
            _pythonWatcher.IncludeSubdirectories = isInclude;
            // 添加事件处理器。  
            _pythonWatcher.Changed += OnScriptFileChanged;
            _pythonWatcher.Created += OnScriptFileChanged;
            _pythonWatcher.Deleted += OnScriptFileChanged;
            _pythonWatcher.Renamed += OnScriptFileChanged;
            _pythonWatcher.Error += OnScriptFileError;
            // 开始监控。  
            _pythonWatcher.EnableRaisingEvents = true;
        }

        private static void OnScriptFileChanged(object source, FileSystemEventArgs e)
        {
            string moduleName = e.Name;
            try
            {
                TryRemoveScript(moduleName);
                TraceLog.ReleaseWrite("Time:{0}-Python file {1} change load success.", DateTime.Now, moduleName);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError(string.Format("Load python:{0} error", moduleName), ex);
            }
        }

        private static void OnScriptFileError(object sender, ErrorEventArgs e)
        {
            string info = "Python watcher error";
            if (sender is FileSystemWatcher)
            {
                var watcher = (FileSystemWatcher)sender;
                info = string.Format("Python watcher \"{0}\" error:", watcher.Path);
            }
            TraceLog.WriteError("Time:{0}-{1}\r\n{2}", DateTime.Now, info, e.GetException());
        }

    }
}