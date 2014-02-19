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
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1467_法宝附加属性删除接口
    /// </summary>
    public class Action1467 : BaseAction
    {
        private AbilityType propertyID;
        private int ops;


        public Action1467(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1467, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("PropertyID", ref propertyID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            GeneralProperty property = null;
            UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
            if (userTrump != null && userTrump.PropertyInfo.Count > 0)
            {
                property = userTrump.PropertyInfo.Find(m => m.AbilityType == propertyID);
            }
            if (property == null)
            {
                return false;
            }
            if (ops == 1)
            {
                ErrorCode = ops;
                ErrorInfo = LanguageManager.GetLang().St1467_WorshipGridNotEnough;
                return false;
            }
            else if (ops == 2)
            {
                ErrorCode = ops;
                userTrump.PropertyInfo.Remove(property);
                
                var usergeneral = UserGeneral.GetMainGeneral(ContextUser.UserID);
                if (usergeneral != null)
                {
                    usergeneral.RefreshMaxLife();
                }
            }

            return true;
        }
    }
}