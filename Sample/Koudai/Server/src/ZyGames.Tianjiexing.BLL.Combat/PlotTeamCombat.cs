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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ZyGames.Framework.Collection.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Combat;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Component;


namespace ZyGames.Tianjiexing.BLL.Combat
{
    public class PlotTeamCombat
    {
        private static readonly object ThisLock = new object();
        /// <summary>
        /// 战斗过程 Key：teamId
        /// </summary>
        private static DictionaryExtend<int, TeamCombatResult> _combatProcessList;
        /// <summary>
        /// Key：userid, value:teamId
        /// </summary>
        private static DictionaryExtend<string, int> _userList;
        /// <summary>
        /// Key：teamId, value:组队类
        /// </summary>
        private static DictionaryExtend<int, MorePlotTeam> _teamList;
        /// <summary>
        /// Key：userid+plotid, value:战斗次数
        /// </summary>
        private static DictionaryExtend<string, int> _userTimesList;
        private int MaxCombatNum = ConfigEnvSet.GetInt("ProplePlot.MaxCombatNum");
        public static int TeamMaxPeople = ConfigEnvSet.GetInt("ProplePlot.MaxPeopleNum");
        private string _userId;


        public static void Init(GameActive gameActive)
        {
            _combatProcessList = new DictionaryExtend<int, TeamCombatResult>();
            _userList = new DictionaryExtend<string, int>();
            _teamList = new DictionaryExtend<int, MorePlotTeam>();
            _userTimesList = new DictionaryExtend<string, int>();
        }

        static PlotTeamCombat()
        {
            Init(null);
        }

        public static void Dispose(GameActive gameActive)
        {
            _combatProcessList = null;
            _userList = null;
            _teamList = null;
            _userTimesList = null;

        }

        public PlotTeamCombat(GameUser user)
        {
            _userId = user.UserID;
        }

        private static void SetCombatResult(int teamId, bool isWin)
        {
            if (_combatProcessList != null)
            {
                if (!_combatProcessList.ContainsKey(teamId))
                {
                    _combatProcessList.TryAdd(teamId, new TeamCombatResult());
                }
                lock (ThisLock)
                {
                    TeamCombatResult tempList = _combatProcessList[teamId];
                    tempList.IsWin = isWin;
                }
            }
        }


        private static int _currTeamId = 1000;

        private static int NextTeamId
        {
            get
            {
                lock (ThisLock)
                {
                    _currTeamId++;
                    return _currTeamId;
                }
            }
        }

        /// <summary>
        /// 获取Team信息
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public MorePlotTeam GetTeam(int teamId)
        {
            return _teamList != null && _teamList.ContainsKey(teamId) ? _teamList[teamId] : new MorePlotTeam();
        }

        /// <summary>
        /// 查询所在Team
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>无：-1</returns>
        public int GetTeamId(string userId)
        {
            if (_userList != null && _userList.ContainsKey(userId))
            {
                return _userList[userId];
            }
            return -1;
        }

        /// <summary>
        /// 设置赢的次数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="plotId"></param>
        public void SetWinNum(string userId, int plotId)
        {
            if (_userTimesList == null) return;
            string key = userId + plotId;
            lock (ThisLock)
            {
                if (_userTimesList.ContainsKey(key))
                {
                    _userTimesList[key] = _userTimesList[key] + 1;
                }
                else
                {
                    _userTimesList.Add(key, 1);
                }
            }
        }

        /// <summary>
        /// 获取所在Team的位置
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public int TeamPos(int teamId)
        {
            var plotTeam = GetTeam(teamId);
            if (plotTeam != null)
            {
                return plotTeam.UserList.FindIndex(m => m.UserId.Equals(_userId));
            }
            return -1;
        }

        /// <summary>
        /// 获取等待组队列表
        /// </summary>
        /// <returns></returns>
        public List<MorePlotTeam> ToTeamList()
        {
            List<MorePlotTeam> list = new List<MorePlotTeam>();
            if (_teamList != null)
            {
                List<KeyValuePair<int, MorePlotTeam>> cuserList = _teamList.ToList();
                foreach (KeyValuePair<int, MorePlotTeam> keyPair in cuserList)
                {
                    MorePlotTeam cuser = keyPair.Value;
                    if (cuser.IsAllow && IsCombat(cuser.MorePlot.PlotID))
                    {
                        list.Add(cuser);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 活动获取等待组队列表
        /// </summary>
        /// <returns></returns>
        public List<MorePlotTeam> ToMoreTeamList()
        {
            List<MorePlotTeam> list = new List<MorePlotTeam>();
            if (_teamList != null)
            {
                var cuserList = _teamList.ToList();
                foreach (KeyValuePair<int, MorePlotTeam> keyPair in cuserList)
                {
                    MorePlotTeam cuser = keyPair.Value;
                    if (cuser.IsAllow && IsMoreCombat(cuser.MorePlot.PlotID))
                    {
                        list.Add(cuser);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 快速加入
        /// 优先顺序:副本低到高,队中人数高到低 改为随机
        /// </summary>
        /// <param name="teamId"></param>
        public bool AddTeam(out int teamId)
        {
            teamId = -1;
            List<MorePlotTeam> tempList = new List<MorePlotTeam>();
            List<MorePlotTeam> list = ToTeamList();
            foreach (MorePlotTeam item in list)
            {
                MorePlotTeam cuser = item;
                if (cuser.IsAllow && IsCombat(cuser.MorePlot.PlotID))
                {
                    tempList.Add(cuser);
                }
            }
            if (tempList.Count > 0)
            {
                MorePlotTeam team = tempList[RandomUtils.GetRandom(0, list.Count)];
                teamId = team.TeamID;
                AddTeam(teamId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 快速加入 活动
        /// 优先顺序:副本低到高,队中人数高到低 改为随机
        /// </summary>
        /// <param name="teamId"></param>
        public bool AddMoreTeam(out int teamId)
        {
            teamId = -1;
            List<MorePlotTeam> tempList = new List<MorePlotTeam>();
            List<MorePlotTeam> list = ToMoreTeamList(); //ToTeamList();
            foreach (MorePlotTeam item in list)
            {
                MorePlotTeam cuser = item;
                if (cuser.IsAllow && IsMoreCombat(cuser.MorePlot.PlotID))
                {
                    tempList.Add(cuser);
                }
            }
            if (tempList.Count > 0)
            {
                MorePlotTeam team = tempList[RandomUtils.GetRandom(0, list.Count)];
                teamId = team.TeamID;
                AddTeam(teamId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加入组队,若已经在其它组队则自动退出
        /// </summary>
        /// <param name="teamId"></param>
        public bool AddTeam(int teamId)
        {
            if (_teamList == null) return false;
            if (_teamList.ContainsKey(teamId))
            {
                var team = _teamList[teamId];
                if (!team.IsAllow) return false;
                //是否在其它组队
                if (_userList.ContainsKey(_userId))
                {
                    int oldTeamId = _userList[_userId];
                    if (oldTeamId != teamId)
                    {
                        LeaveTeam(oldTeamId);
                        _userList[_userId] = teamId;
                    }
                }
                else
                {
                    _userList.Add(_userId, teamId);
                }
                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(_userId);
                team.Append(gameUser);
                return true;
            }
            return false;
        }

        public bool IsCombat(int plotId)
        {
            var plot = new ConfigCacheSet<PlotInfo>().FindKey(plotId);
            if (plot != null)
            {
                //todo
                var userPlot = UserPlotHelper.GetUserPlotInfo(_userId, plot.PrePlotID);
                    //new GameDataCacheSet<UserPlot>().FindKey(_userId, plot.PrePlotID);)
                if (userPlot != null && userPlot.PlotStatus == PlotStatus.Completed)
                {
                    //string key = _userId + plotId;
                    //int timesNum = _userTimesList != null && _userTimesList.ContainsKey(key) ? _userTimesList[key] : 0;
                    //return timesNum < MaxCombatNum;
                    int timesNum = CombatHelper.GetDailyMorePlotNum(_userId, plotId);
                    return timesNum < MaxCombatNum;
                }
            }
            return false;
        }

        public bool IsMoreCombat(int plotId)
        {
            var plot = new ConfigCacheSet<PlotInfo>().FindKey(plotId);
            if (plot != null)
            {
                //string key = _userId + plotId;
                //int timesNum = _userTimesList != null && _userTimesList.ContainsKey(key) ? _userTimesList[key] : 0;
                //return timesNum < MaxCombatNum;
                int timesNum = CombatHelper.GetDailyMorePlotNum(_userId, plotId);
                return timesNum < MaxCombatNum;
            }
            return false;
        }

        /// <summary>
        /// 可创建多人副本列表
        /// </summary>
        /// <returns></returns>
        public MorePlot[] GetMorePlotList()
        {
            List<MorePlot> morePlotsList = new List<MorePlot>();
            var plotsArray = UserPlotHelper.UserPlotFindAll(_userId);
                // todo new GameDataCacheSet<UserPlot>().FindAll(_userId);)
            foreach (UserPlotInfo plot in plotsArray)
            {
                var morePlotArray = new ConfigCacheSet<PlotInfo>().FindAll(u => u.PlotType == PlotType.MorePlot && u.PrePlotID == plot.PlotID);

                if (morePlotArray.Count > 0)
                {
                    var morePlot = morePlotArray[0];
                    if (IsCombat(morePlot.PlotID))
                    {
                        morePlotsList.Add(GetItem(morePlot.PlotID));
                    }
                }
            }
            return morePlotsList.ToArray();
        }

        /// <summary>
        /// 可创建活动多人副本列表
        /// </summary>
        /// <returns></returns>
        public MorePlot[] GetMorePlotFestivalList(FunctionEnum functionEnum)
        {
            List<MorePlot> morePlotsList = new List<MorePlot>();
            var morePlotArray = new List<PlotInfo>();
            if (functionEnum == FunctionEnum.MorePlotCoin)
            {
                morePlotArray = new ConfigCacheSet<PlotInfo>().FindAll(m => m.PlotType == PlotType.MorePlotCoin);
            }
            else if (functionEnum == FunctionEnum.MorePlotEnergy)
            {
                morePlotArray = new ConfigCacheSet<PlotInfo>().FindAll(m => m.PlotType == PlotType.MorePlotEnergy);
            }
            if (morePlotArray.Count > 0)
            {
                //var morePlot = morePlotArray[0];
                foreach (PlotInfo info in morePlotArray)
                {
                    if (functionEnum == FunctionEnum.MorePlotCoin && IsMoreCombat(info.PlotID))
                    {
                        morePlotsList.Add(GetItem(info.PlotID));
                    }
                    else if (functionEnum == FunctionEnum.MorePlotEnergy && IsMoreCombat(info.PlotID))
                    {
                        morePlotsList.Add(GetItem(info.PlotID));
                    }
                }
            }
            return morePlotsList.ToArray();
        }


        /// <summary>
        /// 是否在组队中
        /// </summary>
        /// <returns></returns>
        public bool IsInTeam()
        {
            if (GetTeamId(_userId) != -1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建组队
        /// </summary>
        /// <param name="plotId"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public bool CreateTeam(int plotId, out int teamId)
        {
            teamId = GetTeamId(_userId);
            if (teamId != -1)
            {
                //退出组队
                if (!LeaveTeam(teamId)) return false;
            }
            GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(_userId);
            teamId = Create(gameUser, plotId);
            return true;
        }

        /// <summary>
        /// 创建组队
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plotId"></param>
        /// <returns></returns>
        private int Create(GameUser user, int plotId)
        {
            if (_teamList == null) return -1;

            int teamId = NextTeamId;
            MorePlot morePlot = GetItem(plotId);
            var team = new MorePlotTeam
            {
                MorePlot = morePlot,
                TeamID = teamId,
                TeamUser = new TeamUser
                {
                    UserId = user.UserID,
                    NickName = user.NickName,
                    //UserLv = user.UserLv,
                    //UseMagicID = user.UseMagicID
                },
                CombatResult = false,
                Status = 1,
            };

            _teamList.Add(teamId, team);
            AddTeam(teamId);
            return teamId;
        }
        private MorePlot GetItem(int plotId)
        {
            var plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(plotId);
            MorePlot morePlot = new MorePlot
            {
                PlotID = plotId,
                PlotName = plotInfo.PlotName,
                Experience = plotInfo.Experience,
                ExpNum = plotInfo.ExpNum,
                ObtainNum = plotInfo.ObtainNum
            };

            if (plotInfo == null || string.IsNullOrEmpty(plotInfo.ItemRank))
            {
                return morePlot;
            }
            string[] itemRandArray = plotInfo.ItemRank.Split(',');
            if (itemRandArray.Length > 0)
            {
                string[] itemArray = itemRandArray[0].Split('=');
                if (itemArray.Length == 2)
                {
                    var itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemArray[0]);
                    morePlot.ItemId = itemInfo.ItemID;
                    morePlot.ItemName = itemInfo.ItemName;
                    morePlot.ItemNum = Convert.ToInt32(itemArray[1]);
                }
            }
            return morePlot;
        }


        /// <summary>
        /// 调整队伍位置
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <param name="isUp"></param>
        public bool MoveTeamPos(int teamId, string userId, bool isUp)
        {
            if (_teamList == null) return false;
            if (_teamList.ContainsKey(teamId))
            {
                var team = _teamList[teamId];
                if (team.Status != 1) return false;
                int index = team.UserList.FindIndex(m => m.UserId.Equals(userId));
                if (index == -1) return false;

                if (isUp && index > 0)
                {
                    //向上
                    var temp = team.UserList[index - 1];
                    team.UserList[index - 1] = team.UserList[index];
                    team.UserList[index] = temp;
                }
                else if (!isUp && index < TeamMaxPeople)
                {
                    //向下
                    var temp = team.UserList[index + 1];
                    team.UserList[index + 1] = team.UserList[index];
                    team.UserList[index] = temp;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 离开组队，自己是队长则解散组队
        /// </summary>
        /// <param name="teamId"></param>
        public bool LeaveTeam(int teamId)
        {
            return SendOutMember(teamId, _userId);
        }

        /// <summary>
        /// 踢出队员
        /// </summary>
        public bool SendOutMember(int teamId, string userId)
        {
            if (_teamList == null) return false;
            if (_teamList.ContainsKey(teamId))
            {
                var team = _teamList[teamId];
                if (team == null || team.Status != 1) return false;
                if (team.TeamUser.UserId.Equals(userId))
                {
                    if (_userList.ContainsKey(userId)) _userList.Remove(userId);
                    team.Status = 3;//解散组队
                    team.TeamUser = new TeamUser();
                    team.UserList.Clear();
                }
                team.UserList.RemoveAll(m => m.UserId.Equals(userId));
                if (_userList.ContainsKey(userId)) _userList.Remove(userId);

            }
            return true;
        }


        /// <summary>
        /// 取当前用户所占位置的战斗过程,位置从0开始
        /// </summary>
        /// <returns></returns>
        public TeamCombatResult GetCombatProcess(int teamId)
        {
            var combatResult = new TeamCombatResult();
            if (_combatProcessList != null && _combatProcessList.ContainsKey(teamId))
            {
                var processList = _combatProcessList[teamId];
                combatResult.IsWin = processList.IsWin;
                int[] posList = new int[2];
                int pos = TeamPos(teamId);
                if (ConfigPos.GetLength(0) > pos)
                {
                    for (int i = 0; i < posList.Length; i++)
                    {
                        posList[i] = ConfigPos[pos, i] - 1;
                    }
                }
                combatResult.ProcessList = processList.ProcessList.FindAll(m => m.UserId.Equals(_userId)
                    || (!m.UserId.Equals(_userId) && (m.Position == posList[0] || m.Position == posList[1])));
                //foreach (var process in combatResult.ProcessList)
                //{
                //    Trace.WriteLine(string.Format("多人副本>>{0}打{1}位置{2}结果{3}", process.ProcessContainer.DefenseList.Count,
                //    process.PlotNpcID, process.Position, process.IsWin));
                //}
                //Trace.WriteLine(string.Format("多人副本>>{0}", combatResult.IsWin));
                if (_userList != null && _userList.ContainsKey(_userId)) _userList.Remove(_userId);
            }
            return combatResult;
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        /// <param name="teamId"></param>
        public bool DoStart(int teamId)
        {
            if (_teamList.ContainsKey(teamId))
            {
                var team = _teamList[teamId];

                if (team.UserList.Count > 1)
                {
                    if (team.Status == 1)
                    {
                        lock (this)
                        {
                            team.Status = 2;
                        }
                        DoCombat(team);
                        return true;
                    }
                }
            }
            return false;
        }

        private void DoCombat(MorePlotTeam team)
        {
            //初始阵形
            var plotNpcTeam = new ConfigCacheSet<PlotNPCInfo>().FindAll(m => m.PlotID == team.MorePlot.PlotID);
            List<MonsterQueue> monsterQueueList = new List<MonsterQueue>(plotNpcTeam.Count);
            var userEmbattleList = new List<UserEmbattleQueue>(team.UserList.Count);
            foreach (var npcInfo in plotNpcTeam)
            {
                monsterQueueList.Add(new MonsterQueue(npcInfo.PlotNpcID));
            }
            foreach (var user in team.UserList)
            {
                var gameUser = new GameDataCacheSet<GameUser>().FindKey(user.UserId);
                userEmbattleList.Add(new UserEmbattleQueue(user.UserId, gameUser.UseMagicID, 0, CombatType.MultiPlot));
            }
            bool isLoop = true;
            int maxCount = 0;
            while (isLoop)
            {
                if (maxCount > 500)
                {
                    break;
                }
                int overNum = 0;
                for (int i = 0; i < userEmbattleList.Count; i++)
                {
                    maxCount++;
                    int position;
                    var userEmbattle = userEmbattleList[i];
                    if (userEmbattle.IsOver)
                    {
                        overNum++;
                        continue;
                    }
                    var monster = GetMonster(monsterQueueList, i, out position);
                    if (monster == null || monster.IsOver)
                    {
                        team.CombatResult = true;
                        isLoop = false;
                        break;
                    }
                    ICombatController controller =  new TjxCombatController();
                    ISingleCombat plotCombater = controller.GetSingleCombat(CombatType.MultiPlot);
                    plotCombater.SetAttack(userEmbattle);
                    plotCombater.SetDefend(monster);
                    bool IsWin = plotCombater.Doing();


                    var processLost = new TeamCombatProcess
                    {
                        TeamID = team.TeamID,
                        PlotID = team.MorePlot.PlotID,
                        Position = position,
                        ProcessContainer = (CombatProcessContainer)plotCombater.GetProcessResult(),
                        UserId = team.UserList[i].UserId,
                        PlotNpcID = plotNpcTeam[position].PlotNpcID,
                        IsWin = IsWin
                    };
                    //new BaseLog().SaveDebuLog(string.Format("多人副本>>{4}组队{0}打{1}位置{2}结果{3}", processLost.UserId, processLost.PlotNpcID, position + 1, IsWin, team.TeamID));

                    AppendLog(team.TeamID, processLost);
                }
                if (overNum == userEmbattleList.Count)
                {
                    team.CombatResult = false;
                    isLoop = false;
                }
            }

            //奖励
            if (team.CombatResult)
            {
                //new BaseLog().SaveDebuLog(string.Format("多人副本>>组队{0}结果{1}", team.TeamID, team.CombatResult));
                SetCombatResult(team.TeamID, team.CombatResult);

                var chatService = new TjxChatService();
                foreach (var user in team.UserList)
                {
                    GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(user.UserId);
                    gameUser.ExpNum = MathUtils.Addition(gameUser.ExpNum, team.MorePlot.ExpNum, int.MaxValue);
                    //gameUser.Update();
                    UserItemHelper.AddUserItem(user.UserId, team.MorePlot.ItemId, team.MorePlot.ItemNum);
                    new BaseLog("参加多人副本获得奖励：" + team.MorePlot.ItemName);
                    SetWinNum(user.UserId, team.MorePlot.PlotID);
                    CombatHelper.DailyMorePlotRestrainNum(gameUser.UserID, team.MorePlot.PlotID); // 多人副本获胜加一次
                    chatService.SystemSendWhisper(gameUser, string.Format(LanguageManager.GetLang().St4211_MorePlotReward,
                        team.MorePlot.ExpNum, team.MorePlot.ItemName, team.MorePlot.ItemNum));

                }
            }
        }
        private static void AppendLog(int teamId, TeamCombatProcess logTeam)
        {
            if (_combatProcessList != null)
            {
                if (!_combatProcessList.ContainsKey(teamId))
                {
                    _combatProcessList.Add(teamId, new TeamCombatResult());
                }
                lock (ThisLock)
                {
                    TeamCombatResult tempList = _combatProcessList[teamId];
                    tempList.ProcessList.Add(logTeam);
                }
                UserCombatLog log = new UserCombatLog
                {
                    CombatLogID = Guid.NewGuid().ToString(),
                    CombatType = CombatType.MultiPlot,
                    UserID = logTeam.UserId,
                    PlotID = logTeam.PlotID,
                    NpcID = logTeam.PlotNpcID,
                    HostileUser = logTeam.TeamID.ToString(),
                    IsWin = logTeam.IsWin,
                    CombatProcess = JsonUtils.Serialize(logTeam.ProcessContainer),
                    CreateDate = DateTime.Now
                };
                var sender = DataSyncManager.GetDataSender();
                sender.Send(log);
            }
        }

        private static readonly int[,] ConfigPos = new[,] { 
            { 1, 4, 2, 5, 3, 6 },
            { 2, 5, 1, 4, 3, 6 }, 
            { 3, 6, 2, 5, 1, 4 } 
        };

        private static MonsterQueue GetMonster(List<MonsterQueue> monsterQueueList, int index, out int position)
        {
            MonsterQueue monster = null;
            position = -1;
            if (ConfigPos.GetLength(0) > index)
            {
                for (int i = 0; i < ConfigPos.GetLength(1); i++)
                {
                    position = ConfigPos[index, i] - 1;
                    if (monsterQueueList.Count > position)
                    {
                        var temp = monsterQueueList[position];
                        if (!temp.IsOver)
                        {
                            monster = temp;
                            break;
                        }
                    }
                }
            }

            return monster;
        }

        /// <summary>
        /// 是否多人副本时间
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static bool IsMorePlotDate(string userID, FunctionEnum activeType)
        {
            DateTime beginTime = new DateTime();
            DateTime enableTime = new DateTime();
            GameActive[] gameActivesArray
                = new List<GameActive>(new GameActiveCenter(userID).GetActiveList()).FindAll(m => m.ActiveType == activeType).ToArray();
            foreach (GameActive gameActive in gameActivesArray)
            {
                beginTime = gameActive.BeginTime;
                enableTime = gameActive.EndTime;
                if (DateTime.Now > beginTime && DateTime.Now < enableTime)
                {
                    return true;
                }
            }
            return false;
        }
    }
}