using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ZyGames.Core.Util;
using ZyGames.Core.Web;

namespace ZyGames.OA.WatchService.BLL.Tools
{
    public enum PlanType
    {
        none,
        cpu,
        msmq,
        drive,
        dbbackup
    }
    public static class OaSimplePlanHelper
    {
        private static string serverUrl = ConfigHelper.GetSetting("OAPlan_Server");
        private static string LocalAddress = ConfigHelper.GetSetting("OAPlan_Local_Address", "127.0.0.1");

        /// <summary>
        /// 提交数据到远程
        /// </summary>
        public static void PostDataToServer(string name, object value)
        {
            PostDataToServer(PlanType.none, name, value);
        }

        public static void PostDataToServer(PlanType type, string name, object value)
        {
            name = HttpUtility.UrlEncode(name);
            value = HttpUtility.UrlEncode(value.ToString());
            string postUrl = string.Format("{0}?type={1}&ip={2}&name={3}&data={4}", serverUrl, type.ToString(), GetServerIP(), name, value);
            HttpHelper.GetReponseText(postUrl);
        }

        public static string GetServerIP()
        {
            System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                if (!addressList[i].ToString().StartsWith("10."))
                {
                    return addressList[i].ToString();
                }
            }
            return LocalAddress;
        }
    }
}
