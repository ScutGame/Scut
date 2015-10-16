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

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SnsUser
    {
        /// <summary>
        /// 微信号OpenID
        /// </summary>
        public string WeixinCode
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string IMEI { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PassportId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public RegType RegType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PwdType PwdType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RetailID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RetailUser
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mail
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RealName
        {
            get;
            set;
        }
        /// <summary>
        /// 身份ID
        /// </summary>
        public string IDCards
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RegTime
        {
            get;
            set;
        }
        /// <summary>
        /// 激活码
        /// </summary>
        public string ActiveCode
        {
            get;
            set;
        }
        /// <summary>
        /// 发送激活码时间
        /// </summary>
        public DateTime SendActiveDate
        {
            get;
            set;
        }
        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime ActiveDate
        {
            get;
            set;
        }
    }
}