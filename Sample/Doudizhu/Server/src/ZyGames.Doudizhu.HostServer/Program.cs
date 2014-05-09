using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
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
                var fc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                var setting = new EnvironmentSetting();
                setting.ClientDesDeKey = "j6=9=1ac";
                setting.EntityAssembly = Assembly.Load("ZyGames.Doudizhu.Model");

                Console.WriteLine(string.Format(CharFormat,
                    "6.3.7.500",
                    setting.ProductCode,
                    setting.ProductServerId,
                    setting.GamePort));
                GameEnvironment.Start(setting);
                Console.ForegroundColor = fc;
                instance = new GameHostApp();
                instance.Start(args);
                Console.WriteLine("{0} Server has started successfully!", date);
                Console.WriteLine("# Server is listening...");
                //Task.Factory.StartNew(() =>
                //{
                //    while (true)
                //    {
                //        var cacheSet = new GameDataCacheSet<GameUser>();
                //        var user = cacheSet.FindKey("1380001");
                //        if (user != null && user.Property.OnLine)
                //        {
                //            TraceLog.ReleaseWriteDebug("user:{0},tabls:{1},room:{2}", user.UserId, user.Property.TableId, user.Property.RoomId);
                //        }
                //        Thread.Sleep(1000);
                //    }
                //});
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
