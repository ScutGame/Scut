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
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class EntitySection : ConfigSection
    {
        /// <summary>
        /// 
        /// </summary>
        public EntitySection()
        {
            LogTableNameFormat = ConfigUtils.GetSetting("Log.TableName.Format", "log_$date{0}");
            LogPriorBuildMonth = ConfigUtils.GetSetting("Log.PriorBuild.Month", 2);
            EnableModifyTimeField = ConfigUtils.GetSetting("Schema.EnableModifyTimeField", false);
        }

        /// <summary>
        /// Log table name format string.
        /// </summary>
        public string LogTableNameFormat { get; set; }

        /// <summary>
        /// prior build month table, default:3
        /// </summary>
        public int LogPriorBuildMonth { get; set; }

        /// <summary>
        /// enable field of ModifyTime
        /// </summary>
        public bool EnableModifyTimeField { get; set; }
    }
}
