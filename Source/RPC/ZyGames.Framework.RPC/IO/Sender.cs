using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Sender : IDisposable
    {
        /// <summary>
        /// 编码
        /// </summary>
        public abstract void Encode();

        /// <summary>
        /// 加签名
        /// </summary>
        public abstract void AppendSign();

        /// <summary>
        /// 
        /// </summary>
        public abstract void Send();

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            DoDispose(true);
        }

        protected virtual void DoDispose(bool disposing)
        {
            if (disposing)
            {
                //清理托管对象
                GC.SuppressFinalize(this);
            }
            //清理非托管对象
        }
    }
}
