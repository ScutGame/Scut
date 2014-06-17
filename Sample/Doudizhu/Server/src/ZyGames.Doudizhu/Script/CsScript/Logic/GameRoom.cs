using System;
using System.Collections.Generic;
using System.Linq;
using ZyGames.Doudizhu.Bll.Com.Chat;
using ZyGames.Doudizhu.Model;
using ZyGames.Doudizhu.Script.CsScript.Action;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Doudizhu.Bll.Logic
{
    /// <summary>
    /// 游戏房间
    /// </summary>
    public class GameRoom
    {
        private static GameRoom _instance;

        static GameRoom()
        {
            _instance = new GameRoom();
            var roomStruct = new MemoryCacheStruct<RoomData>();
            var roomList = new ShareCacheStruct<RoomInfo>().FindAll(false);
            foreach (var roomInfo in roomList)
            {
                var roomData = new RoomData() { RoomId = roomInfo.Id };
                string key = roomData.RoomId.ToString();
                roomStruct.TryAdd(key, roomData);
            }
        }

        /// <summary>
        /// 当前游戏房间
        /// </summary>
        public static GameRoom Current
        {
            get { return _instance; }
        }

        private MemoryCacheStruct<RoomData> _roomStruct;
        private ShareCacheStruct<RoomInfo> _roomConfig;

        /// <summary>
        /// 房间
        /// </summary>
        private GameRoom()
        {
            _roomStruct = new MemoryCacheStruct<RoomData>();
            _roomConfig = new ShareCacheStruct<RoomInfo>();
        }

        /// <summary>
        /// 房间列表
        /// </summary>
        /// <returns></returns>
        public List<RoomInfo> RoomList
        {
            get { return _roomConfig.FindAll(); }
        }

        public RoomInfo GetRoom(int roomId)
        {
            return _roomConfig.FindKey(roomId);
        }

        public string Tip(string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        public void Enter(GameUser user, RoomInfo roomInfo)
        {
            string key = roomInfo.Id.ToString();
            RoomData roomData;
            if (_roomStruct.TryGet(key, out roomData))
            {
                var cacheSet = new GameDataCacheSet<GameUser>();
                var userTemp = cacheSet.FindKey(user.PersonalId, user.UserId);
                var list = cacheSet.FindGlobal(t => true);


                var tableData = GameTable.Current.SelectTable(userTemp, roomData, roomInfo);
                if (tableData != null && tableData.Positions.Length > 0)
                {
                    GameTable.Current.SyncNotifyAction(ActionIDDefine.Cst_Action2003, tableData, null,
                        c =>
                        {
                            GameTable.Current.CheckStart(tableData);
                        });
                }
            }
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Exit(GameUser user)
        {
            if (user.Property.RoomId == 0)
            {
                return true;
            }
            string key = user.Property.RoomId.ToString();
            RoomData roomData;
            if (_roomStruct.TryGet(key, out roomData))
            {
                TableData table;
                if (roomData.Tables.TryGetValue(user.Property.TableId, out table))
                {
                    GameTable.Current.ExitTablePosition(user, table);
                    user.Property.RoomId = 0;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取房间对象数据
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public RoomData GetRoomData(int roomId)
        {
            string key = roomId.ToString();
            RoomData roomData;
            if (_roomStruct.TryGet(key, out roomData))
            {
                return roomData;
            }
            return null;
        }

        public TableData GetTableDataByUserId(int userId)
        {
            var cacheSet = new GameDataCacheSet<GameUser>();
            var user = cacheSet.FindKey(userId.ToString(), userId);
            return GetTableData(user);
        }


        /// <summary>
        /// 获取桌面对象数据Pyton中调用
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public TableData GetTableData(GameUser user)
        {
            TableData tableData = GetTableData(user.Property.RoomId, user.Property.TableId);
            if (tableData == null)
            {
                //Console.WriteLine("GetTableData error,uid:{0},Room:{1},TableId:{2}", user.UserId, user.Property.RoomId, user.Property.TableId);
            }
            //if (tableData == null)
            //{
            //    var param = new Parameters();
            //    param.Add("FleeUserId", 0);
            //    var list = new List<GameUser>();
            //    list.Add(user);
            //    GameTable.Current.SyncNotifyAction(ActionIDDefine.Cst_Action2013, list, param, null);
            //}
            return tableData;
        }

        public TableData GetTableData(int roomId, int tableId)
        {
            RoomData roomData = GetRoomData(roomId);
            if (roomData != null)
            {
                TableData table;
                if (roomData.Tables.TryGetValue(tableId, out table))
                {
                    return table;
                }
            }
            return null;
        }

        /// <summary>
        /// 检查赠送金豆
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roomInfo"></param>
        public bool CheckDailyGiffCoin(GameUser user, RoomInfo roomInfo)
        {
            UserDailyRestrain restrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(user.PersonalId);
            if (restrain != null)
            {
                RefreshRestrain(restrain);
                int dailyGiffCoinTime = ConfigEnvSet.GetInt("User.DailyGiffCoinTime", 1);
                if (restrain.RestrainProperty.DailyGiffCoinTime < dailyGiffCoinTime)
                {
                    user.GameCoin = MathUtils.Addition(user.GameCoin, roomInfo.GiffCion);
                    restrain.RestrainProperty.DailyGiffCoinTime = MathUtils.Addition(restrain.RestrainProperty.DailyGiffCoinTime, 1);

                    return true;
                }
            }
            return false;
        }

        public static void RefreshRestrain(UserDailyRestrain restrain)
        {
            if (restrain.RefreshDate.Date != DateTime.Now.Date)
            {
                restrain.RestrainProperty.Init();
                restrain.RefreshDate = DateTime.Now;
            }
        }
    }
}
