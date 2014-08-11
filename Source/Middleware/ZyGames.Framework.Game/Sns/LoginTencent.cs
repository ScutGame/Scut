using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 腾讯0018
    /// </summary>
    public class LoginTencent : AbstractLogin
    {
        private string _retailID = string.Empty;
        private string _pid = string.Empty;
        private string _retailUser = string.Empty;
        private string AppId = string.Empty;
        private string AppKey = string.Empty;
        private string _code = string.Empty;
        private string Url = string.Empty;
        private string _aceessTokenUrl = string.Empty;
        private string _appSecret = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.LoginTencent"/> class.
        /// </summary>
        public LoginTencent(string retailID, string retailUser, string token)
        {
            this._retailID = retailID;
            this._retailUser = retailUser;
        }

        public override string GetRegPassport()
        {
            return this.PassportID;
        }

        public override bool CheckLogin()
        {
            string[] arr = SnsManager.LoginByRetail(_retailID, _retailUser);
            this.UserID = arr[0];
            this.PassportID = arr[1];
            QihooUserID = _retailUser;
            SessionID = GetSessionId();
            return true;
        }
    }
}
