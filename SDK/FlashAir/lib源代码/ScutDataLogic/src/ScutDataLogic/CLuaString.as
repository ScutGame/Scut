package ScutDataLogic
{
	public class CLuaString
	{
		
		private var m_strValue:String="";
		public function CLuaString(strValue:String="")
		{
			m_strValue = strValue;
		}
		
		public function setString(szValue:String):void
		{
			m_strValue = szValue;
		}
		public function getCString():String
		{
			return m_strValue;
		}
		
		public function getSize():int
		{
			return m_strValue.length;
		}
	}
}