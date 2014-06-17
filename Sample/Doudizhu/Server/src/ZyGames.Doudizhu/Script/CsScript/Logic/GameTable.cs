using System;
using System.Collections.Generic;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Doudizhu.Script.CsScript.Action;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Net;
using ZyGames.Framework.Plugin.PythonScript;
using ZyGames.Framework.Script;

namespace ZyGames.Doudizhu.Bll.Logic
{
    /// <summary>
    /// 游戏桌子管理
    /// </summary>
    public class GameTable
    {
        private const string AIScriptCode = "Logic/cardAILogic.py";
        private static GameTable _instance;
        static GameTable()
        {
            _instance = new GameTable();
        }

        /// <summary>
        /// 当前游戏桌子
        /// </summary>
        public static GameTable Current
        {
            get { return _instance; }
        }

        private CardRole _cardRole;
        private ShareCacheStruct<PokerInfo> _pokerConfig;
        private GameDataCacheSet<GameUser> _userCacheSet;
        private MemoryCacheStruct<RoomData> _roomStruct;

        private GameTable()
        {
            _cardRole = new CardRole();
            _pokerConfig = new ShareCacheStruct<PokerInfo>();
            _userCacheSet = new GameDataCacheSet<GameUser>();
            _roomStruct = new MemoryCacheStruct<RoomData>();
        }

        /// <summary>
        /// 牌数据
        /// </summary>
        public List<PokerInfo> PokerList
        {
            get
            {
                return _pokerConfig.FindAll();
            }
        }

        /// <summary>
        /// 操作倒计时
        /// </summary>
        public int CodeTime
        {
            get
            {
                return ConfigEnvSet.GetInt("User.TableCodeTime", 30);
            }
        }


        private RoomData GetRoomData(int roomId)
        {
            string key = roomId.ToString();
            RoomData roomData;
            if (_roomStruct.TryGet(key, out roomData))
            {
                return roomData;
            }
            return null;
        }

        /// <summary>
        /// 获取玩家座位
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tableData"></param>
        /// <returns></returns>
        public PositionData GetUserPosition(GameUser user, TableData tableData)
        {
            return GetUserPosition(user.Property.PositionId, tableData);
        }

        public PositionData GetUserPosition(int positionId, TableData tableData)
        {
            if (positionId < tableData.Positions.Length)
            {
                PositionData pos = tableData.Positions[positionId];
                return pos;
            }
            return null;
        }

        /// <summary>
        /// 选择桌位,找不到桌子自动新开一个
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roomData"></param>
        /// <param name="roomInfo"></param>
        /// <returns></returns>
        public TableData SelectTable(GameUser user, RoomData roomData, RoomInfo roomInfo)
        {
            TableData useTable = null;
            roomData.Tables.Foreach((key, table) =>
            {
                if (SelectPosition(user, roomData, table))
                {
                    useTable = table;
                    //退出Foreach
                    return false;
                }
                return true;
            });

            if (roomData.TablePool.Count == 0)
            {
                //初始桌数
                int minTableCount = ConfigEnvSet.GetInt("Game.Table.MinTableCount", 10);
                var pokers = PokerList;
                TableData tableData = null;
                for (int i = 0; i < minTableCount; i++)
                {
                    int tableId = roomData.NewTableId;
                    tableData = new TableData(roomData.RoomId,
                        tableId,
                        roomInfo.PlayerNum,
                        roomInfo.AnteNum,
                        roomInfo.MultipleNum,
                        DoTableTimer,
                        roomInfo.CardPackNum);
                    roomData.TablePool.Enqueue(tableData);
                    CreateCardData(tableData, pokers);
                }
            }
            if (useTable == null && roomData.TablePool.TryDequeue(out useTable))
            {
                SetTablePosition(roomData.RoomId, useTable, useTable.Positions[0], user);
                roomData.Tables.Add(useTable.TableId, useTable);
            }
            if (useTable != null && !useTable.IsTimerStarted)
            {
                int periodAiStart = ConfigEnvSet.GetInt("Game.Table.AIIntoTime", 5000);
                useTable.ReStartTimer(periodAiStart);
            }
            return useTable;
        }

        private void DoTableTimer(object state)
        {
            try
            {
                if (state is TableData)
                {
                    int outcardPeroid = ConfigEnvSet.GetInt("Game.Table.AIOutCardTime", 5000);
                    var temp = state as TableData;
                    TableData table;
                    RoomData roomData = GetRoomData(temp.RoomId);
                    if (!roomData.Tables.TryGetValue(temp.TableId, out table))
                    {
                        return;
                    }
                    if (table.IsTimeRunning)
                    {
                        return;
                    }
                    table.IsTimeRunning = true;

                    if (table.IsClosed || table.IsDisposed)
                    {
                        //检查是否出完牌
                        table.StopTimer();
                        table.IsTimeRunning = false;
                        return;
                    }
                    if (!table.IsStarting)
                    {
                        //AI Join
                        dynamic scope = ScriptEngines.Execute(AIScriptCode, null);
                        if (scope != null)
                        {
                            HashSet<string> nickSet = new HashSet<string>();
                            foreach (var pos in table.Positions)
                            {
                                if (pos.UserId == 0)
                                {
                                    int aiId = 100 + pos.Id;
                                    string nickName = "雪舞枫红";
                                    string head = "head_1001";
                                    try
                                    {
                                        var nameList = scope.AIConfig.getConfig("nickName");
                                        if (nameList != null && nameList.Count > 0)
                                        {
                                            for (int i = 0; i < 5; i++)
                                            {
                                                int index = RandomUtils.GetRandom(0, nameList.Count);
                                                nickName = nameList[index];
                                                if (!nickSet.Contains(nickName))
                                                {
                                                    nickSet.Add(nickName);
                                                    break;
                                                }
                                            }
                                        }
                                        var headList = scope.AIConfig.getConfig("head");
                                        if (headList != null && headList.Count > 0)
                                        {
                                            int index = RandomUtils.GetRandom(0, headList.Count);
                                            head = headList[index];
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        TraceLog.WriteError("AI get nickname error:{0}", ex.ToString());
                                        //Console.WriteLine(ex.Message);
                                    }
                                    pos.InitAI(table.RoomId, table.TableId, aiId, nickName, head);
                                }
                            }
                            SyncNotifyAction(ActionIDDefine.Cst_Action2003, table, null,
                                c =>
                                {
                                    CheckStart(table);
                                    int periodFirstCall = ConfigEnvSet.GetInt("Game.Table.AIFirstOutCardTime", 20000);
                                    table.ReStartTimer(periodFirstCall);//发牌等待20秒
                                });
                        }
                    }
                    else if (!table.IsCallEnd)
                    {
                        //叫地主
                        var pos = table.Positions[table.CallLandlordPos];
                        if (pos != null && !pos.IsAI && table.IsOperationTimeout)
                        {
                            //超时自动托管
                            pos.IsAI = true;
                            NotifyAutoAiUser(pos.UserId, true);
                        }
                        if (pos != null && pos.IsAI)
                        {
                            //Console.WriteLine("Table:{0} is ai", table.TableId);

                            dynamic scope = ScriptEngines.Execute(AIScriptCode, null);
                            if (scope != null)
                            {
                                bool iscall = false;
                                try
                                {
                                    var myClass = scope.GetVariable<Func<int, int, int, dynamic>>("CardAILogic");
                                    var myInstance = myClass(table.RoomId, table.TableId, pos.Id);
                                    iscall = (bool)myInstance.checkCall();
                                }
                                catch (Exception e)
                                {
                                    TraceLog.WriteError("桌子:{0}Timer error:{1}", table.TableId, e.ToString());
                                    //Console.WriteLine("Table:{0} is error:{1}", table.TableId, e.ToString());
                                }
                                CallCard(pos.Id, table, iscall);
                                table.ReStartTimer(outcardPeroid);
                            }
                        }
                    }
                    else
                    {
                        //出牌
                        var pos = table.Positions[table.OutCardPos];
                        if (pos != null && !pos.IsAI && table.IsOperationTimeout)
                        {
                            //超时自动托管
                            pos.IsAI = true;
                            NotifyAutoAiUser(pos.UserId, true);
                        }
                        if (pos != null && pos.IsAI && pos.CardData.Count > 0)
                        {
                            try
                            {
                                dynamic scope = ScriptEngines.Execute(AIScriptCode, null);
                                if (scope != null)
                                {
                                    var myClass = scope.GetVariable<Func<int, int, int, dynamic>>("CardAILogic");
                                    var myInstance = myClass(table.RoomId, table.TableId, pos.Id);
                                    var outList = myInstance.searchOutCard();
                                    string cards = string.Empty;
                                    if (outList != null)
                                    {
                                        object[] cardArr = new object[outList.Count];
                                        outList.CopyTo(cardArr, 0);
                                        cards = string.Join(",", cardArr);
                                    }
                                    if (!OutCard(pos.UserId, pos.Id, table, cards))
                                    {
                                        table.StopTimer();
                                        TraceLog.WriteError("桌子:{0}玩家{1}-{2}托管出牌:\"{3}\"出错,上一手出牌:\"{4}\"",
                                            table.TableId,
                                            pos.Id,
                                            pos.NickName,
                                            cards,
                                            (table.PreCardData != null ? string.Join(",", table.PreCardData.Cards) : ""));
                                        OutCard(pos.UserId, pos.Id, table, "");
                                    }
                                    table.ReStartTimer(outcardPeroid);
                                }
                            }
                            catch (Exception e)
                            {
                                TraceLog.WriteError("Table:{0} is error:{1}", table.TableId, e);
                                //Console.WriteLine("Table:{0} is error:{1}", table.TableId, e.ToString());
                                //出错过牌
                                OutCard(pos.UserId, pos.Id, table, "");
                                table.ReStartTimer(outcardPeroid);
                            }
                        }
                    }

                    table.IsTimeRunning = false;
                    table.DoTimeNumber();
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Timer error:{0}", ex.ToString());
            }
        }

        /// <summary>
        /// 出牌和叫地主时重置桌子定时
        /// </summary>
        public void ReStarTableTimer(TableData table)
        {
            int outcardPeroid = ConfigEnvSet.GetInt("Game.Table.AIOutCardTime", 5000);
            table.ReStartTimer(outcardPeroid);
        }

        /// <summary>
        /// 通知金豆数据改变
        /// </summary>
        /// <param name="userId"></param>
        public void NotifyUserChange(int userId)
        {
            GameUser user = GetUser(userId);
            if (user != null)
            {
                SyncNotifyAction(ActionIDDefine.Cst_Action1014, user, null, null);
            }
        }

        /// <summary>
        /// 通知玩家托管
        /// </summary>
        public void NotifyAutoAiUser(int userId, bool status)
        {
            GameUser user = GetUser(userId);
            if (user != null)
            {
                Parameters param = new Parameters();
                param.Add("status", status ? 1 : 0);
                SyncNotifyAction(ActionIDDefine.Cst_Action2014, user, param, null);
            }
        }

        /// <summary>
        /// 找桌位,已经进入过保留原位置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roomData"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool SelectPosition(GameUser user, RoomData roomData, TableData table)
        {
            var userPos = Array.Find(table.Positions, m => m.UserId == user.UserId);
            if (userPos != null)
            {
                //已经在桌上
                SetTablePosition(roomData.RoomId, table, userPos, user);
                return true;
            }
            if (!table.IsSettlemented && table.BackCardData.Count > 0)
            {
                //已经发牌
                return false;
            }
            if (table.IsSettlemented && table.BackCardData.Count > 0)
            {
                DoComplatedSettlement(table);
                return false;
            }
            foreach (var pos in table.Positions)
            {
                lock (table)
                {
                    if (pos.UserId == 0)
                    {
                        SetTablePosition(roomData.RoomId, table, pos, user);
                        return true;
                    }
                }
            }
            return false;
        }

        public void SyncNotifyAction(int actionId, TableData table, Parameters parameters, Action<int> callback)
        {
            SyncNotifyAction(actionId, ClientNotifier.GetUserList(table), parameters, callback);
        }

        public void SyncNotifyAction(int actionId, GameUser user, Parameters parameters, Action<int> callback)
        {
            ClientNotifier.NotifyAction(actionId, user, parameters, callback);
        }

        /// <summary>
        /// 异步通知Action处理
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="userList"></param>
        /// <param name="parameters"></param>
        /// <param name="callback"></param>
        public void SyncNotifyAction(int actionId, List<GameUser> userList, Parameters parameters, Action<int> callback)
        {
            ClientNotifier.NotifyAction(actionId, userList, parameters, callback);
        }

        /// <summary>
        /// 离开桌位
        /// </summary>
        /// <param name="user"></param>
        /// <param name="table"></param>
        public void ExitTablePosition(GameUser user, TableData table)
        {
            if (table.IsFlee)
            {
                return;
            }
            foreach (var pos in table.Positions)
            {
                if (pos.Id == user.Property.PositionId)
                {
                    CheckFlee(user, table, pos);
                    break;
                }
            }
        }

        /// <summary>
        /// 检查是否开始
        /// </summary>
        /// <param name="tableData"></param>
        public void CheckStart(TableData tableData)
        {
            bool started = !Array.Exists(tableData.Positions, pos => !pos.IsAI && pos.UserId == 0);
            if (started)
            {
                TraceLog.WriteComplement("房间:{0},桌:{1}开始发牌...", tableData.RoomId, tableData.TableId);
                foreach (var p in tableData.Positions)
                {
                    p.ReSendCard();
                }
                tableData.Init();
                //随机第一个开始叫地主
                int index = RandomUtils.GetRandom(0, tableData.Positions.Length);
                var pos = tableData.Positions[index];
                tableData.CallLandlordPos = pos.Id;
                tableData.CallLandlordId = pos.UserId;
                tableData.CallLandlordName = pos.NickName;
                TraceLog.WriteComplement("房间:{0},桌:{1}第一个开始叫地主{2}", tableData.RoomId, tableData.TableId, pos.Id + pos.NickName);

                SendCard(tableData, TableData.CardBackNum);
                SyncNotifyAction(ActionIDDefine.Cst_Action2004, tableData, null,
                    t =>
                    {
                        TraceLog.WriteComplement("桌子:{0}底牌:{1}", tableData.TableId, string.Join(",", tableData.BackCardData));
                        foreach (var p in tableData.Positions)
                        {
                            _cardRole.SortCard(p.CardData);
                            TraceLog.WriteComplement("桌子:{0}玩家{1}-{2}牌:{3}",
                                tableData.TableId, p.UserId, p.NickName, string.Join(",", p.CardData));
                        }
                        TraceLog.WriteComplement("房间:{0},桌:{1}发牌结束", tableData.RoomId, tableData.TableId);
                    });

            }
        }

        /// <summary>
        /// 获取玩家手上的牌，并自动整理牌从大到小排列
        /// </summary>
        /// <returns></returns>
        public List<int> GetUserCardData(GameUser user, TableData tableData, bool isSort = true)
        {
            var list = new List<int>();
            var pos = GetUserPosition(user, tableData);
            if (pos != null)
            {
                if (pos.CardData.Count > 0)
                {
                    list.AddRange(pos.CardData);
                    if (isSort)
                    {
                        _cardRole.SortCard(pos.CardData);
                    }
                }
            }
            return list;
        }

        public List<int> GetLandlordCardData(TableData tableData)
        {
            int postionId = tableData.LandlordPos;
            var list = new List<int>();
            var pos = GetUserPosition(postionId, tableData);
            if (pos != null)
            {
                if (pos.CardData.Count > 0)
                {
                    list.AddRange(pos.CardData);
                    _cardRole.SortCard(pos.CardData);
                }
            }
            return list;
        }


        /// <summary>
        /// 叫地主
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="tableData"></param>
        /// <param name="isCall">true:叫，false:不叫</param>
        public void CallCard(int positionId, TableData tableData, bool isCall)
        {
            if (positionId != tableData.CallLandlordPos)
            {
                TraceLog.WriteComplement("桌子:{0}未轮到位置{1}叫地主", tableData.TableId, positionId);
                return;
            }
            if (tableData.CallTimes < tableData.CallOperation.Length)
            {
                if (isCall)
                {
                    tableData.DoDouble();
                    TraceLog.WriteComplement("桌子:{0}叫地主加倍{1}", tableData.TableId, tableData.MultipleNum);
                }
                tableData.CallOperation[tableData.CallTimes] = isCall;
                tableData.CallTimes++;
            }
            if (tableData.CallTimes > tableData.PlayerNum - 1 &&
                !Array.Exists(tableData.CallOperation, op => op))
            {
                //都不叫时重新发牌接口
                TraceLog.WriteComplement("桌子:{0}重新发牌,CallTimes:{1},Log:{2}", tableData.TableId, tableData.CallTimes, string.Join(",", tableData.CallOperation));
                CheckStart(tableData);
                return;
            }

            int noCallNum = 0;
            int calledNum = 0;
            int preCallIndex = 0;//上次操作的索引
            //计算叫地主记录中，不叫次数
            int endIndex = tableData.CallTimes - 1;
            for (int i = endIndex; i >= 0; i--)
            {
                bool called = tableData.CallOperation[i];
                if (!called)
                {
                    noCallNum++;
                }
                else
                {
                    calledNum++;
                }
                if (called && calledNum == 1)
                {
                    preCallIndex = i;
                }
            }
            TraceLog.WriteComplement("桌子:{0}位置:{1},前一个:{2},最后一个:{3}叫地主", tableData.TableId, positionId, preCallIndex, endIndex);
            if ((tableData.CallTimes == tableData.PlayerNum && noCallNum == tableData.PlayerNum - 1) ||
                tableData.CallTimes > tableData.PlayerNum)
            {
                int index = endIndex - preCallIndex;
                index = (positionId + tableData.PlayerNum - index) % tableData.PlayerNum;
                PositionData pos = index >= 0 && index < tableData.Positions.Length ? tableData.Positions[index] : null;
                if (pos != null)
                {
                    //确定地主
                    pos.IsLandlord = true;
                    tableData.LandlordPos = pos.Id;
                    tableData.LandlordId = pos.UserId;
                    tableData.IsCallEnd = true;
                    tableData.OutCardPos = pos.Id;
                    tableData.OutCardUserId = pos.UserId;
                    //增加底牌
                    pos.CardData.AddRange(tableData.BackCardData);
                    _cardRole.SortCard(pos.CardData);
                }
            }
            else
            {
                //取下个叫地主玩家
                int nextPos = (positionId + 1) % tableData.PlayerNum;
                PositionData pos = tableData.Positions[nextPos];
                if (pos != null)
                {
                    tableData.CallLandlordPos = pos.Id;
                    tableData.CallLandlordId = pos.UserId;
                    tableData.CallLandlordName = pos.NickName;
                }
            }
            var param = new Parameters();
            param.Add("IsCall", isCall ? 1 : 0);
            param.Add("IsRob", (calledNum == 1 && isCall) || calledNum == 0 ? 0 : 1);
            SyncNotifyAction(ActionIDDefine.Cst_Action2006, tableData, param, null);

            TraceLog.WriteComplement("桌子:{0}叫地主通知成功，地主是:{1},是否结束{2}",
                tableData.TableId,
                tableData.IsCallEnd ? (tableData.LandlordId + "") : (tableData.CallLandlordId + tableData.CallLandlordName),
                tableData.IsCallEnd);
        }

        /// <summary>
        /// 明牌
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tableData"></param>
        public void ShowCard(GameUser user, TableData tableData)
        {
            var pos = GetUserPosition(user, tableData);
            if (pos != null && pos.IsLandlord && !tableData.IsShow)
            {
                tableData.DoDouble();
                pos.IsShow = true;
                tableData.IsShow = true;
                SyncNotifyAction(ActionIDDefine.Cst_Action2008, tableData, null, null);
                
                TraceLog.WriteComplement("桌子:{0}玩家{1}明牌通知成功", tableData.TableId, user.UserId);
            }
        }

        public bool OutCard(int userId, int positionId, TableData tableData, string cardsStr)
        {
            int errorCode;
            return DoOutCard(userId, positionId, tableData, cardsStr, out  errorCode);
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="positionId"></param>
        /// <param name="tableData"></param>
        /// <param name="cardsStr"></param>
        /// <param name="errorCode">0:正常,1:不合规则,2:不存在牌,3:离开桌位</param>
        public bool DoOutCard(int userId, int positionId, TableData tableData, string cardsStr, out int errorCode)
        {
            errorCode = 0;
            int[] cards = new int[0];
            if (!string.IsNullOrEmpty(cardsStr.Trim()))
            {
                string[] tempArray = cardsStr.Split(new char[] { ',' });
                List<int> tempList = new List<int>();
                for (int i = 0; i < tempArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tempArray[i]))
                    {
                        tempList.Add(tempArray[i].ToInt());
                    }
                }
                cards = tempList.ToArray();
            }

            var pos = GetUserPosition(positionId, tableData);
            if (pos == null)
            {
                errorCode = 3;
                return false;
            }
            if (!CheckCardEffective(pos.CardData, cards))
            {
                errorCode = 2;
                TraceLog.WriteComplement("桌子:{0}玩家{0}出牌{1}不在手中的牌内", tableData.TableId, userId, cardsStr);
                return false;
            }
            int cardSize;
            var cardType = _cardRole.GetCardType(cards, out cardSize);
            if (cardType == DeckType.Error)
            {
                errorCode = 1;
                TraceLog.WriteComplement("桌子:{0}玩家{0}出牌{1}不合规则", tableData.TableId, userId, cardsStr);
                return false;
            }
            if (cardType == DeckType.None && CheckOutCardNoneType(tableData))
            {
                //多次循环不出牌，则结束
                tableData.StopTimer();
                var param = new Parameters();
                param.Add("FleeUserId", 0);
                SyncNotifyAction(ActionIDDefine.Cst_Action2013, tableData, param,
                    c =>
                    {
                        //Console.WriteLine("Table:{0} is stop", tableData.TableId);
                        DoComplatedSettlement(tableData);
                        TraceLog.WriteError("桌子{0}多次连续不出牌并强制退出桌位", tableData.TableId);
                    });
                errorCode = 3;
                return true;
            }
            CardData cardData = new CardData(userId, positionId);
            cardData.Cards = cards;
            cardData.Type = cardType;
            cardData.CardSize = cardSize;

            if (tableData.PreCardData != null)
            {
                //压牌
                CardData tempData = tableData.PreCardData;
                if (cardData.Type != DeckType.None &&
                    tempData.Type != DeckType.None &&
                    tempData.UserId != cardData.UserId &&
                    !_cardRole.EqualsCard(cardData, tempData))
                {
                    errorCode = 1;
                    return false;
                }
            }
            foreach (var card in cardData.Cards)
            {
                pos.CardData.Remove(card);
            }
            tableData.OutCardList.Add(cardData);
            if (cardData.Type != DeckType.None)
            {
                tableData.PreCardData = cardData;
            }

            if (cardData.Type == DeckType.Bomb ||
                cardData.Type == DeckType.WangBomb)
            {
                tableData.DoDouble();
            }
            if (pos.CardData.Count > 0)
            {
                int index = (pos.Id + 1) % tableData.PlayerNum;
                var nextPos = tableData.Positions[index];
                tableData.OutCardPos = nextPos.Id;
                tableData.OutCardUserId = nextPos.UserId;
                SyncNotifyAction(ActionIDDefine.Cst_Action2010, tableData, null, null);

            }
            else
            {
                //结束
                foreach (var p in tableData.Positions)
                {
                    TraceLog.WriteComplement("桌子:{0} in {1}玩家:{2}-{3}剩余牌{4}",
                        tableData.TableId, tableData.RoomId, p.UserId, p.NickName, string.Join(",", p.CardData));
                }
                tableData.IsClosed = true;
                tableData.IsLandlordWin = pos.IsLandlord ? true : false;
                DoOutCardEnd(tableData);

                SyncNotifyAction(ActionIDDefine.Cst_Action2012, tableData, null,
                    t =>
                    {
                        DoComplatedSettlement(tableData);
                        TraceLog.WriteComplement("桌子:{0}玩家:{1}出牌结束通知", tableData.TableId, userId);
                    });
            }
            return true;
        }
        /// <summary>
        /// 检查是否出手中的牌
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private bool CheckCardEffective(List<int> cardData, int[] cards)
        {
            foreach (int card in cards)
            {
                if (!cardData.Exists(m => m == card))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckOutCardNoneType(TableData tableData)
        {
            if (tableData.OutCardList.Count > tableData.PlayerNum)
            {
                int noneNum = 0;
                var list = tableData.OutCardList;
                int count = tableData.OutCardList.Count - 1;
                for (int i = 0; i <= tableData.PlayerNum; i++)
                {
                    if (list[count - i].Type == DeckType.None)
                    {
                        noneNum++;
                    }
                    else
                    {
                        noneNum = 0;
                    }
                }
                return noneNum > tableData.PlayerNum;
            }
            return false;
        }

        /// <summary>
        /// 完成结算
        /// </summary>
        /// <param name="tableData"></param>
        private void DoComplatedSettlement(TableData tableData)
        {
            //放入空桌池中RoomData roomData;
            RoomData roomData = GetRoomData(tableData.RoomId);
            if (roomData != null)
            {
                foreach (PositionData position in tableData.Positions)
                {
                    var user = GetUser(position.UserId);
                    if (user != null)
                    {
                        user.Property.InitTablePos();
                    }
                    position.Init();
                }
                tableData.Init();
                if (roomData.Tables.Remove(tableData.TableId))
                {
                    //Console.WriteLine("Table:{0} is init", tableData.TableId);
                    int minTableCount = ConfigEnvSet.GetInt("Game.Table.MinTableCount", 10);
                    if (roomData.TablePool.Count < minTableCount)
                    {
                        roomData.TablePool.Enqueue(tableData);
                    }
                    else
                    {
                        tableData.Dispose();
                    }
                    return;
                }
            }
            TraceLog.WriteError("TableData object {1} in {0} room has be disposed error.", tableData.RoomId, tableData.TableId);
        }

        /// <summary>
        /// 出牌结束,并结算积分
        /// </summary>
        /// <param name="tableData"></param>
        private void DoOutCardEnd(TableData tableData)
        {
            int count = (tableData.CardData.Count - TableData.CardBackNum) / tableData.PlayerNum;
            int landCount = count + TableData.CardBackNum - 1;//地主只出一张
            int[] cardNums = new int[tableData.Positions.Length];
            int landIndex = 0;
            int index = 0;
            int noOutNum = 0;
            List<PositionData> posList = new List<PositionData>();
            foreach (PositionData position in tableData.Positions)
            {
                if (position.IsLandlord)
                {
                    landIndex = index;
                }
                else
                {
                    posList.Add(position);
                }
                if (position.CardData.Count == count)
                {
                    noOutNum++;
                }
                cardNums[index] = position.CardData.Count;
                index++;
            }
            //春天判断
            if ((tableData.IsLandlordWin && noOutNum == 2) ||
                (!tableData.IsLandlordWin && cardNums[landIndex] == landCount))
            {
                tableData.DoDouble();
            }
            var pos = tableData.Positions[landIndex];
            DoSettlement(tableData, tableData.IsLandlordWin, pos, posList);
        }

        /// <summary>
        /// 结算积分
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="isLandlordWin"></param>
        /// <param name="landPos"></param>
        /// <param name="posList"></param>
        private void DoSettlement(TableData tableData, bool isLandlordWin, PositionData landPos, List<PositionData> posList)
        {
            //都有加积分
            GameUser user;
            if (isLandlordWin)
            {
                foreach (PositionData pos in posList)
                {
                    user = GetUser(pos.UserId);
                    if (user != null)
                    {
                        pos.CoinNum = tableData.AnteNum / 2;
                        pos.ScoreNum = tableData.MultipleNum / 2;
                        pos.CoinNum = user.GameCoin > pos.CoinNum ? pos.CoinNum : user.GameCoin;
                        landPos.CoinNum += pos.CoinNum;
                        landPos.ScoreNum += pos.ScoreNum;
                        pos.ScoreNum = user.ScoreNum > pos.ScoreNum ? pos.ScoreNum : user.ScoreNum;
                        
                        TraceLog.WriteComplement("桌子:{0}玩家(农):{1}败,结算:-{2}金豆,-{3}积分,之前剩余:{4}-{5}",
                            tableData.TableId, user.UserId, pos.CoinNum, pos.ScoreNum, user.GameCoin, user.ScoreNum);
                        user.GameCoin = MathUtils.Subtraction(user.GameCoin, pos.CoinNum);
                        user.ScoreNum = MathUtils.Subtraction(user.ScoreNum, pos.ScoreNum);
                        user.FailNum = MathUtils.Addition(user.FailNum, 1);
                    }
                    else
                    {
                        landPos.CoinNum += tableData.AnteNum / 2;
                        landPos.ScoreNum += tableData.MultipleNum / 2;
                    }
                }
                user = GetUser(landPos.UserId);
                if (user != null)
                {
                    TraceLog.WriteComplement("桌子:{0}玩家(主):{1}胜,结算:+{2}金豆,+{3}积分,之前剩余:{4}-{5}",
                        tableData.TableId, user.UserId, landPos.CoinNum, landPos.ScoreNum, user.GameCoin, user.ScoreNum);
                    user.GameCoin = MathUtils.Addition(user.GameCoin, landPos.CoinNum);
                    user.ScoreNum = MathUtils.Addition(user.ScoreNum, landPos.ScoreNum);
                    user.WinNum = MathUtils.Addition(user.WinNum, 1);
                    AchieveTask.SaveUserTask(user.UserId.ToString(), TaskClass.HuanLe, 1);
                    AchieveTask.SaveUserTask(user.UserId.ToString(), TaskClass.HuanLeJiFen, landPos.ScoreNum);
                }
            }
            else
            {
                user = GetUser(landPos.UserId);
                if (user != null)
                {
                    landPos.CoinNum = user.GameCoin > tableData.AnteNum ? tableData.AnteNum : user.GameCoin;
                    landPos.ScoreNum = tableData.MultipleNum;
                    
                    TraceLog.WriteComplement("桌子:{0}玩家(主):{1}败,结算:-{2}金豆,-{3}积分,之前剩余:{4}-{5}",
                        tableData.TableId, user.UserId, landPos.CoinNum, landPos.ScoreNum, user.GameCoin, user.ScoreNum);
                    user.GameCoin = MathUtils.Subtraction(user.GameCoin, landPos.CoinNum);
                    user.ScoreNum = MathUtils.Subtraction(user.ScoreNum, user.ScoreNum > tableData.MultipleNum ? tableData.MultipleNum : user.ScoreNum);
                    user.FailNum = MathUtils.Addition(user.FailNum, 1);
                }
                else
                {
                    landPos.CoinNum += tableData.AnteNum;
                    landPos.ScoreNum += tableData.MultipleNum;
                }

                foreach (PositionData pos in posList)
                {
                    user = GetUser(pos.UserId);
                    if (user != null)
                    {
                        pos.CoinNum = landPos.CoinNum / 2;
                        pos.ScoreNum = landPos.ScoreNum / 2;
                        
                        TraceLog.WriteComplement("桌子:{0}玩家(农):{1}胜,结算:+{2}金豆,+{3}积分,之前剩余:{4}-{5}",
                            tableData.TableId, user.UserId, pos.CoinNum, pos.ScoreNum, user.GameCoin, user.ScoreNum);
                        user.GameCoin = MathUtils.Addition(user.GameCoin, pos.CoinNum);
                        user.ScoreNum = MathUtils.Addition(user.ScoreNum, pos.ScoreNum);
                        user.WinNum = MathUtils.Addition(user.WinNum, 1);
                        AchieveTask.SaveUserTask(user.UserId.ToString(), TaskClass.HuanLe, 1);
                        AchieveTask.SaveUserTask(user.UserId.ToString(), TaskClass.HuanLeJiFen, pos.ScoreNum);
                    }
                }

            }
            _userCacheSet.Update();
            tableData.IsSettlemented = true;
            //出牌记录
            StringBuilder sb = new StringBuilder();
            foreach (var card in tableData.OutCardList)
            {
                sb.AppendLine();
                sb.AppendFormat("User:{0}\t->{1}", card.UserId, string.Join(",", card.Cards));
            }
            TraceLog.WriteComplement("房间:{0}桌子:{1}出牌记录:{2}", tableData.RoomId, tableData.TableId, sb);
        }


        /// <summary>
        /// 检查逃跑，并结束当局游戏，扣分两家平分
        /// </summary>
        /// <param name="user"></param>
        /// <param name="table"></param>
        /// <param name="userPos"></param>
        private void CheckFlee(GameUser user, TableData table, PositionData userPos)
        {
            table.IsClosed = true;
            if (table.IsStarting)
            {
                int multipleMinNum = ConfigEnvSet.GetInt("Game.FleeMultipleNum", 10);
                PositionData pos = null;
                List<PositionData> posList = new List<PositionData>();
                //计算炸弹数量
                foreach (PositionData position in table.Positions)
                {
                    if (position.UserId == user.UserId)
                    {
                        pos = position;
                    }
                    else
                    {
                        posList.Add(position);
                    }
                    var obj = _cardRole.ParseCardGroup(position.CardData.ToArray());
                    for (int i = 0; i < obj.Count; i++)
                    {
                        var item = obj.GetSame(i);
                        if (item.Length == 4 || (item.Length == 2 && _cardRole.GetCardSize(item[0]) > (int)CardSize.C_2))
                        {
                            table.DoDouble();
                        }
                    }
                }
                table.IsFlee = true;
                if (table.MultipleNum < multipleMinNum)
                {
                    table.SetDouble(multipleMinNum);
                }
                DoSettlement(table, false, pos, posList);
                var param = new Parameters();
                param.Add("FleeUserId", user.UserId);
                var userList = ClientNotifier.GetUserList(table, user.UserId);
                SyncNotifyAction(ActionIDDefine.Cst_Action2013, userList, param,
                    c =>
                    {
                        DoComplatedSettlement(table);
                        //
                        TraceLog.WriteComplement("桌子:{0}玩家{1}逃跑通知", table.TableId, user.UserId);
                    });

                NotifyUserChange(user.UserId);
            }
            else
            {
                //还没有开始时，可以离开位置等待新玩家
                user.Property.InitTablePos();
                userPos.Init();
                table.IsClosed = false;
                int periodAiStart = ConfigEnvSet.GetInt("Game.Table.AIIntoTime", 5000);
                table.ReStartTimer(periodAiStart);
                var userList = ClientNotifier.GetUserList(table, user.UserId);
                SyncNotifyAction(ActionIDDefine.Cst_Action2003, userList, null, null);
            }
        }

        public GameUser GetUser(int userId)
        {
            return _userCacheSet.FindKey(userId.ToString());
        }

        /// <summary>
        /// 桌子占位
        /// </summary>
        /// <param name="table"></param>
        /// <param name="pos"></param>
        /// <param name="user"></param>
        /// <param name="roomId"></param>
        private static void SetTablePosition(int roomId, TableData table, PositionData pos, GameUser user)
        {
            pos.Init(user);
            user.Property.InitTablePos(roomId, table.TableId, pos.Id);
        }

        /// <summary>
        /// 创建一副牌数据
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="pokers"></param>
        private void CreateCardData(TableData tableData, List<PokerInfo> pokers)
        {
            if (pokers == null) throw new ArgumentNullException("pokers");
            foreach (var pokerInfo in pokers)
            {
                tableData.CardData.Add(pokerInfo.Id);
            }
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        /// <param name="cardData"></param>
        private void ShuffleCard(List<int> cardData)
        {
            RandomCards(cardData);
        }

        /// <summary>
        /// 随机取牌
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="timeNum">次数</param>
        /// <returns></returns>
        private void RandomCards(List<int> cardData, int timeNum = 0)
        {
            int count = cardData.Count;
            int randCount = 0;
            int index = 0;
            int endIndex = 0;
            int temp = 0;
            int val = 0;
            do
            {
                int endPos = count - randCount;
                if (endPos <= 0)
                {
                    break;
                }
                index = RandomUtils.GetRandom(0, endPos);
                endIndex = endPos - 1;
                if (endIndex == index)
                {
                    //自己与自己不替换
                    randCount++;
                    continue;
                }
                temp = cardData[endIndex];
                val = cardData[index];
                cardData[endIndex] = val;
                cardData[index] = temp;
                randCount++;
            } while (randCount < count && (timeNum == 0 || randCount < timeNum));

        }

        /// <summary>
        /// 设置最后三张为底牌
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="backNum">底牌数</param>
        private void SetBackCard(TableData tableData, int backNum)
        {
            int index = tableData.CardData.Count - backNum;
            var list = tableData.CardData.GetRange(index, backNum);
            if (list.Count > 0)
            {
                tableData.BackCardData.AddRange(list);
            }
        }

        /// <summary>
        /// 发牌
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="backNum">底牌数</param>
        /// <returns></returns>
        private void SendCard(TableData tableData, int backNum)
        {
            ShuffleCard(tableData.CardData);
            SetBackCard(tableData, backNum);

            var cardData = tableData.CardData;
            int cardCount = cardData.Count - backNum;
            int posCount = tableData.Positions.Length;
            for (int i = 0; i < posCount; i++)
            {
                int posIndex = i;
                var position = tableData.Positions[posIndex];
                if (position.CardData.Count > 0)
                {
                    position.ReSendCard();
                }
                for (int j = posIndex; j < cardCount; j = j + posCount)
                {
                    position.CardData.Add(cardData[j]);
                }
            }
        }

    }
}
