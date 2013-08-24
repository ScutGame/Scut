using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public abstract class Receiver : IDisposable
    {
        /// <summary>
        /// 对消息解码
        /// </summary>
        public abstract void Decode();

        /// <summary>
        /// 较验签名
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckSign();

        /// <summary>
        /// 处理
        /// </summary>
        /// <returns></returns>
        public abstract void Process();

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
