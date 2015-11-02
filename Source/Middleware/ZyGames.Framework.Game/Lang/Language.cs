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
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Config;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Lang
{
    /// <summary>
    /// Game language setting.
    /// </summary>
    public class Language
    {
        private static readonly object syncLock = new object();
        private static dynamic _instance;

        static Language()
        {
            _instance = new Language();
        }

        /// <summary>
        /// Get single language.
        /// </summary>
        public static dynamic Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            SetLang();
                        }
                        //By Seamoon if null then use default lang
                        if (_instance == null)
                        {
                            _instance = new Language();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Set language object.
        /// </summary>
        public static void SetLang()
        {
            SetLang(null);
        }

        /// <summary>
        /// Reset lang.
        /// </summary>
        public static void Reset()
        {
            lock (syncLock)
            {
                _instance = null;
            }
        }

        /// <summary>
        /// Set language object.
        /// </summary>
        private static void SetLang(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                typeName = ConfigManager.Configger.GetFirstOrAddConfig<AppServerSection>().LanguageTypeName;
            }

            var obj = ScriptEngines.ExecuteCSharp(typeName);
            if (obj != null)
            {
                _instance = obj;
                return;
            }
            //get default lang
            typeName = ConfigManager.Configger.GetFirstOrAddConfig<AppServerSection>().LanguageTypeName;
            var type = Type.GetType(typeName, false, false);
            if (type != null)
            {
                _instance = type.CreateInstance();
            }
            else
            {
                TraceLog.WriteWarn("Can not find the corresponding language configuration,typeName:{0}", typeName);//By Seamoon
            }
        }

        /// <summary>
        /// The instance
        /// </summary>
        protected Language()
        {
        }

        /// <summary>
        /// The error code.
        /// </summary>
        public int ErrorCode = 10000;

        /// <summary>
        /// The no login or timeout code
        /// </summary>
        public int TimeoutCode = 10001;

        /// <summary>
        /// Was kicked out of the server error code
        /// </summary>
        /// <value>The kicked out code.</value>
        public int KickedOutCode = 10002;

        /// <summary>
        /// Request param error code
        /// </summary>
        /// <value>The validate code.</value>
        public int ValidateCode = 10003;

        /// <summary>
        ///  Duplicate of error code
        /// </summary>
        /// <value>The kicked out code.</value>
        public int DuplicateCode = 10004;

        /// <summary>
        ///  Maintain of error code
        /// </summary>
        /// <value>The kicked out code.</value>
        public int MaintainCode = 10005;
        /// <summary>
        ///  lock time of error code
        /// </summary>
        /// <value>The kicked out code.</value>
        public int LockTimeoutCode = 10006;

        /// <summary>
        /// 验签出错
        /// </summary>
        public string SignError = "Sign error.";

        /// <summary>
        /// validate error
        /// </summary>
        public string ValidateError = "Request's param validate error.";
        /// <summary>
        /// The system is busy
        /// </summary>
        public string ServerBusy = "The server is busy.";

        /// <summary>
        /// param error
        /// </summary>
        public string UrlElement = "Request's param error.";

        /// <summary>
        /// 参数名:{0}不存在
        /// </summary>
        public string UrlNoParam = "Parameter name: {0} does not exist.";
        /// <summary>
        /// 参数名:{0}超出范围[{1}-{2}]
        /// </summary>
        public string UrlParamOutRange = "Parameter name: {0} is out of range [{1} - {2}]";

        /// <summary>
        /// 服务器正在维护
        /// </summary>
        public string ServerMaintain = "Server is being updated.";

        /// <summary>
        /// 服务器正在重启中，请稍候...
        /// </summary>
        public string ServerLoading = "The server is restarted, please wait...";

        /// <summary>
        /// 请求超时
        /// </summary>
        public string RequestTimeout = "Request too often, please rest.";
        /// <summary>
        /// 您输入的账号或密码不正确
        /// </summary>
        public string PasswordError = "You enter the account or password is error.";

        /// <summary>
        /// 加载数据失败
        /// </summary>
        public string LoadDataError = "Server data failed to load.";

        /// <summary>
        /// 该账号已被封禁
        /// </summary>
        public string AcountIsLocked = "The account has been locked.";

        /// <summary>
        /// 您的账号未登录或已过期
        /// </summary>
        public string AcountNoLogin = "The account is not registered or has expired.";

        /// <summary>
        /// 您的账号已在其它地方登录
        /// </summary>
        public string AcountLogined = "The account has been registered elsewhere.";

        /// <summary>
        /// 充值失败
        /// </summary>
        public string AppStorePayError = "Pay error.";
        /// <summary>
        /// 获取受权失败
        /// </summary>
        public string GetAccessFailure = "Get token failed.";

    }
}