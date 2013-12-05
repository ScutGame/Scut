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
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.Sql;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// 用户中心 - 通行证ID操作类
    /// </summary>
    public class SnsPassport : IDisposable
    {
        private static readonly string PreAccount = ConfigUtils.GetSetting("Sns.PreAccount", "Z");
        /// <summary>
        /// ID的状态
        /// </summary>
        private enum PassMark
        {
            /// <summary>
            /// 未分配下发
            /// </summary>
            UnPush = 0,
            /// <summary>
            /// 已分配下发到新注册用户的请求
            /// </summary>
            IsPushToNewUser,
            /// <summary>
            /// 已被注册
            /// </summary>
            IsReg
        }

        /// <summary>
        /// 获取6位随机密码
        /// </summary>
        /// <returns></returns>
        public string GetRandomPwd()
        {
            Random random = new Random();
            int rid = random.Next(0, 999999);
            return rid.ToString().PadLeft(6, '0');
        }
        /// <summary>
        /// 从DB中加载未被注册的通行证ID
        /// </summary>
        /// <returns></returns>
        public string GetRegPassport()
        {
            bool isGet = false;
            var command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Inquiry, "PASSPORTID");
            command.Top = 1;
            command.OrderBy = "PASSPORTID ASC";
            command.Filter = ConnectManager.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("MARK");
            command.Filter.AddParam("MARK", Convert.ToInt32(PassMark.UnPush));
            command.Parser();

            string iPassportId = String.Empty;
            using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                if (aReader.Read())
                {
                    isGet = true;
                    iPassportId = aReader["PASSPORTID"].ToString();
                }
            }

            if (isGet)
            {
                if (!SetStat(iPassportId, PassMark.IsPushToNewUser))
                {
                    throw new Exception("更新状态出现异常");
                }
                return iPassportId.ToString();
            }
            else
            {
                command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Insert);
                command.ReturnIdentity = true;
                command.AddParameter("mark", Convert.ToInt32(PassMark.IsPushToNewUser));
                command.AddParameter("regpushtime", MathUtils.Now);
                command.Parser();

                using (var aReader = ConnectManager.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
                {
                    if (aReader.Read())
                    {
                        iPassportId = aReader[0].ToString();
                        return PreAccount + iPassportId.ToString();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

        }

        /// <summary>
        /// 检验注册的通行证ID是否在SnsPassportLog列表中。
        /// </summary>
        /// <param name="aPid"></param>
        /// <returns>检测通过，则返回True，否则返回False</returns>
        public bool VerifyRegPassportId(string aPid)
        {
            try
            {
                string sPidPre = aPid.Substring(0, PreAccount.Length).ToUpper();
                if (sPidPre != PreAccount)
                {
                    return false;
                }

                string sTmp = aPid.Substring(PreAccount.Length);
                var command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Inquiry, "passportid");
                command.Top = 1;
                command.OrderBy = "PASSPORTID ASC";
                command.Filter = ConnectManager.Provider.CreateCommandFilter();
                command.Filter.Condition = command.Filter.FormatExpression("PassportId");
                command.Filter.AddParam("PassportId", sTmp);
                command.Parser();
                return ConnectManager.Provider.ExecuteScalar(CommandType.Text, command.Sql, command.Parameters) != null;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="aPid"></param>
        /// <returns></returns>
        public bool SetPassportReg(string aPid)
        {
            return this.SetStat(aPid, PassMark.IsReg);
        }


        private bool SetStat(string aPid, PassMark aMark)
        {
            try
            {
                string sTmp = aPid.Substring(PreAccount.Length);
                var command = ConnectManager.Provider.CreateCommandStruct("SnsPassportLog", CommandMode.Modify);
                command.AddParameter("mark", Convert.ToInt32(aMark));
                if (aMark == PassMark.IsPushToNewUser)
                {
                    command.AddParameter("regpushtime", MathUtils.Now);
                }
                else if (aMark == PassMark.IsReg)
                {
                    command.AddParameter("regtime", MathUtils.Now);
                }
                command.Filter = ConnectManager.Provider.CreateCommandFilter();
                command.Filter.Condition = command.Filter.FormatExpression("PassportId");
                command.Filter.AddParam("PassportId", sTmp);
                command.Parser();

                ConnectManager.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }
}