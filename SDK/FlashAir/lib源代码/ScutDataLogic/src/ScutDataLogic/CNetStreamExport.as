package ScutDataLogic
{
	import flash.utils.ByteArray;
	public class CNetStreamExport
	{
		//存储多个RECORDINFO
		protected var m_RecordStack:Array=[];
		protected var m_pDataStream:ByteArray;
		protected var m_nStreamPos:int;
		protected var m_nSize:int;
		protected var m_szRet:String;
		
		public function CNetStreamExport()
		{
			m_pDataStream = null;
			m_nStreamPos = 0;
			m_nSize = 0;
			m_szRet= "";
		}
		
		public function Dispose():void
		{
			m_RecordStack=[];
			m_pDataStream = null;
			m_nStreamPos = 0;
			m_nSize = 0;
			m_szRet= "";
		}
		
		/**
		 * @brief  传入网络消息数据流及大小
		 * @n<b>函数名称</b>					: pushNetStream
		 * @return  bool 如果NETWORK 传入的包大小与包前两个字节的值不一样，返回FALSE。
		 * @n@param  char* 网络消息数据流
		 * @param    int 数据流size大小
		 * @remarks  收到网络消息包时调用此函数进行数据设置
		 * @see		
		 */
		public function pushNetStream(pdataStream:ByteArray, wSize:int):Boolean
		{
			m_pDataStream = pdataStream;		
			m_nSize = wSize;
			m_nStreamPos = 0;
			assert(m_RecordStack.length==0);
			m_RecordStack.length = 0;
			m_pDataStream.position = m_nStreamPos;
			//读取包的长度
			var nStreamSize:int = getInt();
			//如果包长不一致 返回
			if(nStreamSize != wSize)
			{
				trace(" Failed: CNetStreamExport: pushNetStream , wSize error ");
				m_nSize = 0;
				return false;
			}
			return true;
		}
		
		
		/**
		 * @brief  开始取记录数据
		 * @n<b>函数名称</b>					: recordBegin
		 * @return  bool 还有数据结构返回TRUE， 结否返回FALSE。 
		 * @remarks  开始取记录数据
		 * @see	
		 * 
		 */
		public function recordBegin():Boolean
		{
			if(m_nStreamPos + 4 > m_nSize )
			{
				trace(" Failed: 长度越界 CNetStreamExport: recordBegin ");
				return false;	
			}
			
			var nRecoredSize:int = getInt();
			
			var pinfo:RECORDINFO = new RECORDINFO(nRecoredSize, 4);
			m_RecordStack.push(pinfo);
			
			if(nRecoredSize > 4)
				return true;
			else
				return false;
		}
		
		
		public function recordEnd():void
		{
			assert(m_RecordStack.length > 0);
			
			var pInfo:RECORDINFO = m_RecordStack.pop();
			m_nStreamPos += (pInfo.nRecordSize - pInfo.nRecordReadSize);
			m_pDataStream.position = m_nStreamPos;
			if (m_RecordStack.length)
			{
				var pParent:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pParent.nRecordReadSize += pInfo.nRecordSize - 4;
			}
			pInfo=null;
		}
		
		
		/**
		 * @brief  取数据记录数
		 * @remarks   
		 * @see	
		 * 
		 */
		public function getRecordNumber():int
		{
			var by:int;
			by=m_pDataStream.readUnsignedByte();
			
			m_nStreamPos += 1;
			return by;
		}
		
		
		/**
		 * @brief  从数据流中取一个BYTE数据
		 * @remarks   
		 * @see	
		 */
		public function getBYTE():int
		{
			if(m_nStreamPos + 1 > m_nSize )
			{
				trace(" Failed: 长度越界  CNetStreamExport: getBYTE ");
				return 0;	
			}
			m_nStreamPos += 1;
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 1;
			}
			return m_pDataStream.readUnsignedByte();
		}
		
		/**
		 * @brief  从数据流中取一个CHAR数据
		 * @remarks   
		 * @see	
		 */
		public function getCHAR():String
		{
			if (m_nStreamPos + 1 > m_nSize)
			{
				trace(" Failed: 长度越界 CNetStreamExport: getCHAR ");
				return '0';
			}
			
			m_nStreamPos += 1;
			var char : int = getBYTE();
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 1;
			}
			return char.toString();
		}
		
		/**
		 * @brief  从数据流中取一个WORD数据
		 * @remarks   
		 * @see	
		 */
		public function getWORD():int
		{
			if(m_nStreamPos + 2 > m_nSize )
			{
				trace((" Failed: 长度越界 CNetStreamExport: getWORD"));
				return 0;	
			}
			m_nStreamPos += 2;
			
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 2;
			}
			return m_pDataStream.readShort();
		}
		
		/**
		 * @brief  从数据流中取一个SHORT数据
		 * @remarks   
		 * @see	
		 */
		public function getSHORT():int
		{
			if(m_nStreamPos + 2 > m_nSize )
			{
				trace(" Failed: 长度越界 CNetStreamExport: getSHORT ");
				return 0;	
			}
			m_nStreamPos += 2;
			
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 2;
			}
			return m_pDataStream.readShort(); 
		}
		
		/**
		 * @brief  从数据流中取一个DWORD数据
		 * @remarks   
		 * @see	
		 */
		public function getDWORD():int
		{
			if(m_nStreamPos + 4 >= m_nSize )
			{
				trace(" Failed: 长度越界 CNetStreamExport: getDWORD");
				return 0;	
			}
			m_nStreamPos += 4;
			
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 4;
			}
			return m_pDataStream.readInt();
		}
		
		
		/**
		 * @brief  从数据流中取一个Int数据
		 * @remarks   
		 * @see	
		 */
		public function getInt():int
		{
			if (m_nStreamPos + 4 > m_nSize)
			{
				trace(" Failed: 长度越界 CNetStreamExport: getInt");
				return 0;
			}
			
			m_nStreamPos += 4;
			
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 4;
			}
			return m_pDataStream.readInt();
		}
		
		/**
		 * @brief  从数据流中取一个Float数据
		 * @remarks   
		 * @see	
		 */
		public function getFloat():Number
		{
			if(m_nStreamPos + 4 > m_nSize )
			{
				trace(" Failed: 长度越界 CNetStreamExport: getFloat");
				return 0;	
			}
			m_nStreamPos += 4;
			
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 4;
			}
			return m_pDataStream.readFloat();
		}
		
		/**
		 * @brief  从数据流中取一个Double数据
		 * @remarks   
		 * @see	
		 */
		public function getDouble():Number
		{
			if(m_nStreamPos + 8 > m_nSize )
			{
				trace(" Failed: 长度越界 CNetStreamExport: getDouble");
				return 0;	
			}
			m_nStreamPos += 8;
			
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 8;
			}
			return m_pDataStream.readDouble();
		}
		
		/**
		 * @brief  从数据流中取一个Int64数据
		 * @remarks   
		 * @see	
		 */
		public function getInt64():Number
		{
			if(m_nStreamPos + 8 > m_nSize )
			{
				trace(" Failed: 长度越界 CNetStreamExport: getInt64");
				return 0;	
			}
			m_nStreamPos += 8;
			var long_b:uint=m_pDataStream.readUnsignedInt();//高位
			var long_l:int=m_pDataStream.readInt();//低位
//			trace("long_b,long_l",long_b,long_l);
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += 8;
			}
			var result:Number=long_l * 0x100000000  + long_b;
			return result;
		}
		
		
		public function getString(...args):*
		{
			return overload(2,[getString1,[ByteArray,int],0],[getString2,[int],0],args);
		}
		
		
		/**
		 * @brief  在LUA中分配字符内存，传到C中设置内容
		 * @n<b>函数名称</b>					: getString
		 * @return  bool 内存分配失败或数据大小非法，返回FALSE。
		 * @n@param  char* 要获取的数据指针
		 * @param    int 要获取数据大小
		 * @remarks   
		 * @see		
		 */
		public function getString1(psz:ByteArray, nLength:int):Boolean
		{
			return true;
		}
		
		/**
		 * @brief  在C中分配内存，传给LUA ,这个接口与freeStringBuffer() 接口配套使用
		 * @n<b>函数名称</b>					: getString
		 * @return  bool 内存分配失败或数据大小非法，返回FALSE。
		 * @n@param  int 要获取数据大小
		 * @remarks   
		 * @see		
		 */
		public function getString2(nLength:int):String
		{
//			trace("这里使用到了getString nLength="+nLength); 
			if(m_nStreamPos + nLength > m_nSize )
			{
				trace(" Failed: 长度越界 CNetStreamExport: getString");
//				return null;	
			}
			m_nStreamPos += nLength;
			
			if (m_RecordStack.length)
			{
				var pInfo:RECORDINFO = m_RecordStack[m_RecordStack.length-1];
				pInfo.nRecordReadSize += nLength;
			}
			return m_pDataStream.readUTFBytes(nLength);
		}
		
		
		/**
		 * @brief  释放在C中分配的内存,这个接口与getString() 接口配套使用
		 * @n<b>函数名称</b>					: freeStringBuffer
		 * @remarks   
		 * @see		
		 */
		public function freeStringBuffer():void
		{
			if(m_szRet)
			{
				m_szRet="";
				m_szRet = null;	 
			}
		}
		
		public function IsStatusReady():Boolean
		{
			return m_RecordStack.length==0;
		}
	}
}