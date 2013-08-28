using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// 游戏配置数据加载器
    /// </summary>
    public class GameConfigLoader<T> : DataLoader where T : ShareEntity, new()
    {
        private readonly bool _isAuto;
        private readonly ConfigCacheSet<T> _cacheSet;
        private const int MaxCount = int.MaxValue;

        public GameConfigLoader()
            : this(true)
        {
        }

        public GameConfigLoader(bool isAuto)
        {
            _cacheSet = new ConfigCacheSet<T>();
            _isAuto = isAuto;
        }

        public override string LoadTypeName
        {
            get { return typeof(T).FullName; }
        }

        protected override bool IsExistData()
        {
            return _cacheSet.Count > 0;
        }

        public override bool Load()
        {
            if (!_isAuto || !IsExistData())
            {
                return new ConfigCacheSet<T>().AutoLoad(new DbDataFilter(MaxCount));
            }
            return true;
        }
    }
}
