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
		/// <summary>
		/// The mail cache set.
		/// </summary>
        protected PersonalCacheStruct<T> MailCacheSet;
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
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