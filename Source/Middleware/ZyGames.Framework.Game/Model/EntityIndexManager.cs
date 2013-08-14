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