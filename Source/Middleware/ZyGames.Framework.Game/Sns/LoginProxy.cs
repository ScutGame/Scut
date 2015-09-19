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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Reflect;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 登录代理
    /// </summary>
    public class LoginProxy
    {
        private const string DefaultArgs = "Pid,Pwd,DeviceID";

        private LoginProxy()
        {
        }

        /// <summary>
        /// Gets the login.
        /// </summary>
        /// <returns>The login.</returns>
        public static ILogin GetLogin(ActionGetter httpGet, string retaiId)
        {
            return GetLogin(retaiId, httpGet);
        }

        /// <summary>
        /// Gets the login.
        /// </summary>
        /// <param name="retaiId"></param>
        /// <param name="obj">sdk json object of request or ActionGetter object</param>
        /// <returns></returns>
        public static ILogin GetLogin(string retaiId, object obj)
        {
            if (string.IsNullOrEmpty(retaiId))
            {
                return null;
            }
            object[] args = new object[0];
            string typeName = string.Format("{0}.Sns.Login36you,{0}", "ZyGames.Framework.Game");

            bool hasRetail = false;
            if (ZyGameBaseConfigManager.GameSetting.HasSetting)
            {
                var loginSetting = ZyGameBaseConfigManager.GameSetting.GetLoginSetting(retaiId);
                if (loginSetting != null)
                {
                    typeName = loginSetting.TypeName.Contains(",")
                        ? loginSetting.TypeName
                        : string.Format("{0}.Sns.{1},{0}", "ZyGames.Framework.Game", loginSetting.TypeName);
                    args = GetArgs(loginSetting.TypeArgs, obj);
                    hasRetail = true;
                }
            }
            else
            {
                var loginSection = ZyGameBaseConfigManager.GetLogin();
                if (loginSection != null)
                {
                    var retail = loginSection.RetailList[retaiId];
                    if (retail != null)
                    {
                        typeName = retail.TypeName.Contains(",")
                            ? retail.TypeName
                            : string.Format("{0}.Sns.{1},{0}", "ZyGames.Framework.Game", retail.TypeName);
                        args = GetArgs(retail.Args, obj);
                        hasRetail = true;
                    }
                }
            }
            if (!hasRetail)
            {
                args = GetArgs(DefaultArgs, obj);
            }

            var type = Type.GetType(typeName, false, true);
            if (type == null)
            {
                return null;
            }
            return type.CreateInstance<ILogin>(args);
        }

        private static object[] GetArgs(string argsStr, object obj)
        {
            var args = new List<object>();
            string[] paramList = argsStr.Split(',');
            object paramVal = null;
            foreach (string param in paramList)
            {
                var getter = obj as ActionGetter;
                if (getter != null)
                {
                    paramVal = getter.GetString(param);
                }
                else if (obj != null)
                {
                    paramVal = ObjectAccessor.Create(obj, true)[param];
                }
                args.Add(paramVal);
            }
            return args.ToArray();
        }
    }
}