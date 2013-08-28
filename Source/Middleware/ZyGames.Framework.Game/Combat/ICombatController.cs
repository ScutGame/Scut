

namespace ZyGames.Framework.Game.Combat
{
    /// <summary>
    /// 战斗控制器接口
    /// </summary>
    public interface ICombatController
    {
        ISingleCombat GetSingleCombat(object args);

        IManyOneCombat GetManyOneCombat(object args);

        IMultiCombat GetMultiCombat(object args);
    }
}
