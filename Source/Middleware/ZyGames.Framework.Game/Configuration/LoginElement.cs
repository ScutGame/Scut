using System.Configuration;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 登录节点元素
    /// </summary>
    public class LoginElement : ConfigurationElement
    {
        [ConfigurationProperty("retailList", IsRequired = true)]
        public RetailCollection RetailList
        {
            get { return this["retailList"] as RetailCollection; }
            set { this["retail"] = value; }
        }

        [ConfigurationProperty("defaultType")]
        public string DefaultTypeName
        {
            get { return this["defaultType"] as string; }
            set { this["defaultType"] = value; }
        }
    }


}
