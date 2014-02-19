using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Contract.Action;

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
