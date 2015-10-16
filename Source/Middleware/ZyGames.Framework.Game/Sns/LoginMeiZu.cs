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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceModel.Extensions;
using ServiceStack.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Game.Configuration;
using ZyGames.Framework.Game.Context;

namespace ZyGames.Framework.Game.Sns
{
    /// <summary>
    /// AnySDK
    /// </summary>
    public class LoginMeiZu : AbstractLogin
    {
        private string _retailID = string.Empty;
        private string _retailUser = string.Empty;
        private string _token = string.Empty;
        private string AppId = string.Empty;
        private string AppKey = string.Empty;
        private String loginCheckUrl = "http://app.5gwan.com:9000/user/info.php";

        /// <summary>
        /// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Sns.LoginTencent"/> class.
        /// </summary>
        public LoginMeiZu(string retailID, string retailUser, string token)
        {
            _retailID = retailID;
            _retailUser = retailUser;
            _token = token;


            GameChannel gameChannel = ZyGameBaseConfigManager.GameSetting.GetChannelSetting(ChannelType.channelMeiZu);
            if (gameChannel != null)
            {
                GameSdkSetting setting = gameChannel.GetSetting(_retailID);
                if (setting != null)
                {
                    AppKey = setting.AppKey;
                    AppId = setting.AppId;
                }
            }
            else
            {
                TraceLog.ReleaseWrite("The sdkChannelV2 MeiZu section is null.");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetRegPassport()
        {
            return this.PassportID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckLogin()
        {
            try
            {
                string sign = Md5Encode(Md5Encode(AppKey + "_" + _token));
                string queryString = string.Format("{0}?sign={1}&token={2}&app_id={3}", loginCheckUrl, sign, _token, AppId);


                var conn = (HttpWebRequest)WebRequest.Create(queryString);
                conn.Timeout = 2000;
                WebResponse resp = conn.GetResponse();
                Stream stream = resp.GetResponseStream();
                String result ="[]";
                if (stream != null)
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    result = reader.ReadToEnd();
                    stream.Close();
                }
                var rejson = result.ParseJson<Rejson>();
                if (rejson !=null && rejson.state == "1")
                {
                    _retailUser = rejson.data.userid;
                    string[] arr = SnsManager.LoginByRetail(_retailID, _retailUser);
                    this.UserID = arr[0];
                    this.PassportID = arr[1];
                    QihooUserID = _retailUser;
                    SessionID = GetSessionId();
                    return true;

                }

            }
            catch (Exception e)
            {
                new BaseLog().SaveLog(e);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public class Datainfo
        {
            /// <summary>
            /// 玩家ID
            /// </summary>
            public string userid { get; set; }

            /// <summary>
            /// 玩家名称
            /// </summary>
            public string username { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Rejson
        {
            /// <summary>
            /// 状态
            /// </summary>
            public string state { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public Datainfo data { get; set; }

            /// <summary>
            /// 信息
            /// </summary>
            public string message { get; set; }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <returns></returns>
        public static String Md5Encode(String sourceStr)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sourceStr);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] result = md5.ComputeHash(bytes);

                    var sBuilder = new StringBuilder();
                    foreach (byte t in result)
                        sBuilder.Append(t.ToString("x2"));

                    return sBuilder.ToString();
                }
            }
            catch (Exception)
            {
            }
            return null;

        }


    }
}
