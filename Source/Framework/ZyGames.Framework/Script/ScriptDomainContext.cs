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
using System.Reflection;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// Script domain context.
    /// </summary>
    public class ScriptDomainContext : MarshalByRefObject
    {
        private ConcurrentDictionary<string, Assembly> _assemblyList;
        /// <summary>
        /// 
        /// </summary>
        public ScriptDomainContext()
        {
            _assemblyList = new ConcurrentDictionary<string, Assembly>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="path"></param>
        public void LoadAssembly(string assemblyKey, string path)
        {
            if (!_assemblyList.ContainsKey(assemblyKey))
            {
                _assemblyList.TryAdd(assemblyKey, Assembly.LoadFrom(path));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <returns></returns>
        public Assembly GetAssembly(string assemblyKey)
        {
            return _assemblyList.ContainsKey(assemblyKey) ? _assemblyList[assemblyKey] : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetTypeFrom(string assemblyKey, string typeName)
        {
            return _assemblyList.ContainsKey(assemblyKey) ? _assemblyList[assemblyKey].GetType(typeName) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns>返回的类型需要加Serializable标识属性</returns>
        public object GetInstance(string assemblyKey, string typeName, params Object[] args)
        {
            var type = GetTypeFrom(assemblyKey, typeName);
            return type != null ? type.CreateInstance(args) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyKey"></param>
        /// <param name="typeName"></param>
        /// <param name="typeArgs"></param>
        /// <param name="method"></param>
        /// <param name="methodArgs"></param>
        /// <returns>返回的类型需要加Serializable标识属性</returns>
        public object Invoke(string assemblyKey, string typeName, Object[] typeArgs, string method, params Object[] methodArgs)
        {
            var type = GetTypeFrom(assemblyKey, typeName);
            if (type == null)
                return null;

            MethodInfo methodInfo = type.GetMethod(method);
            if (methodInfo == null)
                return null;

            Object obj = typeArgs == null ? type.CreateInstance() : type.CreateInstance(typeArgs);
            Object result = methodInfo.Invoke(obj, methodArgs);
            return result;
        }

    }
}
