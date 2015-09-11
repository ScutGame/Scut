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
using System.Collections.Generic;
using System.Threading;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Framework.Game.Message
{
    /// <summary>
    /// 聊天服务
    /// </summary>
    public abstract class ChatService
    {
        private readonly int _userId;
        private readonly ChatCacheSet _chatCacheSet;
        private readonly WhisperCacheSet _whisperCacheSet;
        private static readonly VersionConfig VersionSet = new VersionConfig();

        /// <summary>
        /// 取下一个版本号
        /// </summary>
        public static int NextVersion
        {
            get
            {
                return VersionSet.NextId;
            }
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Message.ChatService"/> class.
		/// </summary>
		/// <param name="userId">User identifier.</param>
        protected ChatService(int userId)
        {
            _userId = userId;
            _chatCacheSet = new ChatCacheSet();
            _whisperCacheSet = new WhisperCacheSet();
        }
		/// <summary>
		/// Gets the curr version.
		/// </summary>
		/// <value>The curr version.</value>
        public int CurrVersion
        {
            get
            {
                return VersionSet.Id;
            }
        }
		/// <summary>
		/// Determines whether this instance has message the specified version.
		/// </summary>
		/// <returns><c>true</c> if this instance has message the specified version; otherwise, <c>false</c>.</returns>
		/// <param name="version">Version.</param>
        public bool HasMessage(int version)
        {
            return CurrVersion > version;
        }
		/// <summary>
		/// Send the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
        public void Send(ChatMessage message)
		{
		    message.ExpiredTime = message.SendDate;
            _chatCacheSet.Add(message);
            WriteLog(message);
        }
		/// <summary>
		/// Sends the whisper.
		/// </summary>
		/// <param name="toUserId">To user identifier.</param>
		/// <param name="message">Message.</param>
        public void SendWhisper(int toUserId, ChatMessage message)
        {
            message.ExpiredTime = message.SendDate;
            //系统发私聊出现两条的问题
            if (_userId != toUserId)
            {
                _whisperCacheSet.Add(_userId, message);
            }
            _whisperCacheSet.Add(toUserId, message);
            WriteLog(message);
        }
		/// <summary>
		/// Receive this instance.
		/// </summary>
        public List<ChatMessage> Receive()
        {
            var msgList = GetChatMessages();
            var whisperMessages = _whisperCacheSet.GetMessage(_userId);
            msgList.AddRange(whisperMessages);
            msgList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                int result = x.SendDate.CompareTo(y.SendDate);
                if (result == 0)
                {
                    result = x.Version.CompareTo(y.Version);
                }
                return result;
            });
            return msgList;
        }
		/// <summary>
		/// Publics the receive.
		/// </summary>
		/// <returns>The receive.</returns>
        public List<ChatMessage> PublicReceive()
        {
            var msgList = GetChatMessages();
            msgList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                int result = x.SendDate.CompareTo(y.SendDate);
                if (result == 0)
                {
                    result = x.Version.CompareTo(y.Version);
                }
                return result;
            });
            return msgList;
        }
		/// <summary>
		/// Users the receive.
		/// </summary>
		/// <returns>The receive.</returns>
		/// <param name="userId">User identifier.</param>
        public List<ChatMessage> UserReceive(int userId)
        {
            List<ChatMessage> list = new List<ChatMessage>();
            var whisperMessages = _whisperCacheSet.GetMessage(userId);
            list.AddRange(whisperMessages);
            list.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                int result = x.SendDate.CompareTo(y.SendDate);
                if (result == 0)
                {
                    result = x.Version.CompareTo(y.Version);
                }
                return result;
            });
            return list;
        }
		/// <summary>
		/// Gets the chat messages.
		/// </summary>
		/// <returns>The chat messages.</returns>
        protected List<ChatMessage> GetChatMessages()
        {
            var msgList = new List<ChatMessage>();
            var chatMessages = _chatCacheSet.GetMessage();
            foreach (var chatMessage in chatMessages)
            {
                if (HasReceive(chatMessage))
                {
                    msgList.Add(chatMessage);
                }
            }
            return GetRange(msgList);
        }
		/// <summary>
		/// Gets the range.
		/// </summary>
		/// <returns>The range.</returns>
		/// <param name="msgList">Message list.</param>
        protected abstract List<ChatMessage> GetRange(List<ChatMessage> msgList);
		/// <summary>
		/// Determines whether this instance has receive the specified message.
		/// </summary>
		/// <returns><c>true</c> if this instance has receive the specified message; otherwise, <c>false</c>.</returns>
		/// <param name="message">Message.</param>
        protected abstract bool HasReceive(ChatMessage message);
		/// <summary>
		/// Filters the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="message">Message.</param>
        protected abstract string FilterMessage(string message);
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <param name="message">Message.</param>
        protected abstract void WriteLog(ChatMessage message);

    }
}