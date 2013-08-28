using System;

namespace ZyGames.Framework.Game.Lang
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseENLanguage : ILanguage
    {
        public virtual int ErrorCode { get { return 10000; } }
        public virtual int TimeoutCode { get { return 10001; } }
        public virtual int KickedOutCode { get { return 10002; } }
        public int ValidateCode { get { return 10003; } }
        public virtual string ValidateError { get { return "The text contains inappropriate content, please re-enter"; } }
        public virtual string ServerBusy { get { return "System is busy now!"; } }
        public virtual string UrlElement { get { return "Request's parameter is error!"; } }

        public virtual string UrlNoParam { get { return "Parameter name: {0} does not exist"; } }
        public virtual string UrlParamOutRange { get { return "Parameter name: {0} is out of range [{1} - {2}]"; } }

        public virtual string ServerLoading { get { return "The server is restarted, please wait..."; } }
        public string PasswordError { get { return "You enter the account or password is incorrect!"; } }
        public string AcountIsLocked { get { return "The account has been locked, Login failed!"; } }
        public string AcountNoLogin { get { return "Your account is not registered or has expired!"; } }
        public string AcountLogined { get { return "Your account has been registered elsewhere!"; } }
        public string LoadDataError { get { return "Data failed to load!"; } }
        public string AppStorePayError { get { return "Recharge failure!"; } }

        public string GetAccessFailure
        {
            get { return "Get attorney failed!"; }
        }
    }
}