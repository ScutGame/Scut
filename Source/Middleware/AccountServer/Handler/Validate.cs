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
using ZyGames.Framework.Game.Sns.Service;

namespace AccountServer.Handler
{
    /// <summary>
    /// User login token validated is expired
    /// </summary>
    public class Validate : IHandler<LoginToken>
    {
        public ResponseData Excute(LoginToken data)
        {
            UserToken userToken = null;
            if (string.IsNullOrEmpty(data.Token) || (userToken = HandlerManager.GetUserToken(data.Token)) == null)
            {
                throw new HandlerException(StateCode.NoToken, StateDescription.NoToken);
            }
            if (userToken.ExpireTime < DateTime.Now)
            {
                throw new HandlerException(StateCode.TokenExpired, StateDescription.TokenExpired);
            }
            return new LoginToken()
            {
                Token = userToken.Token,
                UserId = userToken.UserId,
                PassportId = userToken.PassportId,
                UserType = userToken.UserType
            };
        }
    }
}