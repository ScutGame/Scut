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
using System.Configuration;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Tianjiexing.Model
{
    public class DbConfig
    {
        public const string Config = "TianjiexingConfig";
        public const string Data = "TianjiexingData";
        public const string Log = "TianjiexingLog";
        public const int GlobalPeriodTime = 0;
        //��������һ��
        public const int PeriodTime = 86400; 
        /// <summary>
        /// 
        /// </summary>
        public const string PersonalName = "UserId";
        
        public static string ConfigConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[Config].ConnectionString;
            }
        }

        public static string DataConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[Data].ConnectionString;
            }
        }

        public static string LogConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[Log].ConnectionString;
            }
        }
    }
}