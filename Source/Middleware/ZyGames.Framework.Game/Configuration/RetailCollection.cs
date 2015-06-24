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
    /// 渠道集合节点属性
    /// </summary>
    public class RetailCollection : ConfigurationElementCollection
    {
		/// <summary>
		/// Creates the new element.
		/// </summary>
		/// <returns>The new element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RetailElement();
        }
		/// <summary>
		/// Gets the element key.
		/// </summary>
		/// <returns>The element key.</returns>
		/// <param name="element">Element.</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RetailElement)element).Id;
        }
		/// <summary>
		/// Gets the <see cref="ZyGames.Framework.Game.Configuration.RetailCollection"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
        public RetailElement this[int index]
        {
            get
            {
                return (RetailElement)base.BaseGet(index);
            }
        }
		/// <summary>
		/// Gets the <see cref="ZyGames.Framework.Game.Configuration.RetailCollection"/> with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
        public new RetailElement this[string key]
        {
            get
            {
                return (RetailElement)base.BaseGet(key);
            }
        }
    }
}