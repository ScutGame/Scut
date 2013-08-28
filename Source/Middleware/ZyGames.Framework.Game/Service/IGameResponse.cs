namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// 游戏输出接口
    /// </summary>
    public interface IGameResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        void BinaryWrite(byte[] buffer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        void Write(byte[] buffer);
    }

}
