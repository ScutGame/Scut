using ZyGames.Framework.Game.Model;

namespace ZyGames.Framework.Game.Combat
{
    /// <summary>
    /// 多对多战斗
    /// </summary>
    public interface IMultiCombat
    {
        /// <summary>
        /// 加入攻击方阵列
        /// </summary>
        /// <param name="EmbattleQueue"></param>
        void AppendAttack(EmbattleQueue combatGrid);

        /// <summary>
        /// 加入防守方阵列
        /// </summary>
        /// <param name="EmbattleQueue"></param>
        void AppendDefend(EmbattleQueue combatGrid);

        /// <summary>
        /// 交战
        /// </summary>
        /// <returns>返回胜利或失败</returns>
        bool Doing();

        /// <summary>
        /// 交战过程
        /// </summary>
        /// <returns></returns>
        object GetProcessResult();

        /// <summary>
        /// 回合数
        /// </summary>
        int BoutNum { get; }
    }
}
