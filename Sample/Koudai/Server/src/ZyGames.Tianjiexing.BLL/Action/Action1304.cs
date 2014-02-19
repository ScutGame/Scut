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
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1304_命运水晶详情接口
    /// </summary>
    public class Action1304 : BaseAction
    {
        private string _userCrystalID = string.Empty;
        private string _attrNum = string.Empty;

        private UserCrystalInfo _userCrystal;
        private CrystalInfo _crystalInfo;
        private CrystalLvInfo _crystalLvInfo;
        private string toUserID;

        public Action1304(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1304, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_crystalInfo == null ? string.Empty : _crystalInfo.CrystalName.ToNotNullString());
            PushIntoStack(_crystalInfo == null ? string.Empty : _crystalInfo.HeadID.ToNotNullString());
            PushIntoStack(_userCrystal == null ? LanguageManager.GetLang().shortInt : _userCrystal.CrystalLv);
            PushIntoStack(_crystalInfo == null ? LanguageManager.GetLang().shortInt : (short)_crystalInfo.CrystalQuality);
            PushIntoStack(_userCrystal == null ? 0 : _userCrystal.CurrExprience);
            PushIntoStack(_crystalLvInfo == null ? 0 : _crystalLvInfo.UpExperience);
            PushIntoStack(_crystalInfo == null ? LanguageManager.GetLang().shortInt : _crystalInfo.AbilityID.ToShort());
            PushIntoStack(_attrNum);
            PushIntoStack(_crystalInfo == null ? 0 : _crystalInfo.Price);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserCrystalID", ref _userCrystalID))
            {
                httpGet.GetString("ToUserID", ref toUserID);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            string publicUserID = string.Empty;
            if (string.IsNullOrEmpty(toUserID))
            {
                publicUserID = ContextUser.UserID;
            }
            else
            {
                publicUserID = toUserID;
            }
            short crystalLv = 0;
            var package = UserCrystalPackage.Get(publicUserID);
            _userCrystal = package.CrystalPackage.Find(m => m.UserCrystalID.Equals(_userCrystalID));
            //_userCrystal = UserCrystal.FindKey(_userCrystalID);)
            if (_userCrystal != null)
            {
                _crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(_userCrystal.CrystalID);
                crystalLv = _userCrystal.CrystalLv;
            }
            else
            {
                var grayCrystal = ContextUser.GrayCrystalList.Find(m => m.UserCrystalID == _userCrystalID);
                if (grayCrystal != null)
                    _crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(grayCrystal.CrystalID);
                else
                    return false;
            }
            if (_crystalInfo == null)
            {
                return false;
            }
            CrystalLvInfo currCrystal = new ConfigCacheSet<CrystalLvInfo>().FindKey(_crystalInfo.CrystalID, crystalLv);
            short upLv = MathUtils.Addition(crystalLv, (short)1, short.MaxValue);
            if (upLv >= 10)
            {
                upLv = 10;
            }
            _crystalLvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(_crystalInfo.CrystalID, upLv);//下级经验
            if (currCrystal != null && currCrystal.AbilityNum > 1)
            {
                _attrNum = currCrystal.AbilityNum.ToString().Replace(".0000", "");
            }
            else if (currCrystal != null)
            {
                _attrNum = Math.Round((currCrystal.AbilityNum * 100), 1, MidpointRounding.AwayFromZero).ToString();
                _attrNum = _attrNum.Replace(".0", "") + "%";
            }

            return true;
        }
    }
}