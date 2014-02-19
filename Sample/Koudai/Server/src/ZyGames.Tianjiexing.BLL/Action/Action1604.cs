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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1604_材料掉落副本列表接口
    /// </summary>
    public class Action1604 : BaseAction
    {
        private int _materialsID;
        private PlotInfo _plotInfo;


        public Action1604(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1604, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_plotInfo == null ? 0 : _plotInfo.CityID);
            PushIntoStack(_plotInfo == null ? LanguageManager.GetLang().shortInt : (short)_plotInfo.PlotType);
            PushIntoStack(_plotInfo == null ? 0 : _plotInfo.PlotID);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MaterialsID", ref _materialsID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ItemBaseInfo itemBaseInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(_materialsID);
            if (itemBaseInfo.ItemType == ItemType.CaiLiao)
            {
                //寻材料
                List<MonsterInfo> monsterArray = new ConfigCacheSet<MonsterInfo>().FindAll(m => m.ItemID == _materialsID);
                if (monsterArray.Count > 0)
                {
                    MonsterInfo monsterInfo = monsterArray[0];
                    var plotEmbattles = new ConfigCacheSet<PlotEmbattleInfo>().FindAll(m => m.MonsterID == monsterInfo.MonsterID);
                    foreach (var embattle in plotEmbattles)
                    {
                        PlotNPCInfo npcInfo = new ConfigCacheSet<PlotNPCInfo>().FindKey(embattle.PlotNpcID);
                        if (npcInfo != null)
                        {
                            PlotInfo temp = new ConfigCacheSet<PlotInfo>().FindKey(npcInfo.PlotID);
                            if (temp != null && temp.CityID > CurrCityID(ContextUser.UserLv))
                            {
                                ErrorCode = LanguageManager.GetLang().ErrorCode;
                                ErrorInfo = LanguageManager.GetLang().St1604_MaterialsCityID;
                                return false;
                            }
                            if (CheckPlot(temp))
                            {
                                _plotInfo = temp;
                                break;
                            }
                        }
                    }
                }
            }

            return true;
        }

        private bool CheckPlot(PlotInfo plotInfo)
        {
            if (plotInfo.CityID == 3 || plotInfo.CityID == 4)
            {
                CountryType countryType = plotInfo.CityID == 3 ? CountryType.M : CountryType.H;
                return ContextUser.CountryID == countryType;
            }
            return true;
        }

        public static int CurrCityID(short userlv)
        {
            int cityID = 0;
            List<CityInfo> cityInfosList = new ConfigCacheSet<CityInfo>().FindAll(m => m.CityType == 0 && m.MinLv <= userlv);
            cityInfosList.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return y.CityID.CompareTo(x.CityID);
            });
            if (cityInfosList.Count > 0)
            {
                cityID = cityInfosList[0].CityID;
            }
            return cityID;
        }
    }
}