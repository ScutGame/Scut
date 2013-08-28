using System;

namespace ZyGames.Framework.Game.Lang
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseBIG5Language : ILanguage
    {
        public virtual int ErrorCode { get { return 10000; } }
        public virtual int TimeoutCode { get { return 10001; } }
        public virtual int KickedOutCode { get { return 10002; } }
        public int ValidateCode { get { return 10003; } }
        public virtual string ValidateError { get { return "文字中包含不當內容，請重新輸入!"; } }
        public virtual string ServerBusy { get { return "系統繁忙中!"; } }
        public virtual string UrlElement { get { return "請求參數錯誤!"; } }

        public virtual string UrlNoParam { get { return "參數名:{0}不存在"; } }
        public virtual string UrlParamOutRange { get { return "參數名:{0}超出範圍[{1}-{2}]"; } }

        public virtual string ServerLoading { get { return "服務器正在重啟中，請稍候..."; } }
        public string PasswordError { get { return "您輸入的帳號或密碼不正確!"; } }
        public string LoadDataError { get { return "載入數據失敗!"; } }
        public string AcountIsLocked { get { return "該賬號已被封禁，登錄失敗！"; } }
        public string AcountNoLogin { get { return "您的賬號未登錄或已過期！"; } }
        public string AcountLogined { get { return "您的賬號已在其它地方登錄！"; } }
        public string AppStorePayError { get { return "充值失敗!"; } }

        public string GetAccessFailure
        {
            get { return "獲取受權失敗!"; }
        }
    }
}