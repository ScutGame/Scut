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
using System.Threading;
using ProtoBuf;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Game.Com.Model;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 单例交易
    /// </summary>
    [Serializable, ProtoContract]
    public class SingleTrade : ITrade
    {
        private GoodsData _goods;
        private readonly int _timeOut;
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Com.Mall.SingleTrade"/> class.
		/// </summary>
		/// <param name="timeOut">Time out.</param>
        public SingleTrade(int timeOut = 1000)
        {
            _timeOut = timeOut;
        }
		/// <summary>
		/// Tries the enter traded.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="goods">Goods.</param>
        public bool TryEnterTraded(GoodsData goods)
        {
            if (goods == null)
            {
                throw new ArgumentNullException("goods");
            }
            _goods = goods;
            return Monitor.TryEnter(_goods, _timeOut);
        }
		/// <summary>
		/// Exits the traded.
		/// </summary>
        public void ExitTraded()
        {
            Monitor.Exit(_goods);
        }

    }
}