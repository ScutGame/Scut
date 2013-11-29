using System;
using ZyGames.Doudizhu.Bll;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Doudizhu.Lang;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Net;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Cache;

namespace ZyGames.Doudizhu.Script.Action
{

    /// <summary>
    /// 1004_用户登录
    /// </summary>
    public class Action1004 : LoginExtendAction
    {
        public Action1004(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1004, httpGet)
        {
        }

        protected override bool DoSuccess(int userId)
        {
            var cacheSet = new GameDataCacheSet<GameUser>();
            GameUser gameUser = cacheSet.FindKey(Uid);
            if (gameUser == null ||
                string.IsNullOrEmpty(gameUser.SessionID) ||
                !gameUser.IsInlining)
            {
                gameUser = cacheSet.FindKey(Uid);
            }

            if (gameUser != null)
            {
                //原因：还在加载中时，返回
                if (gameUser.Property.IsRefreshing)
                {
                    Uid = string.Empty;
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().ServerLoading;
                    return false;
                }
                
            }

            var nowTime = DateTime.Now;
            if (gameUser == null)
            {
                this.ErrorCode = 1005;
                return true;
            }
            else
            {
                if (gameUser.UserStatus == UserStatus.FengJin)
                {
                    ErrorCode = LanguageManager.GetLang().TimeoutCode;
                    ErrorInfo = LanguageManager.GetLang().AcountIsLocked;
                    return false;
                }
                gameUser.SessionID = Sid;
                gameUser.OnlineDate = nowTime;
                gameUser.LoginDate = nowTime;
                gameUser.Property.GameId = this.GameType;
                gameUser.Property.ServerId = this.ServerID;
                gameUser.Property.ChatVesion = 0;
                //gameUser.OnLine = true;
                //gameUser.Logoff = true;
            }



            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                //登录日志
                UserLoginLog userLoginLog = new UserLoginLog();
                userLoginLog.UserId = gameUser.UserId.ToString();
                userLoginLog.SessionID = Sid;
                userLoginLog.MobileType = this.MobileType.ToShort();
                userLoginLog.ScreenX = this.ScreenX;
                userLoginLog.ScreenY = this.ScreenY;
                userLoginLog.RetailId = this.RetailID;
                userLoginLog.AddTime = nowTime;
                userLoginLog.State = LoginStatus.Logined.ToInt();
                userLoginLog.DeviceID = this.DeviceID;
                userLoginLog.Ip = this.GetRealIP();
                userLoginLog.Pid = gameUser.Pid;
                userLoginLog.UserLv = gameUser.UserLv;
                var sender = DataSyncManager.GetDataSender();

                sender.Send(userLoginLog);
            });
            return true;
        }
    }
}