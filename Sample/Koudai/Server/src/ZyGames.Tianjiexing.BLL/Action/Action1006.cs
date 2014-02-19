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
using System;
using System.Text.RegularExpressions;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Net;
using ZyGames.Framework.Common.Security;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;



namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 更新密码
    /// </summary>
    public class Action1006 : BaseAction
    {
        private string password = string.Empty;

        public Action1006(ZyGames.Framework.Game.Contract.HttpGet httpGet)
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
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St1006_PasswordTooLong;
                    return false;
                }
                Regex re = new Regex(@"^[\u4e00-\u9fa5\w]+$");
                if (!re.IsMatch(password))
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1006_PasswordExceptional;
                    return false;
                }
                password = CryptoHelper.DES_Encrypt(password, GameEnvironment.Setting.ProductDesEnKey);
                if (SnsManager.ChangePass(Uid, password) <= 0)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1006_ChangePasswordError;
                    return false;
                }
                else
                {
                    UserOperationLog userOperationLog = new UserOperationLog();
                    userOperationLog.UserID = ContextUser.UserID;
                    userOperationLog.ActionID = ActionIDDefine.Cst_Action1006;
                    userOperationLog.FunctionID = "更新密码";
                    userOperationLog.CreateDate = DateTime.Now;
                    userOperationLog.Num = 0;

                    var sender = DataSyncManager.GetDataSender();
                    sender.Send(userOperationLog);
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