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
using System.Data.SqlClient;
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.Sql;
using ZyGames.Framework.Game.Runtime;

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
        private string _sNickName = String.Empty;
        private string _deviceID = String.Empty;
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
        /// 修改：伍张发
        /// </summary>
        /// <param name="sPassportId"></param>
        /// <param name="passportPwd"></param>
        /// <param name="deviceID"></param>
        public SnsCenterUser(string sPassportId, string passportPwd, string deviceID)
        {
            _PassportId = sPassportId.ToUpper();
            _PassportPwd = passportPwd;
            _deviceID = deviceID;
            RegType = string.IsNullOrEmpty(deviceID) ? RegType.Normal : RegType.Guest;
            RetailID = SysetmRetailID;
        }

        /// <summary>
        /// 增加空密码处理
        /// 修改：伍张发
        /// </summary>
        /// <returns></returns>
        public int GetUserId()
        {
            RegType regType = RegType;
            PwdType pwdType = PwdType.DES;
            SetLoginType(ref regType, ref pwdType, PassportId);

            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry);
            command.OrderBy = "USERID ASC";
            command.Columns = "USERID,PASSPORTID,DEVICEID,REGTYPE,RETAILID,RETAILUSER,WEIXINCODE";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();

            string password = _PassportPwd;

            if (regType == RegType.Normal)
            {
                if (pwdType == PwdType.MD5)
                {
                    password = CryptoHelper.DES_Decrypt(password, GameEnvironment.Setting.ProductDesEnKey);
                    password = PasswordEncryptMd5(password);
                }
                command.Filter.Condition = string.Format("{0} AND {1}",
                        command.Filter.FormatExpression("PassportId"),
                        command.Filter.FormatExpression("PassportPwd")
                    );
                command.Filter.AddParam("PassportId", _PassportId);
                command.Filter.AddParam("PassportPwd", password);
            }
            else if (regType == RegType.Guest)
            {
                if (pwdType == PwdType.MD5)
                {
                    password = CryptoHelper.DES_Decrypt(password, GameEnvironment.Setting.ProductDesEnKey);
                    if (password.Length != 32)
                    {
                        //判断是否已经MD5加密
                        password = PasswordEncryptMd5(password);
                    }
                }

                command.Filter.Condition = string.Format("{0} AND {1} AND {2} AND {3}",
                        command.Filter.FormatExpression("DeviceID"),
                        command.Filter.FormatExpression("PassportPwd"),
                        command.Filter.FormatExpression("PassportId"),
                        command.Filter.FormatExpression("RegType")
                    );
                command.Filter.AddParam("DeviceID", _deviceID);
                command.Filter.AddParam("PassportPwd", password);
                command.Filter.AddParam("PassportId", _PassportId);
                command.Filter.AddParam("RegType", (int)regType);
            }
            else
            {
                command.Filter.Condition = string.Format("{0} AND {1}",
                        command.Filter.FormatExpression("RetailID"),
                        command.Filter.FormatExpression("RetailUser")
                    );
                command.Filter.AddParam("RetailID", RetailID);
                command.Filter.AddParam("RetailUser", RetailUser);
            }

            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                SnsCenterUser user = new SnsCenterUser();
                if (aReader.Read())
                {
                    try
                    {
                        _userid = Convert.ToInt32(aReader["userid"]);
                        _PassportId = aReader["PassportId"].ToString();
                        _deviceID = aReader["DeviceID"].ToNotNullString();
                        RegType = aReader["RegType"].ToEnum<RegType>();
                        RetailID = aReader["RetailID"].ToNotNullString();
                        RetailUser = aReader["RetailUser"].ToNotNullString();
                        WeixinCode = aReader["WeixinCode"].ToNotNullString();
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("GetUserId method error:{0}, sql:{0}", ex, command.Sql);
                    }
                    return _userid;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <returns></returns>
        public RegType GetUserType()
        {
            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry);
            command.OrderBy = "USERID ASC";
            command.Columns = "PassportId,PassportPwd,RegType";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("PassportId");
            command.Filter.AddParam("PassportId", PassportId);
            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    return (RegType)Enum.ToObject(typeof(RegType), Convert.ToInt32(aReader["RegType"]));
                }
            }
            return 0;
        }

        /// <summary>
        /// 是否存在账号
        /// </summary>
        /// <returns></returns>
        public bool IsExist()
        {
            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry);
            command.Columns = "userid";
            command.OrderBy = "USERID ASC";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("PassportId");
            command.Filter.AddParam("PassportId", PassportId);
            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Determines whether this instance is exist retail.
        /// </summary>
        /// <returns><c>true</c> if this instance is exist retail; otherwise, <c>false</c>.</returns>
        public bool IsExistRetail()
        {

            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry);
            command.Columns = "userid";
            command.OrderBy = "USERID ASC";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = string.Format("{0} AND {1}",
                    command.Filter.FormatExpression("RetailID"),
                    command.Filter.FormatExpression("RetailUser")
                );
            command.Filter.AddParam("RetailID", RetailID);
            command.Filter.AddParam("RetailUser", RetailUser);
            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    return true;
                }
            }

            //自动获取通行证
            SnsPassport passport = new SnsPassport();
            this._PassportId = passport.GetRegPassport();
            this._PassportPwd = CryptoHelper.DES_Encrypt(passport.GetRandomPwd(), GameEnvironment.Setting.ProductDesEnKey);
            return false;
        }

        /// <summary>
        /// Sets the type of the login.
        /// </summary>
        /// <param name="regType">Reg type.</param>
        /// <param name="pwdType">Pwd type.</param>
        /// <param name="passportId">Passport identifier.</param>
        public static void SetLoginType(ref RegType regType, ref PwdType pwdType, string passportId)
        {
            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry);
            command.OrderBy = "USERID ASC";
            command.Columns = "RegType,DeviceID,PwdType";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("PassportId");
            command.Filter.AddParam("PassportId", passportId);
            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    string deviceID = Convert.ToString(aReader["DeviceID"]);
                    RegType rt = (RegType)Enum.ToObject(typeof(RegType), Convert.ToInt32(aReader["RegType"]));
                    pwdType = (PwdType)Enum.ToObject(typeof(PwdType), Convert.ToInt32(aReader["PwdType"]));
                    if (rt == RegType.Other && regType != RegType.Other)
                    {
                        //渠道登陆的用户允许更换包登陆
                        regType = string.IsNullOrEmpty(deviceID) ? RegType.Normal : rt;
                    }
                    else
                    {
                        regType = rt;
                    }
                }
            }
        }

        /// <summary>
        /// 是否有绑定DeviceID
        /// </summary>
        /// <returns></returns>
        public static SnsCenterUser GetUserByDeviceID(string deviceID)
        {
            if (deviceID.Length == 0 || deviceID.StartsWith("00:00:00:00:00:"))
            {
                deviceID = Guid.NewGuid().ToString();
            }

            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry);
            command.OrderBy = "USERID ASC";
            command.Columns = "PassportId,PassportPwd,PwdType,RegType";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("DeviceID");
            command.Filter.AddParam("DeviceID", deviceID);
            command.Parser();

            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    PwdType pwdType = (PwdType)Enum.ToObject(typeof(PwdType), Convert.ToInt32(aReader["PwdType"]));
                    string password = Convert.ToString(aReader["PassportPwd"]);
                    if (pwdType == PwdType.MD5)
                    {
                        password = CryptoHelper.DES_Encrypt(password, GameEnvironment.Setting.ProductDesEnKey);
                    }

                    SnsCenterUser user = new SnsCenterUser(Convert.ToString(aReader["PassportId"]), password, deviceID);
                    user.RegType = (RegType)Enum.ToObject(typeof(RegType), Convert.ToInt32(aReader["RegType"]));
                    return user;
                }
            }
            return null;
        }
        /// <summary>
        /// Inserts the sns user.
        /// </summary>
        /// <returns>The sns user.</returns>
        /// <param name="paramNames">Parameter names.</param>
        /// <param name="paramValues">Parameter values.</param>
        public int InsertSnsUser(string[] paramNames, string[] paramValues)
        {
            SnsPassport oSnsPassportLog = new SnsPassport();
            if (!oSnsPassportLog.VerifyRegPassportId(_PassportId))
            {
                return 0;
            }
            //md5加密
            string password = CryptoHelper.DES_Decrypt(_PassportPwd, GameEnvironment.Setting.ProductDesEnKey);
            password = PasswordEncryptMd5(password);

            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Insert);
            command.ReturnIdentity = true;
            command.AddParameter("passportid", _PassportId);
            command.AddParameter("passportpwd", password);
            command.AddParameter("DeviceID", _deviceID);
            command.AddParameter("RegType", (int)RegType);
            command.AddParameter("RegTime", DateTime.Now);
            command.AddParameter("RetailID", RetailID);
            command.AddParameter("RetailUser", RetailUser);
            command.AddParameter("PwdType", (int)PwdType.MD5);
            for (int i = 0; i < paramNames.Length; i++)
            {
                command.AddParameter(paramNames[i], paramValues.Length > i ? paramValues[i] : "");
            }
            command.Parser();

            try
            {
                if (!oSnsPassportLog.SetPassportReg(_PassportId))
                {
                    throw new Exception("SetPassportReg Error");
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
        public int InsertSnsUser()
        {
            return InsertSnsUser(new string[0], new string[0]);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public int ChangePass(string userId)
        {
            try
            {
                //md5加密
                string password = CryptoHelper.DES_Decrypt(_PassportPwd, GameEnvironment.Setting.ProductDesEnKey);
                password = PasswordEncryptMd5(password);

                var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Modify);
                command.AddParameter("passportpwd", password);
                command.AddParameter("RegType", (int)RegType.Normal);
                command.AddParameter("DeviceID", "");
                command.AddParameter("PwdType", (int)PwdType.MD5);

                command.Filter = ConnectManager.Provider.CreateCommandFilter();
                if (userId.ToUpper().StartsWith("Z"))
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
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <returns>The password.</returns>
        /// <param name="passportId">Passport identifier.</param>
        public string GetPassword(string passportId)
        {
            var command = ConnectManager.Provider.CreateCommandStruct("SnsUserInfo", CommandMode.Inquiry, "PassportPwd");
            command.OrderBy = "USERID ASC";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("PassportID");
            command.Filter.AddParam("PassportID", passportId);
            command.Parser();
            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    return Convert.ToString(aReader["PassportPwd"]);
                }
            }
            return string.Empty;
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

        internal static bool CheckDevice(string device)
        {
            if (device == string.Empty)
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

        internal SnsUser GetUserInfo(string passportId)
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
        /// 通过微信号
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        internal SnsUser GetUserByWeixin(string openId)
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
                    snsUser.PassportId = Convert.ToString(aReader["PassportID"]);
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