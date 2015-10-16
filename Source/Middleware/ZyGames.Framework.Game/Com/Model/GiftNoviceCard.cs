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
using ProtoBuf;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 媒体礼包新手卡
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class GiftNoviceCard : BaseEntity
    {
		/// <summary>
		/// Gets or sets the card no.
		/// </summary>
		/// <value>The card no.</value>
        public abstract string CardNo
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the type of the gift.
		/// </summary>
		/// <value>The type of the gift.</value>
        public abstract string GiftType
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
        public abstract int UserId
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the activate date.
		/// </summary>
		/// <value>The activate date.</value>
        public abstract DateTime ActivateDate
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is invalid.
		/// </summary>
		/// <value><c>true</c> if this instance is invalid; otherwise, <c>false</c>.</value>
        public abstract bool IsInvalid
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the create ip.
		/// </summary>
		/// <value>The create ip.</value>
        public abstract string CreateIp
        {
            get;
            set;
        }
		/// <summary>
		/// Gets or sets the create date.
		/// </summary>
		/// <value>The create date.</value>
        public abstract DateTime CreateDate
        {
            get;
            set;
        }
    }
}