package ScutDataLogic
{
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.utils.ByteArray;
	
	import ScutNetwork.CMemoryStream;
	import ScutNetwork.CStream;

	public class DataDispatch
	{
		public static var ScutGameEventDispatch:EventDispatcher = new EventDispatcher();
		
		public function DataDispatch()
		{
		}
		
		public static function netDataDispatch(evtName:String, nTag:int, nNet:int, lpData:*, lpExternal:*):void
		{
			var bValue:Boolean;
			if(lpData is ByteArray){
				bValue = CNetReader.getInstance().pushNetStream(lpData, (lpData as ByteArray).length);
			}else{
				bValue = CNetReader.getInstance().pushNetStream((lpData as CMemoryStream).GetMemory(), (lpData as CMemoryStream).GetSize());
			}
			
			if (!bValue)
			{
				return ;
			}
			
			ScutGameEventDispatch.dispatchEvent(new Event(evtName));
		}
	}
}