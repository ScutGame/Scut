package ScutDataLogic
{
	import flash.utils.ByteArray;
	import flash.utils.getTimer;
	
	import ScutNetwork.CHttpClient;
	import ScutNetwork.CHttpClientResponse;
	import ScutNetwork.CHttpSession;

	public class RequestInfo
	{
		public var Handler:CDataHandler;
		public var HandlerTag:int;
		public var HttpClient:CHttpClient;
		public var HttpResponse:CHttpClientResponse;
		public var HttpSession:CHttpSession;
		public var StartTime:Number;
		public var Url:String="";
		public var LPData:ByteArray;
		public var Status:int;
		public var pScene:Object;
		
		public var EventName:String = "";
		
		
		public function RequestInfo()
		{
			
			Handler = null;
			HandlerTag = 0;
			HttpClient = null;
			HttpResponse = null;
			HttpSession = null;
			StartTime = GetTickCount();
			Url = "";
			LPData = null;
			Status = 0;
			pScene = null;
		}
		
		private function GetTickCount():int
		{
			return getTimer();
		}
	}
}