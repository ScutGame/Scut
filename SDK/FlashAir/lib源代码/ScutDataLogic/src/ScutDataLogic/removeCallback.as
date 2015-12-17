package ScutDataLogic
{
		public function removeCallback(evtName:String,callBackFun:Function):void
		{
			if(ScutDataLogic.DataDispatch.ScutGameEventDispatch.hasEventListener(evtName)){
				ScutDataLogic.DataDispatch.ScutGameEventDispatch.removeEventListener(evtName,callBackFun);
			}
		}
}