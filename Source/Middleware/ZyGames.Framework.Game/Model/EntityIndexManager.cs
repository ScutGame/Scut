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
using ZyGames.Framework.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// 索引管理
    /// </summary>
    [Obsolete("", true)]
    internal class EntityIndexManager
    {
        private readonly string _cachekey;
        private const string CachePackageKey = "Index_CachePackage_Key";
        private CachePackage _indexCache;
        private TDictionary<string, EntityIndexList> _entityIndexDict;

        internal EntityIndexManager(string cachekey)
        {
            _cachekey = cachekey;
            _indexCache = new CachePackage(CachePackageKey);

            _indexCache.TryGetCache(_cachekey, out _entityIndexDict);
            if (_entityIndexDict == null)
            {
                _indexCache.AddCache(_cachekey, new TDictionary<string, EntityIndexList>());
                _indexCache.TryGetCache(_cachekey, out _entityIndexDict);
            }
        }

        public void FindIndex(string indexKey, object[] propertyValues, Action<string> action)
        {
            EntityIndexList indexList = new EntityIndexList();
            if (_entityIndexDict != null && _entityIndexDict.ContainsKey(indexKey) && _entityIndexDict[indexKey] != null)
            {
                indexList = _entityIndexDict[indexKey];
            }
            string indexItemKey = BaseEntity.CreateKeyCode(propertyValues);
            EntityIndex entityIndex = indexList[indexItemKey];//当没有indexKey对应键值时，entityIndex为空
            if (entityIndex != null)
            {
                entityIndex.Foreach(action);
            }
            //TraceLog.Write("获得索引对象为空，缓存实体类:{0}，索引键:{1},字典项Key:{2},字典容量:{3}",_cachekey, indexKey, indexItemKey, indexList.Count);
        }

        public void CreateIndex(string indexKey, object[] propertyValues, string indexValue)
        {
            if (_entityIndexDict == null)
            {
                throw new NullReferenceException(string.Format("索引{0}异常,索引字典对象为空！", indexKey));
            }
            EntityIndexList indexList = new EntityIndexList();
            if (_entityIndexDict != null && _entityIndexDict.ContainsKey(indexKey) && _entityIndexDict[indexKey] != null)
            {
                indexList = _entityIndexDict[indexKey];
            }
            indexList.Add(propertyValues, indexValue);

            if (_entityIndexDict != null && _entityIndexDict.ContainsKey(indexKey))
            {
                _entityIndexDict[indexKey] = indexList;
            }
            else if (_entityIndexDict != null)
            {
                _entityIndexDict.Add(indexKey, indexList);
            }
        }

        public void RemoveIndex(string indexKey, object[] propertyValues, string indexValue)
        {
            EntityIndexList indexList = new EntityIndexList();
            if (_entityIndexDict != null && _entityIndexDict.ContainsKey(indexKey) && _entityIndexDict[indexKey] != null)
            {
                indexList = _entityIndexDict[indexKey];
            }
            indexList.Remove(propertyValues, indexValue);

            if (_entityIndexDict != null && _entityIndexDict.ContainsKey(indexKey))
            {
                _entityIndexDict[indexKey] = indexList;
            }
            else if (_entityIndexDict != null)
            {
                _entityIndexDict.Add(indexKey, indexList);
            }
        }
    }
}