using System;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using ZyGames.Core.Util;
using ZyGames.MSMQ.Model;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.OA.WatchService.BLL;
using ZyGames.OA.WatchService.BLL.Watch;

namespace ZyGames.OA.WatchErrorMSMQService
{
    public partial class WatchErrorMSMQService : ServiceBase
    {
        private System.Timers.Timer watchTimer;
        private static int Interval = Int32.Parse(ConfigHelper.GetSetting("WatchInterval"));
        private static BaseLog Logger = new BaseLog("WatchErrorMSMQService");

        public WatchErrorMSMQService()
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
            Logger.SaveLog("Error message queue processing service is started!");
        }

        protected override void OnStop()
        {
            watchTimer.Enabled = false;
            Logger.SaveLog("Error message queue processing service has been stopped!");
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
