using System;
using System.Collections.Generic;
using System.Configuration;
using ZyGames.GameService.BaseService.LogService;

namespace ZyGames.OA.WatchService.BLL
{
    public static class WatchScheduler
    {
        private static string[] WatchServiceItems = ConfigurationManager.AppSettings["WatchServiceItem"].Split(new char[] { '#' });
        private static List<BaseWatch> handler = new List<BaseWatch>();

        static WatchScheduler()
        {
            foreach (string item in WatchServiceItems)
            {
                try
                {
                    BaseWatch watch = (BaseWatch)Activator.CreateInstance(Type.GetType(string.Format("ZyGames.OA.WatchService.BLL.Watch.{0}Watch,ZyGames.OA.WatchService.BLL", item)));
                    if (watch != null)
                    {
                        handler.Add(watch);
                    }
                }
                catch (Exception ex)
                {
                    new BaseLog().SaveLog(ex);
                }
            }
        }

        public static List<BaseWatch> WatchList
        {
            get
            {
                return handler;
            }
        }
    }
}
