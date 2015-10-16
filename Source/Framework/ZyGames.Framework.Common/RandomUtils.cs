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

namespace ZyGames.Framework.Common
{

    /// <summary>
    /// 随机管理类
    /// </summary>
    public static class RandomUtils
    {
        private static int MinPercent = 1;
        private static int MaxPercent = 100;
        private static Random random = new Random();

        /// <summary>
        /// 随机取名字
        /// </summary>
        /// <returns></returns>
        public static string RandomNickName(string[] firstNames, string[] lastNames)
        {
            if (firstNames == null)
            {
                throw new ArgumentNullException("firstNames");
            }
            if (lastNames == null)
            {
                throw new ArgumentNullException("lastNames");
            }
            string name = "";
            if (firstNames.Length > 0)
            {
                name = firstNames[GetRandom(0, firstNames.Length)];
            }
            int length = GetRandom(1, 3);
            for (int i = 0; i < length; i++)
            {
                if (lastNames.Length > 0)
                {
                    string temp = lastNames[GetRandom(0, lastNames.Length)];
                    if (!string.IsNullOrEmpty(temp))
                    {
                        name += temp.Substring(0, 1);
                    }
                }
            }
            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static bool NextBool(double rate)
        {
            return random.NextDouble() < rate;
        }
        /// <summary>
        /// 获取随机概率值1-100
        /// </summary>
        /// <returns></returns>
        public static int GetRandom()
        {
            return GetRandom(MinPercent, MaxPercent + 1);
        }

        /// <summary>
        /// 获取随机概率值
        /// </summary>
        /// <param name="minNum">大小或包含最小值</param>
        /// <param name="maxNum">小于最大值</param>
        /// <returns></returns>
        public static int GetRandom(int minNum, int maxNum)
        {
            minNum = minNum < 0 ? 0 : minNum;
            if (minNum < maxNum)
            {
                return random.Next(minNum, maxNum);
            }
            throw new ArgumentOutOfRangeException("maxNum");
        }

        /// <summary>
        /// 是否命中，不限制浮点位数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsHitNew(double value)
        {
            return NextBool(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="increase"></param>
        /// <returns></returns>
        public static bool IsHitNew(double value, double increase)
        {
            return NextBool(value * (1 + increase));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="increase"></param>
        /// <param name="radix"></param>
        /// <returns></returns>
        public static bool IsHitNew(int value, int increase, int radix = 100)
        {
            return NextBool((value / radix) * (1 + increase / radix));
        }

        /// <summary>
        /// 是否命中
        /// </summary>
        /// <param name="percent">百分比概率值</param>
        /// <returns></returns>
        public static bool IsHit(int percent)
        {
            percent = percent > MaxPercent ? MaxPercent : percent;
            return NextBool(percent * 0.01);
        }
        /// <summary>
        /// 是否命中1000
        /// </summary>
        /// <param name="percent"></param>
        /// <returns></returns>
        public static bool IsHitByTh(decimal percent)
        {
            return NextBool((double)percent);
        }

        /// <summary>
        /// 是否命中
        /// </summary>
        /// <param name="percent">含百分比形式值</param>
        /// <returns></returns>
        public static bool IsHit(double percent)
        {
            return NextBool(percent);
        }

        /// <summary>
        /// 是否命中
        /// </summary>
        /// <param name="percent">含百分比形式值</param>
        /// <returns></returns>
        public static bool IsHit(decimal percent)
        {
            return NextBool((double)percent);
        }
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="percents"></param>
        /// <returns></returns>
        public static int HitIndex(double[] percents)
        {
            int index = -1;
            int hitIndex = -1;
            double max = 0;
            int maxIndex = 0;
            var r = random.NextDouble();
            double offset = 0;
            foreach (var p in percents)
            {
                index++;
                if (p > max)
                {
                    maxIndex = index;
                    max = p;
                }
                offset += p;
                if (r <= offset)
                {
                    hitIndex = index;
                    break;
                }
            }
            if (hitIndex == -1)
            {
                hitIndex = maxIndex;
            }
            return hitIndex;
        }

        /// <summary>
        /// 获取命中的索引
        /// </summary>
        /// <param name="percents">概率数组</param>
        /// <example>
        /// int[] percents = { 20, 4, 10, 12, 18, 18, 18 }
        /// </example>
        /// <returns></returns>
        public static int GetHitIndex(int[] percents)
        {
            int index = -1;
            int p = GetRandom();
            int num = 0;
            if (percents != null)
            {
                for (int i = 0; i < percents.Length; i++)
                {
                    num += percents[i];
                    if (p <= num)
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// 支持最大4位小数
        /// </summary>
        /// <param name="percents"></param>
        /// <returns></returns>
        public static int GetHitIndex(double[] percents)
        {
            int index = 0;
            int totalNum = 1;
            if (percents == null)
            {
                return -1;
            }
            foreach (var percent in percents)
            {
                totalNum += (int)Math.Floor(Math.Abs(percent) * 10000);
            }

            int tempNum = 0;
            if (totalNum > 0)
            {
                tempNum = GetRandom(0, totalNum);
            }
            double num = 0;
            for (int i = 0; i < percents.Length; i++)
            {
                num += (int)Math.Floor(Math.Abs(percents[i]) * 10000);
                if (tempNum <= num)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        /// <summary>
        /// 获取命中的索引1000
        /// </summary>
        /// <param name="percents">概率数组</param>
        /// <example>
        /// int[] percents = { 20, 4, 10, 12, 18, 18, 18 }
        /// </example>
        /// <returns></returns>
        public static int GetHitIndexByTH(int[] percents)
        {
            int index = -1;
            if (percents == null)
            {
                return index;
            }
            int p = GetRandom(0, 1000 + 1);
            int num = 0;
            for (int i = 0; i < percents.Length; i++)
            {
                num += percents[i];
                if (p <= num)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// 获取随机范围数组
        /// </summary>
        /// <param name="count">数组大小</param>
        /// <param name="randomMax">随机最大值</param>
        /// <param name="totalMin">随机总数最小值</param>
        /// <param name="totalMax">随机总数最大值</param>
        /// <returns></returns>
        public static int[] GetRandom(int count, int randomMax, int totalMin, int totalMax)
        {
            int randomMin = (int)Math.Floor((double)totalMin / count);
            return GetRandom(count, randomMin, randomMax, totalMin, totalMax);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="randomMin"></param>
        /// <param name="randomMax"></param>
        /// <param name="totalMin"></param>
        /// <param name="totalMax"></param>
        /// <returns></returns>
        public static int[] GetRandom(int count, int randomMin, int randomMax, int totalMin, int totalMax)
        {
            if (count > 100)
            {
                throw new ArgumentOutOfRangeException("count", "值不能超过100");
            }
            count = count < 0 ? 0 : count;
            randomMin = randomMin < 0 ? 0 : randomMin;
            randomMax = randomMax < 0 ? 0 : randomMax;
            totalMin = totalMin < 0 ? 0 : totalMin;
            totalMax = totalMax < 0 ? 0 : totalMax;
            int currentTotal = 0;
            int[] values = new int[count];
            for (int i = 0; i < count; i++)
            {
                int tminVal = (count - i - 1) * randomMin;
                tminVal = tminVal == 0 ? randomMin : tminVal;

                int tmaxVal = totalMax - currentTotal - tminVal;
                tmaxVal = tmaxVal > randomMax ? randomMax : tmaxVal;
                tmaxVal = tmaxVal < tminVal ? tminVal : tmaxVal;

                int val = GetRandom(randomMin, tmaxVal + 1);
                currentTotal += val;

                values[i] = val;
            }

            return values;
        }

        /// <summary>
        /// 从数组中随机取N个记录
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] GetRandomArray<T>(T[] array, int count)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            List<T> list = new List<T>();
            if (count > array.Length)
            {
                return list.ToArray();
            }
            bool[] checkedList = new bool[array.Length];

            while (list.Count < count)
            {
                int index = GetRandom(0, array.Length);
                if (!checkedList[index])
                {
                    list.Add(array[index]);
                    checkedList[index] = true;
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获取特定条范围的随机数
        /// </summary>
        /// <param name="count">数组长度</param>
        /// <param name="randomMin">数组单项最小值</param>
        /// <param name="randomMax">数组单项最大值</param>
        /// <param name="totalMin">总和最小值</param>
        /// <param name="totalMax">总和最大值</param>
        /// <returns></returns>
        public static int[] GetRandomNew(int count, int randomMin, int randomMax, int totalMin, int totalMax)
        {
            var val = GetRandom(totalMin, totalMax);
            return GetRandomNew(count, randomMin, randomMax, val);
        }

        /// <summary>
        /// 获取特定条范围的随机数
        /// </summary>
        /// <param name="count">数组长度</param>
        /// <param name="randomMin">数组单项最小值</param>
        /// <param name="randomMax">数组单项最大值</param>
        /// <param name="total">总和</param>
        /// <returns></returns>
        public static int[] GetRandomNew(int count, int randomMin, int randomMax, int total)
        {
            var realMax = randomMax;
            var rollMax = 0;
            var realMin = randomMin;
            var currentTotal = 0;
            var temTotal = total;
            var values = new int[count];
            for (var i = 0; i < count - 1; i++)
            {
                realMax = temTotal - randomMin;
                rollMax = randomMax * (count - i - 1);
                realMax = realMax < rollMax ? realMax : rollMax;


                realMin = MathUtils.Subtraction(temTotal, randomMax, randomMin * (count - i - 1));

                var val = GetRandom(realMin, realMax + 1);
                values[i] = MathUtils.Subtraction(temTotal, val);
                currentTotal = MathUtils.Addition(currentTotal, values[i]);
                temTotal = val;
            }
            values[count - 1] = total - currentTotal;
            return values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] RandomSort<T>(T[] source)
        {
            int num = source.Length / 2;
            for (int i = 0; i < num; i++)
            {
                int num2 = GetRandom(0, source.Length);
                int num3 = GetRandom(0, source.Length);
                if (num2 != num3)
                {
                    T t = source[num3];
                    source[num3] = source[num2];
                    source[num2] = t;
                }
            }
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RandomSort<T>(List<T> source)
        {
            int num = source.Count / 2;
            for (int i = 0; i < num; i++)
            {
                int num2 = GetRandom(0, source.Count);
                int num3 = GetRandom(0, source.Count);
                if (num2 != num3)
                {
                    T t = source[num3];
                    source[num3] = source[num2];
                    source[num2] = t;
                }
            }
            return source;
        }

    }
}