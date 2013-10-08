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