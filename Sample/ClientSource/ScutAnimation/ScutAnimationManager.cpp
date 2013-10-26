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

#include "ScutAnimationManager.h"
#include "SpriteFileStream.h"
#include "FileHelper.h"
#include "ScutAniGroup.h"
#include "ScutSprite.h"

using namespace ScutDataLogic;
using namespace ScutSystem;

namespace ScutAnimation
{
	CScutAnimationManager& CScutAnimationManager::GetInstance()
	{
		static CScutAnimationManager mgr;
		return mgr;
	}
	
	CScutAnimationManager::CScutAnimationManager()
	{
		
	}
	
	CScutAnimationManager::~CScutAnimationManager()
	{
		
	}
	
	void CScutAnimationManager::UnLoadSprite(const char* lpszSprName)
	{
		if (!lpszSprName)
		{
			return;
		}
		
		map_ani_group_it it = this->m_mapAniGroup.find(lpszSprName);
		if (it != this->m_mapAniGroup.end())
		{
			CC_SAFE_RELEASE(it->second);
			this->m_mapAniGroup.erase(it);
		}
	}
	
	CCScutSprite* CScutAnimationManager::LoadSprite(const char* lpszSprName)
	{
		if (!lpszSprName)
		{
			return NULL;
		}
		
		// 已经载入内存
		map_ani_group_it it = this->m_mapAniGroup.find(lpszSprName);
		if (it != this->m_mapAniGroup.end())
		{
			return CCScutSprite::node(it->second);
		}
		
		CScutAniGroup* pAniGroup = new CScutAniGroup;
		
		if (!pAniGroup->Load(lpszSprName))
		{
			pAniGroup->release();
			return NULL;
		}
		
		this->m_mapAniGroup[lpszSprName] = pAniGroup;
		return CCScutSprite::node(pAniGroup);
	}
	
	void CScutAnimationManager::ReleaseAllAniGroup()
	{
		for (map_ani_group_it it = this->m_mapAniGroup.begin(); it != this->m_mapAniGroup.end(); it++)
		{
			CC_SAFE_RELEASE(it->second);
		}
		this->m_mapAniGroup.clear();
	}
}

