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
using System.Linq;
using System.Text;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.Component.Chat
{
    /// <summary>
    /// 聊表功能
    /// </summary>
    public class TjxChatService : ChatService
    {
        private readonly GameUser _user;
        private const int MsgTimeOut = 30;//分钟
        private const int IntervalSend = 10;

        public static List<ChatKeyWord> ChatKeyWordList
        {
            get;
            private set;
        }

        static TjxChatService()
        {
            InitChatKeyWord();
        }

        public static void InitChatKeyWord()
        {
            ChatKeyWordList = new ConfigCacheSet<ChatKeyWord>().FindAll();
        }

        public TjxChatService()
            : this(new GameUser())
        {

        }
        public TjxChatService(GameUser user)
            : base(user.UserID.ToInt())
        {
            this._user = user;
        }

        public static bool IsAllow(GameUser user, ChatType chatType)
        {
            if (user != null)
            {
                if (chatType == ChatType.World && (DateTime.Now - user.ChatDate).TotalSeconds >= IntervalSend)
                {
                    return true;
                }
                else if (chatType != ChatType.World)
                {
                    return true;
                }
            }
            return false;
        }

        public void SystemSend(ChatType chatType, string content)
        {
            if (chatType == ChatType.Whisper) return;
            var chat = new ChatData
            {
                Version = NextVersion,
                FromUserID = LanguageManager.GetLang().SystemUserId,
                FromUserName = LanguageManager.GetLang().KingName,
                FromUserVip = 0,
                ToUserID = 0,
                ToUserName = string.Empty,
                ToUserVip = 0,
                ChatType = chatType,
                Content = content,
                SendDate = DateTime.Now
            };
            Send(chat);
        }

        public void SystemGuildSend(ChatType chatType, string content)
        {
            string guildID = string.Empty;
            if (chatType == ChatType.Whisper) return;
            if (!string.IsNullOrEmpty(_user.MercenariesID))
            {
                guildID = _user.MercenariesID;
            }
            var chat = new ChatData
            {
                Version = NextVersion,
                FromUserID = LanguageManager.GetLang().SystemUserId,
                FromUserName = LanguageManager.GetLang().KingName,
                FromUserVip = 0,
                ToUserID = 0,
                ToUserName = string.Empty,
                ToUserVip = 0,
                ChatType = chatType,
                Content = content,
                SendDate = DateTime.Now,
                GuildID = guildID
            };
            Send(chat);
        }

        public void Send(ChatType chatType, string content)
        {
            string guildID = string.Empty;
            if (chatType == ChatType.Guild)
            {
                guildID = _user.MercenariesID;
            }
            var chat = new ChatData
            {
                Version = NextVersion,
                FromUserID = _user.UserID.ToInt(),
                FromUserName = _user.NickName,
                FromUserVip = (short)_user.VipLv,
                ToUserID = 0,
                ToUserName = string.Empty,
                ToUserVip = 0,
                ChatType = chatType,
                Content = FilterMessage(content),
                SendDate = DateTime.Now,
                GuildID = guildID,
            };
            if (chatType == ChatType.World)
            {
                _user.ChatDate = DateTime.Now;
            }
            Send(chat);
        }


        public void SystemSendWhisper(GameUser toUser, string content)
        {
            if (toUser == null)
            {
                throw new Exception("接收人为空值");
            }
            SystemSendWhisper(toUser.UserID, toUser.NickName, (short)toUser.VipLv, content);
        }

        public void SystemSendWhisper(string userId, string userName, short vipLv, string content)
        {
            if (userId.Equals(LanguageManager.GetLang().SystemUserId.ToString()))
            {
                throw new Exception("不能给系统发私聊");
            }

            var chat = new ChatData
            {
                Version = 0,
                FromUserID = LanguageManager.GetLang().SystemUserId,
                FromUserName = LanguageManager.GetLang().KingName,
                FromUserVip = 0,
                ToUserID = userId.ToInt(),
                ToUserName = userName,
                ToUserVip = vipLv,
                ChatType = ChatType.Whisper,
                Content = content,
                SendDate = DateTime.Now
            };
            SendWhisper(userId.ToInt(), chat);
        }

        public void SendWhisper(GameUser toUser, string content)
        {
            if (_user == null || toUser == null)
            {
                throw new Exception("发送人或接收人为空值");
            }

            _user.ChatDate = DateTime.Now;
            var chat = new ChatData
            {
                Version = 0,
                FromUserID = _user.UserID.ToInt(),
                FromUserName = _user.NickName,
                FromUserVip = (short)_user.VipLv,
                ToUserID = toUser.UserID.ToInt(),
                ToUserName = toUser.NickName,
                ToUserVip = (short)toUser.VipLv,
                ChatType = ChatType.Whisper,
                Content = FilterMessage(content),
                SendDate = DateTime.Now
            };
            SendWhisper(toUser.UserID.ToInt(), chat);

        }


        protected override List<ChatMessage> GetRange(List<ChatMessage> msgList)
        {
            if (msgList.Count > 50)
            {
                int pageCount;
                return msgList.GetPaging(1, 50, out pageCount);
            }
            return msgList;
        }

        protected override bool HasReceive(ChatMessage message)
        {
            var m = message as ChatData;
            return m != null && m.Version > _user.ChatVesion &&
                m.SendDate.AddMinutes(MsgTimeOut) > DateTime.Now &&
                (string.IsNullOrEmpty(m.GuildID) || m.GuildID.Equals(_user.MercenariesID));
        }

        protected override string FilterMessage(string message)
        {
            foreach (ChatKeyWord chatKeyWord in ChatKeyWordList)
            {
                message = message.Replace(chatKeyWord.KeyWord, new string('*', chatKeyWord.KeyWord.Length));
            }
            return message;
        }

        protected override void WriteLog(ChatMessage message)
        {
            var chatData = message as ChatData;
            if (chatData == null) return;

            string guildID = string.Empty;
            if (chatData.ChatType == ChatType.Guild)
            {
                GameUser user = new GameDataCacheSet<GameUser>().FindKey(chatData.FromUserID.ToString());
                if (user == null)
                {
                    user = new GameDataCacheSet<GameUser>().FindKey(chatData.ToUserID.ToString());
                }
                if (user != null)
                {
                    guildID = user.MercenariesID;
                }
            }
            var chatLog = new UserChatLog
            {
                ChatID = Guid.NewGuid().ToString(),
                FromUserID = chatData.FromUserID.ToString(),
                FromUserName = chatData.FromUserName,
                ToUserID = chatData.ToUserID.ToString(),
                ToUserName = chatData.ToUserName,
                ChatType = chatData.ChatType,
                Content = chatData.Content,
                SendDate = chatData.SendDate,
                GuildID = guildID,
            };
            using (var sender = DataSyncManager.GetDataSender())
            {
                sender.Send(chatLog);
            }
        }
    }
}