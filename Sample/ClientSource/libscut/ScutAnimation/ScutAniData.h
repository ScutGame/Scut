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

#ifndef __SCUT_ANI_DATA_H__
#define __SCUT_ANI_DATA_H__

#include "cocos2d.h"
#include <vector>

using namespace std;
using namespace cocos2d;

namespace ScutAnimation
{
	class CScutFrame;
	class CScutAniData
	{
	public:
		enum ANI_TYPE
		{
			ANI_TYPE_CYCLE,
			ANI_TYPE_ONCE_END,
			ANI_TYPE_ONCE_START,
		};
		
	public:
		CScutAniData();
		~CScutAniData();
		// 获取动画帧数
		int getFrameCount();
		// 获取指定帧
		CScutFrame* getFrameByIndex(int frameIndex);
		
	public:
		int m_type;
		
		CCSize m_tContentSize;

		float m_anchorX;
		float m_anchorY;
	
		typedef vector<CScutFrame*> vec_frame;
		typedef vec_frame::iterator vec_frame_it;
		vec_frame m_vFrame;
	};
}

#endif