package ScutDataLogic
{
		
		public class ZyExecRequest
		{
			private static var ZyRequestCounter:int = 1;
			
			public static function request(eventName:String,hostName:String=""):void
			{
				ZyRequestCounter = ZyRequestCounter + 1;
				var requestURL:String = ScutDataLogic.CNetWriter.getInstance().generatePostData();
				if(hostName==""){
					ScutDataLogic.CDataRequest.Instance().AsyncExecRequest(eventName, requestURL, ZyRequestCounter, null);
				}else{
					ScutDataLogic.CDataRequest.Instance().AsyncExecTcpRequest(eventName, hostName, 1, null,null,requestURL,requestURL.length);
				}
				
			}
		}
		
		
}