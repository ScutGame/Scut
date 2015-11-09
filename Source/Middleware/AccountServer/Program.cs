using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AccountServer.Lang;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Config;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Sns.Service;
using ZyGames.Framework.RPC.Http;
using IHttpAsyncHandler = ZyGames.Framework.RPC.Http.IHttpAsyncHandler;

namespace AccountServer
{
    class Program
    {
        private static readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            try
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                try
                {
                    HandlerManager.Init(Assembly.GetExecutingAssembly());
                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("Application_Start error:{0}", ex);
                }

                HttpAsyncHandler.PageListen["/"] = OnService;//default page
                HttpAsyncHandler.PageListen["/default.ashx"] = OnService;

                var listenUrls = new List<string>();
                var httpListener = new HttpAsyncHost(HttpAsyncHandler.Default, 16);
                var section = ConfigManager.Configger.GetFirstOrAddConfig<ProtocolSection>();
                var httpHost = section.HttpHost;
                var httpPort = section.HttpPort;
                var httpName = section.HttpName;

                if (!string.IsNullOrEmpty(httpHost))
                {
                    var hosts = httpHost.Split(',');
                    foreach (var point in hosts)
                    {
                        var addressList = point.Split(':');
                        string host = addressList[0];
                        int port = httpPort;
                        if (addressList.Length > 1)
                        {
                            int.TryParse(addressList[1], out port);
                        }
                        string address = host.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                            ? host
                            : "http://" + host;
                        if (listenUrls.Count == 0) listenUrls.Add(string.Format("{0}:{1}/", address, port));
                        listenUrls.Add(string.Format("{0}:{1}/{2}/", address, port, httpName));
                    }
                }
                httpListener.Run(listenUrls.ToArray());
                foreach (var listenUrl in listenUrls)
                {
                    TraceLog.WriteLine("{0} Http service:{1} is started.", DateTime.Now.ToString("HH:mm:ss"), listenUrl.TrimEnd('/'));
                }
                if (httpListener.Error != null)
                {
                    throw httpListener.Error;
                }
                TraceLog.WriteLine("{0} AccountServer has started successfully!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteLine("{0} AccountServer failed to start!", DateTime.Now.ToString("HH:mm:ss"));
                TraceLog.WriteError("Start Error:{0}", ex);
            }

            try
            {
                TraceLog.WriteLine("# Server exit command \"Ctrl+C\" or \"Ctrl+Break\".");
                RunWait().Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }
        }

        private static async System.Threading.Tasks.Task RunWait()
        {
            while (!GameEnvironment.IsCanceled)
            {
                await System.Threading.Tasks.Task.Delay(1000);
            }
        }
        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            GameEnvironment.IsCanceled = true;
            runCompleteEvent.WaitOne();
        }

        private static byte[] OnService(IHttpRequestContext context)
        {
            return new MyHttpHandler().ProcessRequest(context);
        }

        public class HttpAsyncHandler : IHttpAsyncHandler
        {
            public readonly static Dictionary<string, Func<IHttpRequestContext, byte[]>> PageListen = new Dictionary<string, Func<IHttpRequestContext, byte[]>>();

            /// <summary>
            /// 
            /// </summary>
            public static readonly IHttpAsyncHandler Default = new HttpAsyncHandler();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public async Task<IHttpResponseAction> Execute(IHttpRequestContext context)
            {
                int statusCode = 200;
                string page = (context.Request.RawUrl ?? "").Split('?')[0].ToLower();
                byte[] result;
                if (PageListen.ContainsKey(page))
                {
                    var func = PageListen[page];
                    result = func(context);
                    return new ByteResponse(statusCode, "OK", result);
                }
                return new ByteResponse(404, "Not found", Encoding.UTF8.GetBytes(""));
            }

        }

        public class MyHttpHandler : BaseHttpHandler
        {
            protected override void OnRequest(IHttpRequestContext context, ResponseBody body)
            {
                var watch = Stopwatch.StartNew();
                try
                {
                    string param;
                    if (CheckSign(context, out param))
                    {
                        HandlerData handlerData;
                        if (TryUrlQueryParse(param, out handlerData))
                        {
                            body.Handler = handlerData.Name;
                            body.Data = HandlerManager.Excute(handlerData);
                        }
                        else
                        {
                            body.StateCode = StateCode.NoHandler;
                            body.StateDescription = StateDescription.NoHandler;
                        }
                    }
                    else
                    {
                        body.StateCode = StateCode.SignError;
                        body.StateDescription = StateDescription.SignError;//"Sign error.";
                    }
                }
                catch (HandlerException handlerError)
                {
                    body.StateCode = handlerError.StateCode;
                    body.StateDescription = handlerError.Message;
                    TraceLog.WriteError("Request handle error:{0}", handlerError);
                }
                catch (Exception error)
                {
                    body.StateCode = StateCode.Error;
                    body.StateDescription = StateDescription.Error;// "Process request fail.";
                    TraceLog.WriteError("Request handle error:{0}", error);
                }
                var ms = watch.ElapsedMilliseconds;
                if (ms > 20)
                {
                    TraceLog.Write("Request timeout:{0}ms", ms);
                }
            }

        }
    }
}
