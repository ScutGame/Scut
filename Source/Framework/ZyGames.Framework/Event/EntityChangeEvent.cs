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
using System.Threading;
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Event
{
    /// <summary>
    /// 实体变更事件
    /// </summary>
    [ProtoContract, Serializable]
    public class EntityChangeEvent : IItemChangeEvent
    {
        private bool _isDisableEvent;
        private readonly object _lockFlag = new object();
        private int _modified;

        /// <summary>
        /// 
        /// </summary>
        public EntityChangeEvent()
            : this(false)
        {

        }
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
                _itemEvent.Dispose();
                _itemEvent = null;
            }
            if (_childrenEvent != null)
            {
                _childrenEvent.Dispose();
                _childrenEvent = null;
            }
        }

        /// <summary>
        /// 清除子类事件句柄
        /// </summary>
        internal void ClearChildrenEvent<T>(IEnumerable<T> childs)
        {
            foreach (var child in childs)
            {
                RemoveChildrenListener(child);
            }
        }
        /// <summary>
        /// 实体映射的字段名
        /// </summary>
        [JsonIgnore]
        internal override string PropertyName
        {
            get;
            set;
        }
        [NonSerialized]
        private CacheItemChangeEvent _itemEvent;

        /// <summary>
        /// 当前对象绑定的Chang事件
        /// </summary>
        [JsonIgnore]
        public override CacheItemChangeEvent ItemEvent
        {
            get { return _itemEvent; }
        }

        [NonSerialized]
        private CacheItemChangeEvent _childrenEvent;

        /// <summary>
        /// 当前对象绑定的Chang事件
        /// </summary>
        [JsonIgnore]
        public override CacheItemChangeEvent ChildrenEvent
        {
            get { return _childrenEvent; }
        }


        /// <summary>
        /// 正在被修改中
        /// </summary>
        [JsonIgnore]
        public bool IsModifying
        {
            get { return _modified == 1; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool _hasChanged;

        /// <summary>
        /// 是否有改变
        /// </summary>
        [JsonIgnore]
        public override bool HasChanged
        {
            get { return _hasChanged; }
        }

        /// <summary>
        /// 禁用子类事件通知
        /// </summary>
        internal override void DisableChildNotify()
        {
            UnChangeNotify(this, new CacheItemEventArgs(CacheItemChangeType.DisableEvent, PropertyName));
        }

        /// <summary>
        /// 添加子对象监听
        /// </summary>
        public override void AddChildrenListener(object changeEvent)
        {
            if (!_isDisableEvent && ChildrenEvent != null && changeEvent is IItemChangeEvent)
            {
                var child = (IItemChangeEvent)changeEvent;
                //注册多个子类的事件
                ChildrenEvent.AddSingleItemEvent(child.UnChangeNotify, null);
                var e = child.ItemEvent;
                if (e != null)
                {
                    //注册父亲类单一事件
                    e.AddSingleItemEvent(NotifyByChildren, this);

                }
            }
        }

        internal void CheckSingleBindEvent(object changeEvent)
        {
            if (!_isDisableEvent && ChildrenEvent != null && changeEvent is IItemChangeEvent)
            {
                var child = (IItemChangeEvent)changeEvent;
                if (child.ItemEvent != null && child.ItemEvent.Parent != null)
                {
                    var parent = child.ItemEvent.Parent as IItemChangeEvent;
                    throw new Exception(string.Format("The \"{0}\" has been the other {1} object binding cannot be added",
                        changeEvent.GetType().FullName,
                        parent != null && parent.ItemEvent != null && parent.ItemEvent.Parent != null
                        ? parent.ItemEvent.Parent.GetType().FullName
                        : parent != null ? parent.PropertyName : ""));
                }
            }
        }

        /// <summary>
        /// 移除子对象监听
        /// </summary>
        internal override void RemoveChildrenListener(object changeEvent)
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
        internal override void UnChangeNotify(object sender, CacheItemEventArgs eventArgs)
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
        public override void UpdateNotify(Func<IItemChangeEvent, bool> updateHandle)
        {
            if (updateHandle != null)
            {
                try
                {
                    DelayNotify();
                    updateHandle(this);
                }
                finally
                {
                    ExitModify();
                    Notify(this, CacheItemChangeType.Modify, PropertyName);
                }
            }
        }

        /// <summary>
        /// Get exclusive modify entity property.
        /// </summary>
        /// <param name="modifyHandle"></param>
        [Obsolete("Use ModifyLocked method", true)]
        public override void ExclusiveModify(Action modifyHandle)
        {
            ModifyLocked(modifyHandle);
        }

        /// <summary>
        /// locked modify value.
        /// </summary>
        /// <param name="action"></param>
        public override void ModifyLocked(Action action)
        {
            if (action != null)
            {
                try
                {
                    EnterLock();
                    DelayNotify();
                    action();
                }
                finally
                {
                    TriggerNotify();
                    ExitLock();
                }
            }
        }

        /// <summary>
        /// 推迟数据改变通知
        /// </summary>
        public void DelayNotify()
        {
            Interlocked.Exchange(ref _modified, 1);
        }

        /// <summary>
        /// 触发通知
        /// </summary>
        public override void TriggerNotify()
        {
            ExitModify();
            Notify(this, CacheItemChangeType.Modify, PropertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ExitModify()
        {
            Interlocked.Exchange(ref _modified, 0);
        }

        /// <summary>
        /// 触发修改通知事件
        /// </summary>
        [Obsolete("not used")]
        protected virtual void NotifyByModify()
        {
            Notify(this, CacheItemChangeType.Modify, PropertyName);
        }

        /// <summary>
        /// 绑定事件且通知
        /// </summary>
        /// <param name="obj"></param>
        protected void BindAndNotify(object obj)
        {
            IItemChangeEvent val = obj as IItemChangeEvent;
            if (val != null)
            {
                val.PropertyName = PropertyName;
                AddChildrenListener(val);
            }
            Notify(this, CacheItemChangeType.Modify, PropertyName);
            if (val != null) val.IsInCache = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="changeType"></param>
        /// <param name="propertyName"></param>
        protected void Notify(object sender, CacheItemChangeType changeType, string propertyName)
        {
            //AbstractEntity子类在内存中时才开启事件通知
            if (IsInCache || !(this is AbstractEntity))
            {
                Notify(sender, new CacheItemEventArgs(changeType, propertyName));
            }
        }

        /// <summary>
        /// 当前对象(包括继承)的属性触发通知事件
        /// </summary>
        /// <param name="sender">触发事件源</param>
        /// <param name="eventArgs"></param>
        protected virtual void Notify(object sender, CacheItemEventArgs eventArgs)
        {
            //modify reason:调用ExclusiveModify方法多个属性被修改时,修改状态延后通知，减少频繁同步数据
            if (!_isDisableEvent && !IsModifying && (IsInCache || !(this is AbstractEntity)))
            {
                IItemChangeEvent val;
                if ((val = sender as IItemChangeEvent) != null && !val.IsInCache)
                {
                    val.IsInCache = true;
                }
                _hasChanged = true;
                if (ItemEvent != null)
                {
                    ItemEvent.Notify(sender, eventArgs);
                }
            }
        }

        /// <summary>
        /// 当前对象中的属性包含的子类触发通知事件
        /// </summary>
        /// <param name="sender">触发事件源</param>
        /// <param name="eventArgs"></param>
        protected virtual void NotifyByChildren(object sender, CacheItemEventArgs eventArgs)
        {
            if (!_isDisableEvent && (IsInCache || !(this is AbstractEntity)))
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
                if (_itemEvent != null)
                {
                    _itemEvent.Dispose();
                    _itemEvent = null;
                }
                if (_childrenEvent != null)
                {
                    _childrenEvent.Dispose();
                    _childrenEvent = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Enter lock
        /// </summary>
        public void EnterLock()
        {
            Monitor.Enter(_lockFlag);
        }

        /// <summary>
        /// Exit lock
        /// </summary>
        public void ExitLock()
        {
            Monitor.Exit(_lockFlag);
        }
        /// <summary>
        /// To json string.
        /// </summary>
        /// <returns></returns>
        public override string ToJson()
        {
            return JsonUtils.SerializeCustom(this);
        }

    }
}