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
        /// Script assembly verion
        /// </summary>
        private static int ScriptVerionId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public CSharpRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
        }

        /// <summary>
        /// 是否是Model类型的脚本
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool IsModelScript(string file)
        {
            return file.ToLower().IndexOf(_modelScriptPath.ToLower(), StringComparison.Ordinal) != -1;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            _modelCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
            _csharpCodeCache = new DictionaryExtend<string, ScriptFileInfo>();
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scriptCode"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod<T>(string method, string scriptCode = "")
        {
            object methodResult;
            return InvokeStaticMenthod<T, object>(method, out methodResult, scriptCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="method"></param>
        /// <param name="methodResult"></param>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod<T, TR>(string method, out TR methodResult, string scriptCode = "")
        {
            return InvokeStaticMenthod<T, TR>(method, new Object[0], out  methodResult, scriptCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="scriptCode"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        /// <param name="methodResult"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod<T, TR>(string method, Object[] methodArgs, out TR methodResult, string scriptCode = "")
        {
            methodResult = default(TR);
            object result;
            if (InvokeStaticMenthod(scriptCode, typeof(T).FullName, method, methodArgs, out result))
            {
                if (result != null) methodResult = (TR)result;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        /// <param name="methodResult"></param>
        /// <returns></returns>
        public bool InvokeStaticMenthod(string scriptCode, string typeName, string method, Object[] methodArgs, out object methodResult)
        {
            methodResult = null;
            Type type;
            if (TryParseType(scriptCode, typeName, out type))
            {
                methodResult = type.InvokeMember(method, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, methodArgs);
                return true;
            }
            return false;
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
                Type type = null;
                if (string.IsNullOrEmpty(typeName))
                {
                    type = result.CompiledAssembly.GetTypes()[0];
                }
                else
                {
                    type = result.CompiledAssembly.GetType(typeName, false, true);
                }
                if (type != null) return type.CreateInstance(args);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override bool VerifyScriptHashCode(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".cs", StringComparison.OrdinalIgnoreCase) != 0)
            {
                return base.VerifyScriptHashCode(fileName);
            }
            string scriptCode = GetScriptCode(fileName);
            if (File.Exists(fileName))
            {
                if (fileName.EndsWith("AssemblyInfo.cs", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                ScriptFileInfo code = null;
                if (_modelCodeCache.ContainsKey(scriptCode))
                {
                    code = _modelCodeCache[scriptCode];
                }
                if (_csharpCodeCache.ContainsKey(scriptCode))
                {
                    code = _csharpCodeCache[scriptCode];
                }
                if (code == null) return false;
                string source = Decode(File.ReadAllText(fileName), ext);
                return code.HashCode == CryptoHelper.ToMd5Hash(source);
            }
            return false;
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
                if (files.Length > 0)
                {
                    LoadScriptAssemblyInfo(path);
                }
                foreach (var fileName in files)
                {
                    LoadScript(path, fileName);
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
                    if (files.Length > 0)
                    {
                        LoadScriptAssemblyInfo(path);
                    }
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
                //huhu modify reason: Support for model debugging.
                return SettupInfo.ScriptIsDebug ? t.Value.FileName : src;
            }).ToArray();
            if (sources.Length == 0) return;
            //加载实体程序集
            _modelAssembly = ScriptCompiler.InjectionCompile(SettupInfo.RuntimePrivateBinPath, sources, refAssemblyNames, assemblyName, SettupInfo.ScriptIsDebug, false, out _modelAssemblyPath);
            if (_modelAssembly == null)
            {
                throw new Exception("The model script compile error");
            }
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
            if (sources.Length == 0) return;

            //调试模式使用File编译
            var result = SettupInfo.ScriptIsDebug
                ? ScriptCompiler.Compile(sources, refAssemblyNames.ToArray(), assemblyName, SettupInfo.ScriptIsDebug, false, ScriptCompiler.ScriptAssemblyTemp)
                : ScriptCompiler.CompileSource(sources, refAssemblyNames.ToArray(), assemblyName, SettupInfo.ScriptIsDebug, false, ScriptCompiler.ScriptAssemblyTemp);
            if (result != null)
            {
                _csharpAssemblyPath = result.PathToAssembly;
                _csharpAssembly = result.CompiledAssembly;
            }
            else
            {
                throw new Exception("The csharp script compile error");
            }
        }

        private static string ScriptAssembllyInfo = @"
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(""{0}"")]
[assembly: AssemblyProduct(""{0}"")]
[assembly: AssemblyCopyright(""Copyright Scut"")]
[assembly: ComVisible(false)]
[assembly: Guid(""{1}"")]
[assembly: AssemblyVersion(""1.0.0.{2}"")]
";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptPath"></param>
        private void LoadScriptAssemblyInfo(string scriptPath)
        {
            Interlocked.Increment(ref ScriptVerionId);
            string title = string.Format("Script.{0}", Path.GetFileName(scriptPath));
            string guid = Guid.NewGuid().ToString();
            string path = Path.Combine(scriptPath, "Properties");
            string fileName = Path.Combine(path, "AssemblyInfo.cs");
            string scriptCode = GetScriptCode(fileName);
            string source = string.Format(ScriptAssembllyInfo, title, guid, ScriptVerionId);
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.AppendAllText(fileName, source);
                ScriptFileInfo scriptFile = new CSharpFileInfo(scriptCode, fileName)
                {
                    HashCode = CryptoHelper.ToMd5Hash(source),
                    Source = source
                };

                if (scriptPath == _modelScriptPath)
                {
                    _modelCodeCache[scriptCode] = scriptFile;
                }
                else
                {
                    _csharpCodeCache[scriptCode] = scriptFile;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteWarn("Script AssemblyInfo error:{0}", ex);
            }
        }

        private ScriptFileInfo LoadScript(string scriptPath, string fileName)
        {
            ScriptFileInfo scriptFileInfo = null;
            string scriptCode = GetScriptCode(fileName);
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
            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".cs", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string fileCode = GetScriptCode(fileName);
                scriptFileInfo = new CSharpFileInfo(fileCode, fileName);
                scriptFileInfo.Source = Decode(File.ReadAllText(fileName), ext);
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
            Type type;
            if (TryParseType(scriptCode, typeName, out type))
            {
                result = type.CreateInstance(args);
                return true;
            }
            return false;
        }

        private bool TryParseType(string scriptCode, string typeName, out Type type)
        {
            type = null;
            if (string.IsNullOrEmpty(scriptCode))
            {
                typeName = typeName ?? "";
                scriptCode = ParseScriptCode(typeName);
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
                if (assembly != null)
                {
                    type = assembly.GetType(typeName, false, true);
                }
                return type != null;
            }
            return false;
        }

        private string ParseScriptCode(string typeName)
        {
            string scriptCode;
            int index = typeName.ToLower().IndexOf(SettupInfo.CSharpScriptPath.ToLower() + ".", StringComparison.Ordinal);
            if (index > -1)
            {
                scriptCode = typeName.Substring(index) + ".cs";
            }
            else
            {
                var arr = typeName.Split(',')[0].Split('.');
                scriptCode = arr[arr.Length - 1] + ".cs";
            }
            return scriptCode;
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
                if (assmebly == null) continue;

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
