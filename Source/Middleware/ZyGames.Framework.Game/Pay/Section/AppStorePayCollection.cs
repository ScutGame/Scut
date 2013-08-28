using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Pay.Section
{
    [ConfigurationCollection(typeof(AppStorePayElement), AddItemName = "rate")]
    public class AppStorePayCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppStorePayElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppStorePayElement)element).ProductId;
        }

        public AppStorePayElement this[int index]
        {
            get
            {
                return (AppStorePayElement)base.BaseGet(index);
            }
        }

        public new AppStorePayElement this[string key]
        {
            get
            {
                return (AppStorePayElement)base.BaseGet(key);
            }
        }
    }
}
