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
using ZyGames.Framework.Event;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Cache.Generic
{
    /// <summary>
    /// 缓存数据容器
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IDataContainer<T> : IDisposable where T : IItemChangeEvent, IDataExpired, new()
    {
        /// <summary>
        /// 是否只读容器
        /// </summary>
        bool IsReadonly { get; set; }

        /// <summary>
        /// 缓存池的键，缓存的根部
        /// </summary>
        string RootKey { get; }

        /// <summary>
        /// 缓存数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 容器是否为空
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// 
        /// </summary>
        LoadingStatus LoadStatus { get; }
        /// <summary>
        /// 
        /// </summary>
        CacheItemSet[] ChildrenItem { get;}
        ///// <summary>
        ///// 获取容器对象
        ///// </summary>
        //BaseCollection Container { get; }

        /// <summary>
        /// 数据是否改变
        /// </summary>
        bool HasChange(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemSet"></param>
        /// <returns></returns>
        bool TryGetCache(string key, out CacheItemSet itemSet);

        /// <summary>
        /// 遍历取
        /// </summary>
        T TakeEntityFromKey(string key);

        /// <summary>
        /// 遍历实体
        /// </summary>
        /// <param name="func">遍历项委托方法，返回值为:false结束遍历</param>
        void ForeachEntity(Func<string, T, bool> func);


        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="func">遍历项委托方法，第一个参数为分组Key,第二个为实体Key，返回值为:false结束遍历</param>
        void ForeachGroup(Func<string, string, T, bool> func);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        void ForeachQueue(Func<string, T, bool> func);
        /// <summary>
        /// 取数据以List方式
        /// </summary>
        List<KeyValuePair<string, CacheItemSet>> ToList();

        /// <summary>
        /// 判断条件是否存在数据
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        bool IsExistEntity(Predicate<T> match);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        bool IsExistGroup(Predicate<T> match);

        /// <summary>
        /// 重新加载全部数据
        /// </summary>
        LoadingStatus ReLoad();

        /// <summary>
        /// 加载全部数据
        /// </summary>
        LoadingStatus Load();

        /// <summary>
        /// 
        /// </summary>
        List<V> LoadFrom<V>(TransReceiveParam receiveParam) where V : AbstractEntity, new();

        /// <summary>
        /// 加载指定Key数据
        /// </summary>
        bool LoadItem(string key);

        /// <summary>
        /// 重新加载指定Key数据
        /// </summary>
        bool ReLoadItem(string key);

        /// <summary>
        /// 尝试取实体
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entityData"></param>
        /// <returns></returns>
        bool TryGetEntity(string key, out T entityData);

        /// <summary>
        /// 尝试增加实体
        /// </summary>
        bool TryAddEntity(string key, T entityData, int periodTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityKey"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        bool AddOrUpdateEntity(string entityKey, T entityData, int periodTime);

        /// <summary>
        /// 尝试取实体分组
        /// </summary>
        bool TryGetGroup(string groupKey, out BaseCollection enityGroup, out LoadingStatus loadStatus);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="key"></param>
        /// <param name="entityData"></param>
        /// <param name="periodTime"></param>
        bool AddOrUpdateGroup(string groupKey, string key, T entityData, int periodTime);

        /// <summary>
        /// 尝试增加实体分组
        /// </summary>
        bool TryAddGroup(string groupKey, string key, T entityData, int periodTime);

        /// <summary>
        /// 初始化容器
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="periodTime"></param>
        /// <returns></returns>
        CacheItemSet InitGroupContainer(string groupKey, int periodTime);
        /// <summary>
        /// 尝试取实体队列
        /// </summary>
        bool TryGetQueue(string groupKey, out CacheQueue<T> enityGroup);

        /// <summary>
        /// 尝试增加实体队列
        /// </summary>
        bool TryAddQueue(string groupKey, T entityData, int periodTime, Func<string, CacheQueue<T>, bool> expiredHandle);

        /// <summary>
        /// 尝试移除
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="callback">移除成功回调</param>
        /// <returns></returns>
        bool TryRemove(string groupKey, Func<CacheItemSet, bool> callback);
        /// <summary>
        /// 尝试移除
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="key"></param>
        /// <param name="callback">移除成功回调</param>
        /// <returns></returns>
        bool TryRemove(string groupKey, string key, Func<T, bool> callback);

        /// <summary>
        /// 尝试移除
        /// </summary>
        /// <param name="callback">移除成功回调</param>
        /// <returns></returns>
        bool TryRemove(Func<CacheContainer, bool> callback);

        /// <summary>
        /// 尝试加载接收数据
        /// </summary>
        /// <typeparam name="V">实体类型</typeparam>
        /// <param name="receiveParam">接收转发器参数</param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        bool TryReceiveData<V>(TransReceiveParam receiveParam, out List<V> dataList) where V : AbstractEntity, new();

        /// <summary>
        /// 尝试从Redis历史记录数中检索
        /// </summary>
        /// <typeparam name="V">实体类型</typeparam>
        /// <param name="redisKey">redis Key</param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        bool TryLoadHistory<V>(string redisKey, out List<V> dataList) where V : AbstractEntity, new();
        /// <summary>
        /// 传送数据
        /// </summary>
        /// <typeparam name="V">实体类型</typeparam>
        /// <param name="dataList"></param>
        /// <param name="sendParam">输送转发器参数</param>
        void SendData<V>(V[] dataList, TransSendParam sendParam) where V : AbstractEntity, new();

        /// <summary>
        /// 获取变更的实体数据
        /// </summary>
        /// <param name="changeKey"></param>
        /// <param name="isChange"></param>
        /// <returns></returns>
        List<KeyValuePair<string, T>> GetChangeEntity(string changeKey, bool isChange);

        /// <summary>
        /// 获取变更的分组数据
        /// </summary>
        /// <param name="changeKey"></param>
        /// <param name="isChange"></param>
        /// <returns></returns>
        List<KeyValuePair<string, T>> GetChangeGroup(string changeKey, bool isChange);

        /// <summary>
        /// 获取变更的队列数据
        /// </summary>
        /// <param name="changeKey"></param>
        /// <param name="isChange">更新到库中是全部数据还是改变的数据</param>
        /// <returns></returns>
        List<KeyValuePair<string, T>> GetChangeQueue(string changeKey, bool isChange);

        /// <summary>
        /// 触发UnChange事件数据通知
        /// </summary>
        /// <param name="key">Group键</param>
        void UnChangeNotify(string key);

        /// <summary>
        /// 尝试从DB中恢复数据
        /// </summary>
        /// <param name="receiveParam"></param>
        /// <param name="dataList"></param>
        bool TryRecoverFromDb<V>(TransReceiveParam receiveParam, out List<V> dataList) where V : AbstractEntity, new();
    }
}