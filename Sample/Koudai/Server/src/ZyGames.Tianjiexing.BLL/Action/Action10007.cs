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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 10007_庄园冷却加速接口
    /// </summary>
    public class Action10007 : BaseAction
    {
        private int landPostion = 0;
        private int Ops;


        public Action10007(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10007, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("LandPostion", ref landPostion)
                 && httpGet.GetInt("Ops", ref Ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            int sumGold = 0;
            int userGold = ConfigEnvSet.GetInt("UserLand.UseGold");
            int intervalDate = ConfigEnvSet.GetInt("UserLand.IntervalDate");
            UserLand land = new GameDataCacheSet<UserLand>().FindKey(ContextUser.UserID, landPostion);
            if (land != null)
            {
                int subDate = (land.DoRefresh() / intervalDate);
                sumGold = (MathUtils.Addition(subDate, 1, int.MaxValue) * userGold);
            }
            if (Ops == 1)
            {
                this.ErrorCode = 1;
                this.ErrorInfo = string.Format(LanguageManager.GetLang().St10007_DoRefresh, sumGold);
                return false;
            }
            else if (Ops == 2)
            {
                if (ContextUser.GoldNum < sumGold)
                {
                    this.ErrorCode = 2;
                    this.ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                if (land != null && (DateTime.Now - land.GainDate).TotalSeconds < 28800)
                {
                    land.GainDate = MathUtils.SqlMinDate; 
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, sumGold, int.MaxValue);
                }
            }

            return true;
        }
    }
}