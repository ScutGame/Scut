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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 背包格子类
    /// </summary>
    public class UserPackHelper
    {
        public static GameDataCacheSet<UserPack> _cacheSetUserPack = new GameDataCacheSet<UserPack>();
        public static GameDataCacheSet<UserAbility> _cacheSetUserAbility = new GameDataCacheSet<UserAbility>();
        public static GameDataCacheSet<UserGeneral> _cacheSetUserGeneral = new GameDataCacheSet<UserGeneral>();
        public static GameDataCacheSet<UserItemPackage> _cacheSetUserItem = new GameDataCacheSet<UserItemPackage>();
        public static ConfigCacheSet<BackpackConfigInfo> _cacheSetBackpack = new ConfigCacheSet<BackpackConfigInfo>();
        /// <summary>
        /// 获取某种类型背包格子数
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="userId"></param>
        public static int GetPackTypePositionNum(BackpackType backpackType, string userId)
        {
            var userPack = _cacheSetUserPack.FindKey(userId);
            var packType = userPack != null && userPack.PackTypeList != null
                              ? userPack.PackTypeList.Find(s => s.BackpackType == backpackType)
                              : null;
            int position = 0;
            if (packType != null)
            {
                position = packType.Position;
            }
            return position;
        }
        /// <summary>
        /// 默认玩家注册获得背包格子数
        /// </summary>
        /// <param name="userId"></param>
        public static void AddUserPack(string userId)
        {
            var userPack = _cacheSetUserPack.FindKey(userId);

            var backpackList = _cacheSetBackpack.FindAll();
            backpackList.ForEach(backpack =>
                                     {
                                         if (userPack == null)
                                         {
                                             userPack = new UserPack(userId.ToInt());
                                             userPack.CreateDate = DateTime.Now;
                                             var packType = new PackType();
                                             packType.BackpackType = backpack.BackpackType;
                                             packType.OpenNum = 0;
                                             packType.Position = backpack.DefaultNum;
                                             userPack.PackTypeList.Add(packType);
                                             _cacheSetUserPack.Add(userPack, userId.ToInt());
                                         }
                                         else
                                         {
                                             var packType = userPack.PackTypeList != null
                                                                ? userPack.PackTypeList.Find(
                                                                    s => s.BackpackType == backpack.BackpackType)
                                                                : null;
                                             if (packType == null)
                                             {
                                                 packType = new PackType();
                                                 packType.BackpackType = backpack.BackpackType;
                                                 packType.OpenNum = 0;
                                                 packType.Position = backpack.DefaultNum;
                                                 userPack.PackTypeList.Add(packType);
                                             }
                                             else
                                             {
                                                 packType.BackpackType = backpack.BackpackType;
                                                 packType.OpenNum = 0;
                                                 packType.Position = backpack.DefaultNum;
                                             }
                                         }
                                     });
        }

        /// <summary>
        /// 判断背包是否已满
        /// </summary>
        /// <param name="backpackType"></param>
        /// <returns></returns>
        public static bool PackIsFull(GameUser user, BackpackType backpackType, int buyNum, out string prizeContent)
        {
            prizeContent = "";
            bool isFull = false;
            var userPack = _cacheSetUserPack.FindKey(user.UserID);
            var pack = userPack != null && userPack.PackTypeList != null
                           ? userPack.PackTypeList.Find(s => s.BackpackType == backpackType)
                           : null;
            int position = pack != null ? pack.Position : 0;
            int num = 0;
            switch (backpackType)
            {
                case BackpackType.BeiBao:
                    var userItem = _cacheSetUserItem.FindKey(user.UserID);
                    var bdList = userItem != null && userItem.ItemPackage != null
                                  ? userItem.ItemPackage.FindAll(s => s.ItemType != ItemType.BengDai)
                                  : null;
                    if (bdList != null && bdList.Count > 0)
                    {
                        foreach (var userItemInfo in bdList)
                        {
                            num = MathUtils.Addition(num, userItemInfo.Num);
                        }
                    }
                    if ((num + buyNum) > position * 99)
                    {
                        prizeContent = LanguageManager.GetLang().PackFull;
                        isFull = true;
                    }
                    break;
                case BackpackType.ZhuangBei:
                    var userItem2 = _cacheSetUserItem.FindKey(user.UserID);
                    num = userItem2 != null && userItem2.ItemPackage != null
                                  ? userItem2.ItemPackage.FindAll(s => s.ItemType == ItemType.ZhuangBei).Count
                                  : 0;
                    //if (zbList != null && zbList.Count > 0)
                    //{
                    //    foreach (var userItemInfo in zbList)
                    //    {
                    //        num = MathUtils.Addition(num, userItemInfo.Num);
                    //    }
                    //}
                    if ((num + buyNum) >= position )
                    {
                        prizeContent = LanguageManager.GetLang().EquipFull;
                        isFull = true;
                    }

                    break;
                case BackpackType.YongBing:
                    var userGeneralList = _cacheSetUserGeneral.FindAll(user.UserID);
                    num = userGeneralList != null ? userGeneralList.Count : 0;
                    if ((num + buyNum) >= position)
                    {
                        prizeContent = LanguageManager.GetLang().GeneralFull;
                        isFull = true;
                    }
                    break;
                case BackpackType.HunJi:
                    var userAbility = _cacheSetUserAbility.FindKey(user.UserID);
                    num = userAbility != null && userAbility.AbilityList != null ? userAbility.AbilityList.Count : 0;
                    if ((num + buyNum) >= position)
                    {
                        prizeContent = LanguageManager.GetLang().AbilityFull;
                        isFull = true;
                    }
                    break;
            }

            return isFull;
        }


    }
}