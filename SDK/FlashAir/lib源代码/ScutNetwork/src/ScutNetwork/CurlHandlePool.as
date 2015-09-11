package ScutNetwork
{
	public class CurlHandlePool
	{
		private static var g_Instance:CurlHandlePool;
		
		public function CurlHandlePool()
		{
		}
		
		
		public static function Instance():CurlHandlePool
		{
			if(g_Instance==null){
				g_Instance=new CurlHandlePool();
			}
			return g_Instance;
		}
		
		public function Dispose():void
		{
//			for (unsigned int i = 0; i < m_IdleList.size(); i++)
//			{
//				curl_easy_cleanup(m_IdleList[i]);
//			}
//			m_IdleList.clear();
		}
	}
}