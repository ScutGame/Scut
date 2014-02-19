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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10008_开启土地接口
    /// </summary>
    public class Action10008 : BaseAction
    {
        private int landPostion = 0;
        private int Ops;


        public Action10008(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10008, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("LandPostion", ref landPostion) && httpGet.GetInt("Ops", ref Ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int sumGold = GetPostionUseGold(landPostion);
            UserPlant plant = new GameDataCacheSet<UserPlant>().FindKey(ContextUser.UserID);
            UserLand uLands = new GameDataCacheSet<UserLand>().FindKey(ContextUser.UserID, landPostion);
            if (uLands != null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St10008_LandPostionIsOpen;
                return false;
            }

            if (Ops == 1)
            {
                ErrorCode = 1;
                ErrorInfo = string.Format(LanguageManager.GetLang().St10008_OpenLandPostion, sumGold);
                return false;
            }
            else if (Ops == 2)
            {
                if (ContextUser.GoldNum < sumGold)
                {
                    ErrorCode = 2;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                if (ContextUser.GoldNum >= sumGold)
                {
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, sumGold, int.MaxValue);
                    //ContextUser.Update();

                    plant.LandNum = landPostion;
                    //plant.Update();

                    UserLand land = new UserLand()
                                        {
                                            UserID = ContextUser.UserID,
                                            GeneralID = 0,
                                            LandPositon = landPostion,
                                            PlantType = PlantType.Experience,
                                            IsRedLand = 2,
                                            IsBlackLand = 2,
                                            IsGain = 2,
                                            PlantQuality = PlantQualityType.PuTong
                                        };
                    var cacheSet = new GameDataCacheSet<UserLand>();
                    cacheSet.Add(land);
                    UserLogHelper.AppenLandLog(ContextUser.UserID, 1, 0, landPostion, sumGold, 0, 0, 0);
                }
            }
            return true;
        }

        public int GetPostionUseGold(int landPostion)
        {
            int useGold = 0;
            if (landPostion == 2)
            {
                useGold = 50;
            }
            else if (landPostion == 3)
            {
                useGold = 100;
            }
            else if (landPostion == 4)
            {
                useGold = 200;
            }
            else if (landPostion == 5)
            {
                useGold = 400;
            }
            else if (landPostion == 6)
            {
                useGold = 600;
            }
            else if (landPostion == 7)
            {
                useGold = 800;
            }
            else if (landPostion == 8)
            {
                useGold = 1000;
            }
            else if (landPostion == 9)
            {
                useGold = 1200;
            }
            return useGold;
        }
    }
}