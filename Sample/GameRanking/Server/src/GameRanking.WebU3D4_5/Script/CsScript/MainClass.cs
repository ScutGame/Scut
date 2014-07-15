using System;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Script;

namespace Game.Script
{
    public class MainClass : IMainScript
    {
        public void Start(string[] args)
        {
            GameEnvironment.Setting.ActionDispatcher = new CustomActionDispatcher();
            ActionFactory.SetActionIgnoreAuthorize(1000, 1001);
        }

        public void Stop()
        {
        }
    }
}