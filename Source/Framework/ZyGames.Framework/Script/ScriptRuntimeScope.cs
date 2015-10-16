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
using System.Reflection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Security;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// Script runtime scope
    /// </summary>
    [Serializable]
    public abstract class ScriptBaseScope : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly ScriptSettupInfo SettupInfo;
        /// <summary>
        /// 
        /// </summary>
        protected readonly List<string> WatcherPathList;
        private string[] _rootPathArr;
        /// <summary>
        /// 
        /// </summary>
        protected string _modelAssemblyPath;
        /// <summary>
        /// 
        /// </summary>
        protected Assembly _modelAssembly;
        /// <summary>
        /// 
        /// </summary>
        protected string _csharpAssemblyPath;
        /// <summary>
        /// 
        /// </summary>
        protected Assembly _csharpAssembly;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="settupInfo"></param>
        protected ScriptBaseScope(ScriptSettupInfo settupInfo)
        {
            SettupInfo = settupInfo;
            WatcherPathList = new List<string>();
            _rootPathArr = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath).Split('\\', '/', '.');
        }

        /// <summary>
        /// 
        /// </summary>
        public Assembly ModelAssembly
        {
            get { return _modelAssembly; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Assembly[] GetAssemblies()
        {
            return new[] { _modelAssembly, _csharpAssembly };
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> WatcherPaths
        {
            get { return WatcherPathList; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddWatchPath(string path, string filter)
        {
            WatcherPathList.Add(string.Format("{0};{1}", path, filter));
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="scriptCode"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public abstract object Execute(string scriptCode, string typeName, params object[] args);

        /// <summary>
        /// 执行脚本方法, 方法不支持返回值
        /// </summary>
        /// <param name="scriptCode">脚本目录相对路径</param>
        /// <param name="typeName">类完整名</param>
        /// <param name="typeArgs">类构造函数的参数</param>
        /// <param name="method">方法名</param>
        /// <param name="methodArgs">方法参数</param>
        public abstract bool InvokeMenthod(string scriptCode, string typeName, Object[] typeArgs, string method, params Object[] methodArgs);

        /// <summary>
        /// Verify script file's md5 hash code.
        /// </summary>
        /// <returns></returns>
        public abstract bool VerifyScriptHashCode(string fileName);

        /// <summary>
        /// 脚本解码
        /// </summary>
        /// <param name="source">脚本文件</param>
        /// <param name="ext">扩展名</param>
        /// <returns></returns>
        protected string Decode(string source, string ext)
        {
            return SettupInfo.OnDecodeCallback(source, ext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string GetFileHashCode(string fileName)
        {
            return CryptoHelper.ToFileMd5Hash(fileName);
        }


        /// <summary>
        /// filename or typeName,return relative to "Script" path
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetScriptCode(string name)
        {
            string codeString = "";
            var arr = (name ?? "").Split('\\', '/', '.');

            bool issame = true;
            for (int i = 0; i < arr.Length; i++)
            {
                string str = arr[i];
                if (issame && _rootPathArr.Length > i && MathUtils.IsEquals(str, _rootPathArr[i], true))
                {
                    continue;
                }
                issame = false;
                if (codeString.Length > 0) codeString += ".";
                codeString += str;
            }
            return (codeString ?? "").ToLower();
        }

        /// <summary>
        /// 附加脚本扩展名
        /// </summary>
        /// <param name="path">脚本存储目录</param>
        /// <param name="code"></param>
        /// <param name="ext">.cs .py .lua</param>
        /// <returns></returns>
        protected string FormatScriptCode(string path, string code, string ext)
        {
            code = (code ?? "").Replace("/", ".").Replace("\\", ".");
            if (code.EndsWith(".cs") ||
                code.EndsWith(".py") ||
                code.EndsWith(".lua"))
            {
                return code.StartsWith(path)
                    ? string.Format("{0}", code).ToLower()
                    : string.Format("{0}.{1}", path, code).ToLower();
            }
            return code.StartsWith(path)
                ? string.Format("{0}{1}", code, ext).ToLower()
                : string.Format("{0}.{1}{2}", path, code, ext).ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void DoDispose(bool disposing)
        {
            //释放非托管资源 
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }
    }

    /// <summary>
    /// 脚本执行顺序:cs -> py -> lua
    /// </summary>
    [Serializable]
    public class ScriptRuntimeScope : CSharpRuntimeScope
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public ScriptRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            if (_modelAssembly == null)
            {
                //启动时清理脚本运行环境
                ScriptCompiler.ClearTemp();
            }
            base.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void DoDispose(bool disposing)
        {
            base.DoDispose(disposing);
        }

    }
}
