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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Tianjiexing.Lang
{
    public class LanguageManager
    {
        private static object thisLock = new object();
        private static Dictionary<LangEnum, IGameLanguage> _langTable = new Dictionary<LangEnum, IGameLanguage>();
        private static LangEnum _langEnum;

        static LanguageManager()
        {
            _langEnum = (ConfigUtils.GetSetting("LanguageType", "0")).ToEnum<LangEnum>();
            LanguageHelper.SetLang(_langEnum);
        }

        public static IGameLanguage GetLang()
        {
            return GetLang(_langEnum);
        }

        public static IGameLanguage GetLang(LangEnum langEnum)
        {
            IGameLanguage lang = null;
            if (!_langTable.ContainsKey(langEnum))
            {
                lock (thisLock)
                {
                    if (!_langTable.ContainsKey(langEnum))
                    {
                        switch (langEnum)
                        {
                            case LangEnum.ZH_CN:
                                _langTable.Add(langEnum, new GameZHLanguage());
                                break;
                            case LangEnum.BIG5_TW:
                                _langTable.Add(langEnum, new GameBig5Language());
                                break;
                            case LangEnum.EN_US:
                                //_langTable.Add(langEnum, new GameZHLanguage());
                                break;
                            default:
                                throw new Exception("Language is error.");
                        }
                    }
                }
            }
            lang = _langTable[langEnum];
            return lang;
        }
    }
}