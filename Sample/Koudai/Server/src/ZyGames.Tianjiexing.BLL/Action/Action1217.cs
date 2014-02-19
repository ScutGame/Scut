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
using System.Data;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1217_佣兵培养接口
    /// </summary>
    public class Action1217 : BaseAction
    {
        private int _ops;
        private MultipleType _multipleType;
        private int _generalId;
        private int _potenceNum;
        private int _thoughtNum;
        private int _intelligenceNum;
        private int _potential;
        private BringUpType _bringUpType;
        private Dictionary<int, int> _attributeIndexD = new Dictionary<int, int>();

        public Action1217(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1217, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_potenceNum);
            this.PushIntoStack(_thoughtNum);
            this.PushIntoStack(_intelligenceNum);
            PushIntoStack(_potential);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops)
                 && httpGet.GetEnum("MultipleType", ref _multipleType)
                 && httpGet.GetInt("GeneralID", ref _generalId)
                && httpGet.GetEnum("BringUpType", ref _bringUpType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            Int16 potenceNum = 0;
            Int16 thoughtNum = 0;
            Int16 intelligenceNum = 0;
            var cacheSetUserGeneral = new GameDataCacheSet<UserGeneral>();
            var cacheSetUserItem = new GameDataCacheSet<UserItemPackage>();
            var cacheSetBringUp = new ConfigCacheSet<BringUpInfo>();
            if (_ops == 1)
            {
                var attributeSetArray = ConfigEnvSet.GetString("UserBringUp.Set").Split(',');
                short multiple = attributeSetArray[_multipleType.ToShort() - 1].ToShort();
                var bringUpInfo = cacheSetBringUp.FindKey(_bringUpType.ToInt());
                var userItem = cacheSetUserItem.FindKey(UserId.ToString());
                var item = userItem != null && userItem.ItemPackage != null
                               ? userItem.ItemPackage.Find(s => s.ItemID == ConfigEnvSet.GetInt("User.DrugItemID"))
                               : null;
                var userGeneral = cacheSetUserGeneral.FindKey(UserId.ToString(), _generalId);
                if (_bringUpType == BringUpType.BaiJinPeiYang && ContextUser.VipLv < 3)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1217_NotBaiJin;
                    return false;
                }
                if (_bringUpType == BringUpType.ZhiZunPeiYang && ContextUser.VipLv < 5)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1217_NotZhiZhun;
                    return false;
                }
                if (userGeneral == null || bringUpInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().LoadDataError;
                    return false;
                }
                if (userGeneral.Potential <= 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1217_NotPotential;
                    return false;
                }

                if (bringUpInfo.UseUpType == 2 && ContextUser.GoldNum < (bringUpInfo.UseUpNum * multiple))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                if (bringUpInfo.UseUpType == 1 && (item == null || item.Num < (bringUpInfo.UseUpNum * multiple)))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1217_NotItem;
                    return false;
                }
                var attributeChancePren = new int[3];
                var attributeChance = bringUpInfo.AttributeChance;
                attributeChancePren[0] = attributeChance.PotenceNum * 10;
                attributeChancePren[1] = attributeChance.ThoughtNum * 10;
                attributeChancePren[2] = attributeChance.IntelligenceNum * 10;
                GetGeneralAttribute(attributeChancePren);
                
                foreach (var attributeIndex in _attributeIndexD)
                {
                    switch (attributeIndex.Value)
                    {
                        //力量
                        case 0:
                            var potenceValueArray = bringUpInfo.AttributeValueChance.PotenceValue.Split(',');
                            var potenceNumArray = bringUpInfo.AttributeValueChance.PotenceNum.Split(',');
                            potenceNum = GetAttributeValue(potenceValueArray, potenceNumArray);
                            potenceNum = (potenceNum * multiple).ToShort();

                            break;
                        //魂力
                        case 1:
                            var thoughtValueArray = bringUpInfo.AttributeValueChance.ThoughtValue.Split(',');
                            var thoughtNumArray = bringUpInfo.AttributeValueChance.ThoughtNum.Split(',');
                            thoughtNum = GetAttributeValue(thoughtValueArray, thoughtNumArray);
                            thoughtNum = (thoughtNum * multiple).ToShort();
                            break;
                        //智力
                        case 2:
                            var intelligenceValueArray = bringUpInfo.AttributeValueChance.IntelligenceValue.Split(',');
                            var intelligenceNumArray = bringUpInfo.AttributeValueChance.IntelligenceNum.Split(',');
                            intelligenceNum = GetAttributeValue(intelligenceValueArray, intelligenceNumArray);
                            intelligenceNum = (intelligenceNum * multiple).ToShort();
                            break;
                    }
                }
                userGeneral.PotenceNum = potenceNum;
                userGeneral.ThoughtNum = thoughtNum;
                userGeneral.IntelligenceNum = intelligenceNum;
                _potenceNum = userGeneral.PowerNum + potenceNum;
                _thoughtNum = userGeneral.SoulNum + thoughtNum;
                _intelligenceNum = userGeneral.IntellectNum + intelligenceNum;
                if (userGeneral.Potential <= 0 || userGeneral.Potential < (userGeneral.PotenceNum + userGeneral.ThoughtNum + userGeneral.IntelligenceNum))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1217_NotPotential;
                    return false;
                }
                switch (bringUpInfo.UseUpType)
                {
                    case 1:
                        UserItemHelper.UseUserItem(ContextUser.UserID, item.ItemID, (bringUpInfo.UseUpNum * multiple));
                        break;
                    case 2:
                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, (bringUpInfo.UseUpNum * multiple));
                        break;
                }
                _potential = userGeneral.Potential;
            }
            else
            {
                if (_ops == 2)
                {
                    var userGeneral = cacheSetUserGeneral.FindKey(UserId.ToString(), _generalId);
                    if (userGeneral == null)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().LoadDataError;
                        return false;
                    }
                    int attributeValue = userGeneral.PotenceNum + userGeneral.ThoughtNum + userGeneral.IntelligenceNum;
                    if (attributeValue > 0)
                    {
                        userGeneral.PowerNum = MathUtils.Addition(userGeneral.PowerNum, userGeneral.PotenceNum);
                        userGeneral.SoulNum = MathUtils.Addition(userGeneral.SoulNum, userGeneral.ThoughtNum);
                        userGeneral.IntellectNum = MathUtils.Addition(userGeneral.IntellectNum, userGeneral.IntelligenceNum);
                        userGeneral.Potential = MathUtils.Subtraction(userGeneral.Potential, attributeValue);
                    }
                    else
                    {
                        userGeneral.Potential = MathUtils.Addition(userGeneral.Potential, -attributeValue);
                    }
                    userGeneral.PotenceNum = 0;
                    userGeneral.ThoughtNum = 0;
                    userGeneral.IntelligenceNum = 0;
                    _potential = userGeneral.Potential;
                }

            }

            return true;
        }
        private void GetGeneralAttribute(int[] attributeChancePren)
        {
            int index = RandomUtils.GetHitIndexByTH(attributeChancePren);
            if (_attributeIndexD.Count < 2)
            {
                if (!_attributeIndexD.ContainsKey(index))
                {
                    _attributeIndexD.Add(index, index);
                }
                GetGeneralAttribute(attributeChancePren);
            }
        }
        private Int16 GetAttributeValue(string[] valueArray, string[] numArray)
        {
            var potenceNumPren = new int[valueArray.Length];

            int i = 0;
            foreach (var potenceNum in numArray)
            {
                potenceNumPren[i] = potenceNum.ToInt() * 10;
                i++;
            }
            int index = RandomUtils.GetHitIndexByTH(potenceNumPren);
            return valueArray[index].ToShort();
        }
    }

}