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
using System.Collections;
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
        class BadWordsFilter
        {
            private HashSet<string> hash = new HashSet<string>();
            private byte[] fastCheck = new byte[char.MaxValue];
            private byte[] fastLength = new byte[char.MaxValue];
            private BitArray charCheck = new BitArray(char.MaxValue);
            private BitArray endCheck = new BitArray(char.MaxValue);
            private int maxWordLength = 0;
            private int minWordLength = int.MaxValue;
            
            public void AddKey(string word)
            {
                maxWordLength = Math.Max(maxWordLength, word.Length);
                minWordLength = Math.Min(minWordLength, word.Length);

                for (int i = 0; i < 7 && i < word.Length; i++)
                {
                    fastCheck[word[i]] |= (byte)(1 << i);
                }

                for (int i = 7; i < word.Length; i++)
                {
                    fastCheck[word[i]] |= 0x80;
                }

                if (word.Length == 1)
                {
                    charCheck[word[0]] = true;
                }
                else
                {
                    fastLength[word[0]] |= (byte)(1 << (Math.Min(7, word.Length - 2)));
                    endCheck[word[word.Length - 1]] = true;
                    hash.Add(word);
                }
            }

            public string Filter(string text, char mask)
            {
                char[] chars = text.ToCharArray();
                int index = 0;

                while (index < text.Length)
                {
                    int count = 1;

                    if (index > 0 || (fastCheck[text[index]] & 1) == 0)
                    {
                        while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                    }

                    char begin = text[index];

                    if (minWordLength == 1 && charCheck[begin])
                    {
                        chars[index] = mask;
                    }

                    for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                    {
                        char current = text[index + j];
                        if ((fastCheck[current] & 1) == 0)
                        {
                            ++count;
                        }

                        if ((fastCheck[current] & (1 << Math.Min(j, 7))) == 0)
                        {
                            break;
                        }

                        if (j + 1 >= minWordLength)
                        {
                            if ((fastLength[begin] & (1 << Math.Min(j - 1, 7))) > 0 && endCheck[current])
                            {
                                string sub = text.Substring(index, j + 1);

                                if (hash.Contains(sub))
                                {
                                    int subCount = index + j + 1;
                                    for (int r = index; r < subCount; r++)
                                    {
                                        chars[r] = mask;
                                    }
                                }
                            }
                        }
                    }

                    index += count;
                }
                return new string(chars);
            }

            public bool HasBadWord(string text)
            {
                int index = 0;

                while (index < text.Length)
                {
                    int count = 1;

                    if (index > 0 || (fastCheck[text[index]] & 1) == 0)
                    {
                        while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                    }

                    char begin = text[index];

                    if (minWordLength == 1 && charCheck[begin])
                    {
                        return true;
                    }

                    for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                    {
                        char current = text[index + j];

                        if ((fastCheck[current] & 1) == 0)
                        {
                            ++count;
                        }

                        if ((fastCheck[current] & (1 << Math.Min(j, 7))) == 0)
                        {
                            break;
                        }

                        if (j + 1 >= minWordLength)
                        {
                            if ((fastLength[begin] & (1 << Math.Min(j - 1, 7))) > 0 && endCheck[current])
                            {
                                string sub = text.Substring(index, j + 1);

                                if (hash.Contains(sub))
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    index += count;
                }

                return false;
            }
        }

        private static BadWordsFilter _filter;

        static SensitiveWordService()
        {
            Init();
        }

        /// <summary>
        /// Init word
        /// </summary>
        public static void Init()
        {
            _filter = new BadWordsFilter(); 
            var cacheSet = new ShareCacheStruct<SensitiveWord>();
            cacheSet.Foreach((k, v) =>
            {
                _filter.AddKey(v.Word);
                return true;
            });
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
            return _filter.HasBadWord(str);
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
            return _filter.Filter(str, replaceChar);
        }
         
    }
}