using System;
using ZyGames.Framework.Game.Configuration;

namespace ZyGames.Framework.Game.Combat
{
    /// <summary>
    /// 控制器工厂
    /// </summary>
    public static class CombatControllerFactory
    {
        private static object lockThis = new object();
        private static ICombatController controller = null;

        public static ICombatController Create()
        {
            if (controller == null)
            {
                lock (lockThis)
                {
                    if (controller == null)
                    {
                        controller = (ICombatController)Activator.CreateInstance(Type.GetType(ZyGameBaseConfigManager.GetCombat().TypeName));
                    }
                }
            }
            return controller;
        }
    }
}
