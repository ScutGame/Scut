package ScutDataLogic
{
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.SecurityErrorEvent;
	import flash.net.Socket;
	import flash.utils.ByteArray;
	import flash.utils.Endian;

	public class CTcpClient
	{
		private static var ins:CTcpClient;
		private var socket:Socket;
		
		private var connectSuccessFun:Function;
		private var tcpEventName:String = "";
		private var dispatchEventFun:Function = null;
		
		public function CTcpClient(_sigle:single)
		{
			socket = new Socket();
			addEventListeners();
		}
		
		
		private function addEventListeners():void
		{
			socket.addEventListener(Event.CONNECT,onConnectHandle);
			socket.addEventListener(ProgressEvent.SOCKET_DATA,onSocketData);
			socket.addEventListener(SecurityErrorEvent.SECURITY_ERROR, securityErrorHandler);
			socket.addEventListener(IOErrorEvent.IO_ERROR,onError);
		}
		
		public static function Ins():CTcpClient
		{
			if(ins==null){
				ins = new CTcpClient(new single());
				
			}
			return ins;
		}
		
		/**
		 * 获取当前socket 
		 * @return 
		 * 
		 */		
		public function SocketClient():Socket
		{
			return socket;
		}
		
		/**
		 * 连接socket 
		 * @param host 	地址
		 * @param port	端口
		 * @param successFun		连接成功回调函数
		 * 
		 */		
		public function Connect(host:String,port:int,successFun:Function):void
		{
			socket.connect(host,port);
			connectSuccessFun = successFun;
		}
		/**
		 * 断开Socket 
		 */		
		public function Close():void
		{
			socket.close();
		}
		
		/**
		 * 请求数据 
		 * @param evtName  事件名称
		 * 
		 */		
		public function Send(evtName:String):void
		{
			tcpEventName = evtName;
			socket.flush();
		}
		
		
		public function SetDispatchFun(fun:Function):void
		{
			if(dispatchEventFun!=null){
				return;
			}
			if(fun != null){
				dispatchEventFun = fun;
			}
		}
		
		
		protected function onConnectHandle(event:Event):void
		{
			trace("socket :: connect!!!");
			connectSuccessFun();
		}
		protected function onSocketData(event:ProgressEvent):void
		{
			trace("socket :: onSocketData!!!");
			//总包   结构：    总包长[4位] +  包体长度  +  包内容
			var totalAry:ByteArray = new ByteArray();
			socket.readBytes(totalAry);
			totalAry.position=0;
			totalAry.endian=Endian.LITTLE_ENDIAN;
			//读出总包长
			var packageSize:int = totalAry.readInt();
			
			//内容包体
			var contentArray:ByteArray = new ByteArray();
			contentArray.writeBytes(totalAry,4);
			contentArray.position = 0;
			contentArray.endian=Endian.LITTLE_ENDIAN;
			
			dispatchEventFun(tcpEventName,contentArray);
		}
		protected function onError(event:IOErrorEvent):void
		{
			trace("socket :: onError!!!");
		}
		protected function securityErrorHandler(event:SecurityErrorEvent):void
		{
			trace("socket :: securityErrorHandler!!!");
		}
	}
}

class single{
	
}