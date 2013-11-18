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



#ifndef _CINT64_H
#define _CINT64_H
#include <string>
namespace ScutDataLogic
{
	class CInt64
	{
	public:
		CInt64(void);
		~CInt64(void);
		CInt64(unsigned long long value);

		CInt64(const CInt64& other);
		
		CInt64 operator+ (int value) const;
		CInt64 operator+ (const CInt64& value) const;

		CInt64 operator- (int value) const;
		CInt64 operator- (const CInt64& value) const;

		CInt64 operator* (int value) const;
		CInt64 operator* (const CInt64& value) const;

		CInt64 operator/ (int value) const;
		CInt64 operator/ (const CInt64& value) const;



		bool operator== (const CInt64& value) const;
		bool operator<= (const CInt64& value) const;
		bool operator< (const CInt64& value) const;
		
		bool operator>= (const CInt64& value) const;
		bool equal(const CInt64& value) const;
		std::string str();

		unsigned long long getValue()const;

		CInt64 mod(int value) const;
		CInt64 mod(const CInt64& value) const;


		/*****************以下接口Lua中不支持********************/
		CInt64 operator% (int value) const;
		CInt64 operator% (const CInt64& value) const;
		CInt64& operator= (const CInt64& value);
		CInt64& operator= (int value);
		bool operator> (const CInt64& value) const;

	private:
		unsigned long long mValue;
	};

}

#endif//_CINT64_H