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

namespace ZyGames.Framework.Common.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class IConfigData
    {

        /// <summary>
        /// 
        /// </summary>
        public abstract IList<ConfigNode> GetNodes();

        /// <summary>
        /// 
        /// </summary>
        public abstract string[] GetKeys();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetValue(string key, int defaultValue = 0)
        {
            return GetValue(key, defaultValue.ToString()).ToInt();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public decimal GetValue(string key, decimal defaultValue = 0)
        {
            return GetValue(key, defaultValue.ToString()).ToDecimal();
        }
        /// <summary>
        /// 
        /// </summary>
        public abstract string GetValue(string key, string defaultValue);

        /// <summary>
        /// 
        /// </summary>
        public abstract string[] GetValues(string key);

        /// <summary>
        /// 
        /// </summary>
        public abstract ConfigNode GetNode(string key);

        /// <summary>
        /// 
        /// </summary>
        public abstract void AddNode(ConfigNode node);

        /// <summary>
        /// 
        /// </summary>
        public abstract bool SetValue(string key, string value);

        /// <summary>
        /// 
        /// </summary>
        public abstract bool SetNode(ConfigNode node);

        /// <summary>
        /// 
        /// </summary>
        public abstract bool RemoveNode(ConfigNode node);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Clear();
    }
}
