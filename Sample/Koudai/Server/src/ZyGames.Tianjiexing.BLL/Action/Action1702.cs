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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1702_队列加速接口
    /// </summary>
    public class Action1702 : BaseAction
    {
        private string queueID = string.Empty;
        private int ops = 0;

        public Action1702(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1702, httpGet)
        {

        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("QueueID", ref queueID)
                && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //消除冷却以分钟为单位
            UserQueue queue = new GameDataCacheSet<UserQueue>().FindKey(ContextUser.UserID, queueID);
            if (queue == null)
            {
                return false;
            }
            int queueColdTime = queue.DoRefresh();
            queueColdTime = queueColdTime < 0 ? 0 : queueColdTime;
            if (ops == 1)
            {
                //加速所需晶石
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1702_UseGold, GetPrice());
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum >= GetPrice())
                {
                    ErrorCode = ops;

                    if (ContextUser.GoldNum >= GetPrice())
                    {
                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, GetPrice(), int.MaxValue);
                        //ContextUser.Update();
                        queue.ColdTime = 0;
                        queue.TotalColdTime = 0;
                        //queue.Update();
                    }
                }
                else
                {

                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().UrlElement;
                return false;
            }
            return true;
        }

        private int GetPrice()
        {
            int currGoldNum = 0;
            UserQueue queue = new GameDataCacheSet<UserQueue>().FindKey(ContextUser.UserID, queueID);
            if (queue != null)
            {
                int queueColdTime = (queue.DoRefresh() / 60);
                currGoldNum = MathUtils.Addition(queueColdTime, 1, int.MaxValue);
            }
            return currGoldNum;
        }
    }
}