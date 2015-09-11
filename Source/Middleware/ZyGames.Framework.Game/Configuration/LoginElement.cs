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
using System.Configuration;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 登录节点元素
    /// </summary>
    public class LoginElement : ConfigurationElement
    {
		/// <summary>
		/// Gets or sets the retail list.
		/// </summary>
		/// <value>The retail list.</value>
        [ConfigurationProperty("retailList", IsRequired = true)]
        public RetailCollection RetailList
        {
            get { return this["retailList"] as RetailCollection; }
            set { this["retail"] = value; }
        }
		/// <summary>
		/// Gets or sets the default name of the type.
		/// </summary>
		/// <value>The default name of the type.</value>
        [ConfigurationProperty("defaultType")]
        public string DefaultTypeName
        {
            get { return this["defaultType"] as string; }
            set { this["defaultType"] = value; }
        }
    }


}