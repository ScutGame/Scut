using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 签名配置
    /// </summary>
    public sealed class SignUtils
    {
        public const string DefaultKey = "44CAC8ED53714BF18D60C5C7B6296000";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PasswordMd5(string str)
        {
            string attachParam = str + DefaultKey;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(attachParam, "MD5");
        }
    }
}
