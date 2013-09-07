using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Framework.RPC.Wcf
{
    /// <summary>
    /// 服务请求基类
    /// </summary>
    public sealed class ServiceRequest
    {

        /// <summary>
        /// 发送请求
        /// </summary>
        public static bool Request(RequestSettings setting, out byte[] buffer)
        {
            return DoCall(setting, out buffer);
        }

        /// <summary>
        /// 远端调用
        /// </summary>
        public static bool CallRemote(RequestSettings setting, out byte[] buffer)
        {
            return DoCall(setting, out buffer, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ResetChannel(RequestSettings setting)
        {
            GetWcfClient(setting).ResetChannel();
        }

        private static bool DoCall(RequestSettings setting, out byte[] buffer, bool isRoute = false)
        {
            buffer = new byte[0];
            WcfServiceClient wcfClient = GetWcfClient(setting);

            if (wcfClient.Connect())
            {
                if (isRoute)
                {
                    return wcfClient.TryCallRemote(setting.RouteName, setting.ParamString, setting.RemoteAddress, out buffer);
                }
                return wcfClient.TryRequest(setting.ParamString, setting.RemoteAddress, out buffer);

            }
            return false;
        }

        private static WcfServiceClient GetWcfClient(RequestSettings setting)
        {
            WcfServiceClient wcfClient = WcfServiceClientManager.Current.Get(setting.GameId, setting.ServerId);
            if (wcfClient == null)
            {
                throw new KeyNotFoundException(string.Format("Not found setting for game:{0} server:{1}", setting.GameId, setting.ServerId));
            }
            return wcfClient;
        }
    }
}
