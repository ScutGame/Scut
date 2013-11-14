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
    /// 渠道节点属性
    /// </summary>
    public class RetailElement : ConfigurationElement
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return this["id"] as string; }
            set { this["id"] = value; }
        }
		/// <summary>
		/// Gets or sets the name of the type.
		/// </summary>
		/// <value>The name of the type.</value>
        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return this["type"] as string; }
            set { this["type"] = value; }
        }
		/// <summary>
		/// Gets or sets the arguments.
		/// </summary>
		/// <value>The arguments.</value>
        [ConfigurationProperty("args", IsRequired = false)]
        public string Args
        {
            get { return this["args"] as string; }
            set { this["args"] = value; }
        }
    }
}