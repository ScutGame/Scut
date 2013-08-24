using System;
using System.Collections.Generic;
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
        public static RequestError Request(RequestSettings setting, out byte[] buffer)
        {
            return DoCall(setting, out buffer);
        }

        /// <summary>
        /// 远端调用
        /// </summary>
        public static RequestError CallRemote(RequestSettings setting, out byte[] buffer)
        {
            return DoCall(setting, out buffer, true);
        }


        private static RequestError DoCall(RequestSettings setting, out byte[] buffer, bool isRoute = false)
        {
            buffer = new byte[0];
            WcfServiceClient wcfClient = null;
            try
            {
                wcfClient = WcfServiceClientManager.Current.Get(setting.GameId, setting.ServerId);
                if (wcfClient == null)
                {
                    return RequestError.NotFindService;
                }

                int resetTime = 2; //重置一次
                for (int i = 0; i < resetTime; i++)
                {
                    try
                    {
                        if (wcfClient.Connected || wcfClient.Connect())
                        {
                            bool result = false;
                            if (isRoute)
                            {
                                result = wcfClient.TryCallRemote(setting.RouteName, setting.ParamString, setting.RemoteAddress, out buffer);
                            }
                            else
                            {
                                result = wcfClient.TryRequest(setting.ParamString, setting.RemoteAddress, out buffer);
                            }
                            if (result)
                            {
                                return RequestError.Success;
                            }
                            wcfClient.ResetChannel();
                        }
                    }
                    catch (CommunicationObjectFaultedException fault)
                    {
                        if (i > 0)
                        {
                            TraceLog.WriteError("Request:{0}", fault);
                        }
                        wcfClient.ResetChannel();
                    }
                    catch (Exception ex)
                    {
                        wcfClient.ResetChannel();
                    }
                }
                return RequestError.UnableConnect;
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("ServiceRequest error:{0}", ex);
                return RequestError.Unknown;
            }
        }
    }
}
