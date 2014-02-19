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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1255_卖出附魔符接口
    /// </summary>
    public class Action1255 : BaseAction
    {
        private string userEnchantID;


        public Action1255(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1255, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserEnchantID", ref userEnchantID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var package = UserEnchant.Get(ContextUser.UserID);
            if (package == null)
            {
                return false;
            }
            UserEnchantInfo userEnchantInfo = package.EnchantPackage.Find(m => m.UserEnchantID == userEnchantID);
            if (userEnchantInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1256_EnchantNotEnough;
                return false;
            }
            EnchantInfo enchant = new ConfigCacheSet<EnchantInfo>().FindKey(userEnchantInfo.EnchantID);
            EnchantLvInfo enchantLvInfo = new ConfigCacheSet<EnchantLvInfo>().FindKey(userEnchantInfo.EnchantID, userEnchantInfo.EnchantLv);
            if (enchant != null && enchantLvInfo != null)
            {
                ContextUser.GameCoin = MathUtils.Addition(ContextUser.GameCoin, enchantLvInfo.CoinPrice);
                if (ContextUser.UserExtend == null)
                {
                    ContextUser.UserExtend = new GameUserExtend();
                }
                ContextUser.UserExtend.UpdateNotify(obj =>
                {
                    ContextUser.UserExtend.MoJingNum = MathUtils.Addition(ContextUser.UserExtend.MoJingNum, enchantLvInfo.MoJingPrice);
                    return true;
                });
                UserLogHelper.AppenEnchantLog(ContextUser.UserID, 1, userEnchantInfo, new CacheList<SynthesisInfo>());
                package.RemoveEnchant(userEnchantInfo);
            }
            return true;
        }
    }
}