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
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.RPC.Http
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NullHttpAsyncHandler : IHttpAsyncHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly NullHttpAsyncHandler Default = new NullHttpAsyncHandler();

        private static readonly Task<IHttpResponseAction> NullTask = Task.FromResult<IHttpResponseAction>(null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task<IHttpResponseAction> Execute(IHttpRequestContext state)
        {
            return NullTask;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class HttpAsyncHost : IHttpAsyncHost
    {
        HttpListener _listener;
        IHttpAsyncHandler _handler;
        HostContext _hostContext;
        ConfigurationDictionary _configValues;
        Timer log_timer;
        readonly int _accepts;

        /// <summary>
        /// Creates an asynchronous HTTP host.
        /// </summary>
        /// <param name="handler">Handler to serve requests with</param>
        /// <param name="accepts">
        /// Higher values mean more connections can be maintained yet at a much slower average response time; fewer connections will be rejected.
        /// Lower values mean less connections can be maintained yet at a much faster average response time; more connections will be rejected.
        /// </param>
        public HttpAsyncHost(IHttpAsyncHandler handler, int accepts = 4)
        {
            _handler = handler ?? NullHttpAsyncHandler.Default;
            _listener = new HttpListener();
            // Multiply by number of cores:
            _accepts = accepts * Environment.ProcessorCount;
            log_timer = new Timer(WriteLog, null, 300000, 300000);
        }

        class HostContext : IHttpAsyncHostHandlerContext
        {
            public IHttpAsyncHost Host { get; private set; }
            public IHttpAsyncHandler Handler { get; private set; }

            public HostContext(IHttpAsyncHost host, IHttpAsyncHandler handler)
            {
                Host = host;
                Handler = handler;
            }
        }

        /// <summary>
        /// listen error.
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Prefixes
        {
            get { return _listener.Prefixes.ToList(); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void SetConfiguration(ConfigurationDictionary values)
        {
            _configValues = values;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriPrefixes"></param>
        /// <returns></returns>
        public Task Run(params string[] uriPrefixes)
        {
            Error = null;
            // Establish a host-handler context:
            _hostContext = new HostContext(this, _handler);

            _listener.IgnoreWriteExceptions = true;

            if (uriPrefixes.Length == 0)
            {
                TraceLog.WriteError("Http start listenning url is null");
            }
            // Add the server bindings:
            foreach (var prefix in uriPrefixes)
            {
                _listener.Prefixes.Add(prefix.EndsWith("/") ? prefix : prefix + "/");
            }

            return Task.Run(async () =>
            {
                // Configure the handler:
                if (_configValues != null)
                {
                    var config = _handler as IConfigurationTrait;
                    if (config != null)
                    {
                        var task = config.Configure(_hostContext, _configValues);
                        if (task != null)
                            if (!await task) return;
                    }
                }

                // Initialize the handler:
                var init = _handler as IInitializationTrait;
                if (init != null)
                {
                    var task = init.Initialize(_hostContext);
                    if (task != null)
                        if (!await task) return;
                }

                try
                {
                    // Start the HTTP listener:
                    _listener.Start();
                }
                catch (HttpListenerException hlex)
                {
                    Error = hlex;
                    TraceLog.WriteError("Http start listenning error:{0}", hlex);
                    return;
                }

                // Accept connections:
                // Higher values mean more connections can be maintained yet at a much slower average response time; fewer connections will be rejected.
                // Lower values mean less connections can be maintained yet at a much faster average response time; more connections will be rejected.
                var sem = new Semaphore(_accepts, _accepts);

                while (true)
                {
                    sem.WaitOne();

#pragma warning disable 4014
                    _listener.GetContextAsync().ContinueWith(async (t) =>
                    {
                        try
                        {
                            sem.Release();
                            var ctx = await t;
                            await ProcessListenerContext(ctx, this);

                            return;
                        }
                        catch (Exception ex)
                        {
                            TraceLog.WriteError("Http request unknow error:{0}", ex);
                        }
                    });
#pragma warning restore 4014
                }
            });
        }

        static int _20s, _50s, _100s, _200s, _500s, _1000s, _2000s, _5000s, _up;

        static void WriteLog(object state)
        {
            int m_20s, m_50s, m_100s, m_200s, m_500s, m_1000s, m_2000s, m_5000s, m_up;
            m_20s = Interlocked.Exchange(ref _20s, 0);
            m_50s = Interlocked.Exchange(ref _50s, 0);
            m_100s = Interlocked.Exchange(ref _100s, 0);
            m_200s = Interlocked.Exchange(ref _200s, 0);
            m_500s = Interlocked.Exchange(ref _500s, 0);
            m_1000s = Interlocked.Exchange(ref _1000s, 0);
            m_2000s = Interlocked.Exchange(ref _2000s, 0);
            m_5000s = Interlocked.Exchange(ref _5000s, 0);
            m_up = Interlocked.Exchange(ref _up, 0);
            TraceLog.ReleaseWriteDebug("Http request timeout(ms) 20:{0}  50:{1}  100:{2}  200:{3}  500:{4}  1000:{5}  2000:{6}  5000:{7}  up:{8}", m_20s, m_50s, m_100s, m_200s, m_500s, m_1000s, m_2000s, m_5000s, m_up);
        }

        static async Task ProcessListenerContext(HttpListenerContext listenerContext, HttpAsyncHost host)
        {
            try
            {
                // Get the response action to take:
                Stopwatch sw = Stopwatch.StartNew();
                var requestContext = new HttpRequestContext(host._hostContext, listenerContext.Request, listenerContext.User);
                var action = await host._handler.Execute(requestContext);
                sw.Stop();

                if (sw.ElapsedMilliseconds <= 20) Interlocked.Increment(ref _20s);
                else if (sw.ElapsedMilliseconds <= 50) Interlocked.Increment(ref _50s);
                else if (sw.ElapsedMilliseconds <= 100) Interlocked.Increment(ref _100s);
                else if (sw.ElapsedMilliseconds <= 200) Interlocked.Increment(ref _200s);
                else if (sw.ElapsedMilliseconds <= 500) Interlocked.Increment(ref _500s);
                else if (sw.ElapsedMilliseconds <= 1000) Interlocked.Increment(ref _1000s);
                else if (sw.ElapsedMilliseconds <= 2000) Interlocked.Increment(ref _2000s);
                else if (sw.ElapsedMilliseconds <= 5000) Interlocked.Increment(ref _5000s);
                else
                {
                    TraceLog.WriteError("Http request [{0}] timeout {1}ms", listenerContext.Request.RawUrl, sw.ElapsedMilliseconds);
                    Interlocked.Increment(ref _up);
                }
                if (action != null)
                {
                    // Take the action and await its completion:
                    var responseContext = new HttpRequestResponseContext(requestContext, listenerContext.Response);
                    var task = action.Execute(responseContext);
                    if (task != null) await task;
                }

                // Close the response and send it to the client:
                listenerContext.Response.Close();
            }
            catch (HttpListenerException)
            {
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Http ProcessListenerContext {0}", ex);
            }
        }
    }
}
