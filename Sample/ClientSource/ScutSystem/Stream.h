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
#ifndef _STREAM_H_
#define _STREAM_H_

#include "Defines.h"

namespace ScutSystem
{
	typedef enum tagStreamOrigin
	{
		soBegin = 0,
		soCurrent,
		soEnd,
	} EStreamOrigin;

	/**
	* @brief 流基类
	*    
	* \remarks
	* 
	*/
	class CStream
	{
	public:
		CStream(void);
		CStream(const CStream* pSource);
		virtual ~CStream(void);
	public:
		int GetPosition();
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
		void SetPosition(const int nPos);
		/**
		* @brief 获取流的大小
		* 
		* 
		* @n<b>函数名称</b>					: GetSize
		* @return 流的大小
		* @see	
		*/
		virtual int GetSize();
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
		virtual void SetSize(const int nSize) = 0;

		/**
		* @brief 从流中读取数据
		* 
		* 
		* @n<b>函数名称</b>					: Read
		* @n@param char*	pszBuffer		: 待读取数据的缓存
		* @param   int		nSize			: 要读取的数据大小
		* @return  返回成功读取的数据大小（byte）                     
		* @remarks 
		* @see 
		*/
		virtual int Read(char* pszBuffer, int nSize) = 0;
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
		virtual int Write(const char* pszBuffer, int nSize) = 0;

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
		virtual int Seek(int nOffset, EStreamOrigin origin) = 0;
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
		bool ReadBuffer(char* pszBuffer, int nSize);
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
		bool WriteBuffer(const char* pszBuffer, int nSize);

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
		int CopyFrom(CStream* pSource, int nSize = 0);
		
		/**
		* @brief 从流中取出数据
		* 【注意】不能取太大的数据
		* 
		* @n<b>函数名称</b>					: GetBuffer
		* @param   int		nSize			: 要获取的数据大小
		* @return  返回成功获取的数据块                     
		* @remarks 
		* @see 
		*/
		virtual char* GetBuffer(int nSize);

		/**
		* @brief 释放从流中取出数据
		* 
		* 
		* @n<b>函数名称</b>					: ReleaseBuffer
		* @param   char* pBuffer			: 要要释放的数据指针
		* @return                      
		* @remarks 
		* @see 
		*/
		//virtual void ReleaseBuffer(char* pBuffer);

		private:
			char* m_Buffer;
	};

	/**
	* @brief 句柄流基类
	*    
	* \remarks
	* 
	*/
	class CHandleStream: public CStream
	{
	public:
		CHandleStream();
		/**
		* @brief 设置流的大小
		* 
		* 
		* @n<b>函数名称</b>					: SetSize
		* @see	参见父类说明
		*/
		virtual void SetSize(const int nSize);
		/**
		* @brief 从流中读取数据
		* 
		* 
		* @n<b>函数名称</b>					: Read
		* @see 参见父类说明
		*/
		virtual int Read(char* pszBuffer, int nSize);
		/**
		* @brief 将数据写入到流中
		* 
		* 
		* @n<b>函数名称</b>					: Write
		* @see	参见父类说明
		*/
		virtual int Write(const char* pszBuffer, int nSize);
		/**
		* @brief 从流中搜寻数据，此函数用于设置流当前的游标位置
		* 
		* 
		* @n<b>函数名称</b>					: Seek
		* @see	参见父类说明
		*/
		virtual int Seek(int nOffset, EStreamOrigin origin);
		/**
		* @brief 获取句柄
		* 
		* 
		* @n<b>函数名称</b>					: GetHandle
		* @return  返回流的句柄                     
		* @remarks 
		* @see 
		*/
		intptr_t GetHandle();
		
		void SetHandle(intptr_t hHandle);
	protected:
	private:
		intptr_t m_hHandle;
	};

	/**
	* @brief 文件流，用于存取一个文件
	*    
	* \remarks
	* 
	*/
	class CFileStream: public CHandleStream
	{
	public:
		CFileStream();
		//开启一个文件，chMode在c环境下使用，如IPhone
		int Open(const char* lpszFileName, DWORD dwFlag, DWORD dwMode, char* chMode = NULL);
		virtual ~CFileStream();
	private:
	};

	/**
	* @brief 内存流基类
	*    
	* \remarks
	* 
	*/
	class CBaseMemoryStream: public CStream
	{
	public:
		CBaseMemoryStream();
		/**
		* @brief 从流中读取数据
		* 
		* 
		* @n<b>函数名称</b>					: Read
		* @see 参见父类说明
		*/
		virtual int Read(char* pszBuffer, int nSize);
		/**
		* @brief 从流中搜寻数据，此函数用于设置流当前的游标位置
		* 
		* 
		* @n<b>函数名称</b>					: Seek
		* @see	参见父类说明
		*/
		virtual int Seek(int nOffset, EStreamOrigin origin);
		/**
		* @brief 将当前流中的数据保存到目标流中
		* 
		* 
		* @n<b>函数名称</b>					: SaveTo
		* @n@param CStream*	pDest			: 目标流指针
		* @return  
		* @remarks 
		* @see 
		*/
		void SaveTo(CStream* pDest);
		/**
		* @brief 将当前流中的数据保存到目标文件中
		* 
		* 
		* @n<b>函数名称</b>					: SaveTo
		* @n@param LPCTSTR	lpszFileName	: 待保存的文件名称
		* @return  
		* @remarks 
		* @see 
		*/
		void SaveTo(const char* lpszFileName);
		void* GetMemory();
	protected:
		void SetPointer(void* pNewPtr, int nSize);
	protected:
		void* m_pMemory;
		int m_nSize;
		int m_nPosition;
	};

	/**
	* @brief 内存流，用于存取内存
	*    
	* \remarks
	* 
	*/
	class CMemoryStream: public CBaseMemoryStream
	{
	public:
		CMemoryStream();
		virtual ~CMemoryStream();
		/**
		* @brief 清除流中所有数据
		* 
		* 
		* @n<b>函数名称</b>					: Clear
		* @return  
		* @remarks 
		* @see 
		*/
		void Clear();
		/**
		* @brief 从来源流中拷贝数据
		* 
		* 
		* @n<b>函数名称</b>					: LoadFrom
		* @n@param CStream*	pSource			: 待拷贝的来源流指针
		* @return  
		* @remarks 
		* @see 
		*/
		void LoadFrom(CStream* pSource);
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
		void LoadFrom(const char* lpszFileName);
		/**
		* @brief 设置流的大小
		* 
		* 
		* @n<b>函数名称</b>					: SetSize
		* @see	参见父类说明
		*/
		virtual void SetSize(const int nSize);
		/**
		* @brief 将数据写入到流中
		* 
		* 
		* @n<b>函数名称</b>					: Write
		* @see	参见父类说明
		*/
		virtual int Write(const char* pszBuffer, int nSize);
	protected:
		virtual void* Realloc(int& nNewCapacity);
		void SetCapacity(int nNewCapacity);
		int GetCapacity();
	private:	
	private:
		int m_nCapacity;
	};

}

#endif
