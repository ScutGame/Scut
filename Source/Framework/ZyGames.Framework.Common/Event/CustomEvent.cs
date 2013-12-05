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

namespace ZyGames.Framework.Common.Event
{
    /// <summary>
    /// 自定义事件处理委托
    /// </summary>
    /// <typeparam name="T">类型EventArgs的子类</typeparam>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void CustomEventHandle<T>(object sender, T args) where T : EventArgs;

    /// <summary>
    /// 自定的事模型
    /// </summary>
    /// <typeparam name="T">类型EventArgs的子类</typeparam>
    public class CustomEvent<T> : BaseDisposable where T : EventArgs
    {
        private event CustomEventHandle<T> EventHandle;

        /// <summary>
        /// 注册单一事件
        /// </summary>
        /// <param name="handle"></param>
        public void AddSingle(CustomEventHandle<T> handle)
        {
            if (EventHandle != null)
            {
                EventHandle -= handle;
            }
            EventHandle += handle;
        }

        /// <summary>
        /// 增加事件
        /// </summary>
        /// <param name="handle"></param>
        public void Add(CustomEventHandle<T> handle)
        {
            EventHandle += handle;
        }
        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="handle"></param>
        public void Remove(CustomEventHandle<T> handle)
        {
            EventHandle -= handle;
        }
        /// <summary>
        /// 事件通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Notify(object sender, T args)
        {
            if (EventHandle != null)
            {
                EventHandle(sender, args);
            }
        }
        /// <summary>
        /// 事件通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void NotifyAll(object sender, T args)
        {
            if (EventHandle == null)
            {
                return;
            }
            var tempList = EventHandle.GetInvocationList();
            foreach (var item in tempList)
            {
                if (item != null)
                {
                    item.DynamicInvoke(sender, args);
                }
            }
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EventHandle = null;
            }
            base.Dispose(disposing);
        }
    }
}