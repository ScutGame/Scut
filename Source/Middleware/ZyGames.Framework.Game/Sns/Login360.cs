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
using System.Web;

namespace ZyGames.Framework.Game.Sns
{
    public class Login360 : ILogin
    {
        private string _retailID = string.Empty;
        private string _pid = string.Empty;

        public Login360(string retailID, string pid)
        {
            this._retailID = retailID;
            this._pid = pid;
        }

        #region ILogin 成员

        public string PassportID
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string SessionID
        {
            get;
            set;
        }

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

        public string GetRegPassport()
        {
            return this.PassportID;
        }

        public bool CheckLogin()
        {
            string[] arr = SnsManager.LoginByRetail(_retailID, _pid);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            this.SessionID = GetSessionId();
            return true;
        }

        #endregion
    }
}