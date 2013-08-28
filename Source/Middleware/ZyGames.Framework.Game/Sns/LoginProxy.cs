using System;
using System.Collections.Generic;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 登录代理
    /// </summary>
    public class LoginProxy
    {
        private const string defaultArgs = "Pid,Pwd,DeviceID";
        private HttpGet _httpGet;
        private string retailID = string.Empty;

        public LoginProxy(HttpGet httpGet)
        {
            this._httpGet = httpGet;
            if (_httpGet != null)
            {
                _httpGet.GetString("RetailID", ref retailID);
            }  
        }

        public ILogin GetLogin()
        {
            LoginElement loginSection = ZyGameBaseConfigManager.GetLogin();
            if (loginSection == null) return null;

            RetailElement retail = loginSection.RetailList[retailID];
            object[] args = new object[0];
            string typeName = string.Empty;
            if (retail != null)
            {
                typeName = retail.TypeName;
                args = GetArgs(retail.Args);
            }
            else
            {
                typeName = loginSection.DefaultTypeName;
                args = GetArgs(defaultArgs);
            }
            return (ILogin)Activator.CreateInstance(Type.GetType(typeName), args);
        }

        private object[] GetArgs(string argsStr)
        {
            List<object> args = new List<object>();
            string[] paramList = argsStr.Split(new char[] { ',' });
            foreach (string param in paramList)
            {
                string paramVal = string.Empty;
                if (_httpGet!=null)
                {
                    _httpGet.GetString(param, ref paramVal);
                }                
                args.Add(paramVal);
            }
            return args.ToArray();
        }
    }
}
