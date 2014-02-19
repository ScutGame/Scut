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
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 领土战英雄列表接口
    /// </summary>
    public class Action5203 : BaseAction
    {
        private int _pageIndex;
        private int _pageSize;
        private CountryType _countryID;
        private short _lv;
        private List<CountryUser> _countryUserList = new List<CountryUser>();
        private int _pageCount;


        public Action5203(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action5203, httpGet)
        {

        }

        public override bool TakeAction()
        {
            CountryCombat countryCombat = new CountryCombat(ContextUser);
            _countryUserList = countryCombat.GetCountryUser(_countryID, _lv);
            _countryUserList = _countryUserList.GetPaging(_pageIndex, _pageSize, out _pageCount);
            return true;
        }


        public override void BuildPacket()
        {
            PushIntoStack(_countryUserList.Count);
            foreach (CountryUser countryUser in _countryUserList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(countryUser.UserName.ToNotNullString());
                ds.PushIntoStack(countryUser.UserLv);
                ds.PushIntoStack(countryUser.Status);
                ds.PushIntoStack(countryUser.WinCount);
                ds.PushIntoStack(countryUser.FailCount);
                ds.PushIntoStack(countryUser.CurrWinNum);
                ds.PushIntoStack(countryUser.MaxWinNum);

                PushIntoStack(ds);

            }
            PushIntoStack(_pageCount);
        }

        public override bool GetUrlElement()
        {

            if (httpGet.GetEnum("CountryID", ref _countryID)
                 && httpGet.GetWord("Lv", ref _lv)
                 && httpGet.GetInt("PageIndex", ref _pageIndex)
                 && httpGet.GetInt("PageSize", ref _pageSize))
            {
                return true;
            }
            return false;
        }
    }
}