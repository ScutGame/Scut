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
using ZyGames.Framework.Common.Event;

namespace ZyGames.Framework.Event
{
    /// <summary>
    /// 缓存项改变的方式
    /// </summary>
    public enum CacheItemChangeType
    {
        /// <summary>
        /// 重置未改变
        /// </summary>
        UnChange = -1,
        /// <summary>
        /// 增加
        /// </summary>
        Add = 0,
        /// <summary>
        /// 修改
        /// </summary>
        Modify,
        /// <summary>
        /// 移除
        /// </summary>
        Remove,
        /// <summary>
        /// 移除指定
        /// </summary>
        RemoveAll,
        /// <summary>
        /// 清除
        /// </summary>
        Clear,
        /// <summary>
        /// 禁用事件
        /// </summary>
        DisableEvent
    }

    /// <summary>
    /// 缓存项事件参数
    /// </summary>
    public class CacheItemEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public CacheItemEventArgs()
        {
            HasChanged = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="propertyName"></param>
        public CacheItemEventArgs(CacheItemChangeType changeType, string propertyName)
        {
            ChangeType = changeType;
            PropertyName = propertyName;
        }

        /// <summary>
        /// 判断缓存是否已变动
        /// </summary>
        public bool HasChanged
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CacheItemChangeType ChangeType { get; set; }

        /// <summary>
        /// 绑定的字段名
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// The event source.
        /// </summary>
        public object Source { get; set; }
    }

    /// <summary>
    /// 缓存项改变事件
    /// </summary>
    public class CacheItemChangeEvent : CustomEvent<CacheItemEventArgs>
    {
        /// <summary>
        /// 
        /// </summary>
        public CacheItemChangeEvent()
        {
        }

        /// <summary>
        /// 增加单一
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="target"></param>
        public void AddSingleItemEvent(CustomEventHandle<CacheItemEventArgs> handle, object target)
        {
            AddSingle(handle, target);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="target"></param>
        public void AddItemEvent(CustomEventHandle<CacheItemEventArgs> handle, object target)
        {
            Add(handle, target);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="handle"></param>
        public void RemoveItemEvent(CustomEventHandle<CacheItemEventArgs> handle)
        {
            Remove(handle);
        }
    }
}