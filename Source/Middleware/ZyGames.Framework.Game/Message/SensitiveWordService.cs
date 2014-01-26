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
using System.Linq;
using System.Text;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Pay;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 敏感词组件
    /// </summary>
    public class SensitiveWordService
    {
        private static ShareCacheStruct<SensitiveWord> _cacheSet;

        static SensitiveWordService()
        {
            _cacheSet = new ShareCacheStruct<SensitiveWord>();
        }

        private List<SensitiveWord> _wordList;
        /// <summary>
        /// 
        /// </summary>
        public SensitiveWordService()
        {
            _wordList = _cacheSet.FindAll();
        }

        /// <summary>
        /// 检查是否包含敏感词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsVerified(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            foreach (var item in _wordList)
            {
                if (str.IndexOf(item.Word) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 过滤敏感词
        /// </summary>
        /// <param name="str"></param>
        /// <param name="replaceChar">替换的字符</param>
        /// <returns></returns>
        public string Filter(string str, char replaceChar = '*')
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            foreach (var item in _wordList)
            {
                if (str.IndexOf(item.Word) != -1)
                {
                    str = str.Replace(item.Word, new string(replaceChar, item.Word.Length));
                }
            }
            return str;
        }
    }
}