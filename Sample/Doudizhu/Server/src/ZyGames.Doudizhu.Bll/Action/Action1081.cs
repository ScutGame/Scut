using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;

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
