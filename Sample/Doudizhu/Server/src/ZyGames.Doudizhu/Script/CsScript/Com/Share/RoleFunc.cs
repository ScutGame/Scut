using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Event;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Message;

namespace ZyGames.Doudizhu.Bll.Com.Share
{
    /// <summary>
    /// 角色功能模块
    /// </summary>
    public class RoleFunc
    {
        private SensitiveWordService _wordServer;

        public RoleFunc()
        {
            _wordServer = new SensitiveWordService();
        }
        /// <summary>
        /// 验证有效范围
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool VerifyRange(string nickName, out string msg)
        {
            msg = "";
            nickName = nickName ?? string.Empty;
            nickName = nickName.Trim();
            int minLength = ConfigEnvSet.GetInt("User.MinLength", 4);
            int maxLength = ConfigEnvSet.GetInt("User.MaxLength", 12);
            int length = Encoding.Default.GetByteCount(nickName);
            if (length < minLength || length > maxLength)
            {
                msg = string.Format(Language.Instance.St1005_NickNameOutRange, minLength, maxLength);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证昵称是否有关键词
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool VerifyKeyword(string nickName, out string msg)
        {
            msg = "";
            if (_wordServer.IsVerified(nickName))
            {
                msg = Language.Instance.St1005_NickNameExistKeyword;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否存在角色名
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool IsExistNickName(string nickName, out string msg)
        {
            msg = "";
            var list = new ShareCacheStruct<UserNickName>().FindAll(m => Equals(m.NickName, nickName), false);
            if (list.Count > 0)
            {
                msg = Language.Instance.St1005_NickNameExist;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创始成功之后处理事件
        /// </summary>
        public void OnCreateAfter(GameUser user)
        {
            var cacheSet = new ShareCacheStruct<UserNickName>();
            UserNickName u = new UserNickName(user.UserId);
            u.NickName = user.NickName;
            cacheSet.Add(u);
        }
    }
}
