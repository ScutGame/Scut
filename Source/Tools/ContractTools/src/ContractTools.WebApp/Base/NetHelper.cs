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
using System.Collections.Concurrent;
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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Sockets;

namespace ContractTools.WebApp.Base
{
    public static class NetHelper
    {
        public static int LoginActionId = ConfigUtils.GetSetting("UnitTest.LoginActionId", 1004);
        private static string SignKey = ConfigUtils.GetSetting("Product.SignKey");
        public static string ClientDesDeKey = ConfigUtils.GetSetting("Product.ClientDesDeKey", "j6=9=1ac");

        public static MessageStructure Create(string serverUrl, string requestParams, out MessageHead header, bool isSocket, int actionId, string pid)
        {
            header = null;
            MessageStructure msgReader = null;
            if (isSocket)
            {
                msgReader = DoRequest(serverUrl, requestParams, actionId, pid);
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

        private static MessageStructure DoRequest(string server, string param, int actionId, string pid)
        {
            string[] serverArray = server.Split(':');
            return DoRequest(serverArray[0], Convert.ToInt32(serverArray[1]), GetSign(param), actionId, pid);
        }

        private static MessageStructure DoRequest(string host, int port, string param, int actionId, string pid)
        {
            var remoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(host)[0], port);
            return DoRequest(remoteEndPoint, param, 1024, actionId, pid);
        }

        private static ConcurrentDictionary<string, MyConnect> _clientSockets = new ConcurrentDictionary<string, MyConnect>();
        private static MessageStructure DoRequest(IPEndPoint remoteEndPoint, string param, int bufferSize, int actionId, string pid)
        {
            byte[] data = Encoding.UTF8.GetBytes("?d=" + param);
            MyConnect myConnect = null;
            if (!_clientSockets.ContainsKey(pid))
            {
                myConnect = new MyConnect();
                _clientSockets[pid] = myConnect;
            }
            else
            {
                myConnect = _clientSockets[pid];
            }

            if (myConnect.Client == null || myConnect.Check(remoteEndPoint))
            {
                myConnect.EndPoint = remoteEndPoint;
                myConnect.Client = new ClientSocket(new ClientSocketSettings(bufferSize, remoteEndPoint));
                myConnect.Client.UserData = myConnect;
                myConnect.Bind();
            }
            if (!myConnect.Client.Connected)
            {
                myConnect.Client.Connect();
            }
            if (myConnect.PutAction(actionId))
            {
                myConnect.Client.PostSend(data, 0, data.Length);
                return myConnect.GetResult();
            }
            return null;
        }
    }

    class MyPack
    {
        public MessageHead Head { get; set; }
        public byte[] Data { get; set; }
    }
    internal class MyConnect
    {
        private readonly object syncRoot = new object();
        private ManualResetEvent singal = new ManualResetEvent(false);
        private int _actionId;
        private ConcurrentQueue<MyPack> actionPools = new ConcurrentQueue<MyPack>();
        private ConcurrentQueue<MyPack> pushPools = new ConcurrentQueue<MyPack>();
        private MessageStructure ms;
        private Timer checkTimer;
        private int _isRunning = 0;

        public MyConnect()
        {
            checkTimer = new Timer(OnCheckPack, this, 10, 10);
        }
        public ClientSocket Client { get; set; }
        public IPEndPoint EndPoint { get; set; }

        public bool Check(IPEndPoint endPoint)
        {
            return EndPoint == null ||
                   EndPoint.ToString() != endPoint.ToString();
        }

        private static void OnCheckPack(object state)
        {
            var myConnect = state as MyConnect;
            if (myConnect == null)
            {
                return;
            }
            if (Interlocked.CompareExchange(ref myConnect._isRunning, 1, 0) == 0)
            {
                try
                {
                    MyPack pack;
                    if (myConnect.actionPools.TryDequeue(out pack))
                    {
                        if (pack.Head.Action == myConnect._actionId)
                        {
                            myConnect.ms = new MessageStructure(pack.Data);
                            myConnect.singal.Set();
                        }
                        else
                        {
                            //myConnect.pushPools.Enqueue(pack);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("CheckPack error:{0}", ex);
                }
                finally
                {
                    Interlocked.Exchange(ref myConnect._isRunning, 0);
                }
            }
        }

        public void Bind()
        {
            Client.DataReceived += OnReceive;
        }

        private static void OnReceive(object sender, SocketEventArgs e)
        {
            var myConnect = ((ClientSocket)sender).UserData as MyConnect;
            if (myConnect != null)
            {
                var stream = new MessageStructure(e.Data);
                var head = stream.ReadHeadGzip();
                var pack = new MyPack() { Head = head, Data = e.Data };
                myConnect.actionPools.Enqueue(pack);
            }
        }


        public MessageStructure GetResult()
        {
            singal.WaitOne(10000);
            return ms;
        }

        public bool PutAction(int actionId)
        {
            ms = null;
            _actionId = actionId;
            singal.Reset();
            return true;
        }
    }
}