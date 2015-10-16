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

namespace ZyGames.Framework.Game.Command
{
	/// <summary>
	/// Command collection.
	/// </summary>
    public class CommandCollection : ConfigurationElementCollection
    {
		/// <summary>
		/// Creates the new element.
		/// </summary>
		/// <returns>The new element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CommandElement();
        }
		/// <summary>
		/// Gets the element key.
		/// </summary>
		/// <returns>The element key.</returns>
		/// <param name="element">Element.</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommandElement)element).Cmd;
        }
		/// <summary>
		/// Gets the <see cref="ZyGames.Framework.Game.Command.CommandCollection"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
        public CommandElement this[int index]
        {
            get
            {
                return (CommandElement)base.BaseGet(index);
            }
        }
		/// <summary>
		/// Gets the <see cref="ZyGames.Framework.Game.Command.CommandCollection"/> with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
        public new CommandElement this[string key]
        {
            get
            {
                return (CommandElement)base.BaseGet(key);
            }
        }
    }
}