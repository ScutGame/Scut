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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9204_聊天列表接口
    /// </summary>
    public class Action9204 : BaseAction
    {
        private List<ChatMessage> _chatArray = new List<ChatMessage>();
        private int _currVersion;
        private int _charItemNum;
        public Action9204(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9204, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_currVersion);
            PushIntoStack(_chatArray.Count);

            foreach (var chatMessage in _chatArray)
            {
                var chat = chatMessage as ChatData ?? new ChatData();
                UserFriends friends = new ShareCacheStruct<UserFriends>().FindKey(ContextUser.UserID, chat.FromUserID);
                int isFriend = friends != null && friends.FriendType == FriendType.Friend ? 1 : 2;
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(chat.FromUserID.ToNotNullString());
                dsItem.PushIntoStack(chat.FromUserName.ToNotNullString());
                dsItem.PushIntoStack(chat.Content.ToNotNullString());
                dsItem.PushIntoStack(FormatDate(chat.SendDate));
                dsItem.PushIntoStack((short)chat.ChatType);
                dsItem.PushIntoStack(chat.ToUserID.ToNotNullString());
                dsItem.PushIntoStack(chat.ToUserName.ToNotNullString());
                dsItem.PushIntoStack(isFriend);
                dsItem.PushIntoStack(chat.FromUserVip);
                dsItem.PushIntoStack(chat.ToUserVip);
                PushIntoStack(dsItem);
            }
            PushIntoStack(_charItemNum);


        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            var chatService = new TjxChatService(ContextUser);
            _currVersion = chatService.CurrVersion;
            _chatArray = chatService.Receive();
            ContextUser.ChatVesion = _currVersion;
            //查找背包中的聊天道具
            int chatItemId = ConfigEnvSet.GetInt("UserItem.ChatItemID");
            _charItemNum=UserItemHelper.CheckItemNum(ContextUser.UserID, chatItemId);
            return true;
        }
    }
}