using ZyGames.Framework.Game.Lang;

namespace ZyGames.Doudizhu.Lang
{
    public interface IGameLanguage : ILanguage
    {
        #region 通用提示
        /// <summary>
        /// 君主帐号
        /// </summary>
        int SystemUserId { get; }
        /// <summary>
        /// 玩家名称
        /// </summary>
        string KingName { get; }

        string Date_Yesterday { get; }
        string Date_BeforeYesterday { get; }
        string Date_Day { get; }

        string St1002_GetRegisterPassportIDError { get; }

        string St1005_NickNameOutRange { get; }
        string St1005_NickNameExistKeyword { get; }
        string St1005_NickNameExist { get; }

        string St1006_PasswordTooLong { get;}
        string St1006_ChangePasswordError { get;}
        string St1006_PasswordError { get;}
        string St1066_PayError { get; }

        #endregion
    }
}