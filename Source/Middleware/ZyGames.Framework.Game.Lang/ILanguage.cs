namespace ZyGames.Framework.Game.Lang
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILanguage
    {
        /// <summary>
        /// 
        /// </summary>
        int ErrorCode { get; }
        /// <summary>
        /// 
        /// </summary>
        int TimeoutCode { get; }
        /// <summary>
        /// 踢出
        /// </summary>
        int KickedOutCode { get; }
        /// <summary>
        /// 验证参数错误代码
        /// </summary>
        int ValidateCode { get; }
        /// <summary>
        /// 验证参数错误提示
        /// </summary>
        string ValidateError { get; }
        /// <summary>
        /// 系统繁忙中
        /// </summary>
        string ServerBusy { get; }
        /// <summary>
        /// 请求参数错误
        /// </summary>
        string UrlElement { get; }

        /// <summary>
        /// 参数名:{0}不存在
        /// </summary>
        string UrlNoParam { get; }
        /// <summary>
        /// 参数名:{0}超出范围[{1}-{2}]
        /// </summary>
        string UrlParamOutRange { get; }

        /// <summary>
        /// 服务器正在重启中，请稍候...
        /// </summary>
        string ServerLoading { get; }

        /// <summary>
        /// 您输入的账号或密码不正确
        /// </summary>
        string PasswordError { get; }

        /// <summary>
        /// 加载数据失败
        /// </summary>
        string LoadDataError { get; }
        
        /// <summary>
        /// 该账号已被封禁
        /// </summary>
        string AcountIsLocked { get; }

        /// <summary>
        /// 您的账号未登录或已过期
        /// </summary>
        string AcountNoLogin { get; }

        /// <summary>
        /// 您的账号已在其它地方登录
        /// </summary>
        string AcountLogined { get; }

        /// <summary>
        /// 充值失败
        /// </summary>
        string AppStorePayError { get; }
        /// <summary>
        /// 获取受权失败
        /// </summary>
        string GetAccessFailure { get; }
    }
}
