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
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;



namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9203_聊天发送接口
    /// </summary>
    public class Action9203 : BaseAction
    {
        private ChatType _chatType;
        private string _content = string.Empty;


        public Action9203(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9203, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("ChatType", ref _chatType)
                 && httpGet.GetString("Content", ref _content))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (_chatType == ChatType.Guild && string.IsNullOrEmpty(ContextUser.MercenariesID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9203_ChaTypeNotGuildMember;
                return false;
            }
           
            //查找背包中的聊天道具
            var chatLeng = ConfigEnvSet.GetInt("Chat.ContentLeng");
            int chatItemId = ConfigEnvSet.GetInt("UserItem.ChatItemID");
            int charItemNum = UserItemHelper.CheckItemNum(ContextUser.UserID, chatItemId);
            //判断是否有聊天道具
            if (charItemNum <= 0)
            {
                ErrorCode =3;
                ErrorInfo = LanguageManager.GetLang().St9203_ItemEmpty;
                return false; 
            }

            if (_content.IndexOf("<label") >= 0 || _content.IndexOf("<image") >= 0)
            {
                chatLeng += 150;
            }
            if (_content.Trim().Length == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9201_contentNotEmpty;
                return false;
            }
            //LogHelper.WriteError(string.Format("contentLength={0};length={1};content={2}", _content.Length, chatLeng, _content));
            if (_content.Length >= chatLeng)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9201_TheInputTextTooLong;
                return false;
            }

            if (!TjxChatService.IsAllow(ContextUser, _chatType))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St9203_ChatNotSend;
                return false;
            }

            NoviceHelper.WingFestival(ContextUser.UserID, _content);
            NoviceHelper.WingZhongYuanFestival(ContextUser, _content);
            //使用聊天道具
            UserItemHelper.UseUserItem(ContextUser.UserID, chatItemId, 1);
            var chatService = new TjxChatService(ContextUser);
            chatService.Send(_chatType, _content);

            return true;
        }

    }
}