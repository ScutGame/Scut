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

#ifndef __SCUT_SPRITE_H__
#define __SCUT_SPRITE_H__

#include "cocos2d.h"
#include <vector>

using namespace std;

USING_NS_CC;

namespace ScutAnimation
{
	enum
	{
		ANIMATION_START,
		ANIMATION_PLAYING,
		ANIMATION_END,
	};

	enum REPLACE_INDEX
	{
		REPLACE_START,
		REPLACE_WEAPON = REPLACE_START,
		REPLACE_COUNT,
	};

	class CScutFrame;
	class CScutAniData;
	class CScutAniGroup;
	class CCScutSprite : public CCNode
	{
	public:
		static CCScutSprite *node(CScutAniGroup* aniGroup);
		static CCScutSprite *CopyFromSprite(CCScutSprite* src);
		
		// 设置精灵当前播放的动画
		void setCurAni(int aniIndex);
		
		virtual void draw(void);

		void registerFrameCallback(const char* pszCallback);

		void play(bool bPlay = true);

		void replace(int replaceIndex, const char* pszFile);

		int getAniCount();
		
	private:
		CCScutSprite();
		~CCScutSprite();
		void execFrameCallback(int nPlayingFlag);
		
	protected:
		CScutAniGroup* m_aniGroup;
		CScutAniData* m_curAni;
		CScutFrame* m_curFrame;
		int m_curAniIndex;
		int m_curFrameIndex;
		int m_curAniFrameCount;
		int m_curFrameDuration;
		std::string m_strFrameCallback;
		bool m_bPlay;

		// 用于替换原始图片的资源
		typedef vector<CCTexture2D*> vec_texture;
		typedef vec_texture::iterator vec_texture_it;
		vec_texture m_vReplaceTexture;
	};
}

#endif