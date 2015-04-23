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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 用户中心登录管理类
    /// </summary>
    public class SnsManager
    {
        class PassportExpired
        {
            public PassportExpired(string pid)
            {
                Pid = pid;
                ExpiredTime = DateTime.Now.AddSeconds(10);//5s 后可以产生新的PID
            }
            public string Pid { get; set; }
            public DateTime ExpiredTime { get; set; }
        }
        /// <summary>
        /// 防多次点击产生多个账号与imei绑定
        /// </summary>
        private static ConcurrentDictionary<string, PassportExpired> imeiMap = new ConcurrentDictionary<string, PassportExpired>();

        /// <summary>
        /// 获取通行证
        /// </summary>
        /// <param name="imei">if null then get new pid</param>
        /// <returns></returns>
        public static string[] GetRegPassport(string imei)
        {
            if (!SnsCenterUser.CheckDevice(imei))
                throw (new Exception("禁止登入"));
            var list = new List<string>();
            SnsCenterUser user = SnsCenterUser.GetUserByDeviceId(imei);
            string passportId = string.Empty;

            if (user != null)
            {
                passportId = user.PassportId;
                list.Add(passportId);
                list.Add(user.Password);
            }
            else
            {
                SnsPassport passport = new SnsPassport();
                PassportExpired passportExpired;
                if (!imeiMap.TryGetValue(imei, out passportExpired))
                {
                    passportId = passport.GetRegPassport();
                    imeiMap[imei] = new PassportExpired(passportId);
                }
                else
                {
                    passportId = passportExpired.Pid;
                    if (passportExpired.ExpiredTime < DateTime.Now)
                    {
                        //过期移除
                        imeiMap.TryRemove(imei, out passportExpired);
                    }
                    //检查超出
                    List<string> expiredMap;
                    if (imeiMap.Count > 100 && ((expiredMap = imeiMap.Where(t => t.Value.ExpiredTime < DateTime.Now).Select(t => t.Key).ToList()).Count > 10))
                    {
                        foreach (var expired in expiredMap)
                        {
                            imeiMap.TryRemove(expired, out passportExpired);
                        }
                    }
                }

                string password = passport.GetRandomPwd();
                list.Add(passportId);
                list.Add(password);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Register(string pid, string password)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(pid, password, string.Empty);
            var snsuser = snsCenterUser.GetUserInfo(pid);
            if (snsuser.UserId <= 0)
            {
                return snsCenterUser.InsertSnsUser() > 0;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="password"></param>
        /// <param name="imei"></param>
        /// <param name="openId"></param>
        /// <returns>0:失败, 1:成功 2:已绑定</returns>
        public static int RegisterWeixin(string pid, string password, string imei, string openId)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(pid, password, imei);
            var snsuser = snsCenterUser.GetUserInfo(pid);
            if (snsuser.UserId > 0)
            {
                if (string.IsNullOrEmpty(snsuser.WeixinCode))
                {
                    return snsCenterUser.ChangeUserInfo(pid, new SnsUser() { PassportId = pid, WeixinCode = openId }) > 0 ? 1 : 0;
                }
                return 2;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passportId"></param>
        /// <param name="password"></param>
        /// <returns>userid</returns>
        public static int QuickRegisterPassport(string passportId, string password)
        {
            return DoRegisterPassport(passportId, password, null, null);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="password"></param>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <returns>pid</returns>
        public static string RegisterPassport(string password, string[] paramNames = null, string[] paramValues = null)
        {
            string pid = new SnsPassport().GetRegPassport();
            DoRegisterPassport(pid, password, paramNames, paramValues);
            return pid;
        }

        private static int DoRegisterPassport(string passportId, string password, string[] paramNames, string[] paramValues)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(passportId, password, string.Empty);
            var snsuser = snsCenterUser.GetUserInfo(passportId);
            if (snsuser.UserId > 0)
            {
                return 0;
            }
            return snsCenterUser.InsertSnsUser(paramNames, paramValues);
        }

        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int GetUserType(string user)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(user, string.Empty, string.Empty);
            return (int)snsCenterUser.GetUserType();
        }

        /// <summary>
        /// 登录WEB调用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int Login(string user, string password)
        {
            return LoginByDevice(user, password, string.Empty);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static int LoginByDevice(string user, string password, string imei)
        {
            if (!SnsCenterUser.CheckDevice(imei))
                throw (new Exception("禁止登录"));
            int userId = 0;
            var snsCenterUser = new SnsCenterUser(user, password, imei);
            var snsUser = snsCenterUser.GetUserInfo(user);
            if (snsUser == null || snsUser.UserId <= 0)
            {
                userId = snsCenterUser.InsertSnsUser();
                SnsCenterUser.AddLoginLog(imei, user);

                //过期移除
                PassportExpired passportExpired;
                imeiMap.TryRemove(imei, out passportExpired);
                return userId;
            }
            if (snsCenterUser.ValidatePassport(snsUser))
            {
                return snsUser.UserId;
            }
            SnsCenterUser.AddLoginLog(imei, user);
            return userId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static SnsUser LoginByWeixin(string openId)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser();
            return snsCenterUser.GetUserByWeixin(openId);
        }

        /// <summary>
        /// 通道商验证登录
        /// </summary>
        /// <param name="retailId"></param>
        /// <param name="retailUser"></param>
        /// <returns></returns>
        public static string[] LoginByRetail(string retailId, string retailUser)
        {
            string[] result = new string[2];
            SnsCenterUser snsCenterUser = new SnsCenterUser();
            var snsuser = snsCenterUser.GetUserInfo(retailId, retailUser);
            if (snsuser.UserId <= 0)
            {
                //自动获取通行证
                SnsPassport passport = new SnsPassport();
                string pid = passport.GetRegPassport();
                string pwd = passport.GetRandomPwd();
                //modify login of retail bug.
                snsCenterUser = new SnsCenterUser(pid, pwd, string.Empty) { RetailID = retailId, RetailUser = retailUser };
                result[0] = snsCenterUser.InsertSnsUser().ToString();
                result[1] = pid;
                return result;
            }
            result[0] = snsuser.UserId.ToString();
            result[1] = snsuser.PassportId;
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int ChangePass(string user, string password)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(user, password, string.Empty);
            return snsCenterUser.ChangePass(user);
        }

        /// <summary>
        /// 通行证检查
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckPassport(string user)
        {
            SnsPassport passport = new SnsPassport();
            return passport.VerifyRegPassportId(user);
        }

        /// <summary>
        /// 检查通行证密码
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckPassportPwd(string pid, string password)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(pid, password, string.Empty);
            var snsuser = snsCenterUser.GetUserInfo(pid);
            return snsCenterUser.ValidatePassport(snsuser);
        }

        /// <summary>
        /// 补全用户信息
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="snsuser"></param>
        /// <returns></returns>
        public static int ChangeUserInfo(string pid, SnsUser snsuser)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser();
            return snsCenterUser.ChangeUserInfo(pid, snsuser);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static SnsUser GetUserInfo(string pid)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser();
            return snsCenterUser.GetUserInfo(pid);
        }
    }
}