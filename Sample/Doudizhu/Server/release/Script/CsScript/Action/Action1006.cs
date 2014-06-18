using System;
using ZyGames.Doudizhu.Bll;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Sns;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{
    /// <summary>
    /// 1006_密码更新接口
    /// </summary>
    public class Action1006 : BaseAction
    {
        private string password = string.Empty;

        public Action1006(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1006, httpGet)
        {
        }

        public override bool TakeAction()
        {
            try
            {
                DESAlgorithmNew des = new DESAlgorithmNew();
                password = des.DecodePwd(password, GameEnvironment.Setting.ClientDesDeKey);
                if (password.Length > 12 || password.Length < 4)
                {
                    this.ErrorCode = Language.Instance.ErrorCode;
                    this.ErrorInfo = Language.Instance.St1006_PasswordTooLong;
                    return false;
                }
                password = CryptoHelper.DES_Encrypt(password, GameEnvironment.Setting.ProductDesEnKey);
                if (SnsManager.ChangePass(Uid, password) <= 0)
                {
                    this.ErrorCode = Language.Instance.ErrorCode;
                    ErrorInfo = Language.Instance.St1006_ChangePasswordError;
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.SaveLog(ex.ToString());
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.St1006_PasswordError;
                return false;
            }
            return true;
        }

        public override void BuildPacket()
        {
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("password", ref password))
            {
                return true;
            }
            return false;
        }
    }
}
