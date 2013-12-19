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
#include "LuaString.h"

namespace ScutDataLogic
{
	CLuaString::CLuaString(std::string strValue)
	{
		m_strValue = strValue;
	}
	CLuaString::CLuaString()
	{

	}

	CLuaString::~CLuaString(void)
	{
	}

	void CLuaString::setString(const char* szValue)
	{
		if (szValue)
		{
			m_strValue = szValue;
		}
	}
	const char* CLuaString::getCString()
	{
		return m_strValue.c_str();
	}
	int CLuaString::getSize()
	{
		return m_strValue.size();
	}

}
