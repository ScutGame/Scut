using System;
using ZyGames.Doudizhu.Bll;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{
    /// <summary>
    /// 获取通行证接口
    /// </summary>
    public class Action1002 : BaseStruct
    {
        private string passport = string.Empty;
        private string password = string.Empty;
        private string deviceID = string.Empty;
        private int mobileType = 0;
        private int gameType = 0;
        private string retailID = string.Empty;
        private string clientAppVersion = string.Empty;
        private int ScreenX = 0;
        private int ScreenY = 0;

        public Action1002(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1002, httpGet)
        {
        }

        public override void BuildPacket()
        {
            PushIntoStack(passport);
            PushIntoStack(password);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MobileType", ref mobileType) &&
                httpGet.GetInt("GameType", ref gameType) &&
                httpGet.GetString("RetailID", ref retailID) &&
                httpGet.GetString("ClientAppVersion", ref clientAppVersion) &&
                httpGet.GetString("DeviceID", ref deviceID))
            {
                httpGet.GetInt("ScreenX", ref ScreenX);
                httpGet.GetInt("ScreenY", ref ScreenY);
            }
            else
            {
                return false;
            }
            return true;
        }

        public override bool TakeAction()
        {
            try
            {
                string[] userList = SnsManager.GetRegPassport(deviceID);
                passport = userList[0];
                password = CryptoHelper.DES_Decrypt(userList[1], GameEnvironment.Setting.ProductDesEnKey);
                return true;
            }
            catch (Exception ex)
            {
                this.SaveLog(ex);
                this.ErrorCode = Language.Instance.ErrorCode;
                this.ErrorInfo = Language.Instance.St1002_GetRegisterPassportIDError;
                return false;
            }
        }
    }
}
