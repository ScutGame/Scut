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

namespace ZyGames.Framework.Game.Lang
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    public class BaseKoreanLanguage : ILanguage
    {
		/// <summary>
		/// 
		/// </summary>
		/// <value>The error code.</value>
        public virtual int ErrorCode { get { return 10000; } }
		/// <summary>
		/// 
		/// </summary>
		/// <value>The timeout code.</value>
        public virtual int TimeoutCode { get { return 10001; } }
		/// <summary>
		/// 踢出
		/// </summary>
		/// <value>The kicked out code.</value>
        public virtual int KickedOutCode { get { return 10002; } }
		/// <summary>
		/// 验证参数错误代码
		/// </summary>
		/// <value>The validate code.</value>
        public int ValidateCode { get { return 10003; } }
		/// <summary>
		/// 验证参数错误提示
		/// </summary>
		/// <value>The validate error.</value>
        public virtual string ValidateError { get { return "텍스트 내용에 포함되어, 다시 입력 해주세요!"; } }
		/// <summary>
		/// 系统繁忙中
		/// </summary>
		/// <value>The server busy.</value>
        public virtual string ServerBusy { get { return "이제 시스템이 사용 중입니다!"; } }
		/// <summary>
		/// 请求参数错误
		/// </summary>
		/// <value>The URL element.</value>
        public virtual string UrlElement { get { return "요청의 매개 변수 오류입니다!"; } }
		/// <summary>
		/// 参数名:{0}不存在
		/// </summary>
		/// <value>The URL no parameter.</value>
        public virtual string UrlNoParam { get { return "매개 변수 이름 : {0}이 존재하지 않습니다"; } }
		/// <summary>
		/// 参数名:{0}超出范围[{1}-{2}]
		/// </summary>
		/// <value>The URL parameter out range.</value>
        public virtual string UrlParamOutRange { get { return "매개 변수 이름 : {0} 범위를 벗어난 경우 [{1} - {2}]"; } }
		/// <summary>
		/// 请求超时
		/// </summary>
		/// <value>The request timeout.</value>

        public string RequestTimeout { get { return "요청 시간 초과!"; } }
		/// <summary>
		/// 服务器正在维护
		/// </summary>
		/// <value>The server maintain.</value>
        public string ServerMaintain { get { return "서버가 유지 보수를 위해 다운!"; } }
		/// <summary>
		/// 服务器正在重启中，请稍候...
		/// </summary>
		/// <value>The server loading.</value>
        public virtual string ServerLoading { get { return "서버가 다시 시작됩니다 기다려주십시오 ..."; } }
		/// <summary>
		/// 您输入的账号或密码不正确
		/// </summary>
		/// <value>The password error.</value>
        public virtual string PasswordError { get { return "당신은 계정을 입력 또는 암호가 올바르지 않습니다!"; } }
		/// <summary>
		/// 该账号已被封禁
		/// </summary>
		/// <value>The acount is locked.</value>
        public virtual string AcountIsLocked { get { return "계정이 잠겼습니다, ​​로그인 실패!"; } }
		/// <summary>
		/// 您的账号未登录或已过期
		/// </summary>
		/// <value>The acount no login.</value>
        public virtual string AcountNoLogin { get { return "귀하의 계정이 등록되지 않았거나 만료되었습니다!"; } }
		/// <summary>
		/// 您的账号已在其它地方登录
		/// </summary>
		/// <value>The acount logined.</value>
        public virtual string AcountLogined { get { return "귀하의 계정이 다른 등록되었습니다!"; } }
		/// <summary>
		/// 加载数据失败
		/// </summary>
		/// <value>The load data error.</value>
        public virtual string LoadDataError { get { return "데이터를로드하지 못했습니다!"; } }
		/// <summary>
		/// 充值失败
		/// </summary>
		/// <value>The app store pay error.</value>
        public virtual string AppStorePayError { get { return "실패를 충전!"; } }
		/// <summary>
		/// 获取受权失败
		/// </summary>
		/// <value>The get access failure.</value>
        public string GetAccessFailure
        {
            get { return "변호사 확보 실패!"; }
        }
    }
}