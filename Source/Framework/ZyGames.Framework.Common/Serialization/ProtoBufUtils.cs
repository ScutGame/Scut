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
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime;
using ProtoBuf;
using ProtoBuf.Meta;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Serialization
{
    /// <summary>
    /// Protobuf序列化与反序列化
    /// </summary>
    public static class ProtoBufUtils
    {
        private static RuntimeTypeModel typeModel;

        /// <summary>
        /// byte数组压缩超出(默认10k)开启压缩
        /// </summary>
        public static int CompressOutLength { get; set; }

        static ProtoBufUtils()
        {
            CompressOutLength = 10240;
            Initialize();
        }

        /// <summary>
        /// Initialize RuntimeTypeModel.
        /// </summary>
        public static void Initialize()
        {
            typeModel = TypeModel.Create();
            typeModel.UseImplicitZeroDefaults = false;
        }

        /// <summary>
        /// 使用protobuf序列化对象为二进制文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializeFilePath"></param>
        public static void Serialize(object obj, string serializeFilePath)
        {
            using (var fs = new FileStream(serializeFilePath, FileMode.Create))
            {
                typeModel.Serialize(fs, obj);
            }
        }


        /// <summary>
        /// 使用protobuf反序列化二进制文件为对象
        /// </summary>
        /// <param name="serializeFilePath">反序列化对象的物理文件路径</param>
        /// <param name="type">the type.</param>
        public static object Deserialize(string serializeFilePath, Type type)
        {
            using (var fs = new FileStream(serializeFilePath, FileMode.Open))
            {
                return typeModel.Deserialize(fs, null, type);
            }
        }

        /// <summary>
        /// 使用protobuf把对象序列化为Byte数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Obsolete]
        public static Byte[] ProtobufSerialize<T>(T obj)
        {
            return Serialize(obj);
        }

        //By Seamoon
        /// <summary>
        /// 使用protobuf把对象序列化为Byte数组
        /// </summary>
        /// <param name="obj">序列化的对象</param>
        /// <param name="autoUseGzip"> </param>
        /// <returns></returns>
        public static Byte[] Serialize(object obj, bool autoUseGzip = true)
        {
            return SerializeAutoGZip(obj, autoUseGzip);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        [Obsolete]
        public static T ProtobufDeserialize<T>(Byte[] data) where T : class
        {
            return (T)Deserialize(data, typeof(T));
        }

        /// <summary>
        /// 使用protobuf反序列化二进制数组为对象
        /// </summary>
        /// <typeparam name="T">需要反序列化的对象类型，必须声明[ProtoContract]特征，且相应属性必须声明[ProtoMember(序号)]特征</typeparam>
        /// <param name="data">二进字节数据</param>
        /// <returns></returns>
        public static T Deserialize<T>(Byte[] data)
        {
            return (T)Deserialize(data, typeof(T));
        }

        /// <summary>
        /// 使用protobuf反序列化二进制数组为对象
        /// </summary>
        /// <param name="data">二进字节数据</param>
        /// <param name="type">反序列化的类型.</param>
        /// <param name="isGzip"></param>
        /// <returns></returns>
        public static object Deserialize(Byte[] data, Type type, bool isGzip = true)
        {
            return DeserializeAutoGZip(data, type, isGzip);
        }

        /// <summary>
        /// 序列化对象为二进制数组,并gzip压缩
        /// </summary>
        /// <param name="obj">序列化的对象</param>
        /// <param name="autoUseGzip"> </param>
        /// <returns></returns>
        internal static Byte[] SerializeAutoGZip(object obj, bool autoUseGzip = true)
        {
            using (var memory = new MemoryStream())
            {
                typeModel.Serialize(memory, obj);
                Byte[] bytes = memory.ToArray();

                if (autoUseGzip)
                {
                    if (CompressOutLength > 0 && bytes.Length > CompressOutLength)
                    {
                        using (var gms = new MemoryStream())
                        using (var gzs = new GZipStream(gms, CompressionMode.Compress, true))
                        {
                            gms.Position = 4;//这里从4位之后开始写
                            gzs.Write(bytes, 0, bytes.Length);
                            gzs.Close();
                            bytes = gms.ToArray();
                        }
                    }
                }

                return bytes;
            }
        }

        //By Seamoon
        /// <summary>
        /// 用于判断指定的bytes是否处于gzip状态
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [TargetedPatchingOptOut("Performance critical to inline across NGen")]
        public static bool IsCompressed(byte[] data)
        {
            if (data.Length >= 14 && data[0] == 0 && data[1] == 0 && data[2] == 0 && data[3] == 0 && data[4] == 0x1F &&
                        data[5] == 0x8B)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 反序列化二进制数组为对象,并gzip压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="isGzip"></param>
        /// <returns></returns>
        internal static object DeserializeAutoGZip(Byte[] data, Type type, bool isGzip)
        {
            try
            {
                if (!isGzip)
                {
                    return typeModel.Deserialize(new MemoryStream(data), null, type);
                }
                using (MemoryStream stream = new MemoryStream())
                {
                Start:
                    if (IsCompressed(data))
                    {
                        using (MemoryStream ms = new MemoryStream(data, 4, data.Length - 4))
                        using (GZipStream gzs = new GZipStream(ms, CompressionMode.Decompress))
                        {
                            gzs.CopyTo(stream);
                            var buf = stream.GetBuffer();
                            if (IsCompressed(buf)) //用于防多层gzip
                            {
                                data = stream.ToArray();
                                stream.SetLength(0);
                                goto Start;
                            }
                        }
                    }
                    else
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                    return typeModel.Deserialize(stream, null, type);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ProtoBuf deserialize type:" + type.FullName + " error", ex);
            }
        }

        /// <summary>
        /// 动态绑定实体对象的子类类型
        /// </summary>
        /// <param name="assembly"></param>
        public static void LoadProtobufType(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            Dictionary<Type, int> typeMemberTags = new Dictionary<Type, int>();
            var types = assembly.GetTypes().Where(p => p.GetCustomAttributes(typeof(ProtoContractAttribute), false).Count() > 0).ToList();
            for (int i = 0; i < types.Count; i++)
            {
                var myEntity = types[i];

                //获得所有属性
                var Properties = myEntity.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Where(p => p.GetCustomAttributes(typeof(ProtoMemberAttribute), true).Count() > 0).ToList();
                var Fields = myEntity.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Where(p => p.GetCustomAttributes(typeof(ProtoMemberAttribute), true).Count() > 0).ToList();
                try
                {
                    if (typeModel.CanSerializeContractType(myEntity))
                    {
                        var metaType = typeModel.Add(myEntity, true);
                        int maxMemberTag;
                        LoadProtoTypeMember(myEntity, Properties, metaType, Fields, out maxMemberTag);
                        typeMemberTags[myEntity] = maxMemberTag;

                        //增加继承的子类
                        if (myEntity.BaseType != null)
                        {
                            var parentMetaType = typeModel[myEntity.BaseType];
                            if (parentMetaType != null && typeMemberTags.ContainsKey(myEntity.BaseType))
                            {
                                var parentMemberTag = typeMemberTags[myEntity.BaseType] + 1;
                                parentMetaType.AddSubType(parentMemberTag, myEntity);
                                typeMemberTags[myEntity.BaseType] = parentMemberTag;
                            }
                        }
                    }
                }
                //忽略异常
                catch (Exception e)
                {
                    TraceLog.WriteError("Loading protobuf type \"{0}\" error:{1}", myEntity.FullName, e);
                }

            }

        }

        private static void LoadProtoTypeMember(Type myEntity, List<PropertyInfo> Properties, MetaType metaType, List<FieldInfo> Fields, out int maxMemberTag)
        {
            int memberTag = 0;
            Properties.ForEach((o) =>
            {
                try
                {
                    var fieldNumber = (o.GetCustomAttributes(typeof(ProtoMemberAttribute), false)[0] as ProtoMemberAttribute).Tag;
                    if (memberTag < fieldNumber)
                    {
                        memberTag = fieldNumber;
                    }
                    if (metaType[fieldNumber] == null)
                    {
                        metaType.Add(fieldNumber, o.Name);
                    }
                }
                //忽略异常
                catch (Exception ex)
                {
                    TraceLog.WriteError("Loading protobuf type \"{0}.{1}\" property error:{2}", myEntity.FullName, o.Name, ex);
                }
            });
            Fields.ForEach((o) =>
            {
                try
                {
                    var fieldNumber = (o.GetCustomAttributes(typeof(ProtoMemberAttribute), false)[0] as ProtoMemberAttribute).Tag;
                    if (memberTag < fieldNumber)
                    {
                        memberTag = fieldNumber;
                    }
                    if (metaType[fieldNumber] == null)
                    {
                        metaType.AddField(fieldNumber, o.Name);
                    }
                }
                //忽略异常
                catch (Exception ex)
                {
                    TraceLog.WriteError("Loading protobuf type \"{0}.{1}\" field error:{2}", myEntity.FullName, o.Name, ex);
                }
            });
            maxMemberTag = memberTag;
        }
    }
}