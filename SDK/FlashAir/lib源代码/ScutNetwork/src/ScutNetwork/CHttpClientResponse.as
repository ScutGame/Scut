package ScutNetwork
{
	import flash.utils.ByteArray;

	public class CHttpClientResponse
	{
		
		private var m_nDataSize:uint;
		private var m_pData:ByteArray;
		private var m_cContentType:String;
		private var m_cTargetFile:String;
		private var m_pszRequestUrl:String;
		private var m_pszLastResponseUrl:String;
		private var m_pTarget:CStream;
		private var m_bUseDataResume:Boolean;
		private var m_nRawTargetSize:int;
		private var m_nStatusCode:int;
		
		public function CHttpClientResponse()
		{
			m_pData = null;	
			m_pTarget= new CStream();
			m_nDataSize = 0;
			m_bUseDataResume = false;
			m_nStatusCode = 200;
			m_cTargetFile="";
			m_cContentType="";
			m_pszRequestUrl = null;
			m_pszLastResponseUrl = null;
			Reset();
		}
		
		
		public function GetBodyPtr():ByteArray
		{
			return m_pData;
		}
		
		public function GetBodyLength():uint
		{
			return m_nDataSize;
		}
		
		public function DataContains(searchStr:String):Boolean
		{
			if(m_pData == null)
			{
				return false;
			}
			
			if(m_pData.toString().indexOf(searchStr) == -1)
			{
				return false;
			}
//			if(strstr(m_pData, searchStr) == null)
//			{
//				return false;
//			}
			
			return true;
		}
		
		public function ContentTypeContains(searchStr:String):Boolean
		{
			if(m_cContentType == null)
				return false;
			
			
			if(m_cContentType.indexOf(searchStr) == -1)
			{
				return false;
			}
//			if(strstr(m_cContentType, searchStr) == null)
//			{
//				return false;
//			}
			
			return true;
		}
		
		
		public function Reset():void
		{
			m_cContentType="";
			if (m_pszRequestUrl)
			{
//				free(m_pszRequestUrl);
				m_pszRequestUrl = null;
			}
			if (m_pszLastResponseUrl)
			{
//				free(m_pszLastResponseUrl);
				m_pszLastResponseUrl = null;
			}
//			if(m_pData != null)
//			{
//				delete [] m_pData;
//			}
			m_pData		= null;
			m_nDataSize     = 0;
			m_bUseDataResume = false;
		}
		
		public function SetContentType(contentType:String):void
		{
			if(contentType!=null)
			{
				m_cContentType.concat(contentType.substr(0,m_cContentType.length));
			}
//			if (contentType!=null)
//				strncpy(m_cContentType, contentType, sizeof(m_cContentType));
		}
		
		public function GetTargetFile():String
		{
			return m_cTargetFile;
		}
		
		public function SetTargetFile(pszFileName:String):void
		{
			m_cTargetFile="";
			m_cTargetFile.concat(pszFileName.substr(0,pszFileName.length));
//			memset(m_cTargetFile, 0, sizeof(m_cTargetFile));
//			memcpy(m_cTargetFile, pszFileName, strlen(pszFileName));
		}
		
		public function GetTarget():CStream
		{
//			if (!m_pTarget && m_cTargetFile[0] != 0)
//			{
//				var fs:CFileStream = new CFileStream();
////				#ifdef ND_UPHONE
////				if (fs.Open(m_cTargetFile, PO_CREAT | PO_RDWR | PO_BINARY, PS_IREAD | PS_IWRITE) == 0)
//				char chMode[4] = {0};
//				if (m_bUseDataResume) //使用断点续传
//				{
//					strcpy(chMode, "ab+");
//				}else{
//					strcpy(chMode, "wb+");		
//				}
//				
//				if (fs.Open(m_cTargetFile, 0, 0, chMode) == 0)
//				{
//					m_pTarget = fs;			
//					m_nRawTargetSize = m_pTarget.GetSize();
//				}else{
//					delete fs;
//				}
//			}
			return m_pTarget;
		}
		
		
		//注意：为了方便使用，这边传入的流指针将会被管理（根据需要进行释放）
		public function SetTarget(pTarget:CStream):void
		{
			if (m_pTarget)
			{
				m_pTarget = null;
			}	
			m_pTarget = pTarget;
			if (m_pTarget)
				m_nRawTargetSize = m_pTarget.GetSize();
		}
		
		//是否使用断点续传
		public function GetUseDataResume():Boolean
		{
			return m_bUseDataResume;
		}
		public function SetUseDataResume(bUse:Boolean):void
		{
			m_bUseDataResume = bUse;
		}
		
		public function GetRequestUrl():String
		{
			return m_pszRequestUrl;
		}
		public function SetRequestUrl(pszUrl:String):void
		{
			if (pszUrl)
			{
				if (m_pszRequestUrl)
				{
					m_pszRequestUrl="";
				}		
				m_pszRequestUrl = pszUrl;
			}
		}
		
		public function GetLastResponseUrl():String
		{
			return m_pszLastResponseUrl;
		}
		public function SetLastResponseUrl(pszUrl:String):void
		{
			if (pszUrl)
			{
				if (m_pszLastResponseUrl)
				{
					m_pszLastResponseUrl="";
				}
				m_pszLastResponseUrl = pszUrl;
			}
		}

		public function GetTargetRawSize():int
		{
			return m_nRawTargetSize;
		}
		
		public function GetStatusCode():int
		{
			return m_nStatusCode;
		}
		
		public function SetStatusCode(nCode:int):void
		{
			m_nStatusCode = nCode;
		}
		
		
		protected function SetData( response:ByteArray, responseLen:uint ):void
		{
			if(m_pData != null) {
//				delete [] m_pData;
				m_pData=null;
			}
			
			m_nDataSize = responseLen;
			if(responseLen > 0)
			{
				m_pData=new ByteArray();
				m_pData.length=m_nDataSize + 1;
				m_pData.writeBytes(response,0,m_nDataSize);
				
//				m_pData    = new char[m_nDataSize + 1];
//				memcpy(m_pData, response, m_nDataSize);
//				m_pData[m_nDataSize]=0;
			}		
		}
		
	}
}