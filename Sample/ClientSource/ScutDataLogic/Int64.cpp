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



#include "Int64.h"

namespace ScutDataLogic
{
	CInt64::CInt64(void)
	{
		mValue = 0;
	}
	CInt64::CInt64(unsigned long long value)
	{
		mValue = value;
	}
	CInt64::~CInt64(void)
	{

	}

	CInt64::CInt64(const CInt64& other)
	{
		mValue = other.mValue;
	}

	CInt64& CInt64::operator= (const CInt64& value)
	{
		mValue = value.mValue;
		return *this;
	}
	CInt64& CInt64::operator= (int value)
	{
		mValue = value;
		return *this;
	}

	CInt64 CInt64::operator+( int value ) const
	{
		CInt64  ret;
		ret.mValue = this->mValue + value;
		return ret;
	}

	CInt64 CInt64::operator+ (const CInt64& value) const
	{
		CInt64 ret(mValue + value.mValue);
		return ret;
	}

	CInt64 CInt64::operator- (int value) const
	{
		CInt64 ret(mValue - value);
		return ret;
	}
	CInt64 CInt64::operator- (const CInt64& value) const
	{
		CInt64 ret(mValue - value.mValue);
		return ret;
	}

	CInt64 CInt64::operator* (int value) const
	{
		CInt64 ret(mValue * value);
		return ret;
	}
	CInt64 CInt64::operator* (const CInt64& value) const
	{
		CInt64 ret(mValue * value.mValue);
		return ret;
	}

	CInt64 CInt64::operator/ (int value) const
	{
		CInt64 ret(mValue / value);
		return ret;
	}

	CInt64 CInt64::operator/ (const CInt64& value) const
	{
		CInt64 ret(mValue / value.mValue);
		return ret;
	}

	CInt64 CInt64::operator% (int value) const
	{
		CInt64 ret(mValue / value);
		return ret;
	}
	CInt64 CInt64::operator% (const CInt64& value) const
	{
		CInt64 ret(mValue / value.mValue);
		return ret;
	}

	bool CInt64::operator== (const CInt64& value) const
	{
		return mValue == value.mValue;
	}
	bool CInt64::operator<= (const CInt64& value) const
	{
		return mValue <= value.mValue;
	}
	bool CInt64::operator< (const CInt64& value) const
	{
		return mValue < value.mValue;
	}
	bool CInt64::operator> (const CInt64& value) const
	{
		return mValue > value.mValue;
	}
	bool CInt64::operator>= (const CInt64& value) const
	{
		return mValue >= value.mValue;
	}

	bool CInt64::equal(const CInt64& value) const
	{
		return mValue == value.mValue;
	}
	std::string CInt64::str()
	{
		char szBuf[100] = {0};
#ifdef SCUT_WIN32
		sprintf(szBuf, "%I64d", mValue);
#else
		sprintf(szBuf, "%lld", mValue);
#endif
		return std::string(szBuf);
	}
	unsigned long long CInt64::getValue()const
	{
		return mValue;
	}

	CInt64 CInt64::mod(int value) const
	{
		CInt64 ret(mValue %value);
		return ret;
	}
	CInt64 CInt64::mod(const CInt64& value) const
	{
		CInt64 ret(mValue %value.mValue);
		return ret;
	}
}

