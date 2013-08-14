using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    [ConfigurationCollection(typeof(Channel91Element), AddItemName = "channel")]
    public class Channel91Collection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Channel91Element();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Channel91Element)element).Name;
        }

        public Channel91Element this[int index]
        {
            get
            {
                return (Channel91Element)base.BaseGet(index);
            }
        }

        public new Channel91Element this[string key]
        {
            get
            {
                return (Channel91Element)base.BaseGet(key);
            }
        }
    }
}
