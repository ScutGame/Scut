#include "../ScutEdit.h"
#include "ccTypes.h"
#include "ccmacros.h"
#include "cocos2d.h"
#include <map>
//#include <afxwin.h>

using namespace std;

using namespace cocos2d;

namespace ScutCxControl
{
	static map<HWND, CScutEdit*> g_EditMaps;

	char* WCharToChar(const wchar_t* pwszStr, unsigned int nCodePage)
	{
		int len = WideCharToMultiByte(nCodePage, 0, pwszStr, wcslen(pwszStr), NULL, 0, NULL, NULL);
		char *conv = new char [len + 1];
		WideCharToMultiByte(nCodePage, 0, pwszStr, wcslen(pwszStr), conv, len, NULL, NULL);
		conv[len] = '\0';
		return conv;
	}

	wchar_t* CharToWChar(const char* pszStr, unsigned int nCodePage)
	{
		char *pszend = NULL;
		if(pszStr == NULL)
			return NULL;

		int nLength = strlen(pszStr);
		int nOldLen = -1;
		int wLen = MultiByteToWideChar(nCodePage, 0, pszStr, -1, NULL, 0);
		nOldLen = wLen;
		while(wLen == 0 && nLength > 0)
		{
			char oldchar;
			pszend = const_cast<char*>(pszStr) + nLength -1;
			if(pszend && *pszend)
			{
				oldchar = *pszend;
			}
			*pszend = '\0';
			nLength--;
			wLen = MultiByteToWideChar(nCodePage, 0, pszStr, -1, NULL, 0);
			if(pszend && oldchar > 0)
			{
				*pszend = oldchar;
			}
		}

		if(nLength == 0 && wLen == 0)
		{
			return NULL;
		}
		if (nOldLen != 0)
		{
			nLength = -1;
		}
		wchar_t* pwszTemp = new wchar_t[wLen + 1];
		MultiByteToWideChar(nCodePage, 0, pszStr, nLength, pwszTemp, wLen);
		pwszTemp[wLen] = L'\0';
		return pwszTemp;
#if 0
		if (pszStr == NULL)
		{
			return NULL;
		}
		int wLen = MultiByteToWideChar(nCodePage, 0, pszStr, -1, NULL, 0);
		wchar_t* pwszTemp = new wchar_t[wLen + 1];
		MultiByteToWideChar(nCodePage, 0, pszStr, -1, pwszTemp, wLen);
		pwszTemp[wLen] = L'\0';
		return pwszTemp;
#endif
	}

	CScutEdit::CScutEdit(void)
	{
		m_hEditWin = NULL;
	}

	CScutEdit::~CScutEdit(void)
	{
		this->release();
	}

	void CScutEdit::setMaxText(int nTextCount)
	{
		SendMessage(m_hEditWin, EM_SETLIMITTEXT, nTextCount, 0);
	}

	static LONG g_OldProc;

	LRESULT CALLBACK EditProc( HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam )
	{
		switch(message)  
		{  
		case WM_CHAR:  
		case WM_KEYDOWN:
		case WM_KEYUP:
			{  
				map<HWND, CScutEdit*>::iterator it = g_EditMaps.find(hWnd);
				if (it != g_EditMaps.end())
				{
					CScutEdit* pEdit = it->second;
					pEdit->OnTextChanged();
				}
			}  
			break;  
		}
		return CallWindowProc((WNDPROC)g_OldProc, hWnd, message, wParam, lParam);
	}

	bool CScutEdit::init(bool bMultiline, bool bPwdMode, cocos2d::ccColor4B* pBackColor, cocos2d::ccColor4B* pForeColor, cocos2d::CCPoint* pLocation, cocos2d::CCSize* pSize)
	{
		DWORD dwStyle = WS_CHILD|WS_VISIBLE|WS_BORDER;
		if (bMultiline)
		{
			dwStyle = dwStyle | ES_MULTILINE;
		}
		if (bPwdMode)
		{
			dwStyle = dwStyle | ES_PASSWORD;
		}
		CCPoint ptLocation(0, 0);
		CCSize editSize(100, 20);
		if (pLocation)
		{
			ptLocation = *pLocation;
		}
		if (pSize)
		{
			editSize = *pSize;
		}
		m_hEditWin = CreateWindowW(L"EDIT", //这里是关键,也是edit框的标识
			NULL,
			dwStyle,
			ptLocation.x,
			ptLocation.y,//x,y位置
			editSize.width, 
			editSize.height,                                                    //长和宽
			cocos2d::CCDirector::sharedDirector()->getOpenGLView()->getHWnd(),
			0,
			NULL,
			NULL);
		if (m_hEditWin)
		{
			g_EditMaps[m_hEditWin] = this;
			g_OldProc = ::SetWindowLong(m_hEditWin, GWL_WNDPROC, (LONG)&EditProc);
			return true;
		}
		else
		{
			return false;
		}
	}


	void CScutEdit::SetTextSize(int nTextSize)
	{


	}

	void CScutEdit::setRect(cocos2d::CCRect rcEdit)
	{
		if (m_hEditWin)
		{
			MoveWindow(m_hEditWin, (int)rcEdit.origin.x*CC_CONTENT_SCALE_FACTOR(), (int)rcEdit.origin.y*CC_CONTENT_SCALE_FACTOR(), (int)rcEdit.size.width*CC_CONTENT_SCALE_FACTOR(), (int)rcEdit.size.height*CC_CONTENT_SCALE_FACTOR(),true);
		}
	}

	void CScutEdit::release()
	{
		if (m_hEditWin)
		{
			map<HWND, CScutEdit*>::iterator it = g_EditMaps.find(m_hEditWin);
			if (it != g_EditMaps.end())
			{
				g_EditMaps.erase(it);
			}
			DestroyWindow(m_hEditWin);
			m_hEditWin = NULL;
		}
	}

	void CScutEdit::setVisible(bool bVisible)
	{
		ShowWindow(m_hEditWin, bVisible ? SW_SHOW : SW_HIDE);
	}

	void CScutEdit::setEnabled(bool bEnable)
	{
		EnableWindow(m_hEditWin, bEnable);
	}

	int Replace(string& strValue, const char* szOld, const char* szNew)
	{
		int nReplaced		= 0;
		unsigned int nIdx			= 0;
		unsigned int nOldLen		= strlen(szOld);

		if ( 0 != nOldLen )
		{
			static const char ch	= char(0);
			unsigned int nNewLen	= strlen(szNew);
			const char* szRealNew	= szNew == 0 ? &ch : szNew;

			while ( (nIdx = strValue.find(szOld, nIdx)) != std::string::npos )
			{
				strValue.replace(strValue.begin()+nIdx, strValue.begin() + nIdx + nOldLen, szRealNew);
				nReplaced++;
				nIdx += nNewLen;
			}
		}

		return nReplaced;
	}
	std::string CScutEdit::GetEditText()
	{
#define  MAX_INPUT  2048	
		//char input[MAX_INPUT] = {0};
		//GetWiScutowTextA(m_hEditWin, input, MAX_INPUT);
		//std::string strTemp = GB2312ToUtf8(input, strlen(input));
		//Replace(strTemp, "\r\n", "\n");
		//return strTemp;

		//Unicode处理方式
		wchar_t input[MAX_INPUT] = {0};
		GetWindowTextW(m_hEditWin, input, MAX_INPUT);
		char* pszTemp = WCharToChar(input, CP_UTF8);
		std::string strTemp = pszTemp;
		delete []pszTemp;
		return strTemp;
	}
	void CScutEdit::setText(std::string strText)
	{
		////统一替换\r\n为\n
		//std::string strTemp = Utf8ToGB2312(strText.c_str(), strText.length());
		//Replace(strTemp, "\n", "\r\n");
		//SetWiScutowTextA(m_hEditWin, (LPSTR)strTemp.c_str());

		//Unicode处理方式
		wchar_t* pszTemp = CharToWChar(strText.c_str(), CP_UTF8);
		SetWindowTextW(m_hEditWin, pszTemp);
		delete []pszTemp;
	}

	void CScutEdit::setInputNumber()
	{
		LONG style = GetWindowLong(m_hEditWin, GWL_STYLE);
		SetWindowLong(m_hEditWin, GWL_STYLE, style | ES_NUMBER);
	}

}