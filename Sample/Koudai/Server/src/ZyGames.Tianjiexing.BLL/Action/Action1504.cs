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
    /// 1504_魔术配置信息下发接口
    /// </summary>
    public class Action1504 : BaseAction
    {
        private int ClientVersion;
        private List<MagicInfo> magicList = new List<MagicInfo>();

        public Action1504(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1504, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(magicList.Count);
            foreach (MagicInfo magicInfo in magicList)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(magicInfo.MagicID);
                dsItem.PushIntoStack(magicInfo.MagicType.ToInt());
                dsItem.PushIntoStack(magicInfo.MagicName.ToNotNullString());
                dsItem.PushIntoStack(magicInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(magicInfo.MagicDesc.ToNotNullString());

                var magicLvList = new ConfigCacheSet<MagicLvInfo>().FindAll(m => m.MagicID == magicInfo.MagicID);
                dsItem.PushIntoStack(magicLvList.Count);
                foreach (MagicLvInfo magicLv in magicLvList)
                {
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(magicLv.MagicLv.ToShort());
                    dsItem1.PushIntoStack(magicLv.ExpNum);
                    dsItem1.PushIntoStack(magicLv.ColdTime);
                    dsItem1.PushIntoStack(magicLv.EscalateMinLv.ToShort());
                    dsItem1.PushIntoStack(magicLv.AbilityType.ToShort());
                    dsItem1.PushIntoStack(magicLv.EffectNum.ToNotNullString());
                    dsItem1.PushIntoStack(magicLv.GridMaxNum);

                    dsItem.PushIntoStack(dsItem1);
                }

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("ClientVersion", ref ClientVersion))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            //int currVersion = new ConfigCacheSet<ConfigVersion>().FindKey(VersionType.Magic).CurVersion;
            //magicList = new ConfigCacheSet<MagicInfo>().FindAll(m => m.Version > ClientVersion && m.Version <= currVersion);
            magicList = new ConfigCacheSet<MagicInfo>().FindAll();
            return true;
        }
    }
}