using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scut.Client.Net
{
    public class UserToken
    {
        public int GameType { get; set; }
        public int ServerID { get; set; }
        public string Sid { get; set; }
        public string Uid { get; set; }
        public string DeviceID { get; set; }
        public string Pid { get; set; }
        public string Pwd { get; set; }
        public int MobileType { get; set; }
        public string RetailID { get; set; }
        
        public object UserData { get; set; }
    }
}
