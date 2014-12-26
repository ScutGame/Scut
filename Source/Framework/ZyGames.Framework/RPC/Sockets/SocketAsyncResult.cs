using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 
        /// </summary>
        Wait,
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 
        /// </summary>
        Close,
        /// <summary>
        /// 
        /// </summary>
        Error
    }

    /// <summary>
    /// Socket send async result
    /// </summary>
    public class SocketAsyncResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public SocketAsyncResult(byte[] data)
        {
            Result = ResultCode.Wait;
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public ExSocket Socket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected internal Action<SocketAsyncResult> ResultCallback;

        /// <summary>
        /// 
        /// </summary>
        public ResultCode Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal void Callback()
        {
            if (ResultCallback != null)
            {
                try
                {
                    ResultCallback(this);
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("ResultCallback error{0}", ex);
                }
            }
        }
    }
}
