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
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Build;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// Script compiler
    /// </summary>
    public static class ScriptCompiler
    {
        private readonly static string _dyScriptPath;

        static ScriptCompiler()
        {
            _dyScriptPath = MathUtils.RuntimePath;
        }

        /// <summary>
        /// Get Script generate path.
        /// </summary>
        public static string ScriptPath
        {
            get
            {
                return Path.Combine(_dyScriptPath, ScriptAssemblyTemp);
            }
        }
        /// <summary>
        /// compile temp
        /// </summary>
        public static string CompileTemp = "asmtmp";

        /// <summary>
        /// Script assembly temp
        /// </summary>
        public static string ScriptAssemblyTemp = "temp";

        /// <summary>
        /// Clear temp assmbly.
        /// </summary>
        public static void ClearTemp()
        {
            ClearTemp(ScriptAssemblyTemp);
        }
        /// <summary>
        /// 
        /// </summary>
        public static void ClearScriptRuntimeTemp()
        {
            ClearTemp(typeof(ScriptRuntimeDomain).Name);
        }

        /// <summary>
        /// Clear temp assmbly.
        /// </summary>
        public static void ClearTemp(string dirName)
        {
            string tempPath = Path.Combine(_dyScriptPath, dirName);
            try
            {
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteWarn("ClearTemp dir {0} error:{1}", tempPath, ex);
            }
        }
        /// <summary>
        /// Compile
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="assemblyName"></param>
        /// <param name="isDebug"></param>
        /// <param name="isInMemory"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static CompilerResults Compile(string[] fileNames, string[] refAssemblies, string assemblyName,
            bool isDebug, bool isInMemory, string outputPath = "")
        {
            return Compile(true, fileNames, refAssemblies, assemblyName, isDebug, isInMemory, outputPath);

        }
        /// <summary>
        /// Compile
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="assemblyName"></param>
        /// <param name="isDebug"></param>
        /// <param name="isInMemory"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static CompilerResults CompileSource(string[] sources, string[] refAssemblies, string assemblyName,
            bool isDebug, bool isInMemory, string outputPath = "")
        {
            return Compile(false, sources, refAssemblies, assemblyName, isDebug, isInMemory, outputPath);
        }
        /// <summary>
        /// Compile
        /// </summary>
        /// <param name="isFile"></param>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="assemblyName"></param>
        /// <param name="isDebug"></param>
        /// <param name="isInMemory"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        private static CompilerResults Compile(bool isFile, string[] sources, string[] refAssemblies, string assemblyName, bool isDebug, bool isInMemory, string outputPath = "")
        {
            try
            {
                CodeDomProvider provider = new CSharpCodeProvider();
                CompilerParameters options = new CompilerParameters();
                options.GenerateExecutable = false;
                options.GenerateInMemory = isInMemory;
                options.IncludeDebugInformation = isDebug;
                options.CompilerOptions = "/checked";
                //if (!string.IsNullOrEmpty(assemblyName))
                //{
                //    options.OutputAssembly = assemblyName.EndsWith(".dll") ? assemblyName : assemblyName + ".dll";
                //}

                if (!isInMemory)
                {
                    string tempPath = "";
                    if (string.IsNullOrEmpty(outputPath))
                    {
                        tempPath = Path.Combine(_dyScriptPath, CompileTemp, Guid.NewGuid().ToString());
                    }
                    else
                    {
                        tempPath = Path.Combine(_dyScriptPath, outputPath);
                    }

                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }
                    options.TempFiles = new TempFileCollection(tempPath, isDebug);//调试时保留临时文件
                }
                options.ReferencedAssemblies.Add("System.dll");
                options.ReferencedAssemblies.Add("System.Core.dll");
                options.ReferencedAssemblies.Add("System.Data.dll");
                options.ReferencedAssemblies.Add("System.Xml.dll");
                options.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                options.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                options.ReferencedAssemblies.Add("System.Configuration.dll");
                options.ReferencedAssemblies.Add("System.Web.dll");
                if (refAssemblies != null)
                {
                    foreach (var assembly in refAssemblies)
                    {
                        if (!string.IsNullOrEmpty(assembly) &&
                            !options.ReferencedAssemblies.Contains(assembly))
                        {
                            options.ReferencedAssemblies.Add(assembly);
                        }
                    }
                }
                CompilerResults cr = isFile
                    ? provider.CompileAssemblyFromFile(options, sources)
                    : provider.CompileAssemblyFromSource(options, sources);
                if (cr.Errors.HasErrors)
                {
                    string errStr = string.Format("Compile assembly:{0} error:", assemblyName);
                    foreach (CompilerError err in cr.Errors)
                    {
                        errStr += string.Format("{0}[{1}] File:{2}, Line:{3}{0}Message:{4}",
                            Environment.NewLine,
                            err.IsWarning ? "Warning" : "Error",
                            err.FileName,
                            err.Line,
                            err.ErrorText);
                    }

                    TraceLog.WriteError(errStr);
                    return null;
                }
                return cr;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("CompileCSharp script error:{0}", ex);
                return null;
            }
        }

        /// <summary>
        /// Compile csharp srcipt and injection code.
        /// </summary>
        /// <param name="privateBinPath"></param>
        /// <param name="sources"></param>
        /// <param name="refAssemblies"></param>
        /// <param name="assemblyName"></param>
        /// <param name="isDebug"></param>
        /// <param name="isInMemory"></param>
        /// <param name="pathToAssembly"></param>
        /// <returns></returns>
        internal static Assembly InjectionCompile(string privateBinPath, string[] sources, string[] refAssemblies, string assemblyName, bool isDebug, bool isInMemory, out string pathToAssembly)
        {
            pathToAssembly = null;
            //huhu modify reason: Support for model debugging.
            var result = isDebug
                ? Compile(sources, refAssemblies, assemblyName, isDebug, false, ScriptCompiler.ScriptAssemblyTemp)
                : CompileSource(sources, refAssemblies, assemblyName, isDebug, false);

            //var result = CompileSource(sources, refAssemblies, assemblyName, isDebug, false);
            if (result == null)
            {
                return null;
            }
            pathToAssembly = result.PathToAssembly;
            string outAssembly = Path.Combine(_dyScriptPath, ScriptAssemblyTemp);
            if (!Directory.Exists(outAssembly))
            {
                Directory.CreateDirectory(outAssembly);
            }
            outAssembly = Path.Combine(outAssembly, Path.GetFileNameWithoutExtension(pathToAssembly) + ".dll");
            if (AssemblyBuilder.BuildToFile(pathToAssembly, privateBinPath, outAssembly))
            {
                var ass = AssemblyBuilder.ReadAssembly(outAssembly, null);
                if (ass != null) pathToAssembly = outAssembly;
                ClearTemp(CompileTemp);
                return ass;
            }
            return null;
        }
    }
}