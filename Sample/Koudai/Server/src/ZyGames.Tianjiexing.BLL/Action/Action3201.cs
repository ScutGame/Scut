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
using ZyGames.Tianjiexing.BLL.Base;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3201_赛跑界面接口
    /// </summary>
    public class Action3201 : BaseAction
    {
        private int _runTimes;
        private int _interceptTimes;
        private int _helpTimes;
        private int _coldTime;
        private string _petHead;
        private int _petId;


        public Action3201(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3201, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(ContextUser.GoldNum);
            this.PushIntoStack(ContextUser.GameCoin);
            this.PushIntoStack(_runTimes);
            this.PushIntoStack(_interceptTimes);
            this.PushIntoStack(_helpTimes);
            this.PushIntoStack(_coldTime);
            this.PushIntoStack(_petHead.ToNotNullString());
            this.PushIntoStack(_petId);
            SaveLog(string.Format("3201下发宠物ID{0}", _petId));
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {

            if (new GameDataCacheSet<UserFunction>().FindKey(Uid, FunctionEnum.PetRun) == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_NoFun;
                return false;
            }
            UserDailyRestrain restrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);
            if (restrain != null && restrain.UserExtend != null)
            {
                var dailyCacheSet = new ShareCacheStruct<DailyRestrainSet>();
                if (dailyCacheSet.FindKey(RestrainType.PetRun) != null)
                    _runTimes = MathUtils.Subtraction(dailyCacheSet.FindKey(RestrainType.PetRun).MaxNum, restrain.UserExtend.PetRunTimes, 0);
                if (dailyCacheSet.FindKey(RestrainType.PetIntercept) != null)
                    _interceptTimes = MathUtils.Subtraction(dailyCacheSet.FindKey(RestrainType.PetIntercept).MaxNum, restrain.UserExtend.PetIntercept, 0);
                if (dailyCacheSet.FindKey(RestrainType.PetHelp) != null)
                    _helpTimes = MathUtils.Subtraction(dailyCacheSet.FindKey(RestrainType.PetHelp).MaxNum, restrain.UserExtend.PetHelp, 0);
            }
            var petRun = new ShareCacheStruct<PetRunPool>().FindKey(Uid);
            if (petRun != null)
            {
                //问题：在赛跑时有重刷点亮宠物后，等赛跑完服务端与客户端记录宠物ID不一致，原因是赛跑完有将宠物ID清除
                if (petRun.PetID > 0 && petRun.ColdTime == 0)
                {
                    UserHelper.ProcessPetPrize(petRun);
                }
                _coldTime = petRun.ColdTime;
                _petHead = (new ConfigCacheSet<PetInfo>().FindKey(petRun.PetID) ?? new PetInfo()).PetHead;
                if (ContextUser.UserExtend != null)
                    _petId = ContextUser.UserExtend.LightPetID;
            }
            return true;
        }
    }
}