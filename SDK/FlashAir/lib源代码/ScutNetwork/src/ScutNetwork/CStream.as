package ScutNetwork
{
	import flash.utils.ByteArray;

	/**
	 * 流基类
	 * 
	 * 继承该类 必须该父类所有的方法 
	 * @author xiaor
	 * 
	 */	
	public class CStream
	{
		//缓存大小
		public static var CS_MAXBUFSIZE:uint=0xC000;
		//内存每次递增幅度
		public static var CS_MEMORYDELTA:uint=0x2000;
		
		public static var soBegin:int=0;
		public static var soCurrent:int=1;
		public static var soEnd:int=2;
		
		public function CStream(pSource:CStream=null)
		{
			this.CopyFrom(pSource);
		}
		
		public function GetPosition():int
		{
			return Seek(0, soCurrent);
		}
		
		/**
		 * @brief 设置流中游标的当前位置
		 * 
		 * 
		 * @n<b>函数名称</b>					: SetPosition
		 * @n@param int		nPos			: 待设置的流位置
		 * @return
		 * @remarks 此函数用于调整流中游标的位置，也可以使用Seek方法
		 * @see	Seek方法
		 */
		public function SetPosition(nPos:int):void
		{
			Seek(nPos, soBegin);
		}
		
		/**
		 * @brief 获取流的大小
		 * 
		 * @n<b>函数名称</b>					: GetSize
		 * @return 流的大小
		 * @see	
		 */
		public function GetSize():int
		{
			var nPos:int = Seek(0, soCurrent);
			var ret:int = Seek(0, soEnd);
			Seek(nPos, soBegin);
			return ret;
		}
		
		
		/**
		 * @brief 设置流的大小
		 * 
		 * 
		 * @n<b>函数名称</b>					: SetSize
		 * @n@param int		nSize			: 待设置的流大小
		 * @return 
		 * @remarks 如果设置的流大小比之前的小，那么将会裁剪数据，否则将会填充数据
		 * @see	
		 */
		public function SetSize(nSize:int):void
		{
			
		}

		/**
		 * @brief 从流中读取数据
		 * 
		 * @n<b>函数名称</b>					: Read
		 * @n@param char*	pszBuffer		: 待读取数据的缓存
		 * @param   int		nSize			: 要读取的数据大小
		 * @return  返回成功读取的数据大小（byte）                     
		 * @remarks 
		 * @see 
		 */
		public function Read(pszBuffer:ByteArray, nSize:int):int
		{
			return 0;
		}

		/**
		 * @brief 将数据写入到流中
		 * 
		 * 
		 * @n<b>函数名称</b>					: Write
		 * @n@param char*	pszBuffer		: 待写入数据的缓存
		 * @param   int		nSize			: 要写入的数据大小
		 * @return  返回成功写入的数据大小（byte）                     
		 * @remarks 
		 * @see 
		 */
		public function Write(pszBuffer:ByteArray, nSize:int):int
		{
			return 0;
		}
		
		/**
		 * @brief 从流中搜寻数据，此函数用于设置流当前的游标位置
		 * 
		 * 
		 * @n<b>函数名称</b>						: Seek
		 * @n@param int					nOffset	: 搜寻的偏移值
		 * @param   EStreamOrigin		origin	: 搜寻使用的开始位置
		 * @return  返回当前的游标位置                     
		 * @remarks 此函数用于调整流中游标的位置,EStreamOrigin表明了搜寻使用的开始位置，分别是从最开始，当前以及最后面开始搜寻
		 * @see 
		 */
		public function Seek(nOffset:int, origin:int):int
		{
			return 0;
		}
		
		/**
		 * @brief 将数据从流中读取到缓存
		 * 
		 * 
		 * @n<b>函数名称</b>					: ReadBuffer
		 * @n@param char*	pszBuffer		: 待读取数据的缓存
		 * @param   int		nSize			: 要读取的数据大小
		 * @return  成功返回true，否则返回false
		 * @remarks 此函数与Read的区别在于，Read允许读取的数据大小小于请求的大小，而该函数则要求必须获取nSize指定大小的数据
		 * @see 
		 */
		public function ReadBuffer(pszBuffer:ByteArray, nSize:int):Boolean
		{
			if (nSize != 0 && this.Read(pszBuffer, nSize) != nSize)
			{
				return false;
			}
			return true;
		}
		
		/**
		 * @brief 将数据写入到流中
		 * 
		 * 
		 * @n<b>函数名称</b>					: WriteBuffer
		 * @n@param char*	pszBuffer		: 待写入数据的缓存
		 * @param   int		nSize			: 要写入的数据大小
		 * @return  成功返回true，否则返回false
		 * @remarks 此函数与Write的区别在于，Write允许写入的数据大小小于请求的大小，而该函数则要求必须写入nSize指定大小的数据
		 * @see 
		 */
		public function WriteBuffer(pszBuffer:ByteArray, nSize:int):Boolean
		{
			if (nSize != 0 && this.Write(pszBuffer, nSize) != nSize)
			{
				return false;
			}
			return true;
		}
		
		/**
		 * @brief 从来源流中拷贝指定大小的数据
		 * 
		 * 
		 * @n<b>函数名称</b>					: CopyFrom
		 * @n@param CStream*	pSource			: 待拷贝数据的原始流指针
		 * @param   int		nSize			: 待拷贝的数据大小
		 * @return  返回成功拷贝的数据大小（byte）                     
		 * @remarks 
		 * @see 
		 */
		public function CopyFrom(pSource:CStream, nSize:int=0):int
		{
			if (!pSource || pSource.GetSize() == 0)
			{
				return 0;
			}
			if (nSize == 0) //表示要拷贝全部
			{
				pSource.SetPosition(0);
				nSize = pSource.GetSize();
			}
			var ret:int = nSize;
			var pszBuffer:ByteArray = new ByteArray();	//暂时的缓存指针
			var nBufSize:int = nSize;			//缓存大小
			if (nBufSize > CS_MAXBUFSIZE)
			{
				nBufSize = CS_MAXBUFSIZE;
			}
			pszBuffer.length=nBufSize;
			if (pszBuffer)
			{
				var nTempSize:int = 0;	//每次读的大小
				while (nSize != 0)
				{
					if (nSize > nBufSize)
					{
						nTempSize = nBufSize;
					}
					else
					{
						nTempSize = nSize;
					}
					pSource.ReadBuffer(pszBuffer, nTempSize);
					this.WriteBuffer(pszBuffer, nTempSize);
					nSize -= nTempSize;
				}
				pszBuffer=null;
				return ret;
			}
			return 0;
		}
	}
}