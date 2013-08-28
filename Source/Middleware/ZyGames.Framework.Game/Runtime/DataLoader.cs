using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Runtime
{
    public abstract class DataLoader
    {
        public abstract string LoadTypeName
        {
            get;
        }

        protected abstract bool IsExistData();

        public abstract bool Load();
    }
}
