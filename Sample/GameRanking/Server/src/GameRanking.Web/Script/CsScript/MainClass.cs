using System;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Script;

namespace Game.Script
{
    public class MainClass : IMainScript
    {
        public void Start(string[] args)
        {
            ActionFactory.SetActionIgnoreAuthorize(1000, 1001);
        }

        public void Stop()
        {
        }
    }
}