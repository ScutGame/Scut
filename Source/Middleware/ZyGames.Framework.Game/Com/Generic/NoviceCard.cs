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
using ZyGames.Framework.Common;

namespace ZyGames.Framework.Game.Com.Generic
{
    /// <summary>
    /// 新手卡
    /// </summary>
    public abstract class NoviceCard
    {
        protected int CurrUserId;
        protected int CardLength = 10;

        protected NoviceCard(int userId)
        {
            CurrUserId = userId;
        }

        /// <summary>
        /// 检查是否生成新手卡
        /// </summary>
        public void CheckGenerateCard()
        {
            if (HasGenerateID)
            {
                DoGenerate(GenerateID());
            }
        }

        protected abstract void DoGenerate(string generateID);

        protected abstract bool HasGenerateID { get; }

        /// <summary>
        /// 生成卡号
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateID()
        {
            return "C" + CurrUserId.ToString().PadLeft(CardLength, '0');
        }

        /// <summary>
        /// 激活
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public bool Activate(string cardId)
        {
            int cardUserId;
            if (!TryGetCardUserId(ref cardId, out cardUserId)) return false;
            return DoActivateCard(cardId, cardUserId);
        }

        private bool TryGetCardUserId(ref string cardId, out int cardUserId)
        {
            cardUserId = 0;
            cardId = cardId ?? "";
            cardId = cardId.Trim().Replace('c', 'C');
            if (string.IsNullOrEmpty(cardId) ||
                !cardId.StartsWith("C") ||
                !int.TryParse(cardId.Substring(1), out cardUserId))
            {
                return false;
            }
            return true;
        }

        protected abstract bool DoActivateCard(string cardId, int cardUserId);

        /// <summary>
        /// 领取奖励
        /// </summary>
        public bool CheckLvPrize(short userLv, string cardId)
        {
            int cardUserId;
            if (!TryGetCardUserId(ref cardId, out cardUserId)) return false;

            var prizeLvs = PackagePrizeLv;
            foreach (var item in prizeLvs)
            {
                short lv = item.ToShort();
                if (HasPrize(userLv, lv))
                {
                    DoPrize(cardId, cardUserId, lv);
                }
            }
            return true;
        }

        protected abstract void DoPrize(string cardId, int cardUserId, short lv);

        protected abstract bool HasPrize(short userLv, short prizeLv);

        protected abstract string[] PackagePrizeLv { get; }
    }
}