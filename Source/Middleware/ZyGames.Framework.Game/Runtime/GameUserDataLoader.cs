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
