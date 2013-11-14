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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 广播系统
    /// </summary>
    public abstract class BroadcastService
    {
        private static readonly VersionConfig Version = new VersionConfig();
        private readonly BroadcastTimer _timer;
        private readonly BroadcastCacheSet _cacheSet;

        /// <summary>
        /// 
        /// </summary>
        protected BroadcastService()
        {
            _timer = new BroadcastTimer(TimerCallback);
            _cacheSet = new BroadcastCacheSet();
        }

        /// <summary>
        /// 是否有新消息
        /// </summary>
        /// <returns></returns>
        public bool HasMessage(int versionId)
        {
            return Version.Id > versionId;
        }

        /// <summary>
        /// 创建广播消息对象
        /// </summary>
        /// <param name="noticeType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public NoticeMessage Create(NoticeType noticeType, string content)
        {
            return Create(noticeType, content, "", DateTime.MinValue);
        }

        /// <summary>
        /// 创建广播消息对象
        /// </summary>
        /// <param name="noticeType">类型</param>
        /// <param name="content">内容</param>
        /// <param name="title">标题</param>
        /// <param name="expiryDate">过期时间</param>
        /// <returns></returns>
        public NoticeMessage Create(NoticeType noticeType, string content, string title, DateTime expiryDate)
        {
            return new NoticeMessage(noticeType, Version.NextId)
            {
                Content = content,
                SendDate = MathUtils.Now,
                Title = title,
                ExpiryDate = expiryDate
            };
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(NoticeMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.Content))
            {
                throw new ArgumentNullException("message", "message or message's Content is empty.");
            }
            _cacheSet.Send(message);
            WriteLog(message);
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public bool Remove(Guid messageId)
        {
            return _timer.Remove(messageId);
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool Remove(Predicate<NoticeMessage> match)
        {
            return _timer.Remove(match);
        }
        /// <summary>
        /// 定时发送
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="beginTime">开启时间段 hh:mm</param>
        /// <param name="endTime">结束时间段</param>
        /// <param name="isCycle">是否循环发送</param>
        /// <param name="secondInterval">间隔多久发送一次（秒）</param>
        public void SendTimer(NoticeMessage message, string beginTime, string endTime, bool isCycle, int secondInterval)
        {
            _timer.Add(message, beginTime, endTime, isCycle, secondInterval);
        }

        /// <summary>
        /// 定时发送
        /// </summary>
        /// <param name="message"></param>
        /// <param name="week">按每周几发送</param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isCycle"></param>
        /// <param name="secondInterval"></param>
        public void SendTimer(NoticeMessage message, DayOfWeek week, string beginTime, string endTime, bool isCycle, int secondInterval)
        {
            _timer.Add(message, week, beginTime, endTime, isCycle, secondInterval);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        public List<NoticeMessage> GetMessages(int versionId, int maxCount)
        {
            var msgList = new List<NoticeMessage>();
            var messages = _cacheSet.GetBroadcast();
            foreach (var msg in messages)
            {
                if (HasReceive(versionId, msg))
                {
                    msgList.Add(msg);
                }
            }
            return GetRange(msgList, maxCount);
        }
		/// <summary>
		/// Timers the callback.
		/// </summary>
		/// <param name="message">Message.</param>
        protected void TimerCallback(NoticeMessage message)
        {
            Send(Create(message.NoticeType, message.Content, message.Title, message.ExpiryDate));
        }

        /// <summary>
        /// 截取条数
        /// </summary>
        /// <param name="msgList"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        protected virtual List<NoticeMessage> GetRange(List<NoticeMessage> msgList, int maxCount)
        {
            if (msgList.Count > maxCount)
            {
                int pageCount;
                return msgList.GetPaging(1, maxCount, out pageCount);
            }
            return msgList;
        }

        /// <summary>
        /// 是否可以接收
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool HasReceive(int versionId, NoticeMessage message)
        {
            if (message.VersionId > versionId)
            {
                SetVersionId(message.VersionId);
                return true;
            }
            return false;
        }
		/// <summary>
		/// Sets the version identifier.
		/// </summary>
		/// <param name="versionId">Version identifier.</param>
        protected abstract void SetVersionId(int versionId);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        protected virtual void WriteLog(NoticeMessage message)
        {
        }

        /// <summary>
        /// 过滤屏蔽词
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract string FilterMessage(string message);


    }
}