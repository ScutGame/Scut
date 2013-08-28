using System.Configuration;

namespace ZyGames.Framework.Game.Command
{
    public class GmSection : ConfigurationSection
    {
        [ConfigurationProperty("command", IsRequired = true)]
        public CommandCollection Command
        {
            get { return this["command"] as CommandCollection; }
            set { this["command"] = value; }
        }

    }
}
