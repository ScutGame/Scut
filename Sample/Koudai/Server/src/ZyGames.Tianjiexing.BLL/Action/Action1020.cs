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
using System.Data;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1020_领取俸禄接口
    /// </summary>
    public class Action1020 : BaseAction
    {
        public Action1020(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1020, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (true)
            {
                return true;
            }
        }

        public override bool TakeAction()
        {
            UserDailyRestrain dailyRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
            int baseNum = ConfigEnvSet.GetInt("FengLu");
            int useGold = (ContextUser.UserLv * baseNum);

            if (dailyRestrain != null && dailyRestrain.RefreshDate.Date == DateTime.Now.Date && dailyRestrain.Funtion8 == 0)
            {
                if (dailyRestrain.Funtion8 == 0)
                {
                    dailyRestrain.Funtion8 = MathUtils.Addition(dailyRestrain.Funtion8, 1, int.MaxValue);
                    //dailyRestrain.Update();
                    ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, useGold, int.MaxValue);
                    //ContextUser.Update();
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1020_FengLu, useGold);
                }
            }
            return true;
        }
    }
}