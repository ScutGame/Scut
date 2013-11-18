#include "NinePatch.h"
#include "CCImage.h"
#include "ccTypes.h"
#include "CCObject.h"
#include "support/ccUtils.h"
#include "CCPlatformMacros.h"

#include "shaders/ccGLStateCache.h"
#include "shaders/CCGLProgram.h"
using namespace cocos2d;

CCNinePatch::CCNinePatch()
{
	mPngPtr						= NULL;
	mbVerticalStartWithPatch    = false;
	mbHorizontalStartWidthPatch = false;

	mVerticalPatchesSum			= 0;
	mHorizontalPatchesSum		= 0;
	mRemainderVertical			= 0;
	mRemainderHorizontal		= 0;

	m_nPOTWide = 0;
	m_nPOTHigh = 0;
	m_uName = 0;
}

CCNinePatch::CCNinePatch(CCImage* pngPtr)
{
	//这里不对Png是否是9.png的判断
	mPngPtr						= pngPtr;
	mbVerticalStartWithPatch    = false;
	mbHorizontalStartWidthPatch = false;

	mVerticalPatchesSum			= 0;
	mHorizontalPatchesSum		= 0;
	mRemainderVertical			= 0;
	mRemainderHorizontal		= 0;

	m_nPOTWide = 0;
	m_nPOTHigh = 0;
	m_uName = 0;

	findPatches();
}

CCNinePatch::CCNinePatch(const CCNinePatch& other)
{
	mPngPtr						= other.mPngPtr;
	mbVerticalStartWithPatch    = other.mbVerticalStartWithPatch;
	mbHorizontalStartWidthPatch = other.mbHorizontalStartWidthPatch;

	mVerticalPatchesSum			= other.mVerticalPatchesSum;
	mHorizontalPatchesSum		= other.mHorizontalPatchesSum;
	mRemainderVertical			= other.mRemainderVertical;
	mRemainderHorizontal		= other.mRemainderHorizontal;

	mFixedRect					= other.mFixedRect;
	mPatchesRect                = other.mPatchesRect;
	mHorizontalPatches          = other.mHorizontalPatches;
	mVerticalPatches			= other.mVerticalPatches;

	m_nPOTWide = other.m_nPOTWide;
	m_nPOTHigh = other.m_nPOTHigh;
	m_uName = other.m_uName;
}
CCNinePatch::~CCNinePatch(void)
{
	mFixedRect.clear();
	mPatchesRect.clear();
	mHorizontalPatches.clear();
	mVerticalPatches.clear();
	mPngPtr = NULL;
}

void CCNinePatch::findPatches()
{
	int nHeight = mPngPtr->getHeight();
	int nWidth = mPngPtr->getWidth();
	int* rowPtr    = new int[nWidth];
	int* columnPtr = new int[nHeight];
	unsigned char * pData = mPngPtr->getData();
	if (pData)
	{
		//get V H 1 Pixels
		/*int nIndex = (nHeight - 1) * nWidth * 4;
 		for (int i = 0; i < nWidth; i++)
 		{
			int nPixels = 0;
			for (int k = 0; k < 4; k++)
			{
				nPixels += pData[nIndex+k] << (3-k) * 8;
			}
			nIndex += 4;
			rowPtr[i] = nPixels;
 		}
		
		nIndex = 0;
		for (int i = 0; i < nHeight; i++)
		{
			nIndex = i * nWidth * 4;
			int nPixels = 0;
			for (int k = 0; k < 4; k++)
			{
			nPixels += pData[nIndex+k] << (3-k) * 8;
			}
			columnPtr[nHeight - i - 1] = nPixels;
		}*/
 
		//寻找图片顶部的分割线
		int nIndex = 0;
		for (int i = 0; i < nWidth; i++)
		{
			int nPixels = 0;
			for (int k = 0; k < 4; k++)
			{
				nPixels += pData[nIndex+k] << (3-k) * 8;
				pData[nIndex+k] = 0x00;
			}
			nIndex += 4;
			rowPtr[i] = nPixels;
		}
		//寻找图片左边的分割线
		nIndex = 0;
 		for (int i = 0; i < nHeight; i++)
		{
			nIndex = i * nWidth * 4;
			int nPixels = 0;
			for (int k = 0; k < 4; k++)
			{
				nPixels += pData[nIndex+k] << (3-k) * 8;
				pData[nIndex+k] = 0x00;
			}
			columnPtr[i] = nPixels;
 		}

		list<G_PairInfo> leftFixed;
		list<G_PairInfo> leftPatches;

		list<G_PairInfo> topFixed;
		list<G_PairInfo> topPatches;
		getPatches(rowPtr, nWidth, mbHorizontalStartWidthPatch, topFixed, topPatches);
		getPatches(columnPtr, nHeight, mbVerticalStartWithPatch,leftFixed, leftPatches);

		getRectangles(leftFixed, topFixed, mFixedRect);
		getRectangles(leftPatches, topPatches, mPatchesRect);

		if (mFixedRect.size() > 0)
		{
			getRectangles(leftFixed, topPatches, mHorizontalPatches);
			getRectangles(leftPatches, topFixed, mVerticalPatches);
		}
		else
		{
			if (topFixed.size() > 0)
			{
				mHorizontalPatches.clear();
				getVerticalRectangles(topFixed, mVerticalPatches);
			}
			else if (leftFixed.size() > 0)
			{
				getHorizonalRectangles(leftFixed, mHorizontalPatches);
				mVerticalPatches.clear();
			}
			else
			{
				mHorizontalPatches.clear();
				mVerticalPatches.clear();
			}
		}
	}
	else
	{
		
	}
	delete[]rowPtr;
	rowPtr    = NULL;
	delete[]columnPtr;
	columnPtr = NULL;
}

void CCNinePatch::getRectangles(list<G_PairInfo>&leftParis, list<G_PairInfo>& topPairs, list<CCRect>& outRect)
{
	for(list<G_PairInfo>::iterator itLeftBegin = leftParis.begin(); itLeftBegin!= leftParis.end(); itLeftBegin++)
	{
		float y = itLeftBegin->mFirst;
		float height = itLeftBegin->mSecond - itLeftBegin->mFirst;
		for (list<G_PairInfo>::iterator itTopBegin = topPairs.begin(); itTopBegin != topPairs.end(); itTopBegin++)
		{
			float x    = itTopBegin->mFirst;
			float with = itTopBegin->mSecond - itTopBegin->mFirst;

			outRect.push_back(CCRect(x,y,with,height));
		}	
	}
}

void CCNinePatch::getVerticalRectangles(list<G_PairInfo>& topPairs, list<CCRect>& outRect)
{
	list<G_PairInfo>::iterator itBegin = topPairs.begin();
	list<G_PairInfo>::iterator itEnd   = topPairs.end();
	while(itBegin != itEnd)
	{
		float x     = itBegin->mFirst;
		float width = itBegin->mSecond - itBegin->mFirst;
		outRect.push_back(CCRect(x,1.0f,width,(float)mPngPtr->getHeight() -2));
		itBegin++;
	}

}
void CCNinePatch::getHorizonalRectangles(list<G_PairInfo>& leftPairs, list<CCRect>& outRect)
{
	list<G_PairInfo>::iterator itBegin = leftPairs.begin();
	list<G_PairInfo>::iterator itEnd   = leftPairs.end();
	while(itBegin != itEnd)
	{
		float y      = itBegin->mFirst;
		float height = itBegin->mSecond - itBegin->mFirst;
		outRect.push_back(CCRect(1.0f,y,(float)mPngPtr->getWidth() - 2 , height));
		itBegin ++;
	}
}

void  CCNinePatch::getPatches(int* pixels, int bufferlength, bool& bStartWithPatch, list<G_PairInfo> &outPairLeft, list<G_PairInfo> &outPairRight )
{
	if (bufferlength <= 2 || pixels == NULL)
	{
		return;
	}
	float lastIndex = 1.0f;
	float lastPixel = (float)pixels[1];
	bool bFirst = true;

	if (lastPixel == 0xFF)
	{
		bStartWithPatch = true;
	}
	else
	{
		bStartWithPatch = false;
	}

	for (int i = 1; i < bufferlength -1; i++)
	{
		float pixelValue = (float)pixels[i];
		if (pixelValue != lastPixel)
		{
			if (lastPixel == 0xFF)//
			{
				if (bFirst)
				{
					bStartWithPatch = true;
				}
				outPairRight.push_back(G_PairInfo(lastIndex, (float)i));
			}
			else
			{
				outPairLeft.push_back(G_PairInfo(lastIndex, (float)i));	
			}
			bFirst = false;
			lastIndex = (float)i;
			lastPixel = pixelValue;
		}
	}

	if (lastPixel == 0xFF)
	{
		if (bFirst)
		{
			bStartWithPatch = true;
		}
		float temp = bufferlength -1;
		outPairRight.push_back(G_PairInfo(lastIndex, temp));
	}
	else
	{
		float temp = bufferlength -1;
		outPairLeft.push_back(G_PairInfo(lastIndex, temp));
	}

	if (outPairRight.size() == 0)
	{
		int temp     = bufferlength -1;
		int firstPos = 1; 
		outPairRight.push_back(G_PairInfo(firstPos, temp));
		bStartWithPatch = true;
		outPairLeft.clear();
	}
}

void CCNinePatch::computePatches(int scaledWidth, int scaledHeight)
{
	bool measuredWidth = false;
	bool endRow = true;

	int remainderHorizontal = 0;
	int remainderVertical = 0;

	if (mFixedRect.size() > 0) {
		int start = mFixedRect.begin()->origin.y;
		for (list<CCRect>::iterator itBegin = mFixedRect.begin();itBegin != mFixedRect.end(); itBegin++)
		{
			if (itBegin->origin.y > start) 
			{
				endRow = true;
				measuredWidth = true;
			}
			if (!measuredWidth) 
			{
				remainderHorizontal += itBegin->size.width;
			}
			if (endRow) 
			{
				remainderVertical += itBegin->size.height;
				endRow = false;
				start = itBegin->origin.y;
			}
		}
	}

	mRemainderHorizontal = scaledWidth - remainderHorizontal > 0 ? scaledWidth - remainderHorizontal : 0;

	mRemainderVertical = scaledHeight - remainderVertical > 0 ? scaledHeight - remainderVertical : 0;

	mHorizontalPatchesSum = 0;
	if (mHorizontalPatches.size() > 0) 
	{
		int start = -1;
		for (list<CCRect>::iterator itHori = mHorizontalPatches.begin();itHori != mHorizontalPatches.end(); itHori++)
		{
			if (itHori->origin.x > start)
			{
				mHorizontalPatchesSum += itHori->size.width;
				start = itHori->origin.x;
			}
		}
	} 
	else 
	{
		int start = -1;
		for (list<CCRect>::iterator itPatches = mPatchesRect.begin(); itPatches != mPatchesRect.end(); itPatches++) 
		{
			if (itPatches->origin.x > start) 
			{
				mHorizontalPatchesSum += itPatches->size.width;
				start = itPatches->origin.x;
			}
		}
	}

	mVerticalPatchesSum = 0;
	if (mVerticalPatches.size() > 0)
	{
		int start = -1;
		for (list<CCRect>::iterator itVerPatch = mVerticalPatches.begin(); itVerPatch != mVerticalPatches.end(); itVerPatch ++) 
		{
			if (itVerPatch->origin.y > start) 
			{
				mVerticalPatchesSum += itVerPatch->size.height;
				start = itVerPatch->origin.y;
			}
		}
	}
	else 
	{
		int start = -1;
		for (list<CCRect>::iterator itPatchesBegin = mPatchesRect.begin(); itPatchesBegin != mPatchesRect.end(); itPatchesBegin++) 
		{
			if (itPatchesBegin->origin.y > start) 
			{
				mVerticalPatchesSum += itPatchesBegin->size.height;
				start = itPatchesBegin->origin.y;
			}
		}
	}
}

void CCNinePatch::drawRenderRect(float left, float bottom, float scaledWidth, float scaledHeight)
{
	if (scaledWidth <= 1 || scaledHeight <= 1) {
		return;
	}

	if (mPatchesRect.size() == 0) {
		return;
	}

	mlstRenderRect.clear();
	computePatches(scaledWidth, scaledHeight);

	list<CCRect>::iterator itHorizontal = mHorizontalPatches.begin();
	list<CCRect>::iterator itVertical	  = mVerticalPatches.begin();

	list<CCRect>::iterator itFixed      = mFixedRect.begin();
	list<CCRect>::iterator itPatch      = mPatchesRect.begin();

	bool hStretch = true;
	bool vStretch = mbVerticalStartWithPatch;

	float vWeightSum = 1.0f;
	float vRemainder = mRemainderVertical;
	int x = 0;
	int y = 0;

	bool bOut = false;
	CCRect rFirstFixedRect;
	if (itFixed != mFixedRect.end())
	{
		rFirstFixedRect = *itFixed;
	}
	while (y < scaledHeight - 1) 
	{
		hStretch = mbHorizontalStartWidthPatch;

		int height = 0;
		float vExtra = 0.0f;

		float hWeightSum = 1.0f;
		float hRemainder = mRemainderHorizontal;

		while (x < scaledWidth - 1) 
		{
			CCRect r;
			if (!vStretch) 
			{
				if (hStretch) 
				{
					//画竖向无直线与横向直线交叉的区域（一般为上中和下中两个区域）

					if (itHorizontal == mHorizontalPatches.end())
					{
						hStretch = !hStretch;
						break;
					}
					r = *itHorizontal;
					itHorizontal++;
					float extra = r.size.width* 1.0f / mHorizontalPatchesSum;
					int width = (int) (extra * hRemainder / hWeightSum);
					hWeightSum -= extra;
					hRemainder -= width;

					float dstWidth = width;
					float dstHeight = r.size.height;
					if (dstHeight > 0 && dstWidth > 0)
					{
						drawInRect(CCRectMake(left + x, bottom + scaledHeight - y - r.size.height, width, r.size.height), CCRectMake(r.origin.x, r.origin.y, r.size.width, r.size.height));
					}

					x += width;		
				}
				else 
				{
					//画竖向无直线与横向无直线交叉的区域（一般为左下、左上、右下、右上这4个角）

					if (itFixed == mFixedRect.end())
					{
						bOut = true;
						hStretch = !hStretch;
						break;
					}
					r = *itFixed;
		
					itFixed ++;
					drawInRect(CCRectMake(left + x, bottom + scaledHeight - y - r.size.height, r.size.width, r.size.height), CCRectMake(r.origin.x, r.origin.y, r.size.width, r.size.height));

					x += r.size.width;		
					if (rFirstFixedRect.size.width >= scaledWidth - 1)
					{
						while (rFirstFixedRect.origin.y == (*itFixed).origin.y)
						{
							itFixed ++;
						}
					}					
				
				}
				height = r.size.height;	
			} 
			else 
			{
				if (hStretch) 
				{
					//画竖向直线与横向直线交叉的区域（一般为中间区域）

					if (itPatch == mPatchesRect.end())
					{
						hStretch = !hStretch;
						break;
					}
					r = *itPatch;
					itPatch++;
					vExtra = r.size.height* 1.0f / mVerticalPatchesSum;
					height = (int) (vExtra * vRemainder / vWeightSum);
					float extra = r.size.width* 1.0f / mHorizontalPatchesSum;
					int width = (int) (extra * hRemainder / hWeightSum);
					hWeightSum -= extra;
					hRemainder -= width;
					drawInRect(CCRectMake(left + x, bottom + scaledHeight - y - height, width, height), CCRectMake(r.origin.x, r.origin.y, r.size.width, r.size.height));

					x += width;
				}
				else
				{
					//画竖向直线与横向无直线交叉的区域（一般为左中和右中区域）

					if (itVertical == mVerticalPatches.end())
					{
						bOut = true;
						hStretch = !hStretch;
						break;
					}
					r = *itVertical;
					itVertical++;
					
					vExtra = r.size.height * 1.0f / mVerticalPatchesSum;
					height = (int) (vExtra * vRemainder / vWeightSum);

					float dstWidth = r.size.width;
					float dstHeight = height;
					//CCLOG("destX=%f,destY=%f,destWidth=%f,destHeight=%f",dstX,dstY,dstWidth,dstHeight);
					if (dstHeight > 0 && dstWidth > 0)
					{
						drawInRect(CCRectMake(left + x, bottom + scaledHeight - y - height, r.size.width, height), CCRectMake(r.origin.x, r.origin.y, r.size.width, r.size.height));
					}

					x += r.size.width;
				}

			}
			hStretch = !hStretch;
		}

		if (height < 0)
		{
			height = 0;
		}
		
		x = 0;
		y += height;
		if (vStretch) 
		{
			vWeightSum -= vExtra;
			vRemainder -= height;
		}
		if (vRemainder < 0)
		{
			vRemainder = 0;
		}
		
		vStretch = !vStretch;

		if (bOut)
		{
			break;
		}		
	}
}

void cocos2d::CCNinePatch::setImage( CCImage* pImage )
{
	mPngPtr = pImage;
}

void cocos2d::CCNinePatch::drawInRect( CCRect dstRect, CCRect srcRect )
{
	ccV3F_C4B_T2F_Quad m_sQuad;

	memset(&m_sQuad, 0, sizeof(m_sQuad));
	ccColor4B tmpColor = { 255, 255, 255, 255 };
	m_sQuad.bl.colors = tmpColor;
	m_sQuad.br.colors = tmpColor;
	m_sQuad.tl.colors = tmpColor;
	m_sQuad.tr.colors = tmpColor;

	float atlasWidth = (float)m_nPOTWide;
	float atlasHeight = (float)m_nPOTHigh;

	float left, right, top, bottom;
	left	= srcRect.origin.x/atlasWidth;
	right	= left + srcRect.size.width/atlasWidth;
	top		= srcRect.origin.y/atlasHeight;
	bottom	= top + srcRect.size.height/atlasHeight;

	m_sQuad.bl.texCoords.u = left;
	m_sQuad.bl.texCoords.v = bottom;
	m_sQuad.br.texCoords.u = right;
	m_sQuad.br.texCoords.v = bottom;
	m_sQuad.tl.texCoords.u = left;
	m_sQuad.tl.texCoords.v = top;
	m_sQuad.tr.texCoords.u = right;
	m_sQuad.tr.texCoords.v = top;

	float x1 = 0 + dstRect.origin.x;
	float y1 = 0 + dstRect.origin.y;
	float x2 = x1 + dstRect.size.width;
	float y2 = y1 + dstRect.size.height;

	m_sQuad.bl.vertices = vertex3(x1, y1, 0);
	m_sQuad.br.vertices = vertex3(x2, y1, 0);
	m_sQuad.tl.vertices = vertex3(x1, y2, 0);
	m_sQuad.tr.vertices = vertex3(x2, y2, 0);


	ccGLBindTexture2D( m_uName );
	ccGLEnableVertexAttribs( kCCVertexAttribFlag_PosColorTex );

#define kQuadSize sizeof(m_sQuad.bl)
#ifdef EMSCRIPTEN
	long offset = 0;
	setGLBufferData(&m_sQuad, 4 * kQuadSize, 0);
#else
	long offset = (long)&m_sQuad;
#endif // EMSCRIPTEN

	// vertex
	int diff = offsetof( ccV3F_C4B_T2F, vertices);
	glVertexAttribPointer(kCCVertexAttrib_Position, 3, GL_FLOAT, GL_FALSE, kQuadSize, (void*) (offset + diff));

	// texCoods
	diff = offsetof( ccV3F_C4B_T2F, texCoords);
	glVertexAttribPointer(kCCVertexAttrib_TexCoords, 2, GL_FLOAT, GL_FALSE, kQuadSize, (void*)(offset + diff));

	// color
	diff = offsetof( ccV3F_C4B_T2F, colors);
	glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_UNSIGNED_BYTE, GL_TRUE, kQuadSize, (void*)(offset + diff));


	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

//
//	glBindTexture(GL_TEXTURE_2D, m_uName);
//
//	long offset = (long)&m_sQuad;
//#define kQuadSize sizeof(m_sQuad.bl)
//	// vertex
//	int diff = offsetof(ccV3F_C4B_T2F, vertices);
//	glVertexPointer(3, GL_FLOAT, kQuadSize, (void*)(offset + diff));
//
//	// color
//	diff = offsetof( ccV3F_C4B_T2F, colors);
//	glColorPointer(4, GL_UNSIGNED_BYTE, kQuadSize, (void*)(offset + diff));
//
//	// tex coords
//	diff = offsetof( ccV3F_C4B_T2F, texCoords);
//	glTexCoordPointer(2, GL_FLOAT, kQuadSize, (void*)(offset + diff));
//
//	glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
}