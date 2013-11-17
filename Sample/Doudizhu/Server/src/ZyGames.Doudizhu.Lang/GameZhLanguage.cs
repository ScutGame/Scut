using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Doudizhu.Lang
{
    class GameZhLanguage : BaseZHLanguage, IGameLanguage
    {
        public int SystemUserId
        {
            get { return 1000000; }
        }

        public string KingName
        {
            get { return "系统"; }
        }

        public string Date_Yesterday { get { return "昨天"; } }
        public string Date_BeforeYesterday { get { return "前天"; } }
        public string Date_Day { get { return "{0}天前"; } }

        public string St1002_GetRegisterPassportIDError { get { return "获取注册通行证ID失败!"; } }

        public string St1005_NickNameOutRange { get { return "您的昵称输入有误，请重新输入!"; } }
        public string St1005_NickNameExistKeyword { get { return "您输入的昵称存在非法字符，请重新输入!"; } }
        public string St1005_NickNameExist { get { return "您输入的昵称已存在，请重新输入!"; } }

        public string St1006_PasswordTooLong { get { return "输入错误，请输入4-12位数字或字母!"; } }
        public string St1006_ChangePasswordError { get { return "修改密码失败!"; } }
        public string St1006_PasswordError { get { return "密码格式错误!"; } }
        public string St1066_PayError { get { return "充值失败"; } }
    }
}
