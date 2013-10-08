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
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Framework.Game.Lang
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseZHLanguage : ILanguage
    {
        public virtual int ErrorCode { get { return 10000; } }
        public virtual int TimeoutCode { get { return 10001; } }
        public virtual int KickedOutCode { get { return 10002; } }

        public int ValidateCode { get { return 10003; } }
        public virtual string ValidateError { get { return "文字中包含不当内容，请重新输入!"; } }
        public virtual string ServerBusy { get { return "系统繁忙中!"; } }
        public virtual string UrlElement { get { return "请求参数错误!"; } }

        public virtual string UrlNoParam { get { return "参数名:{0}不存在"; } }
        public virtual string UrlParamOutRange { get { return "参数名:{0}超出范围[{1}-{2}]"; } }

        public string RequestTimeout { get { return "请求响应超时！"; } }
        public string ServerMaintain { get { return "服务器正在维护！"; } }
        public virtual string ServerLoading { get { return "服务器正在重启中，请稍后..."; } }
        public string PasswordError { get { return "您输入的账号或密码不正确!"; } }
        public string LoadDataError { get { return "数据加载失败!"; } }
        public string AcountIsLocked { get { return "该账号已被封禁，登录失败！"; } }
        public string AcountNoLogin { get { return "您的账号未登录或已过期！"; } }
        public string AcountLogined { get { return "您的账号已在其它地方登录！"; } }
        public string AppStorePayError { get { return "充值失败!"; } }

        public string GetAccessFailure
        {
            get { return "获取受权失败！"; }
        }
    }
}