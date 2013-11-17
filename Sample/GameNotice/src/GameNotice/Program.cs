using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;

namespace GameNotice
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GameHostApp.Current.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TraceLog.WriteError("HostServer error:{0}", ex);
            }
            finally
            {
                Console.WriteLine("Press any key to exit the listener!");
                Console.ReadKey();
                GameHostApp.Current.Stop();
            }
        }
    }
}
