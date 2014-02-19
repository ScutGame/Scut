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
    /// 1216_灵件属性开启接口
    /// </summary>
    public class Action1216 : BaseAction
    {
        private int _ops;
        private int _position;
        private string _sparepartID;
        private UserSparePart _sparePart = new UserSparePart();


        public Action1216(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1216, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_sparePart.Propertys.Count);
            for (int i = 0; i < _sparePart.Propertys.Count; i++)
            {
                var property = _sparePart.Propertys[i];
                short proPos = MathUtils.Addition(property.ValueIndex, (short)1, short.MaxValue);
                short isEnable = 0;
                if (property.IsEnable) isEnable = 1;

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(property.AbilityType.ToShort());
                dsItem.PushIntoStack(property.Num.ToNotNullString());
                dsItem.PushIntoStack(property.HitMinValue.ToNotNullString());
                dsItem.PushIntoStack(property.HitMaxValue.ToNotNullString());
                dsItem.PushIntoStack(isEnable);
                dsItem.PushIntoStack(proPos);

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("Ops", ref _ops)
                 && httpGet.GetInt("Position", ref _position)
                && httpGet.GetString("SparepartID", ref _sparepartID))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            ErrorCode = _ops;
            int goldNum = ConfigEnvSet.GetInt("SparePart.PropertyGoldNum");
            if (_ops == 1)
            {
                ErrorInfo = string.Format(LanguageManager.GetLang().St1216_EnableSpartProperty, goldNum, _position);
            }
            else if (_ops == 2)
            {
                if (ContextUser.GoldNum < goldNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                    return false;
                }
                if (ContextUser.EnableSpareProperty(_sparepartID, out _sparePart))
                {
                    ContextUser.UseGold = MathUtils.Addition(ContextUser.UseGold, goldNum);
                    //ContextUser.Update();
                    var itempackage = UserItemPackage.Get(ContextUser.UserID);
                    UserItemInfo userItem = itempackage.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(_sparePart.UserItemID));
                    if (userItem != null && userItem.ItemStatus.Equals(ItemStatus.YongBing))
                    {
                        var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, userItem.GeneralID);
                        if (userGeneral != null) userGeneral.RefreshMaxLife();
                    }
                }
                else
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
            }
            return true;
        }
    }
}