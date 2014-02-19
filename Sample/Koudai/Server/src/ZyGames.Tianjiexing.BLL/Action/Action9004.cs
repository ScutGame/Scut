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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9004_充值礼包详情接口
    /// </summary>
    public class Action9004 : BaseAction
    {
        private int _packsID;
        private short _isRevice;
        private RechargePacks _packsInfo;
        private static int festivalID = 1012;

        public Action9004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9004, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_packsInfo == null ? string.Empty : _packsInfo.PacksName);
            PushIntoStack(_isRevice);
            PushIntoStack(_packsInfo == null ? string.Empty : _packsInfo.PacksDesc);

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PacksID", ref _packsID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //判断充值礼包是否开启
            FestivalInfo festivalInfo = new ShareCacheStruct<FestivalInfo>().FindKey(festivalID);
            if (festivalInfo != null && !festivalInfo.IsStop) return false;

            _packsInfo = new ConfigCacheSet<RechargePacks>().FindKey(_packsID);
            //  List<PackageReceive> receivesArray = new GameDataCacheSet<PackageReceive>().FindAll(PackageReceive.Index_UserID_PacksID,ContextUser.UserID, _packsID);
            if (isRecevies(ContextUser.UserID, _packsID))
            {
                _isRevice = 1;
            }
            else
            {
                _isRevice = 2;
            }
            return true;
        }

        public static bool isRecevies(string userID, int packID)
        {
            bool result = false;
            List<PackageReceive> receivesArray = new GameDataCacheSet<PackageReceive>().FindAll(userID, m => m.PacksID == packID);
            foreach (PackageReceive package in receivesArray)
            {
                if (!package.IsReceive)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}