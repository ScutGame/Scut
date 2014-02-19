using System;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{
    public abstract class BaseAction : AuthorizeAction
    {
        protected BaseAction(short actionID, ZyGames.Framework.Game.Contract.HttpGet httpGet)
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
                    actionId == ActionIDDefine.Cst_Action1016 ||
                       actionId == ActionIDDefine.Cst_Action1504 ||
                       actionId == ActionIDDefine.Cst_Action1505 ||
                       actionId == ActionIDDefine.Cst_Action2005 ||
                       actionId == ActionIDDefine.Cst_Action3008 ||
                       actionId == ActionIDDefine.Cst_Action4011 ||
                       actionId == ActionIDDefine.Cst_Action6206 ||
                       actionId == ActionIDDefine.Cst_Action7010;
            }
        }
        /// <summary>
        /// 1000-Gm执行命令
        /// </summary>
        /// <returns></returns>
        private bool CheckGmAction()
        {
            if (actionId == ActionIDDefine.Cst_Action1000 )
            {
                int timeOut = 10;
                try
                {
                    DateTime dateTime = DateTime.ParseExact(Sid, "yyMMddHHmmss", null);
                    bool result = dateTime.AddSeconds(timeOut) > DateTime.Now;
                    return result;
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
                return actionId != ActionIDDefine.Cst_Action1012 &&
                actionId != ActionIDDefine.Cst_Action1412 &&
                actionId != ActionIDDefine.Cst_Action9204;
            }
        }

        /// <summary>
        /// 格式化日期显示，昨天，前天
        /// </summary>
        /// <param name="sendDate"></param>
        /// <returns></returns>
        protected string FormatDate(DateTime sendDate)
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
                    return string.Format("{0} {1}", LanguageManager.GetLang().Date_Yesterday, result);
                }
                if (day == 2)
                {
                    return string.Format("{0} {1}", LanguageManager.GetLang().Date_BeforeYesterday, result);
                }
                return string.Format("{0} {1}", string.Format(LanguageManager.GetLang().Date_Day, day), result);
            }
            return result;
        }
    }

    /****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
}