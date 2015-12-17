package ScutNetwork
{
	public class CHttpClientRequest
	{
		
		public var m_strUserAgent:String;
		public var m_strHttpProxyHost:String;
		
		public var m_nHttpProxyPort:uint;
		public var m_strHttpsProxyHost:String;
		public var m_nHttpsProxyPort:uint;
		
		public var m_bUseProxy:Boolean;
		public var m_bProxyNTLM:Boolean;
		public var m_bProxyAuth:Boolean;
		public var m_bProxyAuthNTLM:Boolean;
		
		
		public var m_strProxyAuthPassword:String;
		public var m_strProxyAuthUsername:String;
		
		public function CHttpClientRequest()
		{
		}
		
		
	}
}