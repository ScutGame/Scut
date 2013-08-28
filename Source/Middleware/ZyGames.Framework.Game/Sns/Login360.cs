using System.Web;

namespace ZyGames.Framework.Game.Sns
{
    public class Login360 : ILogin
    {
        private string _retailID = string.Empty;
        private string _pid = string.Empty;

        public Login360(string retailID, string pid)
        {
            this._retailID = retailID;
            this._pid = pid;
        }

        #region ILogin 成员

        public string PassportID
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string SessionID
        {
            get;
            set;
        }

        protected static string GetSessionId()
        {
            string sessionId = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                sessionId = HttpContext.Current.Session.SessionID;
            }
            else
            {
                sessionId = System.Guid.NewGuid().ToString().Replace("-", string.Empty);
            }
            return sessionId;
        }

        public string GetRegPassport()
        {
            return this.PassportID;
        }

        public bool CheckLogin()
        {
            string[] arr = SnsManager.LoginByRetail(_retailID, _pid);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            this.SessionID = GetSessionId();
            return true;
        }

        #endregion
    }
}
