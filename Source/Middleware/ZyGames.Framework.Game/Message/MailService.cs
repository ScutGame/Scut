using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public abstract class MailService<T> where T : MailMessage, new()
    {
        protected PersonalCacheStruct<T> MailCacheSet;

        protected MailService()
        {
            MailCacheSet = new PersonalCacheStruct<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <returns></returns>
        public bool Load(string personalId)
        {
            return MailCacheSet.AutoLoad(personalId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(T message)
        {
            MailCacheSet.Add(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public void Remove(T t)
        {
            MailCacheSet.RemoveCache(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public T ReadMail(string personalId, params object[] keys)
        {
            T t = MailCacheSet.FindKey(personalId, keys);
            SetRead(t);
            return t;
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="unReadCount">未读条数</param>
        /// <returns></returns>
        public abstract List<T> GetMail(out int unReadCount);

        /// <summary>
        /// 设置已读
        /// </summary>
        /// <param name="t"></param>
        protected abstract void SetRead(T t);

        /// <summary>
        /// 是否已读
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract bool IsUnRead(T message);
    }
}
