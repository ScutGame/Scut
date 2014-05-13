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

using GameRanking.Pack;
using GameServer.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.RPC.IO;

namespace GameServer.CsScript.Action
{
    public abstract class BaseAction : BaseStruct
    {
        private ResponsePack _responseHead;

        protected BaseAction(int actionId, ActionGetter actionGetter)
            : base(actionId, actionGetter)
        {
        }

        private void SetResponseHead()
        {
            _responseHead = new ResponsePack()
            {
                MsgId = actionGetter.GetMsgId(),
                ActionId = actionGetter.GetActionId(),
                ErrorCode = ErrorCode,
                ErrorInfo = ErrorInfo,
                St = St
            };
        }

        protected virtual byte[] BuildResponsePack()
        {
            return null;
        }

        public override void WriteResponse(BaseGameResponse response)
        {
            SetResponseHead();
            byte[] headBytes = ProtoBufUtils.Serialize(_responseHead);
            byte[] contentBytes = BuildResponsePack();
            byte[] buffer = null;
            if (contentBytes == null || contentBytes.Length == 0)
            {
                buffer = BufferUtils.AppendHeadBytes(headBytes);
            }
            else
            {
                buffer = BufferUtils.MergeBytes(
                    BufferUtils.AppendHeadBytes(headBytes),
                    contentBytes
                );
            }
            //需要对字节数据加密处理，这里跳过

            response.BinaryWrite(buffer);
        }
    }
}