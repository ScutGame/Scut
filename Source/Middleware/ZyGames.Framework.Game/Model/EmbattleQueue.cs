using System;
using System.Collections.Generic;
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Game.Model
{
    public enum EmbattleRole
    {
        RoleA,
        RoleD
    }
    /// <summary>
    /// 九宫格阵形队列
    /// </summary>
    public abstract class EmbattleQueue
    {
        public static int[,] PriorityMapList = new[,]{
            {3, 6, 9, 2, 5, 8, 1, 4, 7},
            {1, 4, 7, 2, 5, 8, 3, 6, 9},
            {2, 5, 8, 1, 4, 7, 3, 6, 9}
        };
        private const int MaxNumber = 3;
        private readonly IGeneral[] _generalList = new IGeneral[9];
        private readonly Queue<int> _generalQueue = new Queue<int>();
        private readonly Queue<IGeneral> _waitQueue = new Queue<IGeneral>();

        protected void SetQueue(int index, IGeneral general)
        {
            if (index < 0 || index > _generalList.Length - 1)
            {
                return;
            }
            if (general != null && general.IsWait)
            {
                _waitQueue.Enqueue(general);
            }
            else
            {
                _generalList[index] = general;
            }
            GeneralCount++;
        }

        public IGeneral[] Items
        {
            get
            {
                return _generalList;
            }
        }

        public int[] ToQueue()
        {
            int[] temp = new int[_generalQueue.Count];
            _generalQueue.CopyTo(temp, 0);
            return temp;
        }

        public EmbattleRole Role
        {
            get;
            set;
        }

        public bool IsOver
        {
            get;
            private set;
        }
        /// <summary>
        /// 优先值
        /// </summary>
        public int PriorityNum
        {
            get;
            set;
        }

        public int GeneralCount
        {
            get;
            set;
        }
        /// <summary>
        /// 当前轮次数
        /// </summary>
        public int Number
        {
            get;
            set;
        }

        public IGeneral PeekWaitGeneral()
        {
            if (_waitQueue.Count > 0)
            {
                return _waitQueue.Peek();
            }
            return null;
        }
        /// <summary>
        /// 获取下一个可战斗佣兵
        /// </summary>
        /// <returns></returns>
        public virtual IGeneral NextGeneral()
        {
            IGeneral general = null;

            GeneralCount = _generalQueue.Count;
            while (_generalQueue.Count > 0)
            {
                int index = _generalQueue.Dequeue();
                var temp = _generalList[index];
                if (temp != null && temp.LifeNum > 0)
                {
                    general = temp;
                    break;
                }
            }
            return general;
        }

        /// <summary>
        /// 替换等待的佣兵
        /// </summary>
        /// <param name="position">替换的位置</param>
        /// <param name="waitGeneral"></param>
        public bool ReplaceWaitGeneral(int position, out IGeneral waitGeneral)
        {
            waitGeneral = null;
            position = position < 0 ? 0 : position;
            if (_waitQueue.Count > 0)
            {
                var general = _waitQueue.Dequeue();
                if (general.LifeNum > 0)
                {
                    general.IsWait = false;
                    if (position != 0)
                    {
                        general.Position = position;
                    }
                    int index = general.Position - 1;
                    if (index > -1 && index < _generalList.Length)
                    {
                        _generalList[index] = general;
                        waitGeneral = general;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 本轮是否存在佣兵
        /// </summary>
        public bool HasGeneral { get { return _generalQueue.Count > 0; } }

        /// <summary>
        /// 是否可战斗
        /// </summary>
        /// <returns></returns>
        public bool HasCombat()
        {
            foreach (var general in _generalList)
            {
                if (general != null && general.LifeNum > 0)
                {
                    return true;
                }
            }

            IsOver = true;
            return false;
        }

        /// <summary>
        /// 初始战斗入阵
        /// </summary>
        public void ResetGeneralQueue()
        {
            IGeneral waitGeneral = PeekWaitGeneral();
            for (int i = 0; i < _generalList.Length; i++)
            {
                IGeneral general = _generalList[i];
                if (general == null)
                {
                    continue;
                }
                if (general.LifeNum > 0)
                {
                    _generalQueue.Enqueue(i);
                }
            }
            Number++;
        }


        /// <summary>
        /// 随机取
        /// </summary>
        /// <returns></returns>
        public virtual IGeneral RandomGeneral()
        {
            IGeneral general = null;
            IGeneral[] generalList = FindAll();
            if (generalList.Length > 0)
            {
                general = generalList[RandomUtils.GetRandom(0, generalList.Length)];
            }
            return general;
        }

        /// <summary>
        /// 根据位置取
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual IGeneral GetGeneral(int position)
        {
            IGeneral general = null;
            if (position > _generalList.Length || position < 1)
            {
                return general;
            }
            int index = position - 1;
            if (_generalList[index] != null && _generalList[index].LifeNum > 0)
            {
                general = _generalList[index];
            }
            return general;
        }
        /// <summary>
        /// 获取被攻击将领通过设定的优先级查找
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual IGeneral FindGeneral(int position)
        {
            IGeneral general = null;
            if (position > _generalList.Length || position < 1)
            {
                return general;
            }
            int rowIndex = position % MaxNumber;
            int columNum = PriorityMapList.GetLength(1);
            for (int i = 0; i < columNum; i++)
            {
                int index = PriorityMapList[rowIndex, i] - 1;
                if (_generalList[index] != null && _generalList[index].LifeNum > 0)
                {
                    general = _generalList[index];
                    break;
                }
            }
            return general;
        }

        /// <summary>
        /// 找横向
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual IGeneral[] FindHorizontal(int position)
        {
            List<IGeneral> generalList = new List<IGeneral>();
            if (position <= _generalList.Length && position > 0)
            {
                int rowIndex = position % MaxNumber;
                int columNum = PriorityMapList.GetLength(1);

                int loopNum = 0;
                for (int i = 0; i < columNum; i++)
                {
                    if (loopNum >= MaxNumber)
                    {
                        if (generalList.Count > 0)
                        {
                            break;
                        }
                        loopNum = 0;
                    }
                    int index = PriorityMapList[rowIndex, i] - 1;
                    if (_generalList[index] != null && _generalList[index].LifeNum > 0)
                    {
                        generalList.Add(_generalList[index]);
                    }
                    loopNum++;
                }
            }
            return generalList.ToArray();
        }

        /// <summary>
        /// 找纵向
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual IGeneral[] FindVertical(int position)
        {
            List<IGeneral> generalList = new List<IGeneral>();
            if (position > _generalList.Length && position <= 0)
            {
                return generalList.ToArray();
            }
            IGeneral general = FindGeneral(position);
            if (general != null)
            {
                int columnIndex = (general.Position - 1) / MaxNumber;
                for (int i = 0; i < MaxNumber; i++)
                {
                    int index = (columnIndex * MaxNumber) + i;
                    if (_generalList[index] != null && _generalList[index].LifeNum > 0)
                    {
                        generalList.Add(_generalList[index]);
                    }
                }
            }

            return generalList.ToArray();
        }

        public virtual IGeneral[] FindAll()
        {
            return FindAll(false);
        }

        /// <summary>
        /// 找所有
        /// </summary>
        /// <returns></returns>
        public virtual IGeneral[] FindAll(bool isAll)
        {
            List<IGeneral> generalList = new List<IGeneral>();
            for (int i = 0; i < _generalList.Length; i++)
            {
                int index = i;
                if (_generalList[index] != null && (isAll || _generalList[index].LifeNum > 0))
                {
                    generalList.Add(_generalList[index]);
                }
            }
            if (isAll && _waitQueue.Count > 0)
            {
                var tempList = new IGeneral[_waitQueue.Count];
                _waitQueue.CopyTo(tempList, 0);
                generalList.AddRange(tempList);
            }
            return generalList.ToArray();
        }
    }
}
