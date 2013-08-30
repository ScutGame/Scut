using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LuaInterface;

namespace Scut.Client.Runtime
{
    public enum LuaError
    {
        None = 0,
        FuncNotFound = 404,
        Unknown = 1000
    }

    public class LuaContext
    {
        private Lua _luaEngine;

        public LuaContext()
        {
            _luaEngine = new Lua();
        }

        public void DoScript(string script)
        {
            _luaEngine.DoString(script);
        }
        public void DoFile(string fileName)
        {
            _luaEngine.DoFile(fileName);
        }

        public void RegisterFunc(string funName, object obj, MethodBase method)
        {
            _luaEngine.RegisterFunction(funName, obj, method);
        }

        public bool TryCall<TFunc, T>(string funcName, out T result, params object[] args)
        {
            LuaError error;
            return TryCall<TFunc, T>(funcName, out result, out error, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFunc">方法代理类型</typeparam>
        /// <typeparam name="T">方法返回类型</typeparam>
        /// <param name="funcName"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool TryCall<TFunc, T>(string funcName, out T result, out LuaError error, params object[] args)
        {
            result = default(T);
            error = LuaError.Unknown;
            dynamic func;
            if (!TryGetFunc<TFunc>(funcName, out func))
            {
                error = LuaError.FuncNotFound;
                return false;
            }
            result = (T)func.DynamicInvoke(args);
            return true;
        }

        public bool TryCall<TAction>(string funcName, params object[] args)
        {
            dynamic func;
            if (!TryGetFunc<TAction>(funcName, out func))
            {
                return false;
            }
            func.DynamicInvoke(args);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFunc"></typeparam>
        /// <param name="funcName"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public bool TryGetFunc<TFunc>(string funcName, out dynamic func)
        {
            func = _luaEngine.GetFunction(typeof(TFunc), funcName);
            return func != null;
        }

        public short GetVarShort(string var)
        {
            return (short)GetVariable<short>(var);
        }

        public int GetVarInt32(string var)
        {
            return (int)GetVariable<int>(var);
        }

        public double GetVarDouble(string var)
        {
            return (double)GetVariable<double>(var);
        }

        public string GetVarString(string var)
        {
            return (string)GetVariable<string>(var);
        }

        public LuaTable GetVarTable(string var)
        {
            return (LuaTable)GetVariable<LuaTable>(var);
        }

        public ListDictionary GetVarListDict(string var)
        {
            return (ListDictionary)GetVariable<ListDictionary>(var);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var"></param>
        /// <returns></returns>
        private object GetVariable<T>(string var)
        {
            Type type = typeof(T);
            object value = null;
            if (type.Equals(typeof(int)) ||
                type.Equals(typeof(double)) ||
                type.Equals(typeof(short)) ||
                type.Equals(typeof(long)) ||
                type.Equals(typeof(byte)))
            {
                value = _luaEngine.GetNumber(var);
            }
            else if (type.Equals(typeof(string)))
            {
                value = _luaEngine.GetString(var);
            }
            else if (type.Equals(typeof(LuaTable)))
            {
                value = _luaEngine.GetTable(var);
            }
            else if (type.Equals(typeof(ListDictionary)))
            {
                var temp = _luaEngine.GetTable(var);
                value = _luaEngine.GetTableDict(temp);
            }
            else if (type.Equals(typeof(LuaTable)))
            {
                value = _luaEngine.GetTable(var);
            }
            return value;
        }
    }
}
