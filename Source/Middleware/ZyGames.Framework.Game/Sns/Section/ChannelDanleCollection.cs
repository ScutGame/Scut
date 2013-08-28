using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    [ConfigurationCollection(typeof(ChannelDanleElement), AddItemName = "channel")]
    public class ChannelDanleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ChannelDanleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ChannelDanleElement)element).Name;
        }

        public ChannelDanleElement this[int index]
        {
            get
            {
                return (ChannelDanleElement)base.BaseGet(index);
            }
        }

        public new ChannelDanleElement this[string key]
        {
            get
            {
                return (ChannelDanleElement)base.BaseGet(key);
            }
        }
    }
}
