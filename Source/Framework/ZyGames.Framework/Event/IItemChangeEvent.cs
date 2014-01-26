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

namespace ZyGames.Framework.Event
{
    /// <summary>
    /// 变更事件接口
    /// </summary>
    public abstract class IItemChangeEvent : IDisposable
    {
        /// <summary>
        /// 是否有变更
        /// </summary>
        public abstract bool HasChanged { get; }

        /// <summary>
        /// 绑定实体类的属性名（表的列名）
        /// </summary>
        internal abstract string PropertyName { get; set; }

        /// <summary>
        /// 当前对象变更事件对象
        /// </summary>
        internal abstract CacheItemChangeEvent ItemEvent { get; }

        /// <summary>
        /// 当前对象的子类变更事件对象
        /// </summary>
        internal abstract CacheItemChangeEvent ChildrenEvent { get; }
        
        /// <summary>
        /// 禁用子类事件通知
        /// </summary>
        internal abstract void DisableChildNotify();
        /// <summary>
        /// 增加子类监听
        /// </summary>
        /// <param name="changeEvent"></param>
        public abstract void AddChildrenListener(object changeEvent);

        /// <summary>
        /// 移除子类监听
        /// </summary>
        /// <param name="changeEvent"></param>
        internal abstract void RemoveChildrenListener(object changeEvent);

        /// <summary>
        /// 解除变更事件通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        internal abstract void UnChangeNotify(object sender, CacheItemEventArgs eventArgs);

        /// <summary>
        /// 对象更新时通知事件
        /// </summary>
        /// <param name="updateHandle"></param>
        public abstract void UpdateNotify(Func<IItemChangeEvent, bool> updateHandle);

        /// <summary>
        /// 序列化Json
        /// </summary>
        /// <returns></returns>
        public abstract string ToJson();

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //释放非托管资源 
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}