package ScutDataLogic
{
	import ScutDataLogic.CInt64;
	import ScutDataLogic.StringUtils;
	import ScutDataLogic.assert;
	import ScutDataLogic.overload;
	import ScutDataLogic.md5.MD5;

	public class CNetWriter
	{
		private static var instance:CNetWriter=null;
		private static var s_Key:String="";
		private static var s_userID:Number=0;
		private static var s_strSessionID:String="";
		private static var s_strSt:String="";
		private static var s_strUrl:String="";
		private static var s_strPostData:String;
		private static var s_strUserData:String;
		private static var s_Counter:int=1;
		private static var s_md5Key:String="44CAC8ED53714BF18D60C5C7B6296000";
		
		
		public function CNetWriter()
		{
			resetData();
		}
		
		public static  function getInstance():CNetWriter
		{
			if(instance==null)
			{
				instance=new CNetWriter();
			}
			return instance;
		}
		
		
		public function Dispose():void
		{
			instance = null;
		}
		
		public function writeInt32( szKey:String, nValue:int ):void
		{
			var szBuf:String="";
			szBuf=StringUtils.Format("&{0}={1}", szKey, nValue);
			s_strUserData += szBuf;
		}
		
		public function writeFloat( szKey:String, fvalue:Number ):void
		{
			var szBuf:String="";
			szBuf=StringUtils.Format("&{0}={1}", szKey, fvalue);
			s_strUserData += szBuf;
		}
		
		public function writeString( szKey:String, szValue:String ):void
		{
			if (szValue == null)
			{
				return ;
			}
			
			var pOut:String=url_encode1(szValue,"",0);
			var szTemp:String="";
			szTemp=StringUtils.Format("&{0}=", szKey);
			s_strUserData	+= szTemp;
			s_strUserData	+= pOut;
			pOut=null;
			
		}
		
		
		public function writeInt64( ...args ):void
		{
			overload(2,[writeInt641,[String,Number],0],[writeInt642,[String,CInt64],0],args);
		}
		
		public function writeInt641( szKey:String, nValue:Number ):void
		{
			var szBuf:String="";
			szBuf=StringUtils.Format("&{0}={1}", szKey, nValue);
			s_strUserData += szBuf;
			
		}
		public function writeInt642( szKey:String, obj:CInt64 ):void
		{
			writeInt641(szKey, obj.getValue());
		}
		
		
		public function writeWord( szKey:String, sValue:int ):void
		{
			var szBuf:String="";
			szBuf=StringUtils.Format("&{0}={1}", szKey, sValue);
			s_strUserData += szBuf;
		}
		public function writeBuf( szKey:String, buf:String, nSize:int ):void
		{
			assert(false);
		}
		
		public static function setUrl(szUrl:String):void
		{
			s_strUrl = szUrl;
		}
		
		public function url_encode(...args):String
		{
			return overload(2,[url_encode1,[String,String,int],0],[url_encode2,[String,int,String,int],0],args);
		}
		
		public function url_encode1( src:String, dst:String, dst_len:int ):String
		{
			return encodeURIComponent(src);
		}
		
		public function url_encode2( src:String, src_len:int, dst:String, dst_len:int ):String
		{
			return encodeURIComponent(src);
		}
		
		public function generatePostData():String
		{
			s_strPostData = s_strUrl;
			s_strPostData += "?d=";
			
			//md5
			var strtemp:String=s_strUserData;
			strtemp += s_md5Key;
			
			var pMd5:String=MD5.hash(strtemp);
			var strSign:String="";
			if (pMd5)
			{
				strSign = pMd5;
				pMd5="";
			}
			s_strUserData += "&sign=";
			s_strUserData += strSign;
			
			
			var pOut:String=url_encode2(s_strUserData,0,"",0);
			s_strPostData += pOut;
			pOut=null;
			
			return s_strPostData;
		}
		
		public static  function resetData():void
		{
			s_strPostData="";
			s_strUserData="";
			
			var szBuf:String="";
			szBuf=StringUtils.Format("MsgId={0}&Sid={1}&Uid={2}&St={3}", s_Counter, s_strSessionID, s_userID, s_strSt);//&codeversion=2
			s_Counter ++;
			s_strUserData += szBuf;
		}
		public static  function setSessionID(pszSessionID:String):void
		{
			if (pszSessionID != null)
			{
				s_strSessionID = pszSessionID;
				resetData();
			}
		}
		public static  function setUserID(value:CInt64):void
		{
			s_userID = value.getValue();
			resetData();
		}
		public static  function setStime(pszTime:String):void
		{
			if (pszTime != null)
			{
				s_strSt = pszTime;
				resetData();
			}
		}
	}
}