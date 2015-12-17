package ScutDataLogic
{
	import flash.utils.ByteArray;

	public class CNetReader extends CNetStreamExport
	{
		
		private static var instance:CNetReader=null;
		private var m_nResult:int;
		private var m_nRmId:int;
		private var m_nActionID:int;
		private var m_pStrErrMsg:CLuaString;
		private var m_pStrStime:CLuaString;
		
		
		public function CNetReader()
		{
			super();
			
			m_nResult	= 0;
			m_nRmId		= 0;
			m_nActionID	= 0;
			m_pStrStime = new CLuaString();
			m_pStrErrMsg= new CLuaString();
		}
		
		public static function getInstance():CNetReader
		{
			if(instance==null)
			{
				instance=new CNetReader();
			}
			return instance;
		}
		
		override public function Dispose():void
		{
			if (m_pStrStime)
			{
				m_pStrStime = null;
			}
			if (m_pStrErrMsg)
			{
				m_pStrErrMsg = null;
			}
			instance = null;
		}
		
		
		/**
		 * CNetReader  字节流读取
		 *  读取了基本响应体
		 * @param pdataStream  服务器下发的字节流
		 * @param wSize
		 * @return 
		 * 
		 */		
		override public function pushNetStream(pdataStream:ByteArray, wSize:int):Boolean
		{
			//传入的包大小与包前两个字节的值不一样，返回FALSE。
			if (!super.pushNetStream(pdataStream, wSize))
			{
				trace("pushNetStream return false");
				return false;
			}
			
			//读取协议相应结果
			m_nResult = getInt();
			//读取西医mId
			m_nRmId = getInt();
			
			
			//读取回应消息  【先读取消息长度 然后读取字符串】
			var nErrorMsgSize:int = getInt();
			if (nErrorMsgSize)
			{
				getString(m_pStrErrMsg, nErrorMsgSize);
			}
			else
			{
				m_pStrErrMsg.setString("");
			}
			
			//读取协议ID
			m_nActionID= getInt();
			
			//读取服务端自定义数据
			
			var nStSize:int = getInt();
			if (nStSize)
			{
				getString(m_pStrStime, nStSize);
				CNetWriter.setStime(m_pStrStime.getCString());
			}
			return true;
		}
		
		
		override public function getString(...args):*
		{
			overload(1,[getStringNetReader,[CLuaString,int],0],args);
			return null;
		}
		
		public function getStringNetReader(pOutString:CLuaString, nLength:int):void
		{
//			if (nLength <= 0)
//			{
//				return;
//			}
			pOutString.setString(super.getString(nLength));
			super.freeStringBuffer();
		}
		
		public function readInt64():CInt64
		{
			var tempValue:Number=getInt64();
			return new CInt64(tempValue);
			
//			return CInt64(getInt64());
//			return getInt64();
		}
		
		/**
		 * 返回协议请求结果 
		 * @return 
		 * 
		 */		
		public function getResult():int
		{
			return m_nResult;
		}
		public function getRmId():int
		{
			return m_nRmId;
		}
		public function getActionID():int
		{
			return m_nActionID;
		}
		public function readErrorMsg():CLuaString
		{
			return m_pStrErrMsg;
		}
		
		public function getStrStime():CLuaString
		{
			return m_pStrStime;
		}
		public function readASString():String
		{
			var nLen:int=this.getInt();
			var str:CLuaString=new CLuaString("default LuaString");
			this.getString(str,nLen);
			return str.getCString();
		}
		
		public function readString():String
		{
			var nLen:int = this.getInt();
			var strRet:String = null;
			if(nLen != 0){
				var str:CLuaString=new CLuaString("default LuaString");
				this.getString(str,nLen);
			}
			return str.getCString();
		}
	}
}