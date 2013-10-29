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

#ifndef __SCUT_ANIMATION_MANAGER_H__
#define __SCUT_ANIMATION_MANAGER_H__

#include <string>
#include <map>

using std::string;
using std::map;

namespace ScutAnimation
{
	class CScutAniGroup;
	class CCScutSprite;
	class CScutAnimationManager
	{
	public:
		virtual ~CScutAnimationManager();
		static CScutAnimationManager& GetInstance();
		
	public:
		// 加载指定的动画组,同时返回动画精灵,默认播放动画组中索引值为0的动画
		CCScutSprite* LoadSprite(const char* lpszSprName);
		// 释放指定的动画组
		void UnLoadSprite(const char* lpszSprName);
		// 释放内存中所有的动画组
		void ReleaseAllAniGroup();

	private:
		CScutAnimationManager();
		
	private:
		typedef map<string, CScutAniGroup*> map_ani_group;
		typedef map_ani_group::iterator map_ani_group_it;
		
		map_ani_group m_mapAniGroup;
	};
}

#endif