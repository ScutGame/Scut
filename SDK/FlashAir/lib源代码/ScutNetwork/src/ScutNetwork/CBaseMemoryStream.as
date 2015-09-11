package ScutNetwork
{
	
	import flash.utils.ByteArray;
	import flash.utils.Endian;
	public class CBaseMemoryStream extends CStream
	{
		protected var m_pMemory:ByteArray=new ByteArray();
		protected var m_nSize:int;
		protected var m_nPosition:int;
		
		public function CBaseMemoryStream(pSource:CStream=null)
		{
			super(pSource);
			m_pMemory.endian = Endian.LITTLE_ENDIAN;
		}
		
		override public function Read(pszBuffer:ByteArray, nSize:int):int
		{
			if (m_nPosition >= 0 && nSize >= 0)
			{
				var ret:int = m_nSize - m_nPosition;
				if (ret > 0)
				{
					if (ret > nSize)
					{
						ret = nSize;
					}
					m_pMemory.position = 0;
					pszBuffer.writeBytes(m_pMemory,m_nPosition,ret);
					m_pMemory.position = m_nPosition;
					m_nPosition += ret;
					return ret;
				}
			}
			return 0;
		}
		
		override public function Seek(nOffset:int, origin:int):int
		{
			switch (origin)
			{
				case soBegin:
					m_nPosition = nOffset;
					//m_pMemory.position=nOffset;
					break;
				case soCurrent:
					m_nPosition += nOffset;
					//m_pMemory.position=m_pMemory.position+nOffset;
					break;
				case soEnd:
					m_nPosition = m_nSize + nOffset;
					//m_pMemory.position=m_nSize+nOffset;
					break;
			}
			return m_nPosition;
		}
		
		
		public function SaveTo(...args):void
		{
			overload(2,[SaveTo1,[CStream],0],[SaveTo2,[String],0],args);
		}
		
		public function SaveTo1(pDest:CStream):void
		{
			if (m_nSize != 0)
			{
				pDest.WriteBuffer(m_pMemory, m_nSize);
			}
		}
		public function SaveTo2(lpszFileName:String):void
		{
//			CFileStream* fs = new CFileStream();
//			#ifdef ND_UPHONE
//			if (fs->Open(lpszFileName, PO_CREAT | PO_RDWR | PO_BINARY, PS_IREAD | PS_IWRITE) == 0)
//			{
//				this->SaveTo(fs);
//				delete fs;
//			}
//			#else
//				if (fs->Open(lpszFileName, 0, 0, "wb+") == 0)
//				{
//					this->SaveTo(fs);
//					delete fs;
//				}
//			#endif
		}
		
		public function GetMemory():ByteArray
		{
			return m_pMemory;
		}
		
		protected function SetPointer(pNewPtr:ByteArray, nSize:int):void
		{
			m_pMemory = pNewPtr;
			m_nSize = nSize;
		}
	}
}