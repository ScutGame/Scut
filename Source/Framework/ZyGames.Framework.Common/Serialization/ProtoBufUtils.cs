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
using System.Linq;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProtoBufUtils
    {
        /// <summary>
        /// 使用protobuf序列化对象为二进制文件
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型，必须声明[ProtoContract]特征，且相应属性必须声明[ProtoMember(序号)]特征</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="serializeFilePath">序列化后的物理文件路径</param>
        public static void ProtobufSerialize<T>(T obj, string serializeFilePath)
        {
            using (var fs = new FileStream(serializeFilePath, FileMode.Create))
            {
                Serializer.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// 使用protobuf反序列化二进制文件为对象
        /// </summary>
        /// <typeparam name="T">需要反序列化的对象类型，必须声明[ProtoContract]特征，且相应属性必须声明[ProtoMember(序号)]特征</typeparam>
        /// <param name="serializeFilePath">反序列化对象的物理文件路径</param>
        public static T ProtobufDeserialize<T>(string serializeFilePath) where T : class
        {
            using (var fs = new FileStream(serializeFilePath, FileMode.Open))
            {
                return Serializer.Deserialize<T>(fs);
            }
        }

        /// <summary>
        /// 使用protobuf把对象序列化为Byte数组
        /// </summary>
        /// <typeparam name="T">需要反序列化的对象类型，必须声明[ProtoContract]特征，且相应属性必须声明[ProtoMember(序号)]特征</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte[] ProtobufSerialize<T>(T obj)
        {
            using (var memory = new MemoryStream())
            {
                Serializer.Serialize(memory, obj);
                return memory.ToArray();
            }
        }

        /// <summary>
        /// 使用protobuf反序列化二进制数组为对象
        /// </summary>
        /// <typeparam name="T">需要反序列化的对象类型，必须声明[ProtoContract]特征，且相应属性必须声明[ProtoMember(序号)]特征</typeparam>
        /// <param name="data"></param>
        public static T ProtobufDeserialize<T>(Byte[] data) where T : class
        {
            using (var memory = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(memory);
            }
        }

        /// <summary>
        /// 动态绑定实体对象的子类类型
        /// </summary>
        /// <param name="assembly"></param>
        public static void LoadProtobufType(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(p => p.GetCustomAttributes(typeof(ProtoContractAttribute), false).Count() > 0).ToList();
            for (int i = 0; i < types.Count; i++)
            {
                var myEntity = types[i];

                //获得所有属性
                var Properties = myEntity.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Where(p => p.GetCustomAttributes(typeof(ProtoMemberAttribute), true).Count() > 0).ToList();
                var Fields = myEntity.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Where(p => p.GetCustomAttributes(typeof(ProtoMemberAttribute), true).Count() > 0).ToList();
                try
                {

                    if (RuntimeTypeModel.Default.CanSerializeContractType(myEntity))
                    {
                        var metaType = RuntimeTypeModel.Default.Add(myEntity, false);
                        Properties.ForEach((o) =>
                        {
                            try
                            {
                                var fieldNumber = (o.GetCustomAttributes(typeof(ProtoMemberAttribute), false)[0] as ProtoMemberAttribute).Tag;
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
                    }
                }
                //忽略异常
                catch (Exception e)
                {
                    TraceLog.WriteError("Loading protobuf type \"{0}\" error:{1}", myEntity.FullName, e);
                }

            }

        }
    }
}