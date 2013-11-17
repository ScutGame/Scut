using System;
using ZyGames.Core.Util;
using ZyGames.MSMQ.Service.Service;

namespace ZyGames.OA.WatchService.BLL.Watch
{
    /// <summary>
    /// 消息队列管理器
    /// </summary>
    public class MSMQWatch : BaseWatch
    {
        public MSMQWatch()
        {
            IsForLoop = false;
        }

        protected override bool DoProcess(object obj)
        {
            try
            {
                Logger.SaveLog("Message queue listener service start...");
                new MSMQService().Run();
                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog("Message queue listener service error", ex);
                return false;
            }
            finally
            {
                GC.Collect();
            }
        }

    }
}
