using System;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Script;

namespace Game.Script
{
    public class MainClass : GameHttpHost, IMainScript
    {
        public override void Start(string[] args)
        {
        }

        public override void Stop()
        {

        }

        protected override void OnRequested(ActionGetter actionGetter, BaseGameResponse response)
        {
            base.OnRequested(actionGetter, response);
        }

    }
}