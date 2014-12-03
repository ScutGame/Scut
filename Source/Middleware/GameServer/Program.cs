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
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.RPC.Service;
using ZyGames.Framework.Script;

namespace GameServer
{
    class Program
    {
        private static readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private static string CharFormat =
@"///////////////////////////////////////////////////////////////////////////

    //   ) )  //   ) )  //   / / /__  ___/   SCUT Server version {0}
   ((        //        //   / /    / /       Game: {1}   Server: {2}
     \\     //        //   / /    / /        Port: {3}
       ) ) //        //   / /    / /        
((___ / / ((____/ / ((___/ /    / /                http://www.scutgame.com

";
        static void Main(string[] args)
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            catch { }
            OnInit(args);
            try
            {
                Console.ForegroundColor = currentForeColor;
            }
            catch { }
            try
            {
                TraceLog.WriteLine("# Server command \"Ctrl+C\" or \"Ctrl+Break\" exit.");
                RunAsync(args).Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }
        }

        private static void OnInit(string[] args)
        {
            try
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                var setting = new EnvironmentSetting();
                TraceLog.WriteLine(string.Format(CharFormat,
                    Assembly.GetExecutingAssembly().GetName().Version,
                    setting.ProductCode,
                    setting.ProductServerId,
                    setting.GamePort));
                GameEnvironment.Start(setting);

            }
            catch (Exception ex)
            {
                TraceLog.WriteLine("{0} Server failed to start error:{1}", DateTime.Now.ToString("HH:mm:ss"), ex.Message);
                TraceLog.WriteError("OnInit error:{0}", ex);
            }
        }

        private static async Task RunAsync(string[] args)
        {
            try
            {
                if (ScriptEngines.RunMainProgram(args))
                {
                    TraceLog.WriteLine("{0} Server has started successfully!", DateTime.Now.ToString("HH:mm:ss"));
                    TraceLog.WriteLine("# Server is listening...");
                }
                else
                {
                    TraceLog.WriteLine("{0} Server failed to start!", DateTime.Now.ToString("HH:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteLine("{0} Server failed to start error:{1}", DateTime.Now.ToString("HH:mm:ss"), ex.Message);
                TraceLog.WriteError("RunMain error:{0}", ex);
            }

            while (!GameEnvironment.IsCanceled)
            {
                await Task.Delay(1000);
            }

            OnStop();
        }

        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            try
            {
                GameEnvironment.IsCanceled = true;
                runCompleteEvent.WaitOne();
                TraceLog.WriteLine("{0} Server has canceled!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnCancelKeyPress error:{1}", ex);
            }
        }

        private static void OnStop()
        {
            try
            {
                TraceLog.WriteLine("{0} Server is stoping...", DateTime.Now.ToString("HH:mm:ss"));
                ScriptEngines.StopMainProgram();
                GameEnvironment.WaitStop().Wait();
                TraceLog.WriteLine("{0} Server has stoped successfully!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnStop error:{1}", ex);
            }
        }
    }
}
