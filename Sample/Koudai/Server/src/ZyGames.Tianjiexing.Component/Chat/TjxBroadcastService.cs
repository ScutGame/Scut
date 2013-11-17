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
using ZyGames.Framework.Game.Message;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.Component.Chat
{
    /// <summary>
    /// 广播中间件
    /// </summary>
    public class TjxBroadcastService : BroadcastService
    {
        private readonly GameUser _user;
        public const int MaxCount = 50;

        /// <summary>
        /// 
        /// </summary>
        public TjxBroadcastService(GameUser user)
        {
            _user = user;
        }

        protected override void SetVersionId(int versionId)
        {
            if (_user != null)
            {
                _user.BroadcastVesion = versionId;
            }
        }

        public bool HasMessage()
        {
            return HasMessage(_user.BroadcastVesion);
        }

        public List<NoticeMessage> GetMessages()
        {
            return GetMessages(_user.BroadcastVesion, MaxCount);
        }

        protected override string FilterMessage(string message)
        {
            foreach (ChatKeyWord chatKeyWord in TjxChatService.ChatKeyWordList)
            {
                message = message.Replace(chatKeyWord.KeyWord, new string('*', chatKeyWord.KeyWord.Length));
            }
            return message;
        }
    }
}