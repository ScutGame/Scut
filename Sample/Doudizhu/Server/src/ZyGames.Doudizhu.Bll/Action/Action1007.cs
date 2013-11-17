using System;
using System.Text;
using ZyGames.Doudizhu.Bll.Com.Share;
using ZyGames.Doudizhu.Lang;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Doudizhu.Bll.Action
{
    /// <summary>
    /// 1007_用户检测接口
    /// </summary>
    public class Action1007 : BaseStruct
    {
        private string _nickName = string.Empty;

        public Action1007(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1007, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("NickName", ref _nickName))
            {
                _nickName = _nickName.Trim();
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            try
            {
                var roleFunc = new RoleFunc();
                string msg;
                if (roleFunc.VerifyRange(_nickName, out msg) ||
                    roleFunc.IsExistNickName(_nickName, out msg))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1005_NickNameExist;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                SaveLog(ex);
                return false;
            }
        }

    }
}
