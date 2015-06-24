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
using System.Web;

namespace AccountServer.Lang
{
    public static class StateDescription
    {
        public const string Error = "账号服务器异常！";
        public const string IMEINullError = "IMEI为空！";
        public const string SignError = "签名错误！";
        public const string NoHandler = "无处理程序！";
        public const string PassworkLengthError = "密码长度错误！";
        public const string PassworkError = "密码错误！";
        public const string PasswordOrPassError = "账号或密码为空！";
        public const string RegistError = "账号已存在，注册失败！";
        public const string ChangePassError = "修改密码失败！";

        public const string NoToken = "登录凭证无效！";
        public const string TokenExpired= "登录凭证已过期！";
    }
}