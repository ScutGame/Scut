using System;
using System.Collections;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Script;

namespace Game.Script
{
    public class ScriptProxy
    {
        public static void Load(string type, string[] files)
        {
            try
            {
                if (".cs".Equals(type))
                {
                    //注册Lua调用C#方法
                    RegistMethodd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void RegistMethodd()
        {
            ScriptProxy proxy = new ScriptProxy();
            ScriptEngines.LuaRegisterObject(proxy);
        }

        [LuaMethod("ContainsParam")]
        public bool ContainsParam(ActionGetter actionGetter, string name)
        {
            return actionGetter != null && actionGetter.Contains(name);
        }

        [LuaMethod("ReadStringParam")]
        public string ReadStringParam(ActionGetter actionGetter, string name)
        {
            return actionGetter == null ? null : actionGetter.GetStringValue(name);
        }

        [LuaMethod("ReadNumberParam")]
        public int ReadNumberParam(ActionGetter actionGetter, string name)
        {
            return actionGetter == null ? -1 : actionGetter.GetIntValue(name);
        }

        [LuaMethod("PushIntoStack")]
        public void PushIntoStack(DataStruct writer, object value)
        {
            if (value is DataStruct)
            {
                writer.PushIntoStack((DataStruct)value);
            }
            else if (value is int)
            {
                writer.PushIntoStack((int)value);
            }
            else if (value is double)
            {
                writer.PushIntoStack(value.ToInt());
            }
            else if (value is short)
            {
                writer.PushIntoStack((short)value);
            }
            else
            {
                writer.PushIntoStack(value.ToString());
            }
        }

        [LuaMethod("PushLenIntoStack")]
        public void PushLenIntoStack(DataStruct writer, object value)
        {
            var list = value as IList;
            writer.PushIntoStack(list == null ? 0 : list.Count);
        }


        [LuaMethod("CreateDataStruct")]
        public DataStruct CreateDataStruct()
        {
            return new DataStruct();
        }


        [LuaMethod("FormatDateString")]
        public string FormatDateString(DateTime value, string formart)
        {
            return value.ToString(formart);
        }


    }
}