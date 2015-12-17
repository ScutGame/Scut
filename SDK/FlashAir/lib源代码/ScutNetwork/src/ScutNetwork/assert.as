package ScutNetwork
{	
	public function assert(e:Boolean, ...args):void
	{
		if(!e)
		{
			var mes:String = "";
			
			for(var i:int=0; i<args.length; i++)
			{
				mes += args[i];
			}
			
			var error:Error = new Error(mes);
			
			trace(error.getStackTrace());
			throw error;
		}
	}
}