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

#include "ScutAniData.h"
#include "Defines.h"
#include "ScutFrame.h"

namespace ScutAnimation
{
	CScutAniData::CScutAniData()
	{
		m_type = 0;
		m_tContentSize = CCSizeZero;
		m_anchorX = 0;
		m_anchorY = 0;
	}
	
	CScutAniData::~CScutAniData()
	{
		for (vec_frame_it it = this->m_vFrame.begin(); it != this->m_vFrame.end(); it++)
		{
			CC_SAFE_DELETE((*it));
		}
	}
	
	int CScutAniData::getFrameCount()
	{
		return this->m_vFrame.size();
	}
	
	CScutFrame* CScutAniData::getFrameByIndex(int frameIndex)
	{
		int frameCount = this->m_vFrame.size();
		assert(frameIndex > -1 && frameIndex < frameCount);
		if (frameIndex > -1 && frameIndex < frameCount)
		{
			return this->m_vFrame[frameIndex];
		}
		return NULL;
	}
}
