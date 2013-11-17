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
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Framework.Game.Pay
{
    internal class ConfigManger
    {
        private static string _connectionString = string.Empty;

        /// <summary>
        /// 连接的数据驱动提供类型，MSSQL,MYSQL
        /// </summary>
        public static string ConnectionProviderType
        {
            get { return ConfigUtils.GetSetting("PayDB_ProviderType"); }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static string connectionString
        {
            get
            {

                if (_connectionString == string.Empty)
                {
                    string cString = ConfigUtils.GetSetting("PayDB_ConnectionString");
                    string dataSource = string.Empty;
                    string userInfo = string.Empty;
                    try
                    {
                        dataSource = ConfigUtils.GetSetting("PayDB_Server");
                        userInfo = ConfigUtils.GetSetting("PayDB_Acount");
                        if (!string.IsNullOrEmpty(userInfo))
                        {
                            userInfo = CryptoHelper.DES_Decrypt(userInfo, GameEnvironment.ProductDesEnKey);
                        }
                    }
                    catch
                    {
                    }
                    if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(userInfo))
                    {
                        _connectionString = string.Format(cString, dataSource, userInfo);
                    }
                }
                return _connectionString;
            }
        }



    }
}