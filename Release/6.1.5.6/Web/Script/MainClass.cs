using System;
using ZyGames.Framework.Common.Log;

namespace GameWebServer.Script
{
    public class MainClass
    {
        public void Start()
        {
            var setting = new EnvironmentSetting();
            GameEnvironment.Start(setting);
        }
    }
}