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
#pragma once
#include <string>
#include <map>

namespace ScutUtility
{
	class CScutLan;

	/**
	* @brief 语言工厂类
	* @remarks
	* @see
	*/
	class CScutLanFactory
	{
	private:
		CScutLanFactory(){}
		~CScutLanFactory(){}

	public:
		static CScutLan* GetLanInstance();

	private:
		static CScutLan* Create(const char* szLan); 

	private:
		static CScutLanFactory* m_instance;
		static CScutLan* m_lanInstance;
	};

	/**
	* @brief 语言基类
	* @remarks 每次只支持一种语言，不支持切换，切换语言只在下次启动生效
	* @see
	*/
	class CScutLan
	{
	protected:
		CScutLan(){};
		virtual ~CScutLan(){}

	public:
		std::string m_szOK;
		std::string m_szCancel;
		std::string m_szKnown;
		std::string m_szTimeOut;
		std::string m_szFalseConnect;
		std::string m_szUpdateError;
		std::string m_szDownload;
		std::string m_szExit;
		std::string m_szSDNotExist;
		std::string m_szSDReadError;
		std::string m_szResPackageLoadingTip;
		std::string m_szResPackageFinishTip;
	};

	//简体中文
	class CScutLanCn : public CScutLan
	{
	public:
		CScutLanCn();
		~CScutLanCn(){}
	};

	//英语
	class CScutLanUs : public CScutLan
	{
	public:
		CScutLanUs();
		~CScutLanUs(){}
	};

	//繁体中文
	class CScutLanTw : public CScutLan
	{
	public:
		CScutLanTw();
		~CScutLanTw(){}
	};

	//日语
	class CScutLanJp : public CScutLan
	{
	public:
		CScutLanJp();
		~CScutLanJp(){}
	};
}