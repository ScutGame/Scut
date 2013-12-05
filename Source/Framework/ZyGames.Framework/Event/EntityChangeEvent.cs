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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.Event
{
    /// <summary>
    /// 实体变更事件,定义属性时不要使用get;set简写方式，在WebService调用时只读时会有异常
    /// </summary>
    [ProtoContract, Serializable]
    public class EntityChangeEvent : BaseDisposable, IItemChangeEvent
    {
        private bool _isDisableEvent;
        /// <summary>
        /// 
        /// </summary>
        protected EntityChangeEvent(bool isDisable)
        {
            _isDisableEvent = isDisable;
            InitializeChangeEvent();
        }

        /// <summary>
        /// 重置子类绑定事件
        /// </summary>
        protected virtual void InitializeChangeEvent()
        {
            if (!_isDisableEvent)
            {
                _itemEvent = new CacheItemChangeEvent();
                _childrenEvent = new CacheItemChangeEvent();
            }
        }

        /// <summary>
        /// 设置禁用事件绑定
        /// </summary>
        protected void SetDisableEvent()
        {
            _isDisableEvent = true;
            if (_itemEvent != null)
            {
                ItemEvent.Dispose();
                _itemEvent = null;
            }
            if (_childrenEvent != null)
            {
                ChildrenEvent.Dispose();
                _childrenEvent = null;
            }
        }

        /// <summary>
        /// 清除绑定
        /// </summary>
        internal void ClearChangeEvent()
        {
            InitializeChangeEvent();
        }
        /// <summary>
        /// 实体映射的字段名
        /// </summary>
        [JsonIgnore]
        public string PropertyName
        {
            get;
            set;
        }

        private CacheItemChangeEvent _itemEvent;

        /// <summary>
        /// 当前对象绑定的Chang事件
        /// </summary>
        [JsonIgnore]
        public CacheItemChangeEvent ItemEvent
        {
            get { return _itemEvent; }
        }

        private CacheItemChangeEvent _childrenEvent;

        /// <summary>
        /// 当前对象绑定的Chang事件
        /// </summary>
        [JsonIgnore]
        public CacheItemChangeEvent ChildrenEvent
        {
            get { return _childrenEvent; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected bool _hasChanged;

        /// <summary>
        /// 是否有改变
        /// </summary>
        [JsonIgnore]
        public bool HasChanged
        {
            get { return _hasChanged; }
        }

        /// <summary>
        /// 禁用子类事件通知
        /// </summary>
        public void DisableChildNotify()
        {
            UnChangeNotify(this, new CacheItemEventArgs(CacheItemChangeType.DisableEvent, PropertyName));
        }

        /// <summary>
        /// 添加子对象监听
        /// </summary>
        public virtual void AddChildrenListener(object changeEvent)
        {
            if (!_isDisableEvent && ChildrenEvent != null && changeEvent is IItemChangeEvent)
            {
                var child = (IItemChangeEvent)changeEvent;
                ChildrenEvent.AddItemEvent(child.UnChangeNotify);
                var e = child.ItemEvent;
                if (e != null)
                {
                    e.AddItemEvent(NotifyByChildren);
                }
            }
        }

        /// <summary>
        /// 移除子对象监听
        /// </summary>
        public virtual void RemoveChildrenListener(object changeEvent)
        {
            if (!_isDisableEvent && ChildrenEvent != null && changeEvent is IItemChangeEvent)
            {
                var child = (IItemChangeEvent)changeEvent;
                ChildrenEvent.RemoveItemEvent(child.UnChangeNotify);
                var e = child.ItemEvent;
                if (e != null)
                {
                    e.RemoveItemEvent(NotifyByChildren);
                }
            }
        }

        /// <summary>
        /// 触发UnChange事件通知
        /// </summary>
        public virtual void UnChangeNotify(object sender, CacheItemEventArgs eventArgs)
        {
            if (ChildrenEvent != null)
            {
                ChildrenEvent.NotifyAll(sender, eventArgs);
            }
            if (!eventArgs.HasChanged)
            {
                _hasChanged = false;
            }
            if (eventArgs.ChangeType == CacheItemChangeType.DisableEvent)
            {
                SetDisableEvent();
            }

        }

        /// <summary>
        /// 更新通知
        /// </summary>
        /// <param name="updateHandle"></param>
        public void UpdateNotify(Func<IItemChangeEvent, bool> updateHandle)
        {
            if (updateHandle != null)
            {
                if (updateHandle(this))
                {
                    Notify(CacheItemChangeType.Modify, PropertyName);
                }
            }
        }
        /// <summary>
        /// 触发修改通知事件
        /// </summary>
        protected virtual void NotifyByModify()
        {
            Notify(CacheItemChangeType.Modify, PropertyName);
        }

        /// <summary>
        /// 当前对象(包括继承)的属性触发通知事件
        /// </summary>
        protected virtual void Notify(CacheItemChangeType changeType, string propertyName)
        {
            if (!_isDisableEvent)
            {
                _hasChanged = true;
                if (ItemEvent != null)
                {
                    ItemEvent.Notify(this, new CacheItemEventArgs(changeType, propertyName));
                }
            }
        }

        /// <summary>
        /// 当前对象中的属性包含的子类触发通知事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected virtual void NotifyByChildren(object sender, CacheItemEventArgs eventArgs)
        {
            if (!_isDisableEvent)
            {
                _hasChanged = true;
                if (ItemEvent != null)
                {
                    if (string.IsNullOrEmpty(eventArgs.PropertyName))
                    {
                        eventArgs.PropertyName = PropertyName;
                    }
                    ItemEvent.Notify(sender, eventArgs);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //释放 托管资源 
                _itemEvent = null;
                _childrenEvent = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonUtils.SerializeCustom(this);
        }

    }
}