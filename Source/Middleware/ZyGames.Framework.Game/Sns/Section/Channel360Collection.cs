using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    [ConfigurationCollection(typeof(Channel360Element), AddItemName = "channel")]
    public class Channel360Collection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Channel360Element();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Channel360Element)element).Name;
        }

        public Channel360Element this[int index]
        {
            get
            {
                return (Channel360Element)base.BaseGet(index);
            }
        }

        public new Channel360Element this[string key]
        {
            get
            {
                return (Channel360Element)base.BaseGet(key);
            }
        }
    }
}
