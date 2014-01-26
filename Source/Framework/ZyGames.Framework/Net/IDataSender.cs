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
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Net
{
    /// <summary>
    /// 实体事件
    /// </summary>
    public class EntityEvent : EventArgs
    {
        /// <summary>
        /// 绑定的实体对象
        /// </summary>
        public AbstractEntity Data { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字体值
        /// </summary>
        public object FieldValue { get; set; }

    }
    /// <summary>
    /// 数据处理句柄,跨服战时，可重新设置KEY主键规则
    /// </summary>
    public delegate object EntityBeforeProcess(EntityEvent entityEvent);

    /// <summary>
    /// 数据传送操作接口
    /// </summary>
    public interface IDataSender : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="isChange"></param>
        void Send<T>(T data, bool isChange = true) where T : AbstractEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        void Send<T>(T[] dataList) where T : AbstractEntity;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="isChange"></param>
        void Send<T>(T[] dataList, bool isChange) where T : AbstractEntity;

        /// <summary>
        /// 发送数据到Redis和Db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="isChange">是否只发送发生变化的实体数据</param>
        /// <param name="connectKey">数据库连接字符串的连接键值</param>
        /// <param name="handle">发送之前处理委托方法</param>
        void Send<T>(T[] dataList, bool isChange, string connectKey, EntityBeforeProcess handle) where T : AbstractEntity;
    }
}