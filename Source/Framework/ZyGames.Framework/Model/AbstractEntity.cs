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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Reflect;
using ZyGames.Framework.Event;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// 实体数据基类
    /// </summary>
    [ProtoContract, Serializable]
    public abstract class AbstractEntity : EntityChangeEvent, IDataExpired, IComparable<AbstractEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        protected const char KeyCodeJoinChar = '-';

        /// <summary>
        /// 
        /// </summary>
        protected const int DefIdentityId = 10000;
        /// <summary>
        /// 存储改变的属性集合
        /// </summary>
        private readonly ConcurrentQueue<string> _changePropertys = new ConcurrentQueue<string>();
        /// <summary>
        /// 等待更新属性
        /// </summary>
        private readonly HashSet<string> _waitUpdateList = new HashSet<string>();

        private ObjectAccessor _typeAccessor;


        /// <summary>
        /// 
        /// </summary>
        protected AbstractEntity()
            : this(false)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Model.AbstractEntity"/> class.
        /// </summary>
        /// <param name="access">Access.</param>
        protected AbstractEntity(AccessLevel access)
            : this(access == AccessLevel.ReadOnly)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isReadOnly"></param>
        protected AbstractEntity(bool isReadOnly)
            : base(isReadOnly)
        {
            _isNew = true;
            _isLoading = false;
            _isReadOnly = isReadOnly;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Init()
        {
            _typeAccessor = ObjectAccessor.Create(this, true);
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        internal void Reset()
        {
            OnUnNew();
            DequeueChangePropertys();
            CompleteUpdate();
            var e = new CacheItemEventArgs { ChangeType = CacheItemChangeType.UnChange };
            UnChangeNotify(this, e);
        }

        private ObjectAccessor GetAccessor()
        {
            if (_typeAccessor == null)
            {
                _typeAccessor = ObjectAccessor.Create(this, true);
            }
            return _typeAccessor;
        }

        /// <summary>
        /// 设置属性值之前处理
        /// </summary>
        internal void SetValueBefore()
        {
            _isLoading = true;
        }

        /// <summary>
        /// 设置属性值之后处理
        /// </summary>
        internal void SetValueAfter()
        {
            ResetChangePropertys();
            _isLoading = false;
            OnUnNew();
        }
        #region property

        /// <summary>
        /// 初始键值，未配置EntityField属性时
        /// </summary>
        [JsonIgnore]
        protected virtual string KeyValue { get { return ""; } }
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(100020)]
        protected bool _isReadOnly;
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
        }

        private bool _isLoading;
        /// <summary>
        /// 判断是否处理加载中设置属性,只读实体为False后才不可修改
        /// </summary>
        [JsonIgnore]
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(100021)]
        protected bool _isNew;
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public bool IsNew
        {
            get
            {
                return _isNew;
            }
        }

        [ProtoMember(100022)]
        private bool _isRemoveFlag;

        /// <summary>
        /// Is remoce flag.
        /// </summary>
        [JsonIgnore]
        public virtual bool IsRemoveFlag
        {
            get { return _isRemoveFlag; }
            protected set
            {
                if (!Equals(_isRemoveFlag, value))
                {
                    _isRemoveFlag = value;
                    Notify(this, CacheItemChangeType.Remove, PropertyName);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool _isDelete;

        /// <summary>
        /// 删除实体标记，将从源数据（DB、Redis）中删除
        /// </summary>
        [JsonIgnore]
        [ProtoMember(100023)]
        public bool IsDelete
        {
            get
            {
                return _isDelete;
            }
            internal set
            {
                //写Redis时标记,但不触发事件
                _isDelete = value;
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //[ProtoMember(100024)]
        //protected AccessLevel _access;
        ///// <summary>
        ///// 内部可见
        ///// </summary>
        //[JsonIgnore]
        //internal AccessLevel Access
        //{
        //    get { return _access; }
        //}

        /// <summary>
        /// 分组Key
        /// </summary>
        [JsonIgnore]
        public virtual string PersonalId
        {
            get
            {
                int id = GetIdentityId();
                if (id <= 0)
                {
                    TraceLog.WriteError("The {0} property \"PersonalId\" is empty.", GetType().FullName);
                }
                return id.ToString();
            }
        }

        #endregion

        /// <summary>
        /// 标识ID，消息队列分发
        /// </summary>
        internal protected abstract int GetIdentityId();

        /// <summary>
        /// 当前对象(包括继承)的属性触发通知事件
        /// </summary>
        /// <param name="sender">触发事件源</param>
        /// <param name="eventArgs"></param>
        protected override void Notify(object sender, CacheItemEventArgs eventArgs)
        {
            eventArgs.Source = sender;
            AddChangePropertys(eventArgs.PropertyName);
            base.Notify(this, eventArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">由IContainer对象触发</param>
        /// <param name="eventArgs"></param>
        protected override void NotifyByChildren(object sender, CacheItemEventArgs eventArgs)
        {
            eventArgs.Source = sender;
            AddChangePropertys(eventArgs.PropertyName);
            //更改子类事件触发者
            base.NotifyByChildren(this, eventArgs);
        }

        /// <summary>
        /// 设置UnChange事件通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        internal override void UnChangeNotify(object sender, CacheItemEventArgs eventArgs)
        {
            if (HasChangePropertys)
            {
                //中断UnChange事件通知
                eventArgs.HasChanged = true;
                return;
            }
            CompleteUpdate();
            base.UnChangeNotify(sender, eventArgs);
        }

        /// <summary>
        /// 对象索引器属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns></returns>
        protected virtual object this[string name]
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canRead"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        internal object GetPropertyValue(bool canRead, string property)
        {
            if (canRead)
            {
                return GetPropertyValue(property);
            }
            return this[property];
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public object GetPropertyValue(string property)
        {
            try
            {
                return GetAccessor()[property];
            }
            catch (Exception ex)
            {
                try
                {
                    return this[property];
                }
                catch
                {
                    TraceLog.WriteError("The {0} get property:{1}\r\n{2}", GetType().Name, property, ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Set property value.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetPropertyValue(string property, object value)
        {
            try
            {
                GetAccessor()[property] = value;
            }
            catch (Exception ex)
            {
                try
                {
                    this[property] = value;
                }
                catch
                {
                    TraceLog.WriteError("The {0} set property:{1} value:{2}\r\n{3}", GetType().Name, property, value, ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        internal void SetFieldValue(string name, object value)
        {
            this[name] = value;
        }

        /// <summary>
        /// 生成Key
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string CreateKeyCode(params object[] keys)
        {
            string value = "";
            foreach (var key in keys)
            {
                if (value.Length > 0)
                {
                    value += "-";
                }
                value += key.ToNotNullString();
            }
            return value;
        }

        /// <summary>
        /// 获取实体KEY
        /// </summary>
        /// <returns></returns>
        public string GetKeyCode()
        {
            string value = KeyValue;
            SchemaTable entitySchema;
            if (EntitySchemaSet.TryGet(GetType(), out entitySchema))
            {
                foreach (string key in entitySchema.Keys)
                {
                    if (value.Length > 0)
                    {
                        value += KeyCodeJoinChar;
                    }
                    value += GetPropertyValue(key).ToNotNullString();
                }
                if (string.IsNullOrEmpty(value))
                {
                    TraceLog.WriteError("Entity {0} primary key is empty.", entitySchema.Name);
                }
            }

            return value;
        }

        /// <summary>
        /// 获得实体DB架构信息
        /// </summary>
        /// <returns>返回值会为Null</returns>
        public SchemaTable GetSchema()
        {
            SchemaTable schemaTable;
            Type type = GetType();
            if (!EntitySchemaSet.TryGet(type, out schemaTable))
            {
                //若实体架构信息为空，则重新加载架构信息
                EntitySchemaSet.InitSchema(type);
                if (!EntitySchemaSet.TryGet(type, out schemaTable))
                {
                    TraceLog.WriteError("Class:{0} scheam is not setting.", type.FullName);
                }
            }
            return schemaTable;
        }

        /// <summary>
        /// Mark entity remove state.
        /// </summary>
        public virtual void OnRemoveFlag()
        {
            IsRemoveFlag = true;
        }

        /// <summary>
        /// The entity has be delete from DB and Redis.
        /// </summary>
        public void OnDelete()
        {
            _isDelete = true;
            Notify(this, CacheItemChangeType.Remove, PropertyName);
        }

        /// <summary>
        /// 设置为非新的实体
        /// </summary>
        internal void OnUnNew()
        {
            _isNew = false;
        }
        /// <summary>
        /// 在索引属性调用
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        protected void SetChange(string propertyName, object value)
        {
            this[propertyName] = value;
            Notify(this, CacheItemChangeType.Modify, propertyName);
        }

        /// <summary>
        /// 绑定事件且通知
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        protected void BindAndChangeProperty(object obj, string propertyName)
        {
            IItemChangeEvent val = obj as IItemChangeEvent;
            if (val != null)
            {
                val.PropertyName = propertyName;
                if (IsReadOnly)
                {
                    val.DisableChildNotify();
                }
                AddChildrenListener(val);
            }
            Notify(this, CacheItemChangeType.Modify, propertyName);
        }

        /// <summary>
        /// 设置变更的属性
        /// </summary>
        /// <param name="propertyName"></param>
        public void SetChangeProperty(string propertyName)
        {
            IItemChangeEvent val = null;
            try
            {
                val = GetAccessor()[propertyName] as IItemChangeEvent;
            }
            catch (Exception ex)
            {
                try
                {
                    val = this[propertyName] as IItemChangeEvent;
                }
                catch
                {
                    throw new Exception("Get property:" + GetType().Name + "." + propertyName, ex);
                }
            }
            if (val != null)
            {
                val.PropertyName = propertyName;
                if (IsReadOnly)
                {
                    val.DisableChildNotify();
                }
                AddChildrenListener(val);
            }
            Notify(this, CacheItemChangeType.Modify, propertyName);
        }

        private void AddChangePropertys(string propertyName)
        {
            //在加载初始数据时，不设置Change属性
            if (!IsLoading && !string.IsNullOrEmpty(propertyName))
            {
                _changePropertys.Enqueue(propertyName);
            }
        }

        /// <summary>
        /// 重置改变的字段属性
        /// </summary>
        private void ResetChangePropertys()
        {
            while (_changePropertys.Count > 0)
            {
                string column;
                _changePropertys.TryDequeue(out column);
            }
        }

        /// <summary>
        /// 完成更新处理
        /// </summary>
        private void CompleteUpdate()
        {
            _waitUpdateList.Clear();
        }

        /// <summary>
        /// 取出改变的字段属性
        /// </summary>
        /// <returns></returns>
        internal string[] DequeueChangePropertys()
        {
            while (_changePropertys.Count > 0)
            {
                string column;
                if (_changePropertys.TryDequeue(out column) && !_waitUpdateList.Contains(column))
                {
                    _waitUpdateList.Add(column);
                }
            }
            return GetWaitUpdateList();
        }

        /// <summary>
        /// 获取等待更新的属性列表
        /// </summary>
        internal string[] GetWaitUpdateList()
        {
            string[] columns = new string[_waitUpdateList.Count];
            _waitUpdateList.CopyTo(columns, 0);
            return columns;
        }

        /// <summary>
        /// 获取改变的字段属性
        /// </summary>
        /// <returns></returns>
        internal string[] GetChangePropertys()
        {
            string[] columns = new string[_changePropertys.Count];
            _changePropertys.CopyTo(columns, 0);
            return columns;
        }

        /// <summary>
        /// 是否有改变的字段列属性
        /// </summary>
        [JsonIgnore]
        public bool HasChangePropertys
        {
            get { return _changePropertys.Count > 0; }
        }
        /// <summary>
        /// 转换自定对象类型，并绑定集合字段信息
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="propertyName"></param>
        internal protected T ConvertCustomField<T>(object fieldValue, string propertyName) where T : IItemChangeEvent, new()
        {
            if (fieldValue == null)
            {
                return new T();
            }
            if (fieldValue is T)
            {
                var temp = (T)fieldValue;
                temp.PropertyName = propertyName;
                if (IsReadOnly)
                {
                    temp.DisableChildNotify();
                }
                AddChildrenListener(temp);
                return temp;
            }
            throw new Exception(string.Format("Conver column:\"{0}\" object to \"{1}\" error.", propertyName, typeof(T).FullName));
        }

        /// <summary>
        /// 移除过期的键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveExpired(string key)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual int CompareTo(AbstractEntity other)
        {
            if (this == null && other == null) return 0;
            if (this == null && other != null) return -1;
            if (this != null && other == null) return 1;
            int result = -1;
            SchemaTable schema = GetSchema();
            if (schema != null)
            {
                //升序
                foreach (var key in schema.Keys)
                {
                    object x = this.GetPropertyValue(key);
                    object y = other.GetPropertyValue(key);
                    if (x is Enum || x is int || x is short || x is byte || x is bool)
                    {
                        result = (x.ToInt()).CompareTo(y.ToInt());
                    }
                    else if (x is string)
                    {
                        result = x.ToNotNullString().CompareTo(y.ToNotNullString());
                    }
                    else if (x is decimal)
                    {
                        var diff = (decimal)x - (decimal)y;
                        result = diff > 0 ? 1 : diff < 0 ? -1 : 0;
                    }
                    else if (x is double)
                    {
                        var diff = (double)x - (double)y;
                        result = diff > 0 ? 1 : diff < 0 ? -1 : 0;
                    }
                    else if (x is long)
                    {
                        var diff = (long)x - (long)y;
                        result = diff > 0 ? 1 : diff < 0 ? -1 : 0;
                    }
                    else
                    {
                        result = x.ToNotNullString().CompareTo(y.ToNotNullString());
                    }
                    if (result != 0)
                    {
                        break;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Set key from keycode
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="typeName"></param>
        internal void SetKeyValue(string keyCode, string typeName)
        {
            SchemaTable schemaTable;
            if (EntitySchemaSet.TryGet(typeName, out schemaTable))
            {
                string[] keyValues = keyCode.Split(KeyCodeJoinChar);
                for (int i = 0; i < schemaTable.Keys.Length; i++)
                {
                    string columnName = schemaTable.Keys[i];
                    var colAttr = schemaTable[columnName];
                    if (i < keyValues.Length && colAttr != null)
                    {
                        object value = ParseValueType(keyValues[i], colAttr.ColumnType);
                        SetPropertyValue(columnName, value);
                    }
                }
            }
        }

        internal object ParseValueType(object value, Type columnType)
        {
            if (columnType == typeof(int))
            {
                return value.ToInt();
            }
            if (columnType == typeof(string))
            {
                return value.ToNotNullString();
            }
            if (columnType == typeof(decimal))
            {
                return value.ToDecimal();
            }
            if (columnType == typeof(double))
            {
                return value.ToDouble();
            }
            if (columnType == typeof(bool))
            {
                return value.ToBool();
            }
            if (columnType == typeof(byte))
            {
                return value.ToByte();
            }
            if (columnType == typeof(DateTime))
            {
                return value.ToDateTime();
            }
            if (columnType == typeof(Guid))
            {
                return (Guid)value;
            }
            if (columnType.IsEnum)
            {
                return value.ToEnum(columnType);
            }
            return value;
        }
    }
}