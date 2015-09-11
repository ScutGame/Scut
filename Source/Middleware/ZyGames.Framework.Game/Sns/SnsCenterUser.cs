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
using System.Data;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Data;
using ZyGames.Framework.Game.Config;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// Reg type.
    /// </summary>
    public enum RegType
    {
        /// <summary>
        /// 正常形式
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 游客通过设备ID登录
        /// </summary>
        Guest,
        /// <summary>
        /// The other.
        /// </summary>
        Other
    }
    /// <summary>
    /// Pwd type.
    /// </summary>
    public enum PwdType
    {
        /// <summary>
        /// The DE.
        /// </summary>
        DES = 0,
        /// <summary>
        /// The M d5.
        /// </summary>
        MD5
    }

    /// <summary>
    /// SnsCenterUser 的摘要说明
    /// </summary>
    public class SnsCenterUser
    {
        /// <summary>
        /// Md5 key
        /// </summary>
        private const string PasswordMd5Key = "1736e1c9-6f40-48b6-8210-da39cf333784";
        /// <summary>
        /// Passwords the encrypt md5.
        /// </summary>
        /// <returns>The encrypt md5.</returns>
        /// <param name="str">String.</param>
        public static string PasswordEncryptMd5(string str)
        {
            return CryptoHelper.RegUser_MD5_Pwd(str);
        }

        /// <summary>
        /// 官网渠道ID
        /// </summary>
        private const string SysetmRetailID = "0000";
        private int _userid;
        /// <summary>
        /// 获得用户ID
        /// </summary>
        /// 
        public int UserId { get { return _userid; } }
        private string _PassportId = String.Empty;
        private string _PassportPwd = String.Empty;
        private string _imei = String.Empty;
        private BaseLog _Logger = new BaseLog();

        /// <summary>
        /// Gets the passport identifier.
        /// </summary>
        /// <value>The passport identifier.</value>
        public string PassportId
        {
            get { return _PassportId; }
        }
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _PassportPwd; }
        }
        /// <summary>
        /// Gets or sets the retail I.
        /// </summary>
        /// <value>The retail I.</value>
        public string RetailID
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the weixin code.
        /// </summary>
        /// <value>The weixin code.</value>
        public string WeixinCode
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the retail user.
        /// </summary>
        /// <value>The retail user.</value>
        public string RetailUser
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the type of the reg.
        /// </summary>
        /// <value>The type of the reg.</value>
        public RegType RegType
        {
            get;
            set;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.SnsCenterUser"/> class.
        /// </summary>
        public SnsCenterUser()
        {
            RegType = RegType.Other;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPassportId"></param>
        /// <param name="passportPwd"></param>
        /// <param name="imei"></param>
        public SnsCenterUser(string sPassportId, string passportPwd, string imei)
        {
            _PassportId = sPassportId.ToUpper();
            _PassportPwd = passportPwd;
            _imei = imei;
            RegType = RegType.Guest;
            RetailID = SysetmRetailID;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="snsUser"></param>
        /// <returns></returns>
        public bool ValidatePassport(SnsUser snsUser)
        {
            string password = _PassportPwd;

            if (snsUser.RegType == RegType.Normal)
            {
                if (snsUser.PwdType == PwdType.MD5)
                {
                    password = PasswordEncryptMd5(_PassportPwd);
                }
                return snsUser.PassportId == _PassportId && snsUser.Password == password;
            }

            if (snsUser.RegType == RegType.Guest)
            {
                if (snsUser.PwdType == PwdType.MD5)
                {
                    //判断是否已经MD5加密
                    password = PasswordEncryptMd5(_PassportPwd);
                }
                return snsUser.PassportId == _PassportId &&
                    snsUser.Password == password;
            }

            return snsUser.RetailID == RetailID &&
                   snsUser.RetailUser == RetailUser;
        }

        /// <summary>
        /// 是否有绑定DeviceID
        /// </summary>
        /// <returns></returns>
        public SnsUser GetUserByDeviceId(string imei)
        {
            SnsUser snsUser = new SnsUser();
            if (string.IsNullOrEmpty(imei))
            {
                return snsUser;
            }
            SetUserInfo(f =>
            {
                f.Condition = string.Format("RegType = 1 AND {0}", f.FormatExpression("DeviceID")); //Guest
                f.AddParam("DeviceID", imei);
            }, snsUser);
            return snsUser;
        }

        /// <summary>
        /// Inserts the sns user.
        /// </summary>
        /// <returns>The sns user.</returns>
        /// <param name="paramNames">Parameter names.</param>
        /// <param name="paramValues">Parameter values.</param>
        /// <param name="isCustom"></param>
        public int InsertSnsUser(string[] paramNames, string[] paramValues, bool isCustom)
        {
            SnsPassport oSnsPassportLog = new SnsPassport();
            if (!isCustom && !oSnsPassportLog.VerifyRegPassportId(_PassportId))
            {
                return 0;
            }
            //md5加密
            string password = PasswordEncryptMd5(_PassportPwd);

            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Insert);
            command.ReturnIdentity = true;
            command.AddParameter("passportid", _PassportId);
            command.AddParameter("passportpwd", password);
            command.AddParameter("DeviceID", _imei);
            command.AddParameter("RegType", (int)RegType);
            command.AddParameter("RegTime", DateTime.Now);
            command.AddParameter("RetailID", RetailID);
            command.AddParameter("RetailUser", RetailUser);
            command.AddParameter("PwdType", (int)PwdType.MD5);

            if (paramNames != null && paramValues != null)
            {
                for (int i = 0; i < paramNames.Length; i++)
                {
                    command.AddParameter(paramNames[i], paramValues.Length > i ? paramValues[i] : "");
                }
            }
            command.Parser();

            try
            {
                if (!isCustom && !oSnsPassportLog.SetPassportReg(_PassportId))
                {
                    throw new Exception("Set passport  State.Reg fail.");
                }
                using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
                {
                    if (aReader.Read())
                    {
                        _userid = Convert.ToInt32(aReader[0]);
                    }
                }
                return _userid;
            }
            catch (Exception ex)
            {
                _Logger.SaveLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 向社区中心添加用户
        /// </summary>
        /// <returns></returns>
        public int InsertSnsUser(bool isCustom)
        {
            return InsertSnsUser(new string[0], new string[0], isCustom);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isReset">只重置密码</param>
        /// <returns></returns>
        public int ChangePass(string userId, bool isReset = false)
        {
            try
            {
                //md5加密
                string password = PasswordEncryptMd5(_PassportPwd);

                var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Modify);
                command.AddParameter("passportpwd", password);
                if (!isReset)
                {
                    command.AddParameter("RegType", (int)RegType.Normal);
                    command.AddParameter("PwdType", (int)PwdType.MD5);
                }

                command.Filter = ConnectManager.Provider.CreateCommandFilter();

                var section = ConfigManager.Configger.GetFirstOrAddConfig<MiddlewareSection>();
                if (userId.ToUpper().StartsWith(section.PreAccount))
                {
                    command.Filter.Condition = command.Filter.FormatExpression("PassportID");
                    command.Filter.AddParam("PassportID", userId);
                }
                else
                {
                    command.Filter.Condition = command.Filter.FormatExpression("UserID");
                    command.Filter.AddParam("UserID", userId);
                }
                command.Parser();

                return ConnectManager.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
            }
            catch (Exception ex)
            {
                _Logger.SaveLog(ex);
                return 0;
            }
        }


        internal int ChangeUserInfo(string pid, SnsUser snsuser)
        {
            try
            {
                var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Modify);
                if (!string.IsNullOrEmpty(snsuser.Mobile))
                {
                    command.AddParameter("Mobile", snsuser.Mobile);
                }
                if (!string.IsNullOrEmpty(snsuser.Mail))
                {
                    command.AddParameter("Mail", snsuser.Mail);
                }
                if (!string.IsNullOrEmpty(snsuser.RealName))
                {
                    command.AddParameter("RealName", snsuser.RealName);
                }
                if (!string.IsNullOrEmpty(snsuser.IDCards))
                {
                    command.AddParameter("IDCards", snsuser.IDCards);
                }
                if (!string.IsNullOrEmpty(snsuser.ActiveCode))
                {
                    command.AddParameter("ActiveCode", snsuser.ActiveCode);
                }
                if (snsuser.SendActiveDate > DateTime.MinValue)
                {
                    command.AddParameter("SendActiveDate", snsuser.SendActiveDate);
                }
                if (snsuser.ActiveDate > DateTime.MinValue)
                {
                    command.AddParameter("ActiveDate", snsuser.ActiveDate);
                }
                if (!string.IsNullOrEmpty(snsuser.WeixinCode))
                {
                    command.AddParameter("WeixinCode", snsuser.WeixinCode);
                }
                command.Filter = ConnectManager.Provider.CreateCommandFilter();
                command.Filter.Condition = command.Filter.FormatExpression("PassportID");
                command.Filter.AddParam("PassportID", pid);
                command.Parser();

                return ConnectManager.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
            }
            catch (Exception ex)
            {
                _Logger.SaveLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool CheckDevice(string device)
        {
            if (string.IsNullOrEmpty(device))
                return true;

            var command = ConnectManager.Provider.CreateCommandStruct("LimitDevice", CommandMode.Inquiry);
            command.Columns = "COUNT(DeviceID)";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("DeviceID");
            command.Filter.AddParam("DeviceID", device);
            command.Parser();

            int count = Convert.ToInt32(ConnectManager.Provider.ExecuteScalar(CommandType.Text, command.Sql, command.Parameters));
            return count <= 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="passportId"></param>
        /// <returns></returns>
        public SnsUser GetUserInfo(string passportId)
        {
            SnsUser snsUser = new SnsUser();
            SetUserInfo(f =>
            {
                f.Condition = f.FormatExpression("PassportId");
                f.AddParam("PassportId", passportId);
            }, snsUser);
            return snsUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retailID"></param>
        /// <param name="retailUser"></param>
        /// <returns></returns>
        public SnsUser GetUserInfo(string retailID, string retailUser)
        {
            SnsUser snsUser = new SnsUser();
            SetUserInfo(f =>
            {
                f.Condition = string.Format("{0} AND {1}", f.FormatExpression("RetailID"), f.FormatExpression("RetailUser"));
                f.AddParam("RetailID", retailID);
                f.AddParam("RetailUser", retailUser);
            }, snsUser);
            return snsUser;
        }

        /// <summary>
        /// 通过微信号
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public SnsUser GetUserByWeixin(string openId)
        {
            SnsUser snsUser = new SnsUser();
            SetUserInfo(f =>
            {
                f.Condition = f.FormatExpression("WeixinCode");
                f.AddParam("WeixinCode", openId);
            }, snsUser);
            return snsUser;
        }

        private void SetUserInfo(Action<CommandFilter> match, SnsUser snsUser)
        {
            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry);
            command.OrderBy = "USERID ASC";
            command.Columns = "UserId,PassportID,PassportPwd,DeviceID,RegType,RegTime,RetailID,RetailUser,Mobile,Mail,PwdType,RealName,IDCards,ActiveCode,SendActiveDate,ActiveDate,WeixinCode";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            match(command.Filter);
            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    snsUser.UserId = Convert.ToInt32(aReader["UserId"]);
                    snsUser.IMEI = Convert.ToString(aReader["DeviceID"]);
                    snsUser.PassportId = Convert.ToString(aReader["PassportID"]);
                    snsUser.Password = Convert.ToString(aReader["PassportPwd"]);
                    snsUser.RegTime = Convert.ToDateTime(aReader["RegTime"]);
                    snsUser.RetailID = Convert.ToString(aReader["RetailID"]);
                    snsUser.RetailUser = Convert.ToString(aReader["RetailUser"]);
                    snsUser.Mobile = Convert.ToString(aReader["Mobile"]);
                    snsUser.Mail = Convert.ToString(aReader["Mail"]);
                    snsUser.RealName = Convert.ToString(aReader["RealName"]);
                    snsUser.IDCards = Convert.ToString(aReader["IDCards"]);
                    snsUser.ActiveCode = Convert.ToString(aReader["ActiveCode"]);
                    snsUser.SendActiveDate = ToDate(Convert.ToString(aReader["SendActiveDate"]));
                    snsUser.ActiveDate = ToDate(Convert.ToString(aReader["ActiveDate"]));
                    snsUser.WeixinCode = Convert.ToString(aReader["WeixinCode"]);
                    snsUser.PwdType = (PwdType)Enum.ToObject(typeof(PwdType), Convert.ToInt32(aReader["PwdType"]));
                    snsUser.RegType = (RegType)Enum.ToObject(typeof(RegType), Convert.ToInt32(aReader["RegType"]));
                }
            }
        }

        private DateTime ToDate(string str)
        {
            DateTime result;
            DateTime.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Adds the login log.
        /// </summary>
        /// <param name="deviceID">Device I.</param>
        /// <param name="passportID">Passport I.</param>
        public static void AddLoginLog(string deviceID, string passportID)
        {
            if (string.IsNullOrEmpty(deviceID) || string.IsNullOrEmpty(passportID))
            {
                return;
            }
            var command = ConnectManager.Provider.CreateCommandStruct("PassportLoginLog", CommandMode.Insert);
            command.AddParameter("DeviceID", deviceID);
            command.AddParameter("PassportID", passportID);
            command.AddParameter("LoginTime", MathUtils.Now);
            command.Parser();

            ConnectManager.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
        }
    }
}