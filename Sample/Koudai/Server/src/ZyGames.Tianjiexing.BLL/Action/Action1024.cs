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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.DataModel;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1024_新手卡激活接口
    /// </summary>
    public class Action1024 : BaseAction
    {
        private string cardID;


        public Action1024(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1024, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("CardID", ref cardID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (ContextUser.CardID == null || string.IsNullOrEmpty(ContextUser.CardID.TrimEnd()))
            {
                short cardLv = (short)ConfigEnvSet.GetInt("UserCard.ActivationPackageLv");
                int handNum = 0;
                DailyRestrainSet restrainSet = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.NewHand);
                if (restrainSet != null)
                {
                    handNum = restrainSet.MaxNum;
                }
                int cardUserID = 0;
                cardID = cardID.Trim().Replace('c', 'C');
                if (string.IsNullOrEmpty(cardID) ||
                    !cardID.StartsWith("C") ||
                    !int.TryParse(cardID.Substring(1), out cardUserID))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1024_NewHandFail;
                    return false;
                }

                string cardReplaceID = cardID.Replace('c', 'C');
                GameUser user = UserCacheGlobal.CheckLoadUser(cardUserID.ToString());
                if (user == null || user.UserExtend.CardUserID != cardReplaceID)
                {
                    return false;
                }
                if (user.CardTimes >= handNum || ContextUser.UserLv >= cardLv)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1024_NewHandFail;
                    return false;
                }
                user.CardTimes = MathUtils.Addition(user.CardTimes, 1);
                ContextUser.CardID = cardID;
                ErrorCode = 1;
                ErrorInfo = LanguageManager.GetLang().St1024_NewHandSuccess;
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1024_NewHandFail;
                return false;
            }
            return true;
        }


    }
}