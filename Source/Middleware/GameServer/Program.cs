using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Script;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string mainClass = "MainClass";
                ScriptEngines.AddReferencedAssembly("ZyGames.Framework.Game.dll");
                bool result = ScriptEngines.RunMainClass(mainClass);
                Console.WriteLine(result ? "Press any key to exit the listener!" : "Run class " + mainClass + " fail.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TraceLog.WriteError("GameServer main error:{0}", ex);
                Console.ReadKey();
            }
        }
    }
}
