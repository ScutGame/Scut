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
using System;
using System.Collections.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1608_变身卡使用接口
    /// </summary>
    public class Action1608 : BaseAction
    {
        private string userItemID;
        private string PictureID = string.Empty;
        private static int festivalID = 1019;

        public Action1608(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1608, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(PictureID.ToNotNullString());

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
            if (NoviceHelper.IsFestivalOpen(festivalID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().ZhongYuanHuodong;
                return false;
            }
            var package = UserItemPackage.Get(Uid);
            UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.UserItemID.Equals(userItemID));
            if (userItem != null)
            {
                ItemBaseInfo itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(userItem.ItemID);
                if (itemInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    //ErrorInfo = LanguageManager.GetLang().St1107_UserItemNotEnough;
                    return false;
                }
                if (userItem.ItemID == 7003 || userItem.ItemID == 5200)
                {
                    //双倍材料掉落卡
                    if (!DoCaiLiaoYueBingItem(userItem, itemInfo)) return false;
                }
                else
                {
                    //其他类型的道具
                    if (!UseProps(ContextUser.UserID, itemInfo))
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St1608_CombatPowerNotEnough;
                        return false;
                    }
                }
                PictureID = itemInfo.PictrueID;
                UserItemHelper.UseUserItem(ContextUser.UserID, userItem.ItemID, 1);
            }
            return true;
        }

        /// <summary>
        /// 双倍材料，月饼道具使用
        /// </summary>
        /// <param name="userItem"></param>
        /// <param name="itemInfo"></param>
        /// <returns></returns>
        private bool DoCaiLiaoYueBingItem(UserItemInfo userItem, ItemBaseInfo itemInfo)
        {
            UserProps props = new GameDataCacheSet<UserProps>().FindKey(ContextUser.UserID, userItem.ItemID);
            if (props != null)
            {
                if (userItem.ItemID == 5200 && props.DoRefresh() > 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1608_CombatPowerNotEnough;
                    return false;
                }

                props.SurplusNum += itemInfo.EffectNum;
                props.ChangeTime = DateTime.Now;
                //props.Update();

            }
            else
            {
                UserProps uProps = new UserProps(ContextUser.UserID, itemInfo.ItemID)
                                       {
                                           SurplusNum = itemInfo.EffectNum,
                                           ChangeTime = DateTime.Now
                                       };
                new GameDataCacheSet<UserProps>().Add(uProps);
            }
            return true;
        }


        private static bool UseProps(string userID, ItemBaseInfo itemInfo)
        {
            var cacheSet = new GameDataCacheSet<UserProps>();
            List<UserProps> propsArray = cacheSet.FindAll(userID, u => u.PropType == itemInfo.PropType);

            if (itemInfo.PropType == 9 && propsArray.Count > 0)
            {
                int refreshNum = propsArray[0].DoRefresh();
                if (refreshNum > 0)
                {
                    return false;
                }
            }
            bool isUsed = false;
            foreach (UserProps propse in propsArray)
            {
                if (propse.ItemID == 7003 || propse.ItemID == 5200)
                {
                    continue;
                }
                if (!isUsed && propse.ItemID == itemInfo.ItemID)
                {
                    isUsed = true;
                    propse.SurplusNum += itemInfo.EffectNum;
                    propse.ChangeTime = DateTime.Now;
                    //propse.Update();
                }
                else
                {
                    cacheSet.Delete(propse);
                }

            }
            //List<UserProps> propsArray1 = new GameDataCacheSet<UserProps>().FindAll(UserProps.Index_UserID, u => u.PropType == itemInfo.PropType && u.ItemID == itemInfo.ItemID, userID);
            if (!isUsed)
            {
                UserProps uProps = new UserProps(userID, itemInfo.ItemID)
                {
                    SurplusNum = itemInfo.EffectNum,
                    ChangeTime = DateTime.Now
                };
                new GameDataCacheSet<UserProps>().Add(uProps);
            }
            return true;
        }
    }
}