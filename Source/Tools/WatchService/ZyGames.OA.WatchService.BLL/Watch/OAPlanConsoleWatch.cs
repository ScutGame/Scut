using System;
using System.Configuration;
using System.Net;
using ZyGames.Core.Util;
using ZyGames.OA.WatchService.BLL.Plugin;

namespace ZyGames.OA.WatchService.BLL.Watch
{
    /// <summary>
    /// cpu,driveº‡ ”
    /// </summary>
    public class OAPlanConsoleWatch : BaseWatch
    {
        private static int WatchInterval = Int32.Parse(ConfigurationManager.AppSettings["OAPlan_Interval"]);
        private static string serverUrl = ConfigHelper.GetSetting("OAPlan_Server");
        private string _serverIp;
        private CpuPlanMonitor cpuPlanMonitor;
        private DrivePlanMonitor drivePlanMonitor;
        private MSMQPlanMonitor msmqPlanmonitor;

        public OAPlanConsoleWatch()
        {
            Interval = WatchInterval;
            _serverIp = GetServerIP();
            cpuPlanMonitor = new CpuPlanMonitor(serverUrl, _serverIp);
            drivePlanMonitor = new DrivePlanMonitor(serverUrl, _serverIp);
            msmqPlanmonitor = new MSMQPlanMonitor(serverUrl, _serverIp);
        }

        protected override bool DoProcess(object obj)
        {
            try
            {
                Logger.SaveLog("OA daemon to start monitoring IP:" + _serverIp);

                cpuPlanMonitor.DoMonitor();
                drivePlanMonitor.CheckDriveInfo();
                msmqPlanmonitor.SearchSimplePlanInfo();

                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog("OA monitor error", ex);
                return false;
            }
        }

    }
}
