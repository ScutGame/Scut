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
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 3203_宠物刷新接口
    /// </summary>
    public class Action3203 : BaseAction
    {
        private int _ops;
        private int petId;
        private int refreshGold = ConfigEnvSet.GetInt("Pet.MinRefrshGold");
        private int increaseGold = ConfigEnvSet.GetInt("Pet.IncreaseGold");
        private int petMinLevel = ConfigEnvSet.GetInt("Pet.MinLevel");
        private int _petVipLvCall = ConfigEnvSet.GetInt("Pet.VipLvCall");


        public Action3203(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action3203, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(petId);
            SaveLog(string.Format("宠物赛跑刷新ID{0}", petId));
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops))
            {
                return true;
            }
            return false;

        }

        public override bool TakeAction()
        {
            //1：刷新提示2：刷新确认3：召唤提示4：召唤 5:刷新失败
            ErrorCode = _ops;
            UserDailyRestrain restrain = new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid) ?? new UserDailyRestrain();
            int petUseGold = 0;
            if (_ops == 3 || _ops == 4)
            {
                if (ContextUser.VipLv < _petVipLvCall)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St3203_VipNotEnouht;
                    return false;

                }
                petUseGold = ConfigEnvSet.GetInt("Pet.CallGold");
            }
            else
            {
                if (restrain.UserExtend != null)
                {
                    petUseGold = MathUtils.Addition(refreshGold, restrain.UserExtend.PetRefeshNum * increaseGold);
                }
            }

            if (_ops == 1)
            {
                ErrorInfo = string.Format(LanguageManager.GetLang().St3203_RefeshPet, petUseGold);
            }
            else if (_ops == 3)
            {
                ErrorInfo = string.Format(LanguageManager.GetLang().St3203_ZhaohuangPet, petUseGold);
            }
            else
            {
                if (ContextUser.GoldNum < petUseGold)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St3203_GoldNotEnouht;
                    return false;
                }

                if (ContextUser.UserExtend == null) ContextUser.UserExtend = new GameUserExtend();
                if (ContextUser.UserExtend.LightPetID == 0) ContextUser.UserExtend.LightPetID = petMinLevel;

                int maxPetId = 0;
                var tempList = new ConfigCacheSet<PetInfo>().FindAll();
                if (tempList.Count > 0) maxPetId = tempList[tempList.Count - 1].PetId;
                if (ContextUser.UserExtend.LightPetID >= maxPetId)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St3203_MaxPet;
                    return false;

                }
                if (_ops == 2)
                {
                    var pet = new ConfigCacheSet<PetInfo>().FindKey(ContextUser.UserExtend.LightPetID) ?? new PetInfo();
                    if (RandomUtils.IsHit(pet.Light))
                    {
                        var petList = new ConfigCacheSet<PetInfo>().FindAll(m => m.PetId == MathUtils.Addition(ContextUser.UserExtend.LightPetID, 1));
                        if (petList.Count > 0)
                        {
                            ContextUser.UserExtend.LightPetID = petList[0].PetId;
                        }
                    }
                    else
                    {
                        ErrorCode = 5;
                    }
                    DoPrecess(restrain, petUseGold);

                }
                else if (_ops == 4)
                {
                    ContextUser.UserExtend.LightPetID = maxPetId;
                    DoPrecess(restrain, petUseGold);
                }
                else
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
            }
            return true;
        }

        private void DoPrecess(UserDailyRestrain restrain, int petUseGold)
        {
            if (ContextUser.GoldNum >= petUseGold)
            {
                petId = ContextUser.UserExtend.LightPetID;
                restrain.UserID = Uid;
                if (restrain.UserExtend == null) restrain.UserExtend = new DailyUserExtend();
                restrain.UserExtend.UpdateNotify(obj =>
                {
                    restrain.UserExtend.PetRefeshNum++;
                    return true;
                });
                //restrain.Update();

                ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, petUseGold, int.MaxValue);
                //ContextUser.Update();
            }
        }
    }
}