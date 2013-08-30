using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scut.Client.Net;

namespace Scut.Client.Runtime
{
    public static class LuaRuntime
    {
        private static LuaContext _luaContext = new LuaContext();

        public static LuaContext GetContext()
        {
            return _luaContext;
        }

        public static string RuntimePath { get; private set; }

        public static void Start(string rootPath)
        {
            RegisterLuaCallFunc();
            RuntimePath = rootPath;

            string[] files = Directory.GetFiles(rootPath, "*.lua", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                _luaContext.DoFile(file);
            }
        }

        private static void RegisterLuaCallFunc()
        {
            var reader = ScutReader.GetIntance();
            Type readerType = reader.GetType();

            _luaContext.RegisterFunc("ScutReader_getResult", reader, readerType.GetMethod("getResult"));
            _luaContext.RegisterFunc("ScutReader_readAction", reader, readerType.GetMethod("readAction"));
            _luaContext.RegisterFunc("ScutReader_readErrorCode", reader, readerType.GetMethod("readErrorCode"));
            _luaContext.RegisterFunc("ScutReader_readErrorMsg", reader, readerType.GetMethod("readErrorMsg"));
            _luaContext.RegisterFunc("ScutReader_getWORD", reader, readerType.GetMethod("getWORD"));
            _luaContext.RegisterFunc("ScutReader_getInt", reader, readerType.GetMethod("getInt"));
            _luaContext.RegisterFunc("ScutReader_getByte", reader, readerType.GetMethod("getByte"));
            _luaContext.RegisterFunc("ScutReader_getLong", reader, readerType.GetMethod("getLong"));
            _luaContext.RegisterFunc("ScutReader_getDouble", reader, readerType.GetMethod("getDouble"));
            _luaContext.RegisterFunc("ScutReader_readString", reader, readerType.GetMethod("readString"));
            _luaContext.RegisterFunc("ScutReader_recordBegin", reader, readerType.GetMethod("recordBegin"));
            _luaContext.RegisterFunc("ScutReader_recordEnd", reader, readerType.GetMethod("recordEnd"));

            var writer = ScutWriter.getInstance();
            Type writerType = writer.GetType();
            
            _luaContext.RegisterFunc("ScutWriter_writeString", writer, writerType.GetMethod("writeString"));
            _luaContext.RegisterFunc("ScutWriter_writeInt32", writer, writerType.GetMethod("writeInt32"));
            _luaContext.RegisterFunc("ScutWriter_writeWord", writer, writerType.GetMethod("writeWord"));
            _luaContext.RegisterFunc("ScutWriter_writeHead", writer, writerType.GetMethod("writeHead"));

        }

    }
}
