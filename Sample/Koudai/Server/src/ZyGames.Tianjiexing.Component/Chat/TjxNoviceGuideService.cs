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
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Generic;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.ConfigModel;
using ZyGames.Tianjiexing.Model.DataModel;
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Component.Chat
{
    public class TjxNoviceGuideService : NoviceGuide<NoviceUser, NoviceTaskInfo>
    {
        public TjxNoviceGuideService(int userId)
            : base(userId)
        {
        }

        protected override NoviceUser CreateUserGuide()
        {
            return new NoviceUser(this.UserId);
        }
        protected override object ProcessPrize()
        {
            var userGeneral = UserGeneral.GetMainGeneral(UserId.ToString());
            if (userGeneral == null)
            {
                return null;
            }
            var noviceUser = new GameDataCacheSet<NoviceUser>().FindKey(UserId.ToString());
            if (noviceUser == null)
            {
                return null;
            }
            var noviceTaskInfo = new ShareCacheStruct<NoviceTaskInfo>().FindKey(noviceUser.CurrGuideId);
            if (noviceTaskInfo == null || noviceTaskInfo.PrizeList.Count <= 0)
            {
                return null;
            }
            var itemList = new List<ItemBaseInfo>();
            foreach (var prizeInfo in noviceTaskInfo.PrizeList)
            {
                switch (prizeInfo.Key)
                {
                    case 0:
                        foreach (var itemId in prizeInfo.Value)
                        {
                            var itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemId);
                            if (itemInfo == null)
                            {
                                continue;
                            }
                            UserItemHelper.AddUserItem(UserId.ToString(), itemId, 1);
                            itemList.Add(itemInfo);

                            var gameUser = new GameDataCacheSet<GameUser>().FindKey(UserId.ToString());
                            if (gameUser!=null)
                            {
                                 if(gameUser.UserExtend.NoviceIsPase == true)
                                 {
                                     var noviceTask = new ConfigCacheSet<NoviceTaskInfo>().FindKey("1013");
                                     if (noviceTask != null && noviceTask.PrizeList!=null)
                                     {
                                         foreach (var gift in noviceTask.PrizeList)
                                         {
                                             foreach (var Id in gift.Value)
                                             {
                                                 var item = new ConfigCacheSet<ItemBaseInfo>().FindKey(Id);
                                                 UserItemHelper.AddUserItem(UserId.ToString(), Id, 1);
                                                 itemList.Add(item);
                                             }
                                           
                                         }
                                         
                                     }
                                     
                                 }
                            }
                        }
                        break;
                    case 1:
                        if (prizeInfo.Value.Count > userGeneral.CareerID)
                        {
                            var itemId = prizeInfo.Value[userGeneral.CareerID];
                            var itemInfo = new ConfigCacheSet<ItemBaseInfo>().FindKey(itemId);
                            if (itemInfo == null)
                            {
                                continue;
                            }
                            UserItemHelper.AddUserItem(UserId.ToString(), itemId, 1);
                            itemList.Add(itemInfo);
                        }
                        break;
                }
            }
            return itemList;
        }

    }
}