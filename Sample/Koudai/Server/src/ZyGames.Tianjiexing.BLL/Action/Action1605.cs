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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Framework.Game.Runtime;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1605_药水加血使用接口
    /// </summary>
    public class Action1605 : BaseAction
    {
        private string userItemID = string.Empty;

        public Action1605(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1605, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("UserItemID", ref userItemID))
            {
                return true;
            }
            return false;


        }

        public override bool TakeAction()
        {
            int subLifeNum = 0;

            var package = UserItemPackage.Get(Uid);
            UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
            if (userItem == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            //wuzf modify 2012-05-19
            ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
            var propsArray = new GameDataCacheSet<UserProps>().FindAll(ContextUser.UserID, u => u.PropType == 1);
            if (propsArray.Count > 0 && propsArray[0].SurplusNum > 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1605_BandageUse;
                return false;
            }

            //判断背包是否有血包,没有提示去商城
            var userItems = package.ItemPackage.FindAll(m => !m.IsRemove && new UserItemHelper(m).PropType == 1);
            if (userItems.Count == 0)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1605_UseTwoGold;
                return false;
            }

            //给佣兵补血
            var userGeneralArray = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong);
            int effectNum = itemInfo.EffectNum;
            foreach (var userGeneral in userGeneralArray)
            {
                int maxLifeNum = UserHelper.GetMaxLife(ContextUser.UserID, userGeneral.GeneralID);
                if (userGeneral.LifeNum < maxLifeNum)
                {
                    subLifeNum = MathUtils.Subtraction(maxLifeNum, userGeneral.LifeNum, 0);
                    userGeneral.LifeNum = MathUtils.Addition(userGeneral.LifeNum, effectNum, maxLifeNum);
                    //userGeneral.Update();
                    effectNum = MathUtils.Subtraction(effectNum, subLifeNum, 0);
                }
            }
            var cacheSet = new GameDataCacheSet<UserProps>();
            UserProps props = new GameDataCacheSet<UserProps>().FindKey(ContextUser.UserID, userItem.ItemID);
            if (props != null)
            {
                props.SurplusNum = effectNum;
                //props.Update();
            }
            else
            {
                props = new UserProps(ContextUser.UserID, itemInfo.ItemID)
                {
                    SurplusNum = effectNum
                };
                cacheSet.Add(props);
            }

            ContextUser.IsUseupItem = false;
            UserItemHelper.UseUserItem(ContextUser.UserID, itemInfo.ItemID, 1);
            foreach (UserProps userPropse in propsArray)
            {
                if (userPropse.SurplusNum == 0)
                {
                    cacheSet.Delete(userPropse);
                }
            }
            return true;
        }
    }
}