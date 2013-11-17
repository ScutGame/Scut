using System;
using System.Collections.Generic;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;

namespace ZyGames.Doudizhu.Bll.Logic
{
    /// <summary>
    /// 牌型规则
    /// </summary>
    public class CardRole
    {
        /// <summary>
        /// 获取牌面花色
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public CardColor GetColor(int card)
        {
            return (CardColor)Math.Floor((double)card / 100);
        }

        /// <summary>
        /// 获取牌面大小
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public int GetCardSize(int card)
        {
            return card % 100;
        }

        /// <summary>
        /// 牌面从小到大排序
        /// </summary>
        /// <param name="cards"></param>
        public void SortCard(int[] cards)
        {
            Array.Sort(cards, (a, b) =>
            {
                int result = (a % 100) - (b % 100);
                if (result == 0)
                {
                    result = (a / 100) - (b / 100);
                }
                return result;
            });
        }
        /// <summary>
        /// 牌面从小到大排序
        /// </summary>
        public void SortCard(List<int> cards)
        {
            //面大小排序
            cards.QuickSort((a, b) =>
            {
                int result = (a % 100) - (b % 100);
                if (result == 0)
                {
                    result = (a / 100) - (b / 100);
                }
                return result;
            });
        }

        /// <summary>
        /// 分组牌面大小
        /// [203,403,405,103]=> [0:[],1:[],2:[],3:[203,403,103],4:[],5:[405]]
        /// </summary>
        /// <param name="cards"></param>
        public CardGroup ParseCardGroup(int[] cards)
        {
            int len = cards.Length;
            CardGroup cardGroup = new CardGroup();
            while (len-- > 0)
            {
                int card = cards[len];
                int val = GetCardSize(card);
                cardGroup.Add(card, (CardSize)val);
            }
            return cardGroup;
        }

        /// <summary>
        /// is顺,不能是2和王
        /// </summary>
        /// <param name="cards">需要排序后</param>
        /// <returns></returns>
        public bool IsShuzi(int[] cards)
        {
            var len = cards.Length;
            if (len > 1 && len < 15)
            {
                int val1, val2;

                for (var i = 0; i < len - 1; i++)
                {
                    val1 = GetCardSize(cards[i]);
                    val2 = GetCardSize(cards[i + 1]);
                    if (val1 != val2 - 1 || val1 >= (int)CardSize.C_2 || val2 >= (int)CardSize.C_2)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 连对
        /// </summary>
        /// <param name="cards">需要排序后</param>
        /// <returns></returns>
        public bool IsLiandui(int[] cards)
        {
            if (cards.Length % 2 != 0)
            {
                return false;
            }
            int val = GetCardSize(cards[0]);
            int mLength = cards.Length / 2;
            var arr = new int[2][];
            arr[0] = new int[mLength];
            arr[1] = new int[mLength];
            for (var i = 0; i < cards.Length; i++)
            {
                arr[i % 2][i / 2] = cards[i];
            }
            return val == GetCardSize(arr[1][0]) && IsShuzi(arr[0]) && IsShuzi(arr[1]);
        }

        /// <summary>
        /// 三张连续
        /// </summary>
        /// <param name="cards">需要排序后</param>
        /// <param name="groupObj"></param>
        /// <param name="cardSize"></param>
        /// <returns></returns>
        public bool IsThee(int[] cards, CardGroup groupObj, out int cardSize)
        {
            cardSize = 0;
            if (cards.Length % 3 != 0)
            {
                return false;
            }
            int val = GetCardSize(cards[0]), count = 0;

            for (var i = 0; i < cards.Length; i = i + 3)
            {
                int val1 = GetCardSize(cards[i]);
                if (val1 != val + count ||
                    groupObj.GetSameSizeCount(val1) != 3 ||
                    val1 >= (int)CardSize.C_2)
                {
                    return false;
                }
                count++;
            }
            cardSize = val;
            return true;
        }

        /// <summary>
        /// 四张
        /// </summary>
        /// <param name="cards">需要排序后</param>
        /// <param name="groupObj"></param>
        /// <param name="cardSize"></param>
        /// <returns></returns>
        public bool IsFour(int[] cards, CardGroup groupObj, out int cardSize)
        {
            cardSize = 0;
            if (cards.Length != 4)
            {
                return false;
            }
            int val = GetCardSize(cards[0]);
            if (groupObj.GetSameSizeCount(val) == 4)
            {
                cardSize = val;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 四张带二或二对
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="groupObj"></param>
        /// <param name="cardSize"></param>
        /// <returns></returns>
        public bool IsFourAndTwo(int[] cards, CardGroup groupObj, out int cardSize)
        {
            cardSize = 0;
            if (cards.Length != 6 && cards.Length != 8)
            {
                return false;
            }
            var list = groupObj.FindSame(4);
            if (list.Length > 0)
            {
                if (cards.Length == 8 && groupObj.FindSameCount(2).Length != 2)
                {
                    return false;
                }
                cardSize = GetCardSize(list[0]);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取牌型
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="cardSize">最小牌面</param>
        /// <returns></returns>
        public DeckType GetCardType(int[] cards, out int cardSize)
        {
            SortCard(cards);
            var obj = ParseCardGroup(cards);
            int val;
            int length = cards.Length;
            cardSize = 0;
            if (length > 0)
            {
                cardSize = GetCardSize(cards[0]);
            }

            switch (length)
            {
                case 0:
                    return DeckType.None;
                case 1:
                    return DeckType.Single;
                case 2:
                    int size1 = GetCardSize(cards[0]);
                    int size2 = GetCardSize(cards[1]);
                    if (size1 > (int)CardSize.C_2 && size2 > (int)CardSize.C_2)
                    {
                        return DeckType.WangBomb;
                    }
                    if (size1 == size2)
                    {
                        return DeckType.Double;
                    }
                    break;
                case 3:
                    if (IsThee(cards, obj, out cardSize))
                    {
                        return DeckType.Three;
                    }
                    break;
                case 4:
                    if (IsFour(cards, obj, out cardSize))
                    {
                        return DeckType.Bomb;
                    }
                    //三带一
                    for (var i = 0; i < 4; i++)
                    {
                        val = GetCardSize(cards[i]);
                        if (obj.GetSameSizeCount(val) == 3)
                        {
                            cardSize = val;
                            return DeckType.ThreeAndOne;
                        }
                    }
                    break;
                case 5:
                    //三带二
                    int len;
                    bool isTri = false, //是否是三张
                        isDouble = false;//是否是对子
                    for (var i = 0; i < 5; i++)
                    {
                        val = GetCardSize(cards[i]);
                        len = obj.GetSameSizeCount(val);
                        if (len == 3)
                        {
                            cardSize = val;
                            isTri = true;
                        }
                        else if (len == 2)
                        {
                            isDouble = true;
                        }
                        if (isTri && isDouble)
                        {
                            return DeckType.ThreeAndTwo;
                        }
                    }
                    break;
                default:
                    break;
            }
            //顺子
            if (length > 4 && IsShuzi(cards))
            {
                return DeckType.Shunzi;
            }
            //连对
            if (length > 5 && IsLiandui(cards))
            {
                return DeckType.Liandui;
            }
            //三顺
            if (length > 5 && IsThee(cards, obj, out cardSize))
            {
                return DeckType.Fly;
            }
            //四带二
            if (length > 5 && IsFourAndTwo(cards, obj, out cardSize))
            {
                return DeckType.FourAndTwo;
            }
            //飞机
            if (length > 7)
            {
                int singleNum = 0, doubleNum = 0, threeNum = 0;
                val = 0;
                bool isBreak = false;
                for (int i = 0; i < obj.Count; i++)
                {
                    var item = obj.GetSame(i);

                    if (item.Length == 1)
                    {
                        singleNum++;
                    }
                    else if (item.Length == 2)
                    {
                        doubleNum++;
                    }
                    else if (item.Length == 3)
                    {
                        int val1 = GetCardSize(item[0]);
                        if (val > 0 && val != val1 - 1)
                        {
                            isBreak = true;
                        }
                        else if (val > 0)
                        {
                            cardSize = val;
                        }
                        val = val1;
                        threeNum++;
                    }
                }
                //飞机单牌
                if (!isBreak &&
                    threeNum > 1 &&
                    threeNum == singleNum &&
                    threeNum * 3 + singleNum == length)
                {
                    return DeckType.FlyAndTwo;
                }

                //飞机对牌
                if (length > 9 &&
                    !isBreak &&
                    threeNum > 1 &&
                    threeNum == doubleNum &&
                    threeNum * 3 + doubleNum * 2 == length)
                {
                    return DeckType.FlyAndTwoDouble;
                }

            }
            return DeckType.Error;
        }
        /// <summary>
        /// 比较牌大小
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a大时返回True</returns>
        public bool EqualsCard(CardData a, CardData b)
        {
            return b.Type == DeckType.None ||
                a.Type == DeckType.WangBomb ||
                (a.Type != b.Type && b.Type != DeckType.WangBomb && a.Type == DeckType.Bomb) ||
                (a.Type == b.Type && a.Cards.Length == b.Cards.Length && a.CardSize > b.CardSize);
        }

    }
}
