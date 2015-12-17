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
using ZyGames.Framework.Profile;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// 实体数据基类
    /// </summary>
    [ProtoContract, Serializable]
    public abstract class AbstractEntity : EntityChangeEvent, IDataExpired, ISqlEntity, IComparable<AbstractEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        internal protected const char KeyCodeJoinChar = '-';

        /// <summary>
        /// 
        /// </summary>
        protected const int DefIdentityId = 10000;
        /// <summary>
        /// 存储改变的属性集合
        /// </summary>
        private ConcurrentQueue<string> _changePropertys = new ConcurrentQueue<string>();
        /// <summary>
        /// 等待更新属性
        /// </summary>
        private readonly HashSet<string> _waitUpdateList = new HashSet<string>();

        [NonSerialized]
        private SchemaTable _schema;
        [NonSerialized]
        private ObjectAccessor _typeAccessor;

        /// <summary>
        /// Conver '-' char
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static string EncodeKeyCode(string keyCode)
        {
            return (keyCode ?? "")
                .Replace("%", "%25")
                .Replace(KeyCodeJoinChar.ToString(), "%45")
                .Replace("_", "%46")
                .Replace("|", "%7C");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static string DecodeKeyCode(string keyCode)
        {
            return (keyCode ?? "")
                .Replace("%25", "%")
                .Replace("%45", KeyCodeJoinChar.ToString())
                .Replace("%46", "_")
                .Replace("%7C", "|");
        }

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
        /// <param name="access">Access. no used</param>
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
            IsInCache = false;
            if (EntitySchemaSet.TryGet(GetType(), out _schema))
            {
                _isReadOnly = _schema.AccessLevel == AccessLevel.ReadOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Init()
        {
            _typeAccessor = ObjectAccessor.Create(this, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetState()
        {
            _hasChanged = false;
            OnUnNew();
            ResetChangePropertys();
            CompleteUpdate();
            ExitModify();
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public void Reset()
        {
            ResetState();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetCreateTime()
        {
            return DateTime.Now;
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
        }

        /// <summary>
        /// 设置属性值之后处理
        /// </summary>
        public void SetValueAfter()
        {
            ResetChangePropertys();
            OnUnNew();
            IsInCache = true;
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


        /// <summary>
        /// 判断是否处理加载中设置属性,只读实体为False后才不可修改
        /// </summary>
        [JsonIgnore]
        [Obsolete]
        public bool IsLoading
        {
            get
            {
                return !IsInCache;
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                if (id == 0)
                {
                    TraceLog.WriteError("The {0} property \"PersonalId\" is empty.", _schema.EntityType.FullName);
                }
                return EncodeKeyCode(id.ToString());
            }
        }

        /// <summary>
        /// entity modify time.
        /// </summary>
        [ProtoMember(100025)]
        public DateTime TempTimeModify { get; set; }

        /// <summary>
        /// expire time
        /// </summary>
        [ProtoMember(100026)]
        public DateTime ExpiredTime { get; set; }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetMessageQueueId()
        {
            return GetIdentityId();
        }

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
            if (_isReadOnly || CheckChnage()) return;
            //modify resean: not notify to ItemSet object.
            //eventArgs.Source = sender;
            _hasChanged = true;
            AddChangePropertys(eventArgs.PropertyName);
            //base.Notify(this, eventArgs);
            PutToChangeKeys(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">由IContainer对象触发</param>
        /// <param name="eventArgs"></param>
        protected override void NotifyByChildren(object sender, CacheItemEventArgs eventArgs)
        {
            if (_isReadOnly || CheckChnage()) return;
            //eventArgs.Source = sender;
            _hasChanged = true;
            AddChangePropertys(eventArgs.PropertyName);
            //更改子类事件触发者
            //base.NotifyByChildren(this, eventArgs);
            PutToChangeKeys(this);
        }

        private bool CheckChnage()
        {
            //不在内存时不触发事件
            if (!IsInCache)
            {
                return true;
            }
            if (IsExpired)
            {
                TraceLog.WriteError("Not found entity {0} key {1} in cache, it is expired.\r\n{2}", GetSchema().EntityName, GetKeyCode(), TraceLog.GetStackTrace());
                return true;
            }
            return false;
        }
        private void PutToChangeKeys(AbstractEntity entity)
        {
            if (!IsModifying)
            {
                //Auto trigger modify event
                ProfileManager.ChangeEntityByAutoOfMessageQueueTimes(entity.GetType().FullName, entity.GetKeyCode());
                DataSyncQueueManager.Send(entity);
            }
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
                    TraceLog.WriteError("The {0} get property:{1}\r\n{2}", _schema.EntityName, property, ex);
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
                    TraceLog.WriteError("The {0} set property:{1} value:{2}\r\n{3}", _schema.EntityName, property, value, ex);
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
                    value += KeyCodeJoinChar;
                }
                value += EncodeKeyCode(key.ToNotNullString());
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
            foreach (string key in _schema.Keys)
            {
                if (value.Length > 0)
                {
                    value += KeyCodeJoinChar;
                }
                value += EncodeKeyCode(GetPropertyValue(key).ToNotNullString());
            }
            if (string.IsNullOrEmpty(value))
            {
                TraceLog.WriteError("Entity {0} primary key is empty.", _schema.EntityName);
            }

            return value;
        }

        /// <summary>
        /// 获得实体DB架构信息
        /// </summary>
        /// <returns>返回值会为Null</returns>
        public SchemaTable GetSchema()
        {
            return _schema;
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
            if (val != null) val.IsInCache = true;
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
                    throw new Exception("Get property:" + _schema.EntityName + "." + propertyName, ex);
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
            if (val != null) val.IsInCache = true;
        }

        private void AddChangePropertys(string propertyName)
        {
            //在加载初始数据时，不设置Change属性
            if (!string.IsNullOrEmpty(propertyName))
            {
                _changePropertys.Enqueue(propertyName);
            }
        }

        /// <summary>
        /// 重置改变的字段属性
        /// </summary>
        internal void ResetChangePropertys()
        {
            string r;
            while (_changePropertys.TryDequeue(out r))
            {
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
                fieldValue = new T();
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
                temp.IsInCache = true;
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
                    if (x is Enum || x is int || x is short || x is byte || x is bool || x is uint || x is ushort)
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
                    else if (x is long || x is ulong)
                    {
                        var diff = (long)x - (long)y;
                        result = diff > 0 ? 1 : diff < 0 ? -1 : 0;
                    }
                    else if (x is float)
                    {
                        var diff = (float)x - (float)y;
                        result = diff > 0 ? 1 : diff < 0 ? -1 : 0;
                    }
                    else if (x is DateTime)
                    {
                        result = DateTime.Compare((DateTime)x, (DateTime)y);
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
                        string key = DecodeKeyCode(keyValues[i]);
                        object value = ParseValueType(key, colAttr.ColumnType);
                        SetPropertyValue(columnName, value);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static object ParseValueType(object value, Type columnType)
        {
            if (columnType == typeof(Int64))
            {
                return value.ToLong();
            }
            if (columnType == typeof(Int32))
            {
                return value.ToInt();
            }
            if (columnType == typeof(Int16))
            {
                return value.ToShort();
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
            if (columnType == typeof(float))
            {
                return value.ToFloat();
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
                return value is Guid ? (Guid)value : Guid.Parse(value.ToString());
            }
            if (columnType.IsEnum)
            {
                return value.ToEnum(columnType);
            }
            if (columnType == typeof(UInt64))
            {
                return value.ToUInt64();
            }
            if (columnType == typeof(UInt32))
            {
                return value.ToUInt32();
            }
            if (columnType == typeof(UInt16))
            {
                return value.ToUInt16();
            }
            return value;
        }
    }
}