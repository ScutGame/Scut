using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 处理下发回调
    /// </summary>
    internal class WcfCallback : IWcfCallback
    {
        /// <summary>
        /// 接收字节流
        /// </summary>
        /// <param name="param"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public void Receive(string param, byte[] buffer)
        {
            try
            {
                if (OnReceived != null)
                {
                    OnReceived.BeginInvoke(param, buffer, null, null);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("WcfCallback receive error:{0}", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<string, byte[]> OnReceived;

        public event EventHandler OnClosed;

        public void Close()
        {
            if (OnClosed != null)
            {
                EventArgs e = new EventArgs();
                OnClosed(this, e);
            }
        }
    }
}
