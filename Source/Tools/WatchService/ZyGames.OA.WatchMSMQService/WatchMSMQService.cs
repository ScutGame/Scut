using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.OA.WatchService.BLL;

namespace ZyGames.OA.WatchMSMQService
{
    partial class WatchMSMQService : ServiceBase
    {
        private static int Interval = Int32.Parse(ConfigurationManager.AppSettings["WatchInterval"]);
        private static BaseLog Logger = new BaseLog("WatchMSMQService");

        public WatchMSMQService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            watchTimer = new System.Timers.Timer();
            //到达时间的时候执行事件；            
            watchTimer.Elapsed += new System.Timers.ElapsedEventHandler(WatchTimer_Tick);
            // 设置引发时间的时间间隔 此处设置为1秒（1000毫秒）
            watchTimer.Interval = Interval;
            //设置是执行一次（false）还是一直执行(true)；
            watchTimer.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件；            
            watchTimer.Enabled = true;
            Logger.SaveLog("Message queue processing service is started!");
        }

        protected override void OnStop()
        {
            watchTimer.Enabled = false;
            Logger.SaveLog("Message queue processing service has been stopped!");
        }
        protected override void OnContinue()
        {
            watchTimer.Enabled = true;
        }

        private void WatchTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //Logger.SaveLog("OA监视服务开始执行...");
                foreach (BaseWatch item in WatchScheduler.WatchList)
                {
                    if ((item.IsForLoop && item.IsElapsed(Interval)) ||
                        (!item.IsForLoop && !item.IsRun))
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(item.Process));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
            }
        }
    }
}
