using System;
using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Bll.Com.Share;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Net;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{

    /// <summary>
    /// 1005_创建角色
    /// </summary>
    public class Action1005 : RegisterAction
    {

        public Action1005(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1005, httpGet)
        {

        }

        protected override bool GetActionParam()
        {
            return true;
        }

        public override bool TakeAction()
        {

            GameUser user = new GameDataCacheSet<GameUser>().FindKey(Uid);
            if (user == null)
            {
                var roleFunc = new RoleFunc();
                string msg;

                if (roleFunc.VerifyRange(UserName, out msg) ||
                    roleFunc.VerifyKeyword(UserName, out msg) ||
                    roleFunc.IsExistNickName(UserName, out msg))
                {
                    ErrorCode = Language.Instance.ErrorCode;
                    ErrorInfo = msg;
                    return false;
                }
                user = CreateRole();
                roleFunc.OnCreateAfter(user);
            }
            Current.User = user;
            UserLoginLog userLoginLog = new UserLoginLog();
            userLoginLog.UserId = Uid;
            userLoginLog.SessionID = Sid;
            userLoginLog.MobileType = (short)MobileType;
            userLoginLog.ScreenX = ScreenX;
            userLoginLog.ScreenY = ScreenY;
            userLoginLog.RetailId = RetailID;
            userLoginLog.AddTime = DateTime.Now;
            userLoginLog.State = (short)LoginStatus.Logined;
            userLoginLog.DeviceID = DeviceID;
            userLoginLog.Ip = GetRealIP();
            userLoginLog.Pid = user.Pid;
            userLoginLog.UserLv = user.UserLv;
            var sender = DataSyncManager.GetDataSender();
            sender.Send(userLoginLog);

            return true;
        }

        private GameUser CreateRole()
        {
            GameUser user = new GameUser(UserId);
            user.SessionID = Sid;
            user.Pid = Pid;
            user.HeadIcon = HeadID;
            user.RetailId = RetailID;
            user.NickName = UserName;
            user.RealName = "";
            user.Hobby = "";
            user.Profession = "";
            user.Sex = Sex.ToBool();
            user.UserLv = (short)ConfigEnvSet.GetInt("User.Level", 1);
            user.GiftGold = ConfigEnvSet.GetInt("User.GiftGold", 100);
            user.GameCoin = ConfigEnvSet.GetInt("User.GameCoin", 1000);
            user.VipLv = ConfigEnvSet.GetInt("User.VipLv");
            user.UserStatus = UserStatus.Normal;
            user.MsgState = true;
            user.MobileType = MobileType;
            user.ScreenX = ScreenX;
            user.ScreenY = ScreenY;
            user.ClientAppVersion = ReqAppVersion;
            user.LoginDate = DateTime.Now;
            user.CreateDate = DateTime.Now;
            user.TitleId = 1001;
            user.Property.GameId = GameID;
            user.Property.ServerId = ServerID;
            user.Birthday = new DateTime(1970, 1, 1);
            var cacheSet = new GameDataCacheSet<GameUser>();
            cacheSet.Add(user);
            cacheSet.Update();

            //增加初始背包、玩家任务表、玩家成就表
            var itemCacheSet = new GameDataCacheSet<UserItemPackage>();
            itemCacheSet.Add(new UserItemPackage(UserId));
            itemCacheSet.Update();
            var taskCacheSet = new GameDataCacheSet<UserTask>();
            taskCacheSet.Add(new UserTask(UserId));
            taskCacheSet.Update();
            var achieveCacheSet = new GameDataCacheSet<UserAchieve>();
            achieveCacheSet.Add(new UserAchieve(UserId));
            achieveCacheSet.Update();
            UserDailyRestrain restrain = new UserDailyRestrain(UserId);
            restrain.RefreshDate = DateTime.Now.AddDays(-1);
            var restrainCacheSet = new GameDataCacheSet<UserDailyRestrain>();
            restrainCacheSet.Add(restrain);
            restrainCacheSet.Update();
            return user;
        }

        public override void BuildPacket()
        {

        }

    }
}
