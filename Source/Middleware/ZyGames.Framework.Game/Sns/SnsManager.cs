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
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 用户中心登录管理类
    /// </summary>
    public class SnsManager
    {
        /// <summary>
        /// 获取通行证
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static string[] GetRegPassport(string deviceID)
        {
            if (!SnsCenterUser.CheckDevice(deviceID))
                throw (new Exception("禁止登入"));
            List<string> list = new List<string>();
            SnsCenterUser user = SnsCenterUser.GetUserByDeviceID(deviceID);

            if (user != null)
            {
                list.Add(user.PassportId);
                list.Add(user.Password);
            }
            else
            {
                SnsPassport passport = new SnsPassport();
                string password = passport.GetRandomPwd();
                list.Add(passport.GetRegPassport());
                list.Add(CryptoHelper.DES_Encrypt(password, GameEnvironment.Setting.ProductDesEnKey));
            }

            return list.ToArray();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Register(string user, string password)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(user, password, string.Empty);
            if (snsCenterUser.GetUserId() == 0)
            {
                return snsCenterUser.InsertSnsUser() > 0;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="password"></param>
        /// <param name="deviceID"></param>
        /// <param name="openId"></param>
        /// <returns>0:失败, 1:成功 2:已绑定</returns>
        public static int RegisterWeixin(string pid, string password, string deviceID, string openId)
        {
            SnsCenterUser snsCenterUser = new SnsCenterUser(pid, password, deviceID);

            if (snsCenterUser.GetUserId() > 0)
            {
                if (string.IsNullOrEmpty(snsCenterUser.WeixinCode))
                {
                    SnsUser snsuser = new SnsUser() { PassportId = pid, WeixinCode = openId };
                    return snsCenterUser.ChangeUserInfo(pid, snsuser) > 0 ? 1 : 0;
                }
                return 2;
            }
            return 0;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="password"></param>
        /// <param name="paramNames"></param>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public static string RegisterPassport(string password, string[] paramNames, string[] paramValues)
        {
            string pid = new SnsPassport().GetRegPassport();
            SnsCenterUser snsCenterUser = new SnsCenterUser(pid, password, string.Empty);
            if (snsCenterUser.InsertSnsUser(paramNames, paramValues) > 0)
            {
                return pid;
            }
            else
            {
                return string.Empty;
            }
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
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static int LoginByDevice(string user, string password, string deviceID)
        {
            if (!SnsCenterUser.CheckDevice(deviceID))
                throw (new Exception("禁止登录"));
            SnsCenterUser snsCenterUser = new SnsCenterUser(user, password, deviceID);
            int userID = snsCenterUser.GetUserId();
            if (userID == 0 && !snsCenterUser.IsExist())
            {
                userID = snsCenterUser.InsertSnsUser();
            }
            SnsCenterUser.AddLoginLog(deviceID, user);
            return userID;
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
        /// <param name="retailID"></param>
        /// <param name="retailUser"></param>
        /// <returns></returns>
        public static string[] LoginByRetail(string retailID, string retailUser)
        {
            string[] sArr = new string[2];
            SnsCenterUser snsCenterUser = new SnsCenterUser()
            {
                RetailID = retailID,
                RetailUser = retailUser
            };
            int userID = snsCenterUser.GetUserId();
            if (userID == 0 && !snsCenterUser.IsExistRetail())
            {
                userID = snsCenterUser.InsertSnsUser();
            }
            sArr[0] = userID.ToString();
            sArr[1] = snsCenterUser.PassportId;
            return sArr;
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
            return snsCenterUser.GetUserId() > 0;
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