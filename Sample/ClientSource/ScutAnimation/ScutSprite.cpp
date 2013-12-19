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

#include "ScutSprite.h"
#include "FileHelper.h"
#include "ScutAniGroup.h"
#include "ScutAniData.h"
#include "ScutFrame.h"
#include "ScutTile.h"
#include "LuaHost.h"
#include "cocos2d.h"

using namespace ScutDataLogic;
using namespace ScutSystem;

namespace ScutAnimation
{
	CCScutSprite::CCScutSprite()
	{
		m_aniGroup = NULL;
		m_curAni = NULL;
		m_curFrame = NULL;
		m_curAniIndex = 0;
		m_curFrameIndex = 0;
		m_curAniFrameCount = 0;
		m_curFrameDuration = 0;
		m_bPlay = false;
		m_vReplaceTexture.assign(REPLACE_COUNT, NULL);
	}
	
	CCScutSprite::~CCScutSprite()
	{
		CC_SAFE_RELEASE(this->m_aniGroup);

		for (vec_texture_it itTex = this->m_vReplaceTexture.begin(); itTex != this->m_vReplaceTexture.end(); itTex++)
		{
			CC_SAFE_RELEASE((*itTex));
		}
	}
	
	CCScutSprite* CCScutSprite::node(CScutAniGroup* aniGroup)
	{
		assert(aniGroup != NULL);
		if (aniGroup)
		{
			aniGroup->retain();
			CCScutSprite* pRet = new CCScutSprite;
			pRet->m_aniGroup = aniGroup;
			pRet->autorelease();
			pRet->setCurAni(0);
			return pRet;
		}
		return NULL;
	}
	
	void CCScutSprite::setCurAni(int aniIndex)
	{
		assert(this->m_aniGroup != NULL);
		this->m_curAni = this->m_aniGroup->getAniData(aniIndex);
		assert(this->m_curAni != NULL);
		if (this->m_curAni)
		{
			this->setContentSize(this->m_curAni->m_tContentSize);
			this->m_curAniIndex = aniIndex;
			this->m_curFrameIndex = 0;
			this->m_curAniFrameCount = this->m_curAni->getFrameCount();
			this->m_curFrame = this->m_curAni->getFrameByIndex(this->m_curFrameIndex);
			this->m_curFrameDuration = 0;
			assert(this->m_curFrame != NULL);
			m_bPlay = false;
			this->execFrameCallback(ANIMATION_START);
		}
	}
	
	void CCScutSprite::draw(void)
	{
		assert(this->m_aniGroup && this->m_curAni && this->m_curFrame);
		if (this->m_aniGroup && this->m_curAni && this->m_curFrame)
		{
#ifdef WIN32
			// strange, ios is ok without this call
			glColor4ub(255, 255, 255, 255);
#endif

            
		int tileCount = this->m_curFrame->getTileCount();
		CScutTile* pTile = NULL;
		CCTexture2D* pTexture = NULL;
		for (int i = 0; i < tileCount; i++)
		{
			pTexture = NULL;
			pTile = this->m_curFrame->getTileByIndex(i);
			assert(pTile != NULL);
			if (NULL == pTile)
			{
				continue;
			}
			ReplaceTexture texture = this->m_aniGroup->getTextureByTile(pTile);
			if (texture.replaceIndex >= REPLACE_START && texture.replaceIndex < REPLACE_COUNT)
			{
				pTexture = this->m_vReplaceTexture[texture.replaceIndex];
			}
                
			if (NULL == pTexture)
			{
				pTexture = texture.pTex;
			}
				
			assert(pTexture);
			if (pTexture)
			{
				kmGLPushMatrix();
				//glTranslatef(pTile->m_centerX, pTile->m_centerY, 0.0f);
				//glRotatef(pTile->m_rotation, 0.0f, 0.0f, -1.0f);
				//glScalef(pTile->m_flipX ? -1.0f : 1.0f, pTile->m_flipY ? -1.0f : 1.0f, 1.0f);
				pTexture->drawCustom(pTile->m_tt.texCoord, pTile->vertices);
				kmGLPopMatrix();
			}
		}

			if (m_bPlay && this->m_curFrameIndex < this->m_curAniFrameCount)
			{
				this->m_curFrameDuration++;
				if (this->m_curFrameDuration >= this->m_curFrame->m_duration)
				{
					this->m_curFrameDuration = 0;
					this->m_curFrameIndex++;

					if (this->m_curFrameIndex == this->m_curAniFrameCount)
					{
						switch (this->m_curAni->m_type)
						{
							case CScutAniData::ANI_TYPE_CYCLE:
								this->m_curFrameIndex = 0;
								break;
							case CScutAniData::ANI_TYPE_ONCE_START:
								this->m_curFrame = this->m_curAni->getFrameByIndex(0);
								break;
							case CScutAniData::ANI_TYPE_ONCE_END:
								this->m_curFrame = this->m_curAni->getFrameByIndex(this->m_curAniFrameCount - 1);
								break;
							default:
								assert(false);
								break;
						}
					}
					
					if (this->m_curFrameIndex < this->m_curAniFrameCount)
					{
						this->m_curFrame = this->m_curAni->getFrameByIndex(this->m_curFrameIndex);
						int nPlayingFlag = ANIMATION_PLAYING;
						if (this->m_curFrameIndex == 0)
						{
							nPlayingFlag = ANIMATION_START;
						}
						else if (this->m_curFrameIndex == this->m_curAniFrameCount - 1)
						{
							nPlayingFlag = ANIMATION_END;
						}
						
						this->execFrameCallback(nPlayingFlag);
					}
				}
			}
		}
	}

	
	void CCScutSprite::registerFrameCallback(const char* pszCallback)
	{
		if (pszCallback)
		{
			m_strFrameCallback = pszCallback;
		}
		else
		{
			m_strFrameCallback = "";
		}
	}

	void CCScutSprite::execFrameCallback(int nPlayingFlag)
	{
		if (m_strFrameCallback.size() > 0)
		{
			ScutDataLogic::LuaHost::Instance()->execSpriteCallback(m_strFrameCallback, (void*)this, m_curAniIndex, m_curFrameIndex, nPlayingFlag);
		}
	}

	void CCScutSprite::play(bool bPlay /* = true */)
	{
		m_bPlay = bPlay;
	}

	void CCScutSprite::replace(int replaceIndex, const char* pszFile)
	{
		assert(pszFile != NULL);
		assert(replaceIndex >= REPLACE_START && replaceIndex < REPLACE_COUNT);
		if (pszFile && replaceIndex >= REPLACE_START && replaceIndex < REPLACE_COUNT)
		{
			CC_SAFE_RELEASE(this->m_vReplaceTexture[replaceIndex]);
			this->m_vReplaceTexture[replaceIndex] = NULL;

			string texFile = pszFile;
			texFile = SPRITE_IMAGE_DIR + texFile;
			texFile = CFileHelper::getPath(texFile.c_str());
			CCTexture2D* pTex = CCTextureCache::sharedTextureCache()->addImage(texFile.c_str());
			assert(pTex != NULL);
			CC_SAFE_RETAIN(pTex);
			this->m_vReplaceTexture[replaceIndex] = pTex;
		}
	}

	int CCScutSprite::getAniCount()
	{
		if (m_aniGroup != NULL)
		{
			return this->m_aniGroup->getAniCount();
		}
		return 0;
	}

	CCScutSprite* CCScutSprite::CopyFromSprite(CCScutSprite* src)
	{
		if (src->m_aniGroup != NULL)
		{
			src->m_aniGroup->retain();
			CCScutSprite* pRet = new CCScutSprite;
			pRet->autorelease();
			pRet->m_aniGroup = src->m_aniGroup;
			pRet->m_curAni = src->m_curAni;
			pRet->m_curFrame = src->m_curFrame;
			pRet->m_curAniIndex = src->m_curAniIndex;
			pRet->m_curFrameIndex = src->m_curFrameIndex;
			pRet->m_curAniFrameCount = src->m_curAniFrameCount;
			pRet->m_curFrameDuration = src->m_curFrameDuration;
			pRet->m_strFrameCallback = src->m_strFrameCallback;
			pRet->m_bPlay = src->m_bPlay;

			for (vec_texture_it itTex = src->m_vReplaceTexture.begin(); itTex != src->m_vReplaceTexture.end(); itTex++)
			{
				CC_SAFE_RETAIN((*itTex));
			}

			pRet->m_vReplaceTexture = src->m_vReplaceTexture;

			return pRet;
		}
		return NULL;
	}
}