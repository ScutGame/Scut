package ScutNetwork
{
	
	import flash.utils.ByteArray;
	import flash.utils.Endian;
	public class CMemoryStream extends CBaseMemoryStream
	{
		//缓存大小
		public static var CS_MAXBUFSIZE:uint=0xC000;
		//内存每次递增幅度
		public static var CS_MEMORYDELTA:uint=0x2000;
		
		
		private var m_nCapacity:int;
		
		public function CMemoryStream(pSource:CStream=null)
		{
			super(pSource);
		}
		
		/**
		 * @brief 清除流中所有数据
		 * 
		 * 
		 * @n<b>函数名称</b>					: Clear
		 * @return  
		 * @remarks 
		 * @see 
		 */
		public function Clear():void
		{
			SetCapacity(0);
			m_nSize = 0;
			m_nPosition = 0;
		}
		
		public function LoadFrom(...args):void
		{
			overload(2,[LoadFrom1,[CStream],0],[LoadFrom2,[String],0],args);
		}
				
		/**
		 * @brief 从来源流中拷贝数据
		 * 
		 * @n<b>函数名称</b>					: LoadFrom
		 * @n@param CStream*	pSource			: 待拷贝的来源流指针
		 * @return  
		 * @remarks 
		 * @see 
		 */
		public function LoadFrom1(pSource:CStream):void
		{
			pSource.SetPosition(0);
			var nSize:int = pSource.GetSize();
			this.SetSize(nSize);
			if (nSize != 0)
			{
				pSource.ReadBuffer(m_pMemory, nSize);
			}
		}
		
		/**
		 * @brief 从指定的文件中载入数据
		 * 
		 * 
		 * @n<b>函数名称</b>					: LoadFrom
		 * @n@param LPCTSTR	lpszFileName	: 待载入的文件名称
		 * @return  
		 * @remarks 
		 * @see 
		 */
		public function LoadFrom2(lpszFileName:String):void
		{
//			CFileStream* pStream = new CFileStream();
//			#ifdef ND_UPHONE
//			if (pStream->Open(lpszFileName, PO_CREAT | PO_RDWR | PO_BINARY, PS_IREAD | PS_IWRITE) == 0)
//			{
//				this->LoadFrom(pStream);
//				delete pStream;
//			}
//			#else
//				if (pStream->Open(lpszFileName, 0, 0, "rb") == 0)
//				{
//					this->LoadFrom(pStream);
//					delete pStream;
//				}
//			#endif
		}
		
		override public function SetSize(nSize:int):void
		{
			var nOldPos:int = m_nPosition;
			this.SetCapacity(nSize);
			m_nSize = nSize;
			if (nOldPos > nSize)
			{
				Seek(0, soEnd);
			}
		}
		
		override public function Write(pszBuffer:ByteArray, nSize:int):int
		{
			if (m_nPosition >= 0 && nSize >= 0)
			{
				var nPos:int = m_nPosition + nSize;
				if (nPos > 0)
				{
					if (nPos > m_nSize)
					{
						if (nPos > m_nCapacity)
						{
							SetCapacity(nPos);
						}
						m_nSize = nPos;
					}
					m_pMemory.position = m_nPosition;
					m_pMemory.writeBytes(pszBuffer,0,nSize);
					m_pMemory.position = m_nPosition;
					
					m_nPosition = nPos;
					return nSize;
				}
			}
			return 0;
		}
		
		
		protected function Realloc(nNewCapacity:int):ByteArray
		{
			if (nNewCapacity > 0 && nNewCapacity != m_nSize)
			{
				nNewCapacity = (nNewCapacity + (CS_MEMORYDELTA - 1)) ^ (CS_MEMORYDELTA - 1);
			}
			var ret:ByteArray = this.GetMemory();
			if (nNewCapacity != m_nCapacity)
			{
				if (nNewCapacity == 0)
				{
					ret=null;
					ret = null;
				}
				else
				{
					if (m_nCapacity == 0)
					{
						ret = new ByteArray();
						ret.endian = Endian.LITTLE_ENDIAN;
						ret.length=nNewCapacity;
					}
					else
						ret.length=nNewCapacity;
				}
			}
			return ret;
		}
		
		protected function SetCapacity(nNewCapacity:int):void
		{
			SetPointer(Realloc(nNewCapacity), m_nSize);
			m_nCapacity = nNewCapacity;
		}
		protected function GetCapacity():int
		{
			return m_nCapacity;
		}
	}
}