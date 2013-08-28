using System.Configuration;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 渠道节点属性
    /// </summary>
    public class RetailElement : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return this["id"] as string; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName
        {
            get { return this["type"] as string; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("args", IsRequired = false)]
        public string Args
        {
            get { return this["args"] as string; }
            set { this["args"] = value; }
        }
    }
}
