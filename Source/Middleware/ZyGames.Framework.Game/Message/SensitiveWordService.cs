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
            SchemaTable schema;
            if (EntitySchemaSet.TryGet<SensitiveWord>(out schema))
            {
                schema.ConnectionType = "";
                schema.ConnectionString = ConfigManger.connectionString;
            }
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
        public string Filter(string str, char replaceChar='*')
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
