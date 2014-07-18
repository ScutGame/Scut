using System;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;

namespace GameServer.Script.CsScript.Action
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>继续BaseStruct类:不检查用户合法性请求;AuthorizeAction:有验证合法性</remarks>
    public class Action1002 : BaseStruct
    {
        private string passport = string.Empty;
        private string password = string.Empty;
        private string deviceID = string.Empty;
        private int mobileType = 0;
        private int gameID = 0;
        private string retailID = string.Empty;
        private string clientAppVersion = string.Empty;
        private int ScreenX = 0;
        private int ScreenY = 0;

        public Action1002(ActionGetter actionGetter)
            : base((short) ActionType.Regist, actionGetter)
        {
        }

        /// <summary>
        /// 客户端请求的参数较验
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MobileType", ref mobileType) &&
                httpGet.GetInt("GameID", ref gameID) &&
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

        /// <summary>
        /// 业务逻辑处理
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
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
        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(passport);
            PushIntoStack(password);
        }

    }
}
