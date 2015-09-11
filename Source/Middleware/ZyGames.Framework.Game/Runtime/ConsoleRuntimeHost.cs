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
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Runtime
{
    /// <summary>
    /// Console runtime host
    /// </summary>
    public class ConsoleRuntimeHost : RuntimeHost
    {
        private bool isStop;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStoped { get { return isStop; } }

        /// <summary>
        /// init
        /// </summary>
        public ConsoleRuntimeHost()
        {
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            ConsoleColor currentForeColor = Console.ForegroundColor;
            SetColor(ConsoleColor.DarkYellow);
            OnInit();
            SetColor(currentForeColor);
            if (!OnStart())
            {
                RunWait().Wait();
                return;
            }
            Run();
            if (!IsStoped)
            {
                OnStop();
            }
        }

        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            try
            {
                isStop = true;
                Stop();
                TraceLog.WriteLine("{0} Server has canceled!", DateTime.Now.ToString("HH:mm:ss"));
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnCancelKeyPress error:{1}", ex);
            }
        }

        private void SetColor(ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
            }
            catch { }
        }
    }
}
