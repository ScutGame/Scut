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

#include "SpriteFileStream.h"

namespace ScutAnimation {
	unsigned char CSpriteFileStream::ReadByte()
	{
		unsigned char buffer[1] = {0};
		this->Read((char*)buffer, sizeof(buffer));
		return buffer[0];
	}
	
	SHORT CSpriteFileStream::ReadShort()
	{
		unsigned char buffer[2] = {0};
		this->Read((char*)buffer, sizeof(buffer));
		return SHORT((buffer[0] << 8) + buffer[1]);
	}
	
	INT CSpriteFileStream::ReadInt()
	{
		unsigned char buffer[4] = {0};
		this->Read((char*)buffer, sizeof(buffer));
		return INT((buffer[0] << 24) + (buffer[1] << 16) + (buffer[2] << 8) + buffer[3]);
	}

	float CSpriteFileStream::ReadFloat()
	{
		unsigned char buffer[4] = {0};
		this->Read((char*)buffer, sizeof(buffer));

		char buf[4] = {buffer[3], buffer[2], buffer[1], buffer[0]};
		return *(float*)buf;
	}
	
	void CSpriteFileStream::ReadUTF(string& str)
	{
		str.clear();
		int strLen = this->ReadShort();
		if (strLen > 0 && strLen < MAX_PATH)
		{
			char buffer[MAX_PATH] = {0};
			this->Read(buffer, strLen);
			str = buffer;
		}
	}
}
