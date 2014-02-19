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

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 5306_公会战参战人员列表接口
    /// </summary>
    public class Action5306 : BaseAction
    {
        private int Ops;
        private int PageIndex;
        private int PageSize;

        public Action5306(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5306, httpGet)
        {

        }

        public override void BuildPacket()
        {
            //this.PushIntoStack(dsItemCollection.Length);
            //foreach (var item in dsItemCollection)
            //{
            //    DataStruct dsItem = new DataStruct();
            //    dsItem.PushIntoStack(UserName);
            //    dsItem.PushIntoStack(UserLv);
            //    dsItem.PushIntoStack(UserStatus);

            //    this.PushIntoStack(dsItem);
            //}

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref Ops)
                    && httpGet.GetInt("PageIndex", ref PageIndex)
                    && httpGet.GetInt("PageSize", ref PageSize))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            return true;
        }
    }
}