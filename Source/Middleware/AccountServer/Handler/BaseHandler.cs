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
using AccountServer.Handler.Data;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Sns.Service;

namespace AccountServer.Handler
{
    public abstract class BaseHandler
    {
        protected string GenrateToken()
        {
            string sessionId;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                sessionId = HttpContext.Current.Session.SessionID;
            }
            else
            {
                sessionId = Guid.NewGuid().ToString("N");
            }
            return sessionId;
        }

        protected string EncodePassword(string password)
        {
            try
            {
                return new DESAlgorithmNew().EncodePwd(password ?? "", HandlerManager.ClientDesDeKey);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Encode password:\"{0}\" error:{1}", password, ex);
            }
            return password;
        }


        protected string DecodePassword(string password)
        {
            try
            {
                return new DESAlgorithmNew().DecodePwd(password ?? "", HandlerManager.ClientDesDeKey);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Decode password:\"{0}\" error:{1}", password, ex);
            }
            return password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passportId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        protected ResponseData AuthorizeLogin(int userId, string passportId, int userType)
        {
            UserToken userToken = new UserToken()
            {
                Token = GenrateToken(),
                UserId = userId,
                PassportId = passportId,
                UserType =  userType,
                ExpireTime = DateTime.Now.AddDays(1)
            };
            HandlerManager.SaveToken(userToken.Token, userToken);
            return new LoginToken()
            {
                Token = userToken.Token,
                UserId = userToken.UserId,
                PassportId = userToken.PassportId,
                UserType = userType
            };
        }
    }
}