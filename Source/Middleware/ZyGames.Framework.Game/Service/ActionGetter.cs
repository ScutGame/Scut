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
using ServiceStack.Text;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Script;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// action Parameter
    /// </summary>
    public class ActionGetter
    {
        /// <summary>
        /// 
        /// </summary>
        protected RequestPackage ReqPackage;
        /// <summary>
        /// 
        /// </summary>
        protected GameSession _session;

        /// <summary>
        /// init
        /// </summary>
        /// <param name="package"></param>
        /// <param name="session"></param>
        public ActionGetter(RequestPackage package, GameSession session)
        {
            ReqPackage = package;
            _session = session;
            OpCode = package.OpCode;
        }

        /// <summary>
        /// 
        /// </summary>
        public RequestPackage RequestPackage
        {
            get { return ReqPackage; }
        }

        /// <summary>
        /// get request input bytes, socket use '\r\n\r\n' split message.
        /// </summary>
        public byte[] InputStreamBytes
        {
            get
            {
                return ReqPackage.Message as byte[];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sbyte OpCode { get; set; }

        /// <summary>
        /// MsgId
        /// </summary>
        public virtual int GetMsgId()
        {
            return ReqPackage.MsgId;
        }

        /// <summary>
        /// St
        /// </summary>
        public virtual string GetSt()
        {
            return "st";
        }

        /// <summary>
        /// ActoinId
        /// </summary>
        public virtual int GetActionId()
        {
            return ReqPackage.ActionId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetRouteName()
        {
            return ReqPackage.RouteName;
        }
        /// <summary>
        /// get current UserId.
        /// </summary>
        public virtual int GetUserId()
        {
            return ReqPackage.UserId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetSessionId()
        {
            return SessionId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ProtocolVersion GetPtcl()
        {
            return ReqPackage.Ptcl;
        }

        /// <summary>
        /// get current sessionid.
        /// </summary>
        public string SessionId { get { return _session != null ? _session.SessionId : ""; } }

        /// <summary>
        /// get address for remote
        /// </summary>
        public string RemoteAddress
        {
            get { return _session != null ? _session.RemoteAddress : string.Empty; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GameSession GetSession()
        {
            return Session;
        }
        /// <summary>
        /// get current session.
        /// </summary>
        public GameSession Session
        {
            get { return _session; }
        }

        /// <summary>
        /// Is Runloader tool's param
        /// </summary>
        /// <returns></returns>
        public virtual bool IsRunloader()
        {
            return false;
        }

        /// <summary>
        /// Get custom message
        /// </summary>
        /// <returns></returns>
        public object GetMessage()
        {
            return ReqPackage.Message;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string ToParamString()
        {
            return ReqPackage.ToJson();
        }

        #region httpGet notImplement method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual long GetLongValue(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual long GetLongValue(string param, long min, long max, bool isRequired = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual int GetIntValue(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int GetIntValue(string param, int min, int max, bool isRequired = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual short GetWordValue(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual short GetWordValue(string param, short min, short max, bool isRequired = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual byte GetByteValue(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual byte GetByteValue(string param, byte min, byte max, bool isRequired = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string GetStringValue(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string GetStringValue(string param, int min, int max, bool isRequired = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        [LuaMethod("ActionGetter_GetInt")]
        public virtual int GetInt(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual short GetWord(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Byte GetByte(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string GetString(string param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual T GetEnum<T>(string param) where T : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Contains(string param)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetInt(string aName, ref Int32 rValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetLong(string aName, ref long rValue, long minValue = 0, long maxValue = long.MaxValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetInt(string aName, ref Int32 rValue, Int32 minValue, Int32 maxValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetWord(string aName, ref Int16 rValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetWord(string aName, ref Int16 rValue, Int16 minValue, Int16 maxValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public virtual bool GetInt(string aName, ref uint rValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetInt(string aName, ref uint rValue, uint minValue, uint maxValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetWord(string aName, ref ushort rValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetWord(string aName, ref ushort rValue, ushort minValue, ushort maxValue)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetBool(string aName, ref bool rValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetByte(string aName, ref Byte rValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetByte(string aName, ref Byte rValue, Byte minValue, Byte maxValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetString(string aName, ref String rValue, bool ignoreError = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetString(string aName, ref String rValue, int minValue, int maxValue, bool ignoreError = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool GetEnum<T>(string aName, ref T rValue) where T : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool CheckSign()
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Url:{0}, Uid:{1}, Pid:{2}\r\nHost:{3}",
                ToParamString(),
                Session.UserId,
                Session.User != null ? Session.User.GetPassportId() : "",
                RemoteAddress);
        }
    }
}
