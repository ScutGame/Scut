using System.Configuration;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 游戏自定义配置集合
    /// zyGameBaseBll
    /// </summary>
    public class ZyGameBaseBllSection : ConfigurationSection
    {
        [ConfigurationProperty("login", IsRequired = true)]
        public LoginElement Login
        {
            get { return this["login"] as LoginElement; }
            set { this["login"] = value; }
        }

        [ConfigurationProperty("combat", IsRequired = false)]
        public CombatElement Combat
        {
            get { return this["combat"] as CombatElement; }
            set { this["combat"] = value; }
        }
    }

}
