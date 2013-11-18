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


#include "StdAfx.h"
#include "XmlDataHandler.h"

using namespace ScutDataLogic;

CXmlDataHandler::CXmlDataHandler(void)
{
}

CXmlDataHandler::~CXmlDataHandler(void)
{
}

bool ScutDataLogic::CXmlDataHandler::HandleData( int nTag, CStream& dataStream, LPVOID lpData )
{
	bool bRet = false;
	CMarkup* pMarkup = this->ParseMarkupData(dataStream);
	if (pMarkup)
	{
		bRet = DoHandleData(nTag, pMarkup, lpData);
		delete pMarkup;
	}
	return bRet;
}

bool ScutDataLogic::CXmlDataHandler::HandleProgress( int nTag, int nProgress, LPVOID lpData )
{
	return false;
}

CMarkup* ScutDataLogic::CXmlDataHandler::ParseMarkupData( CStream& dataStream )
{
	int nSize = dataStream.GetSize();
	if (nSize == 0)
	{
		return NULL;
	}
    
	CMarkup* pMarkup = new CMarkup();
	char* pBuffer = new char[nSize + 1];
	dataStream.SetPosition(0);
    
	if (dataStream.ReadBuffer(pBuffer, nSize))
	{
		pBuffer[nSize] = '\0';
#ifdef WIN32
		if (pMarkup->DetectUTF8(pBuffer, nSize))
		{
			std::string temp = pMarkup->UTF8ToA(pBuffer);
            //std::string temp = utf8ToGB2312(pBuffer, nSize);
			pMarkup->SetDoc(temp.c_str());
		}	
		else
			pMarkup->SetDoc(pBuffer, nSize);
#else
		pMarkup->SetDoc(pBuffer, nSize);
#endif        		
	}
	delete []pBuffer;
	return pMarkup;
}

bool ScutDataLogic::CXmlDataHandler::DoHandleData( int nTag, CMarkup* pMarkup, LPVOID lpData )
{
	return false;
}
