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
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Combat;



namespace ZyGames.Tianjiexing.BLL.Action
{
    public class SdkError
    {
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    public class Action1004 : LoginExtendAction
    {
        public Action1004(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1004, httpGet)
        {
        }

        protected override bool DoSuccess(int userId)
        {
            //原因：重登录时，数据会回档问题
            var cacheSet = new GameDataCacheSet<GameUser>();
            GameUser userInfo = cacheSet.FindKey(Uid);
            if (userInfo != null)
            {
                //原因：还在加载中时，返回
                if (userInfo.IsRefreshing)
                {
                    Uid = string.Empty;
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().ServerLoading;
                    return false;
                }
            }
             
            if (userInfo == null ||
                string.IsNullOrEmpty(userInfo.SessionID) ||
                !userInfo.IsOnline)
            {
                UserCacheGlobal.Load(Uid); //重新刷缓存
                userInfo = cacheSet.FindKey(Uid);
            }
            if (userInfo != null)
            {
                if (userInfo.UserStatus == UserStatus.FengJin)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1004_IDDisable;
                    return false;
                }
                //todo
                //NoticeHelper.RankNotice(userInfo); //公告
                CombatHelper.LoadProperty(userInfo);
                //todo
                //NoviceHelper.OldFriendPack(Uid, userInfo.LoginTime); //好友礼包
                UserLoginLog userLoginLog = new UserLoginLog();
                userLoginLog.UserId = userInfo.UserID;
                userLoginLog.SessionID = Sid;
                userLoginLog.MobileType = MobileType;
                userLoginLog.ScreenX = ScreenX;
                userLoginLog.ScreenY = ScreenY;
                userLoginLog.RetailId = RetailID;
                userLoginLog.AddTime = DateTime.Now;
                userLoginLog.State = LoginStatus.Logined;
                userLoginLog.DeviceID = DeviceID;
                userLoginLog.Ip = this.GetRealIP();
                userLoginLog.Pid = userInfo.Pid;
                userLoginLog.UserLv = userInfo.UserLv;
                //原因：报表统计
                userLoginLog.PlotID = userInfo.PlotProgress;
                PlotInfo plotInfo = new ConfigCacheSet<PlotInfo>().FindKey(userInfo.PlotProgress);
                if (plotInfo != null)
                {
                    userLoginLog.PlotName = plotInfo.PlotName;
                }

                var sender = DataSyncManager.GetDataSender();
                sender.Send(userLoginLog);

                //int vipLv;
                //var vipLvArray = new ConfigCacheSet<VipLvInfo>().FindAll(u => u.PayGold <= userInfo.PayGold);
                //vipLv = vipLvArray.Count > 0 ? vipLvArray[vipLvArray.Count - 1].VipLv : (short)0;

                userInfo.LoginTime = DateTime.Now;
                userInfo.SessionID = Sid;
                userInfo.IsOnline = true;
                //userInfo.VipLv = vipLv;
                userInfo.GameId = GameType;
                userInfo.ServerId = ServerID;
                userInfo.ChatVesion = 0;
                userInfo.ChatDate = DateTime.MinValue;
                userInfo.BroadcastVesion = 0;
                if (userInfo.DailyLoginTime == MathUtils.SqlMinDate ||
                    userInfo.DailyLoginTime.Date != DateTime.Now.Date)
                {
                    userInfo.DailyLoginTime = DateTime.Now;
                }
                //todo
                RankingHelper.DailySportsRankPrize(userInfo);
            }
            else
            {
                ErrorCode = 1005;
                ErrorInfo = LanguageManager.GetLang().St1005_RoleCheck;
            }
            return true;
        }
    }
}