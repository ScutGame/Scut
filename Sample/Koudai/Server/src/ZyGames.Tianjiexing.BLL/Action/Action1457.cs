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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1457_法宝延寿接口
    /// </summary>
    public class Action1457 : BaseAction
    {
        private int ops;


        public Action1457(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1457, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (ops == 1)
            {
                ErrorCode = ops;
                ErrorInfo = LanguageManager.GetLang().St1457_UseLifeExtension;
                return false;
            }
            else if (ops == 2)
            {
                var package = UserItemPackage.Get(ContextUser.UserID);
                UserItemInfo[] userItemArray = package.ItemPackage.FindAll(m => !m.IsRemove && m.ItemType == ItemType.DaoJu && m.PropType == 11).ToArray();
                if (userItemArray.Length == 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1457_LifeExtensionNotEnough;
                    return false;
                }
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItemArray[0].ItemID);
                if (itemInfo == null)
                {
                    return false;
                }
                UserTrump userTrump = new GameDataCacheSet<UserTrump>().FindKey(ContextUser.UserID, TrumpInfo.CurrTrumpID);
                if (userTrump != null)
                {
                    TrumpInfo trumpInfo = new ConfigCacheSet<TrumpInfo>().FindKey(TrumpInfo.CurrTrumpID, userTrump.TrumpLv);
                    if (trumpInfo != null)
                    {
                        if (userTrump.LiftNum < trumpInfo.MaxLift)
                        {
                            userTrump.LiftNum = MathUtils.Addition(userTrump.LiftNum, itemInfo.EffectNum, trumpInfo.MaxLift);
                            UserItemHelper.UseUserItem(ContextUser.UserID, userItemArray[0].ItemID, 1);
                            var usergeneral = UserGeneral.GetMainGeneral(ContextUser.UserID);
                            if (userTrump.LiftNum > 0 && usergeneral != null)
                            {
                                usergeneral.RefreshMaxLife();
                            }
                            ErrorCode = ops;
                            ErrorInfo = string.Format(LanguageManager.GetLang().St1457_ChangeLifeNum, itemInfo.EffectNum);
                        }
                        else
                        {
                            ErrorCode = LanguageManager.GetLang().ErrorCode;
                            ErrorInfo = LanguageManager.GetLang().St1457_MaxLifeExtension;
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}