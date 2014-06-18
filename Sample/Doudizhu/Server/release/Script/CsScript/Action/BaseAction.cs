using System;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{
    public abstract class BaseAction : AuthorizeAction
    {
        protected BaseAction(short actionID, HttpGet httpGet)
            : base(actionID, httpGet)
        {
        }

        /// <summary>
        /// 上下文玩家
        /// </summary>
        public GameUser ContextUser
        {
            get
            {
                return Current.User as GameUser;
            }
        }

        protected override bool IgnoreActionId
        {
            get
            {
                return CheckGmAction() ||
                    actionId == ActionIDDefine.Cst_Action404 ||
                    actionId == ActionIDDefine.Cst_Action1006;
            }
        }
        /// <summary>
        /// 1000-Gm执行命令
        /// </summary>
        /// <returns></returns>
        private bool CheckGmAction()
        {
            if (actionId == ActionIDDefine.Cst_Action1000)
            {
                int timeOut = 10;
                try
                {
                    DateTime dateTime = DateTime.ParseExact(Sid, "yyMMddHHmmss", null);
                    return dateTime.AddSeconds(timeOut) > DateTime.Now;
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        protected override bool IsRefresh
        {
            get
            {
                return true;
            }
        }

        protected override void SaveActionLogToDB(LogActionStat actionStat, string msg)
        {
            if (actionStat == LogActionStat.Fail)
            {
                Console.WriteLine(msg);
            }
            base.SaveActionLogToDB(actionStat, msg);
        }
        /// <summary>
        /// 格式化日期显示，昨天，前天
        /// </summary>
        /// <param name="sendDate"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime sendDate)
        {
            string result = sendDate.ToString("HH:mm:ss");
            if (sendDate.Date == DateTime.Now.Date)
            {
                return result;
            }
            if (DateTime.Now > sendDate)
            {
                TimeSpan timeSpan = DateTime.Now.Date - sendDate.Date;
                int day = (int)Math.Floor(timeSpan.TotalDays);
                if (day == 1)
                {
                    return string.Format("{0} {1}", Language.Instance.Date_Yesterday, result);
                }
                if (day == 2)
                {
                    return string.Format("{0} {1}", Language.Instance.Date_BeforeYesterday, result);
                }
                return string.Format("{0} {1}", string.Format(Language.Instance.Date_Day, day), result);
            }
            return result;
        }
    }
}