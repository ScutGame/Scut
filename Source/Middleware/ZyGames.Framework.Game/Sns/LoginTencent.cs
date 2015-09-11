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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 腾讯0018
    /// </summary>
    public class LoginTencent : AbstractLogin
    {
        private string _retailID = string.Empty;
        private string _pid = string.Empty;
        private string _retailUser = string.Empty;
        private string AppId = string.Empty;
        private string AppKey = string.Empty;
        private string _code = string.Empty;
        private string Url = string.Empty;
        private string _aceessTokenUrl = string.Empty;
        private string _appSecret = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.LoginTencent"/> class.
        /// </summary>
        public LoginTencent(string retailID, string retailUser, string token)
        {
            this._retailID = retailID;
            this._retailUser = retailUser;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetRegPassport()
        {
            return this.PassportID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckLogin()
        {
            string[] arr = SnsManager.LoginByRetail(_retailID, _retailUser);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            QihooUserID = _retailUser;
            SessionID = GetSessionId();
            return true;
        }
    }
}
