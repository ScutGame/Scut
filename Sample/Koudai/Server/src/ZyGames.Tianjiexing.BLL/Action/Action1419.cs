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
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Common;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1419_传承接口
    /// </summary>
    public class Action1419 : BaseAction
    {
        private int ops;


        public Action1419(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1419, httpGet)
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
            List<GeneralHeritage> heritageList = new List<GeneralHeritage>();
            if (ContextUser.HeritageList.Count > 0)
            {
                heritageList = ContextUser.HeritageList.ToList();
                GeneralHeritage heritage = heritageList.Find(m => m.Type == HeritageType.Heritage);
                GeneralHeritage gheritage = heritageList.Find(m => m.Type == HeritageType.IsHeritage);
                if (heritage == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1418_HeritageNotEnough;
                    return false;
                }
                else if (gheritage == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1419_IsHeritageNotEnough;
                    return false;
                }
                var cacheSet = new GameDataCacheSet<UserGeneral>();
                UserGeneral general = cacheSet.FindKey(ContextUser.UserID, heritage.GeneralID);
                UserGeneral heritagegeneral = cacheSet.FindKey(ContextUser.UserID, gheritage.GeneralID);
                if (general == null || heritagegeneral == null || general.GeneralID == heritagegeneral.GeneralID)
                {
                    return false;
                }
                if (general.GeneralID == heritagegeneral.GeneralID)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1419_HeritageNotInIsHeritage;
                    return false;
                }
                if (general.HeritageType == HeritageType.Heritage)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1419_HeritageInUse;
                    return false;
                }
                int useGold = 0;
                int opsType = 0;
                int vipLv = 0;
                if (ops == 1)
                {
                    ErrorCode = ops;
                    if (!IsNomalHeritage(ContextUser.UserID, ops))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1419_DanInsufficientHeritage;
                        return false;
                    }
                }
                else if (ops == 2)
                {
                    opsType = 2;
                    useGold = HeritageUseGold(opsType, out vipLv);
                    ErrorCode = ops;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1419_GoldHeritage, useGold);
                    return false;
                }
                else if (ops == 3)
                {
                    opsType = 2;
                    useGold = HeritageUseGold(opsType, out vipLv);
                    if (ContextUser.GoldNum < useGold)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                    if (gheritage.opsType != opsType)
                    {
                        return false;
                    }
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                }

                else if (ops == 4)
                {
                    opsType = 3;
                    useGold = HeritageUseGold(opsType, out vipLv);
                    ErrorCode = ops;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1419_ExtremeHeritage, useGold);
                    return false;
                }
                else if (ops == 5)
                {
                    opsType = 3;
                    useGold = HeritageUseGold(opsType, out vipLv);
                    if (vipLv > ContextUser.VipLv)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                        return false;
                    }
                    if (ContextUser.GoldNum < useGold)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                        return false;
                    }
                    if (gheritage.opsType != opsType)
                    {
                        return false;
                    }
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, useGold);
                }
                ErrorCode = ops;
                heritagegeneral.GeneralLv = gheritage.GeneralLv;
                general.GeneralStatus = GeneralStatus.YinCang;
                var embattleList = new GameDataCacheSet<UserEmbattle>().FindAll(ContextUser.UserID, s => s.GeneralID == general.GeneralID);
                foreach (var embattle in embattleList)
                {
                    embattle.GeneralID = 0;
                }
                //heritagegeneral.TrainingPower = gheritage.PowerNum;
                //heritagegeneral.TrainingSoul = gheritage.SoulNum;
                //heritagegeneral.TrainingIntellect = gheritage.IntellectNum;
                //heritagegeneral.HeritageType = HeritageType.IsHeritage;
                //general.HeritageType = HeritageType.Heritage;
                ContextUser.HeritageList = new CacheList<GeneralHeritage>();
                ErrorInfo = LanguageManager.GetLang().St1419_HeritageSuccess;
            }
            return true;
        }

        public static int HeritageUseGold(int opsType, out int vipLv)
        {
            vipLv = 0;
            int useGold = 0;
            if (!string.IsNullOrEmpty(GameConfigSet.HeritageList))
            {
                var opsInfosList = JsonUtils.Deserialize<List<OpsInfo>>(GameConfigSet.HeritageList);
                OpsInfo opsInfo = opsInfosList.Find(m => m.Type == opsType);
                if (opsInfo != null)
                {
                    useGold = opsInfo.UseGold;
                    vipLv = opsInfo.VipLv;
                }
            }
            return useGold;
        }

        public static bool IsNomalHeritage(string userID, int opsType)
        {
            int itemid = 0;
            int itemnum = 0;
            OpsInfo opsInfo = GeneralHelper.HeritageOpsInfo(opsType);
            if (opsInfo != null)
            {
                itemid = opsInfo.ItemID;
                itemnum = opsInfo.ItemNum;
            }
            if (itemid > 0 && itemnum > 0)
            {
                var package = UserItemPackage.Get(userID);
                if (package != null)
                {
                    int num = 0;
                    var useritem = package.ItemPackage.FindAll(s => s.ItemID == itemid);
                    foreach (var itemInfo in useritem)
                    {
                        num += itemInfo.Num;
                    }
                    if (num >= itemnum)
                    {
                        UserItemHelper.UseUserItem(userID, itemid, itemnum);
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }
    }
}