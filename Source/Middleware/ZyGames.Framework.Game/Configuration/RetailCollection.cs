using System.Configuration;

namespace ZyGames.Framework.Game.Configuration
{
    /// <summary>
    /// 渠道集合节点属性
    /// </summary>
    public class RetailCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RetailElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RetailElement)element).Id;
        }

        public RetailElement this[int index]
        {
            get
            {
                return (RetailElement)base.BaseGet(index);
            }
        }

        public new RetailElement this[string key]
        {
            get
            {
                return (RetailElement)base.BaseGet(key);
            }
        }
    }
}
