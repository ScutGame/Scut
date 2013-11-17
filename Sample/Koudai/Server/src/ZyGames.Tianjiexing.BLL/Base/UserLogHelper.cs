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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.LogModel;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class UserLogHelper
    {
        /// <summary>
        /// 新增装备合成log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="itemID"></param>
        /// <param name="num"></param>
        /// <param name="synthesisInfo1"></param>
        /// <param name="synthesisInfo2"></param>
        /// <param name="useGold"></param>
        /// <param name="beforeLv"></param>
        /// <param name="afterLv"></param>
        public static void AppenItemSynthesisLog(string userID, short opType, int itemID, int num, CacheList<SynthesisInfo> synthesisInfo1, SynthesisInfo synthesisInfo2, int useGold, short beforeLv, short afterLv)
        {
            UserItemSynthesisLog log = new UserItemSynthesisLog
              {
                  ID = Guid.NewGuid().ToString(),
                  UserID = userID,
                  OpType = opType,
                  ItemID = itemID,
                  OpNum = num,
                  DemandMaterial = synthesisInfo1,
                  SurplusMaterial = synthesisInfo2,
                  UseGold = useGold,
                  BeforeLv = beforeLv,
                  AfterLv = afterLv,
                  CreateDate = DateTime.Now
              };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        /// <summary>
        /// 新增玩家使用晶石购买log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="sumNum"></param>
        /// <param name="opNum"></param>
        /// <param name="useGold"></param>
        /// <param name="surplusGold"></param>
        /// <param name="totalGold"></param>
        public static void AppenUseGoldLog(string userID, short opType, int sumNum, int opNum, int useGold, int surplusGold, int totalGold)
        {
            UserUseGoldLog log = new UserUseGoldLog
                                     {
                                         ID = Guid.NewGuid().ToString(),
                                         UserID = userID,
                                         OpType = opType,
                                         SumNum = sumNum,
                                         OpNum = opNum,
                                         UseGold = useGold,
                                         SurplusGold = surplusGold,
                                         TotalGold = totalGold,
                                         CreateDate = DateTime.Now
                                     };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        /// <summary>
        /// 玩家强化（装备，魔术）log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="sumNum"></param>
        /// <param name="opNum"></param>
        /// <param name="useGold"></param>
        /// <param name="surplusGold"></param>
        /// <param name="totalGold"></param>
        public static void AppenStrongLog(string userID, short opType, string userItemID, int itemID, short useType, short strongLv, int useGold, int generalID)
        {
            UserStrongLog log = new UserStrongLog
            {
                ID = Guid.NewGuid().ToString(),
                UserID = userID,
                OpType = opType,
                UserItemID = userItemID,
                ItemID = itemID,
                UseType = useType,
                StrongLv = strongLv,
                UseGold = useGold,
                GeneralID = generalID,
                CreateDate = DateTime.Now
            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        /// <summary>
        /// 3.3.18 玩家庄园log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="sumNum"></param>
        /// <param name="opNum"></param>
        /// <param name="useGold"></param>
        /// <param name="surplusGold"></param>
        /// <param name="totalGold"></param>
        public static void AppenLandLog(string userID, short opType, int generalID, int postion, int useGold, short plantQuality, int gainNum, int buyNum)
        {
            UserLandLog log = new UserLandLog
            {
                ID = Guid.NewGuid().ToString(),
                UserID = userID,
                OpType = opType,
                GeneralID = generalID,
                Postion = postion,
                PlantQuality = plantQuality,
                GainNum = gainNum,
                BuyNum = buyNum,
                CreateDate = DateTime.Now
            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        /// <summary>
        /// 玩家命运水晶log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="sumNum"></param>
        /// <param name="opNum"></param>
        /// <param name="useGold"></param>
        /// <param name="surplusGold"></param>
        /// <param name="totalGold"></param>
        public static void AppenCtystalLog(string userID, short opType, int crystalID, int coinNum, int useGold, CacheList<SynthesisInfo> synthesisInfo, short crystalLv, int exp)
        {
            UserCrystalLog log = new UserCrystalLog()
            {
                ID = Guid.NewGuid().ToString(),
                UserID = userID,
                OpType = opType,
                CrystalID = crystalID,
                CoinNum = coinNum,
                UseGold = useGold,
                SynthesisCrystal = synthesisInfo,
                CrystalLv = crystalLv,
                Experience = exp,
                CreateDate = DateTime.Now
            };

            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        /// <summary>
        /// 玩家修炼操作日志表log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="sumNum"></param>
        /// <param name="opNum"></param>
        /// <param name="useGold"></param>
        /// <param name="surplusGold"></param>
        /// <param name="totalGold"></param>
        public static void AppenPracticeLog(string userID, DateTime startDate, DateTime endDate, int continuedTime, int gainExperience)
        {
            UserPracticeLog log = new UserPracticeLog()
            {
                ID = Guid.NewGuid().ToString(),
                UserID = userID,
                StartDate = startDate,
                EndDate = endDate,
                ContinuedTime = continuedTime,
                GainExperience = gainExperience,
                CreateDate = DateTime.Now
            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        /// <summary>
        /// 玩家扫荡操作日志表log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="sumNum"></param>
        /// <param name="opNum"></param>
        /// <param name="useGold"></param>
        /// <param name="surplusGold"></param>
        /// <param name="totalGold"></param>
        public static void AppenRaidsLog(string userID, short opType, DateTime startDate, DateTime endDate, short energy, int useGold)
        {
            int continuedTime = (int)(endDate - startDate).TotalSeconds;
            UserRaidsLog log = new UserRaidsLog()
            {
                ID = Guid.NewGuid().ToString(),
                UserID = userID,
                OpType = opType,
                StartDate = startDate,
                EndDate = endDate,
                ContinuedTime = continuedTime,
                UseEnergy = energy,
                UseGold = useGold,
                CreateDate = DateTime.Now
            };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }

        /// <summary>
        /// 灵件操作日志
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sparePartID"></param>
        /// <param name="userSparepartID"></param>
        /// <param name="userItemID"></param>
        /// <param name="position"></param>
        /// <param name="partStatus"></param>
        public static void AppendSparePartLog(string userId, UserSparePart sparePart, short partStatus)
        {
            UserSparePartLog partLog = new UserSparePartLog()
                                          {
                                              UserID = userId,
                                              SparePartID = sparePart.SparePartId,
                                              SparePart = sparePart,
                                              PartStatus = partStatus,
                                              CreateDate = DateTime.Now
                                          };
            var sender = DataSyncManager.GetDataSender();
            sender.Send(partLog);
        }

        /// <summary>
        /// 玩家附魔符log
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="opType"></param>
        /// <param name="enchantID"></param>
        /// <param name="synthesisInfo"></param>
        /// <param name="enchantLv"></param>
        /// <param name="mature"></param>
        /// <param name="exp"></param>
        /// <param name="userEnchantID"></param>
        public static void AppenEnchantLog(string userID, short opType, UserEnchantInfo enchantInfo, CacheList<SynthesisInfo> synthesisInfo)
        {
            UserEnchantLog log = new UserEnchantLog();
            log.ID = Guid.NewGuid().ToString();
            log.UserID = userID;
            log.OpType = opType;
            log.UserEnchantID = enchantInfo.UserEnchantID;
            log.EnchantID = enchantInfo.EnchantID;
            log.EnchantLv = enchantInfo.EnchantLv;
            log.MaxMature = enchantInfo.MaxMature;
            log.Experience = enchantInfo.CurrExprience;
            log.SynthesisEnchant = synthesisInfo;
            log.CreateDate = DateTime.Now;

            var sender = DataSyncManager.GetDataSender();
            sender.Send(log);
        }
    }
}