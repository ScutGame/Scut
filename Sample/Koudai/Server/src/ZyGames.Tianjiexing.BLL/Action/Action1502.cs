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
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 1502_魔术详情接口
    /// </summary>
    public class Action1502 : BaseAction
    {
        private int magicID = 0;
        private int magicLv = 0;
        private MagicInfo magicInfo = null;
        private List<UserMagic> userMagicArray = new List<UserMagic>();
        private List<MagicLvInfo> magicLvInfoArray = new List<MagicLvInfo>();
        private MagicLvInfo magicLvInfo = null;

        public Action1502(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1502, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(magicInfo == null ? string.Empty : magicInfo.MagicName.ToNotNullString());
            PushIntoStack((short)magicLv);
            PushIntoStack(magicInfo == null ? string.Empty : magicInfo.MagicDesc.ToNotNullString());
            PushIntoStack(ContextUser.UserLv);
            PushIntoStack(magicLvInfo == null ? 0 : magicLvInfo.ExpNum);
            PushIntoStack(magicLvInfo == null ? 0 : magicLvInfo.ColdTime);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MagicID", ref magicID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            magicInfo = new ConfigCacheSet<MagicInfo>().FindKey(magicID);
            userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(ContextUser.UserID, u => u.MagicID == magicID);
            if (userMagicArray.Count > 0)
            {
                magicLv = userMagicArray[0].MagicLv;
            }
            int upgradeLv = MathUtils.Addition(magicLv, 1, int.MaxValue);
            if (upgradeLv <= 10)
            {
                magicLvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(magicID, upgradeLv);
            }

            return true;
        }
    }
}