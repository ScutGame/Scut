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
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 登录处理基类
    /// </summary>
    public abstract class AbstractLogin : ILogin
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value>The passport I.</value>
        public string PassportID { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value>The user I.</value>
        public string UserID
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <value>The session I.</value>
        public string SessionID
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserType
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceID
        {
            get;
            protected set;
        }
        /// <summary>
        /// 注册通行证
        /// </summary>
        /// <returns></returns>
        public abstract string GetRegPassport();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckLogin();
        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <returns>The session identifier.</returns>
        protected string GetSessionId()
        {
            string sessionId = string.Empty;
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
        /// <summary>
        /// 
        /// </summary>
        public string AccessToken
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RefeshToken
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string QihooUserID
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int ExpiresIn
        {
            get;
            protected set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Scope
        {
            get;
            protected set;
        }

        /// <summary>
        /// AMs the d5.
        /// </summary>
        /// <returns>The d5.</returns>
        /// <param name="str1">Str1.</param>
        protected string AMD5(string str1)
        {
            return ZyGames.Framework.Common.Security.CryptoHelper.MD5_Encrypt(str1, Encoding.UTF8).ToLower();
        }
        /// <summary>
        /// SHs the a256.
        /// </summary>
        /// <returns>The a256.</returns>
        /// <param name="str">String.</param>
        protected string SHA256(string str)
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

        /// <summary>
        /// UTF8编码字符串计算MD5值(十六进制编码字符串)
        /// </summary>
        /// <param name="sourceStr">UTF8编码的字符串</param>
        /// <returns>MD5(十六进制编码字符串)</returns>
        protected string HashToMD5Hex(string sourceStr)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = md5.ComputeHash(bytes);
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sBuilder.Append(result[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

    }
}