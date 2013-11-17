using System;
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Doudizhu.Lang
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
                                _langTable.Add(langEnum, new GameZhLanguage());
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
