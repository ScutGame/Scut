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
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1302_佣兵命运水晶列表接口
    /// </summary>
    public class Action1302 : BaseAction
    {
        private int _generalID;
        private List<UserCrystalInfo> userCrystalArray = new List<UserCrystalInfo>();
        private UserGeneral userGeneral = null;
        private string toUserID;
        private string _maxHeadID = string.Empty;
        public Action1302(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1302, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(userGeneral == null ? string.Empty : userGeneral.HeadID.ToNotNullString());
            this.PushIntoStack(UserHelper.GetOpenNum(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.GeneralLv));
            this.PushIntoStack(userCrystalArray.Count);
            foreach (UserCrystalInfo crystal in userCrystalArray)
            {
                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystal.CrystalID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(crystal.UserCrystalID);
                dsItem.PushIntoStack(crystalInfo == null ? 0 : crystalInfo.CrystalID);
                dsItem.PushIntoStack(crystalInfo == null ? string.Empty : crystalInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(crystal.Position);
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(_maxHeadID);
            PushIntoStack(userGeneral == null ? 0.ToShort() : userGeneral.GeneralQuality.ToShort());
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref _generalID))
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
            var package = UserCrystalPackage.Get(publicUserID);
            userCrystalArray = package.CrystalPackage.FindAll(m => m.GeneralID.Equals(_generalID));
            userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, _generalID);
            var generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(userGeneral != null ? userGeneral.GeneralID : 0);
            _maxHeadID = generalInfo != null ? generalInfo.PicturesID : string.Empty;
            return true;
        }
    }
}