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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Rank
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRanking
    {
        /// <summary>
        /// 
        /// </summary>
        string Key { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Refresh();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator GetEnumerator();
    }

    /// <summary>
    /// 排行榜基类
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class Ranking<T> : IRanking where T : RankingItem
    {
        private readonly int _top;
        private readonly int _intervalTimes;
        private List<T> _list;
        private int _rankCount;
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="key">Key.</param>
        protected Ranking(string key)
            : this(key, 0, 0)
        {
        }
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="top">Top.</param>
        protected Ranking(string key, int top)
            : this(key, top, 0)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="top"></param>
        /// <param name="intervalTimes">刷新间隔</param>
        protected Ranking(string key, int top, int intervalTimes)
        {
            Key = key;
            _top = top;
            _intervalTimes = intervalTimes;
            if (_top > 0 && _top != int.MaxValue)
            {
                _list = new List<T>(_top);
            }
            else
            {
                _list = new List<T>();
            }
        }

        /// <summary>
        /// 排名数
        /// </summary>
        public int RankCount
        {
            get { return _rankCount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected abstract int ComparerTo(T x, T y);

        /// <summary>
        /// 排行榜键
        /// </summary>
        public string Key
        {
            get;
            private set;
        }

        private int _refreshTimes;
        /// <summary>
        /// 刷新排行榜
        /// </summary>
        /// <returns></returns>
        public virtual bool Refresh()
        {
            _refreshTimes++;
            if (_refreshTimes >= _intervalTimes)
            {
                _refreshTimes = 0;
                OnUpdateCache();
                return InitializeData();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnUpdateCache()
        {
        }

        /// <summary>
        /// 枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// 初始化数据源
        /// </summary>
        /// <returns></returns>
        protected bool InitializeData()
        {
            try
            {
                lock (_list)
                {
                    var list = new List<T>();

                    IList<T> cacheList = GetCacheList();
                    foreach (var item in cacheList)
                    {
                        if (!IgnoreItem(item))
                        {
                            list.InsertSort(item, ComparerTo);
                        }
                    }
                    _list = DoInitializeAfter(list);
                    _rankCount = _list.Count;
                    TraceLog.ReleaseWrite("The {0} has be loaded, count:{1}", Key, _rankCount);
                    return true;
                }
            }
            catch (Exception ex)
            {
                TraceLog.ReleaseWrite("The {0} has be loaded error:{1}", Key, ex);
                return false;
            }
        }

        /// <summary>
        /// 初始化之后处理
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected virtual List<T> DoInitializeAfter(List<T> list)
        {
            RefleshRankId(list);
            int pageCount;
            return list.GetPaging(1, _top, out pageCount);
        }

        /// <summary>
        /// 刷新排行
        /// </summary>
        protected void RefleshRankId(List<T> list)
        {
            var e = list.GetEnumerator();
            int rankId = 1;
            while (e.MoveNext())
            {
                var item = e.Current;
                if (item != null && item.RankId != rankId)
                {
                    lock (item)
                    {
                        item.RankId = rankId;
                        ChangeRankNo(item);
                    }
                }
                rankId++;
            }
        }

        /// <summary>
        /// 设置排行ID
        /// </summary>
        /// <param name="item"></param>
        protected virtual void ChangeRankNo(T item)
        {

        }

        /// <summary>
        /// 忽略的数据项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual bool IgnoreItem(T item)
        {
            return false;
        }

        /// <summary>
        /// 获取排行数据
        /// </summary>
        /// <returns></returns>
        protected abstract IList<T> GetCacheList();

        /// <summary>
        /// 获取在排行中第几名
        /// </summary>
        /// <returns>-1:不在排行榜中</returns>
        public int GetRankNo(Predicate<T> match)
        {
            int rankId;
            TryGetRankNo(match, out rankId);
            return rankId;
        }

        /// <summary>
        /// 获取排名
        /// </summary>
        /// <param name="match"></param>
        /// <param name="rankId">排名ID</param>
        /// <returns></returns>
        public bool TryGetRankNo(Predicate<T> match, out int rankId)
        {
            rankId = -1;
            var item = _list.Find(match);
            if (item == null)
            {
                return false;
            }
            rankId = item.RankId;
            return true;
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Find(Predicate<T> match)
        {
            return _list.Find(match);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCout"></param>
        /// <returns></returns>
        public IList<T> GetRange(int pageIndex, int pageSize, out int pageCout)
        {
            return _list.GetPaging(pageIndex, pageSize, out pageCout);
        }

        /// <summary>
        /// 调整排名
        /// </summary>
        /// <param name="fromRankId">原始排名</param>
        /// <param name="toRankId">移动到新排名</param>
        public bool TryMove(int fromRankId, int toRankId)
        {
            lock (_list)
            {
                var from = fromRankId - 1;
                var to = toRankId - 1;
                if (from < 0 || from >= _list.Count ||
                    to < 0 || to >= _list.Count)
                {
                    return false;
                }
                var tempItem = _list[to];
                var item = _list[from];
                int index = item.RankId;
                item.RankId = tempItem.RankId;
                ChangeRankNo(item);
                tempItem.RankId = index;
                ChangeRankNo(tempItem);

                _list[to] = item;
                _list[from] = tempItem;
                return true;
            }
        }

        /// <summary>
        /// 增加到排行榜
        /// </summary>
        /// <returns></returns>
        public bool TryAppend(T item)
        {
            lock (_list)
            {
                Interlocked.Increment(ref _rankCount);
                item.RankId = _rankCount;
                ChangeRankNo(item);
                _list.Add(item);
                return true;
            }
        }
        /// <summary>
        /// 插入到排行榜
        /// </summary>
        /// <returns></returns>
        public bool TryInsert(T item)
        {
            lock (_list)
            {
                Interlocked.Increment(ref _rankCount);
                _list.InsertSort(item, ComparerTo);
                RefleshRankId(_list);
                return true;
            }
        }
    }
}