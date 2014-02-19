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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1210_装备封灵界面接口
    /// </summary>
    public class Action1210 : BaseAction
    {
        private string toUserID;
        private int _lingshiNum;
        private List<UserGeneral> _userGeneralArray = new List<UserGeneral>();
        private Dictionary<int, List<UserItemInfo>> userItemDict = new Dictionary<int, List<UserItemInfo>>();
        private UserSparePart[] _sparePartList = new UserSparePart[UserSparePart.PartMaxGrid];
        private GameUser user = null;

        public Action1210(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1210, httpGet)
        {

        }

        public override void BuildPacket()
        {
            this.PushIntoStack(_lingshiNum);
            this.PushIntoStack(_userGeneralArray.Count);
            foreach (var general in _userGeneralArray)
            {
                List<UserItemInfo> userItemArray = userItemDict.ContainsKey(general.GeneralID)
                    ? userItemDict[general.GeneralID] : new List<UserItemInfo>();

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(general.GeneralID);
                dsItem.PushIntoStack(general.GeneralName.ToNotNullString());
                dsItem.PushIntoStack(userItemArray.Count);
                foreach (var item in userItemArray)
                {
                    var itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(item.ItemID) ?? new ItemBaseInfo();
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(item.UserItemID.ToNotNullString());
                    dsItem1.PushIntoStack(item.ItemID);
                    dsItem1.PushIntoStack(itemInfo.ItemName.ToNotNullString());
                    dsItem1.PushIntoStack(itemInfo.HeadID.ToNotNullString());
                    dsItem1.PushIntoStack(itemInfo.QualityType.ToShort());
                    dsItem1.PushIntoStack(itemInfo.EquParts.ToShort());

                    dsItem.PushIntoStack(dsItem1);
                }

                PushIntoStack(dsItem);
            }
            this.PushIntoStack(_sparePartList.Length);
            for (int i = 0; i < _sparePartList.Length; i++)
            {
                var sparePart = _sparePartList[i] ?? new UserSparePart();
                if (sparePart.Position == 0) sparePart.SetPosition((short)(i + 1));
                short enableStatus = 0;
                if (sparePart.CheckEnable(user.UserExtend.MaxLayerNum)) enableStatus = 1;
                var sparePartInfo = new ConfigCacheSet<SparePartInfo>().FindKey(sparePart.SparePartId) ?? new SparePartInfo();

                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(sparePart.Position);
                dsItem.PushIntoStack(enableStatus);
                dsItem.PushIntoStack(sparePart.UserSparepartID.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.Name.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.HeadID.ToNotNullString());
                dsItem.PushIntoStack(sparePartInfo.QualityType.ToShort());
                dsItem.PushIntoStack(sparePartInfo.CoinPrice);
                dsItem.PushIntoStack(sparePartInfo.LingshiPrice);

                dsItem.PushIntoStack(sparePart.Propertys.Count);
                for (int r = 0; r < sparePart.Propertys.Count; r++)
                {
                    var property = sparePart.Propertys[r];
                    short proPos = MathUtils.Addition(property.ValueIndex, (short)1, short.MaxValue);
                    short isEnable = 0;
                    if (property.IsEnable) isEnable = 1;
                    DataStruct dsItem1 = new DataStruct();
                    dsItem1.PushIntoStack(property.AbilityType.ToShort());
                    dsItem1.PushIntoStack(property.Num.ToNotNullString());
                    dsItem1.PushIntoStack(property.HitMinValue.ToNotNullString());
                    dsItem1.PushIntoStack(property.HitMaxValue.ToNotNullString());
                    dsItem1.PushIntoStack(isEnable);
                    dsItem1.PushIntoStack(proPos);

                    dsItem.PushIntoStack(dsItem1);
                }

                this.PushIntoStack(dsItem);
            }

        }

        public override bool GetUrlElement()
        {
            httpGet.GetString("ToUserID", ref toUserID);

            return true;
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
                UserCacheGlobal.LoadOffline(publicUserID);
            }
            user = new GameDataCacheSet<GameUser>().FindKey(publicUserID);
            if (user == null)
            {
                return false;
            }
            if (user.UserExtend == null)
            {
                user.UserExtend = new GameUserExtend();
            }
            if (new GameDataCacheSet<UserFunction>().FindKey(publicUserID, FunctionEnum.Fengling) == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_NoFun;
                return false;
            }
            CheckUserPlotLayerNum(user);
            _lingshiNum = user.UserExtend.LingshiNum;
            _userGeneralArray = new GameDataCacheSet<UserGeneral>().FindAll(publicUserID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong && u.GeneralType != GeneralType.Soul);
            int generalId = 0;
            foreach (var userGeneral in _userGeneralArray)
            {
                if (userGeneral != null && !userItemDict.ContainsKey(userGeneral.GeneralID))
                {
                    var package = UserItemPackage.Get(publicUserID);
                    var userItemArray = package.ItemPackage.FindAll(u => !u.IsRemove && u.GeneralID.Equals(userGeneral.GeneralID) && u.ItemStatus == ItemStatus.YongBing);
                    userItemDict.Add(userGeneral.GeneralID, userItemArray);

                    if (generalId == 0) generalId = userGeneral.GeneralID;
                }
            }
            if (generalId > 0)
            {
                //首个是武器的灵件列表
                var package = UserItemPackage.Get(publicUserID);
                var userItems = package.ItemPackage.FindAll(
                    u => !u.IsRemove && u.GeneralID.Equals(generalId) &&
                        new UserItemHelper(u).EquPartsID.Equals((int)EquParts.WuQi) &&
                        u.ItemStatus == ItemStatus.YongBing
                );
                if (userItems.Count > 0)
                {
                    var tempPartList = user.SparePartList.FindAll(m => m.UserItemID.Equals(userItems[0].UserItemID));
                    for (int i = 0; i < tempPartList.Count; i++)
                    {
                        var part = tempPartList[i];
                        if (part != null && part.Position > 0 && part.Position - 1 < _sparePartList.Length)
                        {
                            part.UpdateNotify(obj =>
                            {
                                _sparePartList[part.Position - 1] = part;
                                return true;
                            });
                        }
                        else if (part != null && part.Position == 0 && !string.IsNullOrEmpty(part.UserItemID))
                        {
                            //修正灵石在装备上位置为同一的
                            part.UpdateNotify(obj =>
                            {
                                part.UserItemID = string.Empty;
                                return true;
                            });
                        }
                    }

                    // ContextUser.UpdateSparePart();
                }
            }
            return true;
        }

        /// <summary>
        /// 天地劫副本层数
        /// </summary>
        /// <param name="user"></param>
        public static void CheckUserPlotLayerNum(GameUser user)
        {
            PlotInfo plotInfo = UserHelper.CheckUserPlotKalpa(user);
            if (user != null && plotInfo != null)
            {
                //最高层
                if (user.UserExtend != null && user.UserExtend.MaxLayerNum == 0)
                {
                    user.UserExtend.MaxLayerNum = plotInfo.LayerNum;
                    //user.Update();
                }
            }
        }
    }
}