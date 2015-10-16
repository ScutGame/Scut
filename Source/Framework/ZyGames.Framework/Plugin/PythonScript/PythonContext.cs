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
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Plugin.PythonScript
{
    /// <summary>
    /// Python运行环境上下方对象
    /// </summary>
    [Obsolete("使用ZyGames.Framework.Script.ScriptEngines替代")]
    public class PythonContext : IDisposable
    {
        #region static methond
        
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns></returns>
        public static PythonContext CreateInstance()
        {
            return new PythonContext(PythonScriptHost.CreateScope());
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="moduleName">脚本文件</param>
        /// <example>nu</example>
        /// <returns></returns>
        public static PythonContext CreateInstance(string moduleName)
        {
			try
			{
	            dynamic scope;
	            if (PythonScriptHost.GetScriptScope(moduleName, out scope))
	            {
	                return new PythonContext(scope);
	            }
			}
			catch(Exception ex)
			{
				TraceLog.WriteError ("Create instance of PythonContext error:{0}\r\n{1}", moduleName, ex);
			}
            return null;
        }
        #endregion

        private ScriptScope _scope;
        private int _isDisposed;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope">作用域</param>
        private PythonContext(ScriptScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// 检查对象是否已被显示释放了
        /// </summary>
        protected void CheckDisposed()
        {
            if (_isDisposed == 1)
            {
                throw new Exception(string.Format("The {0} object has be disposed.", this.GetType().Name));
            }
        }
        /// <summary>
        /// 作用域
        /// </summary>
        public dynamic Scope
        {
            get
            {
                CheckDisposed();
                return _scope;
            }
        }

        /// <summary>
        /// 获取对象的变量值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetClassMember<T>(object instance, string name)
        {
            CheckDisposed();
            return PythonScriptHost.Engine.Operations.GetMember<T>(instance, name);
        }

        /// <summary>
        /// 设置对象的变量值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetClassMember<T>(object instance, string name, T value)
        {
            CheckDisposed();
            PythonScriptHost.Engine.Operations.SetMember<T>(instance, name, value);
        }

        /// <summary>
        /// 获取脚本类的实例,T定义类的构造方法要返回类型（Func[object,object]）
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// python code:
        /// class MyClass(object):
        ///     def __init__(self, value):#构造函数
        ///         self.value = value;
        /// 
        /// C#:
        /// PythonContext context = PythonContext.CreateInstance();
        /// var myClass = context.GetClassInstance<Func<object, object>>("MyClass");
        /// var myInstance = myClass("hello");
        /// context.GetClassMember<object>(myInstance, "value");
        /// context.SetClassMember<object>(myInstance, "value", object value);
        /// ]]>
        /// </example>
        /// <typeparam name="T">Func[object,object]</typeparam>
        /// <param name="className"></param>
        /// <returns></returns>
        public T GetClassInstance<T>(string className)
        {
            CheckDisposed();
            return _scope.GetVariable<T>(className);
        }

        /// <summary>
        /// 获取变量
        /// </summary>
        /// <param name="func">变量名</param>
        /// <returns></returns>
        public object GetVariable(string func)
        {
            CheckDisposed();
            return _scope.GetVariable(func);
        }

        /// <summary>
        /// 尝试获取变量
        /// </summary>
        /// <param name="func"></param>
        /// <param name="pyFunc"></param>
        /// <returns></returns>
        public bool TryGetVariable(string func, out object pyFunc)
        {
            CheckDisposed();
            return _scope.TryGetVariable(func, out pyFunc);
        }

        /// <summary>
        /// 获取变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T GetVariable<T>(string func)
        {
            CheckDisposed();
            return _scope.GetVariable<T>(func);
        }

        /// <summary>
        /// 尝试获取变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="pyFunc"></param>
        /// <returns></returns>
        public bool TryGetVariable<T>(string func, out T pyFunc)
        {
            CheckDisposed();
            return _scope.TryGetVariable<T>(func, out pyFunc);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pythonCode"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public T Execute<T>(string pythonCode, SourceCodeKind kind)
        {
            CheckDisposed();
            ScriptSource sourceCode = PythonScriptHost.Engine.CreateScriptSourceFromString(pythonCode, kind);
            return sourceCode.Execute<T>(_scope);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="pythonCode"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public dynamic Execute(string pythonCode, SourceCodeKind kind)
        {
            CheckDisposed();
            ScriptSource sourceCode = PythonScriptHost.Engine.CreateScriptSourceFromString(pythonCode, kind);
            return sourceCode.Execute(_scope);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="pythonCode">编译后的脚本</param>
        /// <returns></returns>
        public dynamic ExecuteDynamic(CompiledCode pythonCode)
        {
            CheckDisposed();
            return pythonCode.Execute(_scope);
        }

        /// <summary>
        /// 动态加载程序集，与主运行程序的运行目录相同
        /// </summary>
        /// <example>
        /// Call:LoadAssembly(new []{"LibforPython.dll"}); 
        /// </example>
        /// <param name="assemblys"></param>
        public void LoadAssembly(string[] assemblys)
        {
            if (assemblys != null && assemblys.Length > 0)
            {
                foreach (var assembly in assemblys)
                {
                    PythonScriptHost.Engine.Runtime.LoadAssembly(Assembly.LoadFrom(assembly));
                }
            }
        }

        /// <summary>
        /// 获取全部变量
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetItems()
        {
            CheckDisposed();
            var enumerable = _scope.GetItems();
            return enumerable.ToDictionary(e => e.Key, e => e.Value);
        }

        /// <summary>
        /// 获取全部变量名
        /// </summary>
        /// <returns></returns>
        public string[] GetVariableNames()
        {
            CheckDisposed();
            var enumerable = _scope.GetVariableNames();
            return enumerable.ToArray();
        }
        /// <summary>
        /// 移除变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RemoveVariable(string name)
        {
            CheckDisposed();
            return _scope.RemoveVariable(name);
        }

        /// <summary>
        /// 设置变量
        /// </summary>
        /// <param name="varList"></param>
        public void SetVariable(PythonParam[] varList)
        {
            CheckDisposed();
            if (varList == null || varList.Length == 0)
            {
                return;
            }

            foreach (var param in varList)
            {
                _scope.SetVariable(param.Name, param.Value);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _scope = null;
                Interlocked.Exchange(ref _isDisposed, 1);
            }
            //释放非托管资源 
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}