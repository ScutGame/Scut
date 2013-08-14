using System.Configuration;

namespace ZyGames.Framework.Game.Command
{
    public class CommandCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CommandElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CommandElement)element).Cmd;
        }

        public CommandElement this[int index]
        {
            get
            {
                return (CommandElement)base.BaseGet(index);
            }
        }

        public new CommandElement this[string key]
        {
            get
            {
                return (CommandElement)base.BaseGet(key);
            }
        }
    }
}