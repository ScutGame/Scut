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
using System.Linq;
using System.Text;
using System.Web;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Framework.Game.Contract
{

    /// <summary>
    /// HttpGet
    /// </summary>
    public class HttpGet : ActionGetter
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ParamSid = "Sid";
        /// <summary>
        /// 
        /// </summary>
        public const string ParamMsgId = "MsgId";
        /// <summary>
        /// 
        /// </summary>
        public const string ParamActionId = "ActionId";
        /// <summary>
        /// 
        /// </summary>
        public const string ParamUid = "Uid";
        /// <summary>
        /// 
        /// </summary>
        public const string ParamSt = "St";
        /// <summary>
        /// 
        /// </summary>
        public const string ParamPtcl = "Ptcl";

        private string _originalParam = string.Empty;
        private StringBuilder _error = new StringBuilder();
        private Dictionary<string, string> _param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpGet(HttpRequest request)
            : base(null, null)
        {
            _paramString = request["d"] ?? "";
            InitData(_paramString);
            //http
            string sessionId = "";
            if (_param.ContainsKey(ParamSid))
            {
                sessionId = _param[ParamSid];
            }
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = request[ParamSid];
            }
            _session = GameSession.Get(sessionId) ?? GameSession.CreateNew(Guid.NewGuid(), request);

            //set cookie
            var cookie = request.Cookies.Get(ParamSid);
            if (cookie == null && HttpContext.Current != null)
            {
                cookie = new HttpCookie(ParamSid, SessionId);
                cookie.Expires = DateTime.Now.AddMinutes(5);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="session"></param>
        public HttpGet(RequestPackage package, GameSession session)
            : base(package, session)
        {
            _param = package.Params;
            _paramString = package.OriginalParam ?? "";

            RemoveSignKey(_paramString);
            SetParams();
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<string> Keys
        {
            get { return _param.Keys; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetCustomKeys()
        {
            return _param.Keys.Where(k =>
                !string.Equals(k, ParamSid, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(k, ParamMsgId, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(k, ParamActionId, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(k, ParamUid, StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(k, ParamSt, StringComparison.OrdinalIgnoreCase)
                ).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override string this[string key]
        {
            get { return _param[key]; }
            set { _param[key] = value; }
        }

        /// <summary>
        /// MsgId
        /// </summary>
        public int MsgId { get; private set; }

        private int _actionId;
        private ProtocolVersion _ptcl = ProtocolVersion.Default;

        /// <summary>
        /// Action编号
        /// </summary>
        public int ActionId
        {
            get { return _actionId; }
        }
        
        private string _paramString;

        /// <summary>
        /// 参数源字串
        /// </summary>
        public string ParamString
        {
            get { return _paramString; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetSt()
        {
            return _param.ContainsKey(ParamSt) ? _param[ParamSt] : string.Empty;
        }

        public override ProtocolVersion GetPtcl()
        {
            return _ptcl;
        }

        /// <summary>
        /// close session
        /// </summary>
        public void CloseSession()
        {
            if (_session != null)
            {
                _session.Close();
            }
        }

        private void InitData(string d)
        {
            RemoveSignKey(d);
            var temp = HttpUtility.ParseQueryString(d);

            foreach (var key in temp.AllKeys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                _param[key] = temp[key];
            }
            SetParams();
        }

        private void SetParams()
        {
            MsgId = (_param.ContainsKey(ParamMsgId) ? _param[ParamMsgId] : "0").ToInt();
            _actionId = (_param.ContainsKey(ParamActionId) ? _param[ParamActionId] : "0").ToInt();
            _ptcl = (_param.ContainsKey(ParamPtcl) ? _param[ParamPtcl] : "0").ToEnum<ProtocolVersion>();
        }

        private void RemoveSignKey(string d)
        {
            int startIndex = d.IndexOf("?d=", StringComparison.OrdinalIgnoreCase);
            if (startIndex > -1)
            {
                d = d.Substring(startIndex + 3);
                d = HttpUtility.UrlDecode(d);
            }

            int index = d.LastIndexOf("&sign=", StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                _originalParam = d.Substring(0, index);
            }
        }

        private const int ZeroNum = 0;

        /// <summary>
        /// 是否有出错
        /// </summary>
        public bool HasError
        {
            get { return _error.Length > 0; }
        }
        /// <summary>
        /// 出错信息
        /// </summary>
        public string ErrorMsg
        {
            get { return _error.ToString(); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToParamString()
        {
            return ParamString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override long GetLongValue(string param)
        {
            return GetLongValue(param, ZeroNum, long.MaxValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        public override long GetLongValue(string param, long min, long max, bool isRequired = true)
        {
            long value = 0;
            if (!GetLong(param, ref value, min, max) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override int GetIntValue(string param)
        {
            return GetIntValue(param, ZeroNum, int.MaxValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public override int GetIntValue(string param, int min, int max, bool isRequired = true)
        {
            int value = 0;
            if (!GetInt(param, ref value, min, max) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override short GetWordValue(string param)
        {
            return GetWordValue(param, ZeroNum, short.MaxValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public override short GetWordValue(string param, short min, short max, bool isRequired = true)
        {
            short value = 0;
            if (!GetWord(param, ref value, min, max) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override byte GetByteValue(string param)
        {
            return GetByteValue(param, ZeroNum, byte.MaxValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public override byte GetByteValue(string param, byte min, byte max, bool isRequired = true)
        {
            byte value = 0;
            if (!GetByte(param, ref value, min, max) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override string GetStringValue(string param)
        {
            return GetStringValue(param, ZeroNum, -1, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public override string GetStringValue(string param, int min, int max, bool isRequired = true)
        {
            string value = "";
            if (!GetString(param, ref value, min, max, true) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value length out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }


        /// <summary>
        /// 读取INT类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override int GetInt(string param)
        {
            return GetIntValue(param);
        }

        /// <summary>
        /// 读取short类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override short GetWord(string param)
        {
            return GetWordValue(param);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override Byte GetByte(string param)
        {
            return GetByteValue(param);
        }
        /// <summary>
        /// 读取String类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override string GetString(string param)
        {
            return GetStringValue(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public override T GetEnum<T>(string param)
        {
            T value = default(T);
            GetEnum(param, ref value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal bool ContainsKey(string name)
        {
            return _param.ContainsKey(name);
        }

        /// <summary>
        /// 检查是否包括指定参数，有参数出错记录
        /// </summary>
        /// <param name="param">参数名</param>
        /// <returns></returns>
        public override bool Contains(string param)
        {
            if (_param.ContainsKey(param))
            {
                return true;
            }
            WriteContainsError(param);
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool GetLong(string aName, ref long rValue, long minValue = 0, long maxValue = long.MaxValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                result = long.TryParse(_param[aName], out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取INT类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public override bool GetInt(string aName, ref Int32 rValue)
        {
            return GetInt(aName, ref rValue, ZeroNum, Int32.MaxValue);
        }
        /// <summary>
        /// 读取INT类型的请求参数,验证值的取值范围
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">取值最小范围</param>
        /// <param name="maxValue">取值最大范围</param>
        /// <returns></returns>
        public override bool GetInt(string aName, ref Int32 rValue, Int32 minValue, Int32 maxValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                result = Int32.TryParse(_param[aName], out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取short类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public override bool GetWord(string aName, ref Int16 rValue)
        {
            return GetWord(aName, ref rValue, ZeroNum, Int16.MaxValue);
        }
        /// <summary>
        /// 读取short类型的请求参数
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">取值最小范围</param>
        /// <param name="maxValue">取值最大范围</param>
        /// <returns></returns>
        public override bool GetWord(string aName, ref Int16 rValue, Int16 minValue, Int16 maxValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                result = Int16.TryParse(_param[aName], out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public override bool GetInt(string aName, ref uint rValue)
        {
            return GetInt(aName, ref rValue, ZeroNum, uint.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool GetInt(string aName, ref uint rValue, uint minValue, uint maxValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                result = uint.TryParse(_param[aName], out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, (int)minValue, (int)maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool GetWord(string aName, ref ushort rValue)
        {
            return GetWord(aName, ref rValue, ZeroNum, ushort.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool GetWord(string aName, ref ushort rValue, ushort minValue, ushort maxValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                result = ushort.TryParse(_param[aName], out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool GetBool(string aName, ref bool rValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                if ("1".Equals(_param[aName]))
                {
                    rValue = true;
                    result = true;
                }
                else if ("0".Equals(_param[aName]))
                {
                    rValue = false;
                    result = true;
                }
                else
                {
                    result = bool.TryParse(_param[aName], out rValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }


        /// <summary>
        /// 读取Byte类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public override bool GetByte(string aName, ref Byte rValue)
        {
            return GetByte(aName, ref rValue, ZeroNum, Byte.MaxValue);
        }
        /// <summary>
        /// 读取Byte类型的请求参数
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">取值最小范围</param>
        /// <param name="maxValue">取值最大范围</param>
        /// <returns></returns>
        public override bool GetByte(string aName, ref Byte rValue, Byte minValue, Byte maxValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                result = Byte.TryParse(_param[aName], out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取String类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <param name="ignoreError"></param>
        /// <returns></returns>
        public override bool GetString(string aName, ref String rValue, bool ignoreError = false)
        {
            return GetString(aName, ref rValue, ZeroNum, -1, ignoreError);
        }

        /// <summary>
        /// 读取String类型的请求参数
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">长度最小范围</param>
        /// <param name="maxValue">长度最大范围，-1:不限制</param>
        /// <param name="ignoreError"></param>
        /// <returns></returns>
        public override bool GetString(string aName, ref String rValue, int minValue, int maxValue, bool ignoreError = false)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                rValue = _param[aName].Trim();
                result = rValue.Length >= minValue && (maxValue < 0 || rValue.Length <= maxValue);
                if (!result && !ignoreError)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else if (!ignoreError)
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取枚举类型的请求参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public override bool GetEnum<T>(string aName, ref T rValue)
        {
            bool result = false;
            if (_param.ContainsKey(aName))
            {
                result = true;
                try
                {
                    rValue = (T)Enum.Parse(typeof(T), _param[aName].Trim());
                }
                catch
                {
                    WriteContainsError(aName);
                    result = false;
                }
            }
            else
            {
                WriteContainsError(aName);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 签名检查
        /// </summary>
        /// <returns></returns>
        public override bool CheckSign()
        {
            string signKey = GameEnvironment.Setting != null ? GameEnvironment.Setting.ProductSignKey : "";
            if (string.IsNullOrEmpty(signKey))
            {
                return true;
            }

            string sign = "";
            if (GetString("sign", ref sign))
            {
                string attachParam = _originalParam + signKey;
                string key = ZyGames.Framework.Common.Security.CryptoHelper.MD5_Encrypt(attachParam, Encoding.UTF8);
                if (!string.IsNullOrEmpty(key) && key.ToLower() == sign)
                {
                    return true;
                }
            }

            return false;
        }


        private void WriteContainsError(string param)
        {
            if (_error.Length > 0)
            {
                _error.Append(",");
            }
            _error.AppendFormat(Language.Instance.UrlNoParam, param);
        }

        private void WriteRangOutError(string param, long min, long max)
        {
            if (_error.Length > 0)
            {
                _error.Append(",");
            }
            _error.AppendFormat(Language.Instance.UrlParamOutRange, param, min, max);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetMsgId()
        {
            return MsgId;
        }

        //public override string GetSt()
        //{
        //    return GetStringValue(ParamSt);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetActionId()
        {
            return _actionId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetUserId()
        {
            return Session != null ? Session.UserId : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsRunloader()
        {
            return ContainsKey("rl");
        }
    }
}