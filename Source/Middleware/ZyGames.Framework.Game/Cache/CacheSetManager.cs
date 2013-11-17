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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Cache
{
    [Obsolete("", true)]
    internal class CacheSetManager<T> where T : BaseEntity
    {
        private EntityIndexManager _indexDictionary;

        public CacheSetManager()
        {
            _indexDictionary = new EntityIndexManager(typeof(T).FullName);
        }

        public T FindKey(TDictionary<string, T> cacheData, params object[] keys)
        {
            if (cacheData == null) return default(T);
            T value;
            cacheData.TryGetValue(EntityData.GetKeyCode(keys), out value);
            return value;
        }

        internal void Add(string keyCode, T value)
        {
            SchemaTable schemaTable;
            if (EntitySchemaSet.TryGet<T>(out schemaTable))
            {
                foreach (var pair in schemaTable.IndexList)
                {
                    _indexDictionary.CreateIndex(pair.Key, GetIndexValues(pair.Value, value), keyCode);
                }
            }
        }

        /// <summary>
        /// 删除自定的索引
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="value"></param>
        internal void Remove(string keyCode, T value)
        {
            SchemaTable schemaTable;
            if (EntitySchemaSet.TryGet<T>(out schemaTable))
            {
                foreach (var pair in schemaTable.IndexList)
                {
                    _indexDictionary.RemoveIndex(pair.Key, GetIndexValues(pair.Value, value), keyCode);
                }
            }
        }

        internal object[] GetIndexValues(string[] propertys, T value)
        {
            object[] resultList = new object[propertys.Length];
            for (int i = 0; i < propertys.Length; i++)
            {
                string property = propertys[i];
                resultList[i] = value.GetPropertyValue(property);
            }
            return resultList;
        }

        public TList<T> FindIndex(TDictionary<string, T> cacheData, string indexKey, Predicate<T> match, params object[] keyValues)
        {
            var resultList = new TList<T>();
            TList<string> keyCodeList = new TList<string>();
            SchemaTable schemaTable;
            if (!EntitySchemaSet.TryGet<T>(out schemaTable))
            {
                return resultList;
            }
            _indexDictionary.FindIndex(indexKey, keyValues, keyCode =>
            {
                if (cacheData != null && cacheData.ContainsKey(keyCode))
                {
                    T t = cacheData[keyCode];
                    //检查不匹配的删除
                    string entityVal = string.Empty;
                    string[] propertys;
                    if (schemaTable.IndexList.TryGetValue(indexKey, out propertys))
                    {
                        entityVal = EntityData.GetKeyCode(GetIndexValues(propertys, t));
                    }
                    string tempVal = EntityData.GetKeyCode(keyValues);
                    if (entityVal.Equals(tempVal))
                    {
                        resultList.Add(t);
                    }
                    else
                    {
                        keyCodeList.Add(keyCode);
                    }
                }
            });
            foreach (string keyCode in keyCodeList)
            {
                _indexDictionary.RemoveIndex(indexKey, keyValues, keyCode);
            }
            if (match != null)
            {
                return resultList.FindAll(match);
            }
            return resultList;

        }

        public TList<T> FindAll(TDictionary<string, T> cacheData, Predicate<T> match)
        {
            var list = new TList<T>();
            if (cacheData != null)
            {
                cacheData.Foreach(item =>
                {
                    if (item is T)
                    {
                        var temp = item as T;
                        if (match == null || (match != null && match(temp)))
                        {
                            list.Add(temp);
                        }
                    }
                });
            }
            return list;
        }

    }
}