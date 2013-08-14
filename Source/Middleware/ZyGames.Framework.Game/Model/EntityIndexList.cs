using System;
using ZyGames.Framework.Collection;

namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// [程序集内使用]索引项对象集合
    /// </summary>
    [Obsolete("", true)]
    internal class EntityIndexList
    {
        private TDictionary<string, EntityIndex> _entityIndexDict = new TDictionary<string, EntityIndex>();

        /// <summary>
        /// 索引器数
        /// </summary>
        internal int Count
        {
            get
            {
                return _entityIndexDict.Count;
            }
        }

        /// <summary>
        /// 增加索引器
        /// </summary>
        /// <param name="propertyValues"></param>
        /// <param name="indexValue"></param>
        internal void Add(object[] propertyValues, string indexValue)
        {
            EntityIndex entityIndex = new EntityIndex(propertyValues);
            string indexItemKey = entityIndex.GetKeyCode();
            if (_entityIndexDict.ContainsKey(indexItemKey))
            {
                entityIndex = _entityIndexDict[indexItemKey];
            }
            entityIndex.Add(indexValue);

            if (_entityIndexDict.ContainsKey(indexItemKey))
            {
                _entityIndexDict[indexItemKey] = entityIndex;
            }
            else
            {
                _entityIndexDict.Add(indexItemKey, entityIndex);
            }
        }

        /// <summary>
        /// 移除索引器
        /// </summary>
        /// <param name="propertyValues"></param>
        /// <param name="indexValue"></param>
        internal void Remove(object[] propertyValues, string indexValue)
        {
            EntityIndex entityIndex = new EntityIndex(propertyValues);
            string indexItemKey = entityIndex.GetKeyCode();
            if (_entityIndexDict.ContainsKey(indexItemKey))
            {
                entityIndex = _entityIndexDict[indexItemKey];
            }
            entityIndex.Remove(indexValue);

            if (_entityIndexDict.ContainsKey(indexItemKey))
            {
                _entityIndexDict[indexItemKey] = entityIndex;
            }
            else
            {
                _entityIndexDict.Add(indexItemKey, entityIndex);
            }
        }

        /// <summary>
        /// 获取或设置实体索引
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        internal EntityIndex this[string keyCode]
        {
            get
            {
                if (_entityIndexDict.ContainsKey(keyCode))
                {
                    return _entityIndexDict[keyCode];
                }
                return null;
            }
            set
            {
                if (_entityIndexDict.ContainsKey(keyCode))
                {
                    _entityIndexDict[keyCode] = value;
                }
            }
        }

    }
}