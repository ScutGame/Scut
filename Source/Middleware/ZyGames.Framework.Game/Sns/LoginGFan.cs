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
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Sns.Section;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 机峰0011
    /// </summary>
    public class LoginGFan : AbstractLogin
    {
        private string _retailID;
        private string _uid;
        private string _token;
        private string username = string.Empty;


        public LoginGFan(string retailID, string RetailUser)
        {
            this._retailID = retailID ?? "0011";
            _uid = RetailUser.Equals("0") ? string.Empty : RetailUser;
            
        }

        #region ILogin 成员

       

        protected static string GetSessionId()
        {
            string sessionId = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                sessionId = HttpContext.Current.Session.SessionID;
            }
            else
            {
                sessionId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            }
            return sessionId;
        }

        public override string GetRegPassport()
        {
            return this.PassportID;
        }

        public override bool CheckLogin()
        {
            
            if(string.IsNullOrEmpty(_uid))
            {
                TraceLog.ReleaseWrite("The ChannelGFansdk  uid is null.");
                return false;
            }
            
            string[] arr = SnsManager.LoginByRetail(_retailID, _uid);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            SessionID = GetSessionId();
            return true;
        }

        #endregion

        private string AMD5(string str1)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str1, "MD5").ToUpper();
        }

        private string SHA256(string str)
        {
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            sha256.Clear();
            string result = string.Empty;
            foreach (byte x in tmpByte)
            {
                result += string.Format("{0:x2}", x);
            }
            return result.ToUpper();
        }
    }

   
}