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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1418_传承人与被传承人选择接口
    /// </summary>
    public class Action1418 : BaseAction
    {
        private int generalID;
        private HeritageType heritageType;


        public Action1418(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1418, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID)
                 && httpGet.GetEnum("HeritageType", ref heritageType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            CacheList<GeneralHeritage> heritageList = new CacheList<GeneralHeritage>();
            GeneralHeritage heritage = new GeneralHeritage();
            UserGeneral general = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            if (general == null)
            {
                return false;
            }
            if (ContextUser.HeritageList.Count > 0)
            {
                heritageList = ContextUser.HeritageList;
                if (heritageList.Find(m => m.Type == heritageType) != null)
                {
                    heritage = heritageList.Find(m => m.Type == heritageType);
                }
            }
            if (heritageType == HeritageType.Heritage)
            {
                int opsid = 0;
                OpsInfo opsInfo = GeneralHelper.HeritageOpsInfo(opsid);
                GeneralHeritage gHeritage = heritageList.Find(m => m.Type == HeritageType.IsHeritage);
                if (opsInfo != null)
                {
                    short genlv = MathUtils.Addition(gHeritage == null ? 0.ToShort() : gHeritage.GeneralLv, 3.ToShort());
                    if (gHeritage != null && general.GeneralLv < genlv)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1418_HeritageLvLow;
                        return false;
                    }
                    ContextUser.HeritageList.Remove(heritage);
                    heritage.GeneralID = generalID;
                    heritage.Type = heritageType;
                    heritage.GeneralLv = MathUtils.RoundCustom(heritage.GeneralLv * opsInfo.Num).ToShort();
                    heritage.GeneralLv = general.GeneralLv;
                    heritage.PowerNum = MathUtils.RoundCustom(heritage.PowerNum * opsInfo.Num).ToShort();
                    if (heritage.PowerNum < general.TrainingPower)
                    {
                        heritage.PowerNum = general.TrainingPower;
                    }
                    heritage.SoulNum = MathUtils.RoundCustom(heritage.SoulNum * opsInfo.Num).ToShort();
                    if (heritage.SoulNum < general.SoulNum)
                    {
                        heritage.SoulNum = general.TrainingSoul;
                    }
                    heritage.IntellectNum = MathUtils.RoundCustom(heritage.IntellectNum * opsInfo.Num).ToShort();
                    if (heritage.IntellectNum < general.IntellectNum)
                    {
                        heritage.IntellectNum = general.TrainingIntellect;
                    }
                    heritage.opsType = 1;
                    ContextUser.HeritageList.Add(heritage);
                }
            }
            else if (heritageType == HeritageType.IsHeritage)
            {
                ContextUser.HeritageList = new CacheList<GeneralHeritage>();
                heritage.GeneralID = generalID;
                heritage.GeneralLv = general.GeneralLv;
                heritage.Type = heritageType;
                heritage.PowerNum = general.TrainingPower;
                heritage.SoulNum = general.TrainingSoul;
                heritage.IntellectNum = general.TrainingIntellect;
                ContextUser.HeritageList.Add(heritage);
            }
            return true;
        }
    }
}