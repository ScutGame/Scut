using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Common.Security;
using ZyGames.Doudizhu.Lang;

namespace ZyGames.Doudizhu.Bll.Action
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
                password = des.DecodePwd(password, GameEnvironment.ClientDesDeKey);
                if (password.Length > 12 || password.Length < 4)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St1006_PasswordTooLong;
                    return false;
                }
                password = CryptoHelper.DES_Encrypt(password, GameEnvironment.ProductDesEnKey);
                if (SnsManager.ChangePass(Uid, password) <= 0)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1006_ChangePasswordError;
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.SaveLog(ex.ToString());
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1006_PasswordError;
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
