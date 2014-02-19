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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1401_玩家佣兵列表接口
    /// </summary>
    public class Action1401 : BaseAction
    {
        private string toUserID = string.Empty;
        private GeneralType generalType;
        private List<UserGeneral> userGeneralArray = new List<UserGeneral>();
        private int currNum = 0;
        private int unopened = 0;
        private short _isEnabled = 1;
        public Action1401(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1401, httpGet)
        {

        }

        public override bool TakeAction()
        {

            string publicUserID = ContextUser.UserID;
            if (!string.IsNullOrEmpty(toUserID))
            {
                publicUserID = toUserID;
            }
            UserMagic userMagic = new GameDataCacheSet<UserMagic>().Find(publicUserID, s => s.IsEnabled);

            var cacheSetUserEmbattle = new GameDataCacheSet<UserEmbattle>();
            var userGeneralList = new List<UserGeneral>();
            switch (generalType)
            {
                case GeneralType.YongBing:

                    userGeneralList = new GameDataCacheSet<UserGeneral>().FindAll(publicUserID, u => u.GeneralType != GeneralType.Soul && u.GeneralStatus == GeneralStatus.DuiWuZhong);

                    break;
                case GeneralType.Soul:
                    userGeneralList = new GameDataCacheSet<UserGeneral>().FindAll(publicUserID, u => u.GeneralType == generalType && u.AtmanNum > 0 && u.GeneralStatus == GeneralStatus.DuiWuZhong);
                    break;
                case GeneralType.Battle:
                    userGeneralList = new GameDataCacheSet<UserGeneral>().FindAll(publicUserID, u => u.GeneralType != GeneralType.Soul && u.GeneralStatus == GeneralStatus.DuiWuZhong);
                    break;

            }
            foreach (var userGeneral in userGeneralList)
            {
                switch (generalType)
                {
                    case GeneralType.YongBing:
                        if (
                          cacheSetUserEmbattle.Find(publicUserID, s => s.MagicID == userMagic.MagicID && s.GeneralID == userGeneral.GeneralID) !=
                          null)
                        {
                            userGeneral.IsBattle = true;
                        }
                        else
                        {
                            userGeneral.IsBattle = false;
                        }
                        userGeneralArray.Add(userGeneral);
                        break;
                    case GeneralType.Soul:
                        userGeneralArray.Add(userGeneral);
                        break;
                    case GeneralType.Battle:

                        if (
                         cacheSetUserEmbattle.Find(publicUserID, s => s.MagicID == userMagic.MagicID && s.GeneralID == userGeneral.GeneralID) !=
                         null)
                        {
                            userGeneral.IsBattle = true;
                            userGeneralArray.Add(userGeneral);
                        }

                        break;
                }
            }
            
            // 佣兵排序
            GeneralSortHelper.GeneralSort(ContextUser.UserID, userGeneralArray);

            return true;
        }

        public override void BuildPacket()
        {

            this.PushIntoStack(currNum);
            this.PushIntoStack(unopened);
            this.PushIntoStack(userGeneralArray.Count);
            foreach (var general in userGeneralArray)
            {
                int worseNum = 0;
                int demandNum = 0;
                short isRecruit = GeneralHelper.IsGeneralRecruit(general.UserID, general.GeneralID, out worseNum, out demandNum);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(general.GeneralID);
                dsItem.PushIntoStack(general.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(general.HeadID.ToNotNullString());
                dsItem.PushIntoStack(general.GeneralLv);
                dsItem.PushIntoStack((short)general.GeneralQuality);
                dsItem.PushIntoStack(general.AtmanNum);
                dsItem.PushIntoStack(worseNum);
                dsItem.PushIntoStack((short)isRecruit);
                dsItem.PushIntoStack(general.IsBattle ? 1 : 0);
                dsItem.PushIntoStack(demandNum);
                this.PushIntoStack(dsItem);
            }
            PushIntoStack(_isEnabled);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetEnum("GeneralType", ref generalType))
            {
                httpGet.GetString("ToUserID", ref toUserID);
                return true;
            }
            return false;
        }

    }
}