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
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 交易出错代码
    /// </summary>
    public enum TradeErrorCode
    {
		/// <summary>
		/// The sucess.
		/// </summary>
        Sucess = 0,
		/// <summary>
		/// The fail.
		/// </summary>
        Fail = 1000,
		/// <summary>
		/// The no godds.
		/// </summary>
        NoGodds,
		/// <summary>
		/// The trade timeout.
		/// </summary>
        TradeTimeout,
		/// <summary>
		/// The amount not enough.
		/// </summary>
        AmountNotEnough,
        /// <summary>
        /// 库存
        /// </summary>
        QuantityNotEnough,
		/// <summary>
		/// The limt number.
		/// </summary>
        LimtNumber
    }

    /// <summary>
    /// 交易策略
    /// </summary>
    public interface ITrade
    {
		/// <summary>
		/// Tries the enter traded.
		/// </summary>
		/// <returns><c>true</c>, if enter traded was tryed, <c>false</c> otherwise.</returns>
		/// <param name="goods">Goods.</param>
        bool TryEnterTraded(GoodsData goods);
        /// <summary>
        /// Exits the traded.
        /// </summary>
        void ExitTraded();
    }
}