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
    /// 登录处理接口
    /// </summary>
    public interface ILogin
    {
        /// <summary>
        /// 
        /// </summary>
        string PassportID { get;}
        /// <summary>
        /// 
        /// </summary>
        string UserID { get; }

        /// <summary>
        /// 用户类型
        /// </summary>
        int UserType { get; }
        /// <summary>
        /// 
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string SessionID { get; }

        /// <summary>
        /// 注册通行证
        /// </summary>
        /// <returns></returns>
        string GetRegPassport();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CheckLogin();

    }

}