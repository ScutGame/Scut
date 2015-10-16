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

namespace ZyGames.Framework.Common.Security
{
    /// <summary>
    /// 加密类
    /// </summary>
    public class DesCopy
    {
        /// <summary>
        /// 构造函数逻辑
        /// </summary>
        public DesCopy()
        {
        }

        /// <summary>
        ///  The unsafe keyword allows pointers to be used within the following method:
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcIndex"></param>
        /// <param name="dst"></param>
        /// <param name="dstIndex"></param>
        /// <param name="count"></param>
        public void Copy(byte[] src, int srcIndex, ref byte[] dst, int dstIndex, int count)
        {
            if (src == null || srcIndex < 0 ||
                dst == null || dstIndex < 0 || count < 0)
            {
                throw new System.ArgumentException();
            }

            int srcLen = src.Length;
            int dstLen = dst.Length;
            if (srcLen - srcIndex < count || dstLen - dstIndex < count)
            {
                throw new System.ArgumentException();
            }

            // The following fixed statement pins the location of the src and dst objects
            // in memory so that they will not be moved by garbage collection.
            for (int i = 0; i < count; i++)
            {
                dst[dstIndex + i] = src[srcIndex + i];
            }

        }


        /// <summary>
        /// // The unsafe keyword allows pointers to be used within the following method:
        /// </summary>
        /// <param name="src"></param>
        /// <param name="srcIndex"></param>
        /// <param name="dst"></param>
        /// <param name="dstIndex"></param>
        /// <param name="count"></param>
        public void Copy(char[] src, int srcIndex, ref char[] dst, int dstIndex, int count)
        {
            if (src == null || srcIndex < 0 ||
                dst == null || dstIndex < 0 || count < 0)
            {
                throw new System.ArgumentException();
            }

            int srcLen = src.Length;
            int dstLen = dst.Length;
            if (srcLen - srcIndex < count || dstLen - dstIndex < count)
            {
                throw new System.ArgumentException();
            }

            // The following fixed statement pins the location of the src and dst objects
            // in memory so that they will not be moved by garbage collection.
            for (int i = 0; i < count; i++)
            {
                dst[dstIndex + i] = src[srcIndex + i];
            }

        }

        /// <summary>
        /// 特殊的右移位操作，补0右移，如果是负数，需要进行特殊的转换处理（右移位）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int MoveByte(int value, int pos)
        {
            if (value < 0)
            {
                string s = Convert.ToString(value, 2);    // 转换为二进制
                for (int i = 0; i < pos; i++)
                {
                    s = "0" + s.Substring(0, 31);
                }
                return Convert.ToInt32(s, 2);            // 将二进制数字转换为数字
            }
            else
            {
                return value >> pos;
            }
        }
    }
}