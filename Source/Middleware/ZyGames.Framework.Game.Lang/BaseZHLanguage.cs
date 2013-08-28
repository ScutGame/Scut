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
