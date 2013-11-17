using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Doudizhu.Bll.Action
{
    /// <summary>
    /// 360获取Token接口
    /// </summary>
    public class Action360 : ReAccessTokenAction
    {
        public Action360(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action360, httpGet)
        {
        }
    }
}
