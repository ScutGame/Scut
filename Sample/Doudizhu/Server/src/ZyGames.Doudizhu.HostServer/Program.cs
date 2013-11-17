using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ProtoBuf;
using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Model;

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
