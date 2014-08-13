using System;
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class RemotePackage
    {
        /// <summary>
        /// init
        /// </summary>
        public RemotePackage()
        {
            SendTime = DateTime.Now;
        }
        /// <summary>
        /// message id of client request
        /// </summary>
        public int MsgId { get; set; }

        /// <summary>
        /// 服务器间内部通讯通道
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Message of custom
        /// </summary>
        public object Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// is pushed package
        /// </summary>
        [JsonIgnore]
        public bool IsPushed { get { return MsgId == 0; } }

        /// <summary>
        /// callback
        /// </summary>
        public event Action<RemotePackage> Callback;

        internal virtual void OnCallback()
        {
            try
            {
                Action<RemotePackage> handler = Callback;
                if (handler != null) handler(this);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("RemotePackage OnCallback error:{0}", ex);
            }
        }
    }
}
