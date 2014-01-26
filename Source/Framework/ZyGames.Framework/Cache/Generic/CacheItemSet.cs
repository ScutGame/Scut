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
    public class CacheItemSet : EntityChangeEvent, IDisposable
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
            : base(isReadOnly)
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
        /// 是否数据已加载成功
        /// </summary>
        public bool HasLoadSuccess
        {
            get { return LoadingStatus == LoadingStatus.Success; }
        }
        /// <summary>
        /// 加载异常
        /// </summary>
        public bool HasLoadError
        {
            get { return LoadingStatus == LoadingStatus.Error; }
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
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _itemData == null;
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
            if (!_isReadOnly)
            {
                BindEvent(_itemData);
                Notify(itemData, CacheItemChangeType.Modify, PropertyName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindEvent(object obj)
        {
            AddChildrenListener(obj);
        }

        /// <summary>
        /// 当前对象(包括继承)的属性触发通知事件
        /// </summary>
        /// <param name="sender">触发事件源</param>
        /// <param name="eventArgs"></param>
        protected override void Notify(object sender, CacheItemEventArgs eventArgs)
        {
            _hasChanged = true;
            PutToChangeKeys(sender);
            DoChangedNotify(sender as AbstractEntity, eventArgs);
        }

        /// <summary>
        /// 当前对象中的属性包含的子类触发通知事件,通知到此层为根
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void NotifyByChildren(object sender, CacheItemEventArgs eventArgs)
        {
            _hasChanged = true;
            PutToChangeKeys(sender);
            DoChangedNotify(sender as AbstractEntity, eventArgs);
        }

        private void DoChangedNotify(AbstractEntity sender, CacheItemEventArgs eventArgs)
        {
            if (OnChangedNotify != null)
            {
                OnChangedNotify.BeginInvoke(sender, eventArgs, null, null);
            }
        }

        internal void SetUnChange()
        {
            _hasChanged = false;
        }

        private void PutToChangeKeys(object sender)
        {
            CacheChangeManager.Current.SetEntity(sender as AbstractEntity);
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
        public void RemoveExpired(string key)
        {
            if (ItemData != null)
            {
                ItemData.RemoveExpired(key);
            }
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
        /// 加载成功
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
        /// 
        /// </summary>
        internal void SetRemoveStatus()
        {
            LoadingStatus = LoadingStatus.Remove;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _period = null;
                _itemData = null;
            }
            base.Dispose(disposing);
        }
    }
}