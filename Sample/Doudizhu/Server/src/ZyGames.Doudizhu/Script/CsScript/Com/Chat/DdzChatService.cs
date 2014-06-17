using System;
using System.Collections.Generic;

using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Net;

namespace ZyGames.Doudizhu.Bll.Com.Chat
{
    /// <summary>
    /// 聊表功能
    /// </summary>
    public class DdzChatService : ChatService
    {
        private readonly GameUser _user;
        private const int MsgTimeOut = 0;//分钟
        private const int IntervalSend = 10;
        private SensitiveWordService _wordServer;

        public DdzChatService()
            : this(new GameUser())
        {

        }
        public DdzChatService(GameUser user)
            : base(user.UserId.ToInt())
        {
            _wordServer = new SensitiveWordService();
            this._user = user;
        }

        public static bool IsAllow(GameUser user, ChatType chatType)
        {
            if (user != null)
            {
                if (chatType == ChatType.World && (DateTime.Now - user.Property.ChatDate).TotalSeconds >= IntervalSend)
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
            //if (chatType == ChatType.Whisper) return;
            var chat = new ChatData
            {
                Version = NextVersion,
                FromUserID = Language.Instance.SystemUserId,
                FromUserName = Language.Instance.KingName,
                ToUserID = 0,
                ChatType = chatType,
                Content = content,
                SendDate = DateTime.Now,
                RoomId = _user.Property.RoomId,
                TableId = _user.Property.TableId
            };
            Send(chat);
        }


        public void Send(ChatType chatType, int chatID, string content)
        {
            var chat = new ChatData
            {
                Version = NextVersion,
                FromUserID = _user.UserId.ToInt(),
                FromUserName = _user.NickName,
                ToUserID = 0,
                ChatType = chatType,
                ChatID = chatID,
                Content = content,
                SendDate = DateTime.Now,
                RoomId = _user.Property.RoomId,
                TableId = _user.Property.TableId
            };
            _user.Property.ChatDate = DateTime.Now;
            Send(chat);
        }


        public void SendWhisper(GameUser toUser, string content)
        {
            if (_user == null || toUser == null)
            {
                throw new Exception("发送人或接收人为空值");
            }
            _user.Property.ChatDate = DateTime.Now;
            var chat = new ChatData
            {
                Version = 0,
                FromUserID = _user.UserId,
                FromUserName = _user.NickName,
                ToUserID = toUser.UserId.ToInt(),
                ChatType = ChatType.Whisper,
                Content = content,
                SendDate = DateTime.Now,
                RoomId = _user.Property.RoomId,
                TableId = _user.Property.TableId
            };
            SendWhisper(toUser.UserId.ToInt(), chat);
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
            return m != null && m.Version > _user.Property.ChatVesion; //&& m.SendDate.AddMinutes(MsgTimeOut) > DateTime.Now;
        }

        protected override string FilterMessage(string message)
        {
            return _wordServer.Filter(message);
        }

        protected override void WriteLog(ChatMessage message)
        {
            var chatData = message as ChatData;
            if (chatData == null) return;

            var chatLog = new ChatLog
            {
                FromUserID = chatData.FromUserID,
                FromUserName = chatData.FromUserName,
                ToUserID = chatData.ToUserID,
                ChatType = chatData.ChatType,
                Content = chatData.Content,
                SendDate = chatData.SendDate
            };
            using (var sender = DataSyncManager.GetDataSender())
            {
                sender.Send(chatLog);
            }
        }
    }
}
