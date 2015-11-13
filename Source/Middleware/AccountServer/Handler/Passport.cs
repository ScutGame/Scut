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
using AccountServer.Handler.Data;
using AccountServer.Lang;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Game.Sns.Service;

namespace AccountServer.Handler
{
    /// <summary>
    /// Generate passport
    /// </summary>
    public class Passport : BaseHandler, IHandler<IMEIInfo>
    {
        public ResponseData Excute(IMEIInfo data)
        {
            //fixed allow null for imei
            //if (string.IsNullOrEmpty(data.IMEI))
            //{
            //    throw new HandlerException(StateCode.Error, StateDescription.IMEINullError);
            //}
            string[] userList = SnsManager.GetRegPassport(data.IMEI, data.IsNew, EncodePassword);
            return new PassportInfo() { PassportId = userList[0], Password = userList[1] };
        }
    }
}