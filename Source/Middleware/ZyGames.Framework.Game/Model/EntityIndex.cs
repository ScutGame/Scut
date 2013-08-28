using System;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// [程序集内使用]索引对象
    /// </summary>
    [Obsolete("", true)]
    internal class EntityIndex
    {
        private THashSet<string> _indexHashSet = new THashSet<string>();

        
        internal EntityIndex(params object[] propertyValues)
        {
            this.PropertyValues = propertyValues;
        }

        /// <summary>
        /// 索引属性值
        /// </summary>
        internal object[] PropertyValues
        {
            get;
            set;
        }

        /// <summary>
        /// 返回索引值[只读]
        /// </summary>
        internal string[] IndexValueList
        {
            get
            {
                string[] valueList = new string[_indexHashSet.Count];
                int index = 0;
                foreach (string value in _indexHashSet)
                {
                    valueList[index] = value;
                    index++;
                }
                return valueList;
            }
        }

        /// <summary>
        /// 获取索引键值
        /// </summary>
        /// <returns></returns>
        internal string GetKeyCode()
        {
            return BaseEntity.CreateKeyCode(PropertyValues);
        }

        /// <summary>
        /// 返回循环访问对象的枚举数
        /// </summary>
        /// <returns></returns>
        internal void Foreach(Action<string> action)
        {
            string[] tempList;
            _indexHashSet.CopyTo(out tempList, 0);
            foreach (var str in tempList)
            {
                action(str);
            };
        }

        /// <summary>
        /// 增加实体Key
        /// </summary>
        /// <param name="indexValue"></param>
        internal void Add(string indexValue)
        {
            if (!_indexHashSet.Contains(indexValue))
            {
                _indexHashSet.Add(indexValue);
            }
        }

        /// <summary>
        /// 移除实体Key
        /// </summary>
        /// <param name="indexValue"></param>
        internal void Remove(string indexValue)
        {
            if (_indexHashSet.Contains(indexValue))
            {
                _indexHashSet.Remove(indexValue);
            }
        }

    }
}