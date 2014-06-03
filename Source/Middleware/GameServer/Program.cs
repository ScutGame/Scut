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
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Script;

namespace GameServer
{
    class Program
    {
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
            try
            {
                ConsoleColor currentForeColor = Console.ForegroundColor;
                var setting = new EnvironmentSetting();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(string.Format(CharFormat,
                    Assembly.GetExecutingAssembly().GetName().Version,
                    setting.ProductCode,
                    setting.ProductServerId,
                    setting.GamePort));
                GameEnvironment.Start(setting);
                Console.ForegroundColor = currentForeColor;
                if (ScriptEngines.RunMainProgram(args))
                {
                    Console.WriteLine("{0} Server has started successfully!", DateTime.Now.ToString("HH:mm:ss"));
                    Console.WriteLine("# Server is listening...");
                }
                else
                {
                    Console.WriteLine("{0} Server failed to start!", DateTime.Now.ToString("HH:mm:ss"));
                }
                Console.ReadKey();
                ScriptEngines.StopMainProgram();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Server failed to start!", DateTime.Now.ToString("HH:mm:ss"));
                TraceLog.WriteError("Server failed to start error:{0}", ex);
                Console.ReadKey();
            }
        }

    }
}
