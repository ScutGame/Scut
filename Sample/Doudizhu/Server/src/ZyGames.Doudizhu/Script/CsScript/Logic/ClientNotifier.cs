using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Net;

namespace ZyGames.Doudizhu.Bll.Logic
{
    public delegate void NotifyActionHandle(int actionId, List<GameUser> userList, Parameters param, Action<int> callback);
    /// <summary>
    /// 客户端通知管理
    /// </summary>
    public class ClientNotifier
    {
        private static readonly object SyncRoot = new object();
        private static event NotifyActionHandle NotifyHandle;

        public static void NotifyOnlineUserAction(int actionId, Parameters parameters, Action<int> callback = null)
        {
            var list = new List<GameUser>();
            new GameDataCacheSet<GameUser>().Foreach((pid, userId, user) =>
            {
                if (user.OnlineDate > DateTime.MinValue)
                {
                    list.Add(user);
                }
                return true;
            });
            NotifyAction(actionId, list, parameters, callback);
        }
        /// <summary>
        /// 触发通知 --单个玩家
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="user"></param>
        /// <param name="parameters"></param>
        /// <param name="callback"></param>
        public static void NotifyAction(int actionId, GameUser user, Parameters parameters, Action<int> callback = null)
        {
            var list = new List<GameUser>();
            list.Add(user);
            NotifyAction(actionId, list, parameters, callback);
        }

        /// <summary>
        /// 触发通知
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="userList"></param>
        /// <param name="parameters"></param>
        /// <param name="callback"></param>
        public static void NotifyAction(int actionId, List<GameUser> userList, Parameters parameters, Action<int> callback)
        {
            if (NotifyHandle == null)
            {
                lock (SyncRoot)
                {
                    if (NotifyHandle == null)
                    {
                        NotifyHandle += new NotifyActionHandle(DoNotifyAction);
                    }
                }
            }
            //NotifyHandle(actionId, userList, parameters, callback);
            NotifyHandle.BeginInvoke(actionId, userList, parameters, callback, null, null);
        }

        private static void DoNotifyAction(int actionId, List<GameUser> userList, Parameters parameters, Action<int> callback)
        {
            //string str = "";
            //foreach (GameUser gameUser in userList)
            //{
            //    str += "," + gameUser.UserId;
            //}
            //str = str.TrimStart(',');
            //Console.WriteLine("Notify action:{0} {1}", actionId, str);

            ActionFactory.SendAsyncAction(userList, actionId, parameters, g => { });
            if (callback != null)
            {
                callback(actionId);
            }
        }

        /// <summary>
        /// 通知玩家，机器人不通知
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="ignoreUserId"></param>
        /// <param name="ignoreAI"></param>
        /// <returns></returns>
        public static List<GameUser> GetUserList(TableData tableData, int ignoreUserId = 0, bool ignoreAI = true)
        {
            var userList = new List<GameUser>();
            var cacheSet = new GameDataCacheSet<GameUser>();
            foreach (var pos in tableData.Positions)
            {
                if (ignoreAI && pos.IsAI && pos.UserId < 1000)
                {
                    continue;
                }
                GameUser user = cacheSet.FindKey(pos.UserId.ToString());
                if (user != null && user.UserId != ignoreUserId)
                {
                    userList.Add(user);
                }
            }
            return userList;
        }

    }
}
