using System;
using System.Web;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.OA.BLL.Remote.Ddz
{
    /// <summary>
    /// 缓存刷新
    /// </summary>
    public class CacheRemote : GameRemote
    {
        private string _action;
        private string host = "ddz.36you.net";
        private int port = 9700;

        public CacheRemote(HttpContext context)
            : base(context)
        {
        }

        public override void Request(string action)
        {
            _action = action.ToLower();
            switch (_action)
            {
                case "1":
                    RefreshUser();
                    break;
                case "2":
                    RefreshData();
                    break;
                case "3":
                    RefreshConfig();
                    break;
                default:
                    RefreshUser();
                    break;
            }
        }

        private void RefreshConfig()
        {
            string route = "cache.refreshConfig";
            string param = string.Empty;//string.Format("&op={0}", GetParam("NoticeID"));
            DoRequest(host, port, route, param);
        }

        private void RefreshData()
        {
            string route = "cache.refreshData";
            string param = string.Empty;
            DoRequest(host, port, route, param);
        }

        private void RefreshUser()
        {
            string route = "cache.refreshUser";
            string param = string.Format("&personalId={0}", GetParam("GameUserId"));
            DoRequest(host, port, route, param);
        }

       
        protected override void SuccessCallback(MessageStructure writer, MessageHead head)
        {
           
        }

    }
}
