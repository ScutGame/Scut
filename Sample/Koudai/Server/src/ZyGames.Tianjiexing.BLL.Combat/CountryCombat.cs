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
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ZyGames.Framework.Cache.Generic;
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

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 国家分组
    /// </summary>
    public class CountryGroup
    {
        internal CountryGroup(CountryType countryType)
        {
            CountryType = countryType;
            NomarlGroup = new CountryLvGroup();
            //NomarlGroup.Country = this;
            AdvancedGroup = new CountryLvGroup();
            //AdvancedGroup.Country = this;
        }

        public CountryType CountryType { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int Score
        {
            get;
            set;
        }

        /// <summary>
        /// 普通组
        /// </summary>
        internal CountryLvGroup NomarlGroup { get; set; }

        /// <summary>
        /// 高级组40级以上
        /// </summary>
        internal CountryLvGroup AdvancedGroup { get; set; }
    }

    /// <summary>
    /// 等级分组
    /// </summary>
    internal class CountryLvGroup
    {
        internal CountryLvGroup()
        {
            CombaQueue = new CountryQueue();
            PairCombatQueue = new CountryQueue();
            UserList = new Dictionary<string, CountryUser>();
        }
        internal int UserCount
        {
            get { return UserList.Count; }
        }

        internal Dictionary<string, CountryUser> UserList { get; private set; }

        internal CountryUser Get(string userId)
        {
            if (UserList.ContainsKey(userId))
            {
                return UserList[userId];
            }
            return null;
        }

        internal void Remove(string userId)
        {
            if (UserList.ContainsKey(userId))
            {
                UserList.Remove(userId);
            }
        }

        internal void Update(CountryUser cuser)
        {
            if (UserList.ContainsKey(cuser.UserId))
            {
                UserList[cuser.UserId] = cuser;
            }
        }

        /// <summary>
        /// 参战队列
        /// </summary>
        internal CountryQueue CombaQueue { get; set; }


        /// <summary>
        /// 等待配对战斗
        /// </summary>
        internal CountryQueue PairCombatQueue { get; set; }

    }

    /// <summary>
    /// 国家领土战
    /// </summary>
    public class CountryCombat
    {


        /// <summary>
        /// 战斗间隔触发时间
        /// </summary>
        private static int CombatInterval = ConfigEnvSet.GetInt("CountryCombat.Interval");
        /// <summary>
        /// VIP3等级战斗失败后自动加入
        /// </summary>
        private static int CombatAutoJoinVipLv = ConfigEnvSet.GetInt("CountryCombat.AutoJoinVipLv");
        /// <summary>
        /// 晶石鼓舞VIP等级
        /// </summary>
        public static int CombatInspireVipLv = ConfigEnvSet.GetInt("CountryCombat.InspireVipLv");
        private static int ObtainNum = ConfigEnvSet.GetInt("CountryCombat.ObtainNum");
        private static int PrizeGameCoin = ConfigEnvSet.GetInt("CountryCombat.GameCoin");
        private static int GameCoinMax = PrizeGameCoin + 20 * 1000;
        public static double InspirePercent = ConfigEnvSet.GetDouble("CountryCombat.InspirePercent");//鼓舞概率
        public static double InspireIncrease = ConfigEnvSet.GetDouble("CountryCombat.InspireIncrease");//鼓舞增加效果

        private static Timer _combatTimer;
        //private static Timer _waitCombatTimer;
        private static GameActive _gameActive;
        private static BaseLog _log = new BaseLog();
        private static object thisLock = new object();
        private const int AdvLv = 40;
        //private static AsyncCombatHandle asyncCombat = new AsyncCombatHandle(AsyncDoCombat);
        private static Dictionary<CountryType, CountryGroup> _countryGroupDict;
        /// <summary>
        /// 个人战报
        /// </summary>
        private static List<CountryCombatProcess> _combatProcessList;
        private static bool isOver = false;
        //public static CountryGroup HashideGroup { get; set; }

        private CountryUser _countryUser;

        /// <summary>
        /// 连胜最高玩家
        /// </summary>
        public static CountryUser FistCountryUser
        {
            get;
            private set;
        }

        public GameActive GameActive
        {
            get { return _gameActive; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        internal static void Init(GameActive gameActive)
        {
            //Trace.WriteLine("领土战初化");
            _gameActive = gameActive;
            isOver = false;
            FistCountryUser = new CountryUser();
            _countryGroupDict = new Dictionary<CountryType, CountryGroup>(2);
            var mogemaGroup = new CountryGroup(CountryType.M);
            var hashideGroup = new CountryGroup(CountryType.H);
            _countryGroupDict.Add(mogemaGroup.CountryType, mogemaGroup);
            _countryGroupDict.Add(hashideGroup.CountryType, hashideGroup);
            CountryCombatProcess.RestVersion();
            _combatProcessList = new List<CountryCombatProcess>();

            _combatTimer = new Timer(DoCombat, gameActive, 60000, 17 * 1000);
            //_waitCombatTimer = new Timer(DoWaitCombat, gameActive, 60000, 1 * 1000);
        }

        public static void Stop()
        {
            if (_combatTimer != null)
            {
                _combatTimer.Dispose();
            }
        }

        internal static void Dispose(GameActive gameActive)
        {
            isOver = true;
            CountryGroup mogemaGroup;
            CountryGroup hashideGroup;
            if (TryGroup(CountryType.M, out mogemaGroup))
            {
                SendMessage(mogemaGroup.NomarlGroup);
                SendMessage(mogemaGroup.AdvancedGroup);
            }
            if (TryGroup(CountryType.H, out hashideGroup))
            {
                SendMessage(hashideGroup.NomarlGroup);
                SendMessage(hashideGroup.AdvancedGroup);
            }
            FistCountryUser = null;
            _countryGroupDict.Clear();
            _countryGroupDict = null;
            _combatProcessList = null;
            _combatTimer.Dispose();
            _combatTimer = null;
            GC.Collect();
        }

        private readonly static object combatLock = new object();
        private static bool isTriggCombat = false;

        private static void DoCombat(object state)
        {
            try
            {
                if (!isTriggCombat)
                {
                    lock (combatLock)
                    {
                        if (!isTriggCombat)
                        {
                            if (!isOver)
                            {
                                ThreadPool.QueueUserWorkItem(TriggerNomarlCombat, state);
                                ThreadPool.QueueUserWorkItem(TriggerAdvancedCombat, state);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("领土战领土战线程出现问题:{0}", ex);
            }
        }


        private static void SendMessage(CountryLvGroup lvGroup)
        {
            List<KeyValuePair<string, CountryUser>> cuserList = lvGroup.UserList.ToList();

            var chatService = new TjxChatService();
            foreach (KeyValuePair<string, CountryUser> keyPair in cuserList)
            {
                CountryUser cuser = keyPair.Value;
                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(cuser.UserId);
                if (gameUser != null)
                {
                    gameUser.UserStatus = UserStatus.Normal;
                    gameUser.GroupType = CountryType.None;
                    //领土战礼包
                    UserItemHelper.AddUserItem(gameUser.UserID, 5013, 1);
                    CombatHelper.EmbattlePostion(gameUser.UserID);
                }
                string content = string.Format(LanguageManager.GetLang().St5204_CombatTransfusion,
                    cuser.WinCount, cuser.FailCount, cuser.GameCoin, cuser.ObtainNum);
                chatService.SystemSendWhisper(cuser.UserId, cuser.UserName, cuser.UserVipLv, content);
            }
        }

        public CountryCombat(GameUser user)
        {
            _countryUser = new CountryUser()
            {
                UserId = user.UserID,
                UserName = user.NickName,
                UserLv = user.UserLv,
                UserVipLv = (short)user.VipLv,
                UserExpNum = user.ExpNum,
                Refresh = DateTime.Now,
                InspirePercent = 0,
                Status = 0,
                IsAdvanced = user.UserLv >= AdvLv
            };
            if (user.GroupType != CountryType.None)
            {
                _countryUser.GroupType = user.GroupType;
            }
        }

        /// <summary>
        /// 分配国家阵营
        /// </summary>
        private void AssignGoup()
        {
            GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(_countryUser.UserId);
            if (gameUser == null) return;
            CountryGroup mogemaGroup;
            CountryGroup hashideGroup;
            if (TryGroup(CountryType.M, out mogemaGroup) && TryGroup(CountryType.H, out hashideGroup))
            {
                CountryType groupType;
                if (gameUser.GroupType == CountryType.None)
                {
                    #region 分配

                    if (gameUser.GroupType == CountryType.None)
                    {
                        if (_countryUser.IsAdvanced)
                        {
                            groupType = mogemaGroup.AdvancedGroup.UserCount < hashideGroup.AdvancedGroup.UserCount
                                            ? CountryType.M
                                            : CountryType.H;

                        }
                        else
                        {
                            groupType = mogemaGroup.NomarlGroup.UserCount < hashideGroup.NomarlGroup.UserCount
                                            ? CountryType.M
                                            : CountryType.H;
                        }
                        gameUser.GroupType = groupType;
                        _countryUser.GroupType = gameUser.GroupType;
                    }
                    #endregion
                }
                if (gameUser.GroupType == CountryType.None) return;
                gameUser.UserStatus = UserStatus.CountryCombat;

                var countryGroup = _countryUser.GroupType == CountryType.M ? mogemaGroup : hashideGroup;
                if (_countryUser.IsAdvanced && countryGroup.NomarlGroup.UserList.ContainsKey(_countryUser.UserId))
                {
                    countryGroup.NomarlGroup.UserList.Remove(_countryUser.UserId);
                }
                CountryLvGroup lvGroup;
                if (TryLvGroup(out lvGroup) && !lvGroup.UserList.ContainsKey(_countryUser.UserId))
                {
                    lock (thisLock)
                    {
                        if (!lvGroup.UserList.ContainsKey(_countryUser.UserId))
                        {
                            lvGroup.UserList.Add(_countryUser.UserId, _countryUser);
                        }
                    }
                }

            }
        }


        /// <summary>
        /// 分为1：先功团，2：护卫队两个阵营
        /// </summary>
        /// <returns></returns>
        public static bool TryGroup(CountryType groupType, out CountryGroup group)
        {
            group = null;
            if (_countryGroupDict != null && _countryGroupDict.ContainsKey(groupType))
            {
                group = _countryGroupDict[groupType];
                return true;
            }
            return false;
        }

        private bool TryLvGroup(out CountryLvGroup lvGroup)
        {
            return TryLvGroup(_countryUser, out lvGroup);
        }

        private static bool TryLvGroup(CountryUser cuser, out CountryLvGroup lvGroup)
        {
            lvGroup = null;
            CountryGroup group;
            if (cuser != null && TryGroup(cuser.GroupType, out group))
            {
                lvGroup = cuser.IsAdvanced ? group.AdvancedGroup : group.NomarlGroup;
                return true;
            }
            return false;
        }


        /// <summary>
        /// 离开主界面
        /// </summary>
        public void Exit()
        {
            CountryLvGroup lvGroup;
            if (TryLvGroup(out lvGroup) && lvGroup.UserList.ContainsKey(_countryUser.UserId))
            {
                CountryUser cuser = lvGroup.UserList[_countryUser.UserId];
                cuser.CurrWinNum = 0;
                cuser.Status = 0; //未参战状态

                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(_countryUser.UserId);
                if (gameUser != null)
                {
                    gameUser.UserStatus = UserStatus.Normal;
                    //gameUser.Update();
                }
            }
        }

        /// <summary>
        /// 获取玩家战斗信息
        /// </summary>
        /// <returns></returns>
        public CountryUser GetCountryUser()
        {
            CountryUser countryUser = new CountryUser();
            CountryLvGroup lvGroup;
            if (TryLvGroup(out lvGroup) && lvGroup.UserList.ContainsKey(_countryUser.UserId))
            {
                countryUser = lvGroup.UserList[_countryUser.UserId];
            }
            return countryUser;
        }

        /// <summary>
        /// 获取国家的参战人员
        /// </summary>
        /// <param name="countryType"></param>
        /// <param name="lv">1:40级内,2:40级发上</param>
        /// <returns></returns>
        public List<CountryUser> GetCountryUser(CountryType countryType, short lv)
        {
            List<CountryUser> list = new List<CountryUser>();
            CountryGroup countryGroup;
            if (TryGroup(countryType, out countryGroup))
            {
                List<CountryLvGroup> lvGroupList = new List<CountryLvGroup>();
                if (lv == 0)
                {
                    lvGroupList.Add(countryGroup.NomarlGroup);
                    lvGroupList.Add(countryGroup.AdvancedGroup);
                }
                if (lv == 2)
                {
                    lvGroupList.Add(countryGroup.AdvancedGroup);
                }
                else
                {
                    lvGroupList.Add(countryGroup.NomarlGroup);
                }
                foreach (var lvGroup in lvGroupList)
                {
                    List<KeyValuePair<string, CountryUser>> cuserList = lvGroup.UserList.ToList();

                    foreach (KeyValuePair<string, CountryUser> keyPair in cuserList)
                    {
                        CountryUser cuser = keyPair.Value;
                        if (!list.Exists(m => m.UserId.Equals(cuser.UserId)))
                        {
                            list.Add(cuser);
                        }
                    }

                }

            }
            return list;
        }

        /// <summary>
        /// 参战
        /// </summary>
        public bool JoinCombat()
        {
            AssignGoup();

            bool result = true;
            CountryLvGroup lvGroup;
            if (TryLvGroup(out lvGroup) && lvGroup.UserList.ContainsKey(_countryUser.UserId))
            {
                CountryUser cuser = lvGroup.UserList[_countryUser.UserId];

                if (cuser != null && cuser.Status == 0)
                {
                    cuser.Status = 1;
                    cuser.Refresh = DateTime.Now;
                    //lvGroup.Update(cuser);
                    lvGroup.CombaQueue.Add(cuser);
                    CombatHelper.EmbattlePostion(cuser.UserId);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 鼓舞
        /// </summary>
        /// <param name="isUseGold">是否使用晶石</param>
        /// <param name="percent"></param>
        public bool Inspire(bool isUseGold, out double percent)
        {
            AssignGoup();
            bool result = false;
            percent = 0;

            CountryLvGroup lvGroup;
            if (TryLvGroup(out lvGroup) && lvGroup.UserList.ContainsKey(_countryUser.UserId))
            {
                CountryUser countryUser = lvGroup.UserList[_countryUser.UserId];

                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(_countryUser.UserId);
                if (gameUser != null && gameUser.UserStatus == UserStatus.CountryCombat)
                {
                    if (countryUser.InspirePercent < 1 && (isUseGold || RandomUtils.IsHit(InspirePercent)))
                    {
                        result = true;
                        countryUser.InspirePercent = MathUtils.Addition(countryUser.InspirePercent, InspireIncrease, 1);
                    }
                    percent = countryUser.InspirePercent;

                }
            }
            return result;
        }

        /// <summary>
        /// 获得战斗过程
        /// </summary>
        /// <param name="isSelf"></param>
        /// <returns></returns>
        public List<CountryCombatProcess> GetCombatProcess(bool isSelf)
        {
            List<CountryCombatProcess> list = new List<CountryCombatProcess>();
            CountryUser countryUser = new CountryUser();
            int version = 0;
            CountryLvGroup lvGroup;
            if (TryLvGroup(out lvGroup))
            {
                if (lvGroup.UserList.ContainsKey(_countryUser.UserId))
                {
                    countryUser = lvGroup.UserList[_countryUser.UserId];
                    version = countryUser.Version;
                }
                if (isSelf && _combatProcessList != null)
                {
                    list = _combatProcessList.FindAll(
                            m => (m.WinUserId == _countryUser.UserId || m.FailUserId == _countryUser.UserId) && m.Version > version);
                }
                else if (_combatProcessList != null)
                {
                    list = _combatProcessList.FindAll(m => m.Version > version);
                }

                foreach (var process in list)
                {
                    if (process.Version > countryUser.Version)
                    {
                        countryUser.Version = process.Version;
                        //lvGroup.Update(countryUser);
                    }
                }
            }
            return list;
        }


        private static List<CountryUser> FilterCombatUsers(CountryLvGroup lvGroup)
        {
            var list = new List<CountryUser>();
            var removeList = new List<CountryUser>();

            while (lvGroup.PairCombatQueue.Count > 0)
            {
                CountryUser cuser = lvGroup.PairCombatQueue.Get();
                if (cuser == null) continue;

                GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(cuser.UserId);
                if (gameUser != null && gameUser.UserStatus == UserStatus.CountryCombat)
                {
                    cuser.Status = 2;
                    list.Add(cuser);
                }
            }
            while (lvGroup.CombaQueue.Count > 0)
            {
                CountryUser cuser = lvGroup.CombaQueue.Get();
                if (cuser == null) continue;

                if (cuser.Status == 1 && cuser.Refresh.AddSeconds(CombatInterval) <= DateTime.Now)
                {
                    GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(cuser.UserId);
                    if (gameUser != null && gameUser.UserStatus == UserStatus.CountryCombat)
                    {
                        cuser.Status = 2;
                        list.Add(cuser);
                    }
                }
                else if (cuser.Status == 1)
                {
                    removeList.Add(cuser);
                }
            }
            foreach (var countryUser in removeList)
            {
                lvGroup.CombaQueue.Add(countryUser);
            }
            return list;
        }

        /// <summary>
        /// 触发战斗
        /// </summary>
        private static void TriggerNomarlCombat(object state)
        {
            try
            {
                CountryGroup mogemaGroup;
                CountryGroup hashideGroup;
                if (TryGroup(CountryType.M, out mogemaGroup) && TryGroup(CountryType.H, out hashideGroup))
                {
                    Stopwatch watck = new Stopwatch();
                    watck.Start();
                    var mogemaUserList = FilterCombatUsers(mogemaGroup.NomarlGroup);
                    var hashideUserList = FilterCombatUsers(hashideGroup.NomarlGroup);
                    TriggerPairCombat(mogemaUserList, hashideUserList);
                    watck.Stop();
                    _log.SaveDebugLog(string.Format("领土战普通组配对战斗莫尔码{0},哈{1},持续时间{2}ms",
                        mogemaUserList.Count, hashideUserList.Count, watck.ElapsedMilliseconds));
                }
            }
            catch (Exception ex)
            {
                _log.SaveLog("领土战普通组配对", ex);
            }
        }
        private static void TriggerAdvancedCombat(object state)
        {
            try
            {
                CountryGroup mogemaGroup;
                CountryGroup hashideGroup;
                if (TryGroup(CountryType.M, out mogemaGroup) && TryGroup(CountryType.H, out hashideGroup))
                {
                    Stopwatch watck = new Stopwatch();
                    watck.Start();
                    var mogemaUserList = FilterCombatUsers(mogemaGroup.AdvancedGroup);
                    var hashideUserList = FilterCombatUsers(hashideGroup.AdvancedGroup);
                    TriggerPairCombat(mogemaUserList, hashideUserList);

                    watck.Stop();
                    _log.SaveDebugLog(string.Format("领土战高级组配对战斗莫尔码{0},哈{1},持续时间{2}ms",
                        mogemaUserList.Count, hashideUserList.Count, watck.ElapsedMilliseconds));
                }
            }
            catch (Exception ex)
            {
                _log.SaveLog("领土战高级组配对", ex);
            }
        }

        /// <summary>
        /// 配对战斗
        /// </summary>
        private static void TriggerPairCombat(List<CountryUser> mogemaUserList, List<CountryUser> hashideUserList)
        {
            int combatCount = 0;
            List<CountryUser> tempList = new List<CountryUser>();
            if (mogemaUserList.Count > hashideUserList.Count)
            {
                combatCount = hashideUserList.Count;
                tempList = mogemaUserList;
            }
            else
            {
                combatCount = mogemaUserList.Count;
                tempList = hashideUserList;
            }

            //轮空的
            for (int i = combatCount; i < tempList.Count; i++)
            {
                CountryUser cuser = tempList[i];
                CountryLvGroup tempGroup;
                if (TryLvGroup(cuser, out tempGroup))
                {
                    tempGroup.PairCombatQueue.Add(cuser);
                }
            }

            for (int i = 0; i < combatCount; i++)
            {
                CountryCombatProcess process = new CountryCombatProcess();

                CountryUser cuser1 = mogemaUserList[i];
                int toIndex = i + 1;
                CountryUser cuser2 = toIndex < hashideUserList.Count ? hashideUserList[toIndex] : hashideUserList[0];
                if (cuser1 == null || cuser2 == null) continue;
                if (cuser1.UserId.Equals(cuser2.UserId)) continue;

                //配对状态
                _log.SaveDebugLog(string.Format("领土战[{0}]配对[{1}]战斗", cuser1.UserName, cuser2.UserName));
                AsyncDoCombat(cuser1, cuser2, process);
            }
        }


        private static void AsyncDoCombat(CountryUser cuser1, CountryUser cuser2, CountryCombatProcess process)
        {
            CountryLvGroup userLvGroup1;
            CountryLvGroup userLvGroup2;
            if (TryLvGroup(cuser1, out userLvGroup1) && TryLvGroup(cuser2, out userLvGroup2))
            {
                ISingleCombat combater = CombatFactory.TriggerTournament(cuser1, cuser2);
                bool isWin = combater.Doing();
                if (isWin)
                {
                    ProcessPrize(cuser1, cuser2, process);
                    cuser1.Refresh = DateTime.Now;
                    cuser1.Status = 1;
                    userLvGroup1.CombaQueue.Add(cuser1);
                    CountryGroup userGroup1;
                    if (TryGroup(cuser1.GroupType, out userGroup1)) userGroup1.Score++;
                    cuser2.Refresh = DateTime.Now;
                    cuser2.Status = 1;
                    userLvGroup2.CombaQueue.Add(cuser2);

                }
                else
                {
                    ProcessPrize(cuser2, cuser1, process);
                    cuser2.Refresh = DateTime.Now;
                    cuser2.Status = 1;
                    userLvGroup2.CombaQueue.Add(cuser2);
                    CountryGroup userGroup2;
                    if (TryGroup(cuser2.GroupType, out userGroup2)) userGroup2.Score++;
                    cuser1.Refresh = DateTime.Now;
                    cuser1.Status = 1;
                    userLvGroup1.CombaQueue.Add(cuser1);

                }

                process.ProcessContainer = (CombatProcessContainer)combater.GetProcessResult();

                lock (thisLock)
                {
                    if (_combatProcessList != null) _combatProcessList.Add(process);
                    //日志
                    UserCombatLog log = new UserCombatLog();
                    log.CombatLogID = Guid.NewGuid().ToString();
                    log.UserID = cuser1.UserId;
                    log.CityID = 0;
                    log.PlotID = 0;
                    log.NpcID = 0;
                    log.CombatType = CombatType.Country;
                    log.HostileUser = cuser2.UserId;
                    log.IsWin = isWin;
                    log.CombatProcess = JsonUtils.Serialize(process);
                    log.CreateDate = DateTime.Now;
                    var sender = DataSyncManager.GetDataSender();
                    sender.Send(log);
                }
            }
        }
        //领土战赢杀人不留痕,败纸鸢线断,连胜1奖声10,金币7000,最高连胜杀人不留痕:1,失败方奖声1,金币500

        private static void ProcessPrize(CountryUser wincuser, CountryUser failcuser, CountryCombatProcess process)
        {
            string logStr = string.Format("领土战赢{0}[连{2}],败{1}[连{3}]", wincuser.UserName, failcuser.UserName, wincuser.CurrWinNum, failcuser.CurrWinNum);
            //胜方自动加入下次战斗
            int obtainNum = 0;
            int gameCoin = 0;
            wincuser.WinCount = wincuser.WinCount + 1;
            wincuser.CurrWinNum = wincuser.CurrWinNum + 1;
            //连胜奖励：声望=M*（X+1），金币=N+X*2000
            obtainNum = ObtainNum * wincuser.CurrWinNum;
            gameCoin = PrizeGameCoin + (wincuser.CurrWinNum - 1) * 2000;
            logStr += string.Format(",连胜{2}奖声{0},金币{1}", obtainNum, gameCoin, wincuser.CurrWinNum);

            if (failcuser.CurrWinNum > 0)
            {
                //击杀连胜额外:声望=M*X，金币=N+X*1000
                obtainNum += ObtainNum * failcuser.CurrWinNum;
                gameCoin += PrizeGameCoin + failcuser.CurrWinNum * 1000;
                logStr += string.Format(",击杀连胜后声{0},金币{1}", obtainNum, gameCoin);
                //Contribution(wincuser.UserId, obtainNum); //公会贡献
            }
            if (FistCountryUser != null && wincuser.UserId.Equals(FistCountryUser.UserId))
            {
                //最高连胜额外:N+X（最高连胜次数）*2000
                gameCoin += PrizeGameCoin + (wincuser.CurrWinNum - 1) * 2000;
                logStr += string.Format(",最高连胜后金币{0}", gameCoin);
            }
            if (wincuser.CurrWinNum > wincuser.MaxWinNum)
            {
                wincuser.MaxWinNum = wincuser.CurrWinNum;
            }
            //最高连胜
            if (FistCountryUser != null && wincuser.CurrWinNum > FistCountryUser.CurrWinNum)
            {
                lock (thisLock)
                {
                    FistCountryUser = new CountryUser();
                    FistCountryUser.CurrWinNum = wincuser.CurrWinNum;
                    FistCountryUser.MaxWinNum = wincuser.MaxWinNum;
                    FistCountryUser.WinCount = wincuser.WinCount;
                    FistCountryUser.UserId = wincuser.UserId;
                    FistCountryUser.UserName = wincuser.UserName;
                    FistCountryUser.UserVipLv = wincuser.UserVipLv;
                }

                logStr += string.Format(",最高连胜{0}:{1}", FistCountryUser.UserName, FistCountryUser.CurrWinNum);
                //Trace.WriteLine(string.Format("领土战最高连胜{0}:{1}", FistCountryUser.UserName, FistCountryUser.CurrWinNum));
            }
            SetUserPrize(wincuser, obtainNum, gameCoin);
            wincuser.Status = 1;
            process.WinUserId = wincuser.UserId;
            process.WinUserName = wincuser.UserName;
            process.KillNum = wincuser.CurrWinNum;
            process.WinObtainNum = obtainNum;
            process.WinGameCoin = gameCoin;
            process.FaildKillNum = failcuser.CurrWinNum;//打败几连杀
            //获胜方公会贡献
            Contribution(wincuser.UserId, obtainNum);

            obtainNum = (int)Math.Floor((double)ObtainNum / 5);
            gameCoin = (int)Math.Floor((double)PrizeGameCoin / 10);

            logStr += string.Format(",失败方[{2}连杀]奖声{0},金币{1}", obtainNum, gameCoin, process.FaildKillNum);
            failcuser.Status = 0;
            failcuser.CurrWinNum = 0;
            failcuser.FailCount = failcuser.FailCount + 1;
            SetUserPrize(failcuser, obtainNum, gameCoin);

            process.FailUserId = failcuser.UserId;
            process.FailUserName = failcuser.UserName;
            process.FailObtainNum = obtainNum;
            process.FailGameCoin = gameCoin;
            //战败方公会贡献
            Contribution(failcuser.UserId, obtainNum);
            _log.SaveDebugLog(logStr);
            CombatHelper.EmbattlePostion(failcuser.UserId);
        }

        private static void SetUserPrize(CountryUser cuser, int obtainNum, int gameCoin)
        {
            cuser.ObtainNum += obtainNum;
            cuser.GameCoin += gameCoin;
            GameUser user = new GameDataCacheSet<GameUser>().FindKey(cuser.UserId);
            if (user != null)
            {
                user.ObtainNum = MathUtils.Addition(user.ObtainNum, obtainNum, int.MaxValue);
                user.GameCoin = MathUtils.Addition(user.GameCoin, gameCoin, int.MaxValue);
                //user.Update();
            }
            else
            {
                _log.SaveLog(new Exception("领土战" + cuser.UserId + "获得奖励异常"));
            }
        }

        /// <summary>
        /// 公会贡献
        /// </summary>
        public static void Contribution(string userID, int experience)
        {
            var memberArray = new ShareCacheStruct<GuildMember>().FindAll(m => m.UserID == userID);
            if (memberArray.Count > 0)
            {
                GuildMember member = memberArray[0];
                if (DateTime.Now.Date == member.RefreshDate.Date)
                {
                    member.Contribution = MathUtils.Addition(member.Contribution, experience, int.MaxValue);
                }
                else
                {
                    member.Contribution = experience;
                }
                member.TotalContribution = MathUtils.Addition(member.TotalContribution, experience, int.MaxValue);
                //member.Update();

                UserGuild userGuild = new ShareCacheStruct<UserGuild>().FindKey(member.GuildID);
                if (userGuild != null)
                {
                    GuildLvInfo lvInfo = new ConfigCacheSet<GuildLvInfo>().FindKey(MathUtils.Addition(userGuild.GuildLv, 1, int.MaxValue));
                    userGuild.CurrExperience = MathUtils.Addition(userGuild.CurrExperience, experience, int.MaxValue);
                    if (lvInfo != null)
                    {
                        if (userGuild.CurrExperience >= lvInfo.UpExperience)
                        {
                            userGuild.CurrExperience = MathUtils.Subtraction(userGuild.CurrExperience, lvInfo.UpExperience, 0);
                            userGuild.GuildLv = (short)MathUtils.Addition(userGuild.GuildLv, 1, int.MaxValue);
                        }
                    }
                    //userGuild.Update();
                }

                GuildMemberLog.AddLog(member.GuildID, new MemberLog
                {
                    UserID = userID,
                    IdolID = 0,
                    LogType = 1,
                    GainObtion = experience,
                    Experience = experience,
                    GainAura = 0,
                    InsertDate = DateTime.Now
                });
            }
        }

    }
}