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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9201_私聊发送接口
    /// </summary>
    public class Action9201 : BaseAction
    {
        private string _toUserID = string.Empty;
        private string _content = string.Empty;


        public Action9201(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9201, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("ToUserID", ref _toUserID)
                 && httpGet.GetString("Content", ref _content))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {

            int chatLeng = ConfigEnvSet.GetInt("Chat.ContentLeng");
            if (_content.IndexOf("<label") >= 0)
            {
                chatLeng += 150;
            }
            GameUser toUser = new GameDataCacheSet<GameUser>().FindKey(_toUserID);
            if (toUser == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9103_DoesNotExistTheUser;
                return false;
            }
            if (_content.Trim().Length == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9201_contentNotEmpty;
                return false;
            }
            if (_content.Length > chatLeng)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9201_TheInputTextTooLong;
                return false;
            }
            //if (!CacheChat.IsAllow(ContextUser))
            //{
            //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    ErrorInfo = LanguageManager.GetLang().St9203_ChatNotSend;
            //    return false;
            //}
            var chatService = new TjxChatService(ContextUser);
            chatService.SendWhisper(toUser, _content);
            UserFriends friends = new ShareCacheStruct<UserFriends>().FindKey(ContextUser.UserID, _toUserID);
            if (friends != null)
            {
                friends.ChatTime = DateTime.Now;
                //friends.Update();
            }

            return true;
        }
    }
}