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
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Pay.Section
{
	/// <summary>
	/// App store pay element.
	/// </summary>
    public class AppStorePayElement : ConfigurationElement
    {
		/// <summary>
		/// Gets or sets the product identifier.
		/// </summary>
		/// <value>The product identifier.</value>
        [ConfigurationProperty("productId", IsRequired = true, IsKey = true)]
        public string ProductId
        {
            get { return this["productId"] as string; }
            set { this["productId"] = value; }
        }
		/// <summary>
		/// Gets or sets the type of the mobile.
		/// </summary>
		/// <value>The type of the mobile.</value>
        [ConfigurationProperty("mobileType")]
        public MobileType MobileType
        {
            get { return this["mobileType"].ToEnum<MobileType>(); }
            set { this["mobileType"] = value; }
        }
		/// <summary>
		/// Gets or sets the dollar.
		/// </summary>
		/// <value>The dollar.</value>
        [ConfigurationProperty("dollar", IsRequired = true)]
        public decimal Dollar
        {
            get { return this["dollar"].ToDecimal(); }
            set { this["dollar"] = value; }
        }
		/// <summary>
		/// Gets or sets the currency.
		/// </summary>
		/// <value>The currency.</value>
        [ConfigurationProperty("currency", IsRequired = true)]
        public int Currency
        {
            get { return this["currency"].ToInt(); }
            set { this["currency"] = value; }
        }
		/// <summary>
		/// Gets or sets the silver piece.
		/// </summary>
		/// <value>The silver piece.</value>
        [ConfigurationProperty("silverPiece", IsRequired = true)]
        public int SilverPiece
        {
            get { return this["silverPiece"].ToInt(); }
            set { this["silverPiece"] = value; }
        }
    }
}