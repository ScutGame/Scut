package ScutDataLogic
{
	import flash.display.Stage;
	import flash.events.Event;

	public class FrameManager
	{
		private static var g_Instance:FrameManager;
		
		public function FrameManager()
		{
		}
		
		public static function Instance():FrameManager
		{
			if(g_Instance==null)
			{
				g_Instance=new FrameManager();
			}
			return g_Instance;
		}
		
		public function init(stage:Stage):void
		{
			stage.addEventListener(Event.ENTER_FRAME,onGameLoopHandler);
		}
		
		private function onGameLoopHandler(event:Event):void
		{
			CDataRequest.Instance().PeekLUAData();
		}
	}
}