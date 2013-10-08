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
using System.Threading;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Game.Runtime
{

    /// <summary>
    /// 游戏数据加载器
    /// </summary>
    public class GameUserDataLoader<T> : DataLoader where T : BaseEntity, new()
    {
        protected readonly bool IsAuto;
        private readonly string _personalId;
        private readonly string _fieldName;
        protected readonly int UserId;
        private readonly int _maxCount;
        private readonly int _periodTime;
        protected readonly GameDataCacheSet<T> CacheSet;
        private readonly bool _igonreKey;

        public GameUserDataLoader(bool isAuto, string personalId, int periodTime)
            : this(isAuto, personalId, null, 0, int.MaxValue, periodTime)
        {
            _igonreKey = true;
        }

        public GameUserDataLoader(bool isAuto, string fieldName, int userId, int maxCount, int periodTime)
            : this(isAuto, userId.ToString(), fieldName, userId, maxCount, periodTime)
        {
        }

        public GameUserDataLoader(bool isAuto, string personalId, string fieldName, int userId, int maxCount, int periodTime)
        {
            _igonreKey = false;
            IsAuto = isAuto;
            _personalId = personalId;
            _fieldName = fieldName;
            UserId = userId;
            _maxCount = maxCount;
            _periodTime = periodTime;
            CacheSet = new GameDataCacheSet<T>();
        }

        public virtual int UpdateWaitTime
        {
            get { return 1000; }
        }

        public override bool Load()
        {
            //if (_igonreKey)
            //{
            //    return IsEmpty() && CacheSet.AutoLoad(_personalId, _fieldName, _maxCount, _periodTime);
            //}
            //if (!IsAuto || !IsExistData())
            //{
            //    return CacheSet.AutoLoad(_personalId, _fieldName, _maxCount, _periodTime);
            //}
            return true;
        }

        public override string LoadTypeName
        {
            get { return typeof(T).FullName; }
        }

        protected virtual bool IsEmpty()
        {
            return !IsAuto || CacheSet.Count == 0;
        }

        protected override bool IsExistData()
        {
            return CacheSet.IsExistData(_personalId);
        }
    }
}