using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Contract.Action;

namespace ZyGames.Doudizhu.Bll.Action
{
    /// <summary>
    /// 充值通用接口
    /// </summary>
    public class Action1081 : PayNormalAction
    {
        public Action1081(short aActionId, HttpGet httpGet)
            : base(aActionId, httpGet)
        {
        }
    }
}
