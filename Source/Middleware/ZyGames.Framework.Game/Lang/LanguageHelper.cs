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

namespace ZyGames.Framework.Game.Lang
{
	/// <summary>
	/// Lang enum.
    /// </summary>
    [Obsolete]
    public enum LangEnum
    {
        /// <summary>
        /// 值=0 中文
        /// </summary>
        ZH_CN,
        /// <summary>
        /// 值=1 英文
        /// </summary>
        EN_US,
        /// <summary>
        /// 值=2 繁体
        /// </summary>
        BIG5_TW,
        /// <summary>
        /// 值=3 韩文
        /// </summary>
        KOREAN
    }

    /// <summary>
    /// 多语言
    /// </summary>
    [Obsolete("Use Language.Instance call.")]
    public class LanguageHelper
    {
        private static object thisLock = new object();
        private static Dictionary<LangEnum, ILanguage> _langTable = new Dictionary<LangEnum, ILanguage>();
        private static LangEnum _langEnum;

        static LanguageHelper()
        {
            _langEnum = LangEnum.ZH_CN;
        }
		/// <summary>
		/// Sets the lang.
		/// </summary>
		/// <param name="lang">Lang.</param>
        public static void SetLang(LangEnum lang)
        {
            lock (thisLock)
            {
                _langEnum = lang;
            }
        }
		/// <summary>
		/// Gets the lang.
		/// </summary>
		/// <returns>The lang.</returns>
        public static ILanguage GetLang()
        {
            return GetLang(_langEnum);
        }
		/// <summary>
		/// Gets the lang.
		/// </summary>
		/// <returns>The lang.</returns>
		/// <param name="langEnum">Lang enum.</param>
        public static ILanguage GetLang(LangEnum langEnum)
        {
            ILanguage lang = null;
            if (!_langTable.ContainsKey(langEnum))
            {
                lock (thisLock)
                {
                    if (!_langTable.ContainsKey(langEnum))
                    {
                        switch (langEnum)
                        {
                            case LangEnum.ZH_CN:
                                _langTable.Add(langEnum, new BaseZHLanguage());
                                break;
                            case LangEnum.BIG5_TW:
                                _langTable.Add(langEnum, new BaseBIG5Language());
                                break;
                            case LangEnum.EN_US:
                                _langTable.Add(langEnum, new BaseENLanguage());
                                break;
                            case LangEnum.KOREAN:
                                _langTable.Add(langEnum, new BaseKoreanLanguage());
                                break;
                            default:
                                _langTable.Add(langEnum, new BaseZHLanguage());
                                break;
                        }
                    }
                }
            }
            lang = _langTable[langEnum];
            return lang;
        }
    }
}