using System;
using System.IO;
using System.Collections.Generic;
using ZyGames.Doudizhu.Bll;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Doudizhu.HostServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
				GameHostApp.Current.Start();
				TraceLog.ReleaseWrite("The server is started!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TraceLog.WriteError("HostServer error:{0}", ex);
            }
            finally
            {
                Console.WriteLine("Press Enter to exit the listener!");
                Console.ReadLine();
//				GameHostApp.Current.Stop();
//				TraceLog.ReleaseWrite("The server is stoped!");
            }
        }
    }
}
