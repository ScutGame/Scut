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
using AccountServer.Handler.Data;
using AccountServer.Lang;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Game.Sns.Service;
using ZyGames.Framework.Common.Timing;
namespace AccountServer.Handler
{
    /// <summary>
    /// User login
    /// </summary>
    public class Login : BaseHandler, IHandler<LoginInfo>
    {
        public ResponseData Excute(LoginInfo data)
        {
            int userId;
            int userType;
            string passportId;
            if (!string.IsNullOrEmpty(data.RetailUser) && !string.IsNullOrEmpty(data.RetailToken))
            {
                ILogin login = LoginProxy.GetLogin(data.RetailID, data);
                login.Password = DecodePassword(login.Password);
                var watch = RunTimeWatch.StartNew("Request login server");
                try
                {
                    if (login.CheckLogin())
                    {
                        watch.Check("GetResponse");
                        userId = int.Parse(login.UserID);
                        passportId = login.PassportID;
                        userType = login.UserType;
                    }
                    else
                    {
                        //DoLoginFail();
                        throw new HandlerException(StateCode.Error, StateDescription.PassworkError);
                    }
                }
                finally
                {
                    watch.Flush(true, 100);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(data.Pwd) || data.Pwd.Length < 5)
                {
                    throw new HandlerException(StateCode.Error, StateDescription.PassworkLengthError);
                }
                data.Pwd = DecodePassword(data.Pwd);
                //快速登录
                RegType regType;
                userId = SnsManager.LoginByDevice(data.Pid, data.Pwd, data.DeviceID, out regType, data.IsCustom);
                if (userId <= 0)
                {
                    throw new HandlerException(StateCode.PassworkError, StateDescription.PassworkError);
                }
                passportId = data.Pid;
                userType = (int)regType;
            }

            return AuthorizeLogin(userId, passportId, userType);
        }

    }
}