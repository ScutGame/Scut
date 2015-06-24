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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Common.Serialization
{

    /// <summary>
    /// Json序列化反序列化器
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// 序列化成中国标准时间,精确值到秒
        /// </summary>
        /// <param name="entity">待序列化的对象</param>
        /// <returns></returns>
        public static string SerializeCustom(object entity)
        {
            return SerializeCustom(entity, "yyyy'-'MM'-'dd' 'HH':'mm':'ss");
        }

        /// <summary>
        /// 序列化成中国标准时间,精确值到秒
        /// </summary>
        /// <param name="entity">待序列化的对象</param>
        /// <param name="formatDate">指定日期格式</param>
        /// <returns></returns>
        public static string SerializeCustom(object entity, string formatDate)
        {
            return Serialize(entity, new IsoDateTimeConverter() { DateTimeFormat = formatDate });
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="entity">待序列化的对象</param>
        /// <param name="converters">自定转换的处理</param>
        /// <returns></returns>
        public static string Serialize(object entity, params JsonConverter[] converters)
        {
            if (entity == null)
            {
                return string.Empty;
            }
            try
            {
                return JsonConvert.SerializeObject(entity, converters);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Serialize object:{0},Error:{1}", entity.GetType().FullName, ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 自定义日期反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="entity">待反序列化的对象</param>
        /// <param name="formatDate">指定日期格式</param>
        /// <returns></returns>
        public static T DeserializeCustom<T>(string entity, string formatDate = "yyyy'-'MM'-'dd' 'HH':'mm':'ss")
        {
            return Deserialize<T>(entity, new IsoDateTimeConverter() { DateTimeFormat = formatDate });
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="entity">待反序列化的对象</param>
        /// <param name="converters">自定转换的处理</param>
        /// <returns></returns>
        public static T Deserialize<T>(string entity, params JsonConverter[] converters)
        {
            if (string.IsNullOrEmpty(entity))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(entity, converters);
        }

        /// <summary>
        /// 自定义日期反序列化
        /// </summary>
        /// <param name="entity">待反序列化的对象</param>
        /// <param name="type">对象类型</param>
        /// <param name="formatDate">指定日期格式</param>
        /// <returns></returns>
        public static object DeserializeCustom(string entity, Type type, string formatDate = "yyyy'-'MM'-'dd' 'HH':'mm':'ss")
        {
            return Deserialize(entity, type, new IsoDateTimeConverter() { DateTimeFormat = formatDate });
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="entity">待反序列化的对象</param>
        /// <param name="type">对象类型</param>
        /// <param name="converters">自定转换的处理</param>
        /// <returns></returns>
        public static object Deserialize(string entity, Type type, params JsonConverter[] converters)
        {
            if (type == null || string.IsNullOrEmpty(entity))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(entity, type, converters);
        }
    }
}