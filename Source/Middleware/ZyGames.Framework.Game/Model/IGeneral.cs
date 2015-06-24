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
namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// 将领接口
    /// </summary>
    public interface IGeneral
    {
		/// <summary>
		/// Gets the general I.
		/// </summary>
		/// <value>The general I.</value>
        int GeneralID { get; }
		/// <summary>
		/// Gets or sets the life number.
		/// </summary>
		/// <value>The life number.</value>
        int LifeNum { get; set; }
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
        int Position { get; set; }
		/// <summary>
		/// Gets or sets the replace position.
		/// </summary>
		/// <value>The replace position.</value>
        int ReplacePosition { get; set; }

        /// <summary>
        /// 等待的佣兵，有其它佣兵死后替补
        /// </summary>
        bool IsWait { get; set; }
    }
}