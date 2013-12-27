package ScutDataLogic
{
		
		public class ZyExecRequest
		{
			private static var ZyRequestCounter:int = 1;
			
			public static function request(eventName:String):void
			{
				ZyRequestCounter = ZyRequestCounter + 1;
				var requestURL:String = ScutDataLogic.CNetWriter.getInstance().generatePostData();
				ScutDataLogic.CDataRequest.Instance().AsyncExecRequest(eventName, requestURL, ZyRequestCounter, null);
			}
		}
		
		
}