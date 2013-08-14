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
