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
#include "DataHandler.h"


using namespace ScutDataLogic;

#define DATAHANDLER_ALIVE_SIGN 0xA08A0E3;

CDataHandler::CDataHandler(void)
{
	m_nAliveSign = DATAHANDLER_ALIVE_SIGN;
}

CDataHandler::~CDataHandler(void)
{
	m_nAliveSign = 0;
}

bool ScutDataLogic::CDataHandler::HandleData( int nTag, CStream& dataStream, LPVOID lpData )
{
	return false;
}

bool ScutDataLogic::CDataHandler::HandleProgress( int nTag, int nProgress, LPVOID lpData )
{
	return false;
}

bool ScutDataLogic::CDataHandler::IsAlive()
{
	return m_nAliveSign == DATAHANDLER_ALIVE_SIGN;
}

bool ScutDataLogic::CDataHandler::HandleFailed( int nTag, ERequestError re )
{
	return false;
}
