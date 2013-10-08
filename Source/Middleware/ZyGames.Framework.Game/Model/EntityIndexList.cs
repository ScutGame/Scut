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