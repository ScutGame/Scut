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
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Generic
{
    /// <summary>
    /// 渠道媒体礼包
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class ChannelGift
    {
        private static readonly object SyncRoot = new object();
		/// <summary>
		/// The length of the gift type.
		/// </summary>
        protected int GiftTypeLength = 3;
		/// <summary>
		/// The length of the card no minimum.
		/// </summary>
        protected int CardNoMinLength = 10;
		/// <summary>
		/// The invalid hour.
		/// </summary>
        protected int InvalidHour = 24;
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Com.Generic.ChannelGift"/> class.
		/// </summary>
        protected ChannelGift()
        {

        }
		/// <summary>
		/// Gets or sets the current card identifier.
		/// </summary>
		/// <value>The current card identifier.</value>
        protected abstract int CurrentCardId { get; set; }
		/// <summary>
		/// Gets the next card identifier.
		/// </summary>
		/// <value>The next card identifier.</value>
        protected int NextCardId
        {
            get
            {
                lock (SyncRoot)
                {
                    int cardId = CurrentCardId;
                    cardId++;
                    CurrentCardId = cardId;
                    return cardId;
                }
            }
        }

        /// <summary>
        /// 规则：礼包类型(3位) + CardId(最小4位) + 验证码(随机数3位)
        /// </summary>
        /// <param name="giftType"></param>
        /// <returns></returns>
        protected virtual string GetCardNo(string giftType)
        {
            if (string.IsNullOrEmpty(giftType) || giftType.Length != GiftTypeLength)
            {
                throw new ArgumentOutOfRangeException("giftType", string.Format("礼包类型\"{0}\"不符合设置的长度{1}", giftType, GiftTypeLength));
            }
            int randId = RandomUtils.GetRandom(1, 1000);
            return string.Format("{0}{1}{2}", giftType, NextCardId, randId.ToString().PadLeft(3, '0'));
        }

        /// <summary>
        /// 尝试取作废的卡
        /// </summary>
        /// <param name="createIp"></param>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        protected bool TryGetInvalidCardNo(string createIp, out string cardNo)
        {
            cardNo = null;
            var cardList = GetInvalidCard();
            foreach (var card in cardList)
            {
                if (card == null)
                {
                    continue;
                }
                if (card.IsInvalid)
                {
                    lock (card)
                    {
                        if (card.IsInvalid)
                        {
                            cardNo = card.CardNo;
                            card.UserId = 0;
                            card.CreateIp = createIp;
                            card.CreateDate = DateTime.Now;
                            card.IsInvalid = false;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否过期作废的卡（24小时过期）
        /// </summary>
        /// <returns></returns>
        public void CheckInvalidCardNo()
        {
            var cardList = GetNotActivatedCard();
            foreach (var giftCard in cardList)
            {
                if (!giftCard.IsInvalid && MathUtils.DiffDate(giftCard.CreateDate).TotalHours > InvalidHour)
                {
                    giftCard.IsInvalid = true;
                }
            }
        }
		/// <summary>
		/// Gets the invalid card.
		/// </summary>
		/// <returns>The invalid card.</returns>
        protected abstract GiftNoviceCard[] GetInvalidCard();
        /// <summary>
        /// 获得未激活的卡
        /// </summary>
        /// <returns></returns>
        protected abstract GiftNoviceCard[] GetNotActivatedCard();

        /// <summary>
        /// 生成媒体礼包卡
        /// </summary>
        /// <param name="createIp">访问的IP</param>
        /// <param name="giftType">礼包类型</param>
        /// <returns></returns>
        public string GenerateCardNo(string giftType, string createIp)
        {
            string cardNo;
            if (!TryGetInvalidCardNo(createIp, out cardNo))
            {
                cardNo = GetCardNo(giftType);
                AddGiftCard(giftType, createIp, cardNo);
            }
            return cardNo;
        }
		/// <summary>
		/// Adds the gift card.
		/// </summary>
		/// <param name="giftType">Gift type.</param>
		/// <param name="createIp">Create ip.</param>
		/// <param name="cardNo">Card no.</param>
        protected abstract void AddGiftCard(string giftType, string createIp, string cardNo);

        /// <summary>
        /// 激活
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardNo"></param>
        public bool Activate(int userId, string cardNo)
        {
            string giftType;
            if (!TryGetGiftType(cardNo, out giftType))
            {
                return false;
            }
            if (!HasActivated(userId, cardNo, giftType))
            {
                DoActivate(userId, cardNo, giftType);
                return true;
            }
            return false;
        }
		/// <summary>
		/// Dos the activate.
		/// </summary>
		/// <param name="userId">User identifier.</param>
		/// <param name="cardNo">Card no.</param>
		/// <param name="giftType">Gift type.</param>
        protected abstract void DoActivate(int userId, string cardNo, string giftType);

        /// <summary>
        /// 是否已激活
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardNo"></param>
        /// <param name="giftType"></param>
        /// <returns></returns>
        protected abstract bool HasActivated(int userId, string cardNo, string giftType);

        private bool TryGetGiftType(string cardNo, out string giftType)
        {
            giftType = null;
            if (!string.IsNullOrEmpty(cardNo) &&
                cardNo.Length > GiftTypeLength &&
                cardNo.Length >= CardNoMinLength)
            {
                giftType = cardNo.Substring(0, GiftTypeLength);
                return true;
            }
            return false;
        }

    }
}