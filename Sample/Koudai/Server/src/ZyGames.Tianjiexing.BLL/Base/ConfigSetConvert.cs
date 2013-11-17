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
using ZyGames.Framework.Collection;


namespace ZyGames.Tianjiexing.BLL.Base
{
    /// <summary>
    /// 配置数据解析类
    /// </summary>
    public static class ConfigSetConvert
    {
        /// <summary>
        /// 解析KeyValue对字串，格式A=X,B=X
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> ParseKeyValue(string str)
        {
            List<KeyValuePair<string, string>> keyList = new List<KeyValuePair<string, string>>();
            str = str == null ? string.Empty : str;
            string[] tempList = str.Split(',');
            for (int i = 0; i < tempList.Length; i++)
            {
                string[] array =tempList[i].Split('=');
                if (array.Length != 2) continue;
                
                keyList.Add(new KeyValuePair<string, string>(array[0], array[1]));
            }

            return keyList;
        }
    }
}