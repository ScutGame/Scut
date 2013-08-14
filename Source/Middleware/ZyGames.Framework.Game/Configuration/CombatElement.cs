using System.Configuration;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 战斗配置
    /// </summary>
    public class CombatElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return this["type"] as string; }
            set { this["type"] = value; }
        }

    }
}
