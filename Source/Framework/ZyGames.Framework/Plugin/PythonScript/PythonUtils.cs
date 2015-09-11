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
using Microsoft.Scripting;

namespace ZyGames.Framework.Plugin.PythonScript
{
    /// <summary>
    /// Python工具
    /// </summary>
    [Obsolete("使用ZyGames.Framework.Script.ScriptEngines替代")]
    public class PythonUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pythonCode"></param>
        /// <param name="kind"></param>
        /// <param name="args"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T ExecuteCode<T>(string pythonCode, SourceCodeKind kind, PythonParam[] args, out PythonContext context)
        {
            return ExecuteCode<T>(new string[0], pythonCode, kind, args, out context);
        }

        /// <summary>
        /// 执行Python代码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblys"></param>
        /// <param name="pythonCode"></param>
        /// <param name="kind"></param>
        /// <param name="args">全局变量</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T ExecuteCode<T>(string[] assemblys, string pythonCode, SourceCodeKind kind, PythonParam[] args, out PythonContext context)
        {
            context = PythonContext.CreateInstance();
            context.LoadAssembly(assemblys);
            context.SetVariable(args);
            return context.Execute<T>(pythonCode, kind);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pythonCode"></param>
        /// <param name="kind"></param>
        /// <param name="args"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object ExecuteCode(string pythonCode, SourceCodeKind kind, PythonParam[] args, out PythonContext context)
        {
            return ExecuteCode(new string[0], pythonCode, kind, args, out context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblys"></param>
        /// <param name="pythonCode"></param>
        /// <param name="kind"></param>
        /// <param name="args">全局变量</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object ExecuteCode(string[] assemblys, string pythonCode, SourceCodeKind kind, PythonParam[] args, out PythonContext context)
        {
            context = PythonContext.CreateInstance();
            context.LoadAssembly(assemblys);
            context.SetVariable(args);
            return context.Execute(pythonCode, kind);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="funcArg"></param>
        /// <param name="varList"></param>
        /// <param name="pyScript"></param>
        /// <returns></returns>
        public static object CallFunc(string funcName, string funcArg, List<PythonParam> varList, string pyScript)
        {
            return CallFunc<object>(new string[0], funcName, funcArg, varList, pyScript);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblys"></param>
        /// <param name="funcName"></param>
        /// <param name="funcArg"></param>
        /// <param name="varList"></param>
        /// <param name="pyScript"></param>
        /// <returns></returns>
        public static object CallFunc(string[] assemblys, string funcName, string funcArg, List<PythonParam> varList, string pyScript)
        {
            return CallFunc<object>(assemblys, funcName, funcArg, varList, pyScript);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcName"></param>
        /// <param name="funcArg"></param>
        /// <param name="varList"></param>
        /// <param name="pyScript"></param>
        /// <returns></returns>
        public static T CallFunc<T>(string funcName, string funcArg, List<PythonParam> varList, string pyScript)
        {
            return CallFunc<T>(new string[0], funcName, funcArg, varList, pyScript);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblys"></param>
        /// <param name="funcName"></param>
        /// <param name="funcArg"></param>
        /// <param name="varList"></param>
        /// <param name="pyScript"></param>
        /// <returns></returns>
        public static T CallFunc<T>(string[] assemblys, string funcName, string funcArg, List<PythonParam> varList, string pyScript)
        {
            PythonContext context;
            ExecuteCode<string>(assemblys, pyScript, SourceCodeKind.Statements, varList.ToArray(), out context);
            if (context != null)
            {
                var funcMain = context.GetVariable<Func<string, T>>(funcName);
                if (funcMain != null)
                {
                    return funcMain(funcArg);
                }
            }
            return default(T);
        }
    }
}