using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Doudizhu.Bll.Action
{
    /// <summary>
    /// 错误日志
    /// </summary>
    public class Action404 : BaseAction
    {
        private string errorInfo = string.Empty;

        public Action404(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action404, httpGet)
        {
        }

        public override bool TakeAction()
        {
            TraceLog.WriteComplement("客户端崩溃日志记录:{0}",errorInfo);
            return true;
        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("errorInfo", ref errorInfo))
            {
                return true;
            }
            return false;
        }
    }
}