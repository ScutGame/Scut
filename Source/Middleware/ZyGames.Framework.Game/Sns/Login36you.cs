using System.Web;
using ZyGames.Framework.Common.Security;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 36you官网登录0000
    /// </summary>
    public class Login36you : ILogin
    {
        private const string DES_KEY = "5^1-34E!";

        public Login36you(string passportID, string password, string deviceID)
        {
            this.PassportID = passportID;
            this.DeviceID = deviceID;
            try
            {
                password = new DESAlgorithmNew().DecodePwd(password, "n7=7=7dk");
                this.Password = CryptoHelper.DES_Encrypt(password, DES_KEY);
            }
            catch { }
        }

        public string UserID
        {
            get;
            set;
        }

        public string PassportID
        {
            get;
            set;
        }


        public string Password
        {
            get;
            set;
        }

        public string DeviceID
        {
            get;
            set;
        }

        public string SessionID
        {
            get;
            set;
        }

        #region ILogin 成员

        public string GetRegPassport()
        {
            string[] userList = SnsManager.GetRegPassport(DeviceID);
            if (userList.Length == 2)
            {
                PassportID = userList[0];
                Password = CryptoHelper.DES_Decrypt(userList[1], DES_KEY);
            }
            return PassportID;
        }

        public bool CheckLogin()
        {
            //快速登录
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }
            UserID = SnsManager.LoginByDevice(PassportID, Password, DeviceID).ToString();

            if (!string.IsNullOrEmpty(UserID) && UserID != "0")
            {
                SessionID = GetSessionId();
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


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

    }
}
