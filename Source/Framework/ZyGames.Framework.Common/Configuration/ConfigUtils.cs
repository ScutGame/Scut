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
using System.Collections.Specialized;
using System.Configuration;
using ZyGames.Framework.Common.Security;

namespace ZyGames.Framework.Common.Configuration
{
    /// <summary>
    /// 操作Web.config文档
    /// </summary>
    public class ConfigUtils
    {
        /// <summary>
        /// 
        /// </summary>
        public static NameValueCollection SettingsCollection
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }
        private ConfigUtils()
        {
        }

        /// <summary>
        /// 读取Config配置文件的Add结点键int配置值
        /// </summary>
        /// <param name="key">键值Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetSetting(string key, int defaultValue)
        {
            int result = defaultValue;
            try
            {
                object obj = SettingsCollection[key];
                result = obj == null ? defaultValue : obj.ToInt();
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 读取Config配置文件的Add结点键decimal配置值
        /// </summary>
        /// <param name="key">键值Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal GetSetting(string key, decimal defaultValue)
        {
            decimal result = defaultValue;
            try
            {
                object obj = SettingsCollection[key];
                result = obj == null ? defaultValue : obj.ToDecimal();
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 读取Config配置文件的Add结点键bool配置值
        /// </summary>
        /// <param name="key">键值Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetSetting(string key, bool defaultValue)
        {
            bool result = defaultValue;
            try
            {
                object obj = SettingsCollection[key];
                result = obj == null ? defaultValue : obj.ToBool();
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 读取Config配置文件的Add结点键配置值
        /// </summary>
        /// <param name="key">键值Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetSetting(string key, string defaultValue)
        {
            string result;
            try
            {
                object obj = SettingsCollection[key];
                result = ((obj == null) ? defaultValue.ToNotNullString() : ((string)obj));
            }
            catch
            {
                result = defaultValue.ToNotNullString();
            }
            return result;
        }

        /// <summary>
        /// 读取Config配置文件的Add结点键值
        /// </summary>
        /// <param name="key">键值Key</param>
        /// <returns></returns>
        public static string GetSetting(string key)
        {
            return GetSetting(key, "");
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="connName">键值Key</param>
        /// <param name="mKey">解密密钥</param>
        /// <returns></returns>
        public static string GetConnectionString(string connName, string mKey)
        {
            string result;
            try
            {
                string text = GetSetting(connName);
                if (!mKey.IsEmpty() && !text.IsEmpty())
                {
                    text = CryptoHelper.DES_Decrypt(text, mKey);
                }
                else if (!text.IsEmpty())
                {
                    text = CryptoHelper.DES_Decrypt(text);
                }
                result = text;
            }
            catch
            {
                result = "";
            }
            return result;
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string connName)
        {
            return GetConnectionString(connName, string.Empty);
        }
    }
}