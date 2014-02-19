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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 1408_培养属性详情接口
    /// </summary>
    public class Action1408 : BaseAction
    {
        private int generalID = 0;
        private UserGeneral userGeneral = null;
        private List<TrainingInfo> cultureList = new List<TrainingInfo>();

        public Action1408(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1408, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(userGeneral == null ? string.Empty : userGeneral.GeneralName);
            PushIntoStack(userGeneral == null ? string.Empty : userGeneral.HeadID);
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.GeneralLv);
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.PowerNum);
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.SoulNum);
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.IntellectNum);
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.TrainingPower);
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.TrainingSoul);
            PushIntoStack(userGeneral == null ? LanguageManager.GetLang().shortInt : userGeneral.TrainingIntellect);

            this.PushIntoStack(cultureList.Count);
            foreach (TrainingInfo culture in cultureList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack((int)culture.CultureID);
                dsItem.PushIntoStack(culture.CultureNum.ToNotNullString());

                this.PushIntoStack(dsItem);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("GeneralID", ref generalID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalID);
            cultureList = UserHelper.GetCultureType(ContextUser.UserID, generalID);
            return true;
        }
    }
}