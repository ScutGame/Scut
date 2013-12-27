package ScutDataLogic
{
	import flash.utils.getTimer;
	
	import cmodule.DesEncrypt.CLibInit;

	public class CNdUtility
	{
		public static var loader:CLibInit;
		public static var lib:Object; 
		public static var initialized:Boolean = false;
		
		
		public function CNdUtility()
		{
		}
		
		public static function DesEncrypt(lpszKey : String, lpDataIn : String) : String
		{
			if(!initialized) 
			{
				initialized = true;
				if (!loader) 
					loader = new CLibInit();
				if (!lib) 
					lib = loader.init();
			}
			
			var result : String = lib.DesEncrypt(lpDataIn, lpszKey);
			
			return result;
		}
		
		public static function GetTickCount():int
		{
			return getTimer();
		}
		
	}
}