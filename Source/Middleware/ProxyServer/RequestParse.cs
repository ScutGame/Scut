/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using NLog;
using ICSharpCode.SharpZipLib.GZip;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.RPC.IO;

namespace ProxyServer
{
    class RequestParse
    {
        private static string signkey = ConfigUtils.GetSetting("ProxySignkey", "44CAC8ED53714BF18D60C5C7B6296000");
        private static bool enableErrorMsg = ConfigUtils.GetSetting("EnableErrorMsg", false);
        public static NameValueCollection Parse(string ip, string rawUrl, string data, out int gameId, out int serverId, out int statuscode)
        {
            NameValueCollection nvc = new NameValueCollection();
            serverId = 0;
            gameId = 0;
            statuscode = (int)HttpStatusCode.OK;
            if (string.IsNullOrEmpty(data))
            {
                statuscode = (int)HttpStatusCode.BadRequest;
                TraceLog.WriteError("参数d不存在，RawUrl[{0}][{1}]", rawUrl, ip);
                return nvc;
            }

            nvc = HttpUtility.ParseQueryString(data);
            if (!nvc.AllKeys.Contains("rl", StringComparer.InvariantCultureIgnoreCase))
            {
                int idx = 0;
                idx = data.IndexOf("sign=", StringComparison.InvariantCultureIgnoreCase);
                if (idx == -1)
                {
                    statuscode = (int)HttpStatusCode.BadRequest;
                    TraceLog.WriteError("参数sign不存在，Request[{0}][{1}]", data, ip);
                    return nvc;
                }

                string clientCheckcode = nvc["sign"];
                string source = data.Substring(0, idx - 1);
                string mycheckcode = Util.MD5_Encrypt(source, signkey, Encoding.UTF8);
                if (string.Compare(clientCheckcode, mycheckcode, true) != 0)
                {
                    statuscode = (int)HttpStatusCode.Forbidden;
                    TraceLog.WriteError("md5校验错误，Request[{0}][{1}]", data, ip);
                    return nvc;
                }
            }
            //nvc.Remove("sign");

            int actionid;
            if (!nvc.AllKeys.Contains("actionid", StringComparer.InvariantCultureIgnoreCase) || !int.TryParse(nvc["actionid"], out actionid))
            {
                statuscode = (int)HttpStatusCode.BadRequest;
                TraceLog.WriteError("参数actionid不存在，Request[{0}][{1}]", data, ip);
                return nvc;
            }

            int msgid;
            if (!nvc.AllKeys.Contains("msgid", StringComparer.InvariantCultureIgnoreCase) || !int.TryParse(nvc["msgid"], out msgid))
            {
                statuscode = (int)HttpStatusCode.BadRequest;
                TraceLog.WriteError("参数msgid不存在，Request[{0}][{1}]", data, ip);
                return nvc;
            }

            var sid = nvc["sid"] ?? "";
            var sp = sid.Split('|');
            if (string.IsNullOrEmpty(nvc["sid"]) || sp.Length != 3)
            {
                //serverid判断
                if (!nvc.AllKeys.Contains("serverid", StringComparer.InvariantCultureIgnoreCase))
                {
                    TraceLog.WriteError("请求未传参数serverid或者sid，Request[{0}][{1}]", data, ip);
                }
                else if (!int.TryParse(nvc["serverid"], out serverId))
                {
                    TraceLog.WriteError("请求未传参数serverid或者sid，Request[{0}][{1}]", data, ip);
                }

                //gameiId判断
                if (!nvc.AllKeys.Contains("GameType", StringComparer.InvariantCultureIgnoreCase))
                {
                    TraceLog.WriteError("请求未传参数gameId或者sid，Request[{0}][{1}]", data, ip);
                }
                else if (!int.TryParse(nvc["GameType"], out gameId))
                {
                    TraceLog.WriteError("请求未传参数gameId或者sid，Request[{0}][{1}]", data, ip);
                }
            }
            else
            {
                gameId = Convert.ToInt32(sp[1]);
                serverId = Convert.ToInt32(sp[2]);
            }

            return nvc;
        }

        public static string ErrorMsgConnectFail = "Connect server fail.";
        public static string ErrorMsgConnectTimeout = "";
        public static string ErrorMsg = "Unknown error.";

        /// <summary>
        /// 转成url格式
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string ToQueryString(NameValueCollection data)
        {
            string value = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            string[] allKeys = data.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string text2 = allKeys[i];
                stringBuilder.Append(value);
                stringBuilder.Append(text2);
                stringBuilder.Append("=");
                stringBuilder.Append(HttpUtility.UrlEncode(data[text2]));
                value = "&";
            }
            return stringBuilder.ToString();
        }

        public static void WriteValue(Stream stream, int value)
        {
            byte[] buf = BitConverter.GetBytes(value);
            stream.Write(buf, 0, buf.Length);
        }

        public static void WriteValue(Stream stream, string value)
        {
            byte[] buf = Encoding.UTF8.GetBytes(value);
            WriteValue(stream, buf.Length);
            stream.Write(buf, 0, buf.Length);
        }

        public static byte[] CtorErrMsg(int error, string msg, NameValueCollection requestParam)
        {
            int msgId = Convert.ToInt32(requestParam["msgid"]);
            int actionid = Convert.ToInt32(requestParam["actionid"]);
            var ms = new MessageStructure();
            var head = new MessageHead(msgId, actionid, "st", error, enableErrorMsg ? msg : "");
            ms.WriteBuffer(head);
            return ms.PosGzipBuffer();
        }

    }
}