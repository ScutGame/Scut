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



#include "stdafx.h"

#include "NetStreamExport.h"
#include <string>
#include <assert.h>
using namespace std;



//////////////////////  here for  DEMO
/*

// 前两位为包大小，紧跟RECORD 个数，然后结构SIZE、结构内容。
unsigned char szText[26] = {26,0,2,14,0,125,0,0,0,5,'k','a','t','t','y',0,0,8,0,100,0,0,0,1,'d'};
char szTest[10];	
ScutDataLogic::GetNetExportInstance()->pushNetStream((char*)&szText,26);


int nRecordNum = ScutDataLogic::GetNetExportInstance()->getRecordNumber();

for( int i=0; i< nRecordNum; i++ )
{
ScutDataLogic::GetNetExportInstance()->beginNextRecord();	
//get struct data		
int n = ScutDataLogic::GetNetExportInstance()->getInt();
BYTE l = ScutDataLogic::GetNetExportInstance()->getBYTE();		
ScutDataLogic::GetNetExportInstance()->getString((char*)&szTest,l);
}

*/
/////////////////////////////////////////////




namespace ScutDataLogic
{	

	// 	CNetStreamExport g_ExportNetStreamData;
	// 
	// 	CNetStreamExport*  GetNetExportInstance()
	// 	{
	// 		return &g_ExportNetStreamData;
	// 	}



	CNetStreamExport::CNetStreamExport(void)
	{
		m_pDataStream = NULL;
		m_nStreamPos = 0;
		m_nSize = 0;
		m_szRet= NULL;
	}



	CNetStreamExport::~CNetStreamExport(void)
	{

		while(!m_RecordStack.empty())
		{
			RECORDINFO* pInfo = m_RecordStack.top();
			delete pInfo;
			m_RecordStack.pop();
		}
	}	


	//如果NETWORK 传入的包大小与包前两个字节的值不一样，返回FALSE。
	bool CNetStreamExport::pushNetStream(char* pdataStream,int wSize)
	{
		m_pDataStream = pdataStream;		
		m_nSize = wSize;
		m_nStreamPos = 0;
		
		////特殊编译给陈波
		//return true;
		////end

		//m_nRecoredSize = 0;
		//m_nRecoredReadSize = 0;
		assert(m_RecordStack.empty());
		while(!m_RecordStack.empty())
		{
			RECORDINFO* pInfo = m_RecordStack.top();
			delete pInfo;
			m_RecordStack.pop();
		}
		int nStreamSize = getInt();
		if(nStreamSize != wSize)
		{
			ScutLog(" Failed: CNetStreamExport: pushNetStream , wSize error ");
			return false;
		}

		//跳过包大小
		m_pDataStream = pdataStream + 4;
		m_nStreamPos = 0;		
		//m_nRecoredReadSize = 0;

		return true;
	}


	bool CNetStreamExport::recordBegin()
	{

		if(m_nStreamPos + 4 > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: recordBegin ");
			return false;	
		}

		//m_nRecoredReadSize = 0;
		int nRecoredSize = getInt();

		RECORDINFO *pinfo = new RECORDINFO(nRecoredSize, 4);
		m_RecordStack.push(pinfo);


		if(nRecoredSize > 4)
			return true;
		else
			return false;

	}
	void CNetStreamExport::recordEnd()
	{
		assert(m_RecordStack.size() > 0);

		RECORDINFO* pInfo = m_RecordStack.top(); 
		m_nStreamPos += (pInfo->nRecordSize - pInfo->nRecordReadSize);
		m_RecordStack.pop();
		if (m_RecordStack.size())
		{
			RECORDINFO* pParent = m_RecordStack.top();
			pParent->nRecordReadSize += pInfo->nRecordSize - 4;
		}
		delete pInfo;
	}


	unsigned char  CNetStreamExport::getRecordNumber(void)
	{
		// unsigned char by = *( (unsigned char*)(m_pDataStream + m_nStreamPos) );
        unsigned char by;
        memcpy(&by, m_pDataStream + m_nStreamPos, sizeof(unsigned char));
		m_nStreamPos=m_nStreamPos+1;		
		return by;
	}


	//设置数据流读取的位置
	// 	void  CNetStreamExport::setStreamPos(int nPos)
	// 	{
	// 		m_nStreamPos = nPos;
	// 	}	
	// 
	// 	//获取数据流读取的位置
	// 	int	 CNetStreamExport::getStreamPos(void)
	// 	{
	// 		return m_nStreamPos;
	// 	}

	unsigned char	CNetStreamExport::getBYTE(void)
	{	 

		if(m_nStreamPos + 1 > m_nSize )
		{
			ScutLog(" Failed: 长度越界  CNetStreamExport: getBYTE ");
			return 0;	
		}

		//unsigned char by = *( (unsigned char*)(m_pDataStream + m_nStreamPos) );
        unsigned char by;
        memcpy(&by, m_pDataStream + m_nStreamPos, sizeof(unsigned char));
		m_nStreamPos=m_nStreamPos+1;		
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 1;
		}

		return by;
	}

	char	CNetStreamExport::getCHAR(void)
	{	 

		if(m_nStreamPos + 1> m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getCHAR ");
			return 0;	
		}

		//char by = *( (char*)(m_pDataStream + m_nStreamPos) );
        char by;
        memcpy(&by, m_pDataStream + m_nStreamPos, sizeof(char));
		m_nStreamPos=m_nStreamPos+1;		
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 1;
		}		
		return by;
	}


	short	CNetStreamExport::getSHORT(void)
	{
		if(m_nStreamPos + 2 > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getSHORT ");
			return 0;	
		}

		//short w = *( (short*)(m_pDataStream + m_nStreamPos) );
        short w;
        memcpy(&w, m_pDataStream + m_nStreamPos, sizeof(short));
		m_nStreamPos=m_nStreamPos+2;		
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 2;
		}
		return w;

	}


	unsigned short	CNetStreamExport::getWORD(void)
	{
		if(m_nStreamPos + 2 > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getWORD");
			return 0;	
		}

		//unsigned short w = *( (unsigned short*)(m_pDataStream + m_nStreamPos) );
        unsigned short w;
        memcpy(&w, m_pDataStream + m_nStreamPos, sizeof(unsigned short));
		m_nStreamPos=m_nStreamPos+2;
		//m_nRecoredReadSize +=2;
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 2;
		}
		return w;

	}
	unsigned int CNetStreamExport::getDWORD(void)
	{
		if(m_nStreamPos + 4 >= m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getDWORD");
			return 0;	
		}

		//unsigned int dw = *( (unsigned int*)(m_pDataStream + m_nStreamPos) );
        unsigned int dw;
        memcpy(&dw, m_pDataStream + m_nStreamPos, sizeof(unsigned int));
		m_nStreamPos = m_nStreamPos + 4;
		//m_nRecoredReadSize +=4;
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 4;
		}
		return dw;

	}

	int CNetStreamExport::getInt(void)
	{
		if(m_nStreamPos + 4 > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getInt");
			return 0;	
		}

		//int dw = *( (int*)(m_pDataStream + m_nStreamPos) );	
        int dw;
        memcpy(&dw, m_pDataStream + m_nStreamPos, sizeof(int));
		m_nStreamPos=m_nStreamPos+4;
		//m_nRecoredReadSize +=4;
		if (m_RecordStack.size())
		{                                                                                                                     
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 4;
		}
		return dw;
	}

	unsigned long long CNetStreamExport::getInt64()
	{

		if(m_nStreamPos + 8 > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getInt64");
			return 0;	
		}

		unsigned long long value;
		memcpy(&value, m_pDataStream + m_nStreamPos, sizeof(unsigned long long));
		m_nStreamPos = m_nStreamPos + 8;
		//m_nRecoredReadSize += 8;
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 8;
		}
		return value;
	}

	float	CNetStreamExport::getFloat(void)
	{
		if(m_nStreamPos + 4 > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getFloat");
			return 0;	
		}

		//float f = *( (float*)(m_pDataStream + m_nStreamPos) );
        
        float f;
        memcpy(&f, m_pDataStream + m_nStreamPos, sizeof(float));
        
		m_nStreamPos=m_nStreamPos+4;
		//m_nRecoredReadSize +=4;
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 4;
		}
		return f;
	}


	double	CNetStreamExport::getDouble(void)
	{

		if(m_nStreamPos + 8 > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getDouble");
			return 0;	
		}

		//double dw = *( (double*)(m_pDataStream + m_nStreamPos) );
        double dw;
        memcpy(&dw, m_pDataStream + m_nStreamPos, sizeof(double));
		m_nStreamPos = m_nStreamPos+8;
		//m_nRecoredReadSize +=8;
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += 8;
		}
		return dw;
	}


	//在LUA中分配字符内存，传到C中设置内容
	bool	CNetStreamExport::getString(char* psz, int nLength)
	{	
		if(!psz)
			return 0;

		if(!m_pDataStream)
			return 0;

		if(m_nStreamPos + nLength > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getString");
			return 0;	
		}

		char ch;
		int i;
		for ( i= 0; i < nLength; i = i+1)
		{
			ch = *( m_pDataStream + m_nStreamPos + i );
			*(psz + i) = ch;			 
		}
		*(psz + i) = '\0';

		m_nStreamPos = m_nStreamPos + nLength;
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += nLength;
		}
		//m_nRecoredReadSize += nLength;
		return 1;
	}


	//备用接口，在C中分配内存
	const char*	CNetStreamExport::getString(/*const  char* pData,*/int nLength)
	{
		if(m_nStreamPos + nLength > m_nSize )
		{
			ScutLog(" Failed: 长度越界 CNetStreamExport: getString");
			return NULL;	
		}

		m_szRet = (char*)malloc(nLength+1);

		if (m_szRet == NULL)
		{
			return NULL;
		}

		memcpy(m_szRet, m_pDataStream + m_nStreamPos, nLength);
		m_szRet[nLength] = '\0';

		m_nStreamPos = m_nStreamPos + nLength;
		if (m_RecordStack.size())
		{
			RECORDINFO* pInfo = m_RecordStack.top(); 
			pInfo->nRecordReadSize += nLength;
		}
		return m_szRet;

	}

	void  CNetStreamExport::freeStringBuffer()
	{
		if(m_szRet)		
			free(m_szRet);

		m_szRet = NULL;	 
	} 

	bool CNetStreamExport::IsStatusReady()
	{
		return m_RecordStack.empty();
	}

	// end 备用接口


}