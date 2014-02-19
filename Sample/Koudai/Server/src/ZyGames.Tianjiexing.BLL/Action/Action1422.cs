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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1422_赠送礼物接口
    /// </summary>
    public class Action1422 : BaseAction
    {
        private int generalID;
        private string userItemID;
        private int ops;


        public Action1422(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1422, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetInt("Ops", ref ops))
            {
                httpGet.GetString("UserItemID", ref userItemID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            UserHelper.ChechDailyRestrain(ContextUser.UserID);
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (general == null)
            {
                return false;
            }
            if (general.FeelLv >= GiftHelper.maxFeelLv)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1422_MaxFeelFull;
                return false;
            }
            int useGold = ConfigEnvSet.GetInt("User.PresentationGold");
            int feelNum = ConfigEnvSet.GetInt("User.PresentationFeelNum");
            int maxSatiationNum = ConfigEnvSet.GetInt("User.FeelMaxSatiationNum");
            decimal generalEffect = ConfigEnvSet.GetDecimal("Gift.GeneralEffectNum");
            if (ops == 1)
            {
                var package = UserItemPackage.Get(ContextUser.UserID);
                var useritem = package.ItemPackage.Find(m => m.UserItemID.Equals(userItemID));
                if (useritem == null)
                {
                    return false;
                }
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(useritem.ItemID);
                if (itemInfo != null)
                {
                    if (general.SaturationNum >= maxSatiationNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1422_FeelMaxSatiationNum;
                        return false;
                    }
                    int giftEffect = itemInfo.EffectNum;
                    //佣兵喜欢类型的礼物，好感度加成
                    GeneralInfo generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
                    if (generalInfo != null && itemInfo.GiftType == generalInfo.GiftType)
                    {
                        decimal sumGeneralEffect = MathUtils.Addition(generalEffect, 1, decimal.MaxValue);
                        giftEffect = MathUtils.RoundCustom(giftEffect * sumGeneralEffect).ToShort();
                    }
                    GiftHelper.GeneralFeelUpgrade(general, giftEffect, itemInfo.SatiationNum);
                    UserItemHelper.UseUserItem(ContextUser.UserID, itemInfo.ItemID, 1);
                    ErrorCode = ops;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1422_PresentationFeelNum, giftEffect);
                }
            }
            else if (ops == 2)
            {
                int addNum = UseGoldZengSong(ContextUser.UserID);
                useGold = MathUtils.Addition(useGold, addNum);
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1422_PresentationUseGold, useGold, feelNum);
                return false;
            }
            else if (ops == 3)
            {
                if (GiftHelper.SurplusGoldNum(ContextUser.UserID) <= 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1422_PresentationGoldNum;
                    return false;
                }
                UserDailyRestrain userDaily = new GameDataCacheSet<UserDailyRestrain>().FindKey(ContextUser.UserID);
                int addNum = UseGoldZengSong(ContextUser.UserID);
                useGold = MathUtils.Addition(useGold, addNum);
                if (ContextUser.GoldNum < useGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }

                if (userDaily != null)
                {
                    if (userDaily.UserExtend == null)
                    {
                        userDaily.UserExtend = new DailyUserExtend();
                    }
                    userDaily.UserExtend.UpdateNotify(obj =>
                    {
                        userDaily.UserExtend.GoldNum = MathUtils.Addition(userDaily.UserExtend.GoldNum, (short)1);
                        return true;
                    });
                }
                int feelExp = general.FeelExperience;
                int upExp = MathUtils.Addition(feelExp, feelNum);
                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                GiftHelper.GeneralFeelUpgrade(general, feelNum, 0);
                //TraceLog.ReleaseWrite(string.Format("玩家{0}（通行证ID）的佣兵{1}（ID）{2}使用晶石赠送增加好感度{3}。好感度从{4}上升到{5}。",
                //    ContextUser.Pid,
                //    ContextUser.UserID,
                //    ContextUser.NickName, feelNum,
                //    feelExp, upExp));
                ErrorCode = ops;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1422_PresentationFeelNum, feelNum);
            }
            return true;
        }

        public static int UseGoldZengSong(string userID)
        {
            int addNum = 0;
            UserDailyRestrain userDaily = new GameDataCacheSet<UserDailyRestrain>().FindKey(userID);

            if (userDaily != null && userDaily.UserExtend != null && userDaily.UserExtend.GoldNum > 0)
            {
                addNum = userDaily.UserExtend.GoldNum * 10;
            }
            return addNum;
        }
    }
}