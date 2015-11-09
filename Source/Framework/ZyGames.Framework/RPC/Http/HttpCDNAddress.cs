using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.RPC.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpCDNAddress
    {
        private string[] _headKeys;

        /// <summary>
        /// 
        /// </summary>
        public HttpCDNAddress(params string[] headKeys)
        {
            _headKeys = headKeys.Length > 0 ? headKeys : new[] { "Client-Ip", "Cdn_Src_Ip", "X-Real-IP", "X-Forwarded-For" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="headGetter"></param>
        /// <returns></returns>
        public string GetUserHostAddress(IPEndPoint endPoint, Func<string, string> headGetter)
        {
            return GetUserHostAddress(endPoint != null ? endPoint.Address.ToString() : string.Empty, headGetter);
        }

        /// <summary>
        /// Get address
        /// </summary>
        public string GetUserHostAddress(string address, Func<string, string> headGetter)
        {
            string result = address;
            try
            {
                foreach (var headKey in _headKeys)
                {
                    string headValue = headGetter(headKey);
                    if (string.IsNullOrEmpty(headValue) || headValue == result)
                    {
                        continue;
                    }
                    var keys = headValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (keys.Length > 0)
                    {
                        result = keys[0];
                        break;
                    }
                }
            }
            catch { }
            return result;
        }
    }
}
