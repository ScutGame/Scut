/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
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