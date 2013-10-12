using System;
using ZyGames.Doudizhu.Bll;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Doudizhu.HostServer
{
    class Program
    {
        [STAThread]
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
