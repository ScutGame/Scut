#include "../ScutEdit.h"
//#include "../Utility.h"
#include "CCCommon.h"
#include <android/log.h>
#include <map>
#include <vector>

using namespace std;
using namespace cocos2d;

namespace ScutCxControl
{


	static map<long, CScutEdit*> g_EditMaps;
	static std::vector<CScutEdit*> g_EditList;

#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
	std::vector<CScutEdit*>* g_pEditList = &g_EditList;
#endif

	CScutEdit::CScutEdit(void)
	{
		m_hEditWin = 0;
	}

	CScutEdit::~CScutEdit(void)
	{

	}

	void CScutEdit::setMaxText(int nTextCount)
	{
		CAndroidWindow::SetTextCount(m_hEditWin,org_cocos2dx_lib_AndroidWindow_VIEW_TYPE_EDIT,nTextCount);
	}

	void TextChangedCallback(long id)
	{
		map<long, CScutEdit*>::iterator it = g_EditMaps.find(id);
		if (it != g_EditMaps.end())
		{
			CScutEdit* pEdit = it->second;

			if (g_pEditList)
			{
				g_pEditList->push_back(pEdit);
			}
		}
	}

	bool CScutEdit::init(bool bMultiline, bool bPwdMode, cocos2d::ccColor4B* pBackColor, cocos2d::ccColor4B* pForeColor, cocos2d::CCPoint* pLocation, cocos2d::CCSize* pSize)
	{
		long bkColor = 0xffffffff;
		long foreColor = 0xff000000;
		if (pBackColor)
		{
			bkColor = (pBackColor->a << 24) | (pBackColor->r << 16) | (pBackColor->g << 8) | (pBackColor->b);
		}
		if (pForeColor)
		{
			foreColor = (pForeColor->a << 24) | (pForeColor->r << 16) | (pForeColor->g << 8) | (pForeColor->b);
		}
		m_hEditWin = CAndroidWindow::CreateWindow(org_cocos2dx_lib_AndroidWindow_VIEW_TYPE_EDIT, bkColor, foreColor);
		if (m_hEditWin)
		{
			if (bMultiline) 
			{
				CAndroidWindow::SetTextMult(m_hEditWin);
			}
			if (bPwdMode)
			{
				CAndroidWindow::SetEditPWD(m_hEditWin);
			}

			cocos2d::CCRect rcEdit;
			if (pLocation != 0)
			{
				rcEdit.origin.x = pLocation->x;
				rcEdit.origin.y = pLocation->y;
			}
			if (pSize != 0)
			{
				rcEdit.size.width = pSize->width;
				rcEdit.size.height = pSize->height;
			}
			setRect(rcEdit);

			g_EditMaps[m_hEditWin] = this;
			CAndroidWindow::SharedDefault()->setTextChangedCallback(&TextChangedCallback);
			return true;
		}
		else
		{
			return false;
		}
	}

	void CScutEdit::setRect(cocos2d::CCRect rcEdit)
	{
		int h = (int)rcEdit.size.height;
		CAndroidWindow::MoveWindow(m_hEditWin, (int)rcEdit.origin.x, (int)rcEdit.origin.y, (int)rcEdit.size.width, h);
	}

	void CScutEdit::release()
	{
		map<long, CScutEdit*>::iterator it = g_EditMaps.find(m_hEditWin);
		if (it != g_EditMaps.end())
		{
			g_EditMaps.erase(it);
		}
		CAndroidWindow::DestroyWindow(m_hEditWin);
	}

	std::string CScutEdit::GetEditText()
	{
		return CAndroidWindow::GetEditText(m_hEditWin);
	}


	void CScutEdit::setVisible(bool bVisible)
	{
		if (bVisible)
		{
			CAndroidWindow::ShowWindow(m_hEditWin, org_cocos2dx_lib_AndroidWindow_OP_SW_VISIBLE);
		}
		else
		{
			CAndroidWindow::ShowWindow(m_hEditWin, org_cocos2dx_lib_AndroidWindow_OP_SW_INVISIBLE);
		}
	}

	void CScutEdit::setEnabled(bool bEnable)
	{
		CAndroidWindow::ShowWindow(m_hEditWin, bEnable ? org_cocos2dx_lib_AndroidWindow_OP_ENABLE : org_cocos2dx_lib_AndroidWindow_OP_DISABLE);
	}

	void CScutEdit::setText(std::string strText)
	{
		CAndroidWindow::SetWindowText(m_hEditWin, org_cocos2dx_lib_AndroidWindow_VIEW_TYPE_EDIT,strText.c_str());
	}

	void CScutEdit::SetTextSize(int nTextSize)
	{
		CAndroidWindow::SetWindowTextSize(m_hEditWin,org_cocos2dx_lib_AndroidWindow_VIEW_TYPE_EDIT,nTextSize);
	}

	void CScutEdit::setInputNumber()
	{
		CAndroidWindow::SetTextInputNumber(m_hEditWin);
	}

	//void CScutEdit::setFocus()
	//{
	//	CAndroidWindow::SetTextFocus(m_hEditWin);
	//}

}