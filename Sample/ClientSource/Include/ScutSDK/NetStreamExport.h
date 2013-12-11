/****************************************************************************
Copyright (c) 2013-2015 Scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

#ifndef NetStreamExport_Header_yay
#define NetStreamExport_Header_yay


 
//#include "lua.h"
#include "LuaString.h"
#include <stack>
#include <list>
///////////////////////////////////////////////////////////////////////////
///// 网络数据包结构: 前两位为包大小，紧跟RECORD 个数，然后结构SIZE、结构内容。
/////////////////////////////////////////////////////////////////////////////

namespace ScutDataLogic 
{
	struct RECORDINFO
	{
		int nRecordSize;
		int nRecordReadSize;
		RECORDINFO(int _RecordSize, int _RecordReadSize)
		{
			nRecordSize		= _RecordSize;
			nRecordReadSize	= _RecordReadSize; 
		}
		
	};


		/**
		* @brief  网络消息数据流解析
		* @remarks 
		* @see		
		*/
	class CNetStreamExport{

	public:
		CNetStreamExport();
		~CNetStreamExport();
	

		/**
		* @brief  传入网络消息数据流及大小
		* @n<b>函数名称</b>					: pushNetStream
		* @return  bool 如果NETWORK 传入的包大小与包前两个字节的值不一样，返回FALSE。
		* @n@param  char* 网络消息数据流
		* @param    int 数据流size大小
		* @remarks  收到网络消息包时调用此函数进行数据设置
		* @see		
		*/
		virtual bool pushNetStream(char* pdataStream,int wSize);

		 
		/**
		* @brief  开始取记录数据
		* @n<b>函数名称</b>					: recordBegin
		* @return  bool 还有数据结构返回TRUE， 结否返回FALSE。 
		* @remarks  开始取记录数据
		* @see	
		* 
		*/
		bool recordBegin();
		void recordEnd();
		
		 
		/**
		* @brief  取数据记录数
		* @remarks   
		* @see	
		* 
		*/
		unsigned char  getRecordNumber(void);

		/**
		* @brief  从数据流中取一个BYTE数据
		* @remarks   
		* @see	
		*/
		unsigned char	getBYTE(void);
		/**
		* @brief  从数据流中取一个CHAR数据
		* @remarks   
		* @see	
		*/
		char			getCHAR(void);
		/**
		* @brief  从数据流中取一个WORD数据
		* @remarks   
		* @see	
		*/
		unsigned short	getWORD(void);
		/**
		* @brief  从数据流中取一个SHORT数据
		* @remarks   
		* @see	
		*/
		short			getSHORT(void);
		/**
		* @brief  从数据流中取一个DWORD数据
		* @remarks   
		* @see	
		*/
		unsigned int	getDWORD(void);
		/**
		* @brief  从数据流中取一个Int数据
		* @remarks   
		* @see	
		*/
		int				getInt(void);
		/**
		* @brief  从数据流中取一个Float数据
		* @remarks   
		* @see	
		*/
		float			getFloat(void);
		/**
		* @brief  从数据流中取一个Double数据
		* @remarks   
		* @see	
		*/
		double			getDouble(void);

		/**
		* @brief  从数据流中取一个Int64数据
		* @remarks   
		* @see	
		*/
		unsigned long long getInt64();

		 
		/**
		* @brief  在LUA中分配字符内存，传到C中设置内容
		* @n<b>函数名称</b>					: getString
		* @return  bool 内存分配失败或数据大小非法，返回FALSE。
		* @n@param  char* 要获取的数据指针
		* @param    int 要获取数据大小
		* @remarks   
		* @see		
		*/
		bool			getString(char* psz, int nLength);

		/**
		* @brief  在C中分配内存，传给LUA ,这个接口与freeStringBuffer() 接口配套使用
		* @n<b>函数名称</b>					: getString
		* @return  bool 内存分配失败或数据大小非法，返回FALSE。
		* @n@param  int 要获取数据大小
		* @remarks   
		* @see		
		*/
		const char*		getString(/*const  char* pData,*/int nLength);
		/**
		* @brief  释放在C中分配的内存,这个接口与getString() 接口配套使用
		* @n<b>函数名称</b>					: freeStringBuffer
		* @remarks   
		* @see		
		*/
		void			freeStringBuffer();

		bool IsStatusReady();
		
	protected:

		std::stack< RECORDINFO*, std::list<RECORDINFO*> > m_RecordStack;
		char*   m_pDataStream;
		int 	m_nStreamPos;
		int		m_nSize;
		char*	m_szRet;

	};


}

//extern "C" int  tolua_NetStreamExport_open(lua_State* tolua_S);

#endif