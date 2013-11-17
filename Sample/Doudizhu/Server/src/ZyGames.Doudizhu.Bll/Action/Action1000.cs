using System;

using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Cache;
using ZyGames.Doudizhu.Lang;
using ZyGames.Framework.Cache.Generic;


namespace ZyGames.Doudizhu.Bll.Action
{
    /// <summary>
    /// GM命令接口（提供给OA操作）
    /// </summary>
    public class Action1000 : BaseAction
    {
        private string Cmd;


        public Action1000(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1000, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("Cmd", ref Cmd))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            try
            {
                if (Cmd.ToLower().StartsWith("gm:cache"))
                {
                    CacheFactory.UpdateNotify(true);
                    SaveLog("Gm update cache end.");
                }
                //BaseCommand.Run(ContextUser.UserID, Cmd);
                return true;
            }
            catch (Exception ex)
            {
                this.SaveLog(ex);
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = ex.Message;
                return false;
            }
        }

    }
}
