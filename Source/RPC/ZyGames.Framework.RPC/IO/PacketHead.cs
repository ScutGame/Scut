namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class PacketHead
    {
        private readonly PacketMsgType _msgType;
        private ConnectType _connectType;

        /// <summary>
        /// 
        /// </summary>
        public PacketHead(ConnectType connectType, PacketMsgType msgType)
        {
            _connectType = connectType;
            _msgType = msgType;
            Address = string.Empty;
            SSID = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectType ConnectType
        {
            get { return _connectType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public PacketMsgType MsgType
        {
            get { return _msgType; }
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string SSID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MsgId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Length { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnableGzip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ActionId { get; set; }
    }
}
