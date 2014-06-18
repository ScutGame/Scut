using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ProtoBuf;
using ZyGames.Framework.Common;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 桌子对象
    /// </summary>
    [Serializable, ProtoContract]
    public class TableData : BaseDisposable
    {
        private const int PackMaxNum = 54;
        /// <summary>
        /// 底牌数
        /// </summary>
        public const int CardBackNum = 3;

        /// <summary>
        /// 叫地主或出牌操作超时时间(30秒)
        /// </summary>
        private const int OperationSecTimeout = 30000;

        private readonly int _roomId;
        private int _tableId;
        private PositionData[] _positions;
        private int _playerNum;
        private int _packNum;
        private int _anteNum;
        private int _multipleNum;
        private int _currAnteNum;
        private int _currMultipleNum;
        private List<int> _cardList;
        private List<int> _backCardData;
        private List<CardData> _outCardList;
        public bool[] _callOperation;
        private Timer _timer;
        private int _timerRunning = 0;
        private int _timerPeriod = 0;
        /// <summary>
        /// 操作计数
        /// </summary>
        private int _timeNumber = 0;


        /// <summary>
        /// 桌子对象
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="tableId"></param>
        /// <param name="playerNum">人数</param>
        /// <param name="anteNum">底注</param>
        /// <param name="multipleNum">倍数</param>
        /// <param name="callback"></param>
        /// <param name="packNum">几副牌</param>
        public TableData(int roomId, int tableId, int playerNum, int anteNum, int multipleNum, TimerCallback callback, int packNum = 1)
        {
            _roomId = roomId;
            _tableId = tableId;
            _playerNum = playerNum;
            _anteNum = anteNum;
            _multipleNum = multipleNum;
            _packNum = packNum;
            _cardList = new List<int>(PackMaxNum * packNum);
            _backCardData = new List<int>();
            _callOperation = new bool[playerNum + 1];
            _outCardList = new List<CardData>();
            _positions = new PositionData[playerNum];
            for (int i = 0; i < playerNum; i++)
            {
                _positions[i] = new PositionData(i);
            }
            _timer = new Timer(callback, this, -1, -1);
            Init();
        }
        #region Timer
        public void StartTimer(int period = 5000)
        {
            //Console.WriteLine("{0}>>Table:{1} in {2} room timer is started", DateTime.Now.ToString("HH:mm:ss"), _tableId, _roomId);
            _timerPeriod = period;
            _timer.Change(period, period);
            _timeNumber = 0;
            IsTimerStarted = true;
        }

        public void ReStartTimer(int period)
        {
            //Console.WriteLine("{0}>>Table:{1} in {2} room timer is restarted", DateTime.Now.ToString("HH:mm:ss"), _tableId, _roomId);
            _timerPeriod = period;
            _timer.Change(-1, -1);
            _timer.Change(period, period);
            _timeNumber = 0;
        }

        public void StopTimer()
        {
           // Console.WriteLine("{0}>>Table:{1} in {2} room timer is stoped", DateTime.Now.ToString("HH:mm:ss"), _tableId, _roomId);
            _timer.Change(-1, -1);
            _timeNumber = 0;
            IsTimerStarted = false;
        }

        /// <summary>
        /// 定时器计数
        /// </summary>
        public void DoTimeNumber()
        {
            Interlocked.Exchange(ref _timeNumber, _timeNumber + _timerPeriod);
        }

        /// <summary>
        /// 操作是否超时
        /// </summary>
        public bool IsOperationTimeout
        {
            get { return _timeNumber > OperationSecTimeout; }
        }

        /// <summary>
        /// 定时器是否开始
        /// </summary>
        public bool IsTimerStarted
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 是否在初始状态
        /// </summary>
        public bool IsIniting
        {
            get
            {
                return _backCardData.Count == 0;
            }
        }
        /// <summary>
        /// 初始化桌面数据
        /// </summary>
        public void Init()
        {
            IsClosed = false;
            _currAnteNum = 0;
            _currMultipleNum = 0;
            _backCardData.Clear();
            _outCardList.Clear();
            Array.Clear(_callOperation, 0, _callOperation.Length);
            CallTimes = 0;
            IsCallEnd = false;
            CallLandlordId = 0;
            CallLandlordName = "";
            LandlordId = 0;
            OutCardPos = 0;
            OutCardUserId = 0;
            IsLandlordWin = false;
            IsFlee = false;
            PreCardData = null;
            CallLandlordPos = 0;
            LandlordPos = 0;
            IsSettlemented = false;
            IsShow = false;

            StopTimer();
        }

        /// <summary>
        /// 加倍
        /// </summary>
        public void DoDouble()
        {
            if (_currMultipleNum == 0)
            {
                _currMultipleNum = _multipleNum;
            }
            else
            {
                _currMultipleNum = _currMultipleNum * 2;
            }
            _currAnteNum = _anteNum * _currMultipleNum;
        }

        /// <summary>
        /// 设置加倍数
        /// </summary>
        /// <param name="num"></param>
        public void SetDouble(int num)
        {
            _currMultipleNum = num;
            _currAnteNum = _anteNum * _currMultipleNum;
        }

        /// <summary>
        /// 是否在游戏中
        /// </summary>
        public bool IsStarting
        {
            get { return !IsSettlemented && _backCardData.Count > 0; }
        }

        public bool IsTimeRunning
        {
            get { return _timerRunning == 1; }
            set { Interlocked.Exchange(ref _timerRunning, value ? 1 : 0); }
        }

        #region property

        /// <summary>
        /// 房间ID
        /// </summary>
        public int RoomId
        {
            get
            {
                return _roomId;
            }
        }
        /// <summary>
        /// 桌子编号
        /// </summary>
        public int TableId
        {
            get { return _tableId; }
        }
        /// <summary>
        /// 底注
        /// </summary>
        public int MinAnteNum
        {
            get { return _anteNum; }
        }
        /// <summary>
        /// 倍数
        /// </summary>
        public int MinMultipleNum
        {
            get { return _multipleNum; }
        }
        /// <summary>
        /// 桌位数据
        /// </summary>
        public PositionData[] Positions
        {
            get { return _positions; }
        }

        /// <summary>
        /// 玩的人数
        /// </summary>
        public int PlayerNum
        {
            get { return _playerNum; }
        }

        /// <summary>
        /// 几副牌
        /// </summary>
        public int PackNum
        {
            get { return _packNum; }
        }

        /// <summary>
        /// 一副牌数据（固定数据）
        /// </summary>
        public List<int> CardData
        {
            get { return _cardList; }
        }

        /// <summary>
        /// 底牌数据
        /// </summary>
        public List<int> BackCardData
        {
            get { return _backCardData; }
        }

        /// <summary>
        /// 当前底分
        /// </summary>
        public int AnteNum
        {
            get { return _currAnteNum; }
        }

        /// <summary>
        /// 当前倍数
        /// </summary>
        public int MultipleNum
        {
            get { return _currMultipleNum; }
        }
        /// <summary>
        /// 叫地主操作,最大4
        /// </summary>
        public bool[] CallOperation
        {
            get { return _callOperation; }
        }

        /// <summary>
        /// 叫地主次数
        /// </summary>
        public int CallTimes { get; set; }
        /// <summary>
        /// 等待叫地主玩家位置
        /// </summary>
        public int CallLandlordPos { get; set; }
        /// <summary>
        /// 等待叫地主玩家Id
        /// </summary>
        public int CallLandlordId { get; set; }

        /// <summary>
        /// 等待叫地主名称
        /// </summary>
        public string CallLandlordName { get; set; }

        /// <summary>
        /// 是否明牌
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 当前地主位置
        /// </summary>
        public int LandlordPos { get; set; }
        /// <summary>
        /// 当前地主
        /// </summary>
        public int LandlordId { get; set; }
        /// <summary>
        /// 是否叫地主结束
        /// </summary>
        public bool IsCallEnd { get; set; }
        /// <summary>
        /// 出牌结束
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// 结束时结算通知次数
        /// </summary>
        //public int CloseTimes { get; set; }

        /// <summary>
        /// 是否地主胜
        /// </summary>
        public bool IsLandlordWin { get; set; }

        /// <summary>
        /// 是否逃跑
        /// </summary>
        public bool IsFlee { get; set; }

        /// <summary>
        /// 上一次出牌
        /// </summary>
        public CardData PreCardData { get; set; }

        /// <summary>
        /// 出牌记录
        /// </summary>
        public List<CardData> OutCardList
        {
            get { return _outCardList; }
        }
        /// <summary>
        /// 出牌玩家
        /// </summary>
        public int OutCardPos { get; set; }

        public int OutCardUserId { get; set; }
        #endregion


        /// <summary>
        /// 结算结束
        /// </summary>
        public bool IsSettlemented { get; set; }

        /// <summary>
        /// 是否被释放
        /// </summary>
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //释放 托管资源 
                _timer.Dispose();
                _timer = null;
                _backCardData = null;
                _outCardList = null;
                _cardList = null;
                IsDisposed = true;
            }
            base.Dispose(disposing);
        }

    }
}
