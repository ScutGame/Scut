package ScutDataLogic
{
	
	
	public class CFileHelper
	{
		
		public static var FILE_SEP:String= "\\";
		
		private static var s_width:int;
		private static var s_height:int;
		private static var s_strResource:String;
		private static var s_strResource480_800:String;
		private static var s_strAndroidSDPath:String;
		private static var s_strAndroidPackagePath:String;
		private static var s_strRelativePath:String;
		private static var s_strIPhoneBundleID:String;
		
		public function CFileHelper()
		{
			s_width = 480;
			s_height = 320;
			
			s_strResource			= "resource";
			s_strResource480_800	= "resource480_800";
			
			s_strAndroidSDPath="";
			s_strAndroidPackagePath="";
			s_strRelativePath="";
			s_strIPhoneBundleID="";
		}
		
		public static function encryptPwd(pPwd:String, key:String):CLuaString
		{
			var DES_KEY:String="n7=7=7dk";
			var strRet:String=CZyUtility.DesEncrypt(DES_KEY, pPwd);
			return new CLuaString(strRet);
		}
	}
}