package ScutNetwork
{
	import flash.utils.ByteArray;

	public class AsyncInfo
	{
		
		public var Sender:CHttpClient;//发起请求的HttpClient
		public var Response:CHttpClientResponse;//响应对象		
		public var Url:String;//URL
		public var PostData:ByteArray;//提交的数据
		public var PostDataSize:int;//提交的数据大小
		public var FormFlag:Boolean;//是否是表单提交
		public var Status:int;//网络请求异步状态
		public var Mode:int;//异步请求模式
		public var Data1:int;//数据1
		public var Data2:int;//数据2
		
		/***EAsyncInfoStatus***/
		public static var aisNone:int=0;
		public static var aisProgress:int=1;
		public static var aisSucceed:int=2;
		public static var aisTimeOut:int=3;
		public static var aisFailed:int=4;
		
		
		/***EAsyncMode***/
		public static var amGet:int=0;
		public static var amPost:int=1;
		
		/***ENetType***/
		public static var ntNone:int=0;
		public static var ntWIFI:int=1;
		public static var ntCMWAP:int=2;
		public static var ntCMNET:int=3;
		
		
		
		public function AsyncInfo()
		{
			Reset();
		}
		
		public function Reset():void
		{
			Response = null;
			PostData = null;
			PostDataSize = 0;
			FormFlag = false;
			Sender = null;
			Status = aisNone;
			Mode = amGet;
			Data1 = 0;
			Data2 = 0;
		}
	}
}