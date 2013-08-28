using System.Configuration;

namespace ZyGames.Framework.Game.Configuration
{
    public static class ZyGameBaseConfigManager
    {
        private static readonly object thisLock = new object();
        private const string KeyName = "zyGameBaseBll";
        private static ZyGameBaseBllSection zyGameBaseBll;

        private static ZyGameBaseBllSection BaseConfig
        {
            get
            {
                if (zyGameBaseBll == null)
                {
                    lock (thisLock)
                    {
                        if (zyGameBaseBll == null)
                        {
                            zyGameBaseBll = (ZyGameBaseBllSection) ConfigurationManager.GetSection(KeyName);
                        }
                    }
                }
                return zyGameBaseBll;
            }
        }

        public static LoginElement GetLogin()
        {
            return BaseConfig != null ? BaseConfig.Login : null;
        }

        public static CombatElement GetCombat()
        {
            return BaseConfig != null ? BaseConfig.Combat : null;
        }
    }
}
