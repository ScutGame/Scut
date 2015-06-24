package ScutDataLogic
{
		public function registerCallback(evtName:String,callBackFun:Function):void
		{
			if(!ScutDataLogic.DataDispatch.ScutGameEventDispatch.hasEventListener(evtName)){
				ScutDataLogic.DataDispatch.ScutGameEventDispatch.addEventListener(evtName,callBackFun);
			}
		}
}