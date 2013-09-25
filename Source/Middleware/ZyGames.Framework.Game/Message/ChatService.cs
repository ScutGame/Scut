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

        protected ChatService(int userId)
        {
            _userId = userId;
            _chatCacheSet = new ChatCacheSet();
            _whisperCacheSet = new WhisperCacheSet();
        }

        public int CurrVersion
        {
            get
            {
                return VersionSet.Id;
            }
        }

        public bool HasMessage(int version)
        {
            return CurrVersion > version;
        }

        public void Send(ChatMessage message)
        {
            _chatCacheSet.Add(message);
            WriteLog(message);
        }

        public void SendWhisper(int toUserId, ChatMessage message)
        {
            //系统发私聊出现两条的问题
            if (_userId != toUserId)
            {
                _whisperCacheSet.Add(_userId, message);
            }
            _whisperCacheSet.Add(toUserId, message);
            WriteLog(message);
        }

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

        protected abstract List<ChatMessage> GetRange(List<ChatMessage> msgList);

        protected abstract bool HasReceive(ChatMessage message);

        protected abstract string FilterMessage(string message);

        protected abstract void WriteLog(ChatMessage message);

    }
}
