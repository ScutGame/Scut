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
using System.Web.Caching;
using ProtoBuf;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Mall
{
    /// <summary>
    /// 商城工厂
    /// </summary>
    [Serializable, ProtoContract]
    public static class MallFactory
    {
        private static Dictionary<int, Merchant> _merchantList = new Dictionary<int, Merchant>();
		/// <summary>
		/// Registers the merchant.
		/// </summary>
		/// <param name="merchantId">Merchant identifier.</param>
		/// <param name="controller">Controller.</param>
        public static void RegisterMerchant(int merchantId, MallController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }
            Merchant merchant = new Merchant(merchantId, controller);
            merchant.InitializeGoods();
            if (!_merchantList.ContainsKey(merchantId))
            {
                _merchantList.Add(merchantId, merchant);
            }
        }
		/// <summary>
		/// Gets the merchant.
		/// </summary>
		/// <returns>The merchant.</returns>
		/// <param name="merchantId">Merchant identifier.</param>
        public static Merchant GetMerchant(int merchantId)
        {
            return _merchantList.ContainsKey(merchantId) ? _merchantList[merchantId] : null;
        }
		/// <summary>
		/// Releases all resource used by the object.
		/// </summary>
        public static void Dispose()
        {
            _merchantList.Clear();
            _merchantList = null;
        }
    }
}