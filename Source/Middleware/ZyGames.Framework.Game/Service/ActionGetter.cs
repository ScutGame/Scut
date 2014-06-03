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
        /// init
        /// </summary>
        /// <param name="package"></param>
        public ActionGetter(RequestPackage package)
        {
            ReqPackage = package;
        }

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
        /// get current UserId.
        /// </summary>
        public virtual int GetUserId()
        {
            return ReqPackage.UserId;
        }

        /// <summary>
        /// get current sessionid.
        /// </summary>
        public virtual string GetSessionId()
        {
            return ReqPackage.SessionId;
        }

        /// <summary>
        /// get current session.
        /// </summary>
        public virtual GameSession GetSession()
        {
            return ReqPackage.Session;
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
    }
}
