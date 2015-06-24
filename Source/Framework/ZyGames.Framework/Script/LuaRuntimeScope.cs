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
using System.IO;
using System.Reflection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Script
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class LuaRuntimeScope : ScriptBaseScope
    {
        private dynamic _luaEngine;
        private string scriptPath;
        private const string FileFilter = "*.lua";
#if LuaInterface
        private const string Lua_Namespace = "LuaInterface";
        private const string LUA_LIB = Lua_Namespace + ".dll";
#else
        private const string Lua_Namespace = "NLua";
        private const string LUA_LIB = Lua_Namespace + ".dll";
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settupInfo"></param>
        public LuaRuntimeScope(ScriptSettupInfo settupInfo)
            : base(settupInfo)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            if (!SettupInfo.DisableLua)
            {
                scriptPath = Path.Combine(SettupInfo.RuntimePath, SettupInfo.ScriptRelativePath, SettupInfo.LuaScriptPath);
                AddWatchPath(scriptPath, FileFilter);

                InitLua();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitLua()
        {
            Assembly luaInterfaceAssembly = null;
            try
            {
                luaInterfaceAssembly = Assembly.LoadFrom(Path.Combine(SettupInfo.RuntimePrivateBinPath, LUA_LIB));
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading Lua library, check \"lua5.dll\" operating platform x86 or x64 or linux", ex);
            }
            Type type = luaInterfaceAssembly.GetType(Lua_Namespace + ".Lua", false, true);
            if (type != null)
            {
                _luaEngine = type.CreateInstance();
                Load();
                RegisterMethod();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funName"></param>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        public void LuaRegister(string funName, object obj, MethodBase method)
        {
            _luaEngine.RegisterFunction(funName, obj, method);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void LuaRegisterObject(params object[] args)
        {
            if (!ScriptEngines.SettupInfo.DisableLua)
            {
                foreach (var obj in args)
                {
                    var methods = obj.GetType().GetMethods();
                    foreach (MethodInfo mInfo in methods)
                    {
                        var attrs = mInfo.GetCustomAttributes(typeof(LuaMethodAttribute));
                        foreach (Attribute attr in attrs)
                        {
                            if (attr != null)
                            {
                                string LuaFunctionName = (attr as LuaMethodAttribute).FuncName;
                                ScriptEngines.LuaRegister(LuaFunctionName, obj, mInfo);
                            }

                        }
                    }
                }
            }
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
            if (!SettupInfo.DisableLua)
            {
                string code = FormatScriptCode(SettupInfo.LuaScriptPath, scriptCode, ".lua");
                return ExecuteLua(code, typeName, args);
            }
            return null;
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
            if (!SettupInfo.DisableLua)
            {
                string func = method;
                object[] args = methodArgs;
                ExecuteLua(scriptCode, func, args);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override bool VerifyScriptHashCode(string fileName)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteLua(string funcName, params object[] args)
        {
            return ExecuteLua("GetFunction", funcName, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaMethod"></param>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteLua(string luaMethod, string funcName, params object[] args)
        {
            switch (luaMethod)
            {
                case "GetFunction":
                    dynamic func = _luaEngine.GetFunction(funcName);
                    object result = func != null ? func.Call(args) : null;
                    if (result != null)
                    {
                        if (result is object[])
                        {
                            var arr = result as object[];
                            return arr.Length == 1 ? arr[0] : arr;
                        }
                        return result;
                    }
                    return null;
                case "GetTable":
                    return _luaEngine.GetTable(funcName);
                case "DoString":
                    return _luaEngine.DoString(funcName);
                case "NewTable":
                    return _luaEngine.NewTable(funcName);
                case "Globals":
                    return _luaEngine.Globals;
                default:
                    throw new NotSupportedException("No \"" + luaMethod + "\" method");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="luaMethod"></param>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteLuaSource(string[] sources, string luaMethod, string funcName, params object[] args)
        {
            foreach (var source in sources)
            {
                LoadFile(source);
            }

            return ExecuteLua(luaMethod, funcName, args);
        }


        private void Load()
        {
            if (Directory.Exists(scriptPath))
            {
                var files = Directory.GetFiles(scriptPath, FileFilter, SearchOption.AllDirectories);
                foreach (var fileName in files)
                {
                    LoadFile(fileName);
                }
            }
        }

        private void LoadFile(string fileName)
        {
            try
            {
                FileInfo fi = new FileInfo(fileName);
                string source = "";
                using (var sr = fi.OpenText())
                {
                    source = Decode(sr.ReadToEnd(), fi.Extension);
                }
                _luaEngine.DoString(source);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Compile lua error:{0}\r\n at file {1}", ex, fileName);
            }
        }

        private void RegisterMethod()
        {
            var writeMethod = typeof(Console).GetMethod("WriteLine", new[] { typeof(string) });
            LuaRegister("CPrint", null, writeMethod);
        }


    }
}
