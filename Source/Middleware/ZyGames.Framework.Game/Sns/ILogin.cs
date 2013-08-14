namespace ZyGames.Framework.Game.Sns
{
    public interface ILogin
    {
        string PassportID { get; }
        string UserID { get; }
        string Password { get; }
        string SessionID { get; }

        /// <summary>
        /// 注册通行证
        /// </summary>
        /// <returns></returns>
        string GetRegPassport();

        bool CheckLogin();

    }

}
