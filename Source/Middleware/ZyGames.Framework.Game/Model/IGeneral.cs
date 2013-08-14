namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// 将领接口
    /// </summary>
    public interface IGeneral
    {
        int GeneralID { get; }

        int LifeNum { get; set; }

        int Position { get; set; }

        int ReplacePosition { get; set; }

        /// <summary>
        /// 等待的佣兵，有其它佣兵死后替补
        /// </summary>
        bool IsWait { get; set; }
    }
}
