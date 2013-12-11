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
#include "ScutSMS.h"

#ifdef SCUT_ANDROID
#include "ANDROID/ScutSmsUtilsjni.h"
#elif SCUT_IPHONE
	#ifndef SCUT_IPHONE_APPSTORE
	#include "iphone/iosSMS.h"
	#endif
#endif

namespace ScutSystem
{
	CScutSMS::CScutSMS(void)
	{

	}

	CScutSMS::~CScutSMS(void)
	{

	}

	bool CScutSMS::sendSMS(std::string strMessage, std::string strNumber, bool silence)
	{
#ifdef SCUT_ANDROID
		return ANDROIDSendSms(strMessage,strNumber);
#endif

#ifdef SCUT_IPHONE
#ifdef SCUT_IPHONE_APPSTORE
		return false;
#else
		return iosSendSMS(strMessage,strNumber, silence);
#endif
#endif
		return false;
	}

	bool CScutSMS::canSendSMS()
	{
#ifdef SCUT_ANDROID
		return true;
#endif

#ifdef SCUT_IPHONE
#ifdef SCUT_IPHONE_APPSTORE
		return false;
#else
		return iosCanSendSMS();
#endif
#endif
		return false;
	}
}

