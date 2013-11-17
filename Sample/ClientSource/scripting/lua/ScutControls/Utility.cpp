#include "Utility.h"
#include "cocos2d.h"
#include <stdlib.h>
#include <cmath>
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
#include <io.h>
#else
#include <unistd.h>
#endif
#include "../cocos2dx_support/LuaEngine.h"

#define STANDARD_SCREEN_WIDTH 480
#define STANDARD_SCREEN_HEIGHT 320


using namespace cocos2d;
//URL±àÂë



	 
 
		
	cocos2d::CCPoint PT(float x, float y)
	{
		return cocos2d::CCPoint(x, y);
	}
	 
	cocos2d::CCSize SZ(float width, float height)
	{
		return cocos2d::CCSize(width, height);
	}
 
	float SX(float x)
	{
		return NdCxControl::SCALEX(x);
	}

	float SY(float y)
	{
		return NdCxControl::SCALEY(y);
	}

	 

namespace NdCxControl
{


	 

 
		CodeConverter::CodeConverter(const char *from_charset, const char *to_charset) 
		{
			#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
			cd = iconv_open(to_charset,from_charset);
			#endif
		}

		CodeConverter::~CodeConverter() 
		{
			#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
			if ((iconv_t)-1 != cd)
			{
				iconv_close(cd);
			}
			#endif
		}

		int CodeConverter::convert(const char *inbuf, int inlen, char *outbuf, int outlen) 
		{
			#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)

			if ((iconv_t)-1 == cd)
			{
				return 0;
			}

			const char **pin = &inbuf;
			char **pout = &outbuf;

			memset(outbuf,0,outlen);
			return iconv(cd, pin, (size_t *)&inlen, pout, (size_t *)&outlen);

			#endif

			return 0;

		}






	std::string IntToString(int nValue)
	{
		char szBuf[20];
		sprintf(szBuf, "%d", nValue);
		return std::string(szBuf);
	}
	std::string DWORDToString(unsigned long dwValue)
	{
		char szBuf[20];
		sprintf(szBuf, "%d", (int)dwValue);
		return std::string(szBuf);
	}
	std::string Utf8ToGB2312(const char *src, int src_size)
	{
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
		CodeConverter convert("utf-8", "gb2312");
		char outbuf[2048];
		int rst = convert.convert(src, src_size, outbuf, 2048);
		if (0 == rst)
		{
			return std::string(outbuf);
		}
		else
		{
			return std::string(src);
		}
#else
		return std::string(src);
#endif
	}

	
	std::string GB2312ToUtf8( const char *src, int src_size )
	{
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
		CodeConverter cc("gb2312", "utf-8");
		char outbuf[2048];
		int rst = cc.convert((char *)src, src_size, (char *)outbuf, 2048);
		if (0 == rst)
		{
			return std::string(outbuf);
		}
		else
		{
			return std::string(src);
		}
#else
		return std::string(src);
#endif
	}



	float SCALEX(int x)
	{
		return CUtility::sharedUtility()->GetScaleX(x);
	}

	float SCALEY(int y)
	{
		return CUtility::sharedUtility()->GetScaleY(y);
	}

	cocos2d::CCTexture2D * TEXTURE(const std::string & strFile)
	{
		return CUtility::sharedUtility()->getTextureFrom(strFile);
	}

	float Distance(cocos2d::CCPoint pt1,cocos2d::CCPoint pt2)
	{
		float x =(int)abs(pt1.x-pt2.x);
		float y =(int)abs(pt1.y-pt2.y);
		
		return sqrt(pow(x,2)+pow(y,2));
	}
	
	CUtility::CUtility(void)
	{
		m_nMainBKWidth = 0;
		m_nMainBKHeight =0;
		m_fScaleX = CCDirector::sharedDirector()->getWinSize().width/(float)STANDARD_SCREEN_WIDTH;
		m_fScaleY = CCDirector::sharedDirector()->getWinSize().height/STANDARD_SCREEN_HEIGHT;
	}

	CUtility* CUtility::sharedUtility()
	{
		static CUtility gUtility;
		return &gUtility;
	}

	cocos2d::CCTexture2D *CUtility::getTextureFrom(const std::string &file1)
	{
		CCTexture2D *pTexture =  CCTextureCache::sharedTextureCache()->addImage(file1.c_str());
		return pTexture;
	}
	int CUtility::getMainBkWidth()
	{
		return m_nMainBKWidth ;
	}
	int CUtility::getMainBkHeight()
	{
		return m_nMainBKHeight ;
	}
	void CUtility::setMainBkWidth(int nWidth)
	{
		m_nMainBKWidth = nWidth ;
	}
	void CUtility::setMainBkHeight(int nHeight)
	{
		m_nMainBKHeight = nHeight ;
	}

}