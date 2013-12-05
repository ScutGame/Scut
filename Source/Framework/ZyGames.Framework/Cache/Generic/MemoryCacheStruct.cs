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
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 内存中一定时间存在的实体，不存储数据库
    /// </summary>
    public class MemoryCacheStruct<T> : BaseDisposable
        where T : MemoryEntity, new()
    {

        /// <summary>
        /// 
        /// </summary>
        protected IDataContainer<T> DataContainer;

        /// <summary>
        /// 
        /// </summary>
        public MemoryCacheStruct()
        {
            DataContainer = CacheFactory.GetOrCreate<T>();
        }

        /// <summary>
        /// 是否空
        /// </summary>
        public bool IsEmpty
        {
            get { return DataContainer.IsEmpty; }
        }

        /// <summary>
        /// 容器数量
        /// </summary>
        public int Count
        {
            get { return DataContainer.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void Foreach(Func<string, string, T, bool> func)
        {
            DataContainer.ForeachGroup(func);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="subKey"></param>
        /// <param name="value"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        public bool TryAdd(string groupKey, string subKey, T value, int periodTime = 0)
        {
            return DataContainer.TryAddGroup(groupKey, subKey, value, periodTime);
        }

        /// <summary>
        /// 尝试移除
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="subKey"></param>
        /// <param name="callback">移除成功回调</param>
        /// <returns></returns>
        public bool TryRemove(string groupKey, string subKey, Func<T, bool> callback)
        {
            return DataContainer.TryRemove(groupKey, subKey, callback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool TryRemove(string groupKey, Func<CacheItemSet, bool> callback)
        {
            return DataContainer.TryRemove(groupKey, callback);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public bool TryGet(string groupKey, out BaseCollection collection)
        {
            LoadingStatus loadStatus;
            return DataContainer.TryGetGroup(groupKey, out collection, out loadStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="subKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string groupKey, string subKey, out T value)
        {
            value = default(T);
            BaseCollection collection;
            LoadingStatus loadStatus;
            if (DataContainer.TryGetGroup(groupKey, out collection, out loadStatus))
            {
                return collection.TryGetValue(subKey, out value);
            }
            return false;
        }

        /// <summary>
        /// 查找第一个匹配数据
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Find(string personalId, Predicate<T> match)
        {
            T t = default(T);
            BaseCollection collection;
            if (TryGet(personalId, out collection))
            {
                collection.Foreach<T>((key, value) =>
                {
                    if (match == null || match(value))
                    {
                        t = value;
                        return false;
                    }
                    return true;
                });
            }
            return t;
        }
        /// <summary>
        /// 查找所有匹配数据
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<T> FindAll(string groupKey, Predicate<T> match)
        {
            List<T> list = new List<T>();
            BaseCollection collection;
            if (TryGet(groupKey, out collection))
            {
                collection.Foreach<T>((key, value) =>
                {
                    if (match == null || match(value))
                    {
                        list.Add(value);
                    }
                    return true;
                });
            }
            return list;
        }
    }
}