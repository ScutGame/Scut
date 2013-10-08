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
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 36you官网登录0000
    /// </summary>
    public class Login36you : ILogin
    {
        public Login36you(string passportID, string password, string deviceID)
        {
            this.PassportID = passportID;
            this.DeviceID = deviceID;
            Password = password;
        }

        public string UserID
        {
            get;
            set;
        }

        public string PassportID
        {
            get;
            set;
        }


        public string Password
        {
            get;
            set;
        }

        public string DeviceID
        {
            get;
            set;
        }

        public string SessionID
        {
            get;
            set;
        }

        #region ILogin 成员

        public string GetRegPassport()
        {
            string[] userList = SnsManager.GetRegPassport(DeviceID);
            if (userList.Length == 2)
            {
                PassportID = userList[0];
                Password = CryptoHelper.DES_Decrypt(userList[1], GameEnvironment.ProductDesEnKey);
            }
            return PassportID;
        }

        public bool CheckLogin()
        {
            try
            {
                string pwd = Password;
                pwd = new DESAlgorithmNew().DecodePwd(pwd, GameEnvironment.ClientDesDeKey);
                pwd = CryptoHelper.DES_Encrypt(pwd, GameEnvironment.ProductDesEnKey);
                Password = pwd;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Decode pwd error:{0}", ex);
            }
            //快速登录
            UserID = SnsManager.LoginByDevice(PassportID, Password, DeviceID).ToString();

            if (!string.IsNullOrEmpty(UserID) && UserID != "0")
            {
                SessionID = GetSessionId();
                return true;
            }
            TraceLog.WriteError("LoginByDevice pid:{0},pwd:{1},device:{2},uid:{3}", PassportID, Password, DeviceID, UserID);
            return false;
        }

        #endregion


        protected static string GetSessionId()
        {
            string sessionId = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                sessionId = HttpContext.Current.Session.SessionID;
            }
            else
            {
                sessionId = System.Guid.NewGuid().ToString().Replace("-", string.Empty);
            }
            return sessionId;
        }

    }
}