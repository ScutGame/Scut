using System;
using GameServer.Script.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;

namespace GameServer.Script.CsScript.Action
{

    /// <summary>
    /// 1008_推送World主界面
    /// </summary>
    /// <remarks>继续BaseStruct类:不检查用户合法性请求;AuthorizeAction:有验证合法性</remarks>
    public class Action1008 : AuthorizeAction
    {
        private UserRole _role;

        public Action1008(ActionGetter actionGetter)
            : base(1008, actionGetter)
        {

        }

        /// <summary>
        /// 客户端请求的参数较验
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool GetUrlElement()
        {
            return true;
        }

        /// <summary>
        /// 业务逻辑处理
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool TakeAction()
        {
            GameUser user = Current.User as GameUser;
            if (user == null) return false;

            var roleCache = new PersonalCacheStruct<UserRole>();
            _role = roleCache.FindKey(user.PersonalId, user.CurrRoleId);
            if (_role == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            this.PushIntoStack(_role.RoleId);
            this.PushIntoStack(_role.RoleName);
            this.PushIntoStack(_role.HeadImg);
            this.PushIntoStack(_role.Sex.ToByte());
            this.PushIntoStack(_role.LvNum);
            this.PushIntoStack(_role.ExperienceNum);
            this.PushIntoStack(_role.LifeNum);
            this.PushIntoStack(_role.LifeMaxNum);

        }

        protected override bool IgnoreActionId
        {
            get { return false; }
        }
    }
}
