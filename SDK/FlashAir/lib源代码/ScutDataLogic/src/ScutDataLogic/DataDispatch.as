package ScutDataLogic
{
	import flash.events.Event;
	import flash.events.EventDispatcher;
	
	import ScutNetwork.CMemoryStream;
	import ScutNetwork.CStream;

	public class DataDispatch
	{
		public static var ScutGameEventDispatch:EventDispatcher = new EventDispatcher();
		
		public function DataDispatch()
		{
		}
		
		public static function netDataDispatch(evtName:String, nTag:int, nNet:int, lpData:CStream, lpExternal:*):void
		{
			var bValue:Boolean = CNetReader.getInstance().pushNetStream((lpData as CMemoryStream).GetMemory(), (lpData as CMemoryStream).GetSize());
			if (!bValue)
			{
				return ;
			}
			
			ScutGameEventDispatch.dispatchEvent(new Event(evtName));
		}
	}
}