using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns
{
    public abstract class AbstractLogin : ILogin
    {
        public string PassportID { get; protected set; }

        public string UserID
        {
            get;
            protected set; 
        }

        public string Password
        {
            get;
            protected set; 
        }

        public string SessionID
        {
            get;
            protected set; 
        }

        public abstract string GetRegPassport();
        public abstract bool CheckLogin();


        public string AccessToken
        {
            get;
            protected set; 
        }

        public string RefeshToken
        {
            get;
            protected set; 
        }

        public string QihooUserID
        {
            get;
            protected set; 
        }

        public int ExpiresIn
        {
            get;
            protected set; 
        }

        public string Scope
        {
            get;
            protected set; 
        }

       
    }
}
