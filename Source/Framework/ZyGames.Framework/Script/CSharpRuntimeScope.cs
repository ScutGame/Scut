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
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CSharpRuntimeScope : PythonRuntimeScope
    {
        private string _csharpScriptPath;
        private string _modelScriptPath;
        private const string FileFilter = "*.cs";
        private DictionaryExtend<string, ScriptFileInfo> _modelCodeCache;
        private DictionaryExtend<string, ScriptFileInfo> _csharpCodeCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public CSharpRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
            _modelCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
            _csharpCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
        }

        /// <summary>
        /// 是否是Model类型的脚本
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool IsModelScript(string file)
        {
            return file.IndexOf(_modelScriptPath, StringComparison.CurrentCultureIgnoreCase) != -1;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            _modelScriptPath = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath, SettupInfo.ModelScriptPath);
            AddWatchPath(_modelScriptPath, FileFilter);

            _csharpScriptPath = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath, SettupInfo.CSharpScriptPath);
            AddWatchPath(_csharpScriptPath, FileFilter);

            Load();
            base.Init();
        }

        /// <summary>
        /// Process csharp script.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteCSharp(string typeName, params object[] args)
        {
            object result;
            if (CreateInstance(null, typeName, args, out result)) return result;
            return null;
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
            string code = FormatScriptCode(SettupInfo.CSharpScriptPath, scriptCode, ".cs");
            object result;
            if (CreateInstance(code, typeName, args, out result)) return result;
            return base.Execute(scriptCode, typeName, args);

        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="typeArgs"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        public override bool InvokeMenthod(string scriptCode, string typeName, Object[] typeArgs, string method, params Object[] methodArgs)
        {
            string code = FormatScriptCode(SettupInfo.CSharpScriptPath, scriptCode, ".cs");
            object obj;
            if (CreateInstance(code, typeName, typeArgs, out obj))
            {
                MethodInfo methodInfo = obj.GetType().GetMethod(method);
                if (methodInfo != null)
                {
                    methodInfo.Invoke(obj, methodArgs);
                    return true;
                }
                return false;
            }
            return base.InvokeMenthod(scriptCode, typeName, typeArgs, method, methodArgs);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteCSharpSource(string[] sources, string[] refAssemblies, string typeName, params object[] args)
        {
            var result = ScriptCompiler.CompileSource(sources, refAssemblies, "DynamicCode", SettupInfo.ScriptIsDebug, true);
            if (result != null)
            {
                var type = result.CompiledAssembly.GetType(typeName, false, true);
                if (type != null) return type.CreateInstance(args);
            }
            return null;
        }

        /// <summary>
        /// Init csharp script.
        /// </summary>
        public void InitCsharp()
        {
            string path = _csharpScriptPath;
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, FileFilter, SearchOption.AllDirectories);
                foreach (var fileName in files)
                {
                    LoadScript(path, fileName, true);
                }
            }
            CompileCsharp();
            BuildPythonReferenceFile();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Load()
        {
            var pathList = new String[] { _modelScriptPath, _csharpScriptPath };

            foreach (var path in pathList)
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, FileFilter, SearchOption.AllDirectories);
                    foreach (var fileName in files)
                    {
                        LoadScript(path, fileName);
                    }
                }
            }
            Compile();
            BuildPythonReferenceFile();
        }


        private void Compile()
        {
            CompileModel();
            CompileCsharp();

        }

        private void CompileModel()
        {
            string assemblyName = string.Format("DynamicScripts.{0}", "Model");
            var refAssemblyNames = SettupInfo.ReferencedAssemblyNames.ToArray();
            string[] sources = _modelCodeCache.Select(t =>
            {
                string src = t.Value.Source;
                t.Value.Source = null;
                return src;
            }).ToArray();
            //加载实体程序集
            _modelAssembly = ScriptCompiler.InjectionCompile(SettupInfo.RuntimePrivateBinPath, sources, refAssemblyNames, assemblyName, SettupInfo.ScriptIsDebug, false, out _modelAssemblyPath);

        }

        private void CompileCsharp()
        {
            string assemblyName = string.Format("DynamicScripts.{0}", "CsScript");
            var refAssemblyNames = new List<string>(SettupInfo.ReferencedAssemblyNames.ToArray());
            refAssemblyNames.Add(_modelAssemblyPath);
            string[] sources = _csharpCodeCache.Select(t =>
            {
                string src = t.Value.Source;
                t.Value.Source = null;
                return SettupInfo.ScriptIsDebug ? t.Value.FileName : src;
            }).ToArray();

            //调试模式使用File编译
            var result = SettupInfo.ScriptIsDebug
                ? ScriptCompiler.Compile(sources, refAssemblyNames.ToArray(), assemblyName, SettupInfo.ScriptIsDebug, false, ScriptCompiler.ScriptAssemblyTemp)
                : ScriptCompiler.CompileSource(sources, refAssemblyNames.ToArray(), assemblyName, SettupInfo.ScriptIsDebug, false, ScriptCompiler.ScriptAssemblyTemp);
            if (result != null)
            {
                _csharpAssemblyPath = result.PathToAssembly;
                _csharpAssembly = result.CompiledAssembly;
            }
        }

        private ScriptFileInfo LoadScript(string scriptPath, string fileName, bool isReLoad = false)
        {
            ScriptFileInfo scriptFileInfo = null;
            string scriptCode = GetScriptCode(fileName);
            if (!isReLoad && _csharpCodeCache.ContainsKey(scriptCode))
            {
                var old = _csharpCodeCache[scriptCode];
                if (!File.Exists(fileName) ||
                    old.HashCode == GetFileHashCode(fileName))
                {
                    return old;
                }
            }
            scriptFileInfo = CreateScriptFile(fileName);
            if (scriptFileInfo != null)
            {
                if (scriptPath == _modelScriptPath)
                {
                    _modelCodeCache[scriptCode] = scriptFileInfo;
                }
                else
                {
                    _csharpCodeCache[scriptCode] = scriptFileInfo;
                }
            }
            return scriptFileInfo;
        }

        /// <summary>
        /// 创建脚本文件信息对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private ScriptFileInfo CreateScriptFile(string fileName)
        {
            ScriptFileInfo scriptFileInfo = null;
            if (!File.Exists(fileName))
            {
                return scriptFileInfo;
            }

            FileInfo fi = new FileInfo(fileName);
            if (fi.Extension == ".cs")
            {
                string fileCode = GetScriptCode(fileName);
                scriptFileInfo = new CSharpFileInfo(fileCode, fileName);
                using (var sr = fi.OpenText())
                {
                    scriptFileInfo.Source = Decode(sr.ReadToEnd(), fi.Extension);
                }
                scriptFileInfo.HashCode = CryptoHelper.ToMd5Hash(scriptFileInfo.Source);
            }
            else
            {
                TraceLog.WriteError("Not supported \"{0}\" file type.", fileName);
            }
            return scriptFileInfo;
        }

        private bool CreateInstance(string scriptCode, string typeName, object[] args, out object result)
        {
            result = null;
            if (string.IsNullOrEmpty(scriptCode))
            {
                typeName = typeName ?? "";
                int index = typeName.IndexOf(SettupInfo.CSharpScriptPath + ".", StringComparison.CurrentCultureIgnoreCase);
                if (index > -1)
                {
                    scriptCode = typeName.Substring(index) + ".cs";
                }
                else
                {
                    var arr = typeName.Split(',')[0].Split('.');
                    scriptCode = arr[arr.Length - 1] + ".cs";
                }
            }
            scriptCode = GetScriptCode(scriptCode);
            Assembly assembly = _csharpAssembly;
            ScriptFileInfo scriptInfo = _csharpCodeCache[scriptCode];
            if (scriptInfo == null)
            {
                scriptInfo = _modelCodeCache[scriptCode];
                assembly = _modelAssembly;
            }

            if (scriptInfo != null)
            {
                Type objType = null;
                if (assembly != null)
                {
                    objType = assembly.GetType(typeName, false, true);
                }
                if (objType != null)
                {
                    result = objType.CreateInstance(args);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 生成Python引用的头文件
        /// </summary>
        private void BuildPythonReferenceFile()
        {
            if (SettupInfo.DisablePython)
            {
                return;
            }
            StringBuilder pyCode = new StringBuilder();
            pyCode.AppendLine(@"import clr, sys");
            pyCode.AppendLine(@"clr.AddReference('ZyGames.Framework.Common')");
            pyCode.AppendLine(@"clr.AddReference('ZyGames.Framework')");
            pyCode.AppendLine(@"clr.AddReference('ZyGames.Framework.Game')");
            var assmeblyList = GetAssemblies();
            foreach (var assmebly in assmeblyList)
            {
                pyCode.AppendFormat(@"clr.AddReference('{0}')", assmebly.GetName().Name);
                pyCode.AppendLine();
            }

            try
            {
                using (var sw = File.CreateText(SettupInfo.PythonReferenceLibFile))
                {
                    sw.Write(pyCode.ToString());
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("BuildPythonReferenceFile error:{0}", ex);
            }
        }
    }
}
