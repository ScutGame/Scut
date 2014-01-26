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
    public class Login36you : AbstractLogin
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.Login36you"/> class.
		/// </summary>
		/// <param name="passportID">Passport I.</param>
		/// <param name="password">Password.</param>
		/// <param name="deviceID">Device I.</param>
        public Login36you(string passportID, string password, string deviceID)
        {
            this.PassportID = passportID;
            this.DeviceID = deviceID;
            Password = password;
        }
		/// <summary>
		/// 注册通行证
		/// </summary>
		/// <returns></returns>
        public override string GetRegPassport()
        {
            string[] userList = SnsManager.GetRegPassport(DeviceID);
            if (userList.Length == 2)
            {
                PassportID = userList[0];
                Password = CryptoHelper.DES_Decrypt(userList[1], GameEnvironment.Setting.ProductDesEnKey);
            }
            return PassportID;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override bool CheckLogin()
        {
            string pwd = Password;
            try
            {
                pwd = new DESAlgorithmNew().DecodePwd(pwd, GameEnvironment.Setting.ClientDesDeKey);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Decode pwd error:{0}", ex);
            }
            pwd = CryptoHelper.DES_Encrypt(pwd, GameEnvironment.Setting.ProductDesEnKey);
            Password = pwd;
            //快速登录
            UserID = SnsManager.LoginByDevice(PassportID, Password, DeviceID).ToString();

            if (!string.IsNullOrEmpty(UserID) && UserID != "0")
            {
                if (string.IsNullOrEmpty(SessionID))
                {
                    SessionID = GetSessionId();
                }
                return true;
            }
            //TraceLog.WriteError("LoginByDevice pid:{0},pwd:{1},device:{2},uid:{3}", PassportID, Password, DeviceID, UserID);
            return false;
        }

    }
}