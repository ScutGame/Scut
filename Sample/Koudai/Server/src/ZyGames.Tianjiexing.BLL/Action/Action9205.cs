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
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Game.Model;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9205_系统广播接口
    /// </summary>
    public class Action9205 : BaseAction
    {
        private List<NoticeMessage> _broadcastList;


        public Action9205(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9205, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_broadcastList.Count);
            foreach (var item in _broadcastList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((short)item.NoticeType);
                dsItem.PushIntoStack(item.Content.ToNotNullString());
                dsItem.PushIntoStack((int)1);
                dsItem.PushIntoStack(item.Sender.ToNotNullString());
                PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            _broadcastList = new TjxBroadcastService(ContextUser).GetMessages();
            return true;
        }
    }
}