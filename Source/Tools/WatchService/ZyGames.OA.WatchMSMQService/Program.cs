using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ZyGames.OA.WatchMSMQService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new WatchMSMQService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
