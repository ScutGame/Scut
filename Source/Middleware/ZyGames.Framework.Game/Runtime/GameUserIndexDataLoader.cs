using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Runtime
{
    public class GameUserIndexDataLoader<T> : GameUserDataLoader<T> where T : BaseEntity, new()
    {
        public GameUserIndexDataLoader(bool isAuto, string personalId, string fieldName, int userId, int maxCount, int periodTime)
            : base(isAuto, personalId, fieldName, userId, maxCount, periodTime)
        {
        }

    }
}
