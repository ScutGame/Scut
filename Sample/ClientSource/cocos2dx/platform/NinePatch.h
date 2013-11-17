#ifndef G_NINE_PATCH_H
#define G_NINE_PATCH_H

#include "cocoa/CCGeometry.h"
#include <list>
using namespace std;

namespace cocos2d
{
struct  G_PairInfo
{
public:
	float mFirst;
	float mSecond;
	G_PairInfo()
	{
		mFirst  = 0;
		mSecond = 0;
	}
	G_PairInfo(float first, float second)
 	{
 		mFirst  = first;
 		mSecond = second;
 	}
	G_PairInfo(const G_PairInfo& other)
 	{
 		mFirst = other.mFirst;
 		mSecond= other.mSecond;
 	}

};
class CCImage;

class  CCNinePatch
{
public:
	CCNinePatch();
	CCNinePatch(CCImage* pngPtr);
	CCNinePatch(const CCNinePatch& other);
	~CCNinePatch(void);

	void findPatches();
	void getRectangles(list<G_PairInfo>&leftParis, list<G_PairInfo>& topPairs, list<CCRect>& outRect);
	void getVerticalRectangles(list<G_PairInfo>& topPairs, list<CCRect>& outRect);
	void getHorizonalRectangles(list<G_PairInfo>& leftPairs, list<CCRect>& outRect);

	void getPatches(int* pixels, int bufferlength, bool& bStartWithPatch, list<G_PairInfo> &outPairLeft, list<G_PairInfo> &outPairRight );
	void computePatches(int scaledWidth, int scaledHeight);

	//输入参数为目标View的实际CCRect大小 坐标
	void drawRenderRect(float left, float bottom, float scaledWidth, float scaledHeight);

	list<CCRect> mlstRenderRect;

	void setImage(CCImage* pImage);
	void setPOTWide(unsigned int nPOTWide){m_nPOTWide = nPOTWide;}
	void setPOTHigh(unsigned int nPOTHigh){m_nPOTHigh = nPOTHigh;}
	void setName(unsigned int uName){m_uName = uName;}
private:
	void drawInRect(CCRect dstRect, CCRect srcRect);
private:
	list<CCRect> mFixedRect;
	list<CCRect> mPatchesRect;
	list<CCRect> mHorizontalPatches;
	list<CCRect> mVerticalPatches;
	CCImage*     mPngPtr;
	bool       mbVerticalStartWithPatch;
	bool       mbHorizontalStartWidthPatch;

	int		   mVerticalPatchesSum;
	int		   mHorizontalPatchesSum;
	int        mRemainderVertical;
	int        mRemainderHorizontal;

	unsigned int m_nPOTWide;
	unsigned int m_nPOTHigh;
	unsigned int       m_uName;
};


}

#endif