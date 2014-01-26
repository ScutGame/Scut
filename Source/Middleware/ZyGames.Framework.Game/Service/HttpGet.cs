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
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Security;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;
using HttpRequest = System.Web.HttpRequest;

namespace ZyGames.Framework.Game.Service
{

    /// <summary>
    /// HttpGet 的摘要说明
    /// </summary>
    public class HttpGet
    {
        private string _requestParam = string.Empty;
        private StringBuilder _error = new StringBuilder();
        private Dictionary<string, string> _param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpGet(HttpRequest request)
        {
            _paramString = request["d"] ?? "";
            InitData(_paramString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">自定义参数字串</param>
        /// <param name="sessionId"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public HttpGet(string param, string sessionId, string remoteAddress, Action<object, int> callback = null, object state = null)
        {
            _paramString = param ?? "";
            _sessionId = sessionId;
            _remoteAddress = remoteAddress;
            Callback = callback;
            _state = state;
            InitData(_paramString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get { return _param[key]; }
            set { _param[key] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Action<object, int> Callback;

        private readonly object _state;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void LoginSuccessCallback(int userId)
        {
            if (Callback != null)
            {
                Callback(_state, userId);
            }
        }

        private int _actionId;

        /// <summary>
        /// Action编号
        /// </summary>
        public int ActionId
        {
            get { return _actionId; }
        }

        private string _remoteAddress;

        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress
        {
            get { return _remoteAddress; }
        }

        private string _sessionId;

        /// <summary>
        /// 远端SessionId
        /// </summary>
        public string SessionId
        {
            get { return _sessionId; }
        }

        private string _paramString;

        /// <summary>
        /// 参数源字串
        /// </summary>
        public string ParamString
        {
            get { return _paramString; }
        }

        private void InitData(string d)
        {
            int index = d.LastIndexOf("&sign=");
            if (index != -1)
            {
                _requestParam = d.Substring(0, index);
            }
            var temp = HttpUtility.ParseQueryString(d);

            foreach (var key in temp.AllKeys)
            {
                _param[key] = temp[key];
            }
            if (string.IsNullOrEmpty(_sessionId))
            {
                _sessionId = _param.ContainsKey("sid") ? _param["sid"] : "";
            }
            _actionId = (_param.ContainsKey("actionId") ? _param["actionId"] : "0").ToInt();
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
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetIntValue(string param)
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
        public int GetIntValue(string param, int min, int max, bool isRequired = true)
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
        public short GetWordValue(string param)
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
        public short GetWordValue(string param, short min, short max, bool isRequired = true)
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
        public byte GetByteValue(string param)
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
        public byte GetByteValue(string param, byte min, byte max, bool isRequired = true)
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
        public string GetStringValue(string param)
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
        public string GetStringValue(string param, int min, int max, bool isRequired = true)
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
        public int GetInt(string param)
        {
            return GetIntValue(param);
        }

        /// <summary>
        /// 读取short类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public short GetWord(string param)
        {
            return GetWordValue(param);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Byte GetByte(string param)
        {
            return GetByteValue(param);
        }
        /// <summary>
        /// 读取String类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetString(string param)
        {
            return GetStringValue(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public T GetEnum<T>(string param) where T : struct
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
        public bool Contains(string param)
        {
            if (_param.ContainsKey(param))
            {
                return true;
            }
            WriteContainsError(param);
            return false;
        }

        /// <summary>
        /// 读取INT类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public bool GetInt(string aName, ref Int32 rValue)
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
        public bool GetInt(string aName, ref Int32 rValue, Int32 minValue, Int32 maxValue)
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
        public bool GetWord(string aName, ref Int16 rValue)
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
        public bool GetWord(string aName, ref Int16 rValue, Int16 minValue, Int16 maxValue)
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
        /// 读取Byte类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public bool GetByte(string aName, ref Byte rValue)
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
        public bool GetByte(string aName, ref Byte rValue, Byte minValue, Byte maxValue)
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
        public bool GetString(string aName, ref String rValue, bool ignoreError = false)
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
        public bool GetString(string aName, ref String rValue, int minValue, int maxValue, bool ignoreError = false)
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
        public bool GetEnum<T>(string aName, ref T rValue) where T : struct
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
        public bool CheckSign()
        {
            string signKey = GameEnvironment.Setting.ProductSignKey;
            if (string.IsNullOrEmpty(signKey))
            {
                return true;
            }

            string sign = "";
            if (GetString("sign", ref sign))
            {
                if (_requestParam != null)
                {
                    string attachParam = _requestParam + signKey;
                    string key = FormsAuthentication.HashPasswordForStoringInConfigFile(attachParam, "MD5");
                    if (!string.IsNullOrEmpty(key) && key.ToLower() == sign)
                    {
                        return true;
                    }
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

        private void WriteRangOutError(string param, int min, int max)
        {
            if (_error.Length > 0)
            {
                _error.Append(",");
            }
            _error.AppendFormat(Language.Instance.UrlParamOutRange, param, min, max);
        }

    }
}