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
using System.Text.RegularExpressions;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.DataModel;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 创建角色
    /// </summary>
    public class Action1005 : RegisterAction
    {
        private short _careerID = 0;
        private int generalID = 0;

        public Action1005(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1005, httpGet)
        {
        }

        protected override bool GetActionParam()
        {
            if (httpGet.GetWord("CareerID", ref _careerID)
                && httpGet.GetInt("GeneralID", ref generalID))
            {
                return true;
            }
            return false;
        }
        //protected override bool ValidateElement()
        //{
        //    return UserHelper.GetKeyWordSubstitution(UserName);

        //}
        public override bool TakeAction()
        {
            int MaxLength = ConfigEnvSet.GetInt("User.MaxLength");
            int length = System.Text.Encoding.Default.GetByteCount(UserName);

            if (UserHelper.GetKeyWordSubstitution(UserName))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1005_RegistNameKeyWord;
                return false;
            }
            if (GameUser.IsNickName(UserName))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1005_Rename;
                return false;
            }
            if (length <= 0 || length > MaxLength)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = string.Format(LanguageManager.GetLang().St1005_KingNameTooLong, MaxLength);
                return false;
            }
            Regex re = new Regex(@"^[\u4e00-\u9fa5\w]+$");
            if (!re.IsMatch(UserName))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1005_RegistNameExceptional;
                return false;
            }
            if (!Pid.ToLower().StartsWith("z"))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1005_PassportError;
                return false;
            }
            GeneralInfo general = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            if (general == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                return false;
            }
            CareerInfo careerInfo = new ConfigCacheSet<CareerInfo>().FindKey(general.CareerID);
            if (careerInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1005_Professional;
                return false;
            }

            GameUser userEntity = new GameDataCacheSet<GameUser>().FindKey(Uid);
            if (userEntity != null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1005_RoleExist;
                return false;
            }
            bool userSex = Sex == 0 ? false : true;

            if (new GameDataCacheSet<GameUser>().FindKey(Uid) == null)
            {
                userEntity = CreateGameUser(userSex);
                Current.User = userEntity;
                NoviceHelper.RetailLoginDaysReceive(userEntity); //渠道登录奖励
                CreateGeneral(careerInfo);
                CreateMagic(userEntity);
                //每日限制次数
                CreateDailyRestrain();
                //开启默认功能
                EnableFunction();
                //SendGifItem();
                UserPrayHelper.AddUserPray(Uid.ToInt(), PrayType.SanTianQiDao);
                UserPackHelper.AddUserPack(Uid);
                UserLoginLog userLoginLog = new UserLoginLog();
                userLoginLog.UserId = Uid;
                userLoginLog.SessionID = Sid;
                userLoginLog.MobileType = MobileType;
                userLoginLog.ScreenX = ScreenX;
                userLoginLog.ScreenY = ScreenY;
                userLoginLog.RetailId = RetailID;
                userLoginLog.AddTime = DateTime.Now;
                userLoginLog.State = LoginStatus.Logined;
                userLoginLog.DeviceID = DeviceID;
                userLoginLog.Ip = this.GetRealIP();
                userLoginLog.Pid = userEntity.Pid;
                userLoginLog.UserLv = userEntity.UserLv;
                var sender = DataSyncManager.GetDataSender();
                sender.Send(userLoginLog);
                BackPackHelper.AddBack(userEntity);
                UserHelper.OpenMagic(Uid, 1);
                //封测注册发放礼包
                UserItemHelper.AddUserItem(Uid, 1704, 1, ItemStatus.BeiBao);
            }
            else
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1005_RoleExist;
                return false;
            }
            PlotHelper.EnablePlot(Uid, ConfigEnvSet.GetInt("UserPlot.OpenLockPlotID"));
            return true;
        }

        /// <summary>
        /// 送新手礼包
        /// </summary>
        private void SendGifItem()
        {
            const int newitemID = 5000;
            UserItemHelper.AddUserItem(Uid, newitemID, 1);

            int itemID = 0;  //送武器
            if (_careerID == 1)
            {
                itemID = 1027;
            }
            else if (_careerID == 2)
            {
                itemID = 1020;
            }
            else if (_careerID == 3)
            {
                itemID = 1013;
            }
            UserItemHelper.AddUserItem(Uid, itemID, 1, ItemStatus.YongBing);
        }

        private void EnableFunction()
        {
            string[] funList = ConfigEnvSet.GetString("User.DefaultFunction").Split(new[] { ',' });
            var cacheSet = new GameDataCacheSet<UserFunction>();
            foreach (string fun in funList)
            {
                if (fun.TrimEnd().Length > 0)
                {
                    UserFunction userFun = new UserFunction();
                    userFun.UserID = Uid;
                    userFun.FunEnum = fun.ToEnum<FunctionEnum>();
                    userFun.CreateDate = DateTime.Now;
                    cacheSet.Add(userFun);
                    cacheSet.Update();
                }
            }
        }

        private void CreateDailyRestrain()
        {
            if (new GameDataCacheSet<UserDailyRestrain>().FindKey(Uid) == null)
            {
                UserDailyRestrain dailyRestrain = new UserDailyRestrain();

                dailyRestrain.UserID = Uid;
                dailyRestrain.RefreshDate = DateTime.Now;
                dailyRestrain.Funtion1 = 0;
                dailyRestrain.Funtion2 = 0;
                dailyRestrain.Funtion3 = 0;
                dailyRestrain.Funtion4 = 0;
                dailyRestrain.Funtion5 = 0;
                dailyRestrain.Funtion6 = 0;
                dailyRestrain.Funtion7 = 0;
                dailyRestrain.Funtion8 = 0;
                dailyRestrain.Funtion9 = 0;
                var cacheSet = new GameDataCacheSet<UserDailyRestrain>();
                cacheSet.Add(dailyRestrain);
                cacheSet.Update();
            }
        }

        private void CreateMagic(GameUser userEntity)
        {
            UserMagic userMagic = new UserMagic
            {
                UserID = userEntity.UserID,
                IsEnabled = true,
                MagicID = new GameUser().UserMagicID,
                MagicLv = 1,
                MagicType = MagicType.MoFaZhen
            };
            var cacheUserMagic = new GameDataCacheSet<UserMagic>();
            cacheUserMagic.Add(userMagic);
            cacheUserMagic.Update();

            //初始阵法
            string[] gridRange = new ConfigCacheSet<MagicLvInfo>().FindKey(userMagic.MagicID, userMagic.MagicLv).GridRange.Split(new char[] { ',' });
            if (gridRange.Length > 0)
            {
                string grid = gridRange[0];
                UserEmbattle userEmbattle = new UserEmbattle
                {
                    UserID = userEntity.UserID,
                    GeneralID = generalID,
                    MagicID = userMagic.MagicID,
                    Position = grid.ToShort()

                };
                var cacheEmbattle = new GameDataCacheSet<UserEmbattle>();
                cacheEmbattle.Add(userEmbattle);
                cacheEmbattle.Update();
            }
        }

        private void CreateGeneral(CareerInfo careerInfo)
        {
            GeneralInfo general = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            List<UserGeneral> userGeneralArray = new GameDataCacheSet<UserGeneral>().FindAll(Uid);
            if (userGeneralArray.Count > 0 || general == null)
            {
                return;
            }

            UserGeneral userGeneral = new UserGeneral();
            userGeneral.UserID = Uid;
            userGeneral.GeneralID = general.GeneralID;
            userGeneral.GeneralName = general.GeneralName;
            userGeneral.HeadID = general.HeadID;
            userGeneral.PicturesID = general.PicturesID;
            userGeneral.GeneralLv = (short)careerInfo.Lv;
            userGeneral.GeneralType = GeneralType.YongHu;
            userGeneral.CareerID = _careerID;
            userGeneral.PowerNum = general.PowerNum;
            userGeneral.SoulNum = general.SoulNum;
            userGeneral.IntellectNum = general.IntellectNum;
            userGeneral.TrainingPower = 0;
            userGeneral.TrainingSoul = 0;
            userGeneral.TrainingIntellect = 0;
            userGeneral.AbilityID = general.AbilityID;
            userGeneral.Momentum = 0;
            userGeneral.Description = string.Empty;
            userGeneral.HitProbability = ConfigEnvSet.GetDecimal("Combat.HitiNum");
            userGeneral.GeneralStatus = GeneralStatus.DuiWuZhong;
            userGeneral.CurrExperience = 0;
            userGeneral.Experience1 = 0;
            userGeneral.Experience2 = 0;
            //userGeneral.LifeNum = MathUtils.Addition(general.LifeNum, careerInfo.LifeIncreaseNum * (MathUtils.Subtraction(careerInfo.Lv, (short)1, (short)0)), int.MaxValue);
            userGeneral.RefreshMaxLife();
            userGeneral.HeritageType = HeritageType.Normal;
            userGeneral.AbilityNum = 3;
            var cacheSet = new GameDataCacheSet<UserGeneral>();
            cacheSet.Add(userGeneral);
            cacheSet.Update();
            UserAbilityHelper.AddUserAbility(general.AbilityID, Uid.ToInt(), generalID, 1);
          
        }

        private GameUser CreateGameUser(bool userSex)
        {
            GameUser userEntity = new GameUser
            {
                UserID = Uid,
                CountryID = 0,
                CityID = ConfigEnvSet.GetInt("user.UserMinCityID"),
                PointX = (short)ConfigEnvSet.GetInt("User.CityPointX"),
                PointY = (short)ConfigEnvSet.GetInt("User.CityPointY"),
                SessionID = Sid,
                NickName = UserName,
                Sex = userSex,
                UserLv = (short)ConfigEnvSet.GetInt("User.Level"),
                ObtainNum = ConfigEnvSet.GetInt("User.ObtainNum"),
                EnergyNum = new GameUser().EnergyMaxNum,
                MercenariesID = string.Empty,
                GiftGold = ConfigEnvSet.GetInt("User.GiftGold"),
                PayGold = 0,
                UseGold = 0,
                ItemGold = 0,
                GameCoin = ConfigEnvSet.GetInt("User.GameCoin"),
                ExpNum = 0,
                VipLv = ConfigEnvSet.GetInt("User.VipLv"),
                UserStatus = UserStatus.Normal,
                MsgState = true,
                GridNum = new GameUser().GridMinNum,
                WarehouseNum = new GameUser().WarehouseMinNum,
                QueueNum = 0,
                RetailID = RetailID,
                Pid = Pid,
                MobileType = MobileType,
                ScreenX = ScreenX,
                ScreenY = ScreenY,
                ClientAppVersion = ReqAppVersion,
                CreateDate = DateTime.Now,
                UseMagicID = new GameUser().UserMagicID,
                CrystalNum = new GameUser().CrystalMinNum,
                UserLocation = Location.City,
                UserExtend = new GameUserExtend(),
                SweepPool = new SweepPoolInfo(),
                GameId = GameID,
                ServerId = ServerID,
                LoginTime = DateTime.Now,
                DailyLoginTime = DateTime.Now
            };
            var cacheSet = new GameDataCacheSet<GameUser>();
            cacheSet.Add(userEntity);
            cacheSet.Update();
            //增加初始背包、灵件、水晶
            new GameDataCacheSet<UserItemPackage>().Add(new UserItemPackage { UserID = userEntity.UserID });
            new GameDataCacheSet<UserCrystalPackage>().Add(new UserCrystalPackage() { UserID = userEntity.UserID });
            new GameDataCacheSet<UserSparePackage>().Add(new UserSparePackage() { UserID = userEntity.UserID });
            //增加初始附魔符
            new GameDataCacheSet<UserEnchant>().Add(new UserEnchant(userEntity.UserID));
            //增加初始副本
            new GameDataCacheSet<UserPlotPackage>().Add(new UserPlotPackage(userEntity.UserID));
            // 增加初始集邮册
            new GameDataCacheSet<UserAlbum>().Add(new UserAlbum(userEntity.UserID));
            // 玩家圣吉塔
            new GameDataCacheSet<UserShengJiTa>().Add(new UserShengJiTa(userEntity.UserID.ToInt()));

            return userEntity;
        }

    }
}