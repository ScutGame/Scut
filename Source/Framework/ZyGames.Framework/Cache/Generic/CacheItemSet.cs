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
using ProtoBuf;
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 实体改变通知事件
    /// </summary>
    public delegate void EntityChangedNotifyEvent(AbstractEntity sender, CacheItemEventArgs eventArgs);

    /// <summary>
    /// 缓存项集合,缓存改变事件通知到此层为根，不需要再向上触发(父亲容器Change事件监听已禁用)
    /// </summary>
    [ProtoContract, Serializable]
    public class CacheItemSet : IDisposable//EntityChangeEvent, 
    {
        private readonly CacheType _cacheItemType;
        private readonly bool _isReadOnly;
        private CachePeriod _period;
        private IDataExpired _itemData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheItemType"></param>
        /// <param name="periodTime"></param>
        /// <param name="isReadOnly"></param>
        public CacheItemSet(CacheType cacheItemType, int periodTime, bool isReadOnly)
        {
            LoadingStatus = LoadingStatus.None;
            _cacheItemType = cacheItemType;
            _isReadOnly = isReadOnly;
            if (periodTime > 0)
            {
                _period = new CachePeriod(periodTime);
            }

        }

        internal event EntityChangedNotifyEvent OnChangedNotify;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventargs"></param>
        protected virtual void DoChangedNotify(AbstractEntity sender, CacheItemEventArgs eventargs)
        {
            EntityChangedNotifyEvent handler = OnChangedNotify;
            if (handler != null) handler(sender, eventargs);
        }

        /// <summary>
        /// 是否数据已加载成功
        /// </summary>
        public bool HasLoadSuccess
        {
            get { return LoadingStatus == LoadingStatus.Success; }
        }
        /// <summary>
        /// 
        /// </summary>
        public LoadingStatus LoadingStatus
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <summary>
        /// 缓存项类型
        /// </summary>
        public CacheType ItemType
        {
            get { return _cacheItemType; }
        }

        /// <summary>
        /// 是否集合
        /// </summary>
        public bool HasCollection { get; internal set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (_itemData == null) return true;
                var data = _itemData as BaseCollection;
                return data != null && data.Count == 0;
            }
        }

        /// <summary>
        /// 获取缓存项对象,不刷新缓存项生命时间
        /// </summary>
        internal IDataExpired ItemData
        {
            get { return _itemData; }
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        public void SetItem(IDataExpired itemData)
        {
            _itemData = itemData;
        }

        /// <summary>
        /// 获取缓存项对象,会刷新缓存项生命时间
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns></returns>
        public IDataExpired GetItem()
        {
            RefreshAccess();
            return ItemData;
        }

        /// <summary>
        /// 刷新访问时间
        /// </summary>
        public void RefreshAccess()
        {
            if (_period != null)
            {
                _period.RefreshAccessTime();
            }
        }

        /// <summary>
        /// 移除过期数据
        /// </summary>
        /// <param name="key"></param>
        public bool RemoveExpired(string key)
        {
            if (ItemData != null)
            {
                return ItemData.RemoveExpired(key);
            }
            return false;
        }

        /// <summary>
        /// 过期对象
        /// </summary>
        public bool IsPeriod
        {
            get
            {
                return _period != null && _period.IsPeriod;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HasChanged { get; private set; }


        /// <summary>
        /// 加载成功,只在加载成功后设置
        /// </summary>
        internal void OnLoadSuccess()
        {
            LoadingStatus = LoadingStatus.Success;
        }

        /// <summary>
        /// 加载失败
        /// </summary>
        internal void OnLoadError()
        {
            LoadingStatus = LoadingStatus.Error;
        }

        /// <summary>
        /// 重置初始状态
        /// </summary>
        internal void ResetStatus()
        {
            LoadingStatus = LoadingStatus.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _period = null;
                _itemData = null;
            }
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        internal bool HasItemChanged
        {
            get
            {
                var t = (_itemData as AbstractEntity);
                return t != null && t.HasChanged;
            }
        }

        /// <summary>
        /// 是否能处理过期
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal bool TryProcessExpired(string key)
        {
            if (_itemData is AbstractEntity)
            {
                var t = ((AbstractEntity)_itemData);
                if (t.HasChanged) return false;

                t.IsInCache = false;
                t.IsExpired = true;
            }
            else if (_itemData is BaseCollection)
            {
                bool hasChanged = false;
                ((BaseCollection)_itemData).Foreach<AbstractEntity>((k, t) =>
                {
                    if (t.HasChanged)
                    {
                        hasChanged = t.HasChanged;
                        return false;
                    }
                    t.IsInCache = false;
                    t.IsExpired = true;
                    return true;
                });

                if (hasChanged) return false;
            }
            return true;
        }
    }
}