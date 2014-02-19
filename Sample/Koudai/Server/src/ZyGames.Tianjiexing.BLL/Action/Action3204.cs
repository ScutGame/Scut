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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3204_赛跑开始接口
    /// </summary>
    public class Action3204 : BaseAction
    {
        private int _petId;
        private string _friendId;
        private int petMinLevel = ConfigEnvSet.GetInt("Pet.MinLevel");


        public Action3204(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3204, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PetId", ref _petId))
            {
                httpGet.GetString("FriendId", ref _friendId);
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            if (!string.IsNullOrEmpty(_friendId) && new GameDataCacheSet<GameUser>().FindKey(_friendId) == null)
            {
                UserCacheGlobal.LoadOffline(_friendId);
            }
            if (ContextUser.UserExtend == null || (_petId != petMinLevel && ContextUser.UserExtend.LightPetID != _petId))
            {
                //SaveLog(string.Format("宠物赛跑开始接口请求ID{0}，当前点亮ID{1}", _petId, ContextUser.UserExtend.LightPetID));
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St3204_PetNoEnable;
                return false;
            }

            PetRunPool petRunPool = new ShareCacheStruct<PetRunPool>().FindKey(Uid);
            if (petRunPool != null && petRunPool.IsRunning)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St3204_PetRunning;
                return false;
            }
            if (new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid) != null)
            {
                var userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);
                int maxNum = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.PetRun).MaxNum;
                if (userRestrain.UserExtend != null)
                {
                    if (userRestrain.UserExtend.PetRunTimes >= maxNum)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St3204_PetRunTimesOut;
                        return false;
                    }
                }
            }
            if (new GameDataCacheSet<UserDailyRestrain>().FindKey(_friendId) != null)
            {
                var userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(_friendId);
                int maxNum = new ShareCacheStruct<DailyRestrainSet>().FindKey(RestrainType.PetHelp).MaxNum;
                if (userRestrain.UserExtend != null)
                {
                    if (userRestrain.UserExtend.PetHelp >= maxNum)
                    {
                        this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St3204_PetHelpeTimesOut;
                        return false;
                    }
                }
            }

            decimal minusNum = (decimal)FestivalHelper.TortoiseHare(ContextUser.UserID);
            ContextUser.UserExtend.UpdateNotify(obj => 
                {
                    ContextUser.UserExtend.LightPetID = 0;
                    return true;
                }
                );
            //ContextUser.Update();
            var petCacheSet = new ShareCacheStruct<PetRunPool>();
            var petInfo = new ConfigCacheSet<PetInfo>().FindKey(_petId) ?? new PetInfo();
            if (petRunPool == null)
            {
                petRunPool = new PetRunPool(Uid);
                petCacheSet.Add(petRunPool);
                petRunPool = petCacheSet.FindKey(Uid);
            }
            petRunPool.FriendID = _friendId;
            petRunPool.PetID = _petId;
            petRunPool.StartDate = DateTime.Now;
            petRunPool.EndDate = DateTime.Now.AddSeconds(petInfo.ColdTime);
            petRunPool.GameCoin = (int)Math.Floor(petInfo.CoinRate * ContextUser.UserLv * minusNum);
            petRunPool.ObtainNum = (int)Math.Floor(petInfo.ObtainRate * ContextUser.UserLv * minusNum);
            petRunPool.InterceptUser = string.Empty;



            if (new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid) != null)
            {
                var userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid);
                userRestrain.UserExtend.UpdateNotify(obj => 
                    {
                        userRestrain.UserExtend.PetRunTimes = MathUtils.Addition(userRestrain.UserExtend.PetRunTimes, 1);
                        return true;
                    });
                //userRestrain.Update();
            }

            if (new GameDataCacheSet<UserDailyRestrain>().FindKey(petRunPool.FriendID) != null)
            {
                var userRestrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(petRunPool.FriendID);
                if (userRestrain.UserExtend == null) userRestrain.UserExtend = new DailyUserExtend();
                userRestrain.UserExtend.UpdateNotify(obj => 
                    {
                        userRestrain.UserExtend.PetHelp = MathUtils.Addition(userRestrain.UserExtend.PetHelp, 1);
                        return true;
                    });
                //userRestrain.Update();
            }
            return true;
        }
    }
}