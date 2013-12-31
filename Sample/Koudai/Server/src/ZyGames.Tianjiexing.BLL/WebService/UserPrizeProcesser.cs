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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using BaseLog = ZyGames.Tianjiexing.BLL.Base.BaseLog;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Component.Chat;

namespace ZyGames.Tianjiexing.BLL.WebService
{
    public class UserPrizeProcesser : BaseDataProcesser
    {
        public override void Process(JsonParameter[] paramList)
        {
            JsonParameterList parameters = JsonParameter.Convert(paramList);
            string op = parameters["op"];
            string[] UserList = Convert.ToString(parameters["UserID"]).Split(new char[] { ',' });
            string itemPackage = Convert.ToString(parameters["Item"]);
            string crystalList = Convert.ToString(parameters["Crystal"]);
            string sparePackage = Convert.ToString(parameters["SparePackage"]);
            string enchantPackage = Convert.ToString(parameters["EnchantPackage"]);

            int obtainNum = 0;
            if (parameters["ObtainNum"].Length > 0)
            {
                obtainNum = Convert.ToInt32(parameters["ObtainNum"]);
            }
            short energyNum = 0;
            if (parameters["EnergyNum"].Length > 0)
            {
                energyNum = Convert.ToInt16(parameters["EnergyNum"]);
            }
            int gameCoin = 0;
            if (parameters["GameCoin"].Length > 0)
            {
                gameCoin = Convert.ToInt32(parameters["GameCoin"]);
            }
            int gold = 0;
            if (parameters["Gold"].Length > 0)
            {
                gold = Convert.ToInt32(parameters["Gold"]);
            }
            int expNum = 0;
            if (parameters["ExpNum"].Length > 0)
            {
                expNum = Convert.ToInt32(parameters["ExpNum"]);
            }
            int vipLv = 0;
            if (parameters["VipLv"].Length > 0)
            {
                vipLv = Convert.ToInt32(parameters["VipLv"]);
            }
            int gainBlessing = 0;
            if (parameters["GainBlessing"].Length > 0)
            {
                gainBlessing = Convert.ToInt32(parameters["GainBlessing"]);
            }
            int honourNum = 0;
            if (parameters["HonourNum"].Length > 0)
            {
                honourNum = Convert.ToInt32(parameters["HonourNum"]);
            }
            string mailContent = Convert.ToString(parameters["MailContent"]);
            int opUserID = Convert.ToInt32(parameters["OpUserID"]);
            //var cacheSetUserItem = new GameDataCacheSet<UserItemPackage>();
            var cacheSetItemInfo = new ShareCacheStruct<ItemBaseInfo>();
            var itemArray = itemPackage.Split(',');
            foreach (string str in UserList)
            {
                try
                {
                    int userID = str.Trim().ToInt();


                    var user = new GameDataCacheSet<GameUser>().FindKey(str);
                    if(user!=null)
                    {
                        user.GiftGold = MathUtils.Addition(user.GiftGold, gold);
                        user.ObtainNum = MathUtils.Addition(user.ObtainNum, obtainNum);
                        user.EnergyNum = MathUtils.Addition(user.EnergyNum, energyNum);
                        user.GameCoin = MathUtils.Addition(user.GameCoin, gameCoin);
                        user.ExpNum = MathUtils.Addition(user.ExpNum, expNum);
                        user.HonourNum = MathUtils.Addition(user.HonourNum, honourNum);
                    }
                   
                     
                    foreach (var item in itemArray)
                    {
                        if(item.Split('=').Length==2)
                        {
                            var itemInfo = cacheSetItemInfo.FindKey(item.Split('=')[0]);
                            if(itemInfo!=null)
                            {
                                UserItemHelper.AddUserItem(str, item.Split('=')[0].ToInt(), item.Split('=')[1].ToInt());
                            }
                        }
                    }
                    Guid newGuid = Guid.NewGuid();
                    UserTakePrize userPrizeLog = new UserTakePrize
                    {
                        ID = newGuid.ToString(),
                        UserID = userID,
                        ObtainNum = obtainNum,
                        EnergyNum = energyNum,
                        GameCoin = gameCoin,
                        Gold = gold,
                        ExpNum = expNum,
                        VipLv = vipLv,
                        GainBlessing = gainBlessing,
                        ItemPackage = itemPackage,
                        CrystalPackage = crystalList,
                        SparePackage = sparePackage,
                        EnchantPackage = enchantPackage,
                        MailContent = mailContent,
                        IsTasked = false,
                        TaskDate = MathUtils.SqlMinDate,
                        OpUserID = opUserID,
                        CreateDate = DateTime.Now,
                        HonourNum = honourNum,
                        Items = itemPackage,
                        
                    };
                    var cacheSet = new ShareCacheStruct<UserTakePrize>();
                    cacheSet.Add(userPrizeLog);
                    cacheSet.Update();
                    PutCrystal(crystalList.Split(','), str);

                    // 发送系统信件
                    UserMail userMail = new UserMail(newGuid);
                    userMail.UserId = userID;
                    userMail.MailType = (int)MailType.System;
                    userMail.Title = LanguageManager.GetLang().St_SystemMailTitle;
                    userMail.Content = mailContent;
                    userMail.SendDate = DateTime.Now;
                    TjxMailService mailService=new TjxMailService(user);
                    mailService.Send(userMail);
                }
                catch (Exception ex)
                {
                    new BaseLog().SaveLog(ex);
                }
            }
        }

        /// <summary>
        /// 命运水晶ID=等级=数量
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userID"></param>
        private static void PutCrystal(string[] list, string userID)
        {
            var package = UserCrystalPackage.Get(userID);
            foreach (string crystal in list)
            {
                if (string.IsNullOrEmpty(crystal)) continue;
                string[] crystalList = crystal.Split(new char[] { '=' });

                int crystalID = crystalList.Length > 0 ? Convert.ToInt32(crystalList[0]) : 0;
                short crystalLv = crystalList.Length > 1 ? Convert.ToInt16(crystalList[1]) : (short)0;
                int crystalNum = crystalList.Length > 2 ? Convert.ToInt32(crystalList[2]) : 0;

                CrystalInfo crystalInfo = new ConfigCacheSet<CrystalInfo>().FindKey(crystalID);
                var crystalLvInfo = new ConfigCacheSet<CrystalLvInfo>().FindKey(crystalID, crystalLv);
                if (crystalNum > 0 && new ConfigCacheSet<CrystalInfo>().FindKey(crystalID) != null && crystalLvInfo != null)
                {
                    for (int i = 0; i < crystalNum; i++)
                    {
                        UserCrystalInfo userCrystal = new UserCrystalInfo();
                        userCrystal.UserCrystalID = Guid.NewGuid().ToString();
                        userCrystal.CrystalID = crystalInfo.CrystalID;
                        userCrystal.CrystalLv = crystalLv;
                        userCrystal.GeneralID = 0;
                        userCrystal.IsSale = 2;
                        userCrystal.Position = 0;
                        userCrystal.CurrExprience = crystalLvInfo.UpExperience;
                        package.SaveCrystal(userCrystal);
                    }
                }
                else
                {
                    new Base.BaseLog().SaveLog("领取命运水晶异常", new Exception(string.Format("userID:{3},crystalID:{0},crystalNum:{1},crystalLv:{2}", crystalID, crystalNum, crystalLv, userID)));
                }
            }
        }

    }
}