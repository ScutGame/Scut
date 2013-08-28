using ZyGames.Framework.Game.Model;

namespace ZyGames.Framework.Game.Combat
{
    /// <summary>
    /// 一对一战斗接口
    /// </summary>
    public interface ISingleCombat
    {
        /// <summary>
        /// 加入攻击方阵列
        /// </summary>
        /// <param name="EmbattleQueue"></param>
        void SetAttack(EmbattleQueue combatGrid);

        /// <summary>
        /// 加入防守方阵列
        /// </summary>
        /// <param name="EmbattleQueue"></param>
        void SetDefend(EmbattleQueue combatGrid);

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
