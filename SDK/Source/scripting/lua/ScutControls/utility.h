#ifndef _CUTILITY_H_
#define _CUTILITY_H_
#include <string>
#include "CCPlatformMacros.h"
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
#include <iconv.h>
#endif
#include "../cocos2dx_support/LuaEngine.h"
#include "cocos2d.h"
#include "CCGeometry.h"
#include "platform.h"

//using namespace cocos2d;



/**
* @brief 跟据X，Y生成CCPoint
* @n<b>函数名称</b>					: PT
* @n@param  float x坐标
* @param    float y坐标
* @remarks   
* @see		
*/



cocos2d::CCPoint PT(float x, float y);



/**
* @brief 跟据W，H生成CCSize
* @n<b>函数名称</b>					: SIZE
* @n@param  float 宽
* @param    float 高
* @remarks   
* @see		
*/

cocos2d::CCSize SZ(float width, float height); 



/**
* @brief  生成适应当前缩放比率的X坐标
* @n<b>函数名称</b>					: SX
* @n@param  float x
* @remarks   
* @see		
*/
float SX(float x);


/**
* @brief  生成适应当前缩放比率的Y坐标
* @n<b>函数名称</b>					: SY
* @n@param  float y
* @remarks   
* @see		
*/
float SY(float y);



namespace NdCxControl
{

	/**
	* @brief  整数转换字符串
	* @n<b>函数名称</b>					: IntToString
	* @n@param  int 整数
	* @return  字符串
	* @remarks   
	* @see		
	*/
	std::string IntToString(int nValue);

	/**
	* @brief  DWORD转换字符串
	* @n<b>函数名称</b>					: DWORDToString
	* @n@param  unsigned 数值
	* @return  字符串
	* @remarks   
	* @see		
	*/
	std::string DWORDToString(unsigned long dwValue);


	/**
	* @brief  字符编码转换Utf8转为GB2312
	* @n<b>函数名称</b>					: Utf8ToGB2312
	* @n@param  char * 字符串
	* @param  int 字符串长度
	* @remarks   
	* @see		
	*/
	std::string  Utf8ToGB2312(const char *src, int src_size);

	/**
	* @brief  字符编码转换GB2312转为Utf8
	* @n<b>函数名称</b>					: GB2312ToUtf8
	* @n@param  char * 字符串
	* @param  int 字符串长度
	* @remarks   
	* @see		
	*/
	std::string  GB2312ToUtf8(const char *src, int src_size);


	/**
	* @brief  求两点距离
	* @n<b>函数名称</b>					: Distance
	* @n@param  CCPoint 起始点
	* @param  CCPoint 终止点
	* @remarks   
	* @see		
	*/
	float Distance(cocos2d::CCPoint pt1,cocos2d::CCPoint pt2);


	/**
	* @brief  生成适应当前缩放比率的X坐标
	* @n<b>函数名称</b>					: SCALEX
	* @n@param  float x
	* @remarks   
	* @see		
	*/
	float SCALEX(int x);


	/**
	* @brief  生成适应当前缩放比率的Y坐标
	* @n<b>函数名称</b>					: SCALEY
	* @n@param  float y
	* @remarks   
	* @see		
	*/
	float SCALEY(int y);



	/**
	* @brief  由文件获取TEXTUR
	* @n<b>函数名称</b>					: TEXTURE
	* @n@param  string 文件名
	* @remarks   
	* @see		
	*/
	cocos2d::CCTexture2D * TEXTURE(const std::string & strFile);




	/**
	* @brief 文件编码转换
	* @remarks   
	* @see		
	*/
	class LUA_DLL CodeConverter 
	{
	private:
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
		iconv_t cd;
#endif
	public:
		CodeConverter(const char *from_charset, const char *to_charset);
		~CodeConverter();
		int convert(const char *inbuf, int inlen, char *outbuf, int outlen);
	};


	/**
	* @brief 常用功能
	* @remarks   
	* @see		
	*/
	class LUA_DLL CUtility
	{
	public:
		static CUtility* sharedUtility(void);
		//URL编码
		float GetScaleX(int x){return (float)x*m_fScaleX;}
		float GetScaleY(int y){return (float)y*m_fScaleY;}
		static cocos2d::CCTexture2D *getTextureFrom(const std::string &file1);
		int getMainBkWidth();
		int getMainBkHeight();
		void setMainBkWidth(int nWidth);
		void setMainBkHeight(int nHeight);


	private:
		CUtility(void);
	private:
		float m_fScaleX;
		float m_fScaleY;
		int m_nMainBKWidth ;
		int m_nMainBKHeight ;
	};

}
#endif
