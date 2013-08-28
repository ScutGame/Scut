using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    [ConfigurationCollection(typeof(Channel10086Element), AddItemName = "channel")]
    public class Channel10086Collection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Channel10086Element();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Channel10086Element)element).Name;
        }

        public Channel10086Element this[int index]
        {
            get
            {
                return (Channel10086Element)base.BaseGet(index);
            }
        }

        public new Channel10086Element this[string key]
        {
            get
            {
                return (Channel10086Element)base.BaseGet(key);
            }
        }
    }
}
