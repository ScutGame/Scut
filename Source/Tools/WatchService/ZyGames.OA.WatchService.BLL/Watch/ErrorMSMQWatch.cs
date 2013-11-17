using System;
using ZyGames.Core.Util;
using ZyGames.MSMQ.Service.Service;

namespace ZyGames.OA.WatchService.BLL.Watch
{
    /// <summary>
    /// Error消息队列管理器
    /// </summary>
    public class ErrorMSMQWatch : BaseWatch
    {
        public ErrorMSMQWatch()
        {
            IsForLoop = false;
        }

        protected override bool DoProcess(object obj)
        {
            try
            {
                Logger.SaveLog("Error message queue listener service start...");
                new MSMQService().RunError();
                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog("Error message queue listener service error", ex);
                return false;
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
