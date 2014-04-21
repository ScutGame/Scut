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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ContractTools.WebApp.Base
{
    public static class NetHelper
    {
        public static int LoginActionId = ConfigUtils.GetSetting("UnitTest.LoginActionId", 1004);
        private static string SignKey = ConfigUtils.GetSetting("Product.SignKey");
        public static string ClientDesDeKey = ConfigUtils.GetSetting("Product.ClientDesDeKey", "j6=9=1ac");

        public static MessageStructure Create(string serverUrl, string requestParams, out MessageHead header, bool isSocket)
        {
            header = null;
            MessageStructure msgReader = null;
            if (isSocket)
            {
                msgReader = DoRequest(serverUrl, requestParams);
            }
            else
            {
                Encoding encode = Encoding.GetEncoding("utf-8");
                string postData = "d=" + GetSign(requestParams);
                byte[] bufferData = encode.GetBytes(postData);

                HttpWebRequest serverRequest = (HttpWebRequest)WebRequest.Create(serverUrl);
                serverRequest.Method = "POST";
                serverRequest.ContentType = "application/x-www-form-urlencoded";
                serverRequest.ContentLength = bufferData.Length;
                Stream requestStream = serverRequest.GetRequestStream();
                requestStream.Write(bufferData, 0, bufferData.Length);
                requestStream.Close();
                //
                WebResponse serverResponse = serverRequest.GetResponse();
                Stream responseStream = serverResponse.GetResponseStream();
                msgReader = MessageStructure.Create(responseStream, Encoding.UTF8);
            }
            if (msgReader != null)
            {
                header = msgReader.ReadHeadGzip();
            }
            return msgReader;
        }

        public static bool GetFieldValue(MessageStructure ms, FieldType fieldType, ref string val)
        {
            bool result = false;
            switch (fieldType)
            {
                case FieldType.Int:
                    val = ms.ReadInt().ToString();
                    result = true;
                    break;
                case FieldType.String:
                    val = ms.ReadString();
                    result = true;
                    break;
                case FieldType.Short:
                    val = ms.ReadShort().ToString();
                    result = true;
                    break;
                case FieldType.Byte:
                    val = ms.ReadByte().ToString();
                    result = true;
                    break;
                case FieldType.Long:
                    val = ms.ReadLong().ToString();
                    result = true;
                    break;
                case FieldType.Bool:
                    val = ms.ReadBool().ToString();
                    result = true;
                    break;
                case FieldType.Float:
                    val = ms.ReadFloat().ToString();
                    result = true;
                    break;
                case FieldType.Double:
                    val = ms.ReadDouble().ToString();
                    result = true;
                    break;
                case FieldType.Record:
                    break;
                case FieldType.End:
                    break;
                case FieldType.Head:
                    break;
                default:
                    break;
            }

            return result;
        }

        private static string GetSign(string requestParams)
        {
            string sign = "";
            if (!string.IsNullOrEmpty(SignKey))
            {
                sign = FormsAuthentication.HashPasswordForStoringInConfigFile(requestParams + SignKey, "MD5").ToLower();
            }
            return Uri.EscapeDataString(string.Format("{0}&sign={1}", requestParams, sign));
        }

        private static MessageStructure DoRequest(string server, string param)
        {
            string[] serverArray = server.Split(':');
            return DoRequest(serverArray[0], Convert.ToInt32(serverArray[1]), GetSign(param));
        }

        private static MessageStructure DoRequest(string host, int port, string param)
        {
            var remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            return DoRequest(remoteEndPoint, param, 4096);
        }

        private static MessageStructure DoRequest(IPEndPoint remoteEndPoint, string param, int bufferSize)
        {
            byte[] data = Encoding.UTF8.GetBytes("?d=" + param);
            MessageStructure ms = null;
            ClientSocket client = null;
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null)
            {
                client = context.Session["CLIENT_CONNECT"] as ClientSocket;
            }
            if (client == null)
            {
                client = new ClientSocket(new ClientSocketSettings(1024, remoteEndPoint));
                if (context != null && context.Session != null)
                {
                    context.Session["CLIENT_CONNECT"] = client;
                }
            }

            var singal = new ManualResetEvent(false);
            client.DataReceived += (sender, e) =>
            {
                ms = new MessageStructure(e.Data);
                singal.Set();
            };
            client.Connect();
            client.PostSend(data, 0, data.Length);
            singal.WaitOne(60000);//60s

            return ms;
        }
    }
}