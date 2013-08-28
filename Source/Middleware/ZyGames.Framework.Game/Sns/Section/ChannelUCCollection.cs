using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    [ConfigurationCollection(typeof(ChannelUCElement), AddItemName = "channel")]
    public class ChannelUCCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ChannelUCElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ChannelUCElement)element).Name;
        }

        public ChannelUCElement this[int index]
        {
            get
            {
                return (ChannelUCElement)base.BaseGet(index);
            }
        }

        public new ChannelUCElement this[string key]
        {
            get
            {
                return (ChannelUCElement)base.BaseGet(key);
            }
        }
    }
}
