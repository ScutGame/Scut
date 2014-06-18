#include "ScutEdit.h"

#import "ScutEditIphoneView.h"
#include "EAGLView.h"

namespace ScutCxControl {

	static int s_nNdTextTag = 3541;
		

	CScutEdit::CScutEdit(void)
	{
		m_nTag = 0;
	}

	CScutEdit::~CScutEdit(void)
	{
		
	}


	//初始化编辑框控件
	bool CScutEdit::init(bool bMultiline , bool bPwdMode, cocos2d::ccColor4B* pBackColor, cocos2d::ccColor4B* pForeColor,
					   cocos2d::CCPoint* pLocation , cocos2d::CCSize* pSize
					   )
	{
		UIColor* bkColor = [UIColor colorWithWhite:1.0 alpha:1.0];
		if (pBackColor)
			bkColor = [UIColor colorWithRed:pBackColor->r/255.0 green:pBackColor->g/255.0 blue:pBackColor->b/255.0 alpha:pBackColor->a/255.0];
		UIColor* frColor = [UIColor blackColor];
		if (pForeColor)
			frColor = [UIColor colorWithRed:pForeColor->r/255.0 green:pForeColor->g/255.0 blue:pForeColor->b/255.0 alpha:pForeColor->a/255.0];
		ScutEditIphoneView *editView = [[ScutEditIphoneView alloc]init:bPwdMode andIsEditView:bMultiline andBackColor:bkColor andForeColor:frColor];
	//	CCDirector::sharedDirector()->getOpenGLView()
		
		
		[[EAGLView sharedEGLView] addSubview:editView];
		[editView registerKeyboradNotification];
		[editView release];
		m_nTag = s_nNdTextTag;
		s_nNdTextTag++;
		editView.tag=m_nTag;
		[editView setOwnerEdit:this];
		
		cocos2d::CCRect rcEdit;
		if (pLocation)
		{
			rcEdit.origin.x = pLocation->x;
			rcEdit.origin.y = pLocation->y;
		}
		if (pSize)
		{
			rcEdit.size.width = pSize->width;
			rcEdit.size.height = pSize->height;
		}			
		CGRect rcEditField = CGRectMake(rcEdit.origin.x,
										rcEdit.origin.y, rcEdit.size.width,rcEdit.size.height);
		[editView setNdEditFrame:rcEditField];
		
		//editView.backgroundColor = [UIColor redColor];
		//editView.transform = CGAffineTransformMakeRotation(M_PI_2);
		//[UIApplication sharedApplication].keyWindow.transform =CGAffineTransformMakeRotation(M_PI_2);
		return YES;
	//	UIView

	}

	void CScutEdit::setInputNumber()
	{
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]])
		{
			ScutEditIphoneView*textFild = (ScutEditIphoneView*)tempView;
			[textFild setInputNum];
		}
	}

	//设置编辑框的位置以及大小
	void CScutEdit::setRect(cocos2d::CCRect rcEdit)
	{
		
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]]) {
			ScutEditIphoneView*editView = (ScutEditIphoneView*)tempView;
		//	textFild.frame = CGRectMake(rcEdit.origin.y,CCDirector::sharedDirector()->getWinSize().height - rcEdit.origin.x, rcEdit.size.height, rcEdit.size.width);
	//		CGRect rcEditField = CGRectMake(cocos2d::CCDirector::sharedDirector()->getWinSize().height-rcEdit.origin.y - rcEdit.size.height,
	//									rcEdit.origin.x, rcEdit.size.height, rcEdit.size.width);
			CGRect rcEditField = CGRectMake(rcEdit.origin.x,
											rcEdit.origin.y, rcEdit.size.width,rcEdit.size.height);
			[editView setNdEditFrame:rcEditField];
	//		rcEditField
	//		textFild.frame = CGRectMake(rcEdit.origin.x ,rcEdit.origin.y, rcEdit.size.width
	//									
	//									, rcEdit.size.height);


		}
	}

	void CScutEdit::hiddenTextPanel()
	{
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]]) {
			ScutEditIphoneView*editView = (ScutEditIphoneView*)tempView;
			[editView hiddenTextPanel];
		}
	}
	std::string CScutEdit::GetEditText()
	{

		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];

		if ([tempView isKindOfClass:[ScutEditIphoneView class]]) {

			ScutEditIphoneView*textFild = (ScutEditIphoneView*)tempView;
			NSString *str = [textFild getTextString];
		
			if (!str) {
				return "";
			}
			return [str UTF8String];
		}

		return "";
	}

	void CScutEdit::release()
	{
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]]) {
			[tempView removeFromSuperview];
		}
	}

	void CScutEdit::setVisible(bool bVisible)
	{
		UIView *tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		tempView.hidden=!bVisible;
	}

	void CScutEdit::setEnabled(bool bEnable)
	{
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]]) {
			ScutEditIphoneView*textFild = (ScutEditIphoneView*)tempView;
			[textFild setEnabled:bEnable];
		}
	}

	void CScutEdit::setText(std::string strText)
	{
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]]) {
			ScutEditIphoneView*textFild = (ScutEditIphoneView*)tempView;
			NSString *str = [NSString stringWithUTF8String:strText.c_str()];
			[textFild setTextString:str];
		}
	}

	void CScutEdit::setMaxText(int nTextCount)
	{
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]])
		{
			ScutEditIphoneView*textFild = (ScutEditIphoneView*)tempView;
			[textFild setMaxInputNum:nTextCount];
		}
	}
	
	void CScutEdit::SetTextSize(int nTextSize)
	{	
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[ScutEditIphoneView class]])
		{
			ScutEditIphoneView*textFild = (ScutEditIphoneView*)tempView;
			[textFild setTextFontSize:nTextSize];
		}
	}
    
    /*void CNdEdit::setFocus()
    {
		id tempView = [[EAGLView sharedEGLView] viewWithTag:m_nTag];
		if ([tempView isKindOfClass:[NdEditIphoneView class]])
		{
			NdEditIphoneView*textFild = (NdEditIphoneView*)tempView;
			[textFild becomeFirstResponder];
		}
    }*/

}
