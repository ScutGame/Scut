using System;
using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Bll.Com.Share;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Doudizhu.Script.CsScript.Action
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
                    ErrorCode = Language.Instance.ErrorCode;
                    ErrorInfo = Language.Instance.St1005_NickNameExist;
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
