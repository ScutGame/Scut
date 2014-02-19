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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;

using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10011_升级黑土地接口
    /// </summary>
    public class Action10011 : BaseAction
    {
        private int ops = 0;

        public Action10011(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10011, httpGet)
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
            if (ContextUser.VipLv < 4)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St_VipNotEnough;
                return false;
            }
            List<UserLand> uLandArray = new GameDataCacheSet<UserLand>().FindAll(ContextUser.UserID, u => u.IsRedLand == 1);
            if (uLandArray.Count < 9)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St10011_RedLandNotEnough;
                return false;
            }
            if (uLandArray.Count > 0 && uLandArray[uLandArray.Count - 1].IsBlackLand == 1)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St10011_BlackLandFull;
                return false;
            }

            List<UserLand> landArray = new GameDataCacheSet<UserLand>().FindAll(ContextUser.UserID, u => u.IsBlackLand == 2);
            landArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return ((int)x.LandPositon).CompareTo((int)y.LandPositon);
            });
            int position = 0;
            if (landArray.Count > 0)
            {
                position = landArray[0].LandPositon;
            }
            int sumGold = GetPostionUseGold(position);

            if (ops == 1)
            {
                this.ErrorCode = 1;
                this.ErrorInfo = string.Format(LanguageManager.GetLang().St10011_UpBlackLandUseGold, sumGold);
                return false;
            }
            else if (ops == 2)
            {
                if (ContextUser.GoldNum < sumGold)
                {
                    this.ErrorCode = 2;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                UserLand land = new GameDataCacheSet<UserLand>().FindKey(ContextUser.UserID, position);
                if (land != null && land.IsRedLand == 1 && land.IsBlackLand == 2)
                {
                    if (land.IsRedLand == 1 && land.IsBlackLand == 2)
                    {
                        ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, sumGold, int.MaxValue);

                        land.IsBlackLand = 1;
                        UserLogHelper.AppenLandLog(ContextUser.UserID, 3, 0, position, sumGold, 0, 0, 0);
                    }
                }
                else if (land != null && land.IsRedLand == 2)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St10011_NotRedLand;
                    return false;
                }
                else
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St10010_UpRedLandNotEnough;
                    return false;
                }
            }
            return true;
        }

        public int GetPostionUseGold(int landPostion)
        {
            int useGold = 0;
            if (landPostion == 1)
            {
                useGold = 1000;
            }
            else if (landPostion == 2)
            {
                useGold = 1200;
            }
            else if (landPostion == 3)
            {
                useGold = 1400;
            }
            else if (landPostion == 4)
            {
                useGold = 1600;
            }
            else if (landPostion == 5)
            {
                useGold = 1800;
            }
            else if (landPostion == 6)
            {
                useGold = 2000;
            }
            else if (landPostion == 7)
            {
                useGold = 2200;
            }
            else if (landPostion == 8)
            {
                useGold = 2400;
            }
            else if (landPostion == 9)
            {
                useGold = 2600;
            }
            return useGold;
        }
    }
}