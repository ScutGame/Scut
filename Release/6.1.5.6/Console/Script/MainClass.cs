using System;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Runtime;

namespace GameServer.Script
{
    public class MainClass : GameSocketHost
    {
        public MainClass()
        {
        }
        
        protected override BaseUser GetUser(int userId)
        {
            return null;
        }

        protected override void OnStartAffer()
        {
            var setting = new EnvironmentSetting();
            GameEnvironment.Start(setting);
        }

        protected override void OnServiceStop()
        {
        }
    }
}