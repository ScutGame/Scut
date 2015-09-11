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
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PythonRuntimeScope : LuaRuntimeScope
    {
        private ScriptEngine _scriptEngine;
        private string pythonPath;
        private const string FileFilter = "*.py";
        private DictionaryExtend<string, PythonFileInfo> _pythonCodeCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public PythonRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            if (!SettupInfo.DisablePython)
            {
                _pythonCodeCache = new DictionaryExtend<string, PythonFileInfo>();
                string refPath = Path.GetDirectoryName(SettupInfo.PythonReferenceLibFile);
                if (!string.IsNullOrEmpty(refPath) && !Directory.Exists(refPath))
                {
                    Directory.CreateDirectory(refPath);
                }
                pythonPath = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath, SettupInfo.PythonScriptPath);
                AddWatchPath(pythonPath, FileFilter);
                InitPython();
            }
            base.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitPython()
        {
            var pythonOptions = new Dictionary<string, object>();
            pythonOptions["Debug"] = SettupInfo.PythonIsDebug;

            var assm = Assembly.LoadFrom(Path.Combine(SettupInfo.RuntimePrivateBinPath, "IronPython.dll"));
            var type = assm.GetType("IronPython.Hosting.Python", false, true);
            if (type == null)
            {
                throw new Exception("Not found Python class in IronPython.dll");
            }
            _scriptEngine = type.InvokeMember("CreateEngine", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, new[] { pythonOptions }) as ScriptEngine;

            //_scriptEngine = IronPython.Hosting.Python.CreateEngine(pythonOptions);
            _scriptEngine.Runtime.LoadAssembly(typeof(string).Assembly);
            _scriptEngine.Runtime.LoadAssembly(Assembly.GetExecutingAssembly());

            //load python path
            var searchPaths = new List<string> { "*" };
            string libPath = Environment.GetEnvironmentVariable("IRONPYTHONPATH");
            if (string.IsNullOrEmpty(libPath))
            {
                TraceLog.WriteError("The environment variables:\"IRONPYTHONPATH\" is not be setting.");
                TraceLog.WriteLine("# Error>>The environment variables:\"IRONPYTHONPATH\" is not be setting.");
            }
            if (!string.IsNullOrEmpty(libPath))
            {
                string[] items = libPath.Split(';');
                HashSet<string> libPathSet = new HashSet<string>(items.Where(p => p.Length > 0));
                HashSet<string> libDirPaths = new HashSet<string>(libPathSet);
                foreach (var path in libPathSet)
                {
                    var dirArray = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
                    foreach (var dir in dirArray)
                    {
                        libDirPaths.Add(dir);
                    }
                }
                searchPaths.AddRange(libDirPaths);
            }
            if (Directory.Exists(pythonPath))
            {
                searchPaths.Add(pythonPath);
                var dirList = Directory.GetDirectories(pythonPath, "*", SearchOption.AllDirectories);
                searchPaths.AddRange(dirList);
            }
            TraceLog.ReleaseWriteDebug("The py path:{0}", string.Join(@";", searchPaths));
            _scriptEngine.SetSearchPaths(searchPaths.ToArray());

            Load();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Execute(string scriptCode, string typeName, params object[] args)
        {
            object scriptScope = ExecutePython(scriptCode);
            if (scriptScope != null)
            {
                return scriptScope;
            }
            return base.Execute(scriptCode, typeName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="typeArgs"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        /// <returns></returns>
        public override bool InvokeMenthod(string scriptCode, string typeName, object[] typeArgs, string method, params object[] methodArgs)
        {
            object scriptScope = ExecutePython(scriptCode);
            if (scriptScope != null)
            {
                MethodInfo methodInfo = scriptScope.GetType().GetMethod(method);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(scriptScope, methodArgs);
                    return true;
                }
                return false;
            }
            return base.InvokeMenthod(scriptCode, typeName, typeArgs, method, methodArgs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override bool VerifyScriptHashCode(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".py", StringComparison.OrdinalIgnoreCase) != 0)
            {
                return base.VerifyScriptHashCode(fileName);
            }
            string scriptCode = GetScriptCode(fileName);
            if (File.Exists(fileName) && _pythonCodeCache.ContainsKey(scriptCode))
            {
                var old = _pythonCodeCache[scriptCode];
                string source = Decode(File.ReadAllText(fileName), ext);
                return old.HashCode == CryptoHelper.ToMd5Hash(source);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public object ExecutePython(string scriptCode)
        {
            string code = FormatScriptCode(SettupInfo.PythonScriptPath, scriptCode, ".py");
            var scriptInfo = _pythonCodeCache != null ? _pythonCodeCache[code] as PythonFileInfo : null;
            if (!SettupInfo.DisablePython && scriptInfo != null)
            {
                var scope = _scriptEngine.CreateScope();
                scriptInfo.CompiledCode.Execute(scope);
                return scope;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="codeKind"></param>
        /// <returns></returns>
        public ScriptScope ExecutePythonSource(string sources, string[] refAssemblies, SourceCodeKind codeKind = SourceCodeKind.Statements)
        {
            if (refAssemblies != null)
            {
                foreach (var refAssembly in refAssemblies)
                {
                    _scriptEngine.Runtime.LoadAssembly(Assembly.LoadFrom(refAssembly));
                }
            }
            var code = _scriptEngine.CreateScriptSourceFromString(sources, codeKind).Compile();
            var scope = _scriptEngine.CreateScope();
            code.Execute(scope);
            return scope;
        }

        private void Load()
        {
            if (Directory.Exists(pythonPath))
            {
                var files = Directory.GetFiles(pythonPath, FileFilter, SearchOption.AllDirectories);
                foreach (var fileName in files)
                {
                    LoadScript(fileName);
                }
            }
            Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNames"></param>
        public void InitPython(params string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                var scriptFile = LoadScript(fileName);
                CompilePython(scriptFile);
            }
        }

        private void Compile()
        {
            foreach (var pair in _pythonCodeCache)
            {
                CompilePython(pair.Value);
            }

        }

        private void CompilePython(PythonFileInfo scriptFile)
        {
            try
            {
                var scriptSource = _scriptEngine.CreateScriptSourceFromString(scriptFile.Source, Path.GetFullPath(scriptFile.FileName), SourceCodeKind.File);
                scriptFile.CompiledCode = scriptSource.Compile();
                scriptFile.Source = null;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CompilePython script:{0} error:{1}", scriptFile.FileName, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private PythonFileInfo LoadScript(string fileName)
        {
            PythonFileInfo scriptFileInfo = null;
            string scriptCode = GetScriptCode(fileName);
            scriptFileInfo = CreateScriptFile(fileName);
            if (scriptFileInfo != null)
            {
                _pythonCodeCache[scriptCode] = scriptFileInfo;
            }
            return scriptFileInfo;
        }

        private PythonFileInfo CreateScriptFile(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".py", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string fileCode = GetScriptCode(fileName);
                string source = Decode(File.ReadAllText(fileName), ext);
                return new PythonFileInfo(fileCode, fileName)
                {
                    Source = source,
                    HashCode = CryptoHelper.ToMd5Hash(source)
                };
            }
            return null;
        }

    }

}
