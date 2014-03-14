using System;
using ZyGames.Doudizhu.Bll;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Doudizhu.HostServer
{

    class Program
    {
        private static string CharFormat =
            @"/////////////////////////////////////////////////////////////////////////

    //   ) )  //   ) )  //   / / /__  ___/   SCUT Server version {0}
   ((        //        //   / /    / /       Game: {1}   Server: {2}
     \\     //        //   / /    / /        Port: {3}
       ) ) //        //   / /    / /        
((___ / / ((____/ / ((___/ /    / /                http://www.scutgame.com

";
        static void Main(string[] args)
        {
            GameHostApp instance = null;
            string date = DateTime.Now.ToString("HH:mm:ss");
            try
            {
                var setting = new EnvironmentSetting();
                Console.WriteLine(string.Format(CharFormat,
                    "6.1.6.5",
                    setting.ProductCode,
                    setting.ProductServerId,
                    setting.GamePort));
                GameEnvironment.Start(setting);
                instance = new GameHostApp();
                instance.Start(args);
                Console.WriteLine("{0} Server has started successfully!", date);
                Console.WriteLine("# Server is listening...");

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Server failed to start!", date);
                TraceLog.WriteError("HostServer error:{0}", ex);
            }
            finally
            {
                Console.ReadKey();
                if (instance != null) instance.Stop();
            }
        }
    }
}
