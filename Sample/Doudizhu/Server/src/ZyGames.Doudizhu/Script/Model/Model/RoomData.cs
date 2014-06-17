using System;
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 房间对象
    /// </summary>
    [Serializable, ProtoContract]
    public class RoomData : MemoryEntity
    {
        private CacheQueue<TableData> _tablePool;
        private CacheDictionary<int, TableData> _tables;
        private VersionConfig _tableVersion;

        public RoomData()
        {
            _tableVersion = new VersionConfig();
            _tables = new CacheDictionary<int, TableData>();
            _tablePool = new CacheQueue<TableData>();
        }

        private int _roomId;

        /// <summary>
        /// 房间编号
        /// </summary>
        public int RoomId
        {
            get { return _roomId; }
            set { _roomId = value; }
        }

        /// <summary>
        /// 玩家人数
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 获得新的桌号
        /// </summary>
        public int NewTableId
        {
            get { return _tableVersion.NextId; }
        }

        /// <summary>
        /// 空桌数据池
        /// </summary>
        public CacheQueue<TableData> TablePool
        {
            get { return _tablePool; }
        }

        /// <summary>
        /// 在线桌子数据
        /// </summary>
        public CacheDictionary<int, TableData> Tables
        {
            get { return _tables; }
        }

    }
}
