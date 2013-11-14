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
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Pay.Section
{
	/// <summary>
	/// App store pay collection.
	/// </summary>
    [ConfigurationCollection(typeof(AppStorePayElement), AddItemName = "rate")]
    public class AppStorePayCollection : ConfigurationElementCollection
    {
		/// <summary>
		/// Creates the new element.
		/// </summary>
		/// <returns>The new element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppStorePayElement();
        }
		/// <summary>
		/// Gets the element key.
		/// </summary>
		/// <returns>The element key.</returns>
		/// <param name="element">Element.</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppStorePayElement)element).ProductId;
        }
		/// <summary>
		/// Gets the <see cref="ZyGames.Framework.Game.Pay.Section.AppStorePayCollection"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
        public AppStorePayElement this[int index]
        {
            get
            {
                return (AppStorePayElement)base.BaseGet(index);
            }
        }
		/// <summary>
		/// Gets the <see cref="ZyGames.Framework.Game.Pay.Section.AppStorePayCollection"/> with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
        public new AppStorePayElement this[string key]
        {
            get
            {
                return (AppStorePayElement)base.BaseGet(key);
            }
        }
    }
}