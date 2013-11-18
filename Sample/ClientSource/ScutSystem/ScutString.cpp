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
#include "ScutString.h"
//#include <regexp.h>
//#include "LogFile.h"

using namespace ScutSystem;

//extern LogFile logFile;

void CScutString::ReplaceStr( const char *crefPattern, const char *crefReplaceBy )
{
	int iPatternLen = (int)strlen(crefPattern);
	if( !iPatternLen )
		return;
	int iPos =  find(crefPattern);
	int iStartPos = 0;
	bool bModified = false;
	CScutString strResult;
	while( iPos != -1 )
	{
		//signal there were replacements made
		bModified = true;
		strResult += substr((size_t)iStartPos, iPos-iStartPos) + crefReplaceBy;		
		
		//search for the next occurence of pattern
		iStartPos = iPos + iPatternLen;
		iPos = find(crefPattern, iStartPos);
	}
	// add the remaining string
	strResult += substr((size_t)iStartPos);
	
	//copy resulting string to ourselves, if replacements took place
	if( bModified )
		*this = strResult;
}
