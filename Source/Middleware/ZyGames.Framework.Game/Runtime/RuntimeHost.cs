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
using System.Reflection;
using System.Threading;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// Runtime host service
    /// </summary>
    public class RuntimeHost
    {

        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        /// <summary>
        /// 
        /// </summary>
        protected static string LoginCharFormat =
@"///////////////////////////////////////////////////////////////////////////

    //   ) )  //   ) )  //   / / /__  ___/   SCUT Server version {0}
   ((        //        //   / /    / /       Login Server
     \\     //        //   / /    / /        
       ) ) //        //   / /    / /        
((___ / / ((____/ / ((___/ /    / /                http://www.scutgame.com

";

        private static string CharFormat =
@"///////////////////////////////////////////////////////////////////////////

    //   ) )  //   ) )  //   / / /__  ___/   SCUT {0} ({1} bit)
   ((        //        //   / /    / /       Running in {2} platform
     \\     //        //   / /    / /        Game: {3}   Server: {4}
       ) ) //        //   / /    / /         
((___ / / ((____/ / ((___/ /    / /                http://www.scutgame.com
";

        private EnvironmentSetting _setting;

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnInit()
        {
            try
            {
                _setting = new EnvironmentSetting();
                var osbit = GetOsBit();
                var platform = GetRunPlatform();
                TraceLog.WriteLine(string.Format(CharFormat,
                    Assembly.GetExecutingAssembly().GetName().Version,
                    osbit,
                    platform,
                    _setting.ProductCode,
                    _setting.ProductServerId));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Process start logic init
        /// </summary>
        /// <returns></returns>
        public virtual bool OnStart()
        {
            try
            {
                GameEnvironment.Start(_setting);
                return true;
            }
            catch (Exception ex)
            {
                TraceLog.WriteLine("{0} Server failed to start error:{1}", DateTime.Now.ToString("HH:mm:ss"), ex.Message);
                TraceLog.WriteError("OnInit error:{0}", ex);
                TraceLog.WriteLine("# Server exit command \"Ctrl+C\" or \"Ctrl+Break\".");
            }
            return false;
        }

        private int GetOsBit()
        {
            try
            {
                return Environment.Is64BitProcess ? 64 : 32;
            }
            catch (Exception)
            {
                return 32;
            }
        }
        private string GetRunPlatform()
        {
            try
            {
                return Environment.OSVersion.Platform.ToString();
            }
            catch (Exception)
            {
                return "Unknow";
            }
        }
        /// <summary>
        /// Run
        /// </summary>
        public void Run()
        {
            try
            {
                RunAsync().Wait();
            }
            finally
            {
                runCompleteEvent.Set();
            }

        }

        /// <summary>
        /// Proccess stop logic
        /// </summary>
        public virtual void OnStop()
        {
            try
            {
                TraceLog.WriteLine("{0} Server is stopping, please wait.", DateTime.Now.ToString("HH:mm:ss"));
                ScriptEngines.StopMainProgram();
                GameEnvironment.WaitStop().Wait();
                TraceLog.WriteLine("{0} Server has stoped successfully!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnStop error:{0}", ex);
            }
        }

        /// <summary>
        /// Set stop
        /// </summary>
        public void Stop()
        {
            GameEnvironment.IsCanceled = true;
            WaitRunComplated();
            OnStop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void WaitRunComplated()
        {
            runCompleteEvent.WaitOne();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual async System.Threading.Tasks.Task RunAsync()
        {
            try
            {
                if (ScriptEngines.RunMainProgram())
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
            finally
            {
                TraceLog.WriteLine("# Server exit command \"Ctrl+C\" or \"Ctrl+Break\".");
            }

            await RunWait();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async System.Threading.Tasks.Task RunWait()
        {
            while (!GameEnvironment.IsCanceled)
            {
                await System.Threading.Tasks.Task.Delay(1000);
            }
        }
    }
}
