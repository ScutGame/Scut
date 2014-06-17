using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 牌组
    /// </summary>
    [Serializable, ProtoContract]
    public class CardGroup
    {
        private const int MaxLength = 20;
        /// <summary>
        /// 几张数量才是炸弹
        /// </summary>
        private const int BombMinNum = 4;
        private List<int>[] _list;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packNum">几副牌</param>
        public CardGroup(int packNum = 1)
        {
            _list = new List<int>[MaxLength];
            int count = packNum * BombMinNum;
            for (int i = 0; i < MaxLength; i++)
            {
                _list[i] = new List<int>(count);
            }
        }

        public int Count
        {
            get { return _list.Length; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardSize"></param>
        /// <returns></returns>
        public int[] GetSame(int cardSize)
        {
            return GetSame((CardSize)cardSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardSize"></param>
        /// <returns></returns>
        public int[] GetSame(CardSize cardSize)
        {
            return _list[(int)cardSize].ToArray();
        }

        /// <summary>
        /// 返回第一个
        /// </summary>
        /// <param name="count">相同的个数</param>
        /// <returns></returns>
        public int[] FindSame(int count)
        {
            foreach (var item in _list)
            {
                if (item.Count == count)
                {
                    return item.ToArray();
                }
            }
            return new int[0];
        }

        /// <summary>
        /// 查找相同个数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int[] FindSameCount(int count)
        {
            List<int> sizeList = new List<int>();
            for (int i = 0; i < _list.Length; i++)
            {
                var item = _list[i];
                if (item.Count == count)
                {
                    sizeList.Add(i);
                }
            }
            return sizeList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card">牌Id</param>
        /// <param name="cardSize">牌大小</param>
        public void Add(int card, CardSize cardSize)
        {
            _list[(int)cardSize].Add(card);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardSize"></param>
        /// <returns></returns>
        public int GetSameSizeCount(int cardSize)
        {
            return GetSame(cardSize).Length;
        }
        public int GetSameSizeCount(CardSize cardSize)
        {
            return GetSame(cardSize).Length;
        }
    }
}
